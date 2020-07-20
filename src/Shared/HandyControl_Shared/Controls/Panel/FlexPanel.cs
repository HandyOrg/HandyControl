using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class FlexPanel : Panel
    {
        public static readonly DependencyProperty OrderProperty = DependencyProperty.RegisterAttached(
            "Order", typeof(int), typeof(FlexPanel), new PropertyMetadata(ValueBoxes.Int0Box));

        public static void SetOrder(DependencyObject element, int value)
            => element.SetValue(OrderProperty, value);

        public static int GetOrder(DependencyObject element)
            => (int) element.GetValue(OrderProperty);

        public static readonly DependencyProperty FlexGrowProperty = DependencyProperty.RegisterAttached(
            "FlexGrow", typeof(double), typeof(FlexPanel), new PropertyMetadata(ValueBoxes.Double0Box));

        public static void SetFlexGrow(DependencyObject element, double value)
            => element.SetValue(FlexGrowProperty, value);

        public static double GetFlexGrow(DependencyObject element)
            => (double) element.GetValue(FlexGrowProperty);

        public static readonly DependencyProperty FlexShrinkProperty = DependencyProperty.RegisterAttached(
            "FlexShrink", typeof(double), typeof(FlexPanel), new PropertyMetadata(ValueBoxes.Double1Box));

        public static void SetFlexShrink(DependencyObject element, double value)
            => element.SetValue(FlexShrinkProperty, value);

        public static double GetFlexShrink(DependencyObject element)
            => (double) element.GetValue(FlexShrinkProperty);

        public static readonly DependencyProperty FlexBasisProperty = DependencyProperty.RegisterAttached(
            "FlexBasis", typeof(double), typeof(FlexPanel), new PropertyMetadata(double.NaN));

        public static void SetFlexBasis(DependencyObject element, double value)
            => element.SetValue(FlexBasisProperty, value);

        public static double GetFlexBasis(DependencyObject element)
            => (double) element.GetValue(FlexBasisProperty);

        public static readonly DependencyProperty AlignSelfProperty = DependencyProperty.RegisterAttached(
            "AlignSelf", typeof(FlexItemAlignment), typeof(FlexPanel), new PropertyMetadata(default(FlexItemAlignment)));

        public static void SetAlignSelf(DependencyObject element, FlexItemAlignment value)
            => element.SetValue(AlignSelfProperty, value);

        public static FlexItemAlignment GetAlignSelf(DependencyObject element)
            => (FlexItemAlignment) element.GetValue(AlignSelfProperty);

        public static readonly DependencyProperty FlexDirectionProperty = DependencyProperty.Register(
            "FlexDirection", typeof(FlexOrientation), typeof(FlexPanel), new PropertyMetadata(default(FlexOrientation)));

        public FlexOrientation FlexDirection
        {
            get => (FlexOrientation) GetValue(FlexDirectionProperty);
            set => SetValue(FlexDirectionProperty, value);
        }

        public static readonly DependencyProperty FlexWrapProperty = DependencyProperty.Register(
            "FlexWrap", typeof(FlexWrapping), typeof(FlexPanel), new PropertyMetadata(default(FlexWrapping)));

        public FlexWrapping FlexWrap
        {
            get => (FlexWrapping) GetValue(FlexWrapProperty);
            set => SetValue(FlexWrapProperty, value);
        }

        public static readonly DependencyProperty JustifyContentProperty = DependencyProperty.Register(
            "JustifyContent", typeof(FlexContentJustify), typeof(FlexPanel), new PropertyMetadata(default(FlexContentJustify)));

        public FlexContentJustify JustifyContent
        {
            get => (FlexContentJustify) GetValue(JustifyContentProperty);
            set => SetValue(JustifyContentProperty, value);
        }

        public static readonly DependencyProperty AlignItemsProperty = DependencyProperty.Register(
            "AlignItems", typeof(FlexItemsAlignment), typeof(FlexPanel), new PropertyMetadata(default(FlexItemsAlignment)));

        public FlexItemsAlignment AlignItems
        {
            get => (FlexItemsAlignment) GetValue(AlignItemsProperty);
            set => SetValue(AlignItemsProperty, value);
        }

        public static readonly DependencyProperty AlignContentProperty = DependencyProperty.Register(
            "AlignContent", typeof(FlexContentAlignment), typeof(FlexPanel), new PropertyMetadata(default(FlexContentAlignment)));

        public FlexContentAlignment AlignContent
        {
            get => (FlexContentAlignment) GetValue(AlignContentProperty);
            set => SetValue(AlignContentProperty, value);
        }
    }
}
