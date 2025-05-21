using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class ScrollViewerAttach
{
    public static readonly DependencyProperty AutoHideProperty = DependencyProperty.RegisterAttached(
        "AutoHide", typeof(bool), typeof(ScrollViewerAttach),
        new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.RegisterAttached(
        "Orientation", typeof(Orientation), typeof(ScrollViewerAttach),
        new FrameworkPropertyMetadata(ValueBoxes.VerticalBox, FrameworkPropertyMetadataOptions.Inherits,
            OnOrientationChanged));

    public static readonly DependencyProperty IsDisabledProperty = DependencyProperty.RegisterAttached(
        "IsDisabled", typeof(bool), typeof(ScrollViewerAttach),
        new PropertyMetadata(ValueBoxes.FalseBox, OnIsDisabledChanged));

    public static readonly DependencyProperty IsHoverResizingEnabledProperty = DependencyProperty.RegisterAttached(
        "IsHoverResizingEnabled", typeof(bool), typeof(ScrollViewerAttach),
        new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetAutoHide(DependencyObject element, bool value)
    {
        element.SetValue(AutoHideProperty, ValueBoxes.BooleanBox(value));
    }

    public static bool GetAutoHide(DependencyObject element)
    {
        return (bool) element.GetValue(AutoHideProperty);
    }

    private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        switch (d)
        {
            case ScrollViewer:
                return;
            case System.Windows.Controls.ScrollViewer scrollViewer
                when (Orientation) e.NewValue == Orientation.Horizontal:
                scrollViewer.PreviewMouseWheel += ScrollViewerPreviewMouseWheel;
                break;
            case System.Windows.Controls.ScrollViewer scrollViewer:
                scrollViewer.PreviewMouseWheel -= ScrollViewerPreviewMouseWheel;
                break;
        }

        return;

        void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs args)
        {
            var scrollViewerNative = (System.Windows.Controls.ScrollViewer) sender;
            scrollViewerNative.ScrollToHorizontalOffset(Math.Min(
                Math.Max(0, scrollViewerNative.HorizontalOffset - args.Delta), scrollViewerNative.ScrollableWidth));

            args.Handled = true;
        }
    }

    public static void SetOrientation(DependencyObject element, Orientation value)
    {
        element.SetValue(OrientationProperty, ValueBoxes.OrientationBox(value));
    }

    public static Orientation GetOrientation(DependencyObject element)
    {
        return (Orientation) element.GetValue(OrientationProperty);
    }

    private static void OnIsDisabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UIElement element)
        {
            return;
        }

        if ((bool) e.NewValue)
        {
            element.PreviewMouseWheel += ScrollViewerPreviewMouseWheel;
        }
        else
        {
            element.PreviewMouseWheel -= ScrollViewerPreviewMouseWheel;
        }

        return;

        void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs args)
        {
            if (args.Handled)
            {
                return;
            }

            args.Handled = true;

            if (VisualHelper.GetParent<System.Windows.Controls.ScrollViewer>((UIElement) sender) is { } scrollViewer)
            {
                scrollViewer.RaiseEvent(new MouseWheelEventArgs(args.MouseDevice, args.Timestamp, args.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                });
            }
        }
    }

    public static void SetIsDisabled(DependencyObject element, bool value)
    {
        element.SetValue(IsDisabledProperty, ValueBoxes.BooleanBox(value));
    }

    public static bool GetIsDisabled(DependencyObject element)
    {
        return (bool) element.GetValue(IsDisabledProperty);
    }

    public static void SetIsHoverResizingEnabled(DependencyObject d, bool value)
    {
        d.SetValue(IsHoverResizingEnabledProperty, value);
    }

    public static bool GetIsHoverResizingEnabled(DependencyObject d)
    {
        return (bool) d.GetValue(IsHoverResizingEnabledProperty);
    }
}
