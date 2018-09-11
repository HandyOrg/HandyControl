using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    /// <summary>
    ///     Growl.xaml 的交互逻辑
    /// </summary>
    public partial class Growl
    {
        /// <summary>
        ///     最大计数
        /// </summary>
        private const int MaxTickCount = 6;

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(DateTime), typeof(Growl), new PropertyMetadata(default(DateTime)));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(Geometry), typeof(Growl), new PropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register(
            "IconBrush", typeof(Brush), typeof(Growl), new PropertyMetadata(default(Brush)));

        /// <summary>
        ///     消息容器
        /// </summary>
        private static Panel GrowlPanel;

        /// <summary>
        ///     计数
        /// </summary>
        private int _tickCount;

        /// <summary>
        ///     关闭计时器
        /// </summary>
        private DispatcherTimer _timerClose;

        public Growl() => InitializeComponent();

        private Action<Action, bool> CloseAction { get; set; }

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public DateTime Time
        {
            get => (DateTime)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public Brush IconBrush
        {
            get => (Brush)GetValue(IconBrushProperty);
            set => SetValue(IconBrushProperty, value);
        }

        /// <summary>
        ///     开始计时器
        /// </summary>
        private void StartTimer()
        {
            _timerClose = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timerClose.Tick += delegate
            {
                if (IsMouseOver)
                {
                    _tickCount = 0;
                    return;
                }

                _tickCount++;
                if (_tickCount >= MaxTickCount) Close();
            };
            _timerClose.Start();
        }

        /// <summary>
        ///     消息容器
        /// </summary>
        /// <param name="panel"></param>
        public static void SetGrowlPanel(Panel panel)
        {
            GrowlPanel = panel;
            var menuItem = new MenuItem
            {
                Header = Properties.Langs.Lang.Clear
            };
            menuItem.Click += (s, e) =>
            {
                foreach (var item in GrowlPanel.Children.OfType<Growl>())
                {
                    item.Close();
                }
            };
            GrowlPanel.ContextMenu = new ContextMenu
            {
                Items =
                {
                    menuItem
                }
            };
            var behavior = new FluidMoveBehavior
            {
                AppliesTo = FluidMoveScope.Children,
                Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                EaseY = new PowerEase()
            };
            var collection = Interaction.GetBehaviors(GrowlPanel);
            collection.Add(behavior);
        }

        /// <summary>
        ///     显示信息
        /// </summary>
        private static void Show(string message, string iconKey, string iconBrushKey, Action<Action, bool> closeAction = null,
            bool staysOpen = false, bool showCloseButton = true)
        {
            var ctl = new Growl
            {
                Message = message,
                Time = DateTime.Now,
                Icon = ResourceHelper.GetResource<Geometry>(iconKey),
                IconBrush = ResourceHelper.GetResource<Brush>(iconBrushKey)
            };
            if (!showCloseButton) ctl.Triggers.Clear();

            if (closeAction != null)
            {
                staysOpen = true;
                ctl.Triggers.Clear();
                ctl.PanelMore.IsEnabled = true;
                ctl.PanelMore.Show();
                ctl.CloseAction = closeAction;
            }

            var transform = new TranslateTransform
            {
                X = ctl.MaxWidth
            };
            ctl.GridMain.RenderTransform = transform;
            GrowlPanel.Children.Insert(0, ctl);
            transform.BeginAnimation(TranslateTransform.XProperty, AnimationHelper.CreateAnimation(0));
            if (!staysOpen) ctl.StartTimer();
        }

        /// <summary>
        ///     成功
        /// </summary>
        public static void Success(string message) => Show(message, "SuccessGeometry", "SuccessBrush");

        /// <summary>
        ///     消息
        /// </summary>
        public static void Info(string message) => Show(message, "InfoGeometry", "InfoBrush");

        /// <summary>
        ///     错误
        /// </summary>
        public static void Error(string message) => Show(message, "ErrorGeometry", "DangerBrush", null, true);

        /// <summary>
        ///     警告
        /// </summary>
        public static void Warning(string message) => Show(message, "WarningGeometry", "WarningBrush");

        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="message"></param>
        /// <param name="showContextMenu"></param>
        public static void Fatal(string message, bool showContextMenu = true)
        {
            if (!showContextMenu)
            {
                GrowlPanel.ContextMenu.Collapse();
            }
            Show(message, "FatalGeometry", "PrimaryTextBrush", null, true, false);
        }

        public static void Ask(string message, Action<Action, bool> closeAction) => Show(message, "AskGeometry", "AccentBrush", closeAction);

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        ///     关闭
        /// </summary>
        private void Close()
        {
            _timerClose?.Stop();
            var transform = new TranslateTransform();
            GridMain.RenderTransform = transform;
            var animation = AnimationHelper.CreateAnimation(MaxWidth);
            animation.Completed += delegate { GrowlPanel.Children.Remove(this); };
            transform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        /// <summary>
        ///     清除
        /// </summary>
        public static void Clear()
        {
            GrowlPanel.Children.Clear();
            GrowlPanel.Show();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => CloseAction?.Invoke(Close, false);

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e) => CloseAction?.Invoke(Close, true);
    }
}