using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementContentName, Type = typeof(ContentControl))]
    public class WatermarkTextBox : TextBox
    {
        #region Constants

        private const string ElementContentName = "PART_Watermark";

        #endregion Constants

        #region Data

        private ContentControl _elementContent;

        #endregion Data

        #region Public Properties

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark", typeof(object), typeof(WatermarkTextBox), new PropertyMetadata(default(object), OnWatermarkChanged));

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (WatermarkTextBox) d;
            ctl.OnWatermarkChanged();
        }

        public object Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        #endregion Watermark

        #endregion Public Properties

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _elementContent = GetTemplateChild(ElementContentName) as ContentControl;
            _elementContent?.SetBinding(ContentControl.ContentProperty, new Binding(WatermarkProperty.Name) {Source = this});

            OnWatermarkChanged();
        }

        private void OnWatermarkChanged()
        {
            if (_elementContent == null) return;
            if (Watermark is Control watermarkControl)
            {
                watermarkControl.IsTabStop = false;
                watermarkControl.IsHitTestVisible = false;
            }
        }

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