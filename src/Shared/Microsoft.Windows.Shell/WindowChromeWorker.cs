using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Standard;

namespace Microsoft.Windows.Shell
{
	// Token: 0x020000A6 RID: 166
	internal class WindowChromeWorker : DependencyObject
	{
		// Token: 0x0600032F RID: 815 RVA: 0x00009088 File Offset: 0x00007288
		public WindowChromeWorker()
		{
			this._messageTable = new List<KeyValuePair<WM, MessageHandler>>
			{
				new KeyValuePair<WM, MessageHandler>(WM.SETTEXT, new MessageHandler(this._HandleSetTextOrIcon)),
				new KeyValuePair<WM, MessageHandler>(WM.SETICON, new MessageHandler(this._HandleSetTextOrIcon)),
				new KeyValuePair<WM, MessageHandler>(WM.NCACTIVATE, new MessageHandler(this._HandleNCActivate)),
				new KeyValuePair<WM, MessageHandler>(WM.NCCALCSIZE, new MessageHandler(this._HandleNCCalcSize)),
				new KeyValuePair<WM, MessageHandler>(WM.NCHITTEST, new MessageHandler(this._HandleNCHitTest)),
				new KeyValuePair<WM, MessageHandler>(WM.NCRBUTTONUP, new MessageHandler(this._HandleNCRButtonUp)),
				new KeyValuePair<WM, MessageHandler>(WM.SIZE, new MessageHandler(this._HandleSize)),
				new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGED, new MessageHandler(this._HandleWindowPosChanged)),
				new KeyValuePair<WM, MessageHandler>(WM.DWMCOMPOSITIONCHANGED, new MessageHandler(this._HandleDwmCompositionChanged))
			};
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				this._messageTable.AddRange(new KeyValuePair<WM, MessageHandler>[]
				{
					new KeyValuePair<WM, MessageHandler>(WM.WININICHANGE, new MessageHandler(this._HandleSettingChange)),
					new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, new MessageHandler(this._HandleEnterSizeMove)),
					new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, new MessageHandler(this._HandleExitSizeMove)),
					new KeyValuePair<WM, MessageHandler>(WM.MOVE, new MessageHandler(this._HandleMove))
				});
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00009244 File Offset: 0x00007444
		public void SetWindowChrome(WindowChrome newChrome)
		{
			base.VerifyAccess();
			if (newChrome == this._chromeInfo)
			{
				return;
			}
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint -= this._OnChromePropertyChangedThatRequiresRepaint;
			}
			this._chromeInfo = newChrome;
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint += this._OnChromePropertyChangedThatRequiresRepaint;
			}
			this._ApplyNewCustomChrome();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000092AC File Offset: 0x000074AC
		private void _OnChromePropertyChangedThatRequiresRepaint(object sender, EventArgs e)
		{
			this._UpdateFrameState(true);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x000092B8 File Offset: 0x000074B8
		private static void _OnChromeWorkerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			WindowChromeWorker windowChromeWorker = (WindowChromeWorker)e.NewValue;
			windowChromeWorker._SetWindow(window);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00009318 File Offset: 0x00007518
		private void _SetWindow(Window window)
		{
			this._window = window;
			this._hwnd = new WindowInteropHelper(this._window).Handle;
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				Utility.AddDependencyPropertyChangeListener(this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
				Utility.AddDependencyPropertyChangeListener(this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
			}
			this._window.Closed += this._UnsetWindow;
			if (IntPtr.Zero != this._hwnd)
			{
				this._hwndSource = HwndSource.FromHwnd(this._hwnd);
				this._window.ApplyTemplate();
				if (this._chromeInfo != null)
				{
					this._ApplyNewCustomChrome();
					return;
				}
			}
			else
			{
				this._window.SourceInitialized += delegate(object sender, EventArgs e)
				{
					this._hwnd = new WindowInteropHelper(this._window).Handle;
					this._hwndSource = HwndSource.FromHwnd(this._hwnd);
					if (this._chromeInfo != null)
					{
						this._ApplyNewCustomChrome();
					}
				};
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x000093F4 File Offset: 0x000075F4
		private void _UnsetWindow(object sender, EventArgs e)
		{
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				Utility.RemoveDependencyPropertyChangeListener(this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
				Utility.RemoveDependencyPropertyChangeListener(this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
			}
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint -= this._OnChromePropertyChangedThatRequiresRepaint;
			}
			this._RestoreStandardChromeState(true);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00009466 File Offset: 0x00007666
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		public static WindowChromeWorker GetWindowChromeWorker(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			return (WindowChromeWorker)window.GetValue(WindowChromeWorker.WindowChromeWorkerProperty);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00009483 File Offset: 0x00007683
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		public static void SetWindowChromeWorker(Window window, WindowChromeWorker chrome)
		{
			Verify.IsNotNull<Window>(window, "window");
			window.SetValue(WindowChromeWorker.WindowChromeWorkerProperty, chrome);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000949C File Offset: 0x0000769C
		private void _OnWindowPropertyChangedThatRequiresTemplateFixup(object sender, EventArgs e)
		{
			if (this._chromeInfo != null && this._hwnd != IntPtr.Zero)
			{
				this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new WindowChromeWorker._Action(this._FixupFrameworkIssues));
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000094D8 File Offset: 0x000076D8
		private void _ApplyNewCustomChrome()
		{
			if (this._hwnd == IntPtr.Zero)
			{
				return;
			}
			if (this._chromeInfo == null)
			{
				this._RestoreStandardChromeState(false);
				return;
			}
			if (!this._isHooked)
			{
				this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
				this._isHooked = true;
			}
			this._FixupFrameworkIssues();
			this._UpdateSystemMenu(new WindowState?(this._window.WindowState));
			this._UpdateFrameState(true);
			NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000956C File Offset: 0x0000776C
		private void _FixupFrameworkIssues()
		{
			if (!Utility.IsPresentationFrameworkVersionLessThan4)
			{
				return;
			}
			if (this._window.Template == null)
			{
				return;
			}
			if (VisualTreeHelper.GetChildrenCount(this._window) == 0)
			{
				this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new WindowChromeWorker._Action(this._FixupFrameworkIssues));
				return;
			}
			FrameworkElement frameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(this._window, 0);
			RECT windowRect = NativeMethods.GetWindowRect(this._hwnd);
			RECT rect = this._GetAdjustedWindowRect(windowRect);
			Rect rect2 = DpiHelper.DeviceRectToLogical(new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height));
			Rect rect3 = DpiHelper.DeviceRectToLogical(new Rect((double)rect.Left, (double)rect.Top, (double)rect.Width, (double)rect.Height));
			Thickness thickness = new Thickness(rect2.Left - rect3.Left, rect2.Top - rect3.Top, rect3.Right - rect2.Right, rect3.Bottom - rect2.Bottom);
			frameworkElement.Margin = new Thickness(0.0, 0.0, -(thickness.Left + thickness.Right), -(thickness.Top + thickness.Bottom));
			if (this._window.FlowDirection == FlowDirection.RightToLeft)
			{
				frameworkElement.RenderTransform = new MatrixTransform(1.0, 0.0, 0.0, 1.0, -(thickness.Left + thickness.Right), 0.0);
			}
			else
			{
				frameworkElement.RenderTransform = null;
			}
			if (!this._isFixedUp)
			{
				this._hasUserMovedWindow = false;
				this._window.StateChanged += this._FixupRestoreBounds;
				this._isFixedUp = true;
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00009744 File Offset: 0x00007944
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private void _FixupWindows7Issues()
		{
			if (this._blackGlassFixupAttemptCount > 5)
			{
				return;
			}
			if (Utility.IsOSWindows7OrNewer && NativeMethods.DwmIsCompositionEnabled())
			{
				this._blackGlassFixupAttemptCount++;
				bool flag = false;
				try
				{
					flag = (NativeMethods.DwmGetCompositionTimingInfo(this._hwnd) != null);
				}
				catch (Exception)
				{
				}
				if (!flag)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new WindowChromeWorker._Action(this._FixupWindows7Issues));
					return;
				}
				this._blackGlassFixupAttemptCount = 0;
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000097C8 File Offset: 0x000079C8
		private void _FixupRestoreBounds(object sender, EventArgs e)
		{
			if ((this._window.WindowState == WindowState.Maximized || this._window.WindowState == WindowState.Minimized) && this._hasUserMovedWindow)
			{
				this._hasUserMovedWindow = false;
				WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(this._hwnd);
				RECT rect = this._GetAdjustedWindowRect(new RECT
				{
					Bottom = 100,
					Right = 100
				});
				Point point = DpiHelper.DevicePixelsToLogical(new Point((double)(windowPlacement.rcNormalPosition.Left - rect.Left), (double)(windowPlacement.rcNormalPosition.Top - rect.Top)));
				this._window.Top = point.Y;
				this._window.Left = point.X;
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000988C File Offset: 0x00007A8C
		private RECT _GetAdjustedWindowRect(RECT rcWindow)
		{
			WS dwStyle = (WS)((int)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE));
			WS_EX dwExStyle = (WS_EX)((int)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.EXSTYLE));
			return NativeMethods.AdjustWindowRectEx(rcWindow, dwStyle, false, dwExStyle);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600033D RID: 829 RVA: 0x000098C8 File Offset: 0x00007AC8
		private bool _IsWindowDocked
		{
			get
			{
				if (this._window.WindowState != WindowState.Normal)
				{
					return false;
				}
				RECT rect = this._GetAdjustedWindowRect(new RECT
				{
					Bottom = 100,
					Right = 100
				});
				Point point = new Point(this._window.Left, this._window.Top);
				point -= (Vector)DpiHelper.DevicePixelsToLogical(new Point((double)rect.Left, (double)rect.Top));
				return this._window.RestoreBounds.Location != point;
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00009964 File Offset: 0x00007B64
		private IntPtr _WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			foreach (KeyValuePair<WM, MessageHandler> keyValuePair in this._messageTable)
			{
				if (keyValuePair.Key == (WM)msg)
				{
					return keyValuePair.Value((WM)msg, wParam, lParam, out handled);
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000099D8 File Offset: 0x00007BD8
		private IntPtr _HandleSetTextOrIcon(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
			IntPtr result = NativeMethods.DefWindowProc(this._hwnd, uMsg, wParam, lParam);
			if (flag)
			{
				this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
			}
			handled = true;
			return result;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00009A18 File Offset: 0x00007C18
		private IntPtr _HandleNCActivate(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			IntPtr result = NativeMethods.DefWindowProc(this._hwnd, WM.NCACTIVATE, wParam, new IntPtr(-1));
			handled = true;
			return result;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00009A42 File Offset: 0x00007C42
		private IntPtr _HandleNCCalcSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			handled = true;
			return new IntPtr(768);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00009A54 File Offset: 0x00007C54
		private IntPtr _HandleNCHitTest(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			IntPtr intPtr = IntPtr.Zero;
			handled = false;
			if (Utility.IsOSVistaOrNewer && this._chromeInfo.GlassFrameThickness != default(Thickness) && this._isGlassEnabled)
			{
				handled = NativeMethods.DwmDefWindowProc(this._hwnd, uMsg, wParam, lParam, out intPtr);
			}
			if (IntPtr.Zero == intPtr)
			{
				Point point = new Point((double)Utility.GET_X_LPARAM(lParam), (double)Utility.GET_Y_LPARAM(lParam));
				Rect deviceRectangle = this._GetWindowRect();
				HT ht = this._HitTestNca(DpiHelper.DeviceRectToLogical(deviceRectangle), DpiHelper.DevicePixelsToLogical(point));
				if (ht != HT.CLIENT)
				{
					Point point2 = point;
					point2.Offset(-deviceRectangle.X, -deviceRectangle.Y);
					point2 = DpiHelper.DevicePixelsToLogical(point2);
					IInputElement inputElement = this._window.InputHitTest(point2);
					if (inputElement != null && WindowChrome.GetIsHitTestVisibleInChrome(inputElement))
					{
						ht = HT.CLIENT;
					}
				}
				handled = true;
				intPtr = new IntPtr((int)ht);
			}
			return intPtr;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00009B3F File Offset: 0x00007D3F
		private IntPtr _HandleNCRButtonUp(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (2 == wParam.ToInt32())
			{
				SystemCommands.ShowSystemMenuPhysicalCoordinates(this._window, new Point((double)Utility.GET_X_LPARAM(lParam), (double)Utility.GET_Y_LPARAM(lParam)));
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00009B74 File Offset: 0x00007D74
		private IntPtr _HandleSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			WindowState? assumeState = null;
			if (wParam.ToInt32() == 2)
			{
				assumeState = new WindowState?(WindowState.Maximized);
			}
			this._UpdateSystemMenu(assumeState);
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00009BAC File Offset: 0x00007DAC
		private IntPtr _HandleWindowPosChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._UpdateSystemMenu(null);
			if (!this._isGlassEnabled)
			{
				WINDOWPOS value = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
				this._SetRoundingRegion(new WINDOWPOS?(value));
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00009BFB File Offset: 0x00007DFB
		private IntPtr _HandleDwmCompositionChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._UpdateFrameState(false);
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00009C0D File Offset: 0x00007E0D
		private IntPtr _HandleSettingChange(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._FixupFrameworkIssues();
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00009C20 File Offset: 0x00007E20
		private IntPtr _HandleEnterSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._isUserResizing = true;
			if (this._window.WindowState != WindowState.Maximized && !this._IsWindowDocked)
			{
				this._windowPosAtStartOfUserMove = new Point(this._window.Left, this._window.Top);
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00009C74 File Offset: 0x00007E74
		private IntPtr _HandleExitSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._isUserResizing = false;
			if (this._window.WindowState == WindowState.Maximized)
			{
				this._window.Top = this._windowPosAtStartOfUserMove.Y;
				this._window.Left = this._windowPosAtStartOfUserMove.X;
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00009CCB File Offset: 0x00007ECB
		private IntPtr _HandleMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._isUserResizing)
			{
				this._hasUserMovedWindow = true;
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00009CE8 File Offset: 0x00007EE8
		private bool _ModifyStyle(WS removeStyle, WS addStyle)
		{
			WS ws = (WS)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt32();
			WS ws2 = (ws & ~removeStyle) | addStyle;
			if (ws == ws2)
			{
				return false;
			}
			NativeMethods.SetWindowLongPtr(this._hwnd, GWL.STYLE, new IntPtr((int)ws2));
			return true;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00009D30 File Offset: 0x00007F30
		private WindowState _GetHwndState()
		{
			WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(this._hwnd);
			switch (windowPlacement.showCmd)
			{
			case SW.SHOWMINIMIZED:
				return WindowState.Minimized;
			case SW.SHOWMAXIMIZED:
				return WindowState.Maximized;
			default:
				return WindowState.Normal;
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00009D68 File Offset: 0x00007F68
		private Rect _GetWindowRect()
		{
			RECT windowRect = NativeMethods.GetWindowRect(this._hwnd);
			return new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00009DA8 File Offset: 0x00007FA8
		private void _UpdateSystemMenu(WindowState? assumeState)
		{
			WindowState windowState = assumeState ?? this._GetHwndState();
			if (assumeState != null || this._lastMenuState != windowState)
			{
				this._lastMenuState = windowState;
				bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
				IntPtr systemMenu = NativeMethods.GetSystemMenu(this._hwnd, false);
				if (IntPtr.Zero != systemMenu)
				{
					WS value = (WS)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt32();
					bool flag2 = Utility.IsFlagSet((int)value, 131072);
					bool flag3 = Utility.IsFlagSet((int)value, 65536);
					bool flag4 = Utility.IsFlagSet((int)value, 262144);
					switch (windowState)
					{
					case WindowState.Minimized:
						NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag3 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
						break;
					case WindowState.Maximized:
						NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag2 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
						NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, MF.GRAYED | MF.DISABLED);
						break;
					default:
						NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, flag4 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
						NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag2 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
						NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag3 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
						break;
					}
				}
				if (flag)
				{
					this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
				}
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00009F6C File Offset: 0x0000816C
		private void _UpdateFrameState(bool force)
		{
			if (IntPtr.Zero == this._hwnd)
			{
				return;
			}
			bool flag = NativeMethods.DwmIsCompositionEnabled();
			if (force || flag != this._isGlassEnabled)
			{
				this._isGlassEnabled = (flag && this._chromeInfo.GlassFrameThickness != default(Thickness));
				if (!this._isGlassEnabled)
				{
					this._SetRoundingRegion(null);
				}
				else
				{
					this._ClearRoundingRegion();
					this._ExtendGlassFrame();
					this._FixupWindows7Issues();
				}
				NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000A008 File Offset: 0x00008208
		private void _ClearRoundingRegion()
		{
			NativeMethods.SetWindowRgn(this._hwnd, IntPtr.Zero, NativeMethods.IsWindowVisible(this._hwnd));
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000A028 File Offset: 0x00008228
		private void _SetRoundingRegion(WINDOWPOS? wp)
		{
			WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(this._hwnd);
			if (windowPlacement.showCmd == SW.SHOWMAXIMIZED)
			{
				int num;
				int num2;
				if (wp != null)
				{
					num = wp.Value.x;
					num2 = wp.Value.y;
				}
				else
				{
					Rect rect = this._GetWindowRect();
					num = (int)rect.Left;
					num2 = (int)rect.Top;
				}
				IntPtr hMonitor = NativeMethods.MonitorFromWindow(this._hwnd, 2U);
				MONITORINFO monitorInfo = NativeMethods.GetMonitorInfo(hMonitor);
				RECT rcWork = monitorInfo.rcWork;
				rcWork.Offset(-num, -num2);
				IntPtr hRgn = IntPtr.Zero;
				try
				{
					hRgn = NativeMethods.CreateRectRgnIndirect(rcWork);
					NativeMethods.SetWindowRgn(this._hwnd, hRgn, NativeMethods.IsWindowVisible(this._hwnd));
					hRgn = IntPtr.Zero;
					return;
				}
				finally
				{
					Utility.SafeDeleteObject(ref hRgn);
				}
			}
			Size size;
			if (wp != null && !Utility.IsFlagSet(wp.Value.flags, 1))
			{
				size = new Size((double)wp.Value.cx, (double)wp.Value.cy);
			}
			else
			{
				if (wp != null && this._lastRoundingState == this._window.WindowState)
				{
					return;
				}
				size = this._GetWindowRect().Size;
			}
			this._lastRoundingState = this._window.WindowState;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				double num3 = Math.Min(size.Width, size.Height);
				double num4 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.TopLeft, 0.0)).X;
				num4 = Math.Min(num4, num3 / 2.0);
				if (WindowChromeWorker._IsUniform(this._chromeInfo.CornerRadius))
				{
					intPtr = WindowChromeWorker._CreateRoundRectRgn(new Rect(size), num4);
				}
				else
				{
					intPtr = WindowChromeWorker._CreateRoundRectRgn(new Rect(0.0, 0.0, size.Width / 2.0 + num4, size.Height / 2.0 + num4), num4);
					double num5 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.TopRight, 0.0)).X;
					num5 = Math.Min(num5, num3 / 2.0);
					Rect region = new Rect(0.0, 0.0, size.Width / 2.0 + num5, size.Height / 2.0 + num5);
					region.Offset(size.Width / 2.0 - num5, 0.0);
					WindowChromeWorker._CreateAndCombineRoundRectRgn(intPtr, region, num5);
					double num6 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.BottomLeft, 0.0)).X;
					num6 = Math.Min(num6, num3 / 2.0);
					Rect region2 = new Rect(0.0, 0.0, size.Width / 2.0 + num6, size.Height / 2.0 + num6);
					region2.Offset(0.0, size.Height / 2.0 - num6);
					WindowChromeWorker._CreateAndCombineRoundRectRgn(intPtr, region2, num6);
					double num7 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.BottomRight, 0.0)).X;
					num7 = Math.Min(num7, num3 / 2.0);
					Rect region3 = new Rect(0.0, 0.0, size.Width / 2.0 + num7, size.Height / 2.0 + num7);
					region3.Offset(size.Width / 2.0 - num7, size.Height / 2.0 - num7);
					WindowChromeWorker._CreateAndCombineRoundRectRgn(intPtr, region3, num7);
				}
				NativeMethods.SetWindowRgn(this._hwnd, intPtr, NativeMethods.IsWindowVisible(this._hwnd));
				intPtr = IntPtr.Zero;
			}
			finally
			{
				Utility.SafeDeleteObject(ref intPtr);
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000A4D0 File Offset: 0x000086D0
		private static IntPtr _CreateRoundRectRgn(Rect region, double radius)
		{
			if (DoubleUtilities.AreClose(0.0, radius))
			{
				return NativeMethods.CreateRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right), (int)Math.Ceiling(region.Bottom));
			}
			return NativeMethods.CreateRoundRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right) + 1, (int)Math.Ceiling(region.Bottom) + 1, (int)Math.Ceiling(radius), (int)Math.Ceiling(radius));
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000A574 File Offset: 0x00008774
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "HRGNs")]
		private static void _CreateAndCombineRoundRectRgn(IntPtr hrgnSource, Rect region, double radius)
		{
			IntPtr hrgnSrc = IntPtr.Zero;
			try
			{
				hrgnSrc = WindowChromeWorker._CreateRoundRectRgn(region, radius);
				if (NativeMethods.CombineRgn(hrgnSource, hrgnSource, hrgnSrc, RGN.OR) == CombineRgnResult.ERROR)
				{
					throw new InvalidOperationException("Unable to combine two HRGNs.");
				}
			}
			catch
			{
				Utility.SafeDeleteObject(ref hrgnSrc);
				throw;
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000A5C4 File Offset: 0x000087C4
		private static bool _IsUniform(CornerRadius cornerRadius)
		{
			return DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.BottomRight) && DoubleUtilities.AreClose(cornerRadius.TopLeft, cornerRadius.TopRight) && DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.TopRight);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000A618 File Offset: 0x00008818
		private void _ExtendGlassFrame()
		{
			if (!Utility.IsOSVistaOrNewer)
			{
				return;
			}
			if (IntPtr.Zero == this._hwnd)
			{
				return;
			}
			if (!NativeMethods.DwmIsCompositionEnabled())
			{
				this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
				return;
			}
			this._hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.GlassFrameThickness.Left, this._chromeInfo.GlassFrameThickness.Top));
			Point point2 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.GlassFrameThickness.Right, this._chromeInfo.GlassFrameThickness.Bottom));
			MARGINS margins = new MARGINS
			{
				cxLeftWidth = (int)Math.Ceiling(point.X),
				cxRightWidth = (int)Math.Ceiling(point2.X),
				cyTopHeight = (int)Math.Ceiling(point.Y),
				cyBottomHeight = (int)Math.Ceiling(point2.Y)
			};
			NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref margins);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000A740 File Offset: 0x00008940
		private HT _HitTestNca(Rect windowPosition, Point mousePosition)
		{
			int num = 1;
			int num2 = 1;
			bool flag = false;
			if (mousePosition.Y >= windowPosition.Top && mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top + this._chromeInfo.CaptionHeight)
			{
				flag = (mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top);
				num = 0;
			}
			else if (mousePosition.Y < windowPosition.Bottom && mousePosition.Y >= windowPosition.Bottom - (double)((int)this._chromeInfo.ResizeBorderThickness.Bottom))
			{
				num = 2;
			}
			if (mousePosition.X >= windowPosition.Left && mousePosition.X < windowPosition.Left + (double)((int)this._chromeInfo.ResizeBorderThickness.Left))
			{
				num2 = 0;
			}
			else if (mousePosition.X < windowPosition.Right && mousePosition.X >= windowPosition.Right - this._chromeInfo.ResizeBorderThickness.Right)
			{
				num2 = 2;
			}
			if (num == 0 && num2 != 1 && !flag)
			{
				num = 1;
			}
			HT ht = WindowChromeWorker._HitTestBorders[num, num2];
			if (ht == HT.TOP && !flag)
			{
				ht = HT.CAPTION;
			}
			return ht;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000A88D File Offset: 0x00008A8D
		private void _RestoreStandardChromeState(bool isClosing)
		{
			base.VerifyAccess();
			this._UnhookCustomChrome();
			if (!isClosing)
			{
				this._RestoreFrameworkIssueFixups();
				this._RestoreGlassFrame();
				this._RestoreHrgn();
				this._window.InvalidateMeasure();
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000A8BB File Offset: 0x00008ABB
		private void _UnhookCustomChrome()
		{
			if (this._isHooked)
			{
				this._hwndSource.RemoveHook(new HwndSourceHook(this._WndProc));
				this._isHooked = false;
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000A8E4 File Offset: 0x00008AE4
		private void _RestoreFrameworkIssueFixups()
		{
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				FrameworkElement frameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(this._window, 0);
				frameworkElement.Margin = default(Thickness);
				this._window.StateChanged -= this._FixupRestoreBounds;
				this._isFixedUp = false;
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000A938 File Offset: 0x00008B38
		private void _RestoreGlassFrame()
		{
			if (!Utility.IsOSVistaOrNewer || this._hwnd == IntPtr.Zero)
			{
				return;
			}
			this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
			if (NativeMethods.DwmIsCompositionEnabled())
			{
				MARGINS margins = default(MARGINS);
				NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref margins);
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000A990 File Offset: 0x00008B90
		private void _RestoreHrgn()
		{
			this._ClearRoundingRegion();
			NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		// Note: this type is marked as 'beforefieldinit'.
		static WindowChromeWorker()
		{
			HT[,] array = new HT[3, 3];
			array[0, 0] = HT.TOPLEFT;
			array[0, 1] = HT.TOP;
			array[0, 2] = HT.TOPRIGHT;
			array[1, 0] = HT.LEFT;
			array[1, 1] = HT.CLIENT;
			array[1, 2] = HT.RIGHT;
			array[2, 0] = HT.BOTTOMLEFT;
			array[2, 1] = HT.BOTTOM;
			array[2, 2] = HT.BOTTOMRIGHT;
			WindowChromeWorker._HitTestBorders = array;
		}

		// Token: 0x040005FB RID: 1531
		private const SWP _SwpFlags = SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER;

		// Token: 0x040005FC RID: 1532
		private readonly List<KeyValuePair<WM, MessageHandler>> _messageTable;

		// Token: 0x040005FD RID: 1533
		private Window _window;

		// Token: 0x040005FE RID: 1534
		private IntPtr _hwnd;

		// Token: 0x040005FF RID: 1535
		private HwndSource _hwndSource;

		// Token: 0x04000600 RID: 1536
		private bool _isHooked;

		// Token: 0x04000601 RID: 1537
		private bool _isFixedUp;

		// Token: 0x04000602 RID: 1538
		private bool _isUserResizing;

		// Token: 0x04000603 RID: 1539
		private bool _hasUserMovedWindow;

		// Token: 0x04000604 RID: 1540
		private Point _windowPosAtStartOfUserMove = default(Point);

		// Token: 0x04000605 RID: 1541
		private int _blackGlassFixupAttemptCount;

		// Token: 0x04000606 RID: 1542
		private WindowChrome _chromeInfo;

		// Token: 0x04000607 RID: 1543
		private WindowState _lastRoundingState;

		// Token: 0x04000608 RID: 1544
		private WindowState _lastMenuState;

		// Token: 0x04000609 RID: 1545
		private bool _isGlassEnabled;

		// Token: 0x0400060A RID: 1546
		public static readonly DependencyProperty WindowChromeWorkerProperty = DependencyProperty.RegisterAttached("WindowChromeWorker", typeof(WindowChromeWorker), typeof(WindowChromeWorker), new PropertyMetadata(null, new PropertyChangedCallback(WindowChromeWorker._OnChromeWorkerChanged)));

		// Token: 0x0400060B RID: 1547
		[SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member")]
		private static readonly HT[,] _HitTestBorders;

		// Token: 0x020000A7 RID: 167
		// (Invoke) Token: 0x0600035F RID: 863
		private delegate void _Action();
	}
}
