using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Interactivity;
using HandyControl.Tools;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    ///     弹出窗口
    /// </summary>
    [TemplatePart(Name = ElementMainBorder, Type = typeof(Border))]
    [TemplatePart(Name = ElementTitleBlock, Type = typeof(TextBlock))]
    public class PopupWindow : Window
    {
        #region Constants

        private const string ElementMainBorder = "PART_MainBorder";

        private const string ElementTitleBlock = "PART_TitleBlock";

        #endregion Constants

        private Border _mainBorder;

        private TextBlock _titleBlock;

        public override void OnApplyTemplate()
        {
            if (_titleBlock != null)
            {
                _titleBlock.MouseLeftButtonDown -= TitleBlock_OnMouseLeftButtonDown;
            }

            base.OnApplyTemplate();

            _mainBorder = GetTemplateChild(ElementMainBorder) as Border;
            _titleBlock = GetTemplateChild(ElementTitleBlock) as TextBlock;

            if (_titleBlock != null)
            {
                _titleBlock.MouseLeftButtonDown += TitleBlock_OnMouseLeftButtonDown;
            }
        }

        internal static readonly DependencyProperty ContentStrProperty = DependencyProperty.Register(
            "ContentStr", typeof(string), typeof(PopupWindow), new PropertyMetadata(default(string)));

        internal string ContentStr
        {
            get => (string) GetValue(ContentStrProperty);
            set => SetValue(ContentStrProperty, value);
        }

        private bool IsDialog { get; set; }

        public PopupWindow()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, CloseButton_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, ButtonOk_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, ButtonCancle_OnClick));

            Closing += (sender, args) =>
            {
                if (!IsDialog)
                    Owner?.Focus();
            };
            try
            {
                Owner = Application.Current.MainWindow;
            }
            catch
            {
                // ignored
            }
        }

        public PopupWindow(Window owner) : this() => Owner = owner;

        private void CloseButton_OnClick(object sender, RoutedEventArgs e) => Close();

        public UIElement Child
        {
            get => _mainBorder?.Child;
            set
            {
                if (_mainBorder != null)
                {
                    _mainBorder.Child = value;
                }
            }
        }

        public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
            "ShowTitle", typeof(bool), typeof(PopupWindow), new PropertyMetadata(true));

        public bool ShowTitle
        {
            get => (bool) GetValue(ShowTitleProperty);
            set => SetValue(ShowTitleProperty, value);
        }

        public static readonly DependencyProperty ShowCancelProperty = DependencyProperty.Register(
            "ShowCancel", typeof(bool), typeof(PopupWindow), new PropertyMetadata(default(bool)));

        public bool ShowCancel
        {
            get => (bool)GetValue(ShowCancelProperty);
            set => SetValue(ShowCancelProperty, value);
        }

        public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register(
            "ShowBorder", typeof(bool), typeof(PopupWindow), new PropertyMetadata(default(bool)));

        public bool ShowBorder
        {
            get => (bool) GetValue(ShowBorderProperty);
            set => SetValue(ShowBorderProperty, value);
        }

        private void TitleBlock_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public void Show(FrameworkElement element)
        {
            var point = ArithmeticHelper.CalSafePoint(element, Child as FrameworkElement);
            Left = point.X;
            Top = point.Y;
            Show();
        }

        public void ShowDialog(FrameworkElement element)
        {
            var point = ArithmeticHelper.CalSafePoint(element, Child as FrameworkElement);
            Left = point.X;
            Top = point.Y;
            ShowDialog();
        }

        public void Show(Window element, Point point)
        {
            Left = element.Left + point.X;
            Top = element.Top + point.Y;
            Show();
        }

        public static void Show(string message)
        {
            var window = new PopupWindow
            {
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                ContentStr = message,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            window.Background = window.FindResource("PrimaryBrush") as SolidColorBrush;
            window.Show();
        }

        public static bool? ShowDialog(string message, string title = default(string), bool showCancel = false)
        {
            var window = new PopupWindow
            {
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                ContentStr = message,
                IsDialog = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowBorder = true,
                Title = string.IsNullOrEmpty(title) ? Properties.Langs.Lang.Tip : title,
                ShowCancel = showCancel
            };
            return window.ShowDialog();
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsDialog)
            {
                DialogResult = true;
            }
            Close();
        }

        private void ButtonCancle_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsDialog)
            {
                DialogResult = false;
            }
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Child = null;
        }
    }
}