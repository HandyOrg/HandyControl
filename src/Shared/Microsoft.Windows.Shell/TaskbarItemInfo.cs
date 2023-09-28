using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Standard;

namespace Microsoft.Windows.Shell;

public sealed class TaskbarItemInfo : Freezable
{
    protected override Freezable CreateInstanceCore()
    {
        return new TaskbarItemInfo();
    }

    [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
    public static TaskbarItemInfo GetTaskbarItemInfo(Window window)
    {
        Verify.IsNotNull<Window>(window, "window");
        return (TaskbarItemInfo) window.GetValue(TaskbarItemInfo.TaskbarItemInfoProperty);
    }

    [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
    public static void SetTaskbarItemInfo(Window window, TaskbarItemInfo value)
    {
        Verify.IsNotNull<Window>(window, "window");
        window.SetValue(TaskbarItemInfo.TaskbarItemInfoProperty, value);
    }

    private static void _OnTaskbarItemInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(d))
        {
            return;
        }
        Window window = (Window) d;
        TaskbarItemInfo taskbarItemInfo = (TaskbarItemInfo) e.OldValue;
        TaskbarItemInfo taskbarItemInfo2 = (TaskbarItemInfo) e.NewValue;
        if (taskbarItemInfo == taskbarItemInfo2)
        {
            return;
        }
        if (!Utility.IsOSWindows7OrNewer)
        {
            return;
        }
        if (taskbarItemInfo != null && taskbarItemInfo._window != null)
        {
            taskbarItemInfo._DetachWindow();
        }
        if (taskbarItemInfo2 != null)
        {
            taskbarItemInfo2._SetWindow(window);
        }
    }

    private static object _CoerceTaskbarItemInfoValue(DependencyObject d, object value)
    {
        if (DesignerProperties.GetIsInDesignMode(d))
        {
            return value;
        }
        Verify.IsNotNull<DependencyObject>(d, "d");
        Window window = (Window) d;
        TaskbarItemInfo taskbarItemInfo = (TaskbarItemInfo) value;
        if (taskbarItemInfo != null && taskbarItemInfo._window != null && taskbarItemInfo._window != window)
        {
            throw new NotSupportedException();
        }
        window.VerifyAccess();
        return taskbarItemInfo;
    }

    public TaskbarItemProgressState ProgressState
    {
        get
        {
            return (TaskbarItemProgressState) base.GetValue(TaskbarItemInfo.ProgressStateProperty);
        }
        set
        {
            base.SetValue(TaskbarItemInfo.ProgressStateProperty, value);
        }
    }

