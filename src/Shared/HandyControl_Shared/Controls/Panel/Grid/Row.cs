using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class Row : Panel
{
    private ColLayoutStatus _layoutStatus;

    private double _maxChildDesiredHeight;

    private double _fixedWidth;

    public static readonly DependencyProperty GutterProperty = DependencyProperty.Register(
        nameof(Gutter), typeof(double), typeof(Row), new FrameworkPropertyMetadata(
            ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure, null, OnGutterCoerce),
        ValidateHelper.IsInRangeOfPosDoubleIncludeZero);

    private static object OnGutterCoerce(DependencyObject d, object basevalue) =>
        ValidateHelper.IsInRangeOfPosDoubleIncludeZero(basevalue) ? basevalue : .0;

    public double Gutter
    {
        get => (double) GetValue(GutterProperty);
        set => SetValue(GutterProperty, value);
    }

    // 当视觉父发生变化，订阅父的 SizeChanged / LayoutUpdated，确保在父确定尺寸后重新测量
    protected override void OnVisualParentChanged(DependencyObject oldParent)
    {
        base.OnVisualParentChanged(oldParent);
        UnsubscribeParentSizeChanged(oldParent as FrameworkElement);

        if (VisualParent is FrameworkElement newParent)
        {
            // 如果父已经有具体宽度，可以立即 InvalidateMeasure
            if (newParent.ActualWidth > 0)
            {
                InvalidateMeasure();
            }
            else
            {
                // 等待父首次布局完成
                newParent.SizeChanged += Parent_SizeChanged;
                newParent.LayoutUpdated += Parent_LayoutUpdated;
            }
        }
    }

    private void UnsubscribeParentSizeChanged(FrameworkElement parent)
    {
        if (parent == null) return;
        parent.SizeChanged -= Parent_SizeChanged;
        parent.LayoutUpdated -= Parent_LayoutUpdated;
    }

    private void Parent_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (sender is FrameworkElement parent && parent.ActualWidth > 0)
        {
            parent.SizeChanged -= Parent_SizeChanged;
            parent.LayoutUpdated -= Parent_LayoutUpdated;
            // 延迟到渲染优先级再触发重测，确保父已稳定
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Render);
        }
    }

    private void Parent_LayoutUpdated(object sender, EventArgs e)
    {
        if (sender is FrameworkElement parent && parent.ActualWidth > 0)
        {
            parent.SizeChanged -= Parent_SizeChanged;
            parent.LayoutUpdated -= Parent_LayoutUpdated;
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Render);
        }
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var gutter = Gutter;

        // 如果父没有给出有限宽度，尝试用父 ActualWidth
        if (double.IsInfinity(constraint.Width) || constraint.Width == 0)
        {
            if (Parent is FrameworkElement parent && parent.ActualWidth > 0)
            {
                constraint = new Size(parent.ActualWidth, constraint.Height);
            }
            else
            {
                // 延迟再测量一帧，避免返回错误测量尺寸被上层缓存
                Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Render);
                // 返回一个合理的最小高度（尽量不要返回 0 宽，否则会导致上层布局认为没有宽度）
                return new Size(constraint.Width, 0);
            }
        }

        // 在 Measure 阶段应该自己根据 constraint 计算 layoutStatus，而不要依赖 _layoutStatus（它在 Arrange 中被设置）
        var localLayoutStatus = ColLayout.GetLayoutStatus(constraint.Width);

        var totalCellCount = 0;
        var totalRowCount = 1;
        _fixedWidth = 0;
        _maxChildDesiredHeight = 0;
        var cols = InternalChildren.OfType<Col>().ToList();

        // 先测量固定或不参与栅格的子元素，累加 fixed 宽度与最大高度
        foreach (var child in cols)
        {
            var cellCount = child.GetLayoutCellCount(localLayoutStatus);
            if (cellCount == 0 || child.IsFixed)
            {
                child.Measure(constraint);
                _maxChildDesiredHeight = Math.Max(_maxChildDesiredHeight, child.DesiredSize.Height);
                _fixedWidth += child.DesiredSize.Width + gutter;
            }
        }

        var availableWidth = Math.Max(0, constraint.Width - _fixedWidth + gutter);
        var itemWidth = availableWidth / ColLayout.ColMaxCellCount;
        itemWidth = Math.Max(0, itemWidth);

        foreach (var child in cols)
        {
            var cellCount = child.GetLayoutCellCount(localLayoutStatus);
            if (cellCount > 0 && !child.IsFixed)
            {
                totalCellCount += cellCount;
                var availableChildWidth = Math.Max(0, cellCount * itemWidth - gutter);

                child.Measure(new Size(availableChildWidth, constraint.Height));
                _maxChildDesiredHeight = Math.Max(_maxChildDesiredHeight, child.DesiredSize.Height);

                if (totalCellCount > ColLayout.ColMaxCellCount)
                {
                    totalCellCount = cellCount;
                    totalRowCount++;
                }
            }
        }

        // 返回宽度尽量用 constraint.Width（如果是 Infinity 则尝试 parent.ActualWidth），避免返回 0 宽度
        var returnWidth = double.IsInfinity(constraint.Width) ? (Parent is FrameworkElement p ? p.ActualWidth : 0) : constraint.Width;
        return new Size(returnWidth, _maxChildDesiredHeight * totalRowCount);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var gutter = Gutter;
        var totalCellCount = 0;
        var cols = InternalChildren.OfType<Col>().ToList();

        // 再次设置布局状态，供 Arrange 使用
        _layoutStatus = ColLayout.GetLayoutStatus(finalSize.Width);

        var itemWidth = (finalSize.Width - _fixedWidth + gutter) / ColLayout.ColMaxCellCount;
        itemWidth = Math.Max(0, itemWidth);

        var childBounds = new Rect(0, 0, 0, _maxChildDesiredHeight);

        foreach (var child in cols)
        {
            if (!child.IsVisible)
            {
                continue;
            }

            var cellCount = child.GetLayoutCellCount(_layoutStatus);
            totalCellCount += cellCount;

            var childWidth = (cellCount > 0 && !child.IsFixed) ? Math.Max(0, cellCount * itemWidth - gutter) : child.DesiredSize.Width;
            childBounds.Width = childWidth;
            childBounds.X += child.Offset * itemWidth;

            if (totalCellCount > ColLayout.ColMaxCellCount)
            {
                childBounds.X = 0;
                childBounds.Y += _maxChildDesiredHeight;
                totalCellCount = cellCount;
            }

            child.Arrange(childBounds);
            childBounds.X += childWidth + gutter;
        }

        return finalSize;
    }
}
