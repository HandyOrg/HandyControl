using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls;

public class ComboBoxAttach
{
    public static readonly DependencyProperty IsMouseWheelEnabledProperty = DependencyProperty.RegisterAttached(
        "IsMouseWheelEnabled", typeof(bool), typeof(ComboBoxAttach), new PropertyMetadata(ValueBoxes.TrueBox, OnIsMouseWheelEnabledChanged));

    private static void OnIsMouseWheelEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not System.Windows.Controls.ComboBox comboBox)
        {
            return;
        }

        if (!(bool) e.NewValue)
        {
            comboBox.PreviewMouseWheel += OnComboBoxPreviewMouseWheel;
        }
        else
        {
            comboBox.PreviewMouseWheel -= OnComboBoxPreviewMouseWheel;
        }

        return;

        static void OnComboBoxPreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs args)
        {
            if (sender is System.Windows.Controls.ComboBox { IsDropDownOpen: true })
            {
                return;
            }

            args.Handled = true;
        }
    }

    public static void SetIsMouseWheelEnabled(DependencyObject element, bool value)
        => element.SetValue(IsMouseWheelEnabledProperty, ValueBoxes.BooleanBox(value));

    public static bool GetIsMouseWheelEnabled(DependencyObject element)
        => (bool) element.GetValue(IsMouseWheelEnabledProperty);
}
