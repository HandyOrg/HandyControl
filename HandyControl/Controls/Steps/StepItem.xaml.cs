using System.Windows;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    /// StepItem.xaml 的交互逻辑
    /// </summary>
    public partial class StepItem
    {
        public StepItem()
        {
            InitializeComponent();
        }

        internal static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
            "Index", typeof(int), typeof(StepItem), new PropertyMetadata(-1));

        internal int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        internal static readonly DependencyProperty IndexStrProperty = DependencyProperty.Register(
            "IndexStr", typeof(string), typeof(StepItem), new PropertyMetadata(default(string)));

        internal string IndexStr
        {
            get => (string) GetValue(IndexStrProperty);
            set => SetValue(IndexStrProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(StepItem), new PropertyMetadata(default(string)));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        private bool? _status;

        public bool? Status
        {
            get => _status;
            set
            {
                _status = value;
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            if (Status == true)
            {
                Foreground = TryFindResource("PrimaryBrush") as Brush;
            }
            else if (Status == false)
            {
                Foreground = TryFindResource("PrimaryTextBrush") as Brush;
            }
            else
            {
                Foreground = TryFindResource("ThirdlyTextBrush") as Brush;
            }
        }
    }
}