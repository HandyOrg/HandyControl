using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace Standard
{
	// Token: 0x02000015 RID: 21
	internal sealed class MessageWindow : DispatcherObject, IDisposable
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00003F54 File Offset: 0x00002154
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00003F5C File Offset: 0x0000215C
		public IntPtr Handle { get; private set; }

		// Token: 0x0600009B RID: 155 RVA: 0x00003F68 File Offset: 0x00002168
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		public MessageWindow(CS classStyle, WS style, WS_EX exStyle, Rect location, string name, WndProc callback)
		{
			this._wndProcCallback = callback;
			this._className = "MessageWindowClass+" + Guid.NewGuid().ToString();
			WNDCLASSEX wndclassex = new WNDCLASSEX
			{
				cbSize = Marshal.SizeOf(typeof(WNDCLASSEX)),
				style = classStyle,
				lpfnWndProc = MessageWindow.s_WndProc,
				hInstance = NativeMethods.GetModuleHandle(null),
				hbrBackground = NativeMethods.GetStockObject(StockObject.NULL_BRUSH),
				lpszMenuName = "",
				lpszClassName = this._className
			};
			NativeMethods.RegisterClassEx(ref wndclassex);
			GCHandle value = default(GCHandle);
			try
			{
				value = GCHandle.Alloc(this);
				IntPtr lpParam = (IntPtr)value;
				this.Handle = NativeMethods.CreateWindowEx(exStyle, this._className, name, style, (int)location.X, (int)location.Y, (int)location.Width, (int)location.Height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, lpParam);
			}
			finally
			{
				value.Free();
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000408C File Offset: 0x0000228C
		~MessageWindow()
		{
			this._Dispose(false, false);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000040BC File Offset: 0x000022BC
		public void Dispose()
		{
			this._Dispose(true, false);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000040FC File Offset: 0x000022FC
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "disposing")]
		private void _Dispose(bool disposing, bool isHwndBeingDestroyed)
		{
			if (this._isDisposed)
			{
				return;
			}
			this._isDisposed = true;
			IntPtr hwnd = this.Handle;
			string className = this._className;
			if (isHwndBeingDestroyed)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback((object arg) => MessageWindow._DestroyWindow(IntPtr.Zero, className)));
			}
			else if (this.Handle != IntPtr.Zero)
			{
				if (base.CheckAccess())
				{
					MessageWindow._DestroyWindow(hwnd, className);
				}
				else
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback((object arg) => MessageWindow._DestroyWindow(hwnd, className)));
				}
			}
			MessageWindow.s_windowLookup.Remove(hwnd);
			this._className = null;
			this.Handle = IntPtr.Zero;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000041D0 File Offset: 0x000023D0
		[SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly")]
		private static IntPtr _WndProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr result = IntPtr.Zero;
			MessageWindow messageWindow = null;
			if (msg == WM.CREATE)
			{
				messageWindow = (MessageWindow)GCHandle.FromIntPtr(((CREATESTRUCT)Marshal.PtrToStructure(lParam, typeof(CREATESTRUCT))).lpCreateParams).Target;
				MessageWindow.s_windowLookup.Add(hwnd, messageWindow);
			}
			else if (!MessageWindow.s_windowLookup.TryGetValue(hwnd, out messageWindow))
			{
				return NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
			}
			WndProc wndProcCallback = messageWindow._wndProcCallback;
			if (wndProcCallback != null)
			{
				result = wndProcCallback(hwnd, msg, wParam, lParam);
			}
			else
			{
				result = NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
			}
			if (msg == WM.NCDESTROY)
			{
				messageWindow._Dispose(true, true);
				GC.SuppressFinalize(messageWindow);
			}
			return result;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000427B File Offset: 0x0000247B
		private static object _DestroyWindow(IntPtr hwnd, string className)
		{
			Utility.SafeDestroyWindow(ref hwnd);
			NativeMethods.UnregisterClass(className, NativeMethods.GetModuleHandle(null));
			return null;
		}

		// Token: 0x04000077 RID: 119
		private static readonly WndProc s_WndProc = new WndProc(MessageWindow._WndProc);

		// Token: 0x04000078 RID: 120
		private static readonly Dictionary<IntPtr, MessageWindow> s_windowLookup = new Dictionary<IntPtr, MessageWindow>();

		// Token: 0x04000079 RID: 121
		private WndProc _wndProcCallback;

		// Token: 0x0400007A RID: 122
		private string _className;

		// Token: 0x0400007B RID: 123
		private bool _isDisposed;
	}
}
