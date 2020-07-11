using System.Windows.Input;

namespace HandyControl.Interactivity
{
    /// <summary>
    ///     控件库使用的所有命令（为了统一，不使用wpf自带的命令）
    /// </summary>
    public static class ControlCommands
    {
        /// <summary>
        ///     搜索
        /// </summary>
        public static RoutedCommand Search { get; } = new RoutedCommand(nameof(Search), typeof(ControlCommands));

        /// <summary>
        ///     清除
        /// </summary>
        public static RoutedCommand Clear { get; } = new RoutedCommand(nameof(Clear), typeof(ControlCommands));

        /// <summary>
        ///     切换
        /// </summary>
        public static RoutedCommand Switch { get; } = new RoutedCommand(nameof(Switch), typeof(ControlCommands));

        /// <summary>
        ///     右转
        /// </summary>
        public static RoutedCommand RotateRight { get; } = new RoutedCommand(nameof(RotateRight), typeof(ControlCommands));

        /// <summary>
        ///     左转
        /// </summary>
        public static RoutedCommand RotateLeft { get; } = new RoutedCommand(nameof(RotateLeft), typeof(ControlCommands));

        /// <summary>
        ///     小
        /// </summary>
        public static RoutedCommand Reduce { get; } = new RoutedCommand(nameof(Reduce), typeof(ControlCommands));

        /// <summary>
        ///     大
        /// </summary>
        public static RoutedCommand Enlarge { get; } = new RoutedCommand(nameof(Enlarge), typeof(ControlCommands));

        /// <summary>
        ///     还原
        /// </summary>
        public static RoutedCommand Restore { get; } = new RoutedCommand(nameof(Restore), typeof(ControlCommands));

        /// <summary>
        ///     打开
        /// </summary>
        public static RoutedCommand Open { get; } = new RoutedCommand(nameof(Open), typeof(ControlCommands));

        /// <summary>
        ///     保存
        /// </summary>
        public static RoutedCommand Save { get; } = new RoutedCommand(nameof(Save), typeof(ControlCommands));

        /// <summary>
        ///     选中
        /// </summary>
        public static RoutedCommand Selected { get; } = new RoutedCommand(nameof(Selected), typeof(ControlCommands));

        /// <summary>
        ///     关闭
        /// </summary>
        public static RoutedCommand Close { get; } = new RoutedCommand(nameof(Close), typeof(ControlCommands));

        /// <summary>
        ///     取消
        /// </summary>
        public static RoutedCommand Cancel { get; } = new RoutedCommand(nameof(Cancel), typeof(ControlCommands));

        /// <summary>
        ///     确定
        /// </summary>
        public static RoutedCommand Confirm { get; } = new RoutedCommand(nameof(Confirm), typeof(ControlCommands));

        /// <summary>
        ///     是
        /// </summary>
        public static RoutedCommand Yes { get; } = new RoutedCommand(nameof(Yes), typeof(ControlCommands));

        /// <summary>
        ///     否
        /// </summary>
        public static RoutedCommand No { get; } = new RoutedCommand(nameof(No), typeof(ControlCommands));

        /// <summary>
        ///     关闭所有
        /// </summary>
        public static RoutedCommand CloseAll { get; } = new RoutedCommand(nameof(CloseAll), typeof(ControlCommands));

        /// <summary>
        ///     关闭其他
        /// </summary>
        public static RoutedCommand CloseOther { get; } = new RoutedCommand(nameof(CloseOther), typeof(ControlCommands));

        /// <summary>
        ///     上一个
        /// </summary>
        public static RoutedCommand Prev { get; } = new RoutedCommand(nameof(Prev), typeof(ControlCommands));

        /// <summary>
        ///     下一个
        /// </summary>
        public static RoutedCommand Next { get; } = new RoutedCommand(nameof(Next), typeof(ControlCommands));

        /// <summary>
        ///     跳转
        /// </summary>
        public static RoutedCommand Jump { get; } = new RoutedCommand(nameof(Jump), typeof(ControlCommands));

        /// <summary>
        ///     上午
        /// </summary>
        public static RoutedCommand Am { get; } = new RoutedCommand(nameof(Am), typeof(ControlCommands));

        /// <summary>
        ///     下午
        /// </summary>
        public static RoutedCommand Pm { get; } = new RoutedCommand(nameof(Pm), typeof(ControlCommands));

        /// <summary>
        ///     确认
        /// </summary>
        public static RoutedCommand Sure { get; } = new RoutedCommand(nameof(Sure), typeof(ControlCommands));

        /// <summary>
        ///     小时改变
        /// </summary>
        public static RoutedCommand HourChange { get; } = new RoutedCommand(nameof(HourChange), typeof(ControlCommands));

        /// <summary>
        ///     分钟改变
        /// </summary>
        public static RoutedCommand MinuteChange { get; } = new RoutedCommand(nameof(MinuteChange), typeof(ControlCommands));

        /// <summary>
        ///     秒改变
        /// </summary>
        public static RoutedCommand SecondChange { get; } = new RoutedCommand(nameof(SecondChange), typeof(ControlCommands));

        /// <summary>
        ///     鼠标移动
        /// </summary>
        public static RoutedCommand MouseMove { get; } = new RoutedCommand(nameof(MouseMove), typeof(ControlCommands));

        /// <summary>
        ///     打开链接
        /// </summary>
        public static OpenLinkCommand OpenLink { get; } = new OpenLinkCommand();

        /// <summary>
        ///     关闭程序
        /// </summary>
        public static ShutdownAppCommand ShutdownApp { get; } = new ShutdownAppCommand();

        /// <summary>
        ///     前置主窗口
        /// </summary>
        public static PushMainWindow2TopCommand PushMainWindow2Top { get; } = new PushMainWindow2TopCommand();

        /// <summary>
        ///     关闭窗口
        /// </summary>
        public static CloseWindowCommand CloseWindow { get; } = new CloseWindowCommand();

        /// <summary>
        ///     开始截图
        /// </summary>
        public static StartScreenshotCommand StartScreenshot { get; } = new StartScreenshotCommand();

        /// <summary>
        ///     按照类别排序
        /// </summary>
        public static RoutedCommand SortByCategory { get; } = new RoutedCommand(nameof(SortByCategory), typeof(ControlCommands));

        /// <summary>
        ///     按照名称排序
        /// </summary>
        public static RoutedCommand SortByName { get; } = new RoutedCommand(nameof(SortByName), typeof(ControlCommands));
    }
}