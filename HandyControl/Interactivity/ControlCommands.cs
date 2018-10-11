using System.Windows.Input;

namespace HandyControl.Interactivity
{
    /// <summary>
    ///     控件库使用的所有命令
    /// </summary>
    public static class ControlCommands
    {
        /// <summary>
        ///     选中
        /// </summary>
        public static RoutedCommand Selected { get; } = new RoutedCommand("Selected", typeof(ControlCommands));

        /// <summary>
        ///     关闭
        /// </summary>
        public static RoutedCommand Close { get; } = new RoutedCommand("Close", typeof(ControlCommands));

        /// <summary>
        ///     取消
        /// </summary>
        public static RoutedCommand Cancel { get; } = new RoutedCommand("Cancel", typeof(ControlCommands));

        /// <summary>
        ///     确定
        /// </summary>
        public static RoutedCommand Confirm { get; } = new RoutedCommand("Confirm", typeof(ControlCommands));

        /// <summary>
        ///     关闭所有
        /// </summary>
        public static RoutedCommand CloseAll { get; } = new RoutedCommand("CloseAll", typeof(ControlCommands));

        /// <summary>
        ///     关闭其他
        /// </summary>
        public static RoutedCommand CloseOther { get; } = new RoutedCommand("CloseOther", typeof(ControlCommands));

        /// <summary>
        ///     上一个
        /// </summary>
        public static RoutedCommand Prev { get; } = new RoutedCommand("Prev", typeof(ControlCommands));

        /// <summary>
        ///     下一个
        /// </summary>
        public static RoutedCommand Next { get; } = new RoutedCommand("Next", typeof(ControlCommands));

        /// <summary>
        ///     上午
        /// </summary>
        public static RoutedCommand Am { get; } = new RoutedCommand("Am", typeof(ControlCommands));

        /// <summary>
        ///     下午
        /// </summary>
        public static RoutedCommand Pm { get; } = new RoutedCommand("Pm", typeof(ControlCommands));

        /// <summary>
        ///     确认
        /// </summary>
        public static RoutedCommand Sure { get; } = new RoutedCommand("Sure", typeof(ControlCommands));

        /// <summary>
        ///     小时改变
        /// </summary>
        public static RoutedCommand HourChange { get; } = new RoutedCommand("HourChange", typeof(ControlCommands));

        /// <summary>
        ///     分钟改变
        /// </summary>
        public static RoutedCommand MinuteChange { get; } = new RoutedCommand("MinuteChange", typeof(ControlCommands));

        /// <summary>
        ///     秒改变
        /// </summary>
        public static RoutedCommand SecondChange { get; } = new RoutedCommand("SecondChange", typeof(ControlCommands));

        /// <summary>
        ///     鼠标移动
        /// </summary>
        public static RoutedCommand MouseMove { get; } = new RoutedCommand("MouseMove", typeof(ControlCommands));
    }
}