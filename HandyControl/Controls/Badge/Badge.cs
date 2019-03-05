using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class Badge : ContentControl
    {
        public Badge()
        {
            
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Badge), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(int), typeof(Badge), new PropertyMetadata(ValueBoxes.Int0Box, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (Badge) d;
            var v = (int) e.NewValue;
            ctl.Text = v.ToString();
        }

        public int Value
        {
            get => (int) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty IsDotProperty = DependencyProperty.Register(
            "IsDot", typeof(bool), typeof(Badge), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsDot
        {
            get => (bool) GetValue(IsDotProperty);
            set => SetValue(IsDotProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(int), typeof(Badge), new PropertyMetadata(ValueBoxes.Int99Box));

        public int Maximum
        {
            get => (int) GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
    }
}