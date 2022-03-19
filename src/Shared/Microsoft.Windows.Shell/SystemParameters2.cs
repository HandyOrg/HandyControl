using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Standard;

namespace Microsoft.Windows.Shell;

[SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
public class SystemParameters2 : INotifyPropertyChanged
{
    private void _InitializeIsGlassEnabled()
    {
        this.IsGlassEnabled = NativeMethods.DwmIsCompositionEnabled();
    }

    private void _UpdateIsGlassEnabled(IntPtr wParam, IntPtr lParam)
    {
        this._InitializeIsGlassEnabled();
    }

    private void _InitializeGlassColor()
    {
        uint num;
        bool flag;
        NativeMethods.DwmGetColorizationColor(out num, out flag);
        num |= (flag ? 4278190080u : 0u);
        this.WindowGlassColor = Utility.ColorFromArgbDword(num);
        SolidColorBrush solidColorBrush = new SolidColorBrush(this.WindowGlassColor);
        solidColorBrush.Freeze();
        this.WindowGlassBrush = solidColorBrush;
    }

    private void _UpdateGlassColor(IntPtr wParam, IntPtr lParam)
    {
        bool flag = lParam != IntPtr.Zero;
        uint num = (uint) ((int) wParam.ToInt64());
        num |= (flag ? 4278190080u : 0u);
        this.WindowGlassColor = Utility.ColorFromArgbDword(num);
        SolidColorBrush solidColorBrush = new SolidColorBrush(this.WindowGlassColor);
        solidColorBrush.Freeze();
        this.WindowGlassBrush = solidColorBrush;
    }

    private void _InitializeCaptionHeight()
    {
        Point devicePoint = new Point(0.0, (double) NativeMethods.GetSystemMetrics(SM.CYCAPTION));
        this.WindowCaptionHeight = DpiHelper.DevicePixelsToLogical(devicePoint).Y;
    }

    private void _UpdateCaptionHeight(IntPtr wParam, IntPtr lParam)
    {
        this._InitializeCaptionHeight();
    }

    private void _InitializeWindowResizeBorderThickness()
    {
        Size deviceSize = new Size((double) NativeMethods.GetSystemMetrics(SM.CXFRAME), (double) NativeMethods.GetSystemMetrics(SM.CYFRAME));
        Size size = DpiHelper.DeviceSizeToLogical(deviceSize);
        this.WindowResizeBorderThickness = new Thickness(size.Width, size.Height, size.Width, size.Height);
    }

    private void _UpdateWindowResizeBorderThickness(IntPtr wParam, IntPtr lParam)
    {
        this._InitializeWindowResizeBorderThickness();
    }

    private void _InitializeWindowNonClientFrameThickness()
    {
        Size deviceSize = new Size((double) NativeMethods.GetSystemMetrics(SM.CXFRAME), (double) NativeMethods.GetSystemMetrics(SM.CYFRAME));
        Size size = DpiHelper.DeviceSizeToLogical(deviceSize);
        int systemMetrics = NativeMethods.GetSystemMetrics(SM.CYCAPTION);
        double y = DpiHelper.DevicePixelsToLogical(new Point(0.0, (double) systemMetrics)).Y;
        this.WindowNonClientFrameThickness = new Thickness(size.Width, size.Height + y, size.Width, size.Height);
    }

    private void _UpdateWindowNonClientFrameThickness(IntPtr wParam, IntPtr lParam)
    {
        this._InitializeWindowNonClientFrameThickness();
    }

    private void _InitializeSmallIconSize()
    {
        this.SmallIconSize = new Size((double) NativeMethods.GetSystemMetrics(SM.CXSMICON), (double) NativeMethods.GetSystemMetrics(SM.CYSMICON));
    }

    private void _UpdateSmallIconSize(IntPtr wParam, IntPtr lParam)
    {
        this._InitializeSmallIconSize();
    }

    private void _LegacyInitializeCaptionButtonLocation()
    {
        int systemMetrics = NativeMethods.GetSystemMetrics(SM.CXSIZE);
        int systemMetrics2 = NativeMethods.GetSystemMetrics(SM.CYSIZE);
        int num = NativeMethods.GetSystemMetrics(SM.CXFRAME) + NativeMethods.GetSystemMetrics(SM.CXEDGE);
        int num2 = NativeMethods.GetSystemMetrics(SM.CYFRAME) + NativeMethods.GetSystemMetrics(SM.CYEDGE);
        Rect windowCaptionButtonsLocation = new Rect(0.0, 0.0, (double) (systemMetrics * 3), (double) systemMetrics2);
        windowCaptionButtonsLocation.Offset((double) (-(double) num) - windowCaptionButtonsLocation.Width, (double) num2);
        this.WindowCaptionButtonsLocation = windowCaptionButtonsLocation;
    }

    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    private void _InitializeCaptionButtonLocation()
    {
        if (!Utility.IsOSVistaOrNewer || !NativeMethods.IsThemeActive())
        {
            this._LegacyInitializeCaptionButtonLocation();
            return;
        }
        TITLEBARINFOEX titlebarinfoex = new TITLEBARINFOEX
        {
            cbSize = Marshal.SizeOf(typeof(TITLEBARINFOEX))
        };
        IntPtr intPtr = Marshal.AllocHGlobal(titlebarinfoex.cbSize);
        try
        {
            Marshal.StructureToPtr(titlebarinfoex, intPtr, false);
            NativeMethods.ShowWindow(this._messageHwnd.Handle, SW.SHOW);
            NativeMethods.SendMessage(this._messageHwnd.Handle, WM.GETTITLEBARINFOEX, IntPtr.Zero, intPtr);
            titlebarinfoex = (TITLEBARINFOEX) Marshal.PtrToStructure(intPtr, typeof(TITLEBARINFOEX));
        }
        finally
        {
            NativeMethods.ShowWindow(this._messageHwnd.Handle, SW.HIDE);
            Utility.SafeFreeHGlobal(ref intPtr);
        }
        RECT rect = RECT.Union(titlebarinfoex.rgrect_CloseButton, titlebarinfoex.rgrect_MinimizeButton);
        RECT windowRect = NativeMethods.GetWindowRect(this._messageHwnd.Handle);
        Rect deviceRectangle = new Rect((double) (rect.Left - windowRect.Width - windowRect.Left), (double) (rect.Top - windowRect.Top), (double) rect.Width, (double) rect.Height);
        Rect windowCaptionButtonsLocation = DpiHelper.DeviceRectToLogical(deviceRectangle);
        this.WindowCaptionButtonsLocation = windowCaptionButtonsLocation;
    }

    private void _UpdateCaptionButtonLocation(IntPtr wParam, IntPtr lParam)
    {
        this._InitializeCaptionButtonLocation();
    }

    private void _InitializeHighContrast()
    {
        this.HighContrast = ((NativeMethods.SystemParameterInfo_GetHIGHCONTRAST().dwFlags & HCF.HIGHCONTRASTON) != (HCF) 0);
    }

    private void _UpdateHighContrast(IntPtr wParam, IntPtr lParam)
    {
        this._InitializeHighContrast();
    }

    private void _InitializeThemeInfo()
    {
        if (!NativeMethods.IsThemeActive())
        {
            this.UxThemeName = "Classic";
            this.UxThemeColor = "";
            return;
        }
        string path;
        string uxThemeColor;
        string text;
        NativeMethods.GetCurrentThemeName(out path, out uxThemeColor, out text);
        this.UxThemeName = Path.GetFileNameWithoutExtension(path);
        this.UxThemeColor = uxThemeColor;
    }

    private void _UpdateThemeInfo(IntPtr wParam, IntPtr lParam)
    {
        this._InitializeThemeInfo();
    }

    private void _InitializeWindowCornerRadius()
    {
        CornerRadius windowCornerRadius = default(CornerRadius);
        string a;
        if ((a = this.UxThemeName.ToUpperInvariant()) != null)
        {
            if (a == "LUNA")
            {
                windowCornerRadius = new CornerRadius(6.0, 6.0, 0.0, 0.0);
                goto IL_E6;
            }
            if (!(a == "AERO"))
            {
                if (!(a == "CLASSIC") && !(a == "ZUNE") && !(a == "ROYALE"))
                {
                }
            }
            else
            {
                if (NativeMethods.DwmIsCompositionEnabled())
                {
                    windowCornerRadius = new CornerRadius(8.0);
                    goto IL_E6;
                }
                windowCornerRadius = new CornerRadius(6.0, 6.0, 0.0, 0.0);
                goto IL_E6;
            }
        }
        windowCornerRadius = new CornerRadius(0.0);
IL_E6:
        this.WindowCornerRadius = windowCornerRadius;
    }

    private void _UpdateWindowCornerRadius(IntPtr wParam, IntPtr lParam)
    {
        this._InitializeWindowCornerRadius();
    }

    private SystemParameters2()
    {
        this._messageHwnd = new MessageWindow((CS) 0u, WS.DISABLED | WS.BORDER | WS.DLGFRAME | WS.SYSMENU | WS.THICKFRAME | WS.GROUP | WS.TABSTOP, WS_EX.None, new Rect(-16000.0, -16000.0, 100.0, 100.0), "", new WndProc(this._WndProc));
        this._messageHwnd.Dispatcher.ShutdownStarted += delegate (object sender, EventArgs e)
        {
            Utility.SafeDispose<MessageWindow>(ref this._messageHwnd);
        };
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
        this._UpdateTable = new Dictionary<WM, List<SystemParameters2._SystemMetricUpdate>>
        {
            {
                WM.THEMECHANGED,
                new List<SystemParameters2._SystemMetricUpdate>
                {
                    new SystemParameters2._SystemMetricUpdate(this._UpdateThemeInfo),
                    new SystemParameters2._SystemMetricUpdate(this._UpdateHighContrast),
                    new SystemParameters2._SystemMetricUpdate(this._UpdateWindowCornerRadius),
                    new SystemParameters2._SystemMetricUpdate(this._UpdateCaptionButtonLocation)
                }
            },
            {
                WM.WININICHANGE,
                new List<SystemParameters2._SystemMetricUpdate>
                {
                    new SystemParameters2._SystemMetricUpdate(this._UpdateCaptionHeight),
                    new SystemParameters2._SystemMetricUpdate(this._UpdateWindowResizeBorderThickness),
                    new SystemParameters2._SystemMetricUpdate(this._UpdateSmallIconSize),
                    new SystemParameters2._SystemMetricUpdate(this._UpdateHighContrast),
                    new SystemParameters2._SystemMetricUpdate(this._UpdateWindowNonClientFrameThickness),
                    new SystemParameters2._SystemMetricUpdate(this._UpdateCaptionButtonLocation)
                }
            },
            {
                WM.DWMNCRENDERINGCHANGED,
                new List<SystemParameters2._SystemMetricUpdate>
                {
                    new SystemParameters2._SystemMetricUpdate(this._UpdateIsGlassEnabled)
                }
            },
            {
                WM.DWMCOMPOSITIONCHANGED,
                new List<SystemParameters2._SystemMetricUpdate>
                {
                    new SystemParameters2._SystemMetricUpdate(this._UpdateIsGlassEnabled)
                }
            },
            {
                WM.DWMCOLORIZATIONCOLORCHANGED,
                new List<SystemParameters2._SystemMetricUpdate>
                {
                    new SystemParameters2._SystemMetricUpdate(this._UpdateGlassColor)
                }
            }
        };
    }

    public static SystemParameters2 Current
    {
        get
        {
            if (SystemParameters2._threadLocalSingleton == null)
            {
                SystemParameters2._threadLocalSingleton = new SystemParameters2();
            }
            return SystemParameters2._threadLocalSingleton;
        }
    }

    private IntPtr _WndProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam)
    {
        List<SystemParameters2._SystemMetricUpdate> list;
        if (this._UpdateTable != null && this._UpdateTable.TryGetValue(msg, out list))
        {
            foreach (SystemParameters2._SystemMetricUpdate systemMetricUpdate in list)
            {
                systemMetricUpdate(wParam, lParam);
            }
        }
        return NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
    }

    public bool IsGlassEnabled
    {
        get
        {
            return NativeMethods.DwmIsCompositionEnabled();
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

    public SolidColorBrush WindowGlassBrush
    {
        get
        {
            return this._glassColorBrush;
        }
        private set
        {
            if (this._glassColorBrush == null || value.Color != this._glassColorBrush.Color)
            {
                this._glassColorBrush = value;
                this._NotifyPropertyChanged("WindowGlassBrush");
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

    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ux")]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ux")]
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

    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ux")]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ux")]
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

    private void _NotifyPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
        if (propertyChanged != null)
        {
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [ThreadStatic]
    private static SystemParameters2 _threadLocalSingleton;

    private MessageWindow _messageHwnd;

    private bool _isGlassEnabled;

    private Color _glassColor;

    private SolidColorBrush _glassColorBrush;

    private Thickness _windowResizeBorderThickness;

    private Thickness _windowNonClientFrameThickness;

    private double _captionHeight;

    private Size _smallIconSize;

    private string _uxThemeName;

    private string _uxThemeColor;

    private bool _isHighContrast;

    private CornerRadius _windowCornerRadius;

    private Rect _captionButtonLocation;

    private readonly Dictionary<WM, List<SystemParameters2._SystemMetricUpdate>> _UpdateTable;

    private delegate void _SystemMetricUpdate(IntPtr wParam, IntPtr lParam);
}
