using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using HandyControl.Data;
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

        private int _waitTime = 6;

        /// <summary>
        ///     计数
        /// </summary>
        private int _tickCount;

        /// <summary>
        ///     关闭计时器
        /// </summary>
        private DispatcherTimer _timerClose;

        #endregion Data

        public Growl()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, ButtonClose_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, ButtonCancel_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, ButtonOk_OnClick));
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            _buttonClose.Show(_showCloseButton);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            _buttonClose.Collapse();
        }

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

        private Func<bool, bool> ActionBeforeClose { get; set; }

        /// <summary>
        ///     消息容器
        /// </summary>
        public static Panel GrowlPanel { get; set; }

        internal static readonly DependencyProperty CancelStrProperty = DependencyProperty.Register(
            "CancelStr", typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

        internal static readonly DependencyProperty ConfirmStrProperty = DependencyProperty.Register(
            "ConfirmStr", typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ShowDateTimeProperty = DependencyProperty.Register(
            "ShowDateTime", typeof(bool), typeof(Growl), new PropertyMetadata(ValueBoxes.TrueBox));

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(DateTime), typeof(Growl), new PropertyMetadata(default(DateTime)));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(Geometry), typeof(Growl), new PropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register(
            "IconBrush", typeof(Brush), typeof(Growl), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(InfoType), typeof(Growl), new PropertyMetadata(default(InfoType)));

        public static readonly DependencyProperty GrowlParentProperty = DependencyProperty.RegisterAttached(
            "GrowlParent", typeof(bool), typeof(Growl), new PropertyMetadata(ValueBoxes.FalseBox, (o, args) =>
            {
                if ((bool)args.NewValue && o is Panel panel)
                {
                    SetGrowlPanel(panel);
                }
            }));

        public static void SetGrowlParent(DependencyObject element, bool value) => element.SetValue(GrowlParentProperty, value);

        public static bool GetGrowlParent(DependencyObject element) => (bool)element.GetValue(GrowlParentProperty);

        public InfoType Type
        {
            get => (InfoType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        internal string CancelStr
        {
            get => (string)GetValue(CancelStrProperty);
            set => SetValue(CancelStrProperty, value);
        }

        internal string ConfirmStr
        {
            get => (string)GetValue(ConfirmStrProperty);
            set => SetValue(ConfirmStrProperty, value);
        }

        public bool ShowDateTime
        {
            get => (bool)GetValue(ShowDateTimeProperty);
            set => SetValue(ShowDateTimeProperty, value);
        }

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
                if (_tickCount >= _waitTime) Close();
            };
            _timerClose.Start();
        }

        /// <summary>
        ///     消息容器
        /// </summary>
        /// <param name="panel"></param>
        private static void SetGrowlPanel(Panel panel)
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

            PanelElement.SetFluidMoveBehavior(GrowlPanel, ResourceHelper.GetResource<FluidMoveBehavior>(ResourceToken.BehaviorXY400));
        }

        private void Update()
        {
            if (ActionBeforeClose != null)
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
        /// <param name="growlInfo"></param>
        private static void Show(GrowlInfo growlInfo)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var ctl = new Growl
                {
                    Message = growlInfo.Message,
                    Time = DateTime.Now,
                    Icon = ResourceHelper.GetResource<Geometry>(growlInfo.IconKey),
                    IconBrush = ResourceHelper.GetResource<Brush>(growlInfo.IconBrushKey),
                    _showCloseButton = growlInfo.ShowCloseButton,
                    ActionBeforeClose = growlInfo.ActionBeforeClose,
                    _staysOpen = growlInfo.StaysOpen,
                    ShowDateTime = growlInfo.ShowDateTime,
                    ConfirmStr = growlInfo.ConfirmStr,
                    CancelStr = growlInfo.CancelStr,
                    Type = growlInfo.Type,
                    _waitTime = Math.Max(growlInfo.WaitTime, 2)
                };
                GrowlPanel.Children.Insert(0, ctl);
            });
        }

        /// <summary>
        ///     成功
        /// </summary>
        /// <param name="message"></param>
        public static void Success(string message) => Success(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     成功
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Success(GrowlInfo growlInfo)
        {
            if (growlInfo == null) throw new ArgumentNullException(nameof(growlInfo));
            if (!growlInfo.IsCustom)
            {
                growlInfo.IconKey = ResourceToken.SuccessGeometry;
                growlInfo.IconBrushKey = ResourceToken.SuccessBrush;
                growlInfo.Type = InfoType.Success;
            }
            Show(growlInfo);
        }

        /// <summary>
        ///     消息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message) => Info(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     消息
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Info(GrowlInfo growlInfo)
        {
            if (growlInfo == null) throw new ArgumentNullException(nameof(growlInfo));
            if (!growlInfo.IsCustom)
            {
                growlInfo.IconKey = ResourceToken.InfoGeometry;
                growlInfo.IconBrushKey = ResourceToken.InfoBrush;
                growlInfo.Type = InfoType.Info;
            }
            Show(growlInfo);
        }

        /// <summary>
        ///     警告
        /// </summary>
        /// <param name="message"></param>
        public static void Warning(string message) => Warning(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     警告
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Warning(GrowlInfo growlInfo)
        {
            if (growlInfo == null) throw new ArgumentNullException(nameof(growlInfo));
            if (!growlInfo.IsCustom)
            {
                growlInfo.IconKey = ResourceToken.WarningGeometry;
                growlInfo.IconBrushKey = ResourceToken.WarningBrush;
                growlInfo.Type = InfoType.Warning;
            }
            Show(growlInfo);
        }

        /// <summary>
        ///     错误
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message) => Error(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     错误
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Error(GrowlInfo growlInfo)
        {
            if (growlInfo == null) throw new ArgumentNullException(nameof(growlInfo));
            if (!growlInfo.IsCustom)
            {
                growlInfo.IconKey = ResourceToken.ErrorGeometry;
                growlInfo.IconBrushKey = ResourceToken.DangerBrush;
                growlInfo.StaysOpen = true;
                growlInfo.Type = InfoType.Error;
            }
            Show(growlInfo);
        }

        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message) => Fatal(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Fatal(GrowlInfo growlInfo)
        {
            if (growlInfo == null) throw new ArgumentNullException(nameof(growlInfo));
            if (!growlInfo.IsCustom)
            {
                growlInfo.IconKey = ResourceToken.FatalGeometry;
                growlInfo.IconBrushKey = ResourceToken.PrimaryTextBrush;
                growlInfo.StaysOpen = true;
                growlInfo.ShowCloseButton = false;
                growlInfo.Type = InfoType.Fatal;
                if (GrowlPanel.ContextMenu != null) GrowlPanel.ContextMenu.Opacity = 0;
            }
            Show(growlInfo);
        }

        /// <summary>
        ///     询问
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionBeforeClose"></param>
        public static void Ask(string message, Func<bool, bool> actionBeforeClose) => Ask(new GrowlInfo
        {
            Message = message,
            ActionBeforeClose = actionBeforeClose
        });

        /// <summary>
        ///     询问
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Ask(GrowlInfo growlInfo)
        {
            if (growlInfo == null) throw new ArgumentNullException(nameof(growlInfo));
            if (!growlInfo.IsCustom)
            {
                growlInfo.IconKey = ResourceToken.AskGeometry;
                growlInfo.IconBrushKey = ResourceToken.AccentBrush;
                growlInfo.StaysOpen = true;
                growlInfo.ShowCloseButton = false;
                growlInfo.Type = InfoType.Ask;
            }
            Show(growlInfo);
        }

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
            animation.Completed += (s, e) =>
            {
                if (Parent is Panel panel)
                {
                    panel.Children.Remove(this);
                }
            };
            transform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        /// <summary>
        ///     清除
        /// </summary>
        public static void Clear()
        {
            GrowlPanel.Children.Clear();
            if (GrowlPanel.ContextMenu != null)
            {
                GrowlPanel.ContextMenu.IsOpen = false;
                GrowlPanel.ContextMenu.Opacity = 1;
            }
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            if (ActionBeforeClose?.Invoke(false) == true)
            {
                Close();
            }
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            if (ActionBeforeClose?.Invoke(true) == true)
            {
                Close();
            }
        }
    }
}