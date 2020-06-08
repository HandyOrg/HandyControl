using System.Windows;

namespace HandyControl.Tools.Extension
{
    // ReSharper disable once InconsistentNaming
    public static class UIElementExtension
    {
        /// <summary>
        ///     显示元素
        /// </summary>
        /// <param name="element"></param>
        public static void Show(this UIElement element) => element.Visibility = Visibility.Visible;

        /// <summary>
        ///     显示元素
        /// </summary>
        /// <param name="element"></param>
        /// <param name="show"></param>
        public static void Show(this UIElement element, bool show) => element.Visibility = show ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        ///     不现实元素，但保留空间
        /// </summary>
        /// <param name="element"></param>
        public static void Hide(this UIElement element) => element.Visibility = Visibility.Hidden;

        /// <summary>
        ///     不显示元素，且不保留空间
        /// </summary>
        /// <param name="element"></param>
        public static void Collapse(this UIElement element) => element.Visibility = Visibility.Collapsed;
    }
}