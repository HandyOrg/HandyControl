using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HandyControl.Controls;

public class ImagePropertyEditor : PropertyEditorBase
{
    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var imageSelector = new ImageSelector
        {
            IsEnabled = !propertyItem.IsReadOnly,
            Width = 50,
            Height = 50,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        BindingOperations.SetBinding(this, UriProperty, new Binding(ImageSelector.UriProperty.Name)
        {
            Source = imageSelector,
            Mode = BindingMode.OneWay
        });

        return imageSelector;
    }

    internal static readonly DependencyProperty UriProperty = DependencyProperty.Register(
        nameof(Uri), typeof(Uri), typeof(ImagePropertyEditor), new PropertyMetadata(default(Uri), OnUriChangedCallback));

    private static void OnUriChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        ((ImagePropertyEditor) d).Source = e.NewValue is Uri uri ? BitmapFrame.Create(uri) : null;

    internal Uri Uri
    {
        get => (Uri) GetValue(UriProperty);
        set => SetValue(UriProperty, value);
    }

    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source), typeof(ImageSource), typeof(ImagePropertyEditor), new PropertyMetadata(default(ImageSource)));

    public ImageSource Source
    {
        get => (ImageSource) GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public override void CreateBinding(PropertyItem propertyItem, DependencyObject element)
        => BindingOperations.SetBinding(this, GetDependencyProperty(),
            new Binding($"({propertyItem.PropertyName})")
            {
                Source = propertyItem.Value,
                Mode = GetBindingMode(propertyItem),
                UpdateSourceTrigger = GetUpdateSourceTrigger(propertyItem),
                Converter = GetConverter(propertyItem)
            });

    public override DependencyProperty GetDependencyProperty() => SourceProperty;
}
