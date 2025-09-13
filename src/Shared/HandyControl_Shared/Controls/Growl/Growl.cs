using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

/// <summary>
///     消息提醒
/// </summary>
[TemplatePart(Name = ElementPanelMore, Type = typeof(Panel))]
[TemplatePart(Name = ElementGridMain, Type = typeof(Grid))]
[TemplatePart(Name = ElementButtonClose, Type = typeof(Button))]
public class Growl : Control
{
    private const string ElementPanelMore = "PART_PanelMore";
    private const string ElementGridMain = "PART_GridMain";
    private const string ElementButtonClose = "PART_ButtonClose";
    private const int MinWaitTime = 2;
    private const int TranslateTransformIndex = 3;

    private static GrowlWindow GrowlWindow;

    private static readonly ControlTokenManager<Panel> TokenManager =
        new(registerCallback: OnTokenRegistered, unregisterCallback: OnTokenUnregistered);

    public static readonly DependencyProperty GrowlParentProperty = DependencyProperty.RegisterAttached(
        "GrowlParent", typeof(bool), typeof(Growl), new PropertyMetadata(ValueBoxes.FalseBox, (o, args) =>
        {
            if ((bool) args.NewValue && o is Panel panel)
            {
                SetGrowlPanel(panel);
            }
        }));

