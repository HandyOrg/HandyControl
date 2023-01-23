using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementTextBox, Type = typeof(DatePickerTextBox))]
public class DatePicker : System.Windows.Controls.DatePicker
{
    private const string ElementTextBox = "PART_TextBox";

    private System.Windows.Controls.TextBox _textBox;

    public DatePicker()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
        {
            SetCurrentValue(SelectedDateProperty, null);
            SetCurrentValue(TextProperty, "");
        }));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _textBox = GetTemplateChild(ElementTextBox) as System.Windows.Controls.TextBox;
        if (_textBox != null)
        {
            _textBox.SetBinding(SelectionBrushProperty, new Binding(SelectionBrushProperty.Name) { Source = this });
#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)
            _textBox.SetBinding(SelectionTextBrushProperty, new Binding(SelectionTextBrushProperty.Name) { Source = this });
#endif
            _textBox.SetBinding(SelectionOpacityProperty, new Binding(SelectionOpacityProperty.Name) { Source = this });
            _textBox.SetBinding(CaretBrushProperty, new Binding(CaretBrushProperty.Name) { Source = this });
        }
    }

    public static readonly DependencyProperty SelectionBrushProperty =
        TextBoxBase.SelectionBrushProperty.AddOwner(typeof(DatePicker));

    public Brush SelectionBrush
    {
        get => (Brush) GetValue(SelectionBrushProperty);
        set => SetValue(SelectionBrushProperty, value);
    }

#if !(NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)

    public static readonly DependencyProperty SelectionTextBrushProperty =
        TextBoxBase.SelectionTextBrushProperty.AddOwner(typeof(DatePicker));

    public Brush SelectionTextBrush
    {
        get => (Brush) GetValue(SelectionTextBrushProperty);
        set => SetValue(SelectionTextBrushProperty, value);
    }

#endif

    public static readonly DependencyProperty SelectionOpacityProperty =
        TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(DatePicker));

    public double SelectionOpacity
    {
        get => (double) GetValue(SelectionOpacityProperty);
        set => SetValue(SelectionOpacityProperty, value);
    }

    public static readonly DependencyProperty CaretBrushProperty =
        TextBoxBase.CaretBrushProperty.AddOwner(typeof(DatePicker));

    public Brush CaretBrush
    {
        get => (Brush) GetValue(CaretBrushProperty);
        set => SetValue(CaretBrushProperty, value);
    }
}
