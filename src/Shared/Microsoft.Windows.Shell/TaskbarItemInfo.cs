using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Standard;

namespace Microsoft.Windows.Shell
{
	// Token: 0x020000A1 RID: 161
	public sealed class TaskbarItemInfo : Freezable
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x00007515 File Offset: 0x00005715
		protected override Freezable CreateInstanceCore()
		{
			return new TaskbarItemInfo();
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000751C File Offset: 0x0000571C
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		public static TaskbarItemInfo GetTaskbarItemInfo(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			return (TaskbarItemInfo)window.GetValue(TaskbarItemInfo.TaskbarItemInfoProperty);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00007539 File Offset: 0x00005739
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		public static void SetTaskbarItemInfo(Window window, TaskbarItemInfo value)
		{
			Verify.IsNotNull<Window>(window, "window");
			window.SetValue(TaskbarItemInfo.TaskbarItemInfoProperty, value);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00007554 File Offset: 0x00005754
		private static void _OnTaskbarItemInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(d))
			{
				return;
			}
			Window window = (Window)d;
			TaskbarItemInfo taskbarItemInfo = (TaskbarItemInfo)e.OldValue;
			TaskbarItemInfo taskbarItemInfo2 = (TaskbarItemInfo)e.NewValue;
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

		// Token: 0x060002B5 RID: 693 RVA: 0x000075B4 File Offset: 0x000057B4
		private static object _CoerceTaskbarItemInfoValue(DependencyObject d, object value)
		{
			if (DesignerProperties.GetIsInDesignMode(d))
			{
				return value;
			}
			Verify.IsNotNull<DependencyObject>(d, "d");
			Window window = (Window)d;
			TaskbarItemInfo taskbarItemInfo = (TaskbarItemInfo)value;
			if (taskbarItemInfo != null && taskbarItemInfo._window != null && taskbarItemInfo._window != window)
			{
				throw new NotSupportedException();
			}
			window.VerifyAccess();
			return taskbarItemInfo;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x00007605 File Offset: 0x00005805
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x00007617 File Offset: 0x00005817
		public TaskbarItemProgressState ProgressState
		{
			get
			{
				return (TaskbarItemProgressState)base.GetValue(TaskbarItemInfo.ProgressStateProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ProgressStateProperty, value);
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000762A File Offset: 0x0000582A
		private void _OnProgressStateChanged()
		{
			if (!this._isAttached)
			{
				return;
			}
			this._UpdateProgressState(true);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00007640 File Offset: 0x00005840
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000766D File Offset: 0x0000586D
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000767F File Offset: 0x0000587F
		public double ProgressValue
		{
			get
			{
				return (double)base.GetValue(TaskbarItemInfo.ProgressValueProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ProgressValueProperty, value);
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00007692 File Offset: 0x00005892
		private void _OnProgressValueChanged()
		{
			if (!this._isAttached)
			{
				return;
			}
			this._UpdateProgressValue(true);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000076A5 File Offset: 0x000058A5
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060002BE RID: 702 RVA: 0x000076DD File Offset: 0x000058DD
		// (set) Token: 0x060002BF RID: 703 RVA: 0x000076EF File Offset: 0x000058EF
		public ImageSource Overlay
		{
			get
			{
				return (ImageSource)base.GetValue(TaskbarItemInfo.OverlayProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.OverlayProperty, value);
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000076FD File Offset: 0x000058FD
		private void _OnOverlayChanged()
		{
			if (!this._isAttached)
			{
				return;
			}
			this._UpdateOverlay(true);
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00007710 File Offset: 0x00005910
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x00007722 File Offset: 0x00005922
		public string Description
		{
			get
			{
				return (string)base.GetValue(TaskbarItemInfo.DescriptionProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.DescriptionProperty, value);
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00007730 File Offset: 0x00005930
		private void _OnDescriptionChanged()
		{
			if (!this._isAttached)
			{
				return;
			}
			this._UpdateTooltip(true);
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00007743 File Offset: 0x00005943
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x00007755 File Offset: 0x00005955
		public Thickness ThumbnailClipMargin
		{
			get
			{
				return (Thickness)base.GetValue(TaskbarItemInfo.ThumbnailClipMarginProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ThumbnailClipMarginProperty, value);
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00007768 File Offset: 0x00005968
		private void _OnThumbnailClipMarginChanged()
		{
			if (!this._isAttached)
			{
				return;
			}
			this._UpdateThumbnailClipping(true);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000777C File Offset: 0x0000597C
		private static Thickness _CoerceThumbnailClipMargin(Thickness margin)
		{
			if (margin.Left < 0.0 || margin.Right < 0.0 || margin.Top < 0.0 || margin.Bottom < 0.0)
			{
				return TaskbarItemInfo._EmptyThickness;
			}
			return margin;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x000077D8 File Offset: 0x000059D8
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x000077EA File Offset: 0x000059EA
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
		public ThumbButtonInfoCollection ThumbButtonInfos
		{
			get
			{
				return (ThumbButtonInfoCollection)base.GetValue(TaskbarItemInfo.ThumbButtonInfosProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ThumbButtonInfosProperty, value);
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x000077F8 File Offset: 0x000059F8
		private void _OnThumbButtonsChanged()
		{
			if (!this._isAttached)
			{
				return;
			}
			this._UpdateThumbButtons(true);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000780B File Offset: 0x00005A0B
		private IntPtr _GetHICONFromImageSource(ImageSource image, Size dimensions)
		{
			if (this._gdipToken == null)
			{
				this._gdipToken = SafeGdiplusStartupToken.Startup();
			}
			return Utility.GenerateHICON(image, dimensions);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00007828 File Offset: 0x00005A28
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
				this._overlaySize = new Size((double)NativeMethods.GetSystemMetrics(SM.CXSMICON), (double)NativeMethods.GetSystemMetrics(SM.CYSMICON));
			}
			this.ThumbButtonInfos = new ThumbButtonInfoCollection();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000078A4 File Offset: 0x00005AA4
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

		// Token: 0x060002CE RID: 718 RVA: 0x00007928 File Offset: 0x00005B28
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

		// Token: 0x060002CF RID: 719 RVA: 0x000079A0 File Offset: 0x00005BA0
		private IntPtr _WndProc(IntPtr hwnd, int uMsg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (uMsg == (int)TaskbarItemInfo.WM_TASKBARBUTTONCREATED)
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

		// Token: 0x060002D0 RID: 720 RVA: 0x00007A28 File Offset: 0x00005C28
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

		// Token: 0x060002D1 RID: 721 RVA: 0x00007A84 File Offset: 0x00005C84
		private void _DetachWindow()
		{
			this._window.SourceInitialized -= this._OnWindowSourceInitialized;
			this._isAttached = false;
			this._OnIsAttachedChanged(false);
			this._window = null;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00007AB4 File Offset: 0x00005CB4
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

		// Token: 0x060002D3 RID: 723 RVA: 0x00007B34 File Offset: 0x00005D34
		private HRESULT _UpdateTooltip(bool attached)
		{
			string pszTip = this.Description ?? "";
			if (!attached)
			{
				pszTip = "";
			}
			return this._taskbarList.SetThumbnailTooltip(this._hwndSource.Handle, pszTip);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00007B74 File Offset: 0x00005D74
		private HRESULT _UpdateProgressValue(bool attached)
		{
			if (!attached || this.ProgressState == TaskbarItemProgressState.None || this.ProgressState == TaskbarItemProgressState.Indeterminate)
			{
				return HRESULT.S_OK;
			}
			ulong ullCompleted = (ulong)(this.ProgressValue * 1000.0);
			return this._taskbarList.SetProgressValue(this._hwndSource.Handle, ullCompleted, 1000UL);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00007BCC File Offset: 0x00005DCC
		private HRESULT _UpdateProgressState(bool attached)
		{
			TaskbarItemProgressState progressState = this.ProgressState;
			TBPF tbpFlags = TBPF.NOPROGRESS;
			if (attached)
			{
				switch (progressState)
				{
				case TaskbarItemProgressState.None:
					tbpFlags = TBPF.NOPROGRESS;
					break;
				case TaskbarItemProgressState.Indeterminate:
					tbpFlags = TBPF.INDETERMINATE;
					break;
				case TaskbarItemProgressState.Normal:
					tbpFlags = TBPF.NORMAL;
					break;
				case TaskbarItemProgressState.Error:
					tbpFlags = TBPF.ERROR;
					break;
				case TaskbarItemProgressState.Paused:
					tbpFlags = TBPF.PAUSED;
					break;
				default:
					tbpFlags = TBPF.NOPROGRESS;
					break;
				}
			}
			HRESULT result = this._taskbarList.SetProgressState(this._hwndSource.Handle, tbpFlags);
			if (result.Succeeded)
			{
				result = this._UpdateProgressValue(attached);
			}
			return result;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00007C44 File Offset: 0x00005E44
		private HRESULT _UpdateThumbnailClipping(bool attached)
		{
			RefRECT prcClip = null;
			if (attached && this.ThumbnailClipMargin != TaskbarItemInfo._EmptyThickness)
			{
				Thickness thumbnailClipMargin = this.ThumbnailClipMargin;
				RECT clientRect = NativeMethods.GetClientRect(this._hwndSource.Handle);
				Rect rect = DpiHelper.DeviceRectToLogical(new Rect((double)clientRect.Left, (double)clientRect.Top, (double)clientRect.Width, (double)clientRect.Height));
				if (thumbnailClipMargin.Left + thumbnailClipMargin.Right >= rect.Width || thumbnailClipMargin.Top + thumbnailClipMargin.Bottom >= rect.Height)
				{
					prcClip = new RefRECT(0, 0, 0, 0);
				}
				else
				{
					Rect logicalRectangle = new Rect(thumbnailClipMargin.Left, thumbnailClipMargin.Top, rect.Width - thumbnailClipMargin.Left - thumbnailClipMargin.Right, rect.Height - thumbnailClipMargin.Top - thumbnailClipMargin.Bottom);
					Rect rect2 = DpiHelper.LogicalRectToDevice(logicalRectangle);
					prcClip = new RefRECT((int)rect2.Left, (int)rect2.Top, (int)rect2.Right, (int)rect2.Bottom);
				}
			}
			return this._taskbarList.SetThumbnailClip(this._hwndSource.Handle, prcClip);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00007D7C File Offset: 0x00005F7C
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
						iId = (uint)i,
						dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND | THBF.HIDDEN),
						dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
					};
				}
				hresult = this._taskbarList.ThumbBarAddButtons(this._hwndSource.Handle, (uint)array.Length, array);
				if (hresult == HRESULT.E_INVALIDARG)
				{
					hresult = HRESULT.S_FALSE;
				}
				this._haveAddedButtons = hresult.Succeeded;
			}
			return hresult;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00007E18 File Offset: 0x00006018
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
				uint num = 0U;
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
						array[(int)((UIntPtr)num)] = thumbbutton;
						num += 1U;
						if (num != 7U)
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
				array[(int)((UIntPtr)num)] = new THUMBBUTTON
				{
					iId = num,
					dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND | THBF.HIDDEN),
					dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
				};
				num += 1U;
				IL_1AE:
				if (num < 7U)
				{
					goto IL_179;
				}
				result2 = this._taskbarList.ThumbBarUpdateButtons(this._hwndSource.Handle, (uint)array.Length, array);
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

		// Token: 0x040005BD RID: 1469
		private const int c_MaximumThumbButtons = 7;

		// Token: 0x040005BE RID: 1470
		private static readonly WM WM_TASKBARBUTTONCREATED = NativeMethods.RegisterWindowMessage("TaskbarButtonCreated");

		// Token: 0x040005BF RID: 1471
		private static readonly Thickness _EmptyThickness = default(Thickness);

		// Token: 0x040005C0 RID: 1472
		private SafeGdiplusStartupToken _gdipToken;

		// Token: 0x040005C1 RID: 1473
		private bool _haveAddedButtons;

		// Token: 0x040005C2 RID: 1474
		private Window _window;

		// Token: 0x040005C3 RID: 1475
		private HwndSource _hwndSource;

		// Token: 0x040005C4 RID: 1476
		private ITaskbarList3 _taskbarList;

		// Token: 0x040005C5 RID: 1477
		private readonly Size _overlaySize;

		// Token: 0x040005C6 RID: 1478
		private bool _isAttached;

		// Token: 0x040005C7 RID: 1479
		public static readonly DependencyProperty TaskbarItemInfoProperty = DependencyProperty.RegisterAttached("TaskbarItemInfo", typeof(TaskbarItemInfo), typeof(TaskbarItemInfo), new PropertyMetadata(null, new PropertyChangedCallback(TaskbarItemInfo._OnTaskbarItemInfoChanged), new CoerceValueCallback(TaskbarItemInfo._CoerceTaskbarItemInfoValue)));

		// Token: 0x040005C8 RID: 1480
		public static readonly DependencyProperty ProgressStateProperty = DependencyProperty.Register("ProgressState", typeof(TaskbarItemProgressState), typeof(TaskbarItemInfo), new PropertyMetadata(TaskbarItemProgressState.None, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d)._OnProgressStateChanged();
		}, (DependencyObject d, object e) => TaskbarItemInfo._CoerceProgressState((TaskbarItemProgressState)e)));

		// Token: 0x040005C9 RID: 1481
		public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register("ProgressValue", typeof(double), typeof(TaskbarItemInfo), new PropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d)._OnProgressValueChanged();
		}, (DependencyObject d, object e) => TaskbarItemInfo._CoerceProgressValue((double)e)));

		// Token: 0x040005CA RID: 1482
		public static readonly DependencyProperty OverlayProperty = DependencyProperty.Register("Overlay", typeof(ImageSource), typeof(TaskbarItemInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d)._OnOverlayChanged();
		}));

		// Token: 0x040005CB RID: 1483
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(TaskbarItemInfo), new PropertyMetadata(string.Empty, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d)._OnDescriptionChanged();
		}));

		// Token: 0x040005CC RID: 1484
		public static readonly DependencyProperty ThumbnailClipMarginProperty = DependencyProperty.Register("ThumbnailClipMargin", typeof(Thickness), typeof(TaskbarItemInfo), new PropertyMetadata(default(Thickness), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d)._OnThumbnailClipMarginChanged();
		}, (DependencyObject d, object e) => TaskbarItemInfo._CoerceThumbnailClipMargin((Thickness)e)));

		// Token: 0x040005CD RID: 1485
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
		public static readonly DependencyProperty ThumbButtonInfosProperty = DependencyProperty.Register("ThumbButtonInfos", typeof(ThumbButtonInfoCollection), typeof(TaskbarItemInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d)._OnThumbButtonsChanged();
		}));
	}
}
