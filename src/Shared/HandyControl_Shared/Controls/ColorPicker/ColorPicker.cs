using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

/// <summary>
///     颜色拾取器
/// </summary>
[TemplatePart(Name = ElementBorderColor, Type = typeof(Border))]
[TemplatePart(Name = ElementBorderPicker, Type = typeof(Border))]
[TemplatePart(Name = ElementBorderDrag, Type = typeof(Border))]
[TemplatePart(Name = ElementPanelColor, Type = typeof(Panel))]
[TemplatePart(Name = ElementSliderColor, Type = typeof(Panel))]
[TemplatePart(Name = ElementSliderOpacity, Type = typeof(Panel))]
[TemplatePart(Name = ElementPanelRgb, Type = typeof(Panel))]
[TemplatePart(Name = ElementButtonDropper, Type = typeof(ToggleButton))]
public class ColorPicker : Control, ISingleOpen
{
    #region Constants

    private const string ElementBorderColor = "PART_BorderColor";
    private const string ElementBorderPicker = "PART_BorderPicker";
    private const string ElementBorderDrag = "PART_BorderDrag";
    private const string ElementPanelColor = "PART_PanelColor";
    private const string ElementSliderColor = "PART_SliderColor";
    private const string ElementSliderOpacity = "PART_SliderOpacity";
    private const string ElementPanelRgb = "PART_PanelRgb";
    private const string ElementButtonDropper = "PART_ButtonDropper";

    #endregion Constants

    #region Data

    private ColorDropper _colorDropper;

    private ToggleButton _toggleButtonDropper;

    private Panel _panelRgb;

    private Border _borderColor;

    private Border _borderPicker;

    private Border _borderDrag;

    private Panel _panelColor;

    private Slider _sliderColor;

    private Slider _sliderOpacity;

    private bool _appliedTemplate;

    private bool _disposed;

    /// <summary>
    ///     当前显示的颜色类型
    /// </summary>
    private int _colorType;

    /// <summary>
    ///     是否已经加载控件
    /// </summary>
    private bool _isLoaded;

    /// <summary>
    ///     是否需要更新小球位置
    /// </summary>
    private bool _isNeedUpdatePicker = true;

    /// <summary>
    ///     是否在拖动小球
    /// </summary>
    private bool _isOnDragging;

    /// <summary>
    ///     是否需要更新信息
    /// </summary>
    private bool IsNeedUpdateInfo { get; set; } = true;

    /// <summary>
    ///     颜色选取面板宽度
    /// </summary>
    private const double ColorPanelWidth = 230;

    /// <summary>
    ///     颜色选取面板高度
    /// </summary>
    private const double ColorPanelHeight = 122;

    /// <summary>
    ///     预设的颜色（一共18个，两行）
    /// </summary>
    private readonly List<string> _colorPresetList = new()
    {
        "#f44336",
        "#e91e63",
        "#9c27b0",
        "#673ab7",
        "#3f51b5",
        "#2196f3",
        "#03a9f4",
        "#00bcd4",
        "#009688",

        "#4caf50",
        "#8bc34a",
        "#cddc39",
        "#ffeb3b",
        "#ffc107",
        "#ff9800",
        "#ff5722",
        "#795548",
        "#9e9e9e"
    };

    /// <summary>
    ///     颜色范围集合
    /// </summary>
    private readonly List<ColorRange> _colorRangeList = new()
    {
        new ColorRange
        {
            Start = Color.FromRgb(255, 0, 0),
            End = Color.FromRgb(255, 0, 255)
        },
        new ColorRange
        {
            Start = Color.FromRgb(255, 0, 255),
            End = Color.FromRgb(0, 0, 255)
        },
        new ColorRange
        {
            Start = Color.FromRgb(0, 0, 255),
            End = Color.FromRgb(0, 255, 255)
        },
        new ColorRange
        {
            Start = Color.FromRgb(0, 255, 255),
            End = Color.FromRgb(0, 255, 0)
        },
        new ColorRange
        {
            Start = Color.FromRgb(0, 255, 0),
            End = Color.FromRgb(255, 255, 0)
        },
        new ColorRange
        {
            Start = Color.FromRgb(255, 255, 0),
            End = Color.FromRgb(255, 0, 0)
        }
    };

    /// <summary>
    ///     颜色分隔集合
    /// </summary>
    private readonly List<Color> _colorSeparateList = new()
    {
        Color.FromRgb(255, 0, 0),
        Color.FromRgb(255, 0, 255),
        Color.FromRgb(0, 0, 255),
        Color.FromRgb(0, 255, 255),
        Color.FromRgb(0, 255, 0),
        Color.FromRgb(255, 255, 0)
    };

