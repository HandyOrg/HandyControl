using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls;

public abstract class AdornerElement : Control, IDisposable
{
    protected FrameworkElement ElementTarget { get; set; }

    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
        nameof(Target), typeof(FrameworkElement), typeof(AdornerElement), new PropertyMetadata(default(FrameworkElement), OnTargetChanged));

    private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (AdornerElement) d;
        ctl.OnTargetChanged(ctl.ElementTarget, false);
        ctl.OnTargetChanged((FrameworkElement) e.NewValue, true);
    }

    [Bindable(true), Category("Layout")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FrameworkElement Target
    {
        get => (FrameworkElement) GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached(
        "Instance", typeof(AdornerElement), typeof(AdornerElement), new PropertyMetadata(default(AdornerElement), OnInstanceChanged));

    private static void OnInstanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement target) return;
        var element = (AdornerElement) e.NewValue;
        element.OnInstanceChanged(target);
    }

    protected virtual void OnInstanceChanged(FrameworkElement target) => Target = target;

    public static void SetInstance(DependencyObject element, AdornerElement value)
        => element.SetValue(InstanceProperty, value);

    public static AdornerElement GetInstance(DependencyObject element)
        => (AdornerElement) element.GetValue(InstanceProperty);

    public static readonly DependencyProperty IsInstanceProperty = DependencyProperty.RegisterAttached(
        "IsInstance", typeof(bool), typeof(AdornerElement), new PropertyMetadata(ValueBoxes.TrueBox));

    public static void SetIsInstance(DependencyObject element, bool value)
        => element.SetValue(IsInstanceProperty, ValueBoxes.BooleanBox(value));

    public static bool GetIsInstance(DependencyObject element)
        => (bool) element.GetValue(IsInstanceProperty);

    protected virtual void OnTargetChanged(FrameworkElement element, bool isNew)
    {
        if (element == null) return;

        if (!isNew)
        {
            element.Unloaded -= TargetElement_Unloaded;
            ElementTarget = null;
        }
        else
        {
            element.Unloaded += TargetElement_Unloaded;
            ElementTarget = element;
        }
    }

    private void TargetElement_Unloaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element)
        {
            element.Unloaded -= TargetElement_Unloaded;
            Dispose();
        }
    }

    protected abstract void Dispose();

    void IDisposable.Dispose() => Dispose();
}
