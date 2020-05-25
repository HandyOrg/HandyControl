using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
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

        private static GrowlWindow GrowlWindow;

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

        private static readonly Dictionary<string, Panel> PanelDic = new Dictionary<string, Panel>();

        #endregion Data

        public Growl()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, ButtonClose_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, ButtonCancel_OnClick));
            CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, ButtonOk_OnClick));
        }

        public static void Register(string token, Panel panel)
        {
            if (string.IsNullOrEmpty(token) || panel == null) return;
            PanelDic[token] = panel;
            InitGrowlPanel(panel);
        }

        public static void Unregister(string token, Panel panel)
        {
            if (string.IsNullOrEmpty(token) || panel == null) return;

            if (PanelDic.ContainsKey(token))
            {
                if (ReferenceEquals(PanelDic[token], panel))
                {
                    PanelDic.Remove(token);
                    panel.ContextMenu = null;
                    panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
                }
            }
        }

        public static void Unregister(Panel panel)
        {
            if (panel == null) return;
            var first = PanelDic.FirstOrDefault(item => ReferenceEquals(panel, item.Value));
            if (!string.IsNullOrEmpty(first.Key))
            {
                PanelDic.Remove(first.Key);
                panel.ContextMenu = null;
                panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
            }
        }

        public static void Unregister(string token)
        {
            if (string.IsNullOrEmpty(token)) return;

            if (PanelDic.ContainsKey(token))
            {
                var panel = PanelDic[token];
                PanelDic.Remove(token);
                panel.ContextMenu = null;
                panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
            }
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

        public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached(
            "Token", typeof(string), typeof(Growl), new PropertyMetadata(default(string), OnTokenChanged));

        private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Panel panel)
            {
                if (e.NewValue == null)
                {
                    Unregister(panel);
                }
                else
                {
                    Register(e.NewValue.ToString(), panel);
                }
            }
        }

        public static void SetToken(DependencyObject element, string value)
            => element.SetValue(TokenProperty, value);

        public static string GetToken(DependencyObject element)
            => (string)element.GetValue(TokenProperty);

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
                if (_tickCount >= _waitTime) Close(true);
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
            InitGrowlPanel(panel);
        }

        private static void InitGrowlPanel(Panel panel)
        {
            if (panel == null) return;

            var menuItem = new MenuItem();
            LangProvider.SetLang(menuItem, HeaderedItemsControl.HeaderProperty, LangKeys.Clear);

            menuItem.Click += (s, e) =>
            {
                foreach (var item in panel.Children.OfType<Growl>())
                {
                    item.Close();
                }
            };
            panel.ContextMenu = new ContextMenu
            {
                Items =
                {
                    menuItem
                }
            };

            PanelElement.SetFluidMoveBehavior(panel, ResourceHelper.GetResource<FluidMoveBehavior>(ResourceToken.BehaviorXY400));
        }

        private void Update()
        {
            if (Type == InfoType.Ask)
            {
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

        private static void ShowGlobal(GrowlInfo growlInfo)
        {
            if (GrowlWindow == null)
            {
                GrowlWindow = new GrowlWindow();
                GrowlWindow.Show();
                InitGrowlPanel(GrowlWindow.GrowlPanel);
                GrowlWindow.Init();
            }

            GrowlWindow.Show(true);

            Application.Current.Dispatcher?.Invoke(
#if NET40
                new Action(
#endif
                    () =>
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
                            GrowlWindow.GrowlPanel.Children.Insert(0, ctl);
                        }
#if NET40
                    )
#endif
                );
        }

        /// <summary>
        ///     显示信息
        /// </summary>
        /// <param name="growlInfo"></param>
        private static void Show(GrowlInfo growlInfo)
        {
            Application.Current.Dispatcher?.Invoke(
#if NET40
                new Action(
#endif                    
                    () =>
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
                            if (!string.IsNullOrEmpty(growlInfo.Token))
                            {
                                if (PanelDic.TryGetValue(growlInfo.Token, out var panel))
                                {
                                    panel?.Children.Insert(0, ctl);
                                }
                            }
                            else
                            {
                                GrowlPanel?.Children.Insert(0, ctl);
                            }
                        }
#if NET40
                    )
#endif
                );
        }

        private static void InitGrowlInfo(ref GrowlInfo growlInfo, InfoType infoType)
        {
            if (growlInfo == null) throw new ArgumentNullException(nameof(growlInfo));
            growlInfo.Type = infoType;

            switch (infoType)
            {
                case InfoType.Success:
                    if (!growlInfo.IsCustom)
                    {
                        growlInfo.IconKey = ResourceToken.SuccessGeometry;
                        growlInfo.IconBrushKey = ResourceToken.SuccessBrush;
                    }
                    else
                    {
                        growlInfo.IconKey ??= ResourceToken.SuccessGeometry;
                        growlInfo.IconBrushKey ??= ResourceToken.SuccessBrush;
                    }
                    break;
                case InfoType.Info:
                    if (!growlInfo.IsCustom)
                    {
                        growlInfo.IconKey = ResourceToken.InfoGeometry;
                        growlInfo.IconBrushKey = ResourceToken.InfoBrush;
                    }
                    else
                    {
                        growlInfo.IconKey ??= ResourceToken.InfoGeometry;
                        growlInfo.IconBrushKey ??= ResourceToken.InfoBrush;
                    }
                    break;
                case InfoType.Warning:
                    if (!growlInfo.IsCustom)
                    {
                        growlInfo.IconKey = ResourceToken.WarningGeometry;
                        growlInfo.IconBrushKey = ResourceToken.WarningBrush;
                    }
                    else
                    {
                        growlInfo.IconKey ??= ResourceToken.WarningGeometry;
                        growlInfo.IconBrushKey ??= ResourceToken.WarningBrush;
                    }
                    break;
                case InfoType.Error:
                    if (!growlInfo.IsCustom)
                    {
                        growlInfo.IconKey = ResourceToken.ErrorGeometry;
                        growlInfo.IconBrushKey = ResourceToken.DangerBrush;
                        growlInfo.StaysOpen = true;
                    }
                    else
                    {
                        growlInfo.IconKey ??= ResourceToken.ErrorGeometry;
                        growlInfo.IconBrushKey ??= ResourceToken.DangerBrush;
                    }
                    break;
                case InfoType.Fatal:
                    if (!growlInfo.IsCustom)
                    {
                        growlInfo.IconKey = ResourceToken.FatalGeometry;
                        growlInfo.IconBrushKey = ResourceToken.PrimaryTextBrush;
                        growlInfo.StaysOpen = true;
                        growlInfo.ShowCloseButton = false;
                        if (GrowlPanel.ContextMenu != null) GrowlPanel.ContextMenu.Opacity = 0;
                    }
                    else
                    {
                        growlInfo.IconKey ??= ResourceToken.FatalGeometry;
                        growlInfo.IconBrushKey ??= ResourceToken.PrimaryTextBrush;
                    }
                    break;
                case InfoType.Ask:
                    growlInfo.StaysOpen = true;
                    growlInfo.ShowCloseButton = false;
                    if (!growlInfo.IsCustom)
                    {
                        growlInfo.IconKey = ResourceToken.AskGeometry;
                        growlInfo.IconBrushKey = ResourceToken.AccentBrush;
                    }
                    else
                    {
                        growlInfo.IconKey ??= ResourceToken.AskGeometry;
                        growlInfo.IconBrushKey ??= ResourceToken.AccentBrush;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(infoType), infoType, null);
            }
        }

        /// <summary>
        ///     成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        public static void Success(string message, string token = "") => Success(new GrowlInfo
        {
            Message = message,
            Token = token
        });

        /// <summary>
        ///     成功
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Success(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Success);
            Show(growlInfo);
        }

        /// <summary>
        ///     成功
        /// </summary>
        /// <param name="message"></param>
        public static void SuccessGlobal(string message) => SuccessGlobal(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     成功
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void SuccessGlobal(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Success);
            ShowGlobal(growlInfo);
        }

        /// <summary>
        ///     消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        public static void Info(string message, string token = "") => Info(new GrowlInfo
        {
            Message = message,
            Token = token
        });

        /// <summary>
        ///     消息
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Info(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Info);
            Show(growlInfo);
        }

        /// <summary>
        ///     消息
        /// </summary>
        /// <param name="message"></param>
        public static void InfoGlobal(string message) => InfoGlobal(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     消息
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void InfoGlobal(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Info);
            ShowGlobal(growlInfo);
        }

        /// <summary>
        ///     警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        public static void Warning(string message, string token = "") => Warning(new GrowlInfo
        {
            Message = message,
            Token = token
        });

        /// <summary>
        ///     警告
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Warning(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Warning);
            Show(growlInfo);
        }

        /// <summary>
        ///     警告
        /// </summary>
        /// <param name="message"></param>
        public static void WarningGlobal(string message) => WarningGlobal(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     警告
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void WarningGlobal(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Warning);
            ShowGlobal(growlInfo);
        }

        /// <summary>
        ///     错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        public static void Error(string message, string token = "") => Error(new GrowlInfo
        {
            Message = message,
            Token = token
        });

        /// <summary>
        ///     错误
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Error(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Error);
            Show(growlInfo);
        }

        /// <summary>
        ///     错误
        /// </summary>
        /// <param name="message"></param>
        public static void ErrorGlobal(string message) => ErrorGlobal(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     错误
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void ErrorGlobal(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Error);
            ShowGlobal(growlInfo);
        }

        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        public static void Fatal(string message, string token = "") => Fatal(new GrowlInfo
        {
            Message = message,
            Token = token
        });

        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Fatal(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Fatal);
            Show(growlInfo);
        }

        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="message"></param>
        public static void FatalGlobal(string message) => FatalGlobal(new GrowlInfo
        {
            Message = message
        });

        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void FatalGlobal(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Fatal);
            ShowGlobal(growlInfo);
        }

        /// <summary>
        ///     询问
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionBeforeClose"></param>
        /// <param name="token"></param>
        public static void Ask(string message, Func<bool, bool> actionBeforeClose, string token = "") => Ask(new GrowlInfo
        {
            Message = message,
            ActionBeforeClose = actionBeforeClose,
            Token = token
        });

        /// <summary>
        ///     询问
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void Ask(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Ask);
            Show(growlInfo);
        }

        /// <summary>
        ///     询问
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionBeforeClose"></param>
        public static void AskGlobal(string message, Func<bool, bool> actionBeforeClose) => AskGlobal(new GrowlInfo
        {
            Message = message,
            ActionBeforeClose = actionBeforeClose
        });

        /// <summary>
        ///     询问
        /// </summary>
        /// <param name="growlInfo"></param>
        public static void AskGlobal(GrowlInfo growlInfo)
        {
            InitGrowlInfo(ref growlInfo, InfoType.Ask);
            ShowGlobal(growlInfo);
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        ///     关闭
        /// </summary>
        private void Close(bool invokeActionBeforeClose = false, bool invokeParam = true)
        {
            if (invokeActionBeforeClose)
            {
                if (ActionBeforeClose?.Invoke(invokeParam) == false) return;
            }

            _timerClose?.Stop();
            var transform = new TranslateTransform();
            _gridMain.RenderTransform = transform;
            var animation = AnimationHelper.CreateAnimation(ActualWidth);
            animation.Completed += (s, e) =>
            {
                if (Parent is Panel panel)
                {
                    panel.Children.Remove(this);

                    if (GrowlWindow?.GrowlPanel != null && GrowlWindow.GrowlPanel.Children.Count == 0)
                    {
                        GrowlWindow.Close();
                        GrowlWindow = null;
                    }
                }
            };
            transform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        /// <summary>
        ///     清除
        /// </summary>
        /// <param name="token"></param>
        public static void Clear(string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (PanelDic.TryGetValue(token, out var panel))
                {
                    Clear(panel);
                }
            }
            else
            {
                Clear(GrowlPanel);
            }
        }

        /// <summary>
        ///     清除
        /// </summary>
        /// <param name="panel"></param>
        private static void Clear(Panel panel)
        {
            if (panel == null) return;
            panel.Children.Clear();
            if (panel.ContextMenu != null)
            {
                panel.ContextMenu.IsOpen = false;
                panel.ContextMenu.Opacity = 1;
            }
        }

        /// <summary>
        ///     清除
        /// </summary>
        public static void ClearGlobal()
        {
            if (GrowlWindow == null) return;
            Clear(GrowlWindow.GrowlPanel);
            GrowlWindow.Close();
            GrowlWindow = null;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => Close(true, false);

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e) => Close(true);
    }
}