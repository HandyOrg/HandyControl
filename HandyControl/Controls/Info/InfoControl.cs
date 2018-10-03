using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using HandyControl.Data.Enum;
using HandyControl.Tools;
using HandyControl.Tools.Converter;
using HandyControl.Tools.Extension;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    internal abstract class InfoControl : UserControl
    {
        private bool _isFocused;

        protected TextBlock ErrorTextBlock;

        protected TextBox ContentTextBox;

        protected StackPanel TitleStackPanel;

        /// <summary>
        ///     标题
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(InfoControl), new PropertyMetadata(default(string)));

        /// <summary>
        ///     标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        ///     文本
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(InfoControl), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        ///     文本
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        ///     是否为必填项
        /// </summary>
        public static readonly DependencyProperty IsNeedlyProperty = DependencyProperty.Register(
            "IsNeedly", typeof(bool), typeof(InfoControl), new PropertyMetadata(true));

        /// <summary>
        ///     是否为必填项
        /// </summary>
        public bool IsNeedly
        {
            get => (bool)GetValue(IsNeedlyProperty);
            set => SetValue(IsNeedlyProperty, value);
        }

        /// <summary>
        ///     占位符
        /// </summary>
        public static readonly DependencyProperty PlaceHolderProperty = DependencyProperty.Register(
            "PlaceHolder", typeof(string), typeof(InfoControl), new PropertyMetadata(default(string)));

        /// <summary>
        ///     占位符
        /// </summary>
        public string PlaceHolder
        {
            get => (string)GetValue(PlaceHolderProperty);
            set => SetValue(PlaceHolderProperty, value);
        }

        /// <summary>
        ///     错误信息
        /// </summary>
        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(InfoControl), new PropertyMetadata(default(string)));

        /// <summary>
        ///     错误信息
        /// </summary>
        public string ErrorStr
        {
            get => (string)GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        /// <summary>
        ///     控件是否包含错误
        /// </summary>
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(InfoControl), new PropertyMetadata(default(bool)));

        /// <summary>
        ///     控件是否包含错误
        /// </summary>
        public bool IsError
        {
            get => (bool)GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, value);
        }

        /// <summary>
        ///     标题宽度
        /// </summary>
        public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.RegisterAttached(
            "TitleWidth", typeof(GridLength), typeof(InfoControl),
            new FrameworkPropertyMetadata(GridLength.Auto, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        ///     设置标题宽度
        /// </summary>
        public static void SetTitleWidth(DependencyObject element, GridLength value)
        {
            element.SetValue(TitleWidthProperty, value);
        }

        /// <summary>
        ///     标题宽度
        /// </summary>
        public static GridLength GetTitleWidth(DependencyObject element)
        {
            return (GridLength)element.GetValue(TitleWidthProperty);
        }

        /// <summary>
        ///     标题宽度
        /// </summary>
        public GridLength TitleWidth
        {
            get => (GridLength)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }

        /// <summary>
        ///     标题对齐方式
        /// </summary>
        public static readonly DependencyProperty TitleAlignmentProperty = DependencyProperty.Register(
            "TitleAlignment", typeof(TitleAlignment), typeof(InfoControl), new PropertyMetadata(TitleAlignment.Left,
                (o, args) =>
                {
                    var ctl = (InfoControl)o;
                    var v = (TitleAlignment)args.NewValue;
                    if (v == TitleAlignment.Left)
                    {
                        Grid.SetColumn(ctl.TitleStackPanel, 0);
                        Grid.SetRow(ctl.TitleStackPanel, 1);
                        ctl.HorizontalAlignment = HorizontalAlignment.Right;
                        ctl.Height = ctl.MaxHeight;
                    }
                    else
                    {
                        Grid.SetColumn(ctl.TitleStackPanel, 1);
                        Grid.SetRow(ctl.TitleStackPanel, 0);
                        ctl.TitleStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                        ctl.TitleStackPanel.Margin = new Thickness(4, 0, 0, 4);
                        ctl.Height += 18;
                    }
                }));

        /// <summary>
        ///     标题对齐方式
        /// </summary>
        public TitleAlignment TitleAlignment
        {
            get => (TitleAlignment)GetValue(TitleAlignmentProperty);
            set => SetValue(TitleAlignmentProperty, value);
        }

        /// <summary>
        ///     文本的水平对齐方式
        /// </summary>
        public static readonly DependencyProperty HorizontalTextAlignmentProperty = DependencyProperty.Register(
            "HorizontalTextAlignment", typeof(HorizontalAlignment), typeof(InfoControl), new PropertyMetadata(HorizontalAlignment.Left));

        /// <summary>
        ///     文本的水平对齐方式
        /// </summary>
        public HorizontalAlignment HorizontalTextAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalTextAlignmentProperty);
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(InfoControl), new PropertyMetadata(true, (o, args) =>
            {
                var ctl = (InfoControl)o;
                var v = (bool)args.NewValue;
                if (!(ctl.FindName("ButtonClear") is Button button)) return;
                BindingOperations.ClearBinding(button, VisibilityProperty);
                button.IsEnabled = v;
                if (v)
                {
                    button.SetBinding(VisibilityProperty, new Binding("Text")
                    {
                        Source = ctl,
                        Converter = new String2VisibilityConverter()
                    });
                }
                else
                {
                    button.Collapse();
                }
            }));

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        /// <summary>
        ///     是否显示小圆角
        /// </summary>
        public static readonly DependencyProperty ShowSmallCornerRadiusProperty = DependencyProperty.Register(
            "ShowSmallCornerRadius", typeof(bool), typeof(InfoControl), new PropertyMetadata(default(bool), (o, args) =>
            {
                var ctl = (InfoControl)o;
                var v = (bool)args.NewValue;
                ctl.CornerRadius = v ? new CornerRadius(2, 2, 2, 2) : new CornerRadius(4, 4, 4, 4);
            }));

        /// <summary>
        ///     是否显示小圆角
        /// </summary>
        public bool ShowSmallCornerRadius
        {
            get => (bool)GetValue(ShowSmallCornerRadiusProperty);
            set => SetValue(ShowSmallCornerRadiusProperty, value);
        }

        /// <summary>
        ///     圆角
        /// </summary>
        protected static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(InfoControl), new PropertyMetadata(new CornerRadius(4, 4, 4, 4)));

        /// <summary>
        ///     圆角
        /// </summary>
        protected CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        ///     文本内边距
        /// </summary>
        public static readonly DependencyProperty TextPaddingProperty = DependencyProperty.Register(
            "TextPadding", typeof(Thickness), typeof(InfoControl), new PropertyMetadata(new Thickness(6, 0, 0, 0)));

        /// <summary>
        ///     文本内边距
        /// </summary>
        public Thickness TextPadding
        {
            get => (Thickness)GetValue(TextPaddingProperty);
            set => SetValue(TextPaddingProperty, value);
        }

        /// <summary>
        ///     更新状态
        /// </summary>
        public virtual void Update()
        {
            if (!IsError)
            {
                if (!_isFocused)
                {
                    (TryFindResource("Storyboard2") as Storyboard)?.Begin();
                }
                else
                {
                    (TryFindResource("Storyboard3") as Storyboard)?.Begin();
                }
                ErrorTextBlock.BeginAnimation(MarginProperty, AnimationHelper.CreateAnimation(new Thickness(4, 0, 0, 1)));
                return;
            }
            (TryFindResource("Storyboard4") as Storyboard)?.Begin();
            ErrorTextBlock.BeginAnimation(MarginProperty, AnimationHelper.CreateAnimation(new Thickness(4, 0, 0, -ErrorTextBlock.ActualHeight - 2)));
        }

        protected virtual void InputBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            _isFocused = false;
            CheckInfo();
        }

        protected virtual void InputBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            _isFocused = true;
            if (!IsError)
            {
                (TryFindResource("Storyboard3") as Storyboard)?.Begin();
            }
        }

        protected virtual void InputBox_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!IsError && !ContentTextBox.IsFocused)
            {
                (TryFindResource("Storyboard1") as Storyboard)?.Begin();
            }
        }

        protected virtual void InputBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsError && !ContentTextBox.IsFocused)
            {
                (TryFindResource("Storyboard2") as Storyboard)?.Begin();
            }
        }

        protected virtual void InputBox_OnTextChanged(object sender, TextChangedEventArgs e) => CheckInfo();

        protected virtual void ButtonClear_OnClick(object sender, RoutedEventArgs e) => Text = string.Empty;

        /// <summary>
        ///     文本类型
        /// </summary>
        public TextType TextType { get; set; }

        /// <summary>
        ///     正则字符串（当文本类型为Common时，此属性才生效）
        /// </summary>
        public string RegularStr { get; set; }

        /// <summary>
        ///     用于监测数据的正确性
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckInfo()
        {
            bool result;
            if (TextType == TextType.Common)
            {
                if (string.IsNullOrEmpty(RegularStr))
                {
                    result = !IsNeedly || !string.IsNullOrEmpty(Text);
                }
                else
                {
                    result = Text.IsKindOf(RegularStr);
                }
            }
            else
            {
                result = Text.IsKindOf(TextType);
            }
            IsError = !result;
            Update();
            return result;
        }
    }
}