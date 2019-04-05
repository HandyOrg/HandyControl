using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class StatusSwitchElement
    {
        /// <summary>
        ///     选中时展示的元素
        /// </summary>
        public static readonly DependencyProperty CheckedElementProperty = DependencyProperty.RegisterAttached(
            "CheckedElement", typeof(object), typeof(StatusSwitchElement), new PropertyMetadata(default(object)));

        public static void SetCheckedElement(DependencyObject element, object value) => element.SetValue(CheckedElementProperty, value);

        public static object GetCheckedElement(DependencyObject element) => element.GetValue(CheckedElementProperty);

        /// <summary>
        ///     是否隐藏元素
        /// </summary>
        public static readonly DependencyProperty HiddenElementProperty = DependencyProperty.RegisterAttached(
            "HiddenElement", typeof(bool), typeof(StatusSwitchElement), new PropertyMetadata(ValueBoxes.FalseBox));

        public static void SetHiddenElement(DependencyObject element, bool value) => element.SetValue(HiddenElementProperty, value);

        public static bool GetHiddenElement(DependencyObject element) => (bool) element.GetValue(HiddenElementProperty);
    }
}