namespace Microsoft.Windows.Shell
{
    using Standard;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;

    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class SystemParameters2 : INotifyPropertyChanged
    {
        private Rect _captionButtonLocation;
        private double _captionHeight;
        private Color _glassColor;
        private SolidColorBrush _glassColorBrush;
        private bool _isGlassEnabled;
        private bool _isHighContrast;
        private Standard.MessageWindow _messageHwnd;
        private Size _smallIconSize;
        [ThreadStatic]
        private static SystemParameters2 _threadLocalSingleton;
        private readonly Dictionary<Standard.WM, List<_SystemMetricUpdate>> _UpdateTable;
        private string _uxThemeColor;
        private string _uxThemeName;
        private CornerRadius _windowCornerRadius;
        private Thickness _windowNonClientFrameThickness;
        private Thickness _windowResizeBorderThickness;

        public event PropertyChangedEventHandler PropertyChanged;

        private SystemParameters2()
        {
            EventHandler handler = null;
            this._messageHwnd = new Standard.MessageWindow(0, Standard.WS.BORDER | Standard.WS.DISABLED | Standard.WS.DLGFRAME | Standard.WS.GROUP | Standard.WS.MAXIMIZEBOX | Standard.WS.SIZEBOX | Standard.WS.SYSMENU, Standard.WS_EX.LEFT, new Rect(-16000.0, -16000.0, 100.0, 100.0), "", new Standard.WndProc(this._WndProc));
            if (handler == null)
            {
                handler = (sender, e) => Standard.Utility.SafeDispose<Standard.MessageWindow>(ref this._messageHwnd);
            }
            this._messageHwnd.Dispatcher.ShutdownStarted += handler;
            this._InitializeIsGlassEnabled();
            this._InitializeGlassColor();
            this._InitializeCaptionHeight();
            this._InitializeWindowNonClientFrameThickness();
            this._InitializeWindowResizeBorderThickness();
            this._InitializeCaptionButtonLocation();
            this._InitializeSmallIconSize();
            this._InitializeHighContrast();
            this._InitializeThemeInfo();
            this._InitializeWindowCornerRadius();
            Dictionary<Standard.WM, List<_SystemMetricUpdate>> dictionary = new Dictionary<Standard.WM, List<_SystemMetricUpdate>>();
            dictionary.Add(Standard.WM.THEMECHANGED, new List<_SystemMetricUpdate> { new _SystemMetricUpdate(this._UpdateThemeInfo), new _SystemMetricUpdate(this._UpdateHighContrast), new _SystemMetricUpdate(this._UpdateWindowCornerRadius), new _SystemMetricUpdate(this._UpdateCaptionButtonLocation) });
            dictionary.Add(Standard.WM.WININICHANGE, new List<_SystemMetricUpdate> { new _SystemMetricUpdate(this._UpdateCaptionHeight), new _SystemMetricUpdate(this._UpdateWindowResizeBorderThickness), new _SystemMetricUpdate(this._UpdateSmallIconSize), new _SystemMetricUpdate(this._UpdateHighContrast), new _SystemMetricUpdate(this._UpdateWindowNonClientFrameThickness), new _SystemMetricUpdate(this._UpdateCaptionButtonLocation) });
            dictionary.Add(Standard.WM.DWMNCRENDERINGCHANGED, new List<_SystemMetricUpdate> { new _SystemMetricUpdate(this._UpdateIsGlassEnabled) });
            dictionary.Add(Standard.WM.DWMCOMPOSITIONCHANGED, new List<_SystemMetricUpdate> { new _SystemMetricUpdate(this._UpdateIsGlassEnabled) });
            dictionary.Add(Standard.WM.DWMCOLORIZATIONCOLORCHANGED, new List<_SystemMetricUpdate> { new _SystemMetricUpdate(this._UpdateGlassColor) });
            this._UpdateTable = dictionary;
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private void _InitializeCaptionButtonLocation()
        {
            if (!Standard.Utility.IsOSVistaOrNewer || !Standard.NativeMethods.IsThemeActive())
            {
                this._LegacyInitializeCaptionButtonLocation();
            }
            else
            {
                Standard.TITLEBARINFOEX structure = new Standard.TITLEBARINFOEX {
                    cbSize = Marshal.SizeOf(typeof(Standard.TITLEBARINFOEX))
                };
                IntPtr ptr = Marshal.AllocHGlobal(structure.cbSize);
                try
                {
                    Marshal.StructureToPtr(structure, ptr, false);
                    Standard.NativeMethods.ShowWindow(this._messageHwnd.Handle, Standard.SW.SHOW);
                    Standard.NativeMethods.SendMessage(this._messageHwnd.Handle, Standard.WM.GETTITLEBARINFOEX, IntPtr.Zero, ptr);
                    structure = (Standard.TITLEBARINFOEX) Marshal.PtrToStructure(ptr, typeof(Standard.TITLEBARINFOEX));
                }
                finally
                {
                    Standard.NativeMethods.ShowWindow(this._messageHwnd.Handle, Standard.SW.HIDE);
                    Standard.Utility.SafeFreeHGlobal(ref ptr);
                }
                Standard.RECT rect = Standard.RECT.Union(structure.rgrect_CloseButton, structure.rgrect_MinimizeButton);
                Standard.RECT windowRect = Standard.NativeMethods.GetWindowRect(this._messageHwnd.Handle);
                Rect deviceRectangle = new Rect((double) ((rect.Left - windowRect.Width) - windowRect.Left), (double) (rect.Top - windowRect.Top), (double) rect.Width, (double) rect.Height);
                Rect rect4 = Standard.DpiHelper.DeviceRectToLogical(deviceRectangle);
                this.WindowCaptionButtonsLocation = rect4;
            }
        }

        private void _InitializeCaptionHeight()
        {
            Point devicePoint = new Point(0.0, (double) Standard.NativeMethods.GetSystemMetrics(Standard.SM.CYCAPTION));
            this.WindowCaptionHeight = Standard.DpiHelper.DevicePixelsToLogical(devicePoint).Y;
        }

        private void _InitializeGlassColor()
        {
            bool flag;
            uint num;
            Standard.NativeMethods.DwmGetColorizationColor(out num, out flag);
            num |= flag ? 0xff000000 : 0;
            this.WindowGlassColor = Standard.Utility.ColorFromArgbDword(num);
            SolidColorBrush brush = new SolidColorBrush(this.WindowGlassColor);
            brush.Freeze();
            this.WindowGlassBrush = brush;
        }

        private void _InitializeHighContrast()
        {
            Standard.HIGHCONTRAST highcontrast = Standard.NativeMethods.SystemParameterInfo_GetHIGHCONTRAST();
            this.HighContrast = (highcontrast.dwFlags & Standard.HCF.HIGHCONTRASTON) != 0;
        }

        private void _InitializeIsGlassEnabled()
        {
            this.IsGlassEnabled = Standard.NativeMethods.DwmIsCompositionEnabled();
        }

        private void _InitializeSmallIconSize()
        {
            this.SmallIconSize = new Size((double) Standard.NativeMethods.GetSystemMetrics(Standard.SM.CXSMICON), (double) Standard.NativeMethods.GetSystemMetrics(Standard.SM.CYSMICON));
        }

        private void _InitializeThemeInfo()
        {
            if (!Standard.NativeMethods.IsThemeActive())
            {
                this.UxThemeName = "Classic";
                this.UxThemeColor = "";
            }
            else
            {
                string str;
                string str2;
                string str3;
                Standard.NativeMethods.GetCurrentThemeName(out str, out str2, out str3);
                this.UxThemeName = Path.GetFileNameWithoutExtension(str);
                this.UxThemeColor = str2;
            }
        }

        private void _InitializeWindowCornerRadius()
        {
            CornerRadius radius = new CornerRadius();
            string str = this.UxThemeName.ToUpperInvariant();
            if (str != null)
            {
                if (!(str == "LUNA"))
                {
                    if (str == "AERO")
                    {
                        if (Standard.NativeMethods.DwmIsCompositionEnabled())
                        {
                            radius = new CornerRadius(8.0);
                        }
                        else
                        {
                            radius = new CornerRadius(6.0, 6.0, 0.0, 0.0);
                        }
                        goto Label_00E6;
                    }
                    if (((str == "CLASSIC") || (str == "ZUNE")) || (str == "ROYALE"))
                    {
                    }
                }
                else
                {
                    radius = new CornerRadius(6.0, 6.0, 0.0, 0.0);
                    goto Label_00E6;
                }
            }
            radius = new CornerRadius(0.0);
        Label_00E6:
            this.WindowCornerRadius = radius;
        }

        private void _InitializeWindowNonClientFrameThickness()
        {
            Size deviceSize = new Size((double) Standard.NativeMethods.GetSystemMetrics(Standard.SM.CXFRAME), (double) Standard.NativeMethods.GetSystemMetrics(Standard.SM.CYFRAME));
            Size size2 = Standard.DpiHelper.DeviceSizeToLogical(deviceSize);
            int systemMetrics = Standard.NativeMethods.GetSystemMetrics(Standard.SM.CYCAPTION);
            double y = Standard.DpiHelper.DevicePixelsToLogical(new Point(0.0, (double) systemMetrics)).Y;
            this.WindowNonClientFrameThickness = new Thickness(size2.Width, size2.Height + y, size2.Width, size2.Height);
        }

        private void _InitializeWindowResizeBorderThickness()
        {
            Size deviceSize = new Size((double) Standard.NativeMethods.GetSystemMetrics(Standard.SM.CXFRAME), (double) Standard.NativeMethods.GetSystemMetrics(Standard.SM.CYFRAME));
            Size size2 = Standard.DpiHelper.DeviceSizeToLogical(deviceSize);
            this.WindowResizeBorderThickness = new Thickness(size2.Width, size2.Height, size2.Width, size2.Height);
        }

        private void _LegacyInitializeCaptionButtonLocation()
        {
            int systemMetrics = Standard.NativeMethods.GetSystemMetrics(Standard.SM.CXSIZE);
            int num2 = Standard.NativeMethods.GetSystemMetrics(Standard.SM.CYSIZE);
            int num3 = Standard.NativeMethods.GetSystemMetrics(Standard.SM.CXFRAME) + Standard.NativeMethods.GetSystemMetrics(Standard.SM.CXEDGE);
            int num4 = Standard.NativeMethods.GetSystemMetrics(Standard.SM.CYFRAME) + Standard.NativeMethods.GetSystemMetrics(Standard.SM.CYEDGE);
            Rect rect = new Rect(0.0, 0.0, (double) (systemMetrics * 3), (double) num2);
            rect.Offset(-num3 - rect.Width, (double) num4);
            this.WindowCaptionButtonsLocation = rect;
        }

        private void _NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void _UpdateCaptionButtonLocation(IntPtr wParam, IntPtr lParam)
        {
            this._InitializeCaptionButtonLocation();
        }

        private void _UpdateCaptionHeight(IntPtr wParam, IntPtr lParam)
        {
            this._InitializeCaptionHeight();
        }

        private void _UpdateGlassColor(IntPtr wParam, IntPtr lParam)
        {
            bool flag = lParam != IntPtr.Zero;
            uint color = (uint) ((int) wParam.ToInt64());
            color |= flag ? 0xff000000 : 0;
            this.WindowGlassColor = Standard.Utility.ColorFromArgbDword(color);
            SolidColorBrush brush = new SolidColorBrush(this.WindowGlassColor);
            brush.Freeze();
            this.WindowGlassBrush = brush;
        }

        private void _UpdateHighContrast(IntPtr wParam, IntPtr lParam)
        {
            this._InitializeHighContrast();
        }

        private void _UpdateIsGlassEnabled(IntPtr wParam, IntPtr lParam)
        {
            this._InitializeIsGlassEnabled();
        }

        private void _UpdateSmallIconSize(IntPtr wParam, IntPtr lParam)
        {
            this._InitializeSmallIconSize();
        }

        private void _UpdateThemeInfo(IntPtr wParam, IntPtr lParam)
        {
            this._InitializeThemeInfo();
        }

        private void _UpdateWindowCornerRadius(IntPtr wParam, IntPtr lParam)
        {
            this._InitializeWindowCornerRadius();
        }

        private void _UpdateWindowNonClientFrameThickness(IntPtr wParam, IntPtr lParam)
        {
            this._InitializeWindowNonClientFrameThickness();
        }

        private void _UpdateWindowResizeBorderThickness(IntPtr wParam, IntPtr lParam)
        {
            this._InitializeWindowResizeBorderThickness();
        }

        private IntPtr _WndProc(IntPtr hwnd, Standard.WM msg, IntPtr wParam, IntPtr lParam)
        {
            List<_SystemMetricUpdate> list;
            if ((this._UpdateTable != null) && this._UpdateTable.TryGetValue(msg, out list))
            {
                foreach (_SystemMetricUpdate update in list)
                {
                    update(wParam, lParam);
                }
            }
            return Standard.NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
        }

        public static SystemParameters2 Current
        {
            get
            {
                if (_threadLocalSingleton == null)
                {
                    _threadLocalSingleton = new SystemParameters2();
                }
                return _threadLocalSingleton;
            }
        }

        public bool HighContrast
        {
            get
            {
                return this._isHighContrast;
            }
            private set
            {
                if (value != this._isHighContrast)
                {
                    this._isHighContrast = value;
                    this._NotifyPropertyChanged("HighContrast");
                }
            }
        }

        public bool IsGlassEnabled
        {
            get
            {
                return Standard.NativeMethods.DwmIsCompositionEnabled();
            }
            private set
            {
                if (value != this._isGlassEnabled)
                {
                    this._isGlassEnabled = value;
                    this._NotifyPropertyChanged("IsGlassEnabled");
                }
            }
        }

        public Size SmallIconSize
        {
            get
            {
                return new Size(this._smallIconSize.Width, this._smallIconSize.Height);
            }
            private set
            {
                if (value != this._smallIconSize)
                {
                    this._smallIconSize = value;
                    this._NotifyPropertyChanged("SmallIconSize");
                }
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="Ux"), SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId="Ux")]
        public string UxThemeColor
        {
            get
            {
                return this._uxThemeColor;
            }
            private set
            {
                if (value != this._uxThemeColor)
                {
                    this._uxThemeColor = value;
                    this._NotifyPropertyChanged("UxThemeColor");
                }
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="Ux"), SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId="Ux")]
        public string UxThemeName
        {
            get
            {
                return this._uxThemeName;
            }
            private set
            {
                if (value != this._uxThemeName)
                {
                    this._uxThemeName = value;
                    this._NotifyPropertyChanged("UxThemeName");
                }
            }
        }

