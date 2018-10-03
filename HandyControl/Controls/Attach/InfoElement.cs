using System.Windows;
using HandyControl.Data.Enum;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    public class InfoElement : DependencyObject
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached(
            "Title", typeof(string), typeof(InfoElement), new PropertyMetadata(default(string)));

        public static void SetTitle(DependencyObject element, string value) => element.SetValue(TitleProperty, value);

        public static string GetTitle(DependencyObject element) => (string) element.GetValue(TitleProperty);

        public static readonly DependencyProperty RequiredProperty = DependencyProperty.RegisterAttached(
            "Required", typeof(bool), typeof(InfoElement), new PropertyMetadata(default(bool)));

        public static void SetRequired(DependencyObject element, bool value) => element.SetValue(RequiredProperty, value);

        public static bool GetRequired(DependencyObject element) => (bool)element.GetValue(RequiredProperty);

        public static readonly DependencyProperty TitleAlignmentProperty = DependencyProperty.RegisterAttached(
            "TitleAlignment", typeof(TitleAlignment), typeof(InfoElement), new PropertyMetadata(default(TitleAlignment)));

        public static void SetTitleAlignment(DependencyObject element, TitleAlignment value) => element.SetValue(TitleAlignmentProperty, value);

        public static TitleAlignment GetTitleAlignment(DependencyObject element) => (TitleAlignment) element.GetValue(TitleAlignmentProperty);
    }
}