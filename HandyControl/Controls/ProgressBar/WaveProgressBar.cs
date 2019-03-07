using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class WaveProgressBar : RangeBase
    {
        static WaveProgressBar()
        {
            FocusableProperty.OverrideMetadata(typeof(CircleProgressBar),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
            MaximumProperty.OverrideMetadata(typeof(CircleProgressBar),
                new FrameworkPropertyMetadata(ValueBoxes.Double100Box));
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(WaveProgressBar), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
            "ShowText", typeof(bool), typeof(WaveProgressBar), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool ShowText
        {
            get => (bool)GetValue(ShowTextProperty);
            set => SetValue(ShowTextProperty, value);
        }

        public static readonly DependencyProperty WaveBrushProperty = DependencyProperty.Register(
            "WaveBrush", typeof(Brush), typeof(WaveProgressBar), new PropertyMetadata(default(Brush)));

        public Brush WaveBrush
        {
            get => (Brush)GetValue(WaveBrushProperty);
            set => SetValue(WaveBrushProperty, value);
        }

        public static readonly DependencyProperty WaveDiameterProperty = DependencyProperty.Register(
            "WaveDiameter", typeof(double), typeof(WaveProgressBar), new PropertyMetadata(default(double)));

        public double WaveDiameter
        {
            get => (double)GetValue(WaveDiameterProperty);
            set => SetValue(WaveDiameterProperty, value);
        }
    }
}