using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementVisualBrush, Type = typeof(VisualBrush))]
    public class Magnifier : Control
    {
        private Size _viewboxSize;

        private AdornerContainer _adorner;

        private UIElement _elementTarget;

        private const string ElementVisualBrush = "PART_VisualBrush";

        private VisualBrush _visualBrush = new VisualBrush();

        private readonly TranslateTransform _translateTransform;

        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
            "HorizontalOffset", typeof(double), typeof(Magnifier), new PropertyMetadata(ValueBoxes.Double0Box));

        public double HorizontalOffset
        {
            get => (double) GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            "VerticalOffset", typeof(double), typeof(Magnifier), new PropertyMetadata(ValueBoxes.Double0Box));

        public double VerticalOffset
        {
            get => (double) GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }

        public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached(
            "Instance", typeof(Magnifier), typeof(Magnifier), new PropertyMetadata(default(Magnifier), OnMagnifierChanged));

        private static void OnMagnifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is UIElement target)) return;
            var magnifier = (Magnifier)e.NewValue;
            magnifier.Target = target;
        }

        public static void SetInstance(DependencyObject element, Magnifier value)
            => element.SetValue(InstanceProperty, value);

        public static Magnifier GetInstance(DependencyObject element)
            => (Magnifier) element.GetValue(InstanceProperty);

        public static Magnifier Default => new Magnifier();

        internal static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target", typeof(UIElement), typeof(Magnifier), new PropertyMetadata(default(UIElement), OnTargetChanged));

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (Magnifier) d;
            ctl.UpdateTarget(ctl._elementTarget, false);
            ctl.UpdateTarget((UIElement)e.NewValue, true);
        }

        internal UIElement Target
        {
            get => (UIElement)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(Magnifier), new PropertyMetadata(5.0, OnScaleChanged), ValidateHelper.IsInRangeOfPosDouble);

        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Magnifier)d).UpdateViewboxSize();

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public Magnifier()
        {
            _translateTransform = new TranslateTransform();
            RenderTransform = _translateTransform;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var visualBrush = GetTemplateChild(ElementVisualBrush) as VisualBrush ?? new VisualBrush();
            visualBrush.Viewbox = _visualBrush.Viewbox;
            _visualBrush = visualBrush;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateViewboxSize();
        }

        private void UpdateViewboxSize() => _viewboxSize = new Size(ActualWidth / Scale, ActualHeight / Scale);

        private void UpdateTarget(UIElement element , bool isNew)
        {
            if (element == null) return;

            if (!isNew)
            {
                element.MouseEnter -= Element_MouseEnter;
                element.MouseLeave -= Element_MouseLeave;
                element.MouseMove -= Element_MouseMove;
                _elementTarget = null;
            }
            else
            {
                element.MouseEnter += Element_MouseEnter;
                element.MouseLeave += Element_MouseLeave;
                element.MouseMove += Element_MouseMove;
                _elementTarget = element;
            }
        }

        private void UpdateLocation()
        {
            var targetPoint = Mouse.GetPosition(Target);
            var subX = targetPoint.X - _visualBrush.Viewbox.Width / 2;
            var subY = targetPoint.Y - _visualBrush.Viewbox.Height / 2;

            var targetVector = VisualTreeHelper.GetOffset(Target);
            _visualBrush.Viewbox = new Rect(new Point(subX + targetVector.X, subY + targetVector.Y), _viewboxSize);

            var adornerPoint = Mouse.GetPosition(_adorner);
            _translateTransform.X = adornerPoint.X + HorizontalOffset;
            _translateTransform.Y = adornerPoint.Y + VerticalOffset;
        }

        private void Element_MouseMove(object sender, MouseEventArgs e) => UpdateLocation();

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            var layer = AdornerLayer.GetAdornerLayer(Target);
            if (layer == null) return;
            layer.Remove(_adorner);
            _adorner.Child = null;
            _adorner = null;
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_adorner == null)
            {
                var layer = AdornerLayer.GetAdornerLayer(Target);
                if (layer == null) return;
                _adorner = new AdornerContainer(layer)
                {
                    Child = this
                };
                layer.Add(_adorner);
            }
            this.Show();
        }
    }
}
