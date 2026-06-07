using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

/// <summary>
///     切换块
/// </summary>
public class ToggleBlock : Control
{
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked),
        typeof(bool?), typeof(ToggleBlock),
        new FrameworkPropertyMetadata(ValueBoxes.FalseBox,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));

    public static readonly DependencyProperty CheckedContentProperty =
        DependencyProperty.Register(nameof(CheckedContent), typeof(object), typeof(ToggleBlock),
            new PropertyMetadata(default(object)));

    public static readonly DependencyProperty UnCheckedContentProperty =
        DependencyProperty.Register(nameof(UnCheckedContent), typeof(object), typeof(ToggleBlock),
            new PropertyMetadata(default(object)));

    public static readonly DependencyProperty IndeterminateContentProperty =
        DependencyProperty.Register(nameof(IndeterminateContent), typeof(object), typeof(ToggleBlock),
            new PropertyMetadata(default(object)));

    public static readonly DependencyProperty ToggleGestureProperty = DependencyProperty.Register(nameof(ToggleGesture),
        typeof(MouseGesture), typeof(ToggleBlock),
        new PropertyMetadata(new MouseGesture(MouseAction.None)));

    [Category("Appearance")]
    [TypeConverter(typeof(NullableBoolConverter))]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public bool? IsChecked
    {
        get
        {
            // Because Nullable<bool> unboxing is very slow (uses reflection) first we cast to bool
            var value = GetValue(IsCheckedProperty);
            // ReSharper disable once RedundantExplicitNullableCreation
            return value == null ? new bool?() : new bool?((bool) value);
        }
        set => SetValue(IsCheckedProperty, value.HasValue ? ValueBoxes.BooleanBox(value.Value) : null);
    }

    public object CheckedContent
    {
        get => GetValue(CheckedContentProperty);
        set => SetValue(CheckedContentProperty, value);
    }

    public object UnCheckedContent
    {
        get => GetValue(UnCheckedContentProperty);
        set => SetValue(UnCheckedContentProperty, value);
    }

    public object IndeterminateContent
    {
        get => GetValue(IndeterminateContentProperty);
        set => SetValue(IndeterminateContentProperty, value);
    }

    [ValueSerializer(typeof(MouseGestureValueSerializer))]
    [TypeConverter(typeof(MouseGestureConverter))]
    public MouseGesture ToggleGesture
    {
        get => (MouseGesture) GetValue(ToggleGestureProperty);
        set => SetValue(ToggleGestureProperty, value);
    }

    public ToggleBlock()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Toggle, OnToggled));
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);

        if (e.ChangedButton is MouseButton.Left && ToggleGesture.MouseAction is MouseAction.LeftClick ||
            e.ChangedButton is MouseButton.Right && ToggleGesture.MouseAction is MouseAction.RightClick ||
            e.ChangedButton is MouseButton.Middle && ToggleGesture.MouseAction is MouseAction.MiddleClick ||
            e.ChangedButton is MouseButton.Left && ToggleGesture.MouseAction is MouseAction.LeftDoubleClick &&
            e.ClickCount == 2 ||
            e.ChangedButton is MouseButton.Right && ToggleGesture.MouseAction is MouseAction.RightDoubleClick &&
            e.ClickCount == 2 ||
            e.ChangedButton is MouseButton.Middle && ToggleGesture.MouseAction is MouseAction.MiddleDoubleClick &&
            e.ClickCount == 2)
        {
            ControlCommands.Toggle.Execute(null, this);
        }
    }

    private void OnToggled(object sender, ExecutedRoutedEventArgs e)
    {
        SetCurrentValue(IsCheckedProperty, IsChecked == true ? ValueBoxes.FalseBox : ValueBoxes.TrueBox);
    }
}
