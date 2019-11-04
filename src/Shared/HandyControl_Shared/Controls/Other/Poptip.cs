using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Data.Enum;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    public class Poptip : Control
    {
        private Popup _popup;

        private AdornerContainer _adorner;

        private UIElement _elementTarget;

        public Poptip()
        {
            _popup = new Popup
            {
                AllowsTransparency = true,
                Child = this
            };

            _popup.SetBinding(Popup.IsOpenProperty, new Binding(IsOpenProperty.Name)
            {
                Source = this,
                Mode = BindingMode.TwoWay
            });
        }

        public static readonly DependencyProperty HitModeProperty = DependencyProperty.Register(
            "HitMode", typeof(HitMode), typeof(Poptip), new PropertyMetadata(HitMode.Hover));

        public HitMode HitMode
        {
            get => (HitMode)GetValue(HitModeProperty);
            set => SetValue(HitModeProperty, value);
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(Poptip), new PropertyMetadata(default(object)));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            "ContentTemplate", typeof(DataTemplate), typeof(Poptip), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register(
            "ContentStringFormat", typeof(string), typeof(Poptip), new PropertyMetadata(default(string)));

        public string ContentStringFormat
        {
            get => (string)GetValue(ContentStringFormatProperty);
            set => SetValue(ContentStringFormatProperty, value);
        }

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(
            "ContentTemplateSelector", typeof(DataTemplateSelector), typeof(Poptip), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector ContentTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty);
            set => SetValue(ContentTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
            "HorizontalOffset", typeof(double), typeof(Poptip), new PropertyMetadata(ValueBoxes.Double0Box));

        public double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            "VerticalOffset", typeof(double), typeof(Poptip), new PropertyMetadata(ValueBoxes.Double0Box));

        public double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }

        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
            "Placement", typeof(TipPlacement), typeof(Poptip), new PropertyMetadata(TipPlacement.Top));

        public TipPlacement Placement
        {
            get => (TipPlacement) GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen", typeof(bool), typeof(Poptip), new PropertyMetadata(default(bool)));

        public bool IsOpen
        {
            get => (bool) GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target", typeof(UIElement), typeof(Poptip), new PropertyMetadata(default(UIElement), OnTargetChanged));

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (Poptip)d;
            ctl.UpdateTarget(ctl._elementTarget, false);
            ctl.UpdateTarget((UIElement)e.NewValue, true);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UIElement Target
        {
            get => (UIElement) GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached(
            "Instance", typeof(Poptip), typeof(Poptip), new PropertyMetadata(default(Poptip), OnMagnifierChanged));

        private static void OnMagnifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is UIElement target)) return;
            var poptip = (Poptip)e.NewValue;
            poptip.Target = target;
        }

        public static void SetInstance(DependencyObject element, Poptip value)
            => element.SetValue(InstanceProperty, value);

        public static Poptip GetInstance(DependencyObject element)
            => (Poptip)element.GetValue(InstanceProperty);

        public static Poptip Default => new Poptip();

        private void UpdateTarget(UIElement element, bool isNew)
        {
            if (element == null) return;

            if (!isNew)
            {
                element.MouseEnter -= Element_MouseEnter;
                element.MouseLeave -= Element_MouseLeave;
                element.GotFocus -= Element_GotFocus;
                element.LostFocus -= Element_LostFocus;
                _elementTarget = null;
            }
            else
            {
                element.MouseEnter += Element_MouseEnter;
                element.MouseLeave += Element_MouseLeave;
                element.GotFocus += Element_GotFocus;
                element.LostFocus += Element_LostFocus;
                _elementTarget = element;
                _popup.PlacementTarget = _elementTarget;
            }
        }

        private void UpdateLocation()
        {
            var targetPoint = Mouse.GetPosition(Target);
        }

        private void ShowPoptip()
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

            UpdateLocation();
            SetCurrentValue(IsOpenProperty, true);
        }

        private void ClosePoptip()
        {
            var layer = AdornerLayer.GetAdornerLayer(Target);
            if (layer == null) return;
            layer.Remove(_adorner);
            _adorner.Child = null;
            _adorner = null;
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            if (HitMode != HitMode.Hover) return;

            ShowPoptip();
        }

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            if (HitMode != HitMode.Hover) return;

            ClosePoptip();
        }

        private void Element_GotFocus(object sender, RoutedEventArgs e)
        {
            if (HitMode != HitMode.Focus) return;

            ShowPoptip();
        }

        private void Element_LostFocus(object sender, RoutedEventArgs e)
        {
            if (HitMode != HitMode.Focus) return;

            ClosePoptip();
        }
    }
}