    #endregion Data

    #region Public Events

    public static readonly RoutedEvent SelectedColorChangedEvent =
        EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble,
            typeof(EventHandler<FunctionEventArgs<Color>>), typeof(ColorPicker));

    public event EventHandler<FunctionEventArgs<Color>> SelectedColorChanged
    {
        add => AddHandler(SelectedColorChangedEvent, value);
        remove => RemoveHandler(SelectedColorChangedEvent, value);
    }

    /// <summary>
    ///     颜色改变事件
    /// </summary>
    public static readonly RoutedEvent ConfirmedEvent =
        EventManager.RegisterRoutedEvent("Confirmed", RoutingStrategy.Bubble,
            typeof(EventHandler<FunctionEventArgs<Color>>), typeof(ColorPicker));

    /// <summary>
    ///     颜色改变事件
    /// </summary>
    public event EventHandler<FunctionEventArgs<Color>> Confirmed
    {
        add => AddHandler(ConfirmedEvent, value);
        remove => RemoveHandler(ConfirmedEvent, value);
    }

    /// <summary>
    ///     取消事件
    /// </summary>
    public static readonly RoutedEvent CanceledEvent =
        EventManager.RegisterRoutedEvent("Canceled", RoutingStrategy.Bubble,
            typeof(EventHandler), typeof(ColorPicker));

    /// <summary>
    ///     取消事件
    /// </summary>
    public event EventHandler Canceled
    {
        add => AddHandler(CanceledEvent, value);
        remove => RemoveHandler(CanceledEvent, value);
    }

    #endregion Public Events

    #region Properties

    internal static readonly DependencyProperty ChannelAProperty = DependencyProperty.Register(
        nameof(ChannelA), typeof(int), typeof(ColorPicker), new PropertyMetadata(255));

    internal int ChannelA
    {
        get => (int) GetValue(ChannelAProperty);
        set => SetValue(ChannelAProperty, value);
    }

    internal static readonly DependencyProperty ChannelRProperty = DependencyProperty.Register(
        nameof(ChannelR), typeof(int), typeof(ColorPicker), new PropertyMetadata(255));

    internal int ChannelR
    {
        get => (int) GetValue(ChannelRProperty);
        set => SetValue(ChannelRProperty, value);
    }

    internal static readonly DependencyProperty ChannelGProperty = DependencyProperty.Register(
        nameof(ChannelG), typeof(int), typeof(ColorPicker), new PropertyMetadata(255));

    internal int ChannelG
    {
        get => (int) GetValue(ChannelGProperty);
        set => SetValue(ChannelGProperty, value);
    }

    internal static readonly DependencyProperty ChannelBProperty = DependencyProperty.Register(
        nameof(ChannelB), typeof(int), typeof(ColorPicker), new PropertyMetadata(255));

    internal int ChannelB
    {
        get => (int) GetValue(ChannelBProperty);
        set => SetValue(ChannelBProperty, value);
    }

    public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register(
        nameof(SelectedBrush), typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(Brushes.White, OnSelectedBrushChanged, CoerceSelectedBrush));

    private static object CoerceSelectedBrush(DependencyObject d, object basevalue)
    {
        return basevalue is SolidColorBrush ? basevalue : Brushes.White;
    }

    private static void OnSelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ColorPicker) d;
        var v = (SolidColorBrush) e.NewValue;

        if (ctl.IsNeedUpdateInfo)
        {
            ctl.IsNeedUpdateInfo = false;
            ctl.ChannelR = v.Color.R;
            ctl.ChannelG = v.Color.G;
            ctl.ChannelB = v.Color.B;
            ctl.ChannelA = v.Color.A;
            ctl.IsNeedUpdateInfo = true;
        }
        ctl.UpdateStatus(v.Color);
        ctl.SelectedBrushWithoutOpacity = new SolidColorBrush(Color.FromRgb(v.Color.R, v.Color.G, v.Color.B));
        ctl.RaiseEvent(new FunctionEventArgs<Color>(SelectedColorChangedEvent, ctl)
        {
            Info = v.Color
        });
    }

    /// <summary>
    ///     当前选中的颜色
    /// </summary>
    public SolidColorBrush SelectedBrush
    {
        get => (SolidColorBrush) GetValue(SelectedBrushProperty);
        set => SetValue(SelectedBrushProperty, value);
    }

    internal static readonly DependencyProperty SelectedBrushWithoutOpacityProperty = DependencyProperty.Register(
        nameof(SelectedBrushWithoutOpacity), typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(Brushes.White));

    internal SolidColorBrush SelectedBrushWithoutOpacity
    {
        get => (SolidColorBrush) GetValue(SelectedBrushWithoutOpacityProperty);
        set => SetValue(SelectedBrushWithoutOpacityProperty, value);
    }

    internal static readonly DependencyProperty BackColorProperty = DependencyProperty.Register(
        nameof(BackColor), typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(Brushes.Red));

    internal SolidColorBrush BackColor
    {
        get => (SolidColorBrush) GetValue(BackColorProperty);
        set => SetValue(BackColorProperty, value);
    }

    internal static readonly DependencyProperty ShowListProperty = DependencyProperty.Register(
        nameof(ShowList), typeof(List<bool>), typeof(ColorPicker), new PropertyMetadata(new List<bool>
        {
            true, false, false
        }));

    internal List<bool> ShowList
    {
        get => (List<bool>) GetValue(ShowListProperty);
        set => SetValue(ShowListProperty, value);
    }

    #endregion Properties

    /// <summary>
    ///     当前显示的颜色类型
    /// </summary>
    private int ColorType
    {
        get => _colorType;
        set
        {
            if (value < 0)
            {
                _colorType = 1;
            }
            else if (value > 1)
            {
                _colorType = 0;
            }
            else
            {
                _colorType = value;
            }

            var list = new List<bool>();
            for (var i = 0; i < 2; i++)
            {
                list.Add(false);
            }
            list[_colorType] = true;
            ShowList = list;
        }
    }

    ~ColorPicker()
    {
        Dispose(false);
    }

    public ColorPicker()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, ButtonConfirm_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Switch, ButtonSwitch_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, ButtonCancel_OnClick));

        Loaded += (s, e) =>
        {
            if (!_isLoaded)
            {
                Init();
                _isLoaded = true;
            }
        };
    }

    public override void OnApplyTemplate()
    {
        _appliedTemplate = false;
        if (_sliderColor != null)
        {
            _sliderColor.ValueChanged -= SliderColor_OnValueChanged;
        }

        if (_sliderOpacity != null)
        {
            _sliderOpacity.ValueChanged -= SliderOpacity_OnValueChanged;
        }

        if (_toggleButtonDropper != null)
        {
            _toggleButtonDropper.Click -= ToggleButtonDropper_Click;
        }

        _panelColor?.Children.Clear();

        _panelRgb?.RemoveHandler(NumericUpDown.ValueChangedEvent, new EventHandler<FunctionEventArgs<double>>(NumericUpDownRgb_OnValueChanged));

        base.OnApplyTemplate();

        _borderColor = GetTemplateChild(ElementBorderColor) as Border;
        _borderDrag = GetTemplateChild(ElementBorderDrag) as Border;
        _borderPicker = GetTemplateChild(ElementBorderPicker) as Border;
        _panelColor = GetTemplateChild(ElementPanelColor) as Panel;
        _sliderColor = GetTemplateChild(ElementSliderColor) as Slider;
        _sliderOpacity = GetTemplateChild(ElementSliderOpacity) as Slider;
        _panelRgb = GetTemplateChild(ElementPanelRgb) as Panel;
        _toggleButtonDropper = GetTemplateChild(ElementButtonDropper) as ToggleButton;

        if (_sliderColor != null)
        {
            _sliderColor.ValueChanged += SliderColor_OnValueChanged;
        }

        if (_sliderOpacity != null)
        {
            _sliderOpacity.ValueChanged += SliderOpacity_OnValueChanged;
        }

        if (_borderDrag != null)
        {
            var behavior = new MouseDragElementBehavior();
            behavior.DragFinished += MouseDragElementBehavior_OnDragFinished;
            behavior.DragBegun += MouseDragElementBehavior_OnDragging;
            behavior.Dragging += MouseDragElementBehavior_OnDragging;
            var collection = Interaction.GetBehaviors(_borderDrag);
            collection.Add(behavior);
        }

        if (_toggleButtonDropper != null)
        {
            _toggleButtonDropper.Click += ToggleButtonDropper_Click;
        }

        _appliedTemplate = true;
        if (_isLoaded)
        {
            Init();
        }
        _panelRgb?.AddHandler(NumericUpDown.ValueChangedEvent, new EventHandler<FunctionEventArgs<double>>(NumericUpDownRgb_OnValueChanged));
    }

    /// <summary>
    ///     初始化
    /// </summary>
    private void Init()
    {
        if (_panelColor == null) return;
        UpdateStatus(SelectedBrush.Color);
        _panelColor.Children.Clear();
        foreach (var item in _colorPresetList)
        {
            _panelColor.Children.Add(CreateColorButton(item));
        }
    }

    /// <summary>
    ///     创建颜色按钮
    /// </summary>
    /// <returns></returns>
    private Button CreateColorButton(string colorStr)
    {
        var color = ColorConverter.ConvertFromString(colorStr) ?? default(Color);
        var brush = new SolidColorBrush((Color) color);

        var button = new Button
        {
            Margin = new Thickness(6),
            Style = ResourceHelper.GetResourceInternal<Style>(ResourceToken.ButtonCustom),
            Content = new Border
            {
                Background = brush,
                Width = 12,
                Height = 12,
                CornerRadius = new CornerRadius(2)
            }
        };

        button.Click += (s, e) =>
        {
            SelectedBrush = brush;
            ChannelA = byte.MaxValue;
        };

        return button;
    }

    /// <summary>
    ///     内部更新
    /// </summary>
    private void UpdateStatus(Color color)
    {
        if (_isOnDragging || _sliderColor == null) return;

        var r = color.R;
        var g = color.G;
        var b = color.B;
        var list = new List<byte>
        {
            r,
            g,
            b
        };

        var max = list.Max();
        var min = list.Min();

        if (min == max)
        {
            if (!(r == g && b == g))
            {
                BackColor = Brushes.Red;
                IsNeedUpdateInfo = false;
                if (!_sliderColor.IsMouseOver && !_sliderOpacity.IsMouseOver)
                {
                    _sliderColor.Value = 0;
                }
                IsNeedUpdateInfo = true;
            }
        }
        else
        {
            var maxIndex = list.IndexOf(max);
            var minIndex = list.IndexOf(min);
            var commonIndex = 3 - maxIndex - minIndex;
            if (commonIndex == 3)
            {
                BackColor = Brushes.Red;
                IsNeedUpdateInfo = false;
                if (!_sliderColor.IsMouseOver && !_sliderOpacity.IsMouseOver)
                {
                    _sliderColor.Value = 0;
                }
                IsNeedUpdateInfo = true;
            }
            else
            {
                var common = list[commonIndex];
                list[maxIndex] = 255;
                list[minIndex] = 0;
                common = (byte) (255 * (min - common) / (double) (min - max));
                list[commonIndex] = common;
                BackColor = new SolidColorBrush(Color.FromRgb(list[0], list[1], list[2]));

                list[commonIndex] = 0;
                var cIndex = _colorSeparateList.IndexOf(Color.FromRgb(list[0], list[1], list[2]));
                int sub;
                var direc = 0;
                if (cIndex is < 5 and > 0)
                {
                    var nextColorList = _colorSeparateList[cIndex + 1].ToList();
                    var prevColorList = _colorSeparateList[cIndex - 1].ToList();
                    if (nextColorList[minIndex] > 0)
                    {
                        var target = prevColorList[commonIndex];
                        direc = 1;
                        sub = target - common;
                    }
                    else
                    {
                        sub = common;
                    }
                }
                else if (cIndex == 0)
                {
                    sub = common;
                    if (minIndex == 2)
                    {
                        sub = 255 - common;
                        direc = -5;
                    }
                }
                else
                {
                    sub = 255 - common;
                }
                var scale = sub / 255.0;
                var scaleTotal = cIndex - direc + scale;
                IsNeedUpdateInfo = false;
                if (!_sliderColor.IsMouseOver && !_sliderOpacity.IsMouseOver)
                {
                    _sliderColor.Value = scaleTotal;
                }
                IsNeedUpdateInfo = true;
            }
        }

        var matrix = _borderPicker.RenderTransform.Value;
        var x = max == 0 ? 0 : (1 - min / (double) max) * ColorPanelWidth;
        var y = (1 - max / 255.0) * ColorPanelHeight;
        if (_isNeedUpdatePicker)
        {
            _borderPicker.RenderTransform = new MatrixTransform(matrix.M11, matrix.M12, matrix.M21, matrix.M22, x, y);
        }
    }

    private void SliderColor_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_appliedTemplate || !IsNeedUpdateInfo) return;
        var index = Math.Min(5, (int) Math.Floor(e.NewValue));
        var sub = e.NewValue - index;
        var range = _colorRangeList[index];

        var color = range.GetColor(sub);
        BackColor = new SolidColorBrush(color);
        var matrix = _borderPicker.RenderTransform.Value;
        _isNeedUpdatePicker = false;
        UpdateColorWhenDrag(new Point(matrix.OffsetX, matrix.OffsetY));
        _isNeedUpdatePicker = true;
    }

    private void SliderOpacity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_appliedTemplate || !IsNeedUpdateInfo) return;
        var color = SelectedBrush.Color;
        SelectedBrush = new SolidColorBrush(Color.FromArgb((byte) _sliderOpacity.Value, color.R, color.G, color.B));
    }

    private void MouseDragElementBehavior_OnDragging(object sender, MouseEventArgs e)
    {
        var p = e.GetPosition(_borderColor);
        _isOnDragging = true;
        UpdateColorWhenDrag(p);
        _isOnDragging = false;
    }

    /// <summary>
    ///     拖动时更新颜色
    /// </summary>
    private void UpdateColorWhenDrag(Point p)
    {
        var matrix = _borderPicker.RenderTransform.Value;

        if (p.X < 0)
        {
            p.X = 0;
        }
        else if (p.X > ColorPanelWidth)
        {
            p.X = ColorPanelWidth;
        }

        if (p.Y < 0)
        {
            p.Y = 0;
        }
        else if (p.Y > ColorPanelHeight)
        {
            p.Y = ColorPanelHeight;
        }

        if (_isNeedUpdatePicker)
        {
            _borderPicker.RenderTransform = new MatrixTransform(matrix.M11, matrix.M12, matrix.M21, matrix.M22, p.X, p.Y);
        }

        var scaleX = p.X / ColorPanelWidth;
        var scaleY = 1 - p.Y / ColorPanelHeight;

        var colorYLeft = Color.FromRgb((byte) (255 * scaleY), (byte) (255 * scaleY), (byte) (255 * scaleY));
        var colorYRight = Color.FromRgb((byte) (BackColor.Color.R * scaleY), (byte) (BackColor.Color.G * scaleY), (byte) (BackColor.Color.B * scaleY));

        var subR = colorYLeft.R - colorYRight.R;
        var subG = colorYLeft.G - colorYRight.G;
        var subB = colorYLeft.B - colorYRight.B;

        var color = Color.FromArgb((byte) _sliderOpacity.Value, (byte) (colorYLeft.R - subR * scaleX),
            (byte) (colorYLeft.G - subG * scaleX), (byte) (colorYLeft.B - subB * scaleX));
        SelectedBrush = new SolidColorBrush(color);
    }

    private void MouseDragElementBehavior_OnDragFinished(object sender, MouseEventArgs e) => _borderDrag.RenderTransform = new MatrixTransform();

    private void ButtonSwitch_OnClick(object sender, RoutedEventArgs e) => ColorType++;

    // ReSharper disable once UnusedParameter.Local
    private void NumericUpDownRgb_OnValueChanged(object sender, FunctionEventArgs<double> e)
    {
        if (!_appliedTemplate || !IsNeedUpdateInfo) return;
        if (e.OriginalSource is NumericUpDown { Tag: string tag })
        {
            var color = SelectedBrush.Color;
            IsNeedUpdateInfo = false;

            SelectedBrush = tag switch
            {
                "R" => new SolidColorBrush(Color.FromArgb(color.A, (byte) e.Info, color.G, color.B)),
                "G" => new SolidColorBrush(Color.FromArgb(color.A, color.R, (byte) e.Info, color.B)),
                "B" => new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, (byte) e.Info)),
                _ => SelectedBrush
            };

            IsNeedUpdateInfo = true;
        }
    }

    private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new FunctionEventArgs<Color>(ConfirmedEvent, this)
        {
            Info = SelectedBrush.Color
        });
    }

    private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(CanceledEvent));

    private void ToggleButtonDropper_Click(object sender, RoutedEventArgs e)
    {
        _colorDropper ??= new ColorDropper(this);
        // ReSharper disable once PossibleInvalidOperationException
        _colorDropper.Update(_toggleButtonDropper.IsChecked.Value);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        Dispatcher.BeginInvoke(new Action(() =>
        {
            _colorDropper?.Update(false);
            System.Windows.Window.GetWindow(this)?.Close();
        }));
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public bool CanDispose => true;
}
