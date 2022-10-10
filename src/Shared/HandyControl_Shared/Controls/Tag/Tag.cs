using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

public class Tag : ContentControl
{
    public Tag()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) =>
        {
            var argsClosing = new CancelRoutedEventArgs(ClosingEvent, this);
            RaiseEvent(argsClosing);
            if (argsClosing.Cancel) return;

            RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
        }));
    }

    public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
        nameof(ShowCloseButton), typeof(bool), typeof(Tag), new PropertyMetadata(ValueBoxes.TrueBox));

    public bool ShowCloseButton
    {
        get => (bool) GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty SelectableProperty = DependencyProperty.Register(
        nameof(Selectable), typeof(bool), typeof(Tag), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool Selectable
    {
        get => (bool) GetValue(SelectableProperty);
        set => SetValue(SelectableProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
        nameof(IsSelected), typeof(bool), typeof(Tag), new PropertyMetadata(ValueBoxes.FalseBox, (o, args) =>
        {
            var ctl = (Tag) o;
            ctl.RaiseEvent(new RoutedEventArgs(SelectedEvent, ctl));
        }));

    public bool IsSelected
    {
        get => (bool) GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header), typeof(object), typeof(Tag), new PropertyMetadata(default, OnHeaderChanged));

    private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctrl = (Tag) d;

        ctrl.SetValue(HasHeaderPropertyKey, e.NewValue != null ? ValueBoxes.TrueBox : ValueBoxes.FalseBox);
        ctrl.OnHeaderChanged(e.OldValue, e.NewValue);
    }

    protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
    {
        RemoveLogicalChild(oldHeader);
        AddLogicalChild(newHeader);
    }

    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    internal static readonly DependencyPropertyKey HasHeaderPropertyKey = DependencyProperty.RegisterReadOnly(
        "HasHeader", typeof(bool), typeof(Tag), new PropertyMetadata(ValueBoxes.FalseBox));

    public static readonly DependencyProperty HasHeaderProperty = HasHeaderPropertyKey.DependencyProperty;

    [Bindable(false), Browsable(false)]
    public bool HasHeader => (bool) GetValue(HasHeaderProperty);

    public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(
        nameof(HeaderStringFormat), typeof(string), typeof(Tag), new PropertyMetadata(default(string)));

    public string HeaderStringFormat
    {
        get => (string) GetValue(HeaderStringFormatProperty);
        set => SetValue(HeaderStringFormatProperty, value);
    }

    public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
        nameof(HeaderTemplateSelector), typeof(DataTemplateSelector), typeof(Tag), new PropertyMetadata(default(DataTemplateSelector)));

    public DataTemplateSelector HeaderTemplateSelector
    {
        get => (DataTemplateSelector) GetValue(HeaderTemplateSelectorProperty);
        set => SetValue(HeaderTemplateSelectorProperty, value);
    }

    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
        nameof(HeaderTemplate), typeof(DataTemplate), typeof(Tag), new PropertyMetadata(default(DataTemplate)));

    public DataTemplate HeaderTemplate
    {
        get => (DataTemplate) GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(EventHandler), typeof(Tag));

    public event EventHandler Selected
    {
        add => AddHandler(SelectedEvent, value);
        remove => RemoveHandler(SelectedEvent, value);
    }

    public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof(EventHandler), typeof(Tag));

    public event EventHandler Closing
    {
        add => AddHandler(ClosingEvent, value);
        remove => RemoveHandler(ClosingEvent, value);
    }

    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(EventHandler), typeof(Tag));

    public event EventHandler Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        if (Selectable)
        {
            IsSelected = true;
        }
    }
}
