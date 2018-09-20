using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    /// <summary>
    ///     颜色拾取器
    /// </summary>
    public partial class ColorPicker
    {
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
        private readonly List<string> _colorPresetList = new List<string>
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
        private readonly List<ColorRange> _colorRangeList = new List<ColorRange>
        {
            new ColorRange
            {
                Color1 = Color.FromRgb(255, 0, 0),
                Color2 = Color.FromRgb(255, 0, 255)
            },
            new ColorRange
            {
                Color1 = Color.FromRgb(255, 0, 255),
                Color2 = Color.FromRgb(0, 0, 255)
            },
            new ColorRange
            {
                Color1 = Color.FromRgb(0, 0, 255),
                Color2 = Color.FromRgb(0, 255, 255)
            },
            new ColorRange
            {
                Color1 = Color.FromRgb(0, 255, 255),
                Color2 = Color.FromRgb(0, 255, 0)
            },
            new ColorRange
            {
                Color1 = Color.FromRgb(0, 255, 0),
                Color2 = Color.FromRgb(255, 255, 0)
            },
            new ColorRange
            {
                Color1 = Color.FromRgb(255, 255, 0),
                Color2 = Color.FromRgb(255, 0, 0)
            }
        };

        /// <summary>
        ///     颜色分隔集合
        /// </summary>
        private readonly List<Color> _colorSeparateList = new List<Color>
        {
            Color.FromRgb(255, 0, 0),
            Color.FromRgb(255, 0, 255),
            Color.FromRgb(0, 0, 255),
            Color.FromRgb(0, 255, 255),
            Color.FromRgb(0, 255, 0),
            Color.FromRgb(255, 255, 0)
        };

        internal static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(ColorPicker), new PropertyMetadata(new CornerRadius(2)));

        internal CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        ///     颜色改变事件
        /// </summary>
        public static readonly RoutedEvent ColorSelectedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<Color>>), typeof(ColorPicker));

        /// <summary>
        ///     颜色改变事件
        /// </summary>
        public event EventHandler<FunctionEventArgs<Color>> ColorSelected
        {
            add => AddHandler(ColorSelectedEvent, value);
            remove => RemoveHandler(ColorSelectedEvent, value);
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

        public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register(
            "SelectedBrush", typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(Brushes.White,
                (o, args) =>
                {
                    var ctl = (ColorPicker)o;
                    var v = (SolidColorBrush)args.NewValue;

                    if (ctl.ChannelR == null) return;

                    if (ctl.IsNeedUpdateInfo)
                    {
                        ctl.IsNeedUpdateInfo = false;
                        ctl.ChannelR.Value = v.Color.R;
                        ctl.ChannelG.Value = v.Color.G;
                        ctl.ChannelB.Value = v.Color.B;
                        ctl.SliderOpacity.Value = v.Color.A;
                        ctl.IsNeedUpdateInfo = true;
                    }
                    ctl.UpdateStatus(v.Color);
                    ctl.SelectedBrushWithoutOpacity = new SolidColorBrush(Color.FromRgb(v.Color.R, v.Color.G, v.Color.B));
                }));

        /// <summary>
        ///     当前选中的颜色
        /// </summary>
        public SolidColorBrush SelectedBrush
        {
            get => (SolidColorBrush)GetValue(SelectedBrushProperty);
            set => SetValue(SelectedBrushProperty, value);
        }

        private static readonly DependencyProperty SelectedBrushWithoutOpacityProperty = DependencyProperty.Register(
            "SelectedBrushWithoutOpacity", typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(Brushes.White));

        private SolidColorBrush SelectedBrushWithoutOpacity
        {
            get => (SolidColorBrush)GetValue(SelectedBrushWithoutOpacityProperty);
            set => SetValue(SelectedBrushWithoutOpacityProperty, value);
        }

        private static readonly DependencyProperty BackColorProperty = DependencyProperty.Register(
            "BackColor", typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(Brushes.Red));

        private SolidColorBrush BackColor
        {
            get => (SolidColorBrush)GetValue(BackColorProperty);
            set => SetValue(BackColorProperty, value);
        }

        private static readonly DependencyProperty ShowListProperty = DependencyProperty.Register(
            "ShowList", typeof(List<bool>), typeof(ColorPicker), new PropertyMetadata(new List<bool>
            {
                true, false, false
            }));

        private List<bool> ShowList
        {
            get => (List<bool>)GetValue(ShowListProperty);
            set => SetValue(ShowListProperty, value);
        }

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
                for (int i = 0; i < 2; i++)
                {
                    list.Add(false);
                }
                list[_colorType] = true;
                ShowList = list;
            }
        }

        public ColorPicker()
        {
            InitializeComponent();

            UpdateStatus(SelectedBrush.Color);

            Loaded += (s, e) =>
            {
                if (!_isLoaded)
                {
                    Init();
                    _isLoaded = true;
                }
            };
        }

        /// <summary>
        ///     初始化
        /// </summary>
        private void Init()
        {
            foreach (var item in _colorPresetList)
            {
                PanelColor.Children.Add(CreateColorButton(item));
            }
        }

        /// <summary>
        ///     创建颜色按钮
        /// </summary>
        /// <returns></returns>
        private Button CreateColorButton(string colorStr)
        {
            var colorObj = ColorConverter.ConvertFromString(colorStr);
            var color = default(Color);
            if (colorObj != null)
            {
                color = (Color)colorObj;
            }
            var brush = new SolidColorBrush(color);

            var button = new Button
            {
                Margin = new Thickness(0, 0, 12, 12),
                Style = FindResource("ButtonCustom") as Style,
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
                SliderOpacity.Value = SliderOpacity.Maximum;
            };

            return button;
        }

        /// <summary>
        ///     内部更新
        /// </summary>
        private void UpdateStatus(Color color)
        {
            if (_isOnDragging) return;

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
                    if (!SliderColor.IsMouseOver && !SliderOpacity.IsMouseOver)
                    {
                        SliderColor.Value = 0;
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
                    if (!SliderColor.IsMouseOver && !SliderOpacity.IsMouseOver)
                    {
                        SliderColor.Value = 0;
                    }
                    IsNeedUpdateInfo = true;
                }
                else
                {
                    var common = list[commonIndex];
                    list[maxIndex] = 255;
                    list[minIndex] = 0;
                    common = (byte)(255 * (min - common) / (double)(min - max));
                    list[commonIndex] = common;
                    BackColor = new SolidColorBrush(Color.FromRgb(list[0], list[1], list[2]));

                    list[commonIndex] = 0;
                    var cIndex = _colorSeparateList.IndexOf(Color.FromRgb(list[0], list[1], list[2]));
                    int sub;
                    var direc = 0;
                    if (cIndex < 5 && cIndex > 0)
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
                    if (!SliderColor.IsMouseOver && !SliderOpacity.IsMouseOver)
                    {
                        SliderColor.Value = scaleTotal;
                    }
                    IsNeedUpdateInfo = true;
                }
            }

            var matrix = BorderPicker.RenderTransform.Value;
            var x = max == 0 ? 0 : (1 - min / (double)max) * ColorPanelWidth;
            var y = (1 - max / 255.0) * ColorPanelHeight;
            if (_isNeedUpdatePicker)
            {
                BorderPicker.RenderTransform = new MatrixTransform(matrix.M11, matrix.M12, matrix.M21, matrix.M22, x, y);
            }
        }

        private void SliderColor_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isLoaded || !IsNeedUpdateInfo) return;
            var index = Math.Min(5, (int)Math.Floor(e.NewValue));
            var sub = e.NewValue - index;
            var range = _colorRangeList[index];

            var color = range.GetColor(sub);
            BackColor = new SolidColorBrush(color);
            var matrix = BorderPicker.RenderTransform.Value;
            _isNeedUpdatePicker = false;
            UpdateColorWhenDrag(new Point(matrix.OffsetX, matrix.OffsetY));
            _isNeedUpdatePicker = true;
        }

        private void SliderOpacity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isLoaded || !IsNeedUpdateInfo) return;
            var color = SelectedBrush.Color;
            SelectedBrush = new SolidColorBrush(Color.FromArgb((byte)SliderOpacity.Value, color.R, color.G, color.B));
        }

        private void MouseDragElementBehavior_OnDragging(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(BorderColor);
            _isOnDragging = true;
            UpdateColorWhenDrag(p);
            _isOnDragging = false;
        }

        /// <summary>
        ///     拖动时更新颜色
        /// </summary>
        private void UpdateColorWhenDrag(Point p)
        {
            var matrix = BorderPicker.RenderTransform.Value;

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
                BorderPicker.RenderTransform = new MatrixTransform(matrix.M11, matrix.M12, matrix.M21, matrix.M22, p.X, p.Y);
            }

            var scaleX = p.X / ColorPanelWidth;
            var scaleY = 1 - p.Y / ColorPanelHeight;

            var colorYLeft = Color.FromRgb((byte)(255 * scaleY), (byte)(255 * scaleY), (byte)(255 * scaleY));
            var colorYRight = Color.FromRgb((byte)(BackColor.Color.R * scaleY), (byte)(BackColor.Color.G * scaleY), (byte)(BackColor.Color.B * scaleY));

            var subR = colorYLeft.R - colorYRight.R;
            var subG = colorYLeft.G - colorYRight.G;
            var subB = colorYLeft.B - colorYRight.B;

            var color = Color.FromArgb((byte)SliderOpacity.Value, (byte)(colorYLeft.R - subR * scaleX),
                (byte)(colorYLeft.G - subG * scaleX), (byte)(colorYLeft.B - subB * scaleX));
            SelectedBrush = new SolidColorBrush(color);
        }

        private void MouseDragElementBehavior_OnDragFinished(object sender, MouseEventArgs e) => BorderDrag.RenderTransform = new MatrixTransform();

        private void ButtonSwitch_OnClick(object sender, RoutedEventArgs e) => ColorType++;

        private void InfoNumericUpDownRgb_OnValueChanged(object sender, FunctionEventArgs<double> e)
        {
            if (!_isLoaded || !IsNeedUpdateInfo) return;
            if (e.OriginalSource is InfoNumericUpDown ctl && ctl.Tag is string tag)
            {
                var color = SelectedBrush.Color;
                IsNeedUpdateInfo = false;

                switch (tag)
                {
                    case "R":
                        {
                            SelectedBrush = new SolidColorBrush(Color.FromArgb(color.A, (byte)e.Info, color.G, color.B));
                            break;
                        }
                    case "G":
                        {
                            SelectedBrush = new SolidColorBrush(Color.FromArgb(color.A, color.R, (byte)e.Info, color.B));
                            break;
                        }
                    case "B":
                        {
                            SelectedBrush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, (byte)e.Info));
                            break;
                        }
                }

                IsNeedUpdateInfo = true;
            }
        }

        private void ButtonSure_OnClick(object sender, RoutedEventArgs e)
            => RaiseEvent(new FunctionEventArgs<Color>(ColorSelectedEvent, this)
            {
                Info = SelectedBrush.Color
            });

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(CanceledEvent));
    }
}