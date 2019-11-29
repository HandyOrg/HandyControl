using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class RelativePanel : Panel
    {
        #region Panel alignment relationships

        public static readonly DependencyProperty AlignLeftWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignLeftWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(ValueBoxes.FalseBox));

        public static void SetAlignLeftWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignLeftWithPanelProperty, value);

        public static bool GetAlignLeftWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignLeftWithPanelProperty);

        public static readonly DependencyProperty AlignTopWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignTopWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(ValueBoxes.FalseBox));

        public static void SetAlignTopWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignTopWithPanelProperty, value);

        public static bool GetAlignTopWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignTopWithPanelProperty);

        public static readonly DependencyProperty AlignRightWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignRightWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(ValueBoxes.FalseBox));

        public static void SetAlignRightWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignRightWithPanelProperty, value);

        public static bool GetAlignRightWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignRightWithPanelProperty);

        public static readonly DependencyProperty AlignBottomWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignBottomWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(ValueBoxes.FalseBox));

        public static void SetAlignBottomWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignBottomWithPanelProperty, value);

        public static bool GetAlignBottomWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignBottomWithPanelProperty);

        public static readonly DependencyProperty AlignHorizontalCenterWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignHorizontalCenterWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(ValueBoxes.FalseBox));

        public static void SetAlignHorizontalCenterWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignHorizontalCenterWithPanelProperty, value);

        public static bool GetAlignHorizontalCenterWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignHorizontalCenterWithPanelProperty);

        public static readonly DependencyProperty AlignVerticalCenterWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignVerticalCenterWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(ValueBoxes.FalseBox));

        public static void SetAlignVerticalCenterWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignVerticalCenterWithPanelProperty, value);

        public static bool GetAlignVerticalCenterWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignVerticalCenterWithPanelProperty);

        #endregion

        #region Sibling alignment relationships

        public static readonly DependencyProperty AlignLeftWithProperty = DependencyProperty.RegisterAttached(
            "AlignLeftWith", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetAlignLeftWith(DependencyObject element, string value)
            => element.SetValue(AlignLeftWithProperty, value);

        public static string GetAlignLeftWith(DependencyObject element)
            => (string) element.GetValue(AlignLeftWithProperty);

        public static readonly DependencyProperty AlignTopWithProperty = DependencyProperty.RegisterAttached(
            "AlignTopWith", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetAlignTopWith(DependencyObject element, string value)
            => element.SetValue(AlignTopWithProperty, value);

        public static string GetAlignTopWith(DependencyObject element)
            => (string) element.GetValue(AlignTopWithProperty);

        public static readonly DependencyProperty AlignRightWithProperty = DependencyProperty.RegisterAttached(
            "AlignRightWith", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetAlignRightWith(DependencyObject element, string value)
            => element.SetValue(AlignRightWithProperty, value);

        public static string GetAlignRightWith(DependencyObject element)
            => (string) element.GetValue(AlignRightWithProperty);

        public static readonly DependencyProperty AlignBottomWithProperty = DependencyProperty.RegisterAttached(
            "AlignBottomWith", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetAlignBottomWith(DependencyObject element, string value)
            => element.SetValue(AlignBottomWithProperty, value);

        public static string GetAlignBottomWith(DependencyObject element)
            => (string) element.GetValue(AlignBottomWithProperty);

        public static readonly DependencyProperty AlignHorizontalCenterWithProperty = DependencyProperty.RegisterAttached(
            "AlignHorizontalCenterWith", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetAlignHorizontalCenterWith(DependencyObject element, string value)
            => element.SetValue(AlignHorizontalCenterWithProperty, value);

        public static string GetAlignHorizontalCenterWith(DependencyObject element)
            => (string) element.GetValue(AlignHorizontalCenterWithProperty);

        public static readonly DependencyProperty AlignVerticalCenterWithProperty = DependencyProperty.RegisterAttached(
            "AlignVerticalCenterWith", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetAlignVerticalCenterWith(DependencyObject element, string value)
            => element.SetValue(AlignVerticalCenterWithProperty, value);

        public static string GetAlignVerticalCenterWith(DependencyObject element)
            => (string) element.GetValue(AlignVerticalCenterWithProperty);

        #endregion

        #region Sibling positional relationships

        public static readonly DependencyProperty LeftOfProperty = DependencyProperty.RegisterAttached(
            "LeftOf", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetLeftOf(DependencyObject element, string value)
            => element.SetValue(LeftOfProperty, value);

        public static string GetLeftOf(DependencyObject element)
            => (string)element.GetValue(LeftOfProperty);

        public static readonly DependencyProperty AboveProperty = DependencyProperty.RegisterAttached(
            "Above", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetAbove(DependencyObject element, string value)
            => element.SetValue(AboveProperty, value);

        public static string GetAbove(DependencyObject element)
            => (string)element.GetValue(AboveProperty);

        public static readonly DependencyProperty RightOfProperty = DependencyProperty.RegisterAttached(
            "RightOf", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetRightOf(DependencyObject element, string value)
            => element.SetValue(RightOfProperty, value);

        public static string GetRightOf(DependencyObject element)
            => (string)element.GetValue(RightOfProperty);

        public static readonly DependencyProperty BelowProperty = DependencyProperty.RegisterAttached(
            "Below", typeof(string), typeof(RelativePanel), new PropertyMetadata(default(string)));

        public static void SetBelow(DependencyObject element, string value)
            => element.SetValue(BelowProperty, value);

        public static string GetBelow(DependencyObject element)
            => (string) element.GetValue(BelowProperty);

        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            return availableSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return arrangeSize;
        }
    }
}