        public Rect WindowCaptionButtonsLocation
        {
            get
            {
                return this._captionButtonLocation;
            }
            private set
            {
                if (value != this._captionButtonLocation)
                {
                    this._captionButtonLocation = value;
                    this._NotifyPropertyChanged("WindowCaptionButtonsLocation");
                }
            }
        }

        public double WindowCaptionHeight
        {
            get
            {
                return this._captionHeight;
            }
            private set
            {
                if (value != this._captionHeight)
                {
                    this._captionHeight = value;
                    this._NotifyPropertyChanged("WindowCaptionHeight");
                }
            }
        }

        public CornerRadius WindowCornerRadius
        {
            get
            {
                return this._windowCornerRadius;
            }
            private set
            {
                if (value != this._windowCornerRadius)
                {
                    this._windowCornerRadius = value;
                    this._NotifyPropertyChanged("WindowCornerRadius");
                }
            }
        }

        public SolidColorBrush WindowGlassBrush
        {
            get
            {
                return this._glassColorBrush;
            }
            private set
            {
                if ((this._glassColorBrush == null) || (value.Color != this._glassColorBrush.Color))
                {
                    this._glassColorBrush = value;
                    this._NotifyPropertyChanged("WindowGlassBrush");
                }
            }
        }

        public Color WindowGlassColor
        {
            get
            {
                return this._glassColor;
            }
            private set
            {
                if (value != this._glassColor)
                {
                    this._glassColor = value;
                    this._NotifyPropertyChanged("WindowGlassColor");
                }
            }
        }

        public Thickness WindowNonClientFrameThickness
        {
            get
            {
                return this._windowNonClientFrameThickness;
            }
            private set
            {
                if (value != this._windowNonClientFrameThickness)
                {
                    this._windowNonClientFrameThickness = value;
                    this._NotifyPropertyChanged("WindowNonClientFrameThickness");
                }
            }
        }

        public Thickness WindowResizeBorderThickness
        {
            get
            {
                return this._windowResizeBorderThickness;
            }
            private set
            {
                if (value != this._windowResizeBorderThickness)
                {
                    this._windowResizeBorderThickness = value;
                    this._NotifyPropertyChanged("WindowResizeBorderThickness");
                }
            }
        }

        private delegate void _SystemMetricUpdate(IntPtr wParam, IntPtr lParam);
    }
}

