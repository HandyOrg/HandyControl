using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Tools;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    ///     步骤条单元项
    /// </summary>
    public class StepItem : Control
    {
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
            get => (string)GetValue(IndexStrProperty);
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
                Foreground = ResourceHelper.GetResource<Brush>("PrimaryTextBrush");
            }
            else if (Status == false)
            {
                Foreground = ResourceHelper.GetResource<Brush>("PrimaryBrush");
            }
            else
            {
                Foreground = ResourceHelper.GetResource<Brush>("ThirdlyTextBrush");
            }
        }
    }
}