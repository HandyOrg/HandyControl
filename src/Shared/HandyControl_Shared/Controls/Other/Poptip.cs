using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Data.Enum;

namespace HandyControl.Controls
{
    public class Poptip : Control
    {
        private readonly Popup _popup;

        private UIElement _elementTarget;

        public Poptip()
        {
            _popup = new Popup
            {
                AllowsTransparency = true,
                Child = this
            };

            _popup.SetBinding(DataContextProperty, new Binding(DataContextProperty.Name) {Source = this});
        }

        public static readonly DependencyProperty HitModeProperty = DependencyProperty.RegisterAttached(
            "HitMode", typeof(HitMode), typeof(Poptip), new PropertyMetadata(HitMode.Hover));

        public static void SetHitMode(DependencyObject element, HitMode value)
            => element.SetValue(HitModeProperty, value);

        public static HitMode GetHitMode(DependencyObject element)
            => (HitMode) element.GetValue(HitModeProperty);

        public HitMode HitMode
        {
            get => (HitMode)GetValue(HitModeProperty);
            set => SetValue(HitModeProperty, value);
        }

        private static readonly DependencyProperty IsInstanceProperty = DependencyProperty.RegisterAttached(
            "IsInstance", typeof(bool), typeof(Poptip), new PropertyMetadata(ValueBoxes.TrueBox));

        private static void SetIsInstance(DependencyObject element, bool value)
            => element.SetValue(IsInstanceProperty, value);

        private static bool GetIsInstance(DependencyObject element)
            => (bool) element.GetValue(IsInstanceProperty);

        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
            "Content", typeof(object), typeof(Poptip), new PropertyMetadata(default, OnContentChanged));

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Poptip) return;
            if (GetInstance(d) == null)
            {
                SetInstance(d, Default);
                SetIsInstance(d, false);
            }
        }

        public static void SetContent(DependencyObject element, object value)
            => element.SetValue(ContentProperty, value);

        public static object GetContent(DependencyObject element)
            => element.GetValue(ContentProperty);

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

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.RegisterAttached(
            "Offset", typeof(double), typeof(Poptip), new PropertyMetadata(6.0));

        public static void SetOffset(DependencyObject element, double value)
            => element.SetValue(OffsetProperty, value);

        public static double GetOffset(DependencyObject element)
            => (double) element.GetValue(OffsetProperty);

        public double Offset
        {
            get => (double)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached(
            "Placement", typeof(TipPlacement), typeof(Poptip), new PropertyMetadata(TipPlacement.Top));

        public static void SetPlacement(DependencyObject element, TipPlacement value)
            => element.SetValue(PlacementProperty, value);

        public static TipPlacement GetPlacement(DependencyObject element)
            => (TipPlacement) element.GetValue(PlacementProperty);

        public TipPlacement Placement
        {
            get => (TipPlacement)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.RegisterAttached(
            "IsOpen", typeof(bool), typeof(Poptip), new PropertyMetadata(ValueBoxes.FalseBox, OnIsOpenChanged));

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Poptip poptip)
            {
                poptip.SwitchPoptip((bool) e.NewValue);
            }
            else
            {
                GetInstance(d)?.SwitchPoptip((bool)e.NewValue);
            }
        }

        public static void SetIsOpen(DependencyObject element, bool value)
            => element.SetValue(IsOpenProperty, value);

        public static bool GetIsOpen(DependencyObject element)
            => (bool) element.GetValue(IsOpenProperty);

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

        [Bindable(true), Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UIElement Target
        {
            get => (UIElement) GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached(
            "Instance", typeof(Poptip), typeof(Poptip), new PropertyMetadata(default(Poptip), OnPoptipChanged));

        private static void OnPoptipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
            var targetWidth = Target.RenderSize.Width;
            var targetHeight = Target.RenderSize.Height;

            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var size = DesiredSize;

            var width = size.Width;
            var height = size.Height;

            var offsetX = .0;
            var offsetY = .0;

            var poptip = GetInstance(Target);
            var popupPlacement = poptip.Placement;
            var popupOffset = poptip.Offset;

            PlacementMode placementMode;

            switch (popupPlacement)
            {
                case TipPlacement.LeftTop:
                    offsetX = -popupOffset;
                    placementMode = PlacementMode.Left;
                    break;
                case TipPlacement.Left:
                    offsetX = -popupOffset;
                    offsetY = (targetHeight - height) / 2;
                    placementMode = PlacementMode.Left;
                    break;
                case TipPlacement.LeftBottom:
                    offsetX = -popupOffset;
                    offsetY = targetHeight - height;
                    placementMode = PlacementMode.Left;
                    break;
                case TipPlacement.TopLeft:
                    offsetY = -popupOffset;
                    placementMode = PlacementMode.Top;
                    break;
                case TipPlacement.Top:
                    offsetX = (targetWidth - width) / 2;
                    offsetY = -popupOffset;
                    placementMode = PlacementMode.Top;
                    break;
                case TipPlacement.TopRight:
                    offsetX = targetWidth - width;
                    offsetY = -popupOffset;
                    placementMode = PlacementMode.Top;
                    break;
                case TipPlacement.RightTop:
                    offsetX = popupOffset;
                    placementMode = PlacementMode.Right;
                    break;
                case TipPlacement.Right:
                    offsetX = popupOffset;
                    offsetY = (targetHeight - height) / 2;
                    placementMode = PlacementMode.Right;
                    break;
                case TipPlacement.RightBottom:
                    offsetX = popupOffset;
                    offsetY = targetHeight - height;
                    placementMode = PlacementMode.Right;
                    break;
                case TipPlacement.BottomLeft:
                    offsetY = popupOffset;
                    placementMode = PlacementMode.Bottom;
                    break;
                case TipPlacement.Bottom:
                    offsetX = (targetWidth - width) / 2;
                    offsetY = popupOffset;
                    placementMode = PlacementMode.Bottom;
                    break;
                case TipPlacement.BottomRight:
                    offsetX = targetWidth - width;
                    offsetY = popupOffset;
                    placementMode = PlacementMode.Bottom;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _popup.HorizontalOffset = offsetX;
            _popup.VerticalOffset = offsetY;
            _popup.Placement = placementMode;
        }

        private void SwitchPoptip(bool isShow)
        {
            if (isShow)
            {
                if (!GetIsInstance(Target))
                {
                    SetCurrentValue(ContentProperty, GetContent(Target));
                    SetCurrentValue(PlacementProperty, GetPlacement(Target));
                    SetCurrentValue(HitModeProperty, GetHitMode(Target));
                    SetCurrentValue(OffsetProperty, GetOffset(Target));
                    SetCurrentValue(IsOpenProperty, GetIsOpen(Target));
                }

                _popup.PlacementTarget = Target;
                UpdateLocation();
            }
            _popup.IsOpen = isShow;
            Target.SetCurrentValue(IsOpenProperty, isShow);
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            var hitMode = GetIsInstance(Target) ? HitMode : GetHitMode(Target);
            if (hitMode != HitMode.Hover || hitMode == HitMode.None) return;

            SwitchPoptip(true);
        }

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            var hitMode = GetIsInstance(Target) ? HitMode : GetHitMode(Target);
            if (hitMode != HitMode.Hover || hitMode == HitMode.None) return;

            SwitchPoptip(false);
        }

        private void Element_GotFocus(object sender, RoutedEventArgs e)
        {
            var hitMode = GetIsInstance(Target) ? HitMode : GetHitMode(Target);
            if (hitMode != HitMode.Focus || hitMode == HitMode.None) return;

            SwitchPoptip(true);
        }

        private void Element_LostFocus(object sender, RoutedEventArgs e)
        {
            var hitMode = GetIsInstance(Target) ? HitMode : GetHitMode(Target);
            if (hitMode != HitMode.Focus || hitMode == HitMode.None) return;

            SwitchPoptip(false);
        }
    }
}