    private void _OnProgressStateChanged()
    {
        if (!this._isAttached)
        {
            return;
        }
        this._UpdateProgressState(true);
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
                break;
            default:
                value = TaskbarItemProgressState.None;
                break;
        }
        return value;
    }

    public double ProgressValue
    {
        get
        {
            return (double) base.GetValue(TaskbarItemInfo.ProgressValueProperty);
        }
        set
        {
            base.SetValue(TaskbarItemInfo.ProgressValueProperty, value);
        }
    }

    private void _OnProgressValueChanged()
    {
        if (!this._isAttached)
        {
            return;
        }
        this._UpdateProgressValue(true);
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

    public ImageSource Overlay
    {
        get
        {
            return (ImageSource) base.GetValue(TaskbarItemInfo.OverlayProperty);
        }
        set
        {
            base.SetValue(TaskbarItemInfo.OverlayProperty, value);
        }
    }

    private void _OnOverlayChanged()
    {
        if (!this._isAttached)
        {
            return;
        }
        this._UpdateOverlay(true);
    }

    public string Description
    {
        get
        {
            return (string) base.GetValue(TaskbarItemInfo.DescriptionProperty);
        }
        set
        {
            base.SetValue(TaskbarItemInfo.DescriptionProperty, value);
        }
    }

    private void _OnDescriptionChanged()
    {
        if (!this._isAttached)
        {
            return;
        }
        this._UpdateTooltip(true);
    }

    public Thickness ThumbnailClipMargin
    {
        get
        {
            return (Thickness) base.GetValue(TaskbarItemInfo.ThumbnailClipMarginProperty);
        }
        set
        {
            base.SetValue(TaskbarItemInfo.ThumbnailClipMarginProperty, value);
        }
    }

    private void _OnThumbnailClipMarginChanged()
    {
        if (!this._isAttached)
        {
            return;
        }
        this._UpdateThumbnailClipping(true);
    }

    private static Thickness _CoerceThumbnailClipMargin(Thickness margin)
    {
        if (margin.Left < 0.0 || margin.Right < 0.0 || margin.Top < 0.0 || margin.Bottom < 0.0)
        {
            return TaskbarItemInfo._EmptyThickness;
        }
        return margin;
    }

    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
    public ThumbButtonInfoCollection ThumbButtonInfos
    {
        get
        {
            return (ThumbButtonInfoCollection) base.GetValue(TaskbarItemInfo.ThumbButtonInfosProperty);
        }
        set
        {
            base.SetValue(TaskbarItemInfo.ThumbButtonInfosProperty, value);
        }
    }

    private void _OnThumbButtonsChanged()
    {
        if (!this._isAttached)
        {
            return;
        }
        this._UpdateThumbButtons(true);
    }

    private IntPtr _GetHICONFromImageSource(ImageSource image, Size dimensions)
    {
        if (this._gdipToken == null)
        {
            this._gdipToken = SafeGdiplusStartupToken.Startup();
        }
        return Utility.GenerateHICON(image, dimensions);
    }

    public TaskbarItemInfo()
    {
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            ITaskbarList taskbarList = null;
            try
            {
                taskbarList = CLSID.CoCreateInstance<ITaskbarList>("56FDF344-FD6D-11d0-958A-006097C9A090");
                taskbarList.HrInit();
                this._taskbarList = (taskbarList as ITaskbarList3);
                taskbarList = null;
            }
            finally
            {
                Utility.SafeRelease<ITaskbarList>(ref taskbarList);
            }
            this._overlaySize = new Size((double) NativeMethods.GetSystemMetrics(SM.CXSMICON), (double) NativeMethods.GetSystemMetrics(SM.CYSMICON));
        }
        this.ThumbButtonInfos = new ThumbButtonInfoCollection();
    }

    private void _SetWindow(Window window)
    {
        if (window == null)
        {
            return;
        }
        this._window = window;
        if (this._taskbarList == null)
        {
            return;
        }
        IntPtr handle = new WindowInteropHelper(this._window).Handle;
        if (!(handle != IntPtr.Zero))
        {
            this._window.SourceInitialized += this._OnWindowSourceInitialized;
            return;
        }
        this._hwndSource = HwndSource.FromHwnd(handle);
        this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
        this._OnIsAttachedChanged(true);
    }

    private void _OnWindowSourceInitialized(object sender, EventArgs e)
    {
        this._window.SourceInitialized -= this._OnWindowSourceInitialized;
        IntPtr handle = new WindowInteropHelper(this._window).Handle;
        this._hwndSource = HwndSource.FromHwnd(handle);
        this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
        MSGFLTINFO msgfltinfo;
        NativeMethods.ChangeWindowMessageFilterEx(handle, TaskbarItemInfo.WM_TASKBARBUTTONCREATED, MSGFLT.ALLOW, out msgfltinfo);
        NativeMethods.ChangeWindowMessageFilterEx(handle, WM.COMMAND, MSGFLT.ALLOW, out msgfltinfo);
    }

    private IntPtr _WndProc(IntPtr hwnd, int uMsg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (uMsg == (int) TaskbarItemInfo.WM_TASKBARBUTTONCREATED)
        {
            this._OnIsAttachedChanged(true);
            this._isAttached = true;
            handled = false;
        }
        else if (uMsg != 5)
        {
            if (uMsg == 273 && Utility.HIWORD(wParam.ToInt32()) == 6144)
            {
                int index = Utility.LOWORD(wParam.ToInt32());
                this.ThumbButtonInfos[index].InvokeClick();
                handled = true;
            }
        }
        else
        {
            this._UpdateThumbnailClipping(this._isAttached);
            handled = false;
        }
        return IntPtr.Zero;
    }

    private void _OnIsAttachedChanged(bool attached)
    {
        this._haveAddedButtons = false;
        if (!attached && this._hwndSource == null)
        {
            return;
        }
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

    private void _DetachWindow()
    {
        this._window.SourceInitialized -= this._OnWindowSourceInitialized;
        this._isAttached = false;
        this._OnIsAttachedChanged(false);
        this._window = null;
    }

    private HRESULT _UpdateOverlay(bool attached)
    {
        ImageSource overlay = this.Overlay;
        if (overlay == null || !attached)
        {
            return this._taskbarList.SetOverlayIcon(this._hwndSource.Handle, IntPtr.Zero, null);
        }
        IntPtr hIcon = IntPtr.Zero;
        HRESULT result;
        try
        {
            hIcon = this._GetHICONFromImageSource(overlay, this._overlaySize);
            result = this._taskbarList.SetOverlayIcon(this._hwndSource.Handle, hIcon, null);
        }
        finally
        {
            Utility.SafeDestroyIcon(ref hIcon);
        }
        return result;
    }

    private HRESULT _UpdateTooltip(bool attached)
    {
        string pszTip = this.Description ?? "";
        if (!attached)
        {
            pszTip = "";
        }
        return this._taskbarList.SetThumbnailTooltip(this._hwndSource.Handle, pszTip);
    }

    private HRESULT _UpdateProgressValue(bool attached)
    {
        if (!attached || this.ProgressState == TaskbarItemProgressState.None || this.ProgressState == TaskbarItemProgressState.Indeterminate)
        {
            return HRESULT.S_OK;
        }
        ulong ullCompleted = (ulong) (this.ProgressValue * 1000.0);
        return this._taskbarList.SetProgressValue(this._hwndSource.Handle, ullCompleted, 1000UL);
    }

    private HRESULT _UpdateProgressState(bool attached)
    {
        TaskbarItemProgressState progressState = this.ProgressState;
        TBPF tbpFlags = TBPF.NOPROGRESS;
        if (attached)
        {
            tbpFlags = progressState switch
            {
                TaskbarItemProgressState.None => TBPF.NOPROGRESS,
                TaskbarItemProgressState.Indeterminate => TBPF.INDETERMINATE,
                TaskbarItemProgressState.Normal => TBPF.NORMAL,
                TaskbarItemProgressState.Error => TBPF.ERROR,
                TaskbarItemProgressState.Paused => TBPF.PAUSED,
                _ => TBPF.NOPROGRESS
            };
        }
        HRESULT result = this._taskbarList.SetProgressState(this._hwndSource.Handle, tbpFlags);
        if (result.Succeeded)
        {
            result = this._UpdateProgressValue(attached);
        }
        return result;
    }

    private HRESULT _UpdateThumbnailClipping(bool attached)
    {
        RefRECT prcClip = null;
        if (attached && this.ThumbnailClipMargin != TaskbarItemInfo._EmptyThickness)
        {
            Thickness thumbnailClipMargin = this.ThumbnailClipMargin;
            RECT clientRect = NativeMethods.GetClientRect(this._hwndSource.Handle);
            Rect rect = DpiHelper.DeviceRectToLogical(new Rect((double) clientRect.Left, (double) clientRect.Top, (double) clientRect.Width, (double) clientRect.Height));
            if (thumbnailClipMargin.Left + thumbnailClipMargin.Right >= rect.Width || thumbnailClipMargin.Top + thumbnailClipMargin.Bottom >= rect.Height)
            {
                prcClip = new RefRECT(0, 0, 0, 0);
            }
            else
            {
                Rect logicalRectangle = new Rect(thumbnailClipMargin.Left, thumbnailClipMargin.Top, rect.Width - thumbnailClipMargin.Left - thumbnailClipMargin.Right, rect.Height - thumbnailClipMargin.Top - thumbnailClipMargin.Bottom);
                Rect rect2 = DpiHelper.LogicalRectToDevice(logicalRectangle);
                prcClip = new RefRECT((int) rect2.Left, (int) rect2.Top, (int) rect2.Right, (int) rect2.Bottom);
            }
        }
        return this._taskbarList.SetThumbnailClip(this._hwndSource.Handle, prcClip);
    }

    private HRESULT _RegisterThumbButtons()
    {
        HRESULT hresult = HRESULT.S_OK;
        if (!this._haveAddedButtons)
        {
            THUMBBUTTON[] array = new THUMBBUTTON[7];
            for (int i = 0; i < 7; i++)
            {
                array[i] = new THUMBBUTTON
                {
                    iId = (uint) i,
                    dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND | THBF.HIDDEN),
                    dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
                };
            }
            hresult = this._taskbarList.ThumbBarAddButtons(this._hwndSource.Handle, (uint) array.Length, array);
            if (hresult == HRESULT.E_INVALIDARG)
            {
                hresult = HRESULT.S_FALSE;
            }
            this._haveAddedButtons = hresult.Succeeded;
        }
        return hresult;
    }

    private HRESULT _UpdateThumbButtons(bool attached)
    {
        THUMBBUTTON[] array = new THUMBBUTTON[7];
        HRESULT result = this._RegisterThumbButtons();
        if (result.Failed)
        {
            return result;
        }
        ThumbButtonInfoCollection thumbButtonInfos = this.ThumbButtonInfos;
        HRESULT result2;
        try
        {
            uint num = 0u;
            if (!attached || thumbButtonInfos == null)
            {
                goto IL_1AE;
            }
            using (FreezableCollection<ThumbButtonInfo>.Enumerator enumerator = thumbButtonInfos.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ThumbButtonInfo thumbButtonInfo = enumerator.Current;
                    THUMBBUTTON thumbbutton = new THUMBBUTTON
                    {
                        iId = num,
                        dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
                    };
                    switch (thumbButtonInfo.Visibility)
                    {
                        case Visibility.Visible:
                            goto IL_A5;
                        case Visibility.Hidden:
                            thumbbutton.dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND);
                            thumbbutton.hIcon = IntPtr.Zero;
                            break;
                        case Visibility.Collapsed:
                            thumbbutton.dwFlags = THBF.HIDDEN;
                            break;
                        default:
                            goto IL_A5;
                    }
IL_146:
                    array[(int) ((UIntPtr) num)] = thumbbutton;
                    num += 1u;
                    if (num != 7u)
                    {
                        continue;
                    }
                    break;
IL_A5:
                    thumbbutton.szTip = (thumbButtonInfo.Description ?? "");
                    thumbbutton.hIcon = this._GetHICONFromImageSource(thumbButtonInfo.ImageSource, this._overlaySize);
                    if (!thumbButtonInfo.IsBackgroundVisible)
                    {
                        thumbbutton.dwFlags |= THBF.NOBACKGROUND;
                    }
                    if (!thumbButtonInfo.IsEnabled)
                    {
                        thumbbutton.dwFlags |= THBF.DISABLED;
                    }
                    else
                    {
                        thumbbutton.dwFlags = thumbbutton.dwFlags;
                    }
                    if (!thumbButtonInfo.IsInteractive)
                    {
                        thumbbutton.dwFlags |= THBF.NONINTERACTIVE;
                    }
                    if (thumbButtonInfo.DismissWhenClicked)
                    {
                        thumbbutton.dwFlags |= THBF.DISMISSONCLICK;
                        goto IL_146;
                    }
                    goto IL_146;
                }
                goto IL_1AE;
            }