    public static readonly DependencyProperty ShowModeProperty = DependencyProperty.RegisterAttached(
        "ShowMode", typeof(GrowlShowMode), typeof(Growl),
        new FrameworkPropertyMetadata(default(GrowlShowMode), FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty TransitionModeProperty = DependencyProperty.RegisterAttached(
        "TransitionMode", typeof(TransitionMode), typeof(Growl),
        new FrameworkPropertyMetadata(default(TransitionMode), FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty TransitionStoryboardProperty = DependencyProperty.RegisterAttached(
        "TransitionStoryboard", typeof(Storyboard), typeof(Growl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty ShowDateTimeProperty = DependencyProperty.Register(
        nameof(ShowDateTime), typeof(bool), typeof(Growl), new PropertyMetadata(ValueBoxes.TrueBox));

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        nameof(Message), typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
        nameof(Time), typeof(DateTime), typeof(Growl), new PropertyMetadata(default(DateTime)));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon), typeof(Geometry), typeof(Growl), new PropertyMetadata(default(Geometry)));

    public static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register(
        nameof(IconBrush), typeof(Brush), typeof(Growl), new PropertyMetadata(default(Brush)));

    public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
        nameof(Type), typeof(InfoType), typeof(Growl), new PropertyMetadata(default(InfoType)));

    public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached(
        "Token", typeof(string), typeof(Growl), new PropertyMetadata(null, TokenManager.OnTokenChanged));

    internal static readonly DependencyProperty CancelStrProperty = DependencyProperty.Register(
        nameof(CancelStr), typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

    internal static readonly DependencyProperty ConfirmStrProperty = DependencyProperty.Register(
        nameof(ConfirmStr), typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

    private static readonly DependencyProperty IsCreatedAutomaticallyProperty = DependencyProperty.RegisterAttached(
        "IsCreatedAutomatically", typeof(bool), typeof(Growl), new PropertyMetadata(ValueBoxes.FalseBox));

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

    /// <summary>
    ///     消息容器
    /// </summary>
    public static Panel GrowlPanel { get; set; }

    public InfoType Type
    {
        get => (InfoType) GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public bool ShowDateTime
    {
        get => (bool) GetValue(ShowDateTimeProperty);
        set => SetValue(ShowDateTimeProperty, ValueBoxes.BooleanBox(value));
    }

    public string Message
    {
        get => (string) GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public DateTime Time
    {
        get => (DateTime) GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public Geometry Icon
    {
        get => (Geometry) GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Brush IconBrush
    {
        get => (Brush) GetValue(IconBrushProperty);
        set => SetValue(IconBrushProperty, value);
    }

    internal string CancelStr
    {
        get => (string) GetValue(CancelStrProperty);
        set => SetValue(CancelStrProperty, value);
    }

    internal string ConfirmStr
    {
        get => (string) GetValue(ConfirmStrProperty);
        set => SetValue(ConfirmStrProperty, value);
    }

    private Func<bool, bool> ActionBeforeClose { get; set; }

    public Growl()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Close, ButtonClose_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, ButtonCancel_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, ButtonOk_OnClick));
    }

    private static void OnTokenRegistered(string token, Panel panel)
    {
        InitGrowlPanel(panel);
    }

    private static void OnTokenUnregistered(string token, Panel panel)
    {
        panel.ContextMenu = null;
        panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
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
        this.Hide();
        Dispatcher.BeginInvoke(DispatcherPriority.Loaded, () =>
        {
            Update();
            this.Show();
        });
    }

    private void CheckNull()
    {
        if (_panelMore == null || _gridMain == null || _buttonClose == null) throw new Exception();
    }

    public static void SetToken(DependencyObject element, string value) => element.SetValue(TokenProperty, value);

    public static string GetToken(DependencyObject element) => (string) element.GetValue(TokenProperty);

    public static void SetShowMode(DependencyObject element, GrowlShowMode value) =>
        element.SetValue(ShowModeProperty, value);

    public static GrowlShowMode GetShowMode(DependencyObject element) =>
        (GrowlShowMode) element.GetValue(ShowModeProperty);

    public static void SetTransitionMode(DependencyObject element, TransitionMode value)
        => element.SetValue(TransitionModeProperty, value);

    public static TransitionMode GetTransitionMode(DependencyObject element)
        => (TransitionMode) element.GetValue(TransitionModeProperty);

    public static void SetTransitionStoryboard(DependencyObject element, Storyboard value)
        => element.SetValue(TransitionStoryboardProperty, value);

    public static Storyboard GetTransitionStoryboard(DependencyObject element)
        => (Storyboard) element.GetValue(TransitionStoryboardProperty);

    public static void SetGrowlParent(DependencyObject element, bool value) =>
        element.SetValue(GrowlParentProperty, ValueBoxes.BooleanBox(value));

    public static bool GetGrowlParent(DependencyObject element) => (bool) element.GetValue(GrowlParentProperty);

    private static void SetIsCreatedAutomatically(DependencyObject element, bool value) =>
        element.SetValue(IsCreatedAutomaticallyProperty, ValueBoxes.BooleanBox(value));

    private static bool GetIsCreatedAutomatically(DependencyObject element) =>
        (bool) element.GetValue(IsCreatedAutomaticallyProperty);

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
            if (_tickCount >= _waitTime)
            {
                Close(true);
            }
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
                item.Close(invokeParam: false, isClear: true);
            }
        };
        panel.ContextMenu = new ContextMenu
        {
            Items =
            {
                menuItem
            }
        };

        PanelElement.SetFluidMoveBehavior(panel,
            ResourceHelper.GetResourceInternal<FluidMoveBehavior>(ResourceToken.BehaviorXY400));
    }

    private void Update()
    {
        if (DesignerHelper.IsInDesignMode) return;

        if (Type == InfoType.Ask)
        {
            _panelMore.IsEnabled = true;
            _panelMore.Show();
        }

        StartTransition(false);

        if (!_staysOpen)
        {
            StartTimer();
        }
    }

    private static void ShowInternal(Panel panel, Growl growl)
    {
        if (panel is null)
        {
            return;
        }

        if (GetShowMode(panel) == GrowlShowMode.Prepend)
        {
            panel.Children.Insert(0, growl);
        }
        else
        {
            panel.Children.Add(growl);
        }
    }

    private static void ShowGlobal(GrowlInfo growlInfo)
    {
        Application.Current.Dispatcher?.Invoke(
#if NET40
            new Action(
#endif
                () =>
                {
                    if (GrowlWindow == null)
                    {
                        GrowlWindow = new GrowlWindow();
                        GrowlWindow.Show();
                        InitGrowlPanel(GrowlWindow.GrowlPanel);
                    }

                    GrowlWindow.UpdatePosition(Growl.GetTransitionMode(Application.Current.MainWindow));
                    GrowlWindow.Show(true);

                    var ctl = new Growl
                    {
                        Message = growlInfo.Message,
                        Time = DateTime.Now,
                        Icon = ResourceHelper.GetResource<Geometry>(growlInfo.IconKey) ?? growlInfo.Icon,
                        IconBrush = ResourceHelper.GetResource<Brush>(growlInfo.IconBrushKey) ?? growlInfo.IconBrush,
                        _showCloseButton = growlInfo.ShowCloseButton,
                        ActionBeforeClose = growlInfo.ActionBeforeClose,
                        _staysOpen = growlInfo.StaysOpen,
                        ShowDateTime = growlInfo.ShowDateTime,
                        ConfirmStr = growlInfo.ConfirmStr,
                        CancelStr = growlInfo.CancelStr,
                        Type = growlInfo.Type,
                        _waitTime = Math.Max(growlInfo.WaitTime, MinWaitTime),
                    };

                    ShowInternal(GrowlWindow.GrowlPanel, ctl);
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
        (Application.Current.Dispatcher ?? growlInfo.Dispatcher)?.Invoke(
#if NET40
            new Action(
#endif
                () =>
                {
                    var ctl = new Growl
                    {
                        Message = growlInfo.Message,
                        Time = DateTime.Now,
                        Icon = ResourceHelper.GetResource<Geometry>(growlInfo.IconKey) ?? growlInfo.Icon,
                        IconBrush = ResourceHelper.GetResource<Brush>(growlInfo.IconBrushKey) ?? growlInfo.IconBrush,
                        _showCloseButton = growlInfo.ShowCloseButton,
                        ActionBeforeClose = growlInfo.ActionBeforeClose,
                        _staysOpen = growlInfo.StaysOpen,
                        ShowDateTime = growlInfo.ShowDateTime,
                        ConfirmStr = growlInfo.ConfirmStr,
                        CancelStr = growlInfo.CancelStr,
                        Type = growlInfo.Type,
                        _waitTime = Math.Max(growlInfo.WaitTime, MinWaitTime),
                    };

                    if (!string.IsNullOrEmpty(growlInfo.Token))
                    {
                        if (TokenManager.TryGetControl(growlInfo.Token, out var panel))
                        {
                            ShowInternal(panel, ctl);
                        }
                    }
                    else
                    {
                        // GrowlPanel is null, we create it automatically
                        GrowlPanel ??= CreateDefaultPanel();
                        ShowInternal(GrowlPanel, ctl);

                        var transitionMode = GetTransitionMode(GrowlPanel);
                        GrowlPanel.VerticalAlignment = GetPanelVerticalAlignment(transitionMode);
                        GrowlPanel.HorizontalAlignment = GetPanelHorizontalAlignment(transitionMode);
                        GrowlPanel.SetValue(InverseStackPanel.IsInverseEnabledProperty,
                            transitionMode is TransitionMode.Bottom2Top or TransitionMode.Bottom2TopWithFade);
                    }
                }
#if NET40
            )
#endif
        );
    }

    private static Panel CreateDefaultPanel()
    {
        FrameworkElement element = WindowHelper.GetActiveWindow();
        var decorator = VisualHelper.GetChild<AdornerDecorator>(element);

        var layer = decorator?.AdornerLayer;
        if (layer == null)
        {
            return null;
        }

        var panel = new InverseStackPanel();

        InitGrowlPanel(panel);
        SetIsCreatedAutomatically(panel, true);

        var scrollViewer = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
            IsInertiaEnabled = true,
            IsPenetrating = true,
            Content = panel
        };

        var container = new AdornerContainer(layer)
        {
            Child = scrollViewer
        };

        layer.Add(container);

        return panel;
    }

    private static void RemoveDefaultPanel(Panel panel)
    {
        FrameworkElement element = WindowHelper.GetActiveWindow();
        var decorator = VisualHelper.GetChild<AdornerDecorator>(element);

        if (decorator != null)
        {
            var layer = decorator.AdornerLayer;
            var adorner = VisualHelper.GetParent<Adorner>(panel);

            if (adorner != null)
            {
                layer?.Remove(adorner);
            }
        }
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

    private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => Close(false);

    /// <summary>
    ///     关闭
    /// </summary>
    private void Close(bool invokeParam, bool isClear = false)
    {
        if (!isClear && ActionBeforeClose?.Invoke(invokeParam) == false)
        {
            return;
        }

        _timerClose?.Stop();
        Panel.SetZIndex(this, int.MinValue);
        StartTransition(true, OnStoryboardCompleted);
        return;

        void OnStoryboardCompleted()
        {
            if (Parent is not Panel panel)
            {
                return;
            }

            panel.Children.Remove(this);

            if (GrowlWindow != null)
            {
                if (GrowlWindow.GrowlPanel is not { Children.Count: 0 })
                {
                    return;
                }

                GrowlWindow.Close();
                GrowlWindow = null;
            }
            else
            {
                if (GrowlPanel is not { Children.Count: 0 } || !GetIsCreatedAutomatically(GrowlPanel))
                {
                    return;
                }

                // If the count of children is zero, we need to remove the panel, provided that the panel was created automatically
                RemoveDefaultPanel(GrowlPanel);
                GrowlPanel = null;
            }
        }
    }

    /// <summary>
    ///     清除
    /// </summary>
    /// <param name="token"></param>
    public static void Clear(string token = "")
    {
        if (!string.IsNullOrEmpty(token))
        {
            if (TokenManager.TryGetControl(token, out var panel))
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
    private static void Clear(Panel panel) => panel?.Children.Clear();

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

    private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => Close(false);

    private void ButtonOk_OnClick(object sender, RoutedEventArgs e) => Close(true);

    private void StartTransition(bool isClose, Action completed = null)
    {
        var actualStoryboard = GetTransitionStoryboard(this) ?? CreateStoryboard(isClose, GetTransitionMode(this));
        if (actualStoryboard is null)
        {
            return;
        }

        if (completed is not null)
        {
            actualStoryboard.Completed -= OnStoryboardCompleted;
            actualStoryboard.Completed += OnStoryboardCompleted;
        }

        actualStoryboard.Begin();
        return;

        void OnStoryboardCompleted(object s, EventArgs e)
        {
            completed?.Invoke();
            actualStoryboard.Completed -= OnStoryboardCompleted;
        }
    }

    private Storyboard CreateStoryboard(bool isClose, TransitionMode transitionMode)
    {
        var transformLength = GetTransformLength(isClose, transitionMode);
        var transformAnimation = CreateTransformAnimation(isClose, transitionMode, transformLength);
        var storyboard = new Storyboard
        {
            Duration = transformAnimation.Duration
        };

        if (transitionMode is not TransitionMode.Fade)
        {
            _gridMain.RenderTransform = CreateRenderTransform(isClose, transitionMode, transformLength);
            Storyboard.SetTarget(transformAnimation, _gridMain);
            storyboard.Children.Add(transformAnimation);
        }

        if (CreateFadeAnimation(isClose, transitionMode) is not { } fadeAnimation)
        {
            return storyboard;
        }

        Storyboard.SetTarget(fadeAnimation, _gridMain);
        storyboard.Children.Add(fadeAnimation);

        return storyboard;
    }

    private double GetTransformLength(bool isClose, TransitionMode transitionMode)
    {
        var length = transitionMode switch
        {
            TransitionMode.Right2Left or TransitionMode.Right2LeftWithFade => ActualWidth,
            TransitionMode.Left2Right or TransitionMode.Left2RightWithFade => -ActualWidth,
            TransitionMode.Bottom2Top or TransitionMode.Bottom2TopWithFade => ActualHeight,
            TransitionMode.Top2Bottom or TransitionMode.Top2BottomWithFade => -ActualHeight,
            _ => ActualWidth
        };


        return isClose ? -length : length;
    }

    private static TransformGroup CreateOriginalTransform()
    {
        return new TransformGroup
        {
            Children =
            {
                new ScaleTransform(),
                new SkewTransform(),
                new RotateTransform(),
                new TranslateTransform(),
            }
        };
    }

    private static Transform CreateRenderTransform(bool isClose, TransitionMode transitionMode, double transformLength)
    {
        var transformGroup = CreateOriginalTransform();
        if (isClose)
        {
            return transformGroup;
        }

        switch (GetOrientation(transitionMode))
        {
            case Orientation.Horizontal:
                ((TranslateTransform) transformGroup.Children[TranslateTransformIndex]).X = transformLength;
                break;
            case Orientation.Vertical:
                ((TranslateTransform) transformGroup.Children[TranslateTransformIndex]).Y = transformLength;
                break;
            default:
                ((TranslateTransform) transformGroup.Children[TranslateTransformIndex]).X = transformLength;
                break;
        }

        return transformGroup;
    }

    private static DoubleAnimation CreateTransformAnimation(bool isClose, TransitionMode transitionMode,
        double transformLength)
    {
        var animation = AnimationHelper.CreateAnimation(isClose ? -transformLength : 0);

        switch (GetOrientation(transitionMode))
        {
            case Orientation.Horizontal:
                Storyboard.SetTargetProperty(animation,
                    new PropertyPath(
                        $"(UIElement.RenderTransform).(TransformGroup.Children)[{TranslateTransformIndex}].(TranslateTransform.X)"));
                break;
            case Orientation.Vertical:
                Storyboard.SetTargetProperty(animation,
                    new PropertyPath(
                        $"(UIElement.RenderTransform).(TransformGroup.Children)[{TranslateTransformIndex}].(TranslateTransform.Y)"));
                break;
            default:
                Storyboard.SetTargetProperty(animation,
                    new PropertyPath(
                        $"(UIElement.RenderTransform).(TransformGroup.Children)[{TranslateTransformIndex}].(TranslateTransform.X)"));
                break;
        }

        return animation;
    }

    private static DoubleAnimation CreateFadeAnimation(bool isClose, TransitionMode transitionMode)
    {
        if (transitionMode is TransitionMode.Right2Left or
            TransitionMode.Left2Right or
            TransitionMode.Bottom2Top or
            TransitionMode.Top2Bottom or
            TransitionMode.Custom)
        {
            return null;
        } 

        var animation = AnimationHelper.CreateAnimation(isClose ? 0 : 1);
        animation.From = isClose ? 1 : 0;
        Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Opacity)"));

        return animation;
    }

    private static Orientation? GetOrientation(TransitionMode transitionMode)
    {
        return transitionMode switch
        {
            TransitionMode.Right2Left or TransitionMode.Right2LeftWithFade or TransitionMode.Left2Right
                or TransitionMode.Left2RightWithFade => Orientation.Horizontal,
            TransitionMode.Bottom2Top or TransitionMode.Bottom2TopWithFade or TransitionMode.Top2Bottom
                or TransitionMode.Top2BottomWithFade => Orientation.Vertical,
            _ => Orientation.Horizontal
        };
    }

    internal static VerticalAlignment GetPanelVerticalAlignment(TransitionMode transitionMode)
    {
        return VerticalAlignment.Stretch;
    }

    internal static HorizontalAlignment GetPanelHorizontalAlignment(TransitionMode transitionMode)
    {
        return transitionMode switch
        {
            TransitionMode.Right2Left or TransitionMode.Right2LeftWithFade => HorizontalAlignment.Right,
            TransitionMode.Left2Right or TransitionMode.Left2RightWithFade => HorizontalAlignment.Left,
            TransitionMode.Bottom2Top or TransitionMode.Bottom2TopWithFade or TransitionMode.Top2Bottom
                or TransitionMode.Top2BottomWithFade => HorizontalAlignment.Center,
            _ => HorizontalAlignment.Right
        };
    }
}
