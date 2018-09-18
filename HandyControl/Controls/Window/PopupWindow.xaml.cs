using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Tools;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    ///     弹出窗口
    /// </summary>
    public partial class PopupWindow
    {
        private bool IsDialog { get; set; }

        public PopupWindow()
        {
            Closing += (sender, args) =>
            {
                if (!IsDialog)
                    Owner?.Focus();
            };
            InitializeComponent();
            try
            {
                Owner = Application.Current.MainWindow;
            }
            catch
            {
                // ignored
            }
        }

        public PopupWindow(Window owner) : this()
        {
            Owner = owner;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public UIElement Child
        {
            get => MainBorder.Child;
            set
            {
                MainBorder.Child = value;
                if (value is FrameworkElement temp)
                {
                    TitleBlock.Width = temp.Width - 40;
                }
            }
        }

        public bool ShowTitle
        {
            set
            {
                if (!value)
                {
                    TitleGrid.Visibility = Visibility.Collapsed;
                    Background = Brushes.Transparent;
                }
                else
                {
                    TitleGrid.Visibility = Visibility.Visible;
                    Background = new SolidColorBrush(Color.FromRgb(1, 1, 1));
                }
            }
        }

        public static readonly DependencyProperty ShowCancelProperty = DependencyProperty.Register(
            "ShowCancel", typeof(bool), typeof(PopupWindow), new PropertyMetadata(default(bool)));


        public bool ShowCancel
        {
            get => (bool)GetValue(ShowCancelProperty);
            set => SetValue(ShowCancelProperty, value);
        }

        public bool ShowBorder
        {
            set => MainBorder.BorderThickness = value ? new Thickness(1, 0, 1, 1) : new Thickness(0);
        }

        private void TitleBlock_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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
                MyStackPanel = { Visibility = Visibility.Visible },
                MyTextBlock = { Text = message },
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            window.Background = window.FindResource("PrimaryBrush") as SolidColorBrush;
            window.Show();
        }

        public static bool? ShowDialog(string message, string title = "提示", bool showCancel = false)
        {
            var window = new PopupWindow
            {
                AllowsTransparency = true,
                MyStackPanel = { Visibility = Visibility.Visible },
                MyTextBlock = { Text = message },
                IsDialog = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowBorder = true,
                Title = title,
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
