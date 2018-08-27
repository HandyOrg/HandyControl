using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    ///     无边框窗口
    /// </summary>
    public class WindowBorderless : Window
    {
        private Thickness _tempThickness;

        public static readonly DependencyProperty NoUserContentProperty = DependencyProperty.Register(
            "NoUserContent", typeof(object), typeof(WindowBorderless), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty CloseButtonHoverBrushProperty = DependencyProperty.Register(
            "CloseButtonHoverBrush", typeof(Brush), typeof(WindowBorderless),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CloseButtonHoverForegroundProperty =
            DependencyProperty.Register(
                "CloseButtonHoverForeground", typeof(Brush), typeof(WindowBorderless),
                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CloseButtonForegroundProperty = DependencyProperty.Register(
            "CloseButtonForeground", typeof(Brush), typeof(WindowBorderless),
            new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty OtherButtonForegroundProperty = DependencyProperty.Register(
            "OtherButtonForeground", typeof(Brush), typeof(WindowBorderless),
            new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty OtherButtonHoverBrushProperty = DependencyProperty.Register(
            "OtherButtonHoverBrush", typeof(Brush), typeof(WindowBorderless),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty OtherButtonHoverForegroundProperty =
            DependencyProperty.Register(
                "OtherButtonHoverForeground", typeof(Brush), typeof(WindowBorderless),
                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty NoUserAreaBackgroundProperty = DependencyProperty.Register(
            "NoUserAreaBackground", typeof(Brush), typeof(WindowBorderless),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty NoUserAreaForegroundProperty = DependencyProperty.Register(
            "NoUserAreaForeground", typeof(Brush), typeof(WindowBorderless),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty NoUserAreaHeightProperty = DependencyProperty.Register(
            "NoUserAreaHeight", typeof(double), typeof(WindowBorderless),
            new PropertyMetadata(28.0));

        public static readonly DependencyProperty ShowNoUserAreaProperty = DependencyProperty.Register(
            "ShowNoUserArea", typeof(bool), typeof(WindowBorderless), new PropertyMetadata(true));

        public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
            "ShowTitle", typeof(bool), typeof(WindowBorderless), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.Register(
            "IsFullScreen", typeof(bool), typeof(WindowBorderless), new PropertyMetadata(default(bool),
                (o, args) =>
                {
                    var ctl = (WindowBorderless)o;
                    var v = (bool)args.NewValue;
                    if (v)
                    {
                        ctl.OriginState = ctl.WindowState;
                        ctl.OriginStyle = ctl.WindowStyle;
                        ctl.OriginResizeMode = ctl.ResizeMode;
                        ctl.WindowStyle = WindowStyle.None;
                        //下面三行不能改变，就是故意的
                        ctl.WindowState = WindowState.Maximized;
                        ctl.WindowState = WindowState.Minimized;
                        ctl.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        ctl.WindowState = ctl.OriginState;
                        ctl.WindowStyle = ctl.OriginStyle;
                        ctl.ResizeMode = ctl.OriginResizeMode;
                    }
                }));

        public WindowBorderless()
        {
            Loaded += delegate
            {
                _tempThickness = BorderThickness;
                if (WindowState == WindowState.Maximized)
                {
                    BorderThickness = new Thickness();
                }

                CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand,
                    (s, e) => WindowState = WindowState.Minimized));
                CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand,
                    (s, e) => WindowState = WindowState.Maximized));
                CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand,
                    (s, e) => WindowState = WindowState.Normal));
                CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (s, e) => Close()));
                CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
            };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (WindowState == WindowState.Maximized)
            {
                BorderThickness = new Thickness();
            }
            else if (WindowState == WindowState.Normal)
            {
                BorderThickness = _tempThickness;
            }
        }

        public Brush CloseButtonForeground
        {
            get => (Brush)GetValue(CloseButtonForegroundProperty);
            set => SetValue(CloseButtonForegroundProperty, value);
        }

        public Brush OtherButtonForeground
        {
            get => (Brush)GetValue(OtherButtonForegroundProperty);
            set => SetValue(OtherButtonForegroundProperty, value);
        }

        /// <summary>
        ///     原始状态
        /// </summary>
        private WindowState OriginState { get; set; }

        /// <summary>
        ///     原始样式
        /// </summary>
        private WindowStyle OriginStyle { get; set; }

        /// <summary>
        ///     原始尺寸调节模式
        /// </summary>
        private ResizeMode OriginResizeMode { get; set; }

        public double NoUserAreaHeight
        {
            get => (double)GetValue(NoUserAreaHeightProperty);
            set => SetValue(NoUserAreaHeightProperty, value);
        }

        public bool IsFullScreen
        {
            get => (bool)GetValue(IsFullScreenProperty);
            set => SetValue(IsFullScreenProperty, value);
        }

        public object NoUserContent
        {
            get => GetValue(NoUserContentProperty);
            set => SetValue(NoUserContentProperty, value);
        }

        public Brush CloseButtonHoverBrush
        {
            get => (Brush)GetValue(CloseButtonHoverBrushProperty);
            set => SetValue(CloseButtonHoverBrushProperty, value);
        }

        public Brush CloseButtonHoverForeground
        {
            get => (Brush)GetValue(CloseButtonHoverForegroundProperty);
            set => SetValue(CloseButtonHoverForegroundProperty, value);
        }

        public Brush OtherButtonHoverBrush
        {
            get => (Brush)GetValue(OtherButtonHoverBrushProperty);
            set => SetValue(OtherButtonHoverBrushProperty, value);
        }

        public Brush OtherButtonHoverForeground
        {
            get => (Brush)GetValue(OtherButtonHoverForegroundProperty);
            set => SetValue(OtherButtonHoverForegroundProperty, value);
        }

        public Brush NoUserAreaBackground
        {
            get => (Brush)GetValue(NoUserAreaBackgroundProperty);
            set => SetValue(NoUserAreaBackgroundProperty, value);
        }

        public Brush NoUserAreaForeground
        {
            get => (Brush)GetValue(NoUserAreaForegroundProperty);
            set => SetValue(NoUserAreaForegroundProperty, value);
        }

        public bool ShowNoUserArea
        {
            get => (bool)GetValue(ShowNoUserAreaProperty);
            set => SetValue(ShowNoUserAreaProperty, value);
        }

        public bool ShowTitle
        {
            get => (bool)GetValue(ShowTitleProperty);
            set => SetValue(ShowTitleProperty, value);
        }

        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            var point = WindowState == WindowState.Maximized
                ? new Point(0, 28)
                : new Point(Left, Top + 28);
            SystemCommands.ShowSystemMenu(this, point);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (SizeToContent == SizeToContent.WidthAndHeight)
                InvalidateMeasure();
        }

        /// <summary>
        ///     获取自定义窗口
        /// </summary>
        /// <returns></returns>
        public static WindowBorderless GetCustomWindow(FrameworkElement content)
        {
            var window = new WindowBorderless
            {
                Style = Application.Current.TryFindResource("CustomWindowStyle") as Style,
                Content = content
            };
            window.Loaded += (s, e) =>
            {
                window.Width = window.BorderThickness.Left + window.BorderThickness.Right + content.Width;
                if (!(window.Template.FindName("GridMenu", window) is Grid nemuArea))
                    throw new NullReferenceException("在样式中未找到名称为GridMenu的元素");
                window.Height = window.BorderThickness.Top + window.BorderThickness.Bottom + content.Height +
                                nemuArea.ActualHeight;
            };

            return window;
        }
    }
}