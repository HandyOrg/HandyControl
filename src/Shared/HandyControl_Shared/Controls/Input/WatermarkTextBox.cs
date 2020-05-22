using System.Windows;

namespace HandyControl.Controls
{
    public class WatermarkTextBox : TextBox
    {
        #region Public Properties

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark", typeof(object), typeof(WatermarkTextBox), new PropertyMetadata(default(object)));

        public object Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        #endregion Watermark

        #endregion Public Properties

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (IsEnabled)
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    Select(0, Text.Length);
                }
            }
        }
    }
}