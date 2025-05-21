using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using HandyControl.Tools;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementSliding, Type = typeof(Layoutable))]
public class SlidingTabContainer : ContentControl
{
    private const string ElementSliding = "PART_Sliding";

    private Control? _sliding;
    private TabControl? _tabControl;

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        OnSelectionChanged(null, null!);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (_tabControl is not null)
        {
            _tabControl.SelectionChanged -= OnSelectionChanged;
        }

        base.OnApplyTemplate(e);

        _tabControl = VisualHelper.GetParent<TabControl>(this);
        if (_tabControl is null)
        {
            return;
        }

        _tabControl.SelectionChanged += OnSelectionChanged;

        _sliding = e.NameScope.Find<Control>(ElementSliding);
        if (_sliding is null)
        {
            return;
        }

        if (IsLoaded)
        {
            OnSelectionChanged(null, null!);
        }
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        if (_tabControl is null)
        {
            return;
        }

        OnSelectionChanged(null, null!);
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        const int animationSpeed = 200;

        if (_sliding is null || _tabControl?.SelectedItem is null || !IsLoaded)
        {
            return;
        }

        if (_tabControl.ContainerFromItem(_tabControl.SelectedItem) is not TabItem tabItem)
        {
            return;
        }

        var padding = Padding;
        Point? offset = tabItem.TranslatePoint(new Point(-padding.Left, -padding.Top), this);
        if (offset is null)
        {
            return;
        }

        new Animation
        {
            FillMode = FillMode.Forward,
            Duration = TimeSpan.FromMilliseconds(animationSpeed),
            Easing = new QuadraticEaseOut(),
            Children =
            {
                new KeyFrame
                {
                    Setters =
                    {
                        new Setter(WidthProperty, tabItem.Bounds.Width),
                        new Setter(HeightProperty, tabItem.Bounds.Height),
                        new Setter(TranslateTransform.XProperty, offset.Value.X),
                        new Setter(TranslateTransform.YProperty, offset.Value.Y),
                    },
                    Cue = new Cue(1),
                }
            }
        }.RunAsync(_sliding);
    }
}
