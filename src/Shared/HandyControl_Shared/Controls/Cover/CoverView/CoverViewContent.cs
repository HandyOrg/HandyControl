using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementTriangle, Type = typeof(FrameworkElement))]
[TemplatePart(Name = ElementContent, Type = typeof(FrameworkElement))]
public class CoverViewContent : ContentControl
{
    private const string ElementTriangle = "PART_Triangle";

    private const string ElementContent = "PART_Content";

    private FrameworkElement _triangle;

    private FrameworkElement _content;

    internal bool WaitForUpdate { get; set; }

    private int _index;

    private int _groups;

    private double _itemWidth;

    internal bool CanSwitch { get; set; } = true;

    private bool _isOpen;

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

    internal static readonly DependencyProperty ManualHeightProperty = DependencyProperty.Register(
        nameof(ManualHeight), typeof(double), typeof(CoverViewContent), new PropertyMetadata(ValueBoxes.Double0Box), ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

    internal double ManualHeight
    {
        get => (double) GetValue(ManualHeightProperty);
        set => SetValue(ManualHeightProperty, value);
    }

    public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(
        nameof(ContentHeight), typeof(double), typeof(CoverViewContent), new PropertyMetadata(ValueBoxes.Double300Box),
        ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

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
        if (IsOpen)
        {
            if (ManualHeight > 0 && !MathHelper.AreClose(ManualHeight, ContentHeight))
            {
                _content.BeginAnimation(HeightProperty, AnimationHelper.CreateAnimation(ManualHeight));
            }
            else
            {
                _content.BeginAnimation(HeightProperty, AnimationHelper.CreateAnimation(ContentHeight));
            }
        }
    }

    private void OpenSwitch(bool isOpen)
    {
        if (_content == null) return;
        var animation = AnimationHelper.CreateAnimation(isOpen ? ManualHeight > 0 ? ManualHeight : ContentHeight : 0);
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
