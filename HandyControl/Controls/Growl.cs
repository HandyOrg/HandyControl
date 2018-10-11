using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    /// <summary>
    ///     消息提醒
    /// </summary>
    [TemplatePart(Name = ElementPanelMore, Type = typeof(Panel))]
    [TemplatePart(Name = ElementGridMain, Type = typeof(Grid))]
    [TemplatePart(Name = ElementButtonClose, Type = typeof(Button))]
    public class Growl : Control
    {
        #region Constants

        private const string ElementPanelMore = "PART_PanelMore";
        private const string ElementGridMain = "PART_GridMain";
        private const string ElementButtonClose = "PART_ButtonClose";

        #endregion Constants

        #region Data

        private Panel _panelMore;

        private Grid _gridMain;

        private Button _buttonClose;

        private bool _showCloseButton;

        private bool _staysOpen;

        #endregion Data

        public Growl()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, ButtonClose_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, ButtonCancel_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, ButtonOk_OnClick));

            MouseEnter += Growl_MouseEnter;
            MouseLeave += Growl_MouseLeave;
        }

        private void Growl_MouseLeave(object sender, MouseEventArgs e) => _buttonClose.Collapse();

        private void Growl_MouseEnter(object sender, MouseEventArgs e) => _buttonClose.Show(_showCloseButton);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _panelMore = GetTemplateChild(ElementPanelMore) as Panel;
            _gridMain = GetTemplateChild(ElementGridMain) as Grid;
            _buttonClose = GetTemplateChild(ElementButtonClose) as Button;

            CheckNull();
            Update();
        }

        private void CheckNull()
        {
            if (_panelMore == null || _gridMain == null || _buttonClose == null) throw new Exception();
        }

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

        private void Update()
        {
            if (CloseAction != null)
            {
                _staysOpen = true;
                _showCloseButton = false;
                _panelMore.IsEnabled = true;
                _panelMore.Show();
            }

            var transform = new TranslateTransform
            {
                X = MaxWidth
            };
            _gridMain.RenderTransform = transform;
            transform.BeginAnimation(TranslateTransform.XProperty, AnimationHelper.CreateAnimation(0));
            if (!_staysOpen) StartTimer();
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
                IconBrush = ResourceHelper.GetResource<Brush>(iconBrushKey),
                _showCloseButton = showCloseButton,
                CloseAction = closeAction,
                _staysOpen = staysOpen
            };
            GrowlPanel.Children.Insert(0, ctl);
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
            _gridMain.RenderTransform = transform;
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