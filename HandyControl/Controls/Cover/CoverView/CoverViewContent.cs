using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementTriangle, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementContent, Type = typeof(FrameworkElement))]
    internal class CoverViewContent : ContentControl
    {
        private const string ElementTriangle = "PART_Triangle";

        private const string ElementContent = "PART_Content";

        private FrameworkElement _triangle;

        private FrameworkElement _content;

        internal bool WaitForUpdate { get; set; }

        private int _index;

        private int _groups;

        private double _itemWidth;

        private bool _isOpen;

        internal bool CanSwitch { get; set; } = true;

        internal bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen == value) return;
                _isOpen = value;
                OpenSwitch(value);
            }
        }

        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(
            "ContentHeight", typeof(double), typeof(CoverViewContent), new PropertyMetadata(ValueBoxes.Double300Box));

        public double ContentHeight
        {
            get => (double) GetValue(ContentHeightProperty);
            set => SetValue(ContentHeightProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _triangle = GetTemplateChild(ElementTriangle) as FrameworkElement;
            _content = GetTemplateChild(ElementContent) as FrameworkElement;

            if (WaitForUpdate)
            {
                _triangle.BeginAnimation(MarginProperty, AnimationHelper.CreateAnimation(new Thickness((_index % _groups + .5) * _itemWidth - _triangle.Width / 2, 0, 0, 0)));
                OpenSwitch(_isOpen);
                WaitForUpdate = false;
            }
        }

        internal void UpdatePosition(int index, int groups, double itemWidth)
        {
            if (_triangle == null)
            {
                _index = index;
                _groups = groups;
                _itemWidth = itemWidth;
                WaitForUpdate = true;
                return;
            }
            _triangle.BeginAnimation(MarginProperty, AnimationHelper.CreateAnimation(new Thickness((index % groups + .5) * itemWidth - _triangle.Width / 2, 0, 0, 0)));
        }

        private void OpenSwitch(bool isOpen)
        {
            if (_content == null) return;
            var animation = AnimationHelper.CreateAnimation(isOpen ? ContentHeight : 0);
            _triangle.Show(false);
            this.Show(true);
            animation.Completed += (s, e) =>
            {
                CanSwitch = true;
                this.Show(IsOpen);
                if (IsOpen)
                {
                    _triangle.Show(true);
                }
            };
            CanSwitch = false;
            _content.BeginAnimation(HeightProperty, animation);
        }
    }
}