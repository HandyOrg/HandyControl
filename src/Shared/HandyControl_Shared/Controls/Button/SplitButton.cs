using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Data.Enum;

namespace HandyControl.Controls;

public class SplitButton : ButtonBase
{
    public static readonly DependencyProperty HitModeProperty = DependencyProperty.Register(
        nameof(HitMode), typeof(HitMode), typeof(SplitButton), new PropertyMetadata(default(HitMode)));

    public HitMode HitMode
    {
        get => (HitMode) GetValue(HitModeProperty);
        set => SetValue(HitModeProperty, value);
    }

    public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(
        nameof(MaxDropDownHeight), typeof(double), typeof(SplitButton), new PropertyMetadata(SystemParameters.PrimaryScreenHeight / 3.0));

    public double MaxDropDownHeight
    {
        get => (double) GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
    }

    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen), typeof(bool), typeof(SplitButton), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsDropDownOpen
    {
        get => (bool) GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register(
        nameof(DropDownContent), typeof(object), typeof(SplitButton), new PropertyMetadata(default(object)));

    public object DropDownContent
    {
        get => GetValue(DropDownContentProperty);
        set => SetValue(DropDownContentProperty, value);
    }

    public SplitButton()
    {
        AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(ItemsOnClick));
    }

    private void ItemsOnClick(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is MenuItem)
        {
            SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.FalseBox);
        }
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonDown(e);

        if (HitMode == HitMode.Hover)
        {
            e.Handled = true;
        }
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        if (HitMode == HitMode.Hover)
        {
            SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.TrueBox);
        }
    }
}