IL_179:
            array[(int) ((UIntPtr) num)] = new THUMBBUTTON
            {
                iId = num,
                dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND | THBF.HIDDEN),
                dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
            };
            num += 1u;
IL_1AE:
            if (num < 7u)
            {
                goto IL_179;
            }
            result2 = this._taskbarList.ThumbBarUpdateButtons(this._hwndSource.Handle, (uint) array.Length, array);
        }
        finally
        {
            foreach (THUMBBUTTON thumbbutton2 in array)
            {
                IntPtr hIcon = thumbbutton2.hIcon;
                if (IntPtr.Zero != hIcon)
                {
                    Utility.SafeDestroyIcon(ref hIcon);
                }
            }
        }
        return result2;
    }

    private const int c_MaximumThumbButtons = 7;

    private static readonly WM WM_TASKBARBUTTONCREATED = NativeMethods.RegisterWindowMessage("TaskbarButtonCreated");

    private static readonly Thickness _EmptyThickness = default(Thickness);

    private SafeGdiplusStartupToken _gdipToken;

    private bool _haveAddedButtons;

    private Window _window;

    private HwndSource _hwndSource;

    private ITaskbarList3 _taskbarList;

    private readonly Size _overlaySize;

    private bool _isAttached;

    public static readonly DependencyProperty TaskbarItemInfoProperty = DependencyProperty.RegisterAttached("TaskbarItemInfo", typeof(TaskbarItemInfo), typeof(TaskbarItemInfo), new PropertyMetadata(null, new PropertyChangedCallback(TaskbarItemInfo._OnTaskbarItemInfoChanged), new CoerceValueCallback(TaskbarItemInfo._CoerceTaskbarItemInfoValue)));

    public static readonly DependencyProperty ProgressStateProperty = DependencyProperty.Register("ProgressState", typeof(TaskbarItemProgressState), typeof(TaskbarItemInfo), new PropertyMetadata(TaskbarItemProgressState.None, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarItemInfo) d)._OnProgressStateChanged();
    }, (DependencyObject d, object e) => TaskbarItemInfo._CoerceProgressState((TaskbarItemProgressState) e)));

    public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register("ProgressValue", typeof(double), typeof(TaskbarItemInfo), new PropertyMetadata(0.0, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarItemInfo) d)._OnProgressValueChanged();
    }, (DependencyObject d, object e) => TaskbarItemInfo._CoerceProgressValue((double) e)));

    public static readonly DependencyProperty OverlayProperty = DependencyProperty.Register("Overlay", typeof(ImageSource), typeof(TaskbarItemInfo), new PropertyMetadata(null, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarItemInfo) d)._OnOverlayChanged();
    }));

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(TaskbarItemInfo), new PropertyMetadata(string.Empty, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarItemInfo) d)._OnDescriptionChanged();
    }));

    public static readonly DependencyProperty ThumbnailClipMarginProperty = DependencyProperty.Register("ThumbnailClipMargin", typeof(Thickness), typeof(TaskbarItemInfo), new PropertyMetadata(default(Thickness), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarItemInfo) d)._OnThumbnailClipMarginChanged();
    }, (DependencyObject d, object e) => TaskbarItemInfo._CoerceThumbnailClipMargin((Thickness) e)));

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
    public static readonly DependencyProperty ThumbButtonInfosProperty = DependencyProperty.Register("ThumbButtonInfos", typeof(ThumbButtonInfoCollection), typeof(TaskbarItemInfo), new PropertyMetadata(null, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarItemInfo) d)._OnThumbButtonsChanged();
    }));
}
