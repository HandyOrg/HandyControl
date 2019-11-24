using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    [ContentProperty("Content")]
    public class Drawer : FrameworkElement
    {
        private AdornerContainer _container;

        private ContentControl _animateControl;

        private TranslateTransform _translateTransform;

        private double _animationLength;

        private DependencyProperty _animationProperty;

        private FrameworkElement _maskElement;

        private AdornerLayer _layer;

        public Drawer()
        {
            Loaded += Drawer_Loaded;
            Unloaded += Drawer_Unloaded;

            CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => SetCurrentValue(IsOpenProperty, ValueBoxes.FalseBox)));
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

        private void OnIsOpenChanged(bool isOpen)
        {
            if (Content == null) return;

            var window = WindowHelper.GetActiveWindow();
            if (window == null) return;

            var decorator = VisualHelper.GetChild<AdornerDecorator>(window);
            _layer = decorator?.AdornerLayer;
            if (_layer == null) return;

            if (_container == null)
            {
                CreateContainer();
            }

            switch (ShowMode)
            {
                case DrawerShowMode.Cover:
                    ShowByCover(isOpen);
                    break;
                case DrawerShowMode.Push:
                    ShowByPush(isOpen);
                    break;
                case DrawerShowMode.Press:
                    ShowByPress(isOpen);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ShowByCover(bool isOpen)
        {
            if (isOpen)
            {
                _maskElement?.BeginAnimation(OpacityProperty, AnimationHelper.CreateAnimation(1));
                _translateTransform.BeginAnimation(_animationProperty, AnimationHelper.CreateAnimation(0));
                _layer.Add(_container);
            }
            else
            {
                var animation = AnimationHelper.CreateAnimation(_animationLength);
                animation.Completed += (s, e) => _layer.Remove(_container);
                _maskElement?.BeginAnimation(OpacityProperty, AnimationHelper.CreateAnimation(0));
                _translateTransform.BeginAnimation(_animationProperty, animation);
            }
        }

        private void ShowByPush(bool isOpen)
        {
            if (isOpen)
            {
                
            }
            else
            {
                
            }
        }

        private void ShowByPress(bool isOpen)
        {
            if (isOpen)
            {

            }
            else
            {

            }
        }

        private void CreateContainer()
        {
            _translateTransform = new TranslateTransform();
            _animateControl = new ContentControl
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

            _animateControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var size = _animateControl.DesiredSize;

            switch (Dock)
            {
                case Dock.Left:
                    _animateControl.HorizontalAlignment = HorizontalAlignment.Left;
                    _animateControl.VerticalAlignment = VerticalAlignment.Stretch;
                    _translateTransform.X = -size.Width;
                    _animationLength = -size.Width;
                    _animationProperty = TranslateTransform.XProperty;
                    break;
                case Dock.Top:
                    _animateControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _animateControl.VerticalAlignment = VerticalAlignment.Top;
                    _translateTransform.Y = -size.Height;
                    _animationLength = -size.Height;
                    _animationProperty = TranslateTransform.YProperty;
                    break;
                case Dock.Right:
                    _animateControl.HorizontalAlignment = HorizontalAlignment.Right;
                    _animateControl.VerticalAlignment = VerticalAlignment.Stretch;
                    _translateTransform.X = size.Width;
                    _animationLength = size.Width;
                    _animationProperty = TranslateTransform.XProperty;
                    break;
                case Dock.Bottom:
                    _animateControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _animateControl.VerticalAlignment = VerticalAlignment.Bottom;
                    _translateTransform.Y = size.Height;
                    _animationLength = size.Height;
                    _animationProperty = TranslateTransform.YProperty;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            panel.Children.Add(_animateControl);
            _container = new AdornerContainer(_layer)
            {
                Child = panel,
                ClipToBounds = true
            };
        }

        private void MaskElement_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MaskCanClose)
            {
                SetCurrentValue(IsOpenProperty, ValueBoxes.FalseBox);
            }
        }
    }
}
