using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls;

/// <summary>
/// 用来使窗口变得全屏的辅助类
/// 采用设置窗口位置和尺寸，确保盖住整个屏幕的方式来实现全屏
/// 目前已知需要满足的条件是：窗口盖住整个屏幕、窗口没有WS_THICKFRAME样式、窗口不能有标题栏且最大化
/// </summary>
internal static class FullScreenHelper
{
    /// <summary>
    /// 用于记录窗口全屏前位置的附加属性
    /// </summary>
    private static readonly DependencyProperty BeforeFullScreenWindowPlacementProperty =
        DependencyProperty.RegisterAttached("BeforeFullScreenWindowPlacement",
            typeof(InteropValues.WINDOWPLACEMENT?), typeof(FullScreenHelper));

    /// <summary>
    /// 用于记录窗口全屏前样式的附加属性
    /// </summary>
    private static readonly DependencyProperty BeforeFullScreenWindowStyleProperty =
        DependencyProperty.RegisterAttached("BeforeFullScreenWindowStyle",
            typeof(InteropValues.WindowStyles?), typeof(FullScreenHelper));

    /// <summary>
    /// 开始进入全屏模式
    /// 进入全屏模式后，窗口可通过 API 方式（也可以用 Win + Shift + Left/Right）移动，调整大小，但会根据目标矩形寻找显示器重新调整到全屏状态。
    /// 进入全屏后，不要修改样式等窗口属性，在退出时，会恢复到进入前的状态
    /// 进入全屏模式后会禁用 DWM 过渡动画
    /// </summary>
    public static void StartFullScreen(System.Windows.Window window)
    {
        if (window == null)
        {
            throw new ArgumentNullException(nameof(window), $"{nameof(window)} 不能为 null");
        }

        //确保不在全屏模式
        if (window.GetValue(BeforeFullScreenWindowPlacementProperty) == null &&
            window.GetValue(BeforeFullScreenWindowStyleProperty) == null)
        {
            var hwnd = new WindowInteropHelper(window).EnsureHandle();
            var hwndSource = HwndSource.FromHwnd(hwnd);

            //获取当前窗口的位置大小状态并保存
            var placement = InteropMethods.GetWindowPlacement(hwnd);
            window.SetValue(BeforeFullScreenWindowPlacementProperty, placement);

            //修改窗口样式
            var style = (InteropValues.WindowStyles) InteropMethods.GetWindowLongPtr(hwnd, InteropValues.GWL_STYLE);
            window.SetValue(BeforeFullScreenWindowStyleProperty, style);
            //将窗口恢复到还原模式，在有标题栏的情况下最大化模式下无法全屏,
            //这里采用还原，不修改标题栏的方式
            //在退出全屏时，窗口原有的状态会恢复
            //去掉WS_THICKFRAME，在有该样式的情况下不能全屏
            //去掉WS_MAXIMIZEBOX，禁用最大化，如果最大化会退出全屏
            //去掉WS_MAXIMIZE，使窗口变成还原状态，不使用ShowWindow(hwnd, ShowWindowCommands.SW_RESTORE)，避免看到窗口变成还原状态这一过程（也避免影响窗口的Visible状态）
            style &= ~(InteropValues.WindowStyles.WS_THICKFRAME | InteropValues.WindowStyles.WS_MAXIMIZEBOX | InteropValues.WindowStyles.WS_MAXIMIZE);
            InteropMethods.SetWindowLong(hwnd, InteropValues.GWL_STYLE, (IntPtr) style);

            //禁用 DWM 过渡动画 忽略返回值，若DWM关闭不做处理
            InteropMethods.DwmSetWindowAttribute(hwnd, InteropValues.DwmWindowAttribute.DWMWA_TRANSITIONS_FORCEDISABLED, 1,
                sizeof(int));

            //添加Hook，在窗口尺寸位置等要发生变化时，确保全屏
            hwndSource.AddHook(KeepFullScreenHook);

            if (InteropMethods.GetWindowRect(hwnd, out var rect))
            {
                //不能用 placement 的坐标，placement是工作区坐标，不是屏幕坐标。

                //使用窗口当前的矩形调用下设置窗口位置和尺寸的方法，让Hook来进行调整窗口位置和尺寸到全屏模式
                InteropMethods.SetWindowPos(hwnd, (IntPtr) InteropValues.HWND_TOP, rect.Left, rect.Top, rect.Width,
                    rect.Height, (int) InteropValues.WindowPositionFlags.SWP_NOZORDER);
            }
        }
    }

