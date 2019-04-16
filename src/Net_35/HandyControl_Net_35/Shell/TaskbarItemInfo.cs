namespace Microsoft.Windows.Shell
{
    using Standard;
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;

    public sealed class TaskbarItemInfo : Freezable
    {
        private static readonly Thickness _EmptyThickness = new Thickness();
        private Standard.SafeGdiplusStartupToken _gdipToken;
        private bool _haveAddedButtons;
        private HwndSource _hwndSource;
        private bool _isAttached;
        private readonly Size _overlaySize;
        private Standard.ITaskbarList3 _taskbarList;
        private Window _window;
        private const int c_MaximumThumbButtons = 7;
        public static readonly DependencyProperty DescriptionProperty;
        public static readonly DependencyProperty OverlayProperty;
        public static readonly DependencyProperty ProgressStateProperty;
        public static readonly DependencyProperty ProgressValueProperty;
        public static readonly DependencyProperty TaskbarItemInfoProperty = DependencyProperty.RegisterAttached("TaskbarItemInfo", typeof(TaskbarItemInfo), typeof(TaskbarItemInfo), new PropertyMetadata(null, new PropertyChangedCallback(TaskbarItemInfo._OnTaskbarItemInfoChanged), new CoerceValueCallback(TaskbarItemInfo._CoerceTaskbarItemInfoValue)));
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId="Infos")]
        public static readonly DependencyProperty ThumbButtonInfosProperty;
        public static readonly DependencyProperty ThumbnailClipMarginProperty;
        private static readonly Standard.WM WM_TASKBARBUTTONCREATED = Standard.NativeMethods.RegisterWindowMessage("TaskbarButtonCreated");

        static TaskbarItemInfo()
        {
            ProgressStateProperty = DependencyProperty.Register("ProgressState", typeof(TaskbarItemProgressState), typeof(TaskbarItemInfo), new PropertyMetadata(TaskbarItemProgressState.None, (d, e) => ((TaskbarItemInfo) d)._OnProgressStateChanged(), (CoerceValueCallback) ((d, e) => _CoerceProgressState((TaskbarItemProgressState) e))));
            ProgressValueProperty = DependencyProperty.Register("ProgressValue", typeof(double), typeof(TaskbarItemInfo), new PropertyMetadata(0.0, (d, e) => ((TaskbarItemInfo) d)._OnProgressValueChanged(), (CoerceValueCallback) ((d, e) => _CoerceProgressValue((double) e))));
            OverlayProperty = DependencyProperty.Register("Overlay", typeof(ImageSource), typeof(TaskbarItemInfo), new PropertyMetadata(null, (d, e) => ((TaskbarItemInfo) d)._OnOverlayChanged()));
            DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(TaskbarItemInfo), new PropertyMetadata(string.Empty, (d, e) => ((TaskbarItemInfo) d)._OnDescriptionChanged()));
            ThumbnailClipMarginProperty = DependencyProperty.Register("ThumbnailClipMargin", typeof(Thickness), typeof(TaskbarItemInfo), new PropertyMetadata(new Thickness(), (d, e) => ((TaskbarItemInfo) d)._OnThumbnailClipMarginChanged(), (CoerceValueCallback) ((d, e) => _CoerceThumbnailClipMargin((Thickness) e))));
            ThumbButtonInfosProperty = DependencyProperty.Register("ThumbButtonInfos", typeof(ThumbButtonInfoCollection), typeof(TaskbarItemInfo), new PropertyMetadata(null, (d, e) => ((TaskbarItemInfo) d)._OnThumbButtonsChanged()));
        }

        public TaskbarItemInfo()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Standard.ITaskbarList comObject = null;
                try
                {
                    comObject = Standard.CLSID.CoCreateInstance<Standard.ITaskbarList>("56FDF344-FD6D-11d0-958A-006097C9A090");
                    comObject.HrInit();
                    this._taskbarList = comObject as Standard.ITaskbarList3;
                    comObject = null;
                }
                finally
                {
                    Standard.Utility.SafeRelease<Standard.ITaskbarList>(ref comObject);
                }
                this._overlaySize = new Size((double) Standard.NativeMethods.GetSystemMetrics(Standard.SM.CXSMICON), (double) Standard.NativeMethods.GetSystemMetrics(Standard.SM.CYSMICON));
            }
            this.ThumbButtonInfos = new ThumbButtonInfoCollection();
        }

        private static TaskbarItemProgressState _CoerceProgressState(TaskbarItemProgressState value)
        {
            switch (value)
            {
                case TaskbarItemProgressState.None:
                case TaskbarItemProgressState.Indeterminate:
                case TaskbarItemProgressState.Normal:
                case TaskbarItemProgressState.Error:
                case TaskbarItemProgressState.Paused:
                    return value;
            }
            value = TaskbarItemProgressState.None;
            return value;
        }

        private static double _CoerceProgressValue(double progressValue)
        {
            if (double.IsNaN(progressValue))
            {
                progressValue = 0.0;
            }
            progressValue = Math.Max(progressValue, 0.0);
            progressValue = Math.Min(1.0, progressValue);
            return progressValue;
        }

        private static object _CoerceTaskbarItemInfoValue(DependencyObject d, object value)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
            {
                return value;
            }
            Standard.Verify.IsNotNull<DependencyObject>(d, "d");
            Window window = (Window) d;
            TaskbarItemInfo info = (TaskbarItemInfo) value;
            if (((info != null) && (info._window != null)) && (info._window != window))
            {
                throw new NotSupportedException();
            }
            window.VerifyAccess();
            return info;
        }

        private static Thickness _CoerceThumbnailClipMargin(Thickness margin)
        {
            if (((margin.Left >= 0.0) && (margin.Right >= 0.0)) && ((margin.Top >= 0.0) && (margin.Bottom >= 0.0)))
            {
                return margin;
            }
            return _EmptyThickness;
        }

        private void _DetachWindow()
        {
            this._window.SourceInitialized -= new EventHandler(this._OnWindowSourceInitialized);
            this._isAttached = false;
            this._OnIsAttachedChanged(false);
            this._window = null;
        }

        private IntPtr _GetHICONFromImageSource(ImageSource image, Size dimensions)
        {
            if (this._gdipToken == null)
            {
                this._gdipToken = Standard.SafeGdiplusStartupToken.Startup();
            }
            return Standard.Utility.GenerateHICON(image, dimensions);
        }

        private void _OnDescriptionChanged()
        {
            if (this._isAttached)
            {
                this._UpdateTooltip(true);
            }
        }

        private void _OnIsAttachedChanged(bool attached)
        {
            this._haveAddedButtons = false;
            if (attached || (this._hwndSource != null))
            {
                this._UpdateOverlay(attached);
                this._UpdateProgressState(attached);
                this._UpdateProgressValue(attached);
                this._UpdateTooltip(attached);
                this._UpdateThumbnailClipping(attached);
                this._UpdateThumbButtons(attached);
                if (!attached)
                {
                    this._hwndSource = null;
                }
            }
        }

        private void _OnOverlayChanged()
        {
            if (this._isAttached)
            {
                this._UpdateOverlay(true);
            }
        }

        private void _OnProgressStateChanged()
        {
            if (this._isAttached)
            {
                this._UpdateProgressState(true);
            }
        }

        private void _OnProgressValueChanged()
        {
            if (this._isAttached)
            {
                this._UpdateProgressValue(true);
            }
        }

        private static void _OnTaskbarItemInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(d))
            {
                Window window = (Window) d;
                TaskbarItemInfo oldValue = (TaskbarItemInfo) e.OldValue;
                TaskbarItemInfo newValue = (TaskbarItemInfo) e.NewValue;
                if ((oldValue != newValue) && Standard.Utility.IsOSWindows7OrNewer)
                {
                    if ((oldValue != null) && (oldValue._window != null))
                    {
                        oldValue._DetachWindow();
                    }
                    if (newValue != null)
                    {
                        newValue._SetWindow(window);
                    }
                }
            }
        }

        private void _OnThumbButtonsChanged()
        {
            if (this._isAttached)
            {
                this._UpdateThumbButtons(true);
            }
        }

        private void _OnThumbnailClipMarginChanged()
        {
            if (this._isAttached)
            {
                this._UpdateThumbnailClipping(true);
            }
        }

        private void _OnWindowSourceInitialized(object sender, EventArgs e)
        {
            Standard.MSGFLTINFO msgfltinfo;
            this._window.SourceInitialized -= new EventHandler(this._OnWindowSourceInitialized);
            IntPtr handle = new WindowInteropHelper(this._window).Handle;
            this._hwndSource = HwndSource.FromHwnd(handle);
            this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
            Standard.NativeMethods.ChangeWindowMessageFilterEx(handle, WM_TASKBARBUTTONCREATED, Standard.MSGFLT.ALLOW, out msgfltinfo);
            Standard.NativeMethods.ChangeWindowMessageFilterEx(handle, Standard.WM.COMMAND, Standard.MSGFLT.ALLOW, out msgfltinfo);
        }

        private Standard.HRESULT _RegisterThumbButtons()
        {
            Standard.HRESULT hresult = Standard.HRESULT.S_OK;
            if (!this._haveAddedButtons)
            {
                Standard.THUMBBUTTON[] pButtons = new Standard.THUMBBUTTON[7];
                for (int i = 0; i < 7; i++)
                {
                    pButtons[i] = new Standard.THUMBBUTTON { iId = (uint) i, dwFlags = Standard.THBF.DISABLED | Standard.THBF.HIDDEN | Standard.THBF.NOBACKGROUND, dwMask = Standard.THB.FLAGS | Standard.THB.ICON | Standard.THB.TOOLTIP };
                }
                hresult = this._taskbarList.ThumbBarAddButtons(this._hwndSource.Handle, (uint) pButtons.Length, pButtons);
                if (hresult == Standard.HRESULT.E_INVALIDARG)
                {
                    hresult = Standard.HRESULT.S_FALSE;
                }
                this._haveAddedButtons = hresult.Succeeded;
            }
            return hresult;
        }

        private void _SetWindow(Window window)
        {
            if (window != null)
            {
                this._window = window;
                if (this._taskbarList != null)
                {
                    IntPtr handle = new WindowInteropHelper(this._window).Handle;
                    if (!(handle != IntPtr.Zero))
                    {
                        this._window.SourceInitialized += new EventHandler(this._OnWindowSourceInitialized);
                    }
                    else
                    {
                        this._hwndSource = HwndSource.FromHwnd(handle);
                        this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
                        this._OnIsAttachedChanged(true);
                    }
                }
            }
        }

        private Standard.HRESULT _UpdateOverlay(bool attached)
        {
            Standard.HRESULT hresult;
            ImageSource overlay = this.Overlay;
            if ((overlay == null) || !attached)
            {
                return this._taskbarList.SetOverlayIcon(this._hwndSource.Handle, IntPtr.Zero, null);
            }
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = this._GetHICONFromImageSource(overlay, this._overlaySize);
                hresult = this._taskbarList.SetOverlayIcon(this._hwndSource.Handle, zero, null);
            }
            finally
            {
                Standard.Utility.SafeDestroyIcon(ref zero);
            }
            return hresult;
        }

        private Standard.HRESULT _UpdateProgressState(bool attached)
        {
            Standard.HRESULT hresult;
            TaskbarItemProgressState progressState = this.ProgressState;
            Standard.TBPF nOPROGRESS = Standard.TBPF.NOPROGRESS;
            if (attached)
            {
                switch (progressState)
                {
                    case TaskbarItemProgressState.None:
                        nOPROGRESS = Standard.TBPF.NOPROGRESS;
                        goto Label_0040;

                    case TaskbarItemProgressState.Indeterminate:
                        nOPROGRESS = Standard.TBPF.INDETERMINATE;
                        goto Label_0040;

                    case TaskbarItemProgressState.Normal:
                        nOPROGRESS = Standard.TBPF.NORMAL;
                        goto Label_0040;

                    case TaskbarItemProgressState.Error:
                        nOPROGRESS = Standard.TBPF.ERROR;
                        goto Label_0040;

                    case TaskbarItemProgressState.Paused:
                        nOPROGRESS = Standard.TBPF.PAUSED;
                        goto Label_0040;
                }
                nOPROGRESS = Standard.TBPF.NOPROGRESS;
            }
        Label_0040:
            hresult = this._taskbarList.SetProgressState(this._hwndSource.Handle, nOPROGRESS);
            if (hresult.Succeeded)
            {
                hresult = this._UpdateProgressValue(attached);
            }
            return hresult;
        }

        private Standard.HRESULT _UpdateProgressValue(bool attached)
        {
            if ((!attached || (this.ProgressState == TaskbarItemProgressState.None)) || (this.ProgressState == TaskbarItemProgressState.Indeterminate))
            {
                return Standard.HRESULT.S_OK;
            }
            ulong ullCompleted = (ulong) (this.ProgressValue * 1000.0);
            return this._taskbarList.SetProgressValue(this._hwndSource.Handle, ullCompleted, 0x3e8L);
        }

        private Standard.HRESULT _UpdateThumbButtons(bool attached)
        {
            Standard.HRESULT hresult2;
            Standard.THUMBBUTTON[] pButtons = new Standard.THUMBBUTTON[7];
            Standard.HRESULT hresult = this._RegisterThumbButtons();
            if (hresult.Failed)
            {
                return hresult;
            }
            ThumbButtonInfoCollection thumbButtonInfos = this.ThumbButtonInfos;
            try
            {
                uint index = 0;
                if (attached && (thumbButtonInfos != null))
                {
                    foreach (ThumbButtonInfo info in thumbButtonInfos)
                    {
                        Standard.THUMBBUTTON thumbbutton = new Standard.THUMBBUTTON {
                            iId = index,
                            dwMask = Standard.THB.FLAGS | Standard.THB.ICON | Standard.THB.TOOLTIP
                        };
                        switch (info.Visibility)
                        {
                            case Visibility.Hidden:
                                thumbbutton.dwFlags = Standard.THBF.DISABLED | Standard.THBF.NOBACKGROUND;
                                thumbbutton.hIcon = IntPtr.Zero;
                                break;

                            case Visibility.Collapsed:
                                thumbbutton.dwFlags = Standard.THBF.ENABLED | Standard.THBF.HIDDEN;
                                break;

                            default:
                                thumbbutton.szTip = info.Description ?? "";
                                thumbbutton.hIcon = this._GetHICONFromImageSource(info.ImageSource, this._overlaySize);
                                if (!info.IsBackgroundVisible)
                                {
                                    thumbbutton.dwFlags |= Standard.THBF.ENABLED | Standard.THBF.NOBACKGROUND;
                                }
                                if (!info.IsEnabled)
                                {
                                    thumbbutton.dwFlags |= Standard.THBF.DISABLED;
                                }
                                else
                                {
                                    thumbbutton.dwFlags = thumbbutton.dwFlags;
                                }
                                if (!info.IsInteractive)
                                {
                                    thumbbutton.dwFlags |= Standard.THBF.ENABLED | Standard.THBF.NONINTERACTIVE;
                                }
                                if (info.DismissWhenClicked)
                                {
                                    thumbbutton.dwFlags |= Standard.THBF.DISMISSONCLICK;
                                }
                                break;
                        }
                        pButtons[index] = thumbbutton;
                        index++;
                        if (index == 7)
                        {
                            break;
                        }
                    }
                }
                while (index < 7)
                {
                    pButtons[index] = new Standard.THUMBBUTTON { iId = index, dwFlags = Standard.THBF.DISABLED | Standard.THBF.HIDDEN | Standard.THBF.NOBACKGROUND, dwMask = Standard.THB.FLAGS | Standard.THB.ICON | Standard.THB.TOOLTIP };
                    index++;
                }
                hresult2 = this._taskbarList.ThumbBarUpdateButtons(this._hwndSource.Handle, (uint) pButtons.Length, pButtons);
            }
            finally
            {
                foreach (Standard.THUMBBUTTON thumbbutton4 in pButtons)
                {
                    IntPtr hIcon = thumbbutton4.hIcon;
                    if (IntPtr.Zero != hIcon)
                    {
                        Standard.Utility.SafeDestroyIcon(ref hIcon);
                    }
                }
            }
            return hresult2;
        }

        private Standard.HRESULT _UpdateThumbnailClipping(bool attached)
        {
            Standard.RefRECT prcClip = null;
            if (attached && (this.ThumbnailClipMargin != _EmptyThickness))
            {
                Thickness thumbnailClipMargin = this.ThumbnailClipMargin;
                Standard.RECT clientRect = Standard.NativeMethods.GetClientRect(this._hwndSource.Handle);
                Rect rect2 = Standard.DpiHelper.DeviceRectToLogical(new Rect((double) clientRect.Left, (double) clientRect.Top, (double) clientRect.Width, (double) clientRect.Height));
                if (((thumbnailClipMargin.Left + thumbnailClipMargin.Right) >= rect2.Width) || ((thumbnailClipMargin.Top + thumbnailClipMargin.Bottom) >= rect2.Height))
                {
                    prcClip = new Standard.RefRECT(0, 0, 0, 0);
                }
                else
                {
                    Rect logicalRectangle = new Rect(thumbnailClipMargin.Left, thumbnailClipMargin.Top, (rect2.Width - thumbnailClipMargin.Left) - thumbnailClipMargin.Right, (rect2.Height - thumbnailClipMargin.Top) - thumbnailClipMargin.Bottom);
                    Rect rect4 = Standard.DpiHelper.LogicalRectToDevice(logicalRectangle);
                    prcClip = new Standard.RefRECT((int) rect4.Left, (int) rect4.Top, (int) rect4.Right, (int) rect4.Bottom);
                }
            }
            return this._taskbarList.SetThumbnailClip(this._hwndSource.Handle, prcClip);
        }

        private Standard.HRESULT _UpdateTooltip(bool attached)
        {
            string pszTip = this.Description ?? "";
            if (!attached)
            {
                pszTip = "";
            }
            return this._taskbarList.SetThumbnailTooltip(this._hwndSource.Handle, pszTip);
        }

        private IntPtr _WndProc(IntPtr hwnd, int uMsg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Standard.WM wm = (Standard.WM) uMsg;
            if (wm == WM_TASKBARBUTTONCREATED)
            {
                this._OnIsAttachedChanged(true);
                this._isAttached = true;
                handled = false;
            }
            else
            {
                Standard.WM wm2 = wm;
                if (wm2 != Standard.WM.SIZE)
                {
                    if ((wm2 == Standard.WM.COMMAND) && (Standard.Utility.HIWORD(wParam.ToInt32()) == 0x1800))
                    {
                        int num = Standard.Utility.LOWORD(wParam.ToInt32());
                        this.ThumbButtonInfos[num].InvokeClick();
                        handled = true;
                    }
                }
                else
                {
                    this._UpdateThumbnailClipping(this._isAttached);
                    handled = false;
                }
            }
            return IntPtr.Zero;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new TaskbarItemInfo();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0")]
        public static TaskbarItemInfo GetTaskbarItemInfo(Window window)
        {
            Standard.Verify.IsNotNull<Window>(window, "window");
            return (TaskbarItemInfo) window.GetValue(TaskbarItemInfoProperty);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0")]
        public static void SetTaskbarItemInfo(Window window, TaskbarItemInfo value)
        {
            Standard.Verify.IsNotNull<Window>(window, "window");
            window.SetValue(TaskbarItemInfoProperty, value);
        }

        public string Description
        {
            get
            {
                return (string) base.GetValue(DescriptionProperty);
            }
            set
            {
                base.SetValue(DescriptionProperty, value);
            }
        }

        public ImageSource Overlay
        {
            get
            {
                return (ImageSource) base.GetValue(OverlayProperty);
            }
            set
            {
                base.SetValue(OverlayProperty, value);
            }
        }

        public TaskbarItemProgressState ProgressState
        {
            get
            {
                return (TaskbarItemProgressState) base.GetValue(ProgressStateProperty);
            }
            set
            {
                base.SetValue(ProgressStateProperty, value);
            }
        }

        public double ProgressValue
        {
            get
            {
                return (double) base.GetValue(ProgressValueProperty);
            }
            set
            {
                base.SetValue(ProgressValueProperty, value);
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId="Infos")]
        public ThumbButtonInfoCollection ThumbButtonInfos
        {
            get
            {
                return (ThumbButtonInfoCollection) base.GetValue(ThumbButtonInfosProperty);
            }
            set
            {
                base.SetValue(ThumbButtonInfosProperty, value);
            }
        }

        public Thickness ThumbnailClipMargin
        {
            get
            {
                return (Thickness) base.GetValue(ThumbnailClipMarginProperty);
            }
            set
            {
                base.SetValue(ThumbnailClipMarginProperty, value);
            }
        }
    }
}

