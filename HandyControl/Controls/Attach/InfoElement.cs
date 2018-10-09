using System.Windows;
using HandyControl.Data.Enum;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    public class InfoElement : DependencyObject
    {
        /// <summary>
        ///     标题
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached(
            "Title", typeof(string), typeof(InfoElement), new PropertyMetadata(default(string)));

        public static void SetTitle(DependencyObject element, string value) => element.SetValue(TitleProperty, value);

        public static string GetTitle(DependencyObject element) => (string) element.GetValue(TitleProperty);

        /// <summary>
        ///     占位符
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached(
            "Placeholder", typeof(string), typeof(InfoElement), new PropertyMetadata(default(string)));

        public static void SetPlaceholder(DependencyObject element, string value) => element.SetValue(PlaceholderProperty, value);

        public static string GetPlaceholder(DependencyObject element) => (string)element.GetValue(PlaceholderProperty);

        /// <summary>
        ///     是否必填
        /// </summary>
        public static readonly DependencyProperty NecessaryProperty = DependencyProperty.RegisterAttached(
            "Necessary", typeof(bool), typeof(InfoElement), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetNecessary(DependencyObject element, bool value) => element.SetValue(NecessaryProperty, value);

        public static bool GetNecessary(DependencyObject element) => (bool)element.GetValue(NecessaryProperty);

        /// <summary>
        ///     标记
        /// </summary>
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.RegisterAttached(
            "Symbol", typeof(string), typeof(InfoElement), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetSymbol(DependencyObject element, string value) => element.SetValue(SymbolProperty, value);

        public static string GetSymbol(DependencyObject element) => (string)element.GetValue(SymbolProperty);

        /// <summary>
        ///     标题对齐方式
        /// </summary>
        public static readonly DependencyProperty TitleAlignmentProperty = DependencyProperty.RegisterAttached(
            "TitleAlignment", typeof(TitleAlignment), typeof(InfoElement), new FrameworkPropertyMetadata(TitleAlignment.Top, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetTitleAlignment(DependencyObject element, TitleAlignment value) => element.SetValue(TitleAlignmentProperty, value);

        public static TitleAlignment GetTitleAlignment(DependencyObject element) => (TitleAlignment) element.GetValue(TitleAlignmentProperty);

        /// <summary>
        ///     标题宽度
        /// </summary>
        public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.RegisterAttached(
            "TitleWidth", typeof(GridLength), typeof(InfoElement), new FrameworkPropertyMetadata(new GridLength(120.0), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetTitleWidth(DependencyObject element, GridLength value) => element.SetValue(TitleWidthProperty, value);

        public static GridLength GetTitleWidth(DependencyObject element) => (GridLength)element.GetValue(TitleWidthProperty);

        /// <summary>
        ///     内容高度
        /// </summary>
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.RegisterAttached(
            "ContentHeight", typeof(double), typeof(InfoElement), new PropertyMetadata(30.0));

        public static void SetContentHeight(DependencyObject element, double value) => element.SetValue(ContentHeightProperty, value);

        public static double GetContentHeight(DependencyObject element) => (double)element.GetValue(ContentHeightProperty);
    }
}