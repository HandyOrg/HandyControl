using System.Windows;
using System.Windows.Media.Animation;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    /// InfoTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class InfoTextBox
    {
        /// <summary>
        ///     可输入的最大字符数
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
            "MaxLength", typeof(int), typeof(InfoTextBox), new PropertyMetadata(default(int)));

        /// <summary>
        ///     可输入的最大字符数
        /// </summary>
        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public InfoTextBox()
        {
            InitializeComponent();

            ErrorTextBlock = TextBlockError;
            ContentTextBox = MyTextBox;
            TitleStackPanel = StackPanelTitle;

            (TryFindResource("Storyboard2") as Storyboard)?.Begin();

            Loaded += (s, e) =>
            {
                StackPanelTitle.Focusable = true;
                StackPanelTitle.Focus();
                StackPanelTitle.Focusable = false;
            };
        }
    }
}