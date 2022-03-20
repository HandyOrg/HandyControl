//reference doc : https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.RelativePanel

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class RelativePanel : Panel
{
    private readonly Graph _childGraph;

    public RelativePanel() => _childGraph = new Graph();

    #region Panel alignment

    public static readonly DependencyProperty AlignLeftWithPanelProperty = DependencyProperty.RegisterAttached(
        "AlignLeftWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignLeftWithPanel(DependencyObject element, bool value)
        => element.SetValue(AlignLeftWithPanelProperty, ValueBoxes.BooleanBox(value));

    public static bool GetAlignLeftWithPanel(DependencyObject element)
        => (bool) element.GetValue(AlignLeftWithPanelProperty);

    public static readonly DependencyProperty AlignTopWithPanelProperty = DependencyProperty.RegisterAttached(
        "AlignTopWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignTopWithPanel(DependencyObject element, bool value)
        => element.SetValue(AlignTopWithPanelProperty, ValueBoxes.BooleanBox(value));

    public static bool GetAlignTopWithPanel(DependencyObject element)
        => (bool) element.GetValue(AlignTopWithPanelProperty);

    public static readonly DependencyProperty AlignRightWithPanelProperty = DependencyProperty.RegisterAttached(
        "AlignRightWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignRightWithPanel(DependencyObject element, bool value)
        => element.SetValue(AlignRightWithPanelProperty, ValueBoxes.BooleanBox(value));

    public static bool GetAlignRightWithPanel(DependencyObject element)
        => (bool) element.GetValue(AlignRightWithPanelProperty);

    public static readonly DependencyProperty AlignBottomWithPanelProperty = DependencyProperty.RegisterAttached(
        "AlignBottomWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignBottomWithPanel(DependencyObject element, bool value)
        => element.SetValue(AlignBottomWithPanelProperty, ValueBoxes.BooleanBox(value));

    public static bool GetAlignBottomWithPanel(DependencyObject element)
        => (bool) element.GetValue(AlignBottomWithPanelProperty);

    #endregion

    #region Sibling alignment

    public static readonly DependencyProperty AlignLeftWithProperty = DependencyProperty.RegisterAttached(
        "AlignLeftWith", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignLeftWith(DependencyObject element, UIElement value)
        => element.SetValue(AlignLeftWithProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetAlignLeftWith(DependencyObject element)
        => (UIElement) element.GetValue(AlignLeftWithProperty);

    public static readonly DependencyProperty AlignTopWithProperty = DependencyProperty.RegisterAttached(
        "AlignTopWith", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignTopWith(DependencyObject element, UIElement value)
        => element.SetValue(AlignTopWithProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetAlignTopWith(DependencyObject element)
        => (UIElement) element.GetValue(AlignTopWithProperty);

    public static readonly DependencyProperty AlignRightWithProperty = DependencyProperty.RegisterAttached(
        "AlignRightWith", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignRightWith(DependencyObject element, UIElement value)
        => element.SetValue(AlignRightWithProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetAlignRightWith(DependencyObject element)
        => (UIElement) element.GetValue(AlignRightWithProperty);

    public static readonly DependencyProperty AlignBottomWithProperty = DependencyProperty.RegisterAttached(
        "AlignBottomWith", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignBottomWith(DependencyObject element, UIElement value)
        => element.SetValue(AlignBottomWithProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetAlignBottomWith(DependencyObject element)
        => (UIElement) element.GetValue(AlignBottomWithProperty);

    #endregion

    #region Sibling positional

    public static readonly DependencyProperty LeftOfProperty = DependencyProperty.RegisterAttached(
        "LeftOf", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetLeftOf(DependencyObject element, UIElement value)
        => element.SetValue(LeftOfProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetLeftOf(DependencyObject element)
        => (UIElement) element.GetValue(LeftOfProperty);

    public static readonly DependencyProperty AboveProperty = DependencyProperty.RegisterAttached(
        "Above", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAbove(DependencyObject element, UIElement value)
        => element.SetValue(AboveProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetAbove(DependencyObject element)
        => (UIElement) element.GetValue(AboveProperty);

    public static readonly DependencyProperty RightOfProperty = DependencyProperty.RegisterAttached(
        "RightOf", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetRightOf(DependencyObject element, UIElement value)
        => element.SetValue(RightOfProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetRightOf(DependencyObject element)
        => (UIElement) element.GetValue(RightOfProperty);

    public static readonly DependencyProperty BelowProperty = DependencyProperty.RegisterAttached(
        "Below", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetBelow(DependencyObject element, UIElement value)
        => element.SetValue(BelowProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetBelow(DependencyObject element)
        => (UIElement) element.GetValue(BelowProperty);

    #endregion

    #region Center alignment

    public static readonly DependencyProperty AlignHorizontalCenterWithPanelProperty = DependencyProperty.RegisterAttached(
        "AlignHorizontalCenterWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignHorizontalCenterWithPanel(DependencyObject element, bool value)
        => element.SetValue(AlignHorizontalCenterWithPanelProperty, ValueBoxes.BooleanBox(value));

    public static bool GetAlignHorizontalCenterWithPanel(DependencyObject element)
        => (bool) element.GetValue(AlignHorizontalCenterWithPanelProperty);

    public static readonly DependencyProperty AlignVerticalCenterWithPanelProperty = DependencyProperty.RegisterAttached(
        "AlignVerticalCenterWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignVerticalCenterWithPanel(DependencyObject element, bool value)
        => element.SetValue(AlignVerticalCenterWithPanelProperty, ValueBoxes.BooleanBox(value));

    public static bool GetAlignVerticalCenterWithPanel(DependencyObject element)
        => (bool) element.GetValue(AlignVerticalCenterWithPanelProperty);

    public static readonly DependencyProperty AlignHorizontalCenterWithProperty = DependencyProperty.RegisterAttached(
        "AlignHorizontalCenterWith", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignHorizontalCenterWith(DependencyObject element, UIElement value)
        => element.SetValue(AlignHorizontalCenterWithProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetAlignHorizontalCenterWith(DependencyObject element)
        => (UIElement) element.GetValue(AlignHorizontalCenterWithProperty);

    public static readonly DependencyProperty AlignVerticalCenterWithProperty = DependencyProperty.RegisterAttached(
        "AlignVerticalCenterWith", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetAlignVerticalCenterWith(DependencyObject element, UIElement value)
        => element.SetValue(AlignVerticalCenterWithProperty, value);

    [TypeConverter(typeof(NameReferenceConverter))]
    public static UIElement GetAlignVerticalCenterWith(DependencyObject element)
        => (UIElement) element.GetValue(AlignVerticalCenterWithProperty);

    #endregion

    protected override Size MeasureOverride(Size availableSize)
    {
        #region Calc DesiredSize

        _childGraph.Clear();
        foreach (UIElement child in InternalChildren)
        {
            if (child == null) continue;
            var node = _childGraph.AddNode(child);

            node.AlignLeftWithNode = _childGraph.AddLink(node, GetAlignLeftWith(child));
            node.AlignTopWithNode = _childGraph.AddLink(node, GetAlignTopWith(child));
            node.AlignRightWithNode = _childGraph.AddLink(node, GetAlignRightWith(child));
            node.AlignBottomWithNode = _childGraph.AddLink(node, GetAlignBottomWith(child));

            node.LeftOfNode = _childGraph.AddLink(node, GetLeftOf(child));
            node.AboveNode = _childGraph.AddLink(node, GetAbove(child));
            node.RightOfNode = _childGraph.AddLink(node, GetRightOf(child));
            node.BelowNode = _childGraph.AddLink(node, GetBelow(child));

            node.AlignHorizontalCenterWith = _childGraph.AddLink(node, GetAlignHorizontalCenterWith(child));
            node.AlignVerticalCenterWith = _childGraph.AddLink(node, GetAlignVerticalCenterWith(child));
        }
        _childGraph.Measure(availableSize);

        #endregion

        #region Calc AvailableSize

        _childGraph.Reset(false);


        var calcWidth = Width.IsNaN() && HorizontalAlignment != HorizontalAlignment.Stretch;
        var calcHeight = Height.IsNaN() && VerticalAlignment != VerticalAlignment.Stretch;

        var boundingSize = _childGraph.GetBoundingSize(calcWidth, calcHeight);
        _childGraph.Reset();
        _childGraph.Measure(boundingSize);
        return boundingSize;

        #endregion
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        _childGraph.GetNodes().Do(node => node.Arrange(arrangeSize));
        return arrangeSize;
    }

    private class GraphNode
    {
        public bool Measured { get; set; }

        public UIElement Element { get; }

        private bool HorizontalOffsetFlag { get; set; }

        private bool VerticalOffsetFlag { get; set; }

        private Size BoundingSize { get; set; }

        public Size OriginDesiredSize { get; set; }

        public double Left { get; set; } = double.NaN;

        public double Top { get; set; } = double.NaN;

        public double Right { get; set; } = double.NaN;

        public double Bottom { get; set; } = double.NaN;

        public HashSet<GraphNode> OutgoingNodes { get; }

        public GraphNode AlignLeftWithNode { get; set; }

        public GraphNode AlignTopWithNode { get; set; }

        public GraphNode AlignRightWithNode { get; set; }

        public GraphNode AlignBottomWithNode { get; set; }

        public GraphNode LeftOfNode { get; set; }

        public GraphNode AboveNode { get; set; }

        public GraphNode RightOfNode { get; set; }

        public GraphNode BelowNode { get; set; }

        public GraphNode AlignHorizontalCenterWith { get; set; }

        public GraphNode AlignVerticalCenterWith { get; set; }

        public GraphNode(UIElement element)
        {
            OutgoingNodes = new HashSet<GraphNode>();
            Element = element;
        }

        public void Arrange(Size arrangeSize) => Element.Arrange(new Rect(Left, Top, Math.Max(arrangeSize.Width - Left - Right, 0), Math.Max(arrangeSize.Height - Top - Bottom, 0)));

        public void Reset(bool clearPos)
        {
            if (clearPos)
            {
                Left = double.NaN;
                Top = double.NaN;
                Right = double.NaN;
                Bottom = double.NaN;
            }
            Measured = false;
        }

        public Size GetBoundingSize()
        {
            if (Left < 0 || Top < 0) return default;
            if (Measured) return BoundingSize;

            if (!OutgoingNodes.Any())
            {
                BoundingSize = Element.DesiredSize;
                Measured = true;
            }
            else
            {
                BoundingSize = GetBoundingSize(this, Element.DesiredSize, OutgoingNodes);
                Measured = true;
            }

            return BoundingSize;
        }

        private static Size GetBoundingSize(GraphNode prevNode, Size prevSize, IEnumerable<GraphNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.Measured || !node.OutgoingNodes.Any())
                {
                    if (prevNode.LeftOfNode != null && prevNode.LeftOfNode == node ||
                        prevNode.RightOfNode != null && prevNode.RightOfNode == node)
                    {
                        prevSize.Width += node.BoundingSize.Width;
                        if (GetAlignHorizontalCenterWithPanel(node.Element) || node.HorizontalOffsetFlag)
                        {
                            prevSize.Width += prevNode.OriginDesiredSize.Width;
                            prevNode.HorizontalOffsetFlag = true;
                        }
                        if (node.VerticalOffsetFlag)
                        {
                            prevNode.VerticalOffsetFlag = true;
                        }
                    }

                    if (prevNode.AboveNode != null && prevNode.AboveNode == node ||
                        prevNode.BelowNode != null && prevNode.BelowNode == node)
                    {
                        prevSize.Height += node.BoundingSize.Height;
                        if (GetAlignVerticalCenterWithPanel(node.Element) || node.VerticalOffsetFlag)
                        {
                            prevSize.Height += prevNode.OriginDesiredSize.Height;
                            prevNode.VerticalOffsetFlag = true;
                        }
                        if (node.HorizontalOffsetFlag)
                        {
                            prevNode.HorizontalOffsetFlag = true;
                        }
                    }
                }
                else
                {
                    return GetBoundingSize(node, prevSize, node.OutgoingNodes);
                }
            }

            return prevSize;
        }
    }

    private class Graph
    {
        private readonly Dictionary<DependencyObject, GraphNode> _nodeDic;

        private Size AvailableSize { get; set; }

        public Graph() => _nodeDic = new Dictionary<DependencyObject, GraphNode>();

        public IEnumerable<GraphNode> GetNodes() => _nodeDic.Values;

        public void Clear()
        {
            AvailableSize = new Size();
            _nodeDic.Clear();
        }

        public void Reset(bool clearPos = true) => _nodeDic.Values.Do(node => node.Reset(clearPos));

        public GraphNode AddLink(GraphNode from, UIElement to)
        {
            if (to == null) return null;

            GraphNode nodeTo;
            if (_nodeDic.ContainsKey(to))
            {
                nodeTo = _nodeDic[to];
            }
            else
            {
                nodeTo = new GraphNode(to);
                _nodeDic[to] = nodeTo;
            }

            from.OutgoingNodes.Add(nodeTo);
            return nodeTo;
        }

        public GraphNode AddNode(UIElement value)
        {
            if (!_nodeDic.ContainsKey(value))
            {
                var node = new GraphNode(value);
                _nodeDic.Add(value, node);
                return node;
            }

            return _nodeDic[value];
        }

        public void Measure(Size availableSize)
        {
            AvailableSize = EnsureValidSize(availableSize);
            Measure(_nodeDic.Values, null);
        }

        private static Size EnsureValidSize(Size size)
        {
            var width = double.IsInfinity(size.Width) ? 0 : size.Width;
            var height = double.IsInfinity(size.Height) ? 0 : size.Height;

            return new Size(width, height);
        }

        private void Measure(IEnumerable<GraphNode> nodes, HashSet<DependencyObject> set)
        {
            set ??= new HashSet<DependencyObject>();

            foreach (var node in nodes)
            {
                /*
                 * 该节点无任何依赖，所以从这里开始计算元素位置。
                 * 因为无任何依赖，所以忽略同级元素
                 */
                if (!node.Measured && !node.OutgoingNodes.Any())
                {
                    MeasureChild(node);
                    continue;
                }

                //  判断依赖元素是否全部排列完毕
                if (node.OutgoingNodes.All(item => item.Measured))
                {
                    MeasureChild(node);
                    continue;
                }

                //  判断是否有循环
                if (!set.Add(node.Element)) throw new Exception("RelativePanel error: Circular dependency detected. Layout could not complete.");

                //  没有循环，且有依赖，则继续往下
                Measure(node.OutgoingNodes, set);

                if (!node.Measured)
                {
                    MeasureChild(node);
                }
            }
        }

        private void MeasureChild(GraphNode node)
        {
            var child = node.Element;
            child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            node.OriginDesiredSize = child.DesiredSize;

            var alignLeftWithPanel = GetAlignLeftWithPanel(child);
            var alignTopWithPanel = GetAlignTopWithPanel(child);
            var alignRightWithPanel = GetAlignRightWithPanel(child);
            var alignBottomWithPanel = GetAlignBottomWithPanel(child);

            #region Panel alignment

            if (alignLeftWithPanel) node.Left = 0;
            if (alignTopWithPanel) node.Top = 0;
            if (alignRightWithPanel) node.Right = 0;
            if (alignBottomWithPanel) node.Bottom = 0;

            #endregion

            #region Sibling alignment

            if (node.AlignLeftWithNode != null)
            {
                node.Left = node.Left.IsNaN() ? node.AlignLeftWithNode.Left : node.AlignLeftWithNode.Left * 0.5;
            }

            if (node.AlignTopWithNode != null)
            {
                node.Top = node.Top.IsNaN() ? node.AlignTopWithNode.Top : node.AlignTopWithNode.Top * 0.5;
            }

            if (node.AlignRightWithNode != null)
            {
                node.Right = node.Right.IsNaN()
                    ? node.AlignRightWithNode.Right
                    : node.AlignRightWithNode.Right * 0.5;
            }

            if (node.AlignBottomWithNode != null)
            {
                node.Bottom = node.Bottom.IsNaN()
                    ? node.AlignBottomWithNode.Bottom
                    : node.AlignBottomWithNode.Bottom * 0.5;
            }

            #endregion

            #region Measure

            var availableHeight = AvailableSize.Height - node.Top - node.Bottom;
            if (availableHeight.IsNaN())
            {
                availableHeight = AvailableSize.Height;

                if (!node.Top.IsNaN() && node.Bottom.IsNaN())
                {
                    availableHeight -= node.Top;
                }
                else if (node.Top.IsNaN() && !node.Bottom.IsNaN())
                {
                    availableHeight -= node.Bottom;
                }
            }

            var availableWidth = AvailableSize.Width - node.Left - node.Right;
            if (availableWidth.IsNaN())
            {
                availableWidth = AvailableSize.Width;

                if (!node.Left.IsNaN() && node.Right.IsNaN())
                {
                    availableWidth -= node.Left;
                }
                else if (node.Left.IsNaN() && !node.Right.IsNaN())
                {
                    availableWidth -= node.Right;
                }
            }

            child.Measure(new Size(Math.Max(availableWidth, 0), Math.Max(availableHeight, 0)));
            var childSize = child.DesiredSize;

            #endregion

            #region Sibling positional

            if (node.LeftOfNode != null && node.Left.IsNaN())
            {
                node.Left = node.LeftOfNode.Left - childSize.Width;
            }

            if (node.AboveNode != null && node.Top.IsNaN())
            {
                node.Top = node.AboveNode.Top - childSize.Height;
            }

            if (node.RightOfNode != null)
            {
                if (node.Right.IsNaN())
                {
                    node.Right = node.RightOfNode.Right - childSize.Width;
                }

                if (node.Left.IsNaN())
                {
                    node.Left = AvailableSize.Width - node.RightOfNode.Right;
                }
            }

            if (node.BelowNode != null)
            {
                if (node.Bottom.IsNaN())
                {
                    node.Bottom = node.BelowNode.Bottom - childSize.Height;
                }

                if (node.Top.IsNaN())
                {
                    node.Top = AvailableSize.Height - node.BelowNode.Bottom;
                }
            }

            #endregion

            #region Sibling-center alignment

            if (node.AlignHorizontalCenterWith != null)
            {
                var halfWidthLeft = (AvailableSize.Width + node.AlignHorizontalCenterWith.Left - node.AlignHorizontalCenterWith.Right - childSize.Width) * 0.5;
                var halfWidthRight = (AvailableSize.Width - node.AlignHorizontalCenterWith.Left + node.AlignHorizontalCenterWith.Right - childSize.Width) * 0.5;

                if (node.Left.IsNaN()) node.Left = halfWidthLeft;
                else node.Left = (node.Left + halfWidthLeft) * 0.5;

                if (node.Right.IsNaN()) node.Right = halfWidthRight;
                else node.Right = (node.Right + halfWidthRight) * 0.5;
            }

            if (node.AlignVerticalCenterWith != null)
            {
                var halfHeightTop = (AvailableSize.Height + node.AlignVerticalCenterWith.Top - node.AlignVerticalCenterWith.Bottom - childSize.Height) * 0.5;
                var halfHeightBottom = (AvailableSize.Height - node.AlignVerticalCenterWith.Top + node.AlignVerticalCenterWith.Bottom - childSize.Height) * 0.5;

                if (node.Top.IsNaN()) node.Top = halfHeightTop;
                else node.Top = (node.Top + halfHeightTop) * 0.5;

                if (node.Bottom.IsNaN()) node.Bottom = halfHeightBottom;
                else node.Bottom = (node.Bottom + halfHeightBottom) * 0.5;
            }

            #endregion

            #region Panel-center alignment

            if (GetAlignHorizontalCenterWithPanel(child))
            {
                var halfSubWidth = (AvailableSize.Width - childSize.Width) * 0.5;

                if (node.Left.IsNaN()) node.Left = halfSubWidth;
                else node.Left = (node.Left + halfSubWidth) * 0.5;

                if (node.Right.IsNaN()) node.Right = halfSubWidth;
                else node.Right = (node.Right + halfSubWidth) * 0.5;
            }

            if (GetAlignVerticalCenterWithPanel(child))
            {
                var halfSubHeight = (AvailableSize.Height - childSize.Height) * 0.5;

                if (node.Top.IsNaN()) node.Top = halfSubHeight;
                else node.Top = (node.Top + halfSubHeight) * 0.5;

                if (node.Bottom.IsNaN()) node.Bottom = halfSubHeight;
                else node.Bottom = (node.Bottom + halfSubHeight) * 0.5;
            }

            #endregion

            if (node.Left.IsNaN())
            {
                if (!node.Right.IsNaN())
                    node.Left = AvailableSize.Width - node.Right - childSize.Width;
                else
                {
                    node.Left = 0;
                    node.Right = AvailableSize.Width - childSize.Width;
                }
            }
            else if (!node.Left.IsNaN() && node.Right.IsNaN())
            {
                node.Right = AvailableSize.Width - node.Left - childSize.Width;
            }

            if (node.Top.IsNaN())
            {
                if (!node.Bottom.IsNaN())
                    node.Top = AvailableSize.Height - node.Bottom - childSize.Height;
                else
                {
                    node.Top = 0;
                    node.Bottom = AvailableSize.Height - childSize.Height;
                }
            }
            else if (!node.Top.IsNaN() && node.Bottom.IsNaN())
            {
                node.Bottom = AvailableSize.Height - node.Top - childSize.Height;
            }

            node.Measured = true;
        }

        public Size GetBoundingSize(bool calcWidth, bool calcHeight)
        {
            var boundingSize = new Size();

            foreach (var node in _nodeDic.Values)
            {
                var size = node.GetBoundingSize();
                boundingSize.Width = Math.Max(boundingSize.Width, size.Width);
                boundingSize.Height = Math.Max(boundingSize.Height, size.Height);
            }

            boundingSize.Width = calcWidth ? boundingSize.Width : AvailableSize.Width;
            boundingSize.Height = calcHeight ? boundingSize.Height : AvailableSize.Height;
            return boundingSize;
        }
    }
}
