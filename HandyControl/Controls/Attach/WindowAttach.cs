using System.Windows;
using System.Windows.Input;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public static class WindowAttach
    {
        public static readonly DependencyProperty IsDragElementProperty = DependencyProperty.RegisterAttached(
            "IsDragElement", typeof(bool), typeof(WindowAttach), new PropertyMetadata(ValueBoxes.FalseBox, OnIsDragElementChanged));

        public static void SetIsDragElement(DependencyObject element, bool value)
            => element.SetValue(IsDragElementProperty, value);

        public static bool GetIsDragElement(DependencyObject element)
            => (bool) element.GetValue(IsDragElementProperty);

        private static void OnIsDragElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement ctl)
            {
                if ((bool)e.NewValue)
                {
                    ctl.MouseLeftButtonDown += DragElement_MouseLeftButtonDown; ;
                }
                else
                {
                    ctl.MouseLeftButtonDown -= DragElement_MouseLeftButtonDown;
                }
            }
        }

        private static void DragElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DependencyObject obj && e.ButtonState == MouseButtonState.Pressed)
            {
                System.Windows.Window.GetWindow(obj)?.DragMove();
            }
        }
    }
}