    /// <summary>
    /// 退出全屏模式
    /// 窗口会回到进入全屏模式时保存的状态
    /// 退出全屏模式后会重新启用 DWM 过渡动画
    /// </summary>
    public static void EndFullScreen(System.Windows.Window window)
    {
        if (window == null)
        {
            throw new ArgumentNullException(nameof(window), $"{nameof(window)} 不能为 null");
        }

        //确保在全屏模式并获取之前保存的状态
        if (window.GetValue(BeforeFullScreenWindowPlacementProperty) is InteropValues.WINDOWPLACEMENT placement
            && window.GetValue(BeforeFullScreenWindowStyleProperty) is InteropValues.WindowStyles style)
        {
            var hwnd = new WindowInteropHelper(window).Handle;

            if (hwnd == IntPtr.Zero)
            {
                // 句柄为 0 只有两种情况：
                //  1. 虽然窗口已进入全屏，但窗口已被关闭；
                //  2. 窗口初始化前，在还没有调用 StartFullScreen 的前提下就调用了此方法。
                // 所以，直接 return 就好。
                return;
            }


            var hwndSource = HwndSource.FromHwnd(hwnd);

            //去除hook
            hwndSource.RemoveHook(KeepFullScreenHook);

            //恢复保存的状态
            //不要改变Style里的WS_MAXIMIZE，否则会使窗口变成最大化状态，但是尺寸不对
            //也不要设置回Style里的WS_MINIMIZE,否则会导致窗口最小化按钮显示成还原按钮
            InteropMethods.SetWindowLong(hwnd, InteropValues.GWL_STYLE,
                (IntPtr) (style & ~(InteropValues.WindowStyles.WS_MAXIMIZE | InteropValues.WindowStyles.WS_MINIMIZE)));

            if ((style & InteropValues.WindowStyles.WS_MINIMIZE) != 0)
            {
                //如果窗口进入全屏前是最小化的，这里不让窗口恢复到之前的最小化状态，而是到还原的状态。
                //大多数情况下，都不期望在退出全屏的时候，恢复到最小化。
                placement.showCmd = InteropValues.SW.RESTORE;
            }

            if ((style & InteropValues.WindowStyles.WS_MAXIMIZE) != 0)
            {
                //提前调用 ShowWindow 使窗口恢复最大化，若通过 SetWindowPlacement 最大化会导致闪烁，只靠其恢复 RestoreBounds.
                InteropMethods.ShowWindow(hwnd, InteropValues.SW.MAXIMIZE);
            }

            InteropMethods.SetWindowPlacement(hwnd, ref placement);

            if ((style & InteropValues.WindowStyles.WS_MAXIMIZE) ==
                0) //如果窗口是最大化就不要修改WPF属性，否则会破坏RestoreBounds，且WPF窗口自身在最大化时，不会修改 Left Top Width Height 属性
            {
                if (InteropMethods.GetWindowRect(hwnd, out var rect))
                {
                    //不能用 placement 的坐标，placement是工作区坐标，不是屏幕坐标。

                    //确保窗口的 WPF 属性与 Win32 位置一致
                    var logicalPos =
                        hwndSource.CompositionTarget.TransformFromDevice.Transform(
                            new Point(rect.Left, rect.Top));
                    var logicalSize =
                        hwndSource.CompositionTarget.TransformFromDevice.Transform(
                            new Point(rect.Width, rect.Height));
                    window.Left = logicalPos.X;
                    window.Top = logicalPos.Y;
                    window.Width = logicalSize.X;
                    window.Height = logicalSize.Y;
                }
            }

            //重新启用 DWM 过渡动画 忽略返回值，若DWM关闭不做处理
            InteropMethods.DwmSetWindowAttribute(hwnd, InteropValues.DwmWindowAttribute.DWMWA_TRANSITIONS_FORCEDISABLED, 0,
                sizeof(int));

            //删除保存的状态
            window.ClearValue(BeforeFullScreenWindowPlacementProperty);
            window.ClearValue(BeforeFullScreenWindowStyleProperty);
        }
    }

    /// <summary>
    /// 确保窗口全屏的Hook
    /// 使用HandleProcessCorruptedStateExceptions，防止访问内存过程中因为一些致命异常导致程序崩溃
    /// </summary>
    [HandleProcessCorruptedStateExceptions]
    private static IntPtr KeepFullScreenHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        //处理WM_WINDOWPOSCHANGING消息
        const int WINDOWPOSCHANGING = 0x0046;
        if (msg == WINDOWPOSCHANGING)
        {
            try
            {
                //得到WINDOWPOS结构体
                var pos = (InteropValues.WindowPosition) Marshal.PtrToStructure(lParam, typeof(InteropValues.WindowPosition));

                if ((pos.Flags & InteropValues.WindowPositionFlags.SWP_NOMOVE) != 0 &&
                    (pos.Flags & InteropValues.WindowPositionFlags.SWP_NOSIZE) != 0)
                {
                    //既然你既不改变位置，也不改变尺寸，我就不管了...
                    return IntPtr.Zero;
                }

                if (InteropMethods.IsIconic(hwnd))
                {
                    // 如果在全屏期间最小化了窗口，那么忽略后续的位置调整。
                    // 否则按后续逻辑，会根据窗口在 -32000 的位置，计算出错误的目标位置，然后就跳到主屏了。
                    return IntPtr.Zero;
                }

                //获取窗口现在的矩形，下面用来参考计算目标矩形
                if (InteropMethods.GetWindowRect(hwnd, out var rect))
                {
                    var targetRect = rect; //窗口想要变化的目标矩形

                    if ((pos.Flags & InteropValues.WindowPositionFlags.SWP_NOMOVE) == 0)
                    {
                        //需要移动
                        targetRect.Left = pos.X;
                        targetRect.Top = pos.Y;
                    }

                    if ((pos.Flags & InteropValues.WindowPositionFlags.SWP_NOSIZE) == 0)
                    {
                        //要改变尺寸
                        targetRect.Right = targetRect.Left + pos.Width;
                        targetRect.Bottom = targetRect.Top + pos.Height;
                    }
                    else
                    {
                        //不改变尺寸
                        targetRect.Right = targetRect.Left + rect.Width;
                        targetRect.Bottom = targetRect.Top + rect.Height;
                    }

                    //使用目标矩形获取显示器信息
                    var monitor = InteropMethods.MonitorFromRect(ref targetRect, InteropValues.MONITOR_DEFAULTTOPRIMARY);
                    var info = new InteropValues.MONITORINFO();
                    info.cbSize = (uint) Marshal.SizeOf(info);
                    if (InteropMethods.GetMonitorInfo(monitor, ref info))
                    {
                        //基于显示器信息设置窗口尺寸位置
                        pos.X = info.rcMonitor.Left;
                        pos.Y = info.rcMonitor.Top;
                        pos.Width = info.rcMonitor.Right - info.rcMonitor.Left;
                        pos.Height = info.rcMonitor.Bottom - info.rcMonitor.Top;
                        pos.Flags &= ~(InteropValues.WindowPositionFlags.SWP_NOSIZE | InteropValues.WindowPositionFlags.SWP_NOMOVE |
                                       InteropValues.WindowPositionFlags.SWP_NOREDRAW);
                        pos.Flags |= InteropValues.WindowPositionFlags.SWP_NOCOPYBITS;

                        if (rect == info.rcMonitor)
                        {
                            var hwndSource = HwndSource.FromHwnd(hwnd);
                            if (hwndSource?.RootVisual is Window window)
                            {
                                //确保窗口的 WPF 属性与 Win32 位置一致，防止有逗比全屏后改 WPF 的属性，发生一些诡异的行为
                                //下面这样做其实不太好，会再次触发 WM_WINDOWPOSCHANGING 来着.....但是又没有其他时机了
                                // WM_WINDOWPOSCHANGED 不能用 
                                //（例如：在进入全屏后，修改 Left 属性，会进入 WM_WINDOWPOSCHANGING，然后在这里将消息里的结构体中的 Left 改回，
                                // 使对 Left 的修改无效，那么将不会进入 WM_WINDOWPOSCHANGED，窗口尺寸正常，但窗口的 Left 属性值错误。）
                                var logicalPos =
                                    hwndSource.CompositionTarget.TransformFromDevice.Transform(
                                        new Point(pos.X, pos.Y));
                                var logicalSize =
                                    hwndSource.CompositionTarget.TransformFromDevice.Transform(
                                        new Point(pos.Width, pos.Height));
                                window.Left = logicalPos.X;
                                window.Top = logicalPos.Y;
                                window.Width = logicalSize.X;
                                window.Height = logicalSize.Y;
                            }
                        }

                        //将修改后的结构体拷贝回去
                        Marshal.StructureToPtr(pos, lParam, false);
                    }
                }
            }
            catch
            {
                // 这里也不需要日志啥的，只是为了防止上面有逗比逻辑，在消息循环里面炸了
            }
        }

        return IntPtr.Zero;
    }
}
