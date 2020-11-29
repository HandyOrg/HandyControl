using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace HandyControl.Tools.Helper
{
    /// <summary>
    /// 使用触摸拖动窗口的辅助类
    /// </summary>
    /// <remarks>
    /// 细节请看 [WPF 多指触摸拖拽窗口 拖动修改窗口坐标](https://blog.lindexi.com/post/WPF-%E5%A4%9A%E6%8C%87%E8%A7%A6%E6%91%B8%E6%8B%96%E6%8B%BD%E7%AA%97%E5%8F%A3-%E6%8B%96%E5%8A%A8%E4%BF%AE%E6%94%B9%E7%AA%97%E5%8F%A3%E5%9D%90%E6%A0%87.html)
    /// </remarks>
    public static class TouchDragMoveWindowHelper
    {
        /// <summary>
        /// 开始使用触摸拖动窗口，在触摸抬起后自动结束
        /// </summary>
        /// <param name="window"></param>
        public static void DragMove(Window window)
        {
            var dragMoveMode = new DragMoveMode(window);
            dragMoveMode.Start();
        }

        class DragMoveMode
        {
            public DragMoveMode(Window window)
            {
                _window = window;
            }

            public void Start()
            {
                var window = _window;

                window.PreviewMouseMove += Window_PreviewMouseMove;
                window.PreviewMouseUp += Window_PreviewMouseUp;
                window.LostMouseCapture += Window_LostMouseCapture;
            }

            public void Stop()
            {
                Window window = _window;

                window.PreviewMouseMove -= Window_PreviewMouseMove;
                window.PreviewMouseUp -= Window_PreviewMouseUp;
                window.LostMouseCapture -= Window_LostMouseCapture;
            }

            private readonly Window _window;

            private Win32.User32.Point? _lastPoint;

            private void Window_LostMouseCapture(object sender, MouseEventArgs e)
            {
                Stop();
            }

            private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
            {
                Stop();
            }

            private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
            {
                Win32.User32.GetCursorPos(out var lpPoint);

                if (_lastPoint == null)
                {
                    _lastPoint = lpPoint;
                    _window.CaptureMouse();
                }

                var dx = lpPoint.X - _lastPoint.Value.X;
                var dy = lpPoint.Y - _lastPoint.Value.Y;

                Debug.WriteLine($"dx={dx} dy={dy}");

                // 以下的 60 是表示最大移动速度
                if (Math.Abs(dx) < 60 && Math.Abs(dy) < 60)
                {
                    var handle = new WindowInteropHelper(_window).Handle;

                    Win32.User32.GetWindowRect(handle, out var lpRect);

                    Win32.User32.SetWindowPos(handle, IntPtr.Zero, lpRect.Left + dx, lpRect.Top + dy, 0, 0,
                        (int)(Win32.User32.WindowPositionFlags.SWP_NOSIZE |
                               Win32.User32.WindowPositionFlags.SWP_NOZORDER));
                }

                _lastPoint = lpPoint;
            }
        }

        private static class Win32
        {
            public static class User32
            {
                /// <summary>
                /// 改变一个子窗口、弹出式窗口和顶层窗口的尺寸、位置和 Z 序。
                /// </summary>
                /// <param name="hWnd">窗口句柄。</param>
                /// <param name="hWndInsertAfter">
                /// 在z序中的位于被置位的窗口前的窗口句柄。该参数必须为一个窗口句柄，或下列值之一：
                /// <para>HWND_BOTTOM：将窗口置于 Z 序的底部。如果参数hWnd标识了一个顶层窗口，则窗口失去顶级位置，并且被置在其他窗口的底部。</para>
                /// <para>HWND_NOTOPMOST：将窗口置于所有非顶层窗口之上（即在所有顶层窗口之后）。如果窗口已经是非顶层窗口则该标志不起作用。</para>
                /// <para>HWND_TOP：将窗口置于Z序的顶部。</para>
                /// <para>HWND_TOPMOST：将窗口置于所有非顶层窗口之上。即使窗口未被激活窗口也将保持顶级位置。</para>
                /// </param>
                /// <param name="x">以客户坐标指定窗口新位置的左边界。</param>
                /// <param name="y">以客户坐标指定窗口新位置的顶边界。</param>
                /// <param name="cx">以像素指定窗口的新的宽度。</param>
                /// <param name="cy">以像素指定窗口的新的高度。</param>
                /// <param name="wFlagslong">
                /// 窗口尺寸和定位的标志。该参数可以是下列值的组合：
                /// <para>SWP_ASYNCWINDOWPOS：如果调用进程不拥有窗口，系统会向拥有窗口的线程发出需求。这就防止调用线程在其他线程处理需求的时候发生死锁。</para>
                /// <para>SWP_DEFERERASE：防止产生 WM_SYNCPAINT 消息。</para>
                /// <para>SWP_DRAWFRAME：在窗口周围画一个边框（定义在窗口类描述中）。</para>
                /// <para>SWP_FRAMECHANGED：给窗口发送 WM_NCCALCSIZE 消息，即使窗口尺寸没有改变也会发送该消息。如果未指定这个标志，只有在改变了窗口尺寸时才发送 WM_NCCALCSIZE。</para>
                /// <para>SWP_HIDEWINDOW：隐藏窗口。</para>
                /// <para>SWP_NOACTIVATE：不激活窗口。如果未设置标志，则窗口被激活，并被设置到其他最高级窗口或非最高级组的顶部（根据参数hWndlnsertAfter设置）。</para>
                /// <para>SWP_NOCOPYBITS：清除客户区的所有内容。如果未设置该标志，客户区的有效内容被保存并且在窗口尺寸更新和重定位后拷贝回客户区。</para>
                /// <para>SWP_NOMOVE：维持当前位置（忽略X和Y参数）。</para>
                /// <para>SWP_NOOWNERZORDER：不改变 Z 序中的所有者窗口的位置。</para>
                /// <para>SWP_NOREDRAW：不重画改变的内容。如果设置了这个标志，则不发生任何重画动作。适用于客户区和非客户区（包括标题栏和滚动条）和任何由于窗回移动而露出的父窗口的所有部分。如果设置了这个标志，应用程序必须明确地使窗口无效并区重画窗口的任何部分和父窗口需要重画的部分。</para>
                /// <para>SWP_NOREPOSITION：与 SWP_NOOWNERZORDER 标志相同。</para>
                /// <para>SWP_NOSENDCHANGING：防止窗口接收 WM_WINDOWPOSCHANGING 消息。</para>
                /// <para>SWP_NOSIZE：维持当前尺寸（忽略 cx 和 cy 参数）。</para>
                /// <para>SWP_NOZORDER：维持当前 Z 序（忽略 hWndlnsertAfter 参数）。</para>
                /// <para>SWP_SHOWWINDOW：显示窗口。</para>
                /// </param>
                /// <returns>如果函数成功，返回值为非零；如果函数失败，返回值为零。若想获得更多错误消息，请调用 GetLastError 函数。</returns>
                [DllImport(LibraryName, ExactSpelling = true, SetLastError = true)]
                public static extern Int32 SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, Int32 x, Int32 y, Int32 cx,
                    Int32 cy, Int32 wFlagslong);

                [Flags]
                public enum WindowPositionFlags
                {
                    /// <summary>
                    ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts
                    ///     the request to the thread that owns the window. This prevents the calling thread from blocking its execution while
                    ///     other threads process the request.
                    /// </summary>
                    SWP_ASYNCWINDOWPOS = 0x4000,

                    /// <summary>
                    ///     Prevents generation of the WM_SYNCPAINT message.
                    /// </summary>
                    SWP_DEFERERASE = 0x2000,

                    /// <summary>
                    ///     Draws a frame (defined in the window's class description) around the window.
                    /// </summary>
                    SWP_DRAWFRAME = 0x0020,

                    /// <summary>
                    ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if
                    ///     the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's
                    ///     size is being changed.
                    /// </summary>
                    SWP_FRAMECHANGED = 0x0020,

                    /// <summary>
                    ///     Hides the window.
                    /// </summary>
                    SWP_HIDEWINDOW = 0x0080,

                    /// <summary>
                    ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the
                    ///     topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
                    /// </summary>
                    SWP_NOACTIVATE = 0x0010,

                    /// <summary>
                    ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client
                    ///     area are saved and copied back into the client area after the window is sized or repositioned.
                    /// </summary>
                    SWP_NOCOPYBITS = 0x0100,

                    /// <summary>
                    ///     Retains the current position (ignores X and Y parameters).
                    /// </summary>
                    SWP_NOMOVE = 0x0002,

                    /// <summary>
                    ///     Does not change the owner window's position in the Z order.
                    /// </summary>
                    SWP_NOOWNERZORDER = 0x0200,

                    /// <summary>
                    ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area,
                    ///     the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a
                    ///     result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any
                    ///     parts of the window and parent window that need redrawing.
                    /// </summary>
                    SWP_NOREDRAW = 0x0008,

                    /// <summary>
                    ///     Same as the SWP_NOOWNERZORDER flag.
                    /// </summary>
                    SWP_NOREPOSITION = 0x0200,

                    /// <summary>
                    ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
                    /// </summary>
                    SWP_NOSENDCHANGING = 0x0400,

                    /// <summary>
                    ///     Retains the current size (ignores the cx and cy parameters).
                    /// </summary>
                    SWP_NOSIZE = 0x0001,

                    /// <summary>
                    ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
                    /// </summary>
                    SWP_NOZORDER = 0x0004,

                    /// <summary>
                    ///     Displays the window.
                    /// </summary>
                    SWP_SHOWWINDOW = 0x0040
                }


                public const string LibraryName = "user32";

                /// <summary>
                /// 获取的是以屏幕为坐标轴窗口坐标
                /// </summary>
                /// <param name="hWnd"></param>
                /// <param name="lpRect"></param>
                /// <returns></returns>
                [return: MarshalAs(UnmanagedType.Bool)]
                [DllImport(LibraryName, ExactSpelling = true)]
                public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

                /// <summary>
                /// 在 Win32 函数使用的矩形
                /// </summary>
                [StructLayout(LayoutKind.Sequential)]
                public partial struct Rectangle : IEquatable<Rectangle>
                {
                    /// <summary>
                    ///  创建在 Win32 函数使用的矩形
                    /// </summary>
                    /// <param name="left"></param>
                    /// <param name="top"></param>
                    /// <param name="right"></param>
                    /// <param name="bottom"></param>
                    public Rectangle(int left = 0, int top = 0, int right = 0, int bottom = 0)
                    {
                        Left = left;
                        Top = top;
                        Right = right;
                        Bottom = bottom;
                    }

                    /// <summary>
                    /// 创建在 Win32 函数使用的矩形
                    /// </summary>
                    /// <param name="width">矩形的宽度</param>
                    /// <param name="height">矩形的高度</param>
                    public Rectangle(int width = 0, int height = 0) : this(0, 0, width, height)
                    {
                    }

                    public int Left;
                    public int Top;
                    public int Right;
                    public int Bottom;

                    public bool Equals(Rectangle other)
                    {
                        return (Left == other.Left) && (Right == other.Right) && (Top == other.Top) &&
                               (Bottom == other.Bottom);
                    }

                    public override bool Equals(object obj)
                    {
                        return obj is Rectangle && Equals((Rectangle)obj);
                    }

                    public static bool operator ==(Rectangle left, Rectangle right)
                    {
                        return left.Equals(right);
                    }

                    public static bool operator !=(Rectangle left, Rectangle right)
                    {
                        return !(left == right);
                    }

                    public override int GetHashCode()
                    {
                        unchecked
                        {
                            var hashCode = (int)Left;
                            hashCode = (hashCode * 397) ^ (int)Top;
                            hashCode = (hashCode * 397) ^ (int)Right;
                            hashCode = (hashCode * 397) ^ (int)Bottom;
                            return hashCode;
                        }
                    }

                    /// <summary>
                    /// 获取当前矩形是否空矩形
                    /// </summary>
                    public bool IsEmpty => this.Left == 0 && this.Top == 0 && this.Right == 0 && this.Bottom == 0;

                    /// <summary>
                    /// 矩形的宽度
                    /// </summary>
                    public int Width
                    {
                        get { return unchecked((int)(Right - Left)); }
                        set { Right = unchecked((int)(Left + value)); }
                    }

                    /// <summary>
                    /// 矩形的高度
                    /// </summary>
                    public int Height
                    {
                        get { return unchecked((int)(Bottom - Top)); }
                        set { Bottom = unchecked((int)(Top + value)); }
                    }

                    /// <summary>
                    /// 通过 x、y 坐标和宽度高度创建矩形
                    /// </summary>
                    /// <param name="x"></param>
                    /// <param name="y"></param>
                    /// <param name="width"></param>
                    /// <param name="height"></param>
                    /// <returns></returns>
                    public static Rectangle Create(int x, int y, int width, int height)
                    {
                        unchecked
                        {
                            return new Rectangle(x, y, (int)(width + x), (int)(height + y));
                        }
                    }

                    public override string ToString()
                    {
                        var culture = CultureInfo.CurrentCulture;
                        return
                            $"{{ Left = {Left.ToString(culture)}, Top = {Top.ToString(culture)} , Right = {Right.ToString(culture)}, Bottom = {Bottom.ToString(culture)} }}, {{ Width: {Width.ToString(culture)}, Height: {Height.ToString(culture)} }}";
                    }

                    public static Rectangle From(ref Rectangle lvalue, ref Rectangle rvalue,
                        Func<int, int, int> leftTopOperation,
                        Func<int, int, int> rightBottomOperation = null)
                    {
                        if (rightBottomOperation == null)
                            rightBottomOperation = leftTopOperation;
                        return new Rectangle(
                            leftTopOperation(lvalue.Left, rvalue.Left),
                            leftTopOperation(lvalue.Top, rvalue.Top),
                            rightBottomOperation(lvalue.Right, rvalue.Right),
                            rightBottomOperation(lvalue.Bottom, rvalue.Bottom)
                        );
                    }

                    public void Add(Rectangle value)
                    {
                        Add(ref this, ref value);
                    }

                    public void Subtract(Rectangle value)
                    {
                        Subtract(ref this, ref value);
                    }

                    public void Multiply(Rectangle value)
                    {
                        Multiply(ref this, ref value);
                    }

                    public void Divide(Rectangle value)
                    {
                        Divide(ref this, ref value);
                    }

                    public void Deflate(Rectangle value)
                    {
                        Deflate(ref this, ref value);
                    }

                    public void Inflate(Rectangle value)
                    {
                        Inflate(ref this, ref value);
                    }

                    public void Offset(int x, int y)
                    {
                        Offset(ref this, x, y);
                    }

                    public void OffsetTo(int x, int y)
                    {
                        OffsetTo(ref this, x, y);
                    }

                    public void Scale(int x, int y)
                    {
                        Scale(ref this, x, y);
                    }

                    public void ScaleTo(int x, int y)
                    {
                        ScaleTo(ref this, x, y);
                    }

                    public static void Add(ref Rectangle lvalue, ref Rectangle rvalue)
                    {
                        lvalue.Left += rvalue.Left;
                        lvalue.Top += rvalue.Top;
                        lvalue.Right += rvalue.Right;
                        lvalue.Bottom += rvalue.Bottom;
                    }

                    public static void Subtract(ref Rectangle lvalue, ref Rectangle rvalue)
                    {
                        lvalue.Left -= rvalue.Left;
                        lvalue.Top -= rvalue.Top;
                        lvalue.Right -= rvalue.Right;
                        lvalue.Bottom -= rvalue.Bottom;
                    }

                    public static void Multiply(ref Rectangle lvalue, ref Rectangle rvalue)
                    {
                        lvalue.Left *= rvalue.Left;
                        lvalue.Top *= rvalue.Top;
                        lvalue.Right *= rvalue.Right;
                        lvalue.Bottom *= rvalue.Bottom;
                    }

                    public static void Divide(ref Rectangle lvalue, ref Rectangle rvalue)
                    {
                        lvalue.Left /= rvalue.Left;
                        lvalue.Top /= rvalue.Top;
                        lvalue.Right /= rvalue.Right;
                        lvalue.Bottom /= rvalue.Bottom;
                    }

                    public static void Deflate(ref Rectangle target, ref Rectangle deflation)
                    {
                        target.Top += deflation.Top;
                        target.Left += deflation.Left;
                        target.Bottom -= deflation.Bottom;
                        target.Right -= deflation.Right;
                    }

                    public static void Inflate(ref Rectangle target, ref Rectangle inflation)
                    {
                        target.Top -= inflation.Top;
                        target.Left -= inflation.Left;
                        target.Bottom += inflation.Bottom;
                        target.Right += inflation.Right;
                    }

                    public static void Offset(ref Rectangle target, int x, int y)
                    {
                        target.Top += y;
                        target.Left += x;
                        target.Bottom += y;
                        target.Right += x;
                    }

                    public static void OffsetTo(ref Rectangle target, int x, int y)
                    {
                        var width = target.Width;
                        var height = target.Height;
                        target.Left = x;
                        target.Top = y;
                        target.Right = width;
                        target.Bottom = height;
                    }

                    public static void Scale(ref Rectangle target, int x, int y)
                    {
                        target.Top *= y;
                        target.Left *= x;
                        target.Bottom *= y;
                        target.Right *= x;
                    }

                    public static void ScaleTo(ref Rectangle target, int x, int y)
                    {
                        unchecked
                        {
                            x = (int)(target.Left / x);
                            y = (int)(target.Top / y);
                        }

                        Scale(ref target, x, y);
                    }
                }

                [DllImport(LibraryName, SetLastError = true)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool GetCursorPos(out Point lpPoint);

                [StructLayout(LayoutKind.Sequential)]
                public struct Point
                {
                    public int X;
                    public int Y;

                    public Point(int x, int y)
                    {
                        this.X = x;
                        this.Y = y;
                    }

                    public static implicit operator System.Drawing.Point(Point p)
                    {
                        return new System.Drawing.Point(p.X, p.Y);
                    }

                    public static implicit operator Point(System.Drawing.Point p)
                    {
                        return new Point(p.X, p.Y);
                    }
                }
            }
        }
    }
}
