using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    [ContentProperty("Content")]
    public class Drawer : FrameworkElement
    {
        private Storyboard _storyboard;

        private AdornerContainer _container;

        private ContentControl _animationControl;

        private TranslateTransform _translateTransform;

        private double _animationLength;

        private string _animationPropertyName;

        private FrameworkElement _maskElement;

        private AdornerLayer _layer;

        private System.Windows.Window _window;

        private UIElement _windowContentElement;

        private Point _contentRenderTransformOrigin;

        static Drawer()
        {
            DataContextProperty.OverrideMetadata(typeof(Drawer), new FrameworkPropertyMetadata(DataContextPropertyChanged));
        }

        public Drawer()
        {
            Loaded += Drawer_Loaded;
            Unloaded += Drawer_Unloaded;
        }

        private void Drawer_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsOpen)
            {
                OnIsOpenChanged(true);
            }
        }

        private void Drawer_Unloaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Drawer_Loaded;
            
            if (_maskElement != null)
            {
                _maskElement.PreviewMouseLeftButtonDown -= MaskElement_PreviewMouseLeftButtonDown;
            }

            if (_storyboard != null)
            {
                _storyboard.Completed -= Storyboard_Completed;
            }
        }

        private static void DataContextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((Drawer)d).OnDataContextPropertyChanged(e);

        private void OnDataContextPropertyChanged(DependencyPropertyChangedEventArgs e) => UpdateDataContext(_animationControl, e.OldValue, e.NewValue);

        public static readonly RoutedEvent OpenedEvent =
            EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(Drawer));

        public event RoutedEventHandler Opened
        {
            add => AddHandler(OpenedEvent, value);
            remove => RemoveHandler(OpenedEvent, value);
        }

        public static readonly RoutedEvent ClosedEvent =
            EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(Drawer));

        public event RoutedEventHandler Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen", typeof(bool), typeof(Drawer), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsOpenChanged));

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (Drawer) d;
            ctl.OnIsOpenChanged((bool)e.NewValue);
        }

        public bool IsOpen
        {
            get => (bool) GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty MaskCanCloseProperty = DependencyProperty.Register(
            "MaskCanClose", typeof(bool), typeof(Drawer), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool MaskCanClose
        {
            get => (bool) GetValue(MaskCanCloseProperty);
            set => SetValue(MaskCanCloseProperty, value);
        }

        public static readonly DependencyProperty ShowMaskProperty = DependencyProperty.Register(
            "ShowMask", typeof(bool), typeof(Drawer), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool ShowMask
        {
            get => (bool) GetValue(ShowMaskProperty);
            set => SetValue(ShowMaskProperty, value);
        }

        public static readonly DependencyProperty DockProperty = DependencyProperty.Register(
            "Dock", typeof(Dock), typeof(Drawer), new PropertyMetadata(default(Dock)));

        public Dock Dock
        {
            get => (Dock) GetValue(DockProperty);
            set => SetValue(DockProperty, value);
        }

        public static readonly DependencyProperty ShowModeProperty = DependencyProperty.Register(
            "ShowMode", typeof(DrawerShowMode), typeof(Drawer), new PropertyMetadata(default(DrawerShowMode)));

        public DrawerShowMode ShowMode
        {
            get => (DrawerShowMode) GetValue(ShowModeProperty);
            set => SetValue(ShowModeProperty, value);
        }

        public static readonly DependencyProperty MaskBrushProperty = DependencyProperty.Register(
            "MaskBrush", typeof(Brush), typeof(Drawer), new PropertyMetadata(default(Brush)));

        public Brush MaskBrush
        {
            get => (Brush) GetValue(MaskBrushProperty);
            set => SetValue(MaskBrushProperty, value);
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(Drawer), new PropertyMetadata(default(object)));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        private void CreateContainer()
        {
            _storyboard = new Storyboard();
            _storyboard.Completed += Storyboard_Completed;

            _translateTransform = new TranslateTransform();
            _animationControl = new ContentControl
            {
                Content = Content,
                RenderTransform = _translateTransform,
                DataContext = this
            };

            var panel = new SimplePanel
            {
                ClipToBounds = true
            };

            if (ShowMask)
            {
                _maskElement = new Border
                {
                    Background = MaskBrush,
                    Opacity = 0
                };
                _maskElement.PreviewMouseLeftButtonDown += MaskElement_PreviewMouseLeftButtonDown;
                panel.Children.Add(_maskElement);
            }

            _animationControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var size = _animationControl.DesiredSize;

            switch (Dock)
            {
                case Dock.Left:
                    _animationControl.HorizontalAlignment = HorizontalAlignment.Left;
                    _animationControl.VerticalAlignment = VerticalAlignment.Stretch;
                    _translateTransform.X = -size.Width;
                    _animationLength = -size.Width;
                    _animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.X)";
                    break;
                case Dock.Top:
                    _animationControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _animationControl.VerticalAlignment = VerticalAlignment.Top;
                    _translateTransform.Y = -size.Height;
                    _animationLength = -size.Height;
                    _animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.Y)";
                    break;
                case Dock.Right:
                    _animationControl.HorizontalAlignment = HorizontalAlignment.Right;
                    _animationControl.VerticalAlignment = VerticalAlignment.Stretch;
                    _translateTransform.X = size.Width;
                    _animationLength = size.Width;
                    _animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.X)";
                    break;
                case Dock.Bottom:
                    _animationControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _animationControl.VerticalAlignment = VerticalAlignment.Bottom;
                    _translateTransform.Y = size.Height;
                    _animationLength = size.Height;
                    _animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.Y)";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _animationControl.DataContext = DataContext;
            _animationControl.CommandBindings.Clear();
            _animationControl.CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => SetCurrentValue(IsOpenProperty, ValueBoxes.FalseBox)));
            panel.Children.Add(_animationControl);
            _container = new AdornerContainer(_layer)
            {
                Child = panel,
                ClipToBounds = true
            };
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            if (!IsOpen)
            {
                _windowContentElement.SetCurrentValue(RenderTransformOriginProperty, _contentRenderTransformOrigin);
                _layer.Remove(_container);
                RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
            }
            else
            {
                RaiseEvent(new RoutedEventArgs(OpenedEvent, this));
            }
        }

        private void OnIsOpenChanged(bool isOpen)
        {
            if (Content == null) return;

            _window = WindowHelper.GetActiveWindow();
            if (_window == null) return;

            _windowContentElement = _window.Content as UIElement;
            if (_windowContentElement == null) return;

            _contentRenderTransformOrigin = _windowContentElement.RenderTransformOrigin;

            var decorator = VisualHelper.GetChild<AdornerDecorator>(_window);
            _layer = decorator?.AdornerLayer;
            if (_layer == null) return;

            if (_container == null)
            {
                CreateContainer();
            }

            switch (ShowMode)
            {
                case DrawerShowMode.Push:
                    ShowByPush(isOpen);
                    break;
                case DrawerShowMode.Press:
                    _windowContentElement.SetCurrentValue(RenderTransformOriginProperty, new Point(0.5, 0.5));
                    ShowByPress(isOpen);
                    break;
            }

            if (isOpen)
            {
                if (_maskElement != null)
                {
                    var maskAnimation = AnimationHelper.CreateAnimation(1);
                    Storyboard.SetTarget(maskAnimation, _maskElement);
                    Storyboard.SetTargetProperty(maskAnimation, new PropertyPath(OpacityProperty.Name));
                    _storyboard.Children.Add(maskAnimation);
                }

                var drawerAnimation = AnimationHelper.CreateAnimation(0);
                Storyboard.SetTarget(drawerAnimation, _animationControl);
                Storyboard.SetTargetProperty(drawerAnimation, new PropertyPath(_animationPropertyName));
                _storyboard.Children.Add(drawerAnimation);
                _layer.Remove(_container);
                _layer.Add(_container);
            }
            else
            {
                if (_maskElement != null)
                {
                    var maskAnimation = AnimationHelper.CreateAnimation(0);
                    Storyboard.SetTarget(maskAnimation, _maskElement);
                    Storyboard.SetTargetProperty(maskAnimation, new PropertyPath(OpacityProperty.Name));
                    _storyboard.Children.Add(maskAnimation);
                }

                var drawerAnimation = AnimationHelper.CreateAnimation(_animationLength);
                Storyboard.SetTarget(drawerAnimation, _animationControl);
                Storyboard.SetTargetProperty(drawerAnimation, new PropertyPath(_animationPropertyName));
                _storyboard.Children.Add(drawerAnimation);
            }
            _storyboard.Begin();
        }

        private void ShowByPush(bool isOpen)
        {
            string animationPropertyName;

            switch (Dock)
            {
                case Dock.Left:
                case Dock.Right:
                    animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.X)";
                    _windowContentElement.RenderTransform = new TranslateTransform
                    {
                        X = isOpen ? 0 : -_animationLength
                    };
                    break;
                case Dock.Top:
                case Dock.Bottom:
                    animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.Y)";
                    _windowContentElement.RenderTransform = new TranslateTransform
                    {
                        Y = isOpen ? 0 : -_animationLength
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var animation = isOpen
                ? AnimationHelper.CreateAnimation(-_animationLength)
                : AnimationHelper.CreateAnimation(0);
            Storyboard.SetTarget(animation, _windowContentElement);
            Storyboard.SetTargetProperty(animation, new PropertyPath(animationPropertyName));

            _storyboard.Children.Add(animation);
        }

        private void ShowByPress(bool isOpen)
        {
            _windowContentElement.RenderTransform = isOpen
                ? new ScaleTransform()
                : new ScaleTransform
                {
                    ScaleX = 0.9,
                    ScaleY = 0.9
                };

            var animationX = isOpen
                ? AnimationHelper.CreateAnimation(.9)
                : AnimationHelper.CreateAnimation(1);
            Storyboard.SetTarget(animationX, _windowContentElement);
            Storyboard.SetTargetProperty(animationX, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));

            _storyboard.Children.Add(animationX);

            var animationY = isOpen
                ? AnimationHelper.CreateAnimation(.9)
                : AnimationHelper.CreateAnimation(1);
            Storyboard.SetTarget(animationY, _windowContentElement);
            Storyboard.SetTargetProperty(animationY, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            _storyboard.Children.Add(animationY);
        }

        private void MaskElement_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MaskCanClose)
            {
                SetCurrentValue(IsOpenProperty, ValueBoxes.FalseBox);
            }
        }

        private void UpdateDataContext(FrameworkElement target, object oldValue, object newValue)
        {
            if (target == null || BindingOperations.GetBindingExpression(target, DataContextProperty) != null) return;
            if (ReferenceEquals(this, target.DataContext) || Equals(oldValue, target.DataContext))
            {
                target.DataContext = newValue ?? this;
            }
        }
    }
}
