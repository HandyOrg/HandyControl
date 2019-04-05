using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementIcon, Type = typeof(FrameworkElement))]
    public class RateItem : Control
    {
        private const string ElementIcon = "PART_Icon";

        public static readonly DependencyProperty AllowClearProperty =
            Rate.AllowClearProperty.AddOwner(typeof(RateItem));

        public static readonly DependencyProperty AllowHalfProperty =
            Rate.AllowHalfProperty.AddOwner(typeof(RateItem), new PropertyMetadata(OnAllowHalfChanged));

        public static readonly DependencyProperty IconProperty = Rate.IconProperty.AddOwner(typeof(RateItem));

        public static readonly DependencyProperty IsReadOnlyProperty = Rate.IsReadOnlyProperty.AddOwner(typeof(RateItem));

        internal static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(RateItem),
            new PropertyMetadata(ValueBoxes.FalseBox, OnIsSelectedChanged));

        public static readonly RoutedEvent SelectedChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(RateItem));

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(RateItem));

        private FrameworkElement _icon;

        private bool _isHalf;

        private bool _isLoaded;

        private bool _isMouseLeftButtonDown;

        private bool _isSentValue;

        public RateItem()
        {
            Loaded += (s, e) => _isLoaded = true;
        }

        public bool AllowClear
        {
            get => (bool) GetValue(AllowClearProperty);
            set => SetValue(AllowClearProperty, value);
        }

        public bool AllowHalf
        {
            get => (bool) GetValue(AllowHalfProperty);
            set => SetValue(AllowHalfProperty, value);
        }

        public Geometry Icon
        {
            get => (Geometry) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        internal bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        internal bool IsHalf
        {
            get => _isHalf;
            set
            {
                if (_isHalf == value) return;
                _isHalf = value;
                if (_icon == null) return;
                _icon.Width = value ? ActualWidth / 2 : ActualWidth;
            }
        }

        internal int Index { get; set; }

        private static void OnAllowHalfChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (RateItem) d;
            ctl.HandleMouseMoveEvent((bool) e.NewValue);
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (RateItem) d;
            ctl._icon?.Show((bool) e.NewValue);
        }

        public event RoutedEventHandler SelectedChanged
        {
            add => AddHandler(SelectedChangedEvent, value);
            remove => RemoveHandler(SelectedChangedEvent, value);
        }

        public event RoutedEventHandler ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        private void HandleMouseMoveEvent(bool handle)
        {
            if (handle)
            {
                MouseMove += RateItem_MouseMove;
            }
            else
            {
                MouseMove -= RateItem_MouseMove;
            }
        }

        private void RateItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsReadOnly) return;
            if (!AllowHalf) return;
            var p = e.GetPosition(this);
            IsHalf = p.X < ActualWidth / 2;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _icon = GetTemplateChild(ElementIcon) as FrameworkElement;

            if (_isLoaded)
            {
                if (_icon == null) return;
                _icon.Show(IsSelected);
                _icon.Width = IsHalf ? ActualWidth / 2 : ActualWidth;
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (IsReadOnly) return;
            _isSentValue = false;
            IsSelected = true;
            RaiseEvent(new RoutedEventArgs(SelectedChangedEvent) {Source = this});
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (IsReadOnly) return;
            _isMouseLeftButtonDown = true;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsReadOnly) return;
            _isMouseLeftButtonDown = false;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (IsReadOnly) return;
            if (_isMouseLeftButtonDown)
            {
                if (Index == 1 && AllowClear)
                {
                    if (IsSelected)
                    {
                        if (!_isSentValue)
                        {
                            RaiseEvent(new RoutedEventArgs(ValueChangedEvent) {Source = this});
                            _isMouseLeftButtonDown = false;
                            _isSentValue = true;
                            return;
                        }

                        _isSentValue = false;
                        IsSelected = false;
                        IsHalf = false;
                    }
                    else
                    {
                        IsSelected = true;
                        if (AllowHalf)
                        {
                            var p = e.GetPosition(this);
                            IsHalf = p.X < ActualWidth / 2;
                        }
                    }
                }

                RaiseEvent(new RoutedEventArgs(ValueChangedEvent) {Source = this});
                _isMouseLeftButtonDown = false;
            }
        }
    }
}