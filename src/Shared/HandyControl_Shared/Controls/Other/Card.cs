using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls;

public class Card : ContentControl
{
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header), typeof(object), typeof(Card), new PropertyMetadata(default(object)));

    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        nameof(HeaderTemplate), typeof(DataTemplate), typeof(Card), new PropertyMetadata(default(DataTemplate)));

    [Bindable(true), Category("Content")]
    public DataTemplate HeaderTemplate
    {
        get => (DataTemplate) GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
        nameof(HeaderTemplateSelector), typeof(DataTemplateSelector), typeof(Card), new PropertyMetadata(default(DataTemplateSelector)));

    [Bindable(true), Category("Content")]
    public DataTemplateSelector HeaderTemplateSelector
    {
        get => (DataTemplateSelector) GetValue(HeaderTemplateSelectorProperty);
        set => SetValue(HeaderTemplateSelectorProperty, value);
    }

    public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(
        nameof(HeaderStringFormat), typeof(string), typeof(Card), new PropertyMetadata(default(string)));

    [Bindable(true), Category("Content")]
    public string HeaderStringFormat
    {
        get => (string) GetValue(HeaderStringFormatProperty);
        set => SetValue(HeaderStringFormatProperty, value);
    }

    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
        nameof(Footer), typeof(object), typeof(Card), new PropertyMetadata(default(object)));

    public object Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(
        nameof(FooterTemplate), typeof(DataTemplate), typeof(Card), new PropertyMetadata(default(DataTemplate)));

    [Bindable(true), Category("Content")]
    public DataTemplate FooterTemplate
    {
        get => (DataTemplate) GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    public static readonly DependencyProperty FooterTemplateSelectorProperty = DependencyProperty.Register(
        nameof(FooterTemplateSelector), typeof(DataTemplateSelector), typeof(Card), new PropertyMetadata(default(DataTemplateSelector)));

    [Bindable(true), Category("Content")]
    public DataTemplateSelector FooterTemplateSelector
    {
        get => (DataTemplateSelector) GetValue(FooterTemplateSelectorProperty);
        set => SetValue(FooterTemplateSelectorProperty, value);
    }

    public static readonly DependencyProperty FooterStringFormatProperty = DependencyProperty.Register(
        nameof(FooterStringFormat), typeof(string), typeof(Card), new PropertyMetadata(default(string)));

    [Bindable(true), Category("Content")]
    public string FooterStringFormat
    {
        get => (string) GetValue(FooterStringFormatProperty);
        set => SetValue(FooterStringFormatProperty, value);
    }
}
