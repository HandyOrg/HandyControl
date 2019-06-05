using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Tools.Converter;

namespace HandyControl.Controls
{
    public class RelativePanel : Panel
    {
        public static readonly DependencyProperty AboveProperty = DependencyProperty.RegisterAttached(
            "Above", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetAbove(DependencyObject element, DependencyObject value) => element.SetValue(AboveProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetAbove(DependencyObject element) => (DependencyObject)element.GetValue(AboveProperty);

        public static readonly DependencyProperty AlignBottomWithProperty = DependencyProperty.RegisterAttached(
            "AlignBottomWith", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetAlignBottomWith(DependencyObject element, DependencyObject value) => element.SetValue(AlignBottomWithProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetAlignBottomWith(DependencyObject element) => (DependencyObject)element.GetValue(AlignBottomWithProperty);

        public static readonly DependencyProperty AlignBottomWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignBottomWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(default(bool)));

        public static void SetAlignBottomWithPanel(DependencyObject element, bool value) => element.SetValue(AlignBottomWithPanelProperty, value);

        public static bool GetAlignBottomWithPanel(DependencyObject element) => (bool)element.GetValue(AlignBottomWithPanelProperty);

        public static readonly DependencyProperty AlignHorizontalCenterWithProperty = DependencyProperty.RegisterAttached(
            "AlignHorizontalCenterWith", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetAlignHorizontalCenterWith(DependencyObject element, DependencyObject value) => element.SetValue(AlignHorizontalCenterWithProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetAlignHorizontalCenterWith(DependencyObject element) => (DependencyObject)element.GetValue(AlignHorizontalCenterWithProperty);

        public static readonly DependencyProperty AlignHorizontalCenterWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignHorizontalCenterWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(default(bool)));

        public static void SetAlignHorizontalCenterWithPanel(DependencyObject element, bool value) => element.SetValue(AlignHorizontalCenterWithPanelProperty, value);

        public static bool GetAlignHorizontalCenterWithPanel(DependencyObject element) => (bool)element.GetValue(AlignHorizontalCenterWithPanelProperty);

        public static readonly DependencyProperty AlignLeftWithProperty = DependencyProperty.RegisterAttached(
            "AlignLeftWith", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetAlignLeftWith(DependencyObject element, DependencyObject value) => element.SetValue(AlignLeftWithProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetAlignLeftWith(DependencyObject element) => (DependencyObject)element.GetValue(AlignLeftWithProperty);

        public static readonly DependencyProperty AlignLeftWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignLeftWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(default(bool)));

        public static void SetAlignLeftWithPanel(DependencyObject element, bool value) => element.SetValue(AlignLeftWithPanelProperty, value);

        public static bool GetAlignLeftWithPanel(DependencyObject element) => (bool)element.GetValue(AlignLeftWithPanelProperty);

        public static readonly DependencyProperty AlignRightWithProperty = DependencyProperty.RegisterAttached(
            "AlignRightWith", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetAlignRightWith(DependencyObject element, DependencyObject value) => element.SetValue(AlignRightWithProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetAlignRightWith(DependencyObject element) => (DependencyObject)element.GetValue(AlignRightWithProperty);

        public static readonly DependencyProperty AlignRightWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignRightWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(default(bool)));

        public static void SetAlignRightWithPanel(DependencyObject element, bool value) => element.SetValue(AlignRightWithPanelProperty, value);

        public static bool GetAlignRightWithPanel(DependencyObject element) => (bool)element.GetValue(AlignRightWithPanelProperty);

        public static readonly DependencyProperty AlignTopWithProperty = DependencyProperty.RegisterAttached(
            "AlignTopWith", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetAlignTopWith(DependencyObject element, DependencyObject value) => element.SetValue(AlignTopWithProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetAlignTopWith(DependencyObject element) => (DependencyObject)element.GetValue(AlignTopWithProperty);

        public static readonly DependencyProperty AlignTopWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignTopWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(default(bool)));

        public static void SetAlignTopWithPanel(DependencyObject element, bool value) => element.SetValue(AlignTopWithPanelProperty, value);

        public static bool GetAlignTopWithPanel(DependencyObject element) => (bool)element.GetValue(AlignTopWithPanelProperty);

        public static readonly DependencyProperty AlignVerticalCenterWithProperty = DependencyProperty.RegisterAttached(
            "AlignVerticalCenterWith", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetAlignVerticalCenterWith(DependencyObject element, DependencyObject value) => element.SetValue(AlignVerticalCenterWithProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetAlignVerticalCenterWith(DependencyObject element) => (DependencyObject)element.GetValue(AlignVerticalCenterWithProperty);

        public static readonly DependencyProperty AlignVerticalCenterWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignVerticalCenterWithPanel", typeof(bool), typeof(RelativePanel), new PropertyMetadata(default(bool)));

        public static void SetAlignVerticalCenterWithPanel(DependencyObject element, bool value) => element.SetValue(AlignVerticalCenterWithPanelProperty, value);

        public static bool GetAlignVerticalCenterWithPanel(DependencyObject element) => (bool)element.GetValue(AlignVerticalCenterWithPanelProperty);

        public static readonly DependencyProperty BelowProperty = DependencyProperty.RegisterAttached(
            "Below", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetBelow(DependencyObject element, DependencyObject value) => element.SetValue(BelowProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetBelow(DependencyObject element) => (DependencyObject)element.GetValue(BelowProperty);

        public static readonly DependencyProperty LeftOfProperty = DependencyProperty.RegisterAttached(
            "LeftOf", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetLeftOf(DependencyObject element, DependencyObject value) => element.SetValue(LeftOfProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetLeftOf(DependencyObject element) => (DependencyObject)element.GetValue(LeftOfProperty);

        public static readonly DependencyProperty RightOfProperty = DependencyProperty.RegisterAttached(
            "RightOf", typeof(DependencyObject), typeof(RelativePanel), new PropertyMetadata(default(DependencyObject)));

        public static void SetRightOf(DependencyObject element, DependencyObject value) => element.SetValue(RightOfProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static DependencyObject GetRightOf(DependencyObject element) => (DependencyObject)element.GetValue(RightOfProperty);
    }
}