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
#if NET35
        private bool _canCoerce;

        private object _isDropDownOpen;
#endif

        public static readonly DependencyProperty HitModeProperty = DependencyProperty.Register(
            "HitMode", typeof(HitMode), typeof(SplitButton), new PropertyMetadata(default(HitMode)));

        public HitMode HitMode
        {
            get => (HitMode) GetValue(HitModeProperty);
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
            "IsDropDownOpen", typeof(bool), typeof(SplitButton), new PropertyMetadata(ValueBoxes.FalseBox
#if NET35
                , null, CoerceIsDropDownOpen
#endif
                ));

#if NET35
        private static object CoerceIsDropDownOpen(DependencyObject d, object baseValue)
            => d is SplitButton splitButton ? splitButton.CoerceIsDropDownOpen(baseValue) : baseValue;

        private object CoerceIsDropDownOpen(object baseValue) => _canCoerce ? _isDropDownOpen : baseValue;

        private void SetIsDropDownOpen(object isDropDownOpen)
        {
            _isDropDownOpen = isDropDownOpen;
            _canCoerce = true;
            CoerceValue(IsDropDownOpenProperty);
            _canCoerce = false;
        }
#endif

        public bool IsDropDownOpen
        {
            get => (bool) GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
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
#if NET35
                SetIsDropDownOpen(ValueBoxes.FalseBox);
#else
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.FalseBox);
#endif
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (HitMode == HitMode.Hover)
            {
                e.Handled = true;
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (HitMode == HitMode.Hover)
            {
#if NET35
                SetIsDropDownOpen(ValueBoxes.TrueBox);
#else
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.TrueBox);
#endif
            }
        }
    }
}
