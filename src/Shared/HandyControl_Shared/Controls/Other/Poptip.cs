using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Data.Enum;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class Poptip : AdornerElement
{
    private readonly Popup _popup;

    private DispatcherTimer _openTimer;

    public Poptip()
    {
        _popup = new Popup
        {
            AllowsTransparency = true,
            Child = this,
            Placement = PlacementMode.Relative
        };

        _popup.SetBinding(DataContextProperty, new Binding(DataContextProperty.Name) { Source = this });
    }

    public static readonly DependencyProperty HitModeProperty = DependencyProperty.RegisterAttached(
        "HitMode", typeof(HitMode), typeof(Poptip), new PropertyMetadata(HitMode.Hover));

    public static void SetHitMode(DependencyObject element, HitMode value)
        => element.SetValue(HitModeProperty, value);

    public static HitMode GetHitMode(DependencyObject element)
        => (HitMode) element.GetValue(HitModeProperty);

    public HitMode HitMode
    {
        get => (HitMode) GetValue(HitModeProperty);
        set => SetValue(HitModeProperty, value);
    }

    public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
        "Content", typeof(object), typeof(Poptip), new PropertyMetadata(default, OnContentChanged));

    private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Poptip) return;
        if (GetInstance(d) == null)
        {
            SetInstance(d, Default);
            SetIsInstance(d, false);
        }
    }

    public static void SetContent(DependencyObject element, object value)
        => element.SetValue(ContentProperty, value);

    public static object GetContent(DependencyObject element)
        => element.GetValue(ContentProperty);

    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
        nameof(ContentTemplate), typeof(DataTemplate), typeof(Poptip), new PropertyMetadata(default(DataTemplate)));

    public DataTemplate ContentTemplate
    {
        get => (DataTemplate) GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register(
        nameof(ContentStringFormat), typeof(string), typeof(Poptip), new PropertyMetadata(default(string)));

    public string ContentStringFormat
    {
        get => (string) GetValue(ContentStringFormatProperty);
        set => SetValue(ContentStringFormatProperty, value);
    }

    public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(
        nameof(ContentTemplateSelector), typeof(DataTemplateSelector), typeof(Poptip), new PropertyMetadata(default(DataTemplateSelector)));

    public DataTemplateSelector ContentTemplateSelector
    {
        get => (DataTemplateSelector) GetValue(ContentTemplateSelectorProperty);
        set => SetValue(ContentTemplateSelectorProperty, value);
    }

    public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached(
        "VerticalOffset", typeof(double), typeof(Poptip), new PropertyMetadata(ValueBoxes.Double0Box));

    public static void SetVerticalOffset(DependencyObject element, double value)
    {
        element.SetValue(VerticalOffsetProperty, value);
    }

    public static double GetVerticalOffset(DependencyObject element)
    {
        return (double) element.GetValue(VerticalOffsetProperty);
    }

    public double VerticalOffset
    {
        get => (double) GetValue(VerticalOffsetProperty);
        set => SetValue(VerticalOffsetProperty, value);
    }

    public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached(
        "HorizontalOffset", typeof(double), typeof(Poptip), new PropertyMetadata(ValueBoxes.Double0Box));

    public static void SetHorizontalOffset(DependencyObject element, double value)
    {
        element.SetValue(HorizontalOffsetProperty, value);
    }

    public static double GetHorizontalOffset(DependencyObject element)
    {
        return (double) element.GetValue(HorizontalOffsetProperty);
    }

    public double HorizontalOffset
    {
        get => (double) GetValue(HorizontalOffsetProperty);
        set => SetValue(HorizontalOffsetProperty, value);
    }

    public static readonly DependencyProperty PlacementTypeProperty = DependencyProperty.RegisterAttached(
        "PlacementType", typeof(PlacementType), typeof(Poptip), new PropertyMetadata(PlacementType.Top));

    public static void SetPlacement(DependencyObject element, PlacementType value)
        => element.SetValue(PlacementTypeProperty, value);

    public static PlacementType GetPlacement(DependencyObject element)
        => (PlacementType) element.GetValue(PlacementTypeProperty);

    public PlacementType PlacementType
    {
        get => (PlacementType) GetValue(PlacementTypeProperty);
        set => SetValue(PlacementTypeProperty, value);
    }

    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.RegisterAttached(
        "IsOpen", typeof(bool), typeof(Poptip), new PropertyMetadata(ValueBoxes.FalseBox, OnIsOpenChanged));

    private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Poptip poptip)
        {
            poptip.SwitchPoptip((bool) e.NewValue);
        }
        else
        {
            ((Poptip) GetInstance(d))?.SwitchPoptip((bool) e.NewValue);
        }
    }

    public static void SetIsOpen(DependencyObject element, bool value)
        => element.SetValue(IsOpenProperty, ValueBoxes.BooleanBox(value));

    public static bool GetIsOpen(DependencyObject element)
        => (bool) element.GetValue(IsOpenProperty);

    public bool IsOpen
    {
        get => (bool) GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(
        nameof(Delay), typeof(double), typeof(Poptip), new PropertyMetadata(1000.0), ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

    public double Delay
    {
        get => (double) GetValue(DelayProperty);
        set => SetValue(DelayProperty, value);
    }

    public static Poptip Default => new();

    protected sealed override void OnTargetChanged(FrameworkElement element, bool isNew)
    {
        base.OnTargetChanged(element, isNew);

        if (element == null) return;

        if (!isNew)
        {
            element.MouseEnter -= Element_MouseEnter;
            element.MouseLeave -= Element_MouseLeave;
            element.GotFocus -= Element_GotFocus;
            element.LostFocus -= Element_LostFocus;
            ElementTarget = null;
        }
        else
        {
            element.MouseEnter += Element_MouseEnter;
            element.MouseLeave += Element_MouseLeave;
            element.GotFocus += Element_GotFocus;
            element.LostFocus += Element_LostFocus;
            ElementTarget = element;
            _popup.PlacementTarget = ElementTarget;
        }
    }

    protected override void Dispose() => SwitchPoptip(false);

    private void UpdateLocation()
    {
        var targetWidth = Target.RenderSize.Width;
        var targetHeight = Target.RenderSize.Height;

        Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var size = DesiredSize;

        var width = size.Width;
        var height = size.Height;

        var offsetX = .0;
        var offsetY = .0;

        var poptip = (Poptip) GetInstance(Target);
        var popupPlacement = poptip.PlacementType;
        var popupOffsetX = poptip.HorizontalOffset;
        var popupOffsetY = poptip.VerticalOffset;

        switch (popupPlacement)
        {
            case PlacementType.LeftTop:
                break;
            case PlacementType.Left:
                offsetY = -(height - targetHeight) * 0.5;
                break;
            case PlacementType.LeftBottom:
                offsetY = -(height - targetHeight);
                break;
            case PlacementType.TopLeft:
                offsetX = width;
                offsetY = -height;
                break;
            case PlacementType.Top:
                offsetX = (width + targetWidth) * 0.5;
                offsetY = -height;
                break;
            case PlacementType.TopRight:
                offsetX = targetWidth;
                offsetY = -height;
                break;
            case PlacementType.RightTop:
                offsetX = width + targetWidth;
                break;
            case PlacementType.Right:
                offsetX = width + targetWidth;
                offsetY = -(height - targetHeight) * 0.5;
                break;
            case PlacementType.RightBottom:
                offsetX = width + targetWidth;
                offsetY = -(height - targetHeight);
                break;
            case PlacementType.BottomLeft:
                offsetX = width;
                offsetY = targetHeight;
                break;
            case PlacementType.Bottom:
                offsetX = (width + targetWidth) * 0.5;
                offsetY = targetHeight;
                break;
            case PlacementType.BottomRight:
                offsetX = targetWidth;
                offsetY = targetHeight;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _popup.HorizontalOffset = offsetX + popupOffsetX;
        _popup.VerticalOffset = offsetY + popupOffsetY;
    }

    private void SwitchPoptip(bool isShow)
    {
        if (isShow)
        {
            if (!GetIsInstance(Target))
            {
                SetCurrentValue(ContentProperty, GetContent(Target));
                SetCurrentValue(PlacementTypeProperty, GetPlacement(Target));
                SetCurrentValue(HitModeProperty, GetHitMode(Target));
                SetCurrentValue(HorizontalOffsetProperty, GetHorizontalOffset(Target));
                SetCurrentValue(VerticalOffsetProperty, GetVerticalOffset(Target));
                SetCurrentValue(IsOpenProperty, GetIsOpen(Target));
            }

            _popup.PlacementTarget = Target;
            UpdateLocation();
        }

        ResetTimer();

        var delay = Delay;
        if (!isShow || HitMode != HitMode.Hover || MathHelper.IsVerySmall(delay))
        {

            _popup.IsOpen = isShow;
            Target.SetCurrentValue(IsOpenProperty, isShow);
        }
        else
        {
            _openTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(delay)
            };

            _openTimer.Tick += OpenTimer_Tick;
            _openTimer.Start();
        }
    }

    private void ResetTimer()
    {
        if (_openTimer != null)
        {
            _openTimer.Stop();
            _openTimer = null;
        }
    }

    private void OpenTimer_Tick(object sender, EventArgs e)
    {
        _popup.IsOpen = true;
        Target.SetCurrentValue(IsOpenProperty, true);

        ResetTimer();
    }

    private void Element_MouseEnter(object sender, MouseEventArgs e)
    {
        var hitMode = GetIsInstance(Target) ? HitMode : GetHitMode(Target);
        if (hitMode != HitMode.Hover) return;

        SwitchPoptip(true);
    }

    private void Element_MouseLeave(object sender, MouseEventArgs e)
    {
        var hitMode = GetIsInstance(Target) ? HitMode : GetHitMode(Target);
        if (hitMode != HitMode.Hover) return;

        SwitchPoptip(false);
    }

    private void Element_GotFocus(object sender, RoutedEventArgs e)
    {
        var hitMode = GetIsInstance(Target) ? HitMode : GetHitMode(Target);
        if (hitMode != HitMode.Focus) return;

        SwitchPoptip(true);
    }

    private void Element_LostFocus(object sender, RoutedEventArgs e)
    {
        var hitMode = GetIsInstance(Target) ? HitMode : GetHitMode(Target);
        if (hitMode != HitMode.Focus) return;

        SwitchPoptip(false);
    }
}
