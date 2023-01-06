using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls;

[TemplatePart(Name = BackElement, Type = typeof(Border))]
public class Dialog : ContentControl
{
    private const string BackElement = "PART_BackElement";

    private string _token;
    private Border _backElement;
    private AdornerContainer _container;

    private static readonly Dictionary<string, FrameworkElement> ContainerDict = new();
    private static readonly Dictionary<string, Dialog> DialogDict = new();

    public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
        nameof(IsClosed), typeof(bool), typeof(Dialog), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsClosed
    {
        get => (bool) GetValue(IsClosedProperty);
        internal set => SetValue(IsClosedProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty MaskCanCloseProperty = DependencyProperty.RegisterAttached(
        "MaskCanClose", typeof(bool), typeof(Dialog), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetMaskCanClose(DependencyObject element, bool value)
        => element.SetValue(MaskCanCloseProperty, ValueBoxes.BooleanBox(value));

    public static bool GetMaskCanClose(DependencyObject element)
        => (bool) element.GetValue(MaskCanCloseProperty);

    public static readonly DependencyProperty MaskBrushProperty = DependencyProperty.Register(
        nameof(MaskBrush), typeof(Brush), typeof(Dialog), new PropertyMetadata(default(Brush)));

    public Brush MaskBrush
    {
        get => (Brush) GetValue(MaskBrushProperty);
        set => SetValue(MaskBrushProperty, value);
    }

    public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached(
        "Token", typeof(string), typeof(Dialog), new PropertyMetadata(default(string), OnTokenChanged));

    private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement element)
        {
            if (e.NewValue == null)
            {
                Unregister(element);
            }
            else
            {
                Register(e.NewValue.ToString(), element);
            }
        }
    }

    public static void SetToken(DependencyObject element, string value)
        => element.SetValue(TokenProperty, value);

    public static string GetToken(DependencyObject element)
        => (string) element.GetValue(TokenProperty);

    public Dialog()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => Close()));
    }

    public static void Register(string token, FrameworkElement element)
    {
        if (string.IsNullOrEmpty(token) || element == null) return;
        ContainerDict[token] = element;
    }

    public static void Unregister(string token, FrameworkElement element)
    {
        if (string.IsNullOrEmpty(token) || element == null) return;

        if (ContainerDict.ContainsKey(token))
        {
            if (ReferenceEquals(ContainerDict[token], element))
            {
                ContainerDict.Remove(token);
            }
        }
    }

    public static void Unregister(FrameworkElement element)
    {
        if (element == null) return;
        var first = ContainerDict.FirstOrDefault(item => ReferenceEquals(element, item.Value));
        if (!string.IsNullOrEmpty(first.Key))
        {
            ContainerDict.Remove(first.Key);
        }
    }

    public static void Unregister(string token)
    {
        if (string.IsNullOrEmpty(token)) return;

        if (ContainerDict.ContainsKey(token))
        {
            ContainerDict.Remove(token);
        }
    }

    public static Dialog Show<T>(string token = "") where T : new() => Show(new T(), token);

    public static Dialog Show(object content, string token = "")
    {
        var dialog = new Dialog
        {
            _token = token,
            Content = content
        };

        FrameworkElement element;
        AdornerDecorator decorator;

        if (string.IsNullOrEmpty(token))
        {
            element = WindowHelper.GetActiveWindow();
            decorator = VisualHelper.GetChild<AdornerDecorator>(element);
        }
        else
        {
            Close(token);
            DialogDict[token] = dialog;
            ContainerDict.TryGetValue(token, out element);
            decorator = element is System.Windows.Window ?
                VisualHelper.GetChild<AdornerDecorator>(element) :
                VisualHelper.GetChild<DialogContainer>(element);
        }

        if (decorator != null)
        {
            if (decorator.Child != null)
            {
                decorator.Child.IsEnabled = false;
            }

            var layer = decorator.AdornerLayer;
            if (layer != null)
            {
                var container = new AdornerContainer(layer)
                {
                    Child = dialog
                };
                dialog._container = container;
                dialog.IsClosed = false;
                layer.Add(container);
            }
        }

        return dialog;
    }

    public static void Close(string token)
    {
        if (DialogDict.TryGetValue(token, out Dialog dialog))
        {
            dialog.Close();
        }
    }

    public void Close()
    {
        if (string.IsNullOrEmpty(_token))
        {
            Close(WindowHelper.GetActiveWindow());
        }
        else if (ContainerDict.TryGetValue(_token, out var element))
        {
            Close(element);
            DialogDict.Remove(_token);
        }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _backElement = GetTemplateChild(BackElement) as Border;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (GetMaskCanClose(this) && _backElement is { IsMouseDirectlyOver: true })
        {
            Close();
        }
    }

    private void Close(DependencyObject element)
    {
        if (element != null && _container != null)
        {
            var decorator = VisualHelper.GetChild<AdornerDecorator>(element);
            if (decorator != null)
            {
                if (decorator.Child != null)
                {
                    decorator.Child.IsEnabled = true;
                }
                var layer = decorator.AdornerLayer;
                layer?.Remove(_container);
                IsClosed = true;
            }
        }
    }
}
