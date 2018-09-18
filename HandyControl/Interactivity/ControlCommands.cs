using System.Windows.Input;

namespace HandyControl.Interactivity
{
    /// <summary>
    ///     控件库使用的所有命令
    /// </summary>
    public static class ControlCommands
    {
        /// <summary>
        ///     关闭
        /// </summary>
        public static RoutedCommand Close { get; } = new RoutedCommand("Close", typeof(ControlCommands));

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
    }
}