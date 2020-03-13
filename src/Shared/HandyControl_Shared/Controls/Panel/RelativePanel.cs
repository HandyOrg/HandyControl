//reference doc : https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.RelativePanel

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class RelativePanel : Panel
    {
        private readonly Graph _childGraph;

        public RelativePanel() => _childGraph = new Graph();

        #region 容器对齐方式

        public static readonly DependencyProperty AlignLeftWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignLeftWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetAlignLeftWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignLeftWithPanelProperty, value);

        public static bool GetAlignLeftWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignLeftWithPanelProperty);

        public static readonly DependencyProperty AlignTopWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignTopWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetAlignTopWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignTopWithPanelProperty, value);

        public static bool GetAlignTopWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignTopWithPanelProperty);

        public static readonly DependencyProperty AlignRightWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignRightWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetAlignRightWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignRightWithPanelProperty, value);

        public static bool GetAlignRightWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignRightWithPanelProperty);

        public static readonly DependencyProperty AlignBottomWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignBottomWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetAlignBottomWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignBottomWithPanelProperty, value);

        public static bool GetAlignBottomWithPanel(DependencyObject element)
            => (bool) element.GetValue(AlignBottomWithPanelProperty);

        #endregion

        #region 元素间对齐方式

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

        #region 元素间位置关系

        public static readonly DependencyProperty LeftOfProperty = DependencyProperty.RegisterAttached(
            "LeftOf", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetLeftOf(DependencyObject element, UIElement value)
            => element.SetValue(LeftOfProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static UIElement GetLeftOf(DependencyObject element)
            => (UIElement)element.GetValue(LeftOfProperty);

        public static readonly DependencyProperty AboveProperty = DependencyProperty.RegisterAttached(
            "Above", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetAbove(DependencyObject element, UIElement value)
            => element.SetValue(AboveProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static UIElement GetAbove(DependencyObject element)
            => (UIElement)element.GetValue(AboveProperty);

        public static readonly DependencyProperty RightOfProperty = DependencyProperty.RegisterAttached(
            "RightOf", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetRightOf(DependencyObject element, UIElement value)
            => element.SetValue(RightOfProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static UIElement GetRightOf(DependencyObject element)
            => (UIElement)element.GetValue(RightOfProperty);

        public static readonly DependencyProperty BelowProperty = DependencyProperty.RegisterAttached(
            "Below", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetBelow(DependencyObject element, UIElement value)
            => element.SetValue(BelowProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static UIElement GetBelow(DependencyObject element)
            => (UIElement) element.GetValue(BelowProperty);

        #endregion

        #region 居中对齐方式

        public static readonly DependencyProperty AlignHorizontalCenterWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignHorizontalCenterWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetAlignHorizontalCenterWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignHorizontalCenterWithPanelProperty, value);

        public static bool GetAlignHorizontalCenterWithPanel(DependencyObject element)
            => (bool)element.GetValue(AlignHorizontalCenterWithPanelProperty);

        public static readonly DependencyProperty AlignVerticalCenterWithPanelProperty = DependencyProperty.RegisterAttached(
            "AlignVerticalCenterWithPanel", typeof(bool), typeof(RelativePanel), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetAlignVerticalCenterWithPanel(DependencyObject element, bool value)
            => element.SetValue(AlignVerticalCenterWithPanelProperty, value);

        public static bool GetAlignVerticalCenterWithPanel(DependencyObject element)
            => (bool)element.GetValue(AlignVerticalCenterWithPanelProperty);

        public static readonly DependencyProperty AlignHorizontalCenterWithProperty = DependencyProperty.RegisterAttached(
            "AlignHorizontalCenterWith", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetAlignHorizontalCenterWith(DependencyObject element, UIElement value)
            => element.SetValue(AlignHorizontalCenterWithProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static UIElement GetAlignHorizontalCenterWith(DependencyObject element)
            => (UIElement)element.GetValue(AlignHorizontalCenterWithProperty);

        public static readonly DependencyProperty AlignVerticalCenterWithProperty = DependencyProperty.RegisterAttached(
            "AlignVerticalCenterWith", typeof(UIElement), typeof(RelativePanel), new FrameworkPropertyMetadata(default(UIElement), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetAlignVerticalCenterWith(DependencyObject element, UIElement value)
            => element.SetValue(AlignVerticalCenterWithProperty, value);

        [TypeConverter(typeof(NameReferenceConverter))]
        public static UIElement GetAlignVerticalCenterWith(DependencyObject element)
            => (UIElement)element.GetValue(AlignVerticalCenterWithProperty);

        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child?.Measure(availableSize);
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            _childGraph.Reset(arrangeSize);

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

            if (_childGraph.CheckCyclic())
            {
                throw new Exception("RelativePanel error: Circular dependency detected. Layout could not complete.");
            }

            return arrangeSize;
        }

        private class GraphNode
        {
            public Point Position { get; set; }

            public bool Arranged { get; set; }

            public UIElement Element { get; }

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
        }

        private class Graph
        {
            private readonly Dictionary<DependencyObject, GraphNode> _nodeDic;

            private Size _arrangeSize;

            public Graph()
            {
                _nodeDic = new Dictionary<DependencyObject, GraphNode>();
            }

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

            public void Reset(Size arrangeSize)
            {
                _arrangeSize = arrangeSize;
                _nodeDic.Clear();
            }

            public bool CheckCyclic() => CheckCyclic(_nodeDic.Values, null);

            private bool CheckCyclic(IEnumerable<GraphNode> nodes, HashSet<DependencyObject> set)
            {
                if (set == null)
                {
                    set = new HashSet<DependencyObject>();
                }

                foreach (var node in nodes)
                {
                    /*
                     * 该节点无任何依赖，所以从这里开始计算元素位置。
                     * 因为无任何依赖，所以忽略同级元素
                     */
                    if (!node.Arranged && node.OutgoingNodes.Count == 0)
                    {
                        ArrangeChild(node, true);
                        continue;
                    }

                    //  判断依赖元素是否全部排列完毕
                    if (node.OutgoingNodes.All(item => item.Arranged))
                    {
                        ArrangeChild(node);
                        continue;
                    }

                    //  判断是否有循环
                    if (!set.Add(node.Element)) return true;

                    //  没有循环，且有依赖，则继续往下
                    return CheckCyclic(node.OutgoingNodes, set);
                }

                return false;
            }

            private void ArrangeChild(GraphNode node, bool ignoneSibling = false)
            {
                var child = node.Element;
                var childSize = child.DesiredSize;
                var childPos = new Point();

                #region 容器居中对齐

                if (GetAlignHorizontalCenterWithPanel(child))
                {
                    childPos.X = (_arrangeSize.Width - childSize.Width) / 2;
                }

                if (GetAlignVerticalCenterWithPanel(child))
                {
                    childPos.Y = (_arrangeSize.Height - childSize.Height) / 2;
                }

                #endregion

                var alignLeftWithPanel = GetAlignLeftWithPanel(child);
                var alignTopWithPanel = GetAlignTopWithPanel(child);
                var alignRightWithPanel = GetAlignRightWithPanel(child);
                var alignBottomWithPanel = GetAlignBottomWithPanel(child);

                if (!ignoneSibling)
                {
                    #region 元素间位置

                    if (node.LeftOfNode != null)
                    {
                        childPos.X = node.LeftOfNode.Position.X - childSize.Width;
                    }

                    if (node.AboveNode != null)
                    {
                        childPos.Y = node.AboveNode.Position.Y - childSize.Height;
                    }

                    if (node.RightOfNode != null)
                    {
                        childPos.X = node.RightOfNode.Position.X + node.RightOfNode.Element.DesiredSize.Width;
                    }

                    if (node.BelowNode != null)
                    {
                        childPos.Y = node.BelowNode.Position.Y + node.BelowNode.Element.DesiredSize.Height;
                    }

                    #endregion

                    #region 元素居中对齐

                    if (node.AlignHorizontalCenterWith != null)
                    {
                        childPos.X = node.AlignHorizontalCenterWith.Position.X +
                                     (node.AlignHorizontalCenterWith.Element.DesiredSize.Width - childSize.Width) / 2;
                    }

                    if (node.AlignVerticalCenterWith != null)
                    {
                        childPos.Y = node.AlignVerticalCenterWith.Position.Y +
                                     (node.AlignVerticalCenterWith.Element.DesiredSize.Height - childSize.Height) / 2;
                    }

                    #endregion

                    #region 元素间对齐

                    if (node.AlignLeftWithNode != null)
                    {
                        childPos.X = node.AlignLeftWithNode.Position.X;
                    }

                    if (node.AlignTopWithNode != null)
                    {
                        childPos.Y = node.AlignTopWithNode.Position.Y;
                    }

                    if (node.AlignRightWithNode != null)
                    {
                        childPos.X = node.AlignRightWithNode.Element.DesiredSize.Width + node.AlignRightWithNode.Position.X - childSize.Width;
                    }

                    if (node.AlignBottomWithNode != null)
                    {
                        childPos.Y = node.AlignBottomWithNode.Element.DesiredSize.Height + node.AlignBottomWithNode.Position.Y - childSize.Height;
                    }

                    #endregion
                }
                
                #region 容器对齐

                if (alignLeftWithPanel)
                {
                    if (node.AlignRightWithNode != null)
                    {
                        childPos.X = (node.AlignRightWithNode.Element.DesiredSize.Width + node.AlignRightWithNode.Position.X - childSize.Width) / 2;
                    }
                    else
                    {
                        childPos.X = 0;
                    }
                }

                if (alignTopWithPanel)
                {
                    if (node.AlignBottomWithNode != null)
                    {
                        childPos.Y = (node.AlignBottomWithNode.Element.DesiredSize.Height + node.AlignBottomWithNode.Position.Y - childSize.Height) / 2;
                    }
                    else
                    {
                        childPos.Y = 0;
                    }
                }

                if (alignRightWithPanel)
                {
                    if (alignLeftWithPanel)
                    {
                        childPos.X = (_arrangeSize.Width - childSize.Width) / 2;
                    }
                    else if(node.AlignLeftWithNode == null)
                    {
                        childPos.X = _arrangeSize.Width - childSize.Width;
                    }
                    else
                    {
                        childPos.X = (_arrangeSize.Width + node.AlignLeftWithNode.Position.X - childSize.Width) / 2;
                    }
                }

                if (alignBottomWithPanel)
                {
                    if (alignTopWithPanel)
                    {
                        childPos.Y = (_arrangeSize.Height - childSize.Height) / 2;
                    }
                    else if (node.AlignTopWithNode == null)
                    {
                        childPos.Y = _arrangeSize.Height - childSize.Height;
                    }
                    else
                    {
                        childPos.Y = (_arrangeSize.Height + node.AlignLeftWithNode.Position.Y - childSize.Height) / 2;
                    }
                }

                #endregion

                child.Arrange(new Rect(childPos.X, childPos.Y, childSize.Width, childSize.Height));
                node.Position = childPos;
                node.Arranged = true;
            }
        }
    }
}
