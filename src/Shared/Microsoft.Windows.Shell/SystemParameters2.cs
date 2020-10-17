using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Standard;

namespace Microsoft.Windows.Shell
{
	// Token: 0x0200009E RID: 158
	[SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
	public class SystemParameters2 : INotifyPropertyChanged
	{
		// Token: 0x06000279 RID: 633 RVA: 0x00006A7A File Offset: 0x00004C7A
		private void _InitializeIsGlassEnabled()
		{
			this.IsGlassEnabled = NativeMethods.DwmIsCompositionEnabled();
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00006A87 File Offset: 0x00004C87
		private void _UpdateIsGlassEnabled(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeIsGlassEnabled();
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00006A90 File Offset: 0x00004C90
		private void _InitializeGlassColor()
		{
			uint num;
			bool flag;
			NativeMethods.DwmGetColorizationColor(out num, out flag);
			num |= (flag ? 4278190080U : 0U);
			this.WindowGlassColor = Utility.ColorFromArgbDword(num);
			SolidColorBrush solidColorBrush = new SolidColorBrush(this.WindowGlassColor);
			solidColorBrush.Freeze();
			this.WindowGlassBrush = solidColorBrush;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00006ADC File Offset: 0x00004CDC
		private void _UpdateGlassColor(IntPtr wParam, IntPtr lParam)
		{
			bool flag = lParam != IntPtr.Zero;
			uint num = (uint)((int)wParam.ToInt64());
			num |= (flag ? 4278190080U : 0U);
			this.WindowGlassColor = Utility.ColorFromArgbDword(num);
			SolidColorBrush solidColorBrush = new SolidColorBrush(this.WindowGlassColor);
			solidColorBrush.Freeze();
			this.WindowGlassBrush = solidColorBrush;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00006B34 File Offset: 0x00004D34
		private void _InitializeCaptionHeight()
		{
			Point devicePoint = new Point(0.0, (double)NativeMethods.GetSystemMetrics(SM.CYCAPTION));
			this.WindowCaptionHeight = DpiHelper.DevicePixelsToLogical(devicePoint).Y;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00006B6C File Offset: 0x00004D6C
		private void _UpdateCaptionHeight(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeCaptionHeight();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00006B74 File Offset: 0x00004D74
		private void _InitializeWindowResizeBorderThickness()
		{
			Size deviceSize = new Size((double)NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)NativeMethods.GetSystemMetrics(SM.CYFRAME));
			Size size = DpiHelper.DeviceSizeToLogical(deviceSize);
			this.WindowResizeBorderThickness = new Thickness(size.Width, size.Height, size.Width, size.Height);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00006BC6 File Offset: 0x00004DC6
		private void _UpdateWindowResizeBorderThickness(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeWindowResizeBorderThickness();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00006BD0 File Offset: 0x00004DD0
		private void _InitializeWindowNonClientFrameThickness()
		{
			Size deviceSize = new Size((double)NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)NativeMethods.GetSystemMetrics(SM.CYFRAME));
			Size size = DpiHelper.DeviceSizeToLogical(deviceSize);
			int systemMetrics = NativeMethods.GetSystemMetrics(SM.CYCAPTION);
			double y = DpiHelper.DevicePixelsToLogical(new Point(0.0, (double)systemMetrics)).Y;
			this.WindowNonClientFrameThickness = new Thickness(size.Width, size.Height + y, size.Width, size.Height);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00006C4A File Offset: 0x00004E4A
		private void _UpdateWindowNonClientFrameThickness(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeWindowNonClientFrameThickness();
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00006C52 File Offset: 0x00004E52
		private void _InitializeSmallIconSize()
		{
			this.SmallIconSize = new Size((double)NativeMethods.GetSystemMetrics(SM.CXSMICON), (double)NativeMethods.GetSystemMetrics(SM.CYSMICON));
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00006C6F File Offset: 0x00004E6F
		private void _UpdateSmallIconSize(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeSmallIconSize();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00006C78 File Offset: 0x00004E78
		private void _LegacyInitializeCaptionButtonLocation()
		{
			int systemMetrics = NativeMethods.GetSystemMetrics(SM.CXSIZE);
			int systemMetrics2 = NativeMethods.GetSystemMetrics(SM.CYSIZE);
			int num = NativeMethods.GetSystemMetrics(SM.CXFRAME) + NativeMethods.GetSystemMetrics(SM.CXEDGE);
			int num2 = NativeMethods.GetSystemMetrics(SM.CYFRAME) + NativeMethods.GetSystemMetrics(SM.CYEDGE);
			Rect windowCaptionButtonsLocation = new Rect(0.0, 0.0, (double)(systemMetrics * 3), (double)systemMetrics2);
			windowCaptionButtonsLocation.Offset((double)(-(double)num) - windowCaptionButtonsLocation.Width, (double)num2);
			this.WindowCaptionButtonsLocation = windowCaptionButtonsLocation;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00006CF0 File Offset: 0x00004EF0
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
				titlebarinfoex = (TITLEBARINFOEX)Marshal.PtrToStructure(intPtr, typeof(TITLEBARINFOEX));
			}
			finally
			{
				NativeMethods.ShowWindow(this._messageHwnd.Handle, SW.HIDE);
				Utility.SafeFreeHGlobal(ref intPtr);
			}
			RECT rect = RECT.Union(titlebarinfoex.rgrect_CloseButton, titlebarinfoex.rgrect_MinimizeButton);
			RECT windowRect = NativeMethods.GetWindowRect(this._messageHwnd.Handle);
			Rect deviceRectangle = new Rect((double)(rect.Left - windowRect.Width - windowRect.Left), (double)(rect.Top - windowRect.Top), (double)rect.Width, (double)rect.Height);
			Rect windowCaptionButtonsLocation = DpiHelper.DeviceRectToLogical(deviceRectangle);
			this.WindowCaptionButtonsLocation = windowCaptionButtonsLocation;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00006E34 File Offset: 0x00005034
		private void _UpdateCaptionButtonLocation(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeCaptionButtonLocation();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00006E3C File Offset: 0x0000503C
		private void _InitializeHighContrast()
		{
			this.HighContrast = ((NativeMethods.SystemParameterInfo_GetHIGHCONTRAST().dwFlags & HCF.HIGHCONTRASTON) != (HCF)0);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00006E64 File Offset: 0x00005064
		private void _UpdateHighContrast(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeHighContrast();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00006E6C File Offset: 0x0000506C
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

		// Token: 0x0600028B RID: 651 RVA: 0x00006EB5 File Offset: 0x000050B5
		private void _UpdateThemeInfo(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeThemeInfo();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00006EC0 File Offset: 0x000050C0
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

		// Token: 0x0600028D RID: 653 RVA: 0x00006FBA File Offset: 0x000051BA
		private void _UpdateWindowCornerRadius(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeWindowCornerRadius();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00006FD0 File Offset: 0x000051D0
		private SystemParameters2()
		{
			this._messageHwnd = new MessageWindow((CS)0U, WS.DISABLED | WS.BORDER | WS.DLGFRAME | WS.SYSMENU | WS.THICKFRAME | WS.GROUP | WS.TABSTOP, WS_EX.None, new Rect(-16000.0, -16000.0, 100.0, 100.0), "", new WndProc(this._WndProc));
			this._messageHwnd.Dispatcher.ShutdownStarted += delegate(object sender, EventArgs e)
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600028F RID: 655 RVA: 0x000071E6 File Offset: 0x000053E6
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

		// Token: 0x06000290 RID: 656 RVA: 0x00007200 File Offset: 0x00005400
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00007274 File Offset: 0x00005474
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000727B File Offset: 0x0000547B
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00007298 File Offset: 0x00005498
		// (set) Token: 0x06000294 RID: 660 RVA: 0x000072A0 File Offset: 0x000054A0
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000295 RID: 661 RVA: 0x000072C2 File Offset: 0x000054C2
		// (set) Token: 0x06000296 RID: 662 RVA: 0x000072CA File Offset: 0x000054CA
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000297 RID: 663 RVA: 0x000072FE File Offset: 0x000054FE
		// (set) Token: 0x06000298 RID: 664 RVA: 0x00007306 File Offset: 0x00005506
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00007328 File Offset: 0x00005528
		// (set) Token: 0x0600029A RID: 666 RVA: 0x00007330 File Offset: 0x00005530
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600029B RID: 667 RVA: 0x00007352 File Offset: 0x00005552
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000735A File Offset: 0x0000555A
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00007377 File Offset: 0x00005577
		// (set) Token: 0x0600029E RID: 670 RVA: 0x00007394 File Offset: 0x00005594
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600029F RID: 671 RVA: 0x000073B6 File Offset: 0x000055B6
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x000073BE File Offset: 0x000055BE
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x000073E0 File Offset: 0x000055E0
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x000073E8 File Offset: 0x000055E8
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000740A File Offset: 0x0000560A
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x00007412 File Offset: 0x00005612
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000742F File Offset: 0x0000562F
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x00007437 File Offset: 0x00005637
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00007459 File Offset: 0x00005659
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x00007461 File Offset: 0x00005661
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

		// Token: 0x060002A9 RID: 681 RVA: 0x00007484 File Offset: 0x00005684
		private void _NotifyPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002AA RID: 682 RVA: 0x000074A8 File Offset: 0x000056A8
		// (remove) Token: 0x060002AB RID: 683 RVA: 0x000074E0 File Offset: 0x000056E0
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040005A7 RID: 1447
		[ThreadStatic]
		private static SystemParameters2 _threadLocalSingleton;

		// Token: 0x040005A8 RID: 1448
		private MessageWindow _messageHwnd;

		// Token: 0x040005A9 RID: 1449
		private bool _isGlassEnabled;

		// Token: 0x040005AA RID: 1450
		private Color _glassColor;

		// Token: 0x040005AB RID: 1451
		private SolidColorBrush _glassColorBrush;

		// Token: 0x040005AC RID: 1452
		private Thickness _windowResizeBorderThickness;

		// Token: 0x040005AD RID: 1453
		private Thickness _windowNonClientFrameThickness;

		// Token: 0x040005AE RID: 1454
		private double _captionHeight;

		// Token: 0x040005AF RID: 1455
		private Size _smallIconSize;

		// Token: 0x040005B0 RID: 1456
		private string _uxThemeName;

		// Token: 0x040005B1 RID: 1457
		private string _uxThemeColor;

		// Token: 0x040005B2 RID: 1458
		private bool _isHighContrast;

		// Token: 0x040005B3 RID: 1459
		private CornerRadius _windowCornerRadius;

		// Token: 0x040005B4 RID: 1460
		private Rect _captionButtonLocation;

		// Token: 0x040005B5 RID: 1461
		private readonly Dictionary<WM, List<SystemParameters2._SystemMetricUpdate>> _UpdateTable;

		// Token: 0x0200009F RID: 159
		// (Invoke) Token: 0x060002AE RID: 686
		private delegate void _SystemMetricUpdate(IntPtr wParam, IntPtr lParam);
	}
}
