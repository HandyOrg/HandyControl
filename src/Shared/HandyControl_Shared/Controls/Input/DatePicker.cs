using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementTextBox, Type = typeof(DatePickerTextBox))]
public class DatePicker : System.Windows.Controls.DatePicker, IDataInput
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
        if (_textBox != null)
        {
            _textBox.TextChanged -= TextBox_TextChanged;
        }

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

            _textBox.TextChanged += TextBox_TextChanged;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => VerifyData();

    public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

    public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
        nameof(IsError), typeof(bool), typeof(DatePicker), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsError
    {
        get => (bool) GetValue(IsErrorProperty);
        set => SetValue(IsErrorProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
        nameof(ErrorStr), typeof(string), typeof(DatePicker), new PropertyMetadata(default(string)));

    public string ErrorStr
    {
        get => (string) GetValue(ErrorStrProperty);
        set => SetValue(ErrorStrProperty, value);
    }

    public static readonly DependencyProperty TextTypeProperty = DependencyProperty.Register(
        nameof(TextType), typeof(TextType), typeof(DatePicker), new PropertyMetadata(default(TextType)));

    public TextType TextType
    {
        get => (TextType) GetValue(TextTypeProperty);
        set => SetValue(TextTypeProperty, value);
    }

    public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
        nameof(ShowClearButton), typeof(bool), typeof(DatePicker), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool ShowClearButton
    {
        get => (bool) GetValue(ShowClearButtonProperty);
        set => SetValue(ShowClearButtonProperty, ValueBoxes.BooleanBox(value));
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

    public virtual bool VerifyData()
    {
        OperationResult<bool> result;

        if (VerifyFunc != null)
        {
            result = VerifyFunc.Invoke(Text);
        }
        else
        {
            if (!string.IsNullOrEmpty(Text))
            {
                result = OperationResult.Success();
            }
            else if (InfoElement.GetNecessary(this))
            {
                result = OperationResult.Failed(Properties.Langs.Lang.IsNecessary);
            }
            else
            {
                result = OperationResult.Success();
            }
        }

        var isError = !result.Data;
        if (isError)
        {
            SetCurrentValue(IsErrorProperty, ValueBoxes.TrueBox);
            SetCurrentValue(ErrorStrProperty, result.Message);
        }
        else
        {
            isError = Validation.GetHasError(this);
            if (isError)
            {
                SetCurrentValue(ErrorStrProperty, Validation.GetErrors(this)[0].ErrorContent?.ToString());
            }
            else
            {
                SetCurrentValue(IsErrorProperty, ValueBoxes.FalseBox);
                SetCurrentValue(ErrorStrProperty, default(string));
            }
        }
        return !isError;
    }
}
