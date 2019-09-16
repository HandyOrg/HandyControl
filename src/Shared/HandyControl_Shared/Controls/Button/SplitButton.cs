using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Data.Enum;

namespace HandyControl.Controls
{
    public class SplitButton : ButtonBase
    {
        public static readonly DependencyProperty HitModeProperty = DependencyProperty.Register(
            "HitMode", typeof(MouseHitMode), typeof(SplitButton), new PropertyMetadata(default(MouseHitMode)));

        public MouseHitMode HitMode
        {
            get => (MouseHitMode) GetValue(HitModeProperty);
            set => SetValue(HitModeProperty, value);
        }

        public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(
            "MaxDropDownHeight", typeof(double), typeof(SplitButton), new PropertyMetadata(SystemParameters.PrimaryScreenHeight / 3.0));

        public double MaxDropDownHeight
        {
            get => (double) GetValue(MaxDropDownHeightProperty);
            set => SetValue(MaxDropDownHeightProperty, value);
        }

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen", typeof(bool), typeof(SplitButton), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsDropDownOpen
        {
            get => (bool) GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register(
            "DropDownContent", typeof(object), typeof(SplitButton), new PropertyMetadata(default(object)));

        public object DropDownContent
        {
            get => GetValue(DropDownContentProperty);
            set => SetValue(DropDownContentProperty, value);
        }

        public SplitButton()
        {
            AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(ItemsOnClick));
        }

        private void ItemsOnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is MenuItem)
            {
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.FalseBox);
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (HitMode == MouseHitMode.Hover)
            {
                e.Handled = true;
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (HitMode == MouseHitMode.Hover)
            {
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.TrueBox);
            }
        }
    }
}
