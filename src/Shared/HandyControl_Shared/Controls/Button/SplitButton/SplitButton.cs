using System.Windows;
using System.Windows.Controls;
using HandyControl.Data.Enum;

namespace HandyControl.Controls
{
    public class SplitButton : ItemsControl
    {
        public static readonly DependencyProperty HitModeProperty = DependencyProperty.Register(
            "HitMode", typeof(MouseHitMode), typeof(SplitButton), new PropertyMetadata(default(MouseHitMode)));

        public MouseHitMode HitMode
        {
            get => (MouseHitMode) GetValue(HitModeProperty);
            set => SetValue(HitModeProperty, value);
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(SplitButton), new PropertyMetadata(default(object)));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            "ContentTemplate", typeof(DataTemplate), typeof(SplitButton), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate) GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(
            "MaxDropDownHeight", typeof(double), typeof(SplitButton), new PropertyMetadata(SystemParameters.PrimaryScreenHeight / 3.0));

        public double MaxDropDownHeight
        {
            get => (double) GetValue(MaxDropDownHeightProperty);
            set => SetValue(MaxDropDownHeightProperty, value);
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is SplitButtonItem;

        protected override DependencyObject GetContainerForItemOverride() => new SplitButtonItem();
    }
}
