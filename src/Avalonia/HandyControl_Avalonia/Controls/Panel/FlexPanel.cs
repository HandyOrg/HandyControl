using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.VisualTree;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class FlexPanel : Panel
{
    private UVSize _uvConstraint;

    private int _lineCount;

    private readonly List<FlexItemInfo> _orderList = new();

    #region Item

    public static readonly AttachedProperty<int> OrderProperty =
        AvaloniaProperty.RegisterAttached<FlexPanel, Control, int>("Order");

    public static void SetOrder(Control element, int value) => element.SetValue(OrderProperty, value);

    public static int GetOrder(Control element) => element.GetValue(OrderProperty);

    public static readonly AttachedProperty<double> FlexGrowProperty =
        AvaloniaProperty.RegisterAttached<FlexPanel, Control, double>("FlexGrow", validate: IsPositiveOrZero);

    public static void SetFlexGrow(Control element, double value) => element.SetValue(FlexGrowProperty, value);

    public static double GetFlexGrow(Control element) => element.GetValue(FlexGrowProperty);

    public static readonly AttachedProperty<double> FlexShrinkProperty =
        AvaloniaProperty.RegisterAttached<FlexPanel, Control, double>("FlexShrink", defaultValue: 1d, validate: IsPositiveOrZero);

    public static void SetFlexShrink(Control element, double value) => element.SetValue(FlexShrinkProperty, value);

    public static double GetFlexShrink(Control element) => element.GetValue(FlexShrinkProperty);

    public static readonly AttachedProperty<double> FlexBasisProperty =
        AvaloniaProperty.RegisterAttached<FlexPanel, Control, double>("FlexBasis", defaultValue: double.NaN);

    public static void SetFlexBasis(Control element, double value) => element.SetValue(FlexBasisProperty, value);

    public static double GetFlexBasis(Control element) => element.GetValue(FlexBasisProperty);

    public static readonly AttachedProperty<FlexItemAlignment> AlignSelfProperty =
        AvaloniaProperty.RegisterAttached<FlexPanel, Control, FlexItemAlignment>("AlignSelf");

    public static void SetAlignSelf(Control element, FlexItemAlignment value) => element.SetValue(AlignSelfProperty, value);

    public static FlexItemAlignment GetAlignSelf(Control element) => element.GetValue(AlignSelfProperty);

    private static bool IsPositiveOrZero(double value) => !double.IsNaN(value) && value >= 0;

    #endregion

    #region Panel

    public static readonly StyledProperty<FlexDirection> FlexDirectionProperty =
        AvaloniaProperty.Register<FlexPanel, FlexDirection>(nameof(FlexDirection));

    public FlexDirection FlexDirection
    {
        get => GetValue(FlexDirectionProperty);
        set => SetValue(FlexDirectionProperty, value);
    }

    public static readonly StyledProperty<FlexWrap> FlexWrapProperty =
        AvaloniaProperty.Register<FlexPanel, FlexWrap>(nameof(FlexWrap));

    public FlexWrap FlexWrap
    {
        get => GetValue(FlexWrapProperty);
        set => SetValue(FlexWrapProperty, value);
    }

    public static readonly StyledProperty<FlexContentJustify> JustifyContentProperty =
        AvaloniaProperty.Register<FlexPanel, FlexContentJustify>(nameof(JustifyContent));

    public FlexContentJustify JustifyContent
    {
        get => GetValue(JustifyContentProperty);
        set => SetValue(JustifyContentProperty, value);
    }

    public static readonly StyledProperty<FlexItemsAlignment> AlignItemsProperty =
        AvaloniaProperty.Register<FlexPanel, FlexItemsAlignment>(nameof(AlignItems));

    public FlexItemsAlignment AlignItems
    {
        get => GetValue(AlignItemsProperty);
        set => SetValue(AlignItemsProperty, value);
    }

    public static readonly StyledProperty<FlexContentAlignment> AlignContentProperty =
        AvaloniaProperty.Register<FlexPanel, FlexContentAlignment>(nameof(AlignContent));

    public FlexContentAlignment AlignContent
    {
        get => GetValue(AlignContentProperty);
        set => SetValue(AlignContentProperty, value);
    }

    #endregion

    static FlexPanel()
    {
        AffectsMeasure<FlexPanel>(
            FlexDirectionProperty,
            FlexWrapProperty,
            JustifyContentProperty,
            AlignItemsProperty,
            AlignContentProperty
        );

        OrderProperty.Changed.AddClassHandler<Control>(OnItemPropertyChanged);
        FlexGrowProperty.Changed.AddClassHandler<Control>(OnItemPropertyChanged);
        FlexShrinkProperty.Changed.AddClassHandler<Control>(OnItemPropertyChanged);
        FlexBasisProperty.Changed.AddClassHandler<Control>(OnItemPropertyChanged);
        AlignSelfProperty.Changed.AddClassHandler<Control>(OnItemPropertyChanged);
    }

    private static void OnItemPropertyChanged(Control element, AvaloniaPropertyChangedEventArgs e)
    {
        if (element.GetVisualParent() is FlexPanel p)
        {
            p.InvalidateMeasure();
        }
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var flexDirection = FlexDirection;
        var flexWrap = FlexWrap;

        var curLineSize = new UVSize(flexDirection);
        var panelSize = new UVSize(flexDirection);
        _uvConstraint = new UVSize(flexDirection, constraint);
        var childConstraint = new Size(constraint.Width, constraint.Height);
        _lineCount = 1;
        var children = Children;

        _orderList.Clear();
        for (var i = 0; i < children.Count; i++)
        {
            var child = children[i];
            if (child == null) continue;

            _orderList.Add(new FlexItemInfo(i, GetOrder(child)));
        }

        _orderList.Sort();

        for (var i = 0; i < children.Count; i++)
        {
            var child = children[_orderList[i].Index];
            if (child == null) continue;

            var flexBasis = GetFlexBasis(child);
            if (!flexBasis.IsNaN())
            {
                child.SetCurrentValue(Layoutable.WidthProperty, flexBasis);
            }
            child.Measure(childConstraint);

            var sz = new UVSize(flexDirection, child.DesiredSize);

            if (flexWrap == FlexWrap.NoWrap)
            {
                curLineSize.U += sz.U;
                curLineSize.V = Math.Max(sz.V, curLineSize.V);
            }
            else
            {
                if (MathHelper.GreaterThan(curLineSize.U + sz.U, _uvConstraint.U))
                {
                    panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                    curLineSize = sz;
                    _lineCount++;

                    if (MathHelper.GreaterThan(sz.U, _uvConstraint.U))
                    {
                        panelSize.U = Math.Max(sz.U, panelSize.U);
                        panelSize.V += sz.V;
                        curLineSize = new UVSize(flexDirection);
                        _lineCount++;
                    }
                }
                else
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                }
            }
        }

        panelSize.U = Math.Max(curLineSize.U, panelSize.U);
        panelSize.V += curLineSize.V;

        return new Size(panelSize.Width, panelSize.Height);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        var flexDirection = FlexDirection;
        var flexWrap = FlexWrap;
        var alignContent = AlignContent;

        var uvFinalSize = new UVSize(flexDirection, arrangeSize);
        if (MathHelper.IsZero(uvFinalSize.U) || MathHelper.IsZero(uvFinalSize.V)) return arrangeSize;

        var children = Children;
        var lineIndex = 0;

        var curLineSizeArr = new UVSize[_lineCount];
        curLineSizeArr[0] = new UVSize(flexDirection);

        var lastInLineArr = new int[_lineCount];
        for (var i = 0; i < _lineCount; i++)
        {
            lastInLineArr[i] = int.MaxValue;
        }

        for (var i = 0; i < children.Count; i++)
        {
            var child = children[_orderList[i].Index];
            if (child == null) continue;

            var sz = new UVSize(flexDirection, child.DesiredSize);

            if (flexWrap == FlexWrap.NoWrap)
            {
                curLineSizeArr[lineIndex].U += sz.U;
                curLineSizeArr[lineIndex].V = Math.Max(sz.V, curLineSizeArr[lineIndex].V);
            }
            else
            {
                if (MathHelper.GreaterThan(curLineSizeArr[lineIndex].U + sz.U, uvFinalSize.U))
                {
                    lastInLineArr[lineIndex] = i;
                    lineIndex++;
                    curLineSizeArr[lineIndex] = sz;

                    if (MathHelper.GreaterThan(sz.U, uvFinalSize.U))
                    {
                        lastInLineArr[lineIndex] = i;
                        lineIndex++;
                        curLineSizeArr[lineIndex] = new UVSize(flexDirection);
                    }
                }
                else
                {
                    curLineSizeArr[lineIndex].U += sz.U;
                    curLineSizeArr[lineIndex].V = Math.Max(sz.V, curLineSizeArr[lineIndex].V);
                }
            }
        }

        var scaleU = Math.Min(_uvConstraint.U / uvFinalSize.U, 1);
        var firstInLine = 0;
        var wrapReverseAdd = 0;
        var wrapReverseFlag = flexWrap == FlexWrap.WrapReverse ? -1 : 1;
        var accumulatedFlag = flexWrap == FlexWrap.WrapReverse ? 1 : 0;
        var itemsU = .0;
        var accumulatedV = .0;
        var freeV = uvFinalSize.V;
        foreach (var flexSize in curLineSizeArr)
        {
            freeV -= flexSize.V;
        }

        var freeItemV = freeV;

        var lineFreeVArr = new double[_lineCount];
        switch (alignContent)
        {
            case FlexContentAlignment.Stretch:
                if (_lineCount > 1)
                {
                    freeItemV = freeV / _lineCount;
                    for (var i = 0; i < _lineCount; i++)
                    {
                        lineFreeVArr[i] = freeItemV;
                    }

                    accumulatedV = flexWrap == FlexWrap.WrapReverse ? uvFinalSize.V - curLineSizeArr[0].V - lineFreeVArr[0] : 0;
                }

                break;
            case FlexContentAlignment.FlexStart:
                wrapReverseAdd = flexWrap == FlexWrap.WrapReverse ? 0 : 1;
                if (_lineCount > 1)
                {
                    accumulatedV = flexWrap == FlexWrap.WrapReverse ? uvFinalSize.V - curLineSizeArr[0].V : 0;
                }
                else
                {
                    wrapReverseAdd = 0;
                }

                break;
            case FlexContentAlignment.FlexEnd:
                wrapReverseAdd = flexWrap == FlexWrap.WrapReverse ? 1 : 0;
                if (_lineCount > 1)
                {
                    accumulatedV = flexWrap == FlexWrap.WrapReverse ? uvFinalSize.V - curLineSizeArr[0].V - freeV : freeV;
                }
                else
                {
                    wrapReverseAdd = 0;
                }

                break;
            case FlexContentAlignment.Center:
                if (_lineCount > 1)
                {
                    accumulatedV = flexWrap == FlexWrap.WrapReverse ? uvFinalSize.V - curLineSizeArr[0].V - freeV * 0.5 : freeV * 0.5;
                }

                break;
            case FlexContentAlignment.SpaceBetween:
                if (_lineCount > 1)
                {
                    freeItemV = freeV / (_lineCount - 1);
                    for (var i = 0; i < _lineCount - 1; i++)
                    {
                        lineFreeVArr[i] = freeItemV;
                    }

                    accumulatedV = flexWrap == FlexWrap.WrapReverse ? uvFinalSize.V - curLineSizeArr[0].V : 0;
                }

                break;
            case FlexContentAlignment.SpaceAround:
                if (_lineCount > 1)
                {
                    freeItemV = freeV / _lineCount * 0.5;
                    for (var i = 0; i < _lineCount - 1; i++)
                    {
                        lineFreeVArr[i] = freeItemV * 2;
                    }

                    accumulatedV = flexWrap == FlexWrap.WrapReverse ? uvFinalSize.V - curLineSizeArr[0].V - freeItemV : freeItemV;
                }

                break;
        }

        lineIndex = 0;

        for (var i = 0; i < children.Count; i++)
        {
            var child = children[_orderList[i].Index];
            if (child == null) continue;

            var sz = new UVSize(flexDirection, child.DesiredSize);

            if (flexWrap != FlexWrap.NoWrap)
            {
                if (i >= lastInLineArr[lineIndex])
                {
                    ArrangeLine(new FlexLineInfo
                    {
                        ItemsU = itemsU,
                        OffsetV = accumulatedV + freeItemV * wrapReverseAdd,
                        LineV = curLineSizeArr[lineIndex].V,
                        LineFreeV = freeItemV,
                        LineU = uvFinalSize.U,
                        ItemStartIndex = firstInLine,
                        ItemEndIndex = i,
                        ScaleU = scaleU
                    });

                    accumulatedV += (lineFreeVArr[lineIndex] + curLineSizeArr[lineIndex + accumulatedFlag].V) * wrapReverseFlag;
                    lineIndex++;
                    itemsU = 0;

                    if (i >= lastInLineArr[lineIndex])
                    {
                        ArrangeLine(new FlexLineInfo
                        {
                            ItemsU = itemsU,
                            OffsetV = accumulatedV + freeItemV * wrapReverseAdd,
                            LineV = curLineSizeArr[lineIndex].V,
                            LineFreeV = freeItemV,
                            LineU = uvFinalSize.U,
                            ItemStartIndex = i,
                            ItemEndIndex = ++i,
                            ScaleU = scaleU
                        });

                        accumulatedV += (lineFreeVArr[lineIndex] + curLineSizeArr[lineIndex + accumulatedFlag].V) * wrapReverseFlag;
                        lineIndex++;
                        itemsU = 0;
                    }

                    firstInLine = i;
                }
            }

            itemsU += sz.U;
        }

        if (firstInLine < children.Count)
        {
            ArrangeLine(new FlexLineInfo
            {
                ItemsU = itemsU,
                OffsetV = accumulatedV + freeItemV * wrapReverseAdd,
                LineV = curLineSizeArr[lineIndex].V,
                LineFreeV = freeItemV,
                LineU = uvFinalSize.U,
                ItemStartIndex = firstInLine,
                ItemEndIndex = children.Count,
                ScaleU = scaleU
            });
        }

        return arrangeSize;
    }

    private void ArrangeLine(FlexLineInfo lineInfo)
    {
        var flexDirection = FlexDirection;
        var flexWrap = FlexWrap;
        var justifyContent = JustifyContent;
        var alignItems = AlignItems;

        var isHorizontal = flexDirection == FlexDirection.Row || flexDirection == FlexDirection.RowReverse;
        var isReverse = flexDirection == FlexDirection.RowReverse || flexDirection == FlexDirection.ColumnReverse;
        var itemCount = lineInfo.ItemEndIndex - lineInfo.ItemStartIndex;
        if (itemCount <= 0) return;
        var children = Children;
        var lineFreeU = lineInfo.LineU - lineInfo.ItemsU;
        var constraintFreeU = _uvConstraint.U - lineInfo.ItemsU;

        var u = .0;
        if (isReverse)
        {
            u = justifyContent switch
            {
                FlexContentJustify.FlexStart => lineInfo.LineU,
                FlexContentJustify.SpaceBetween => lineInfo.LineU,
                FlexContentJustify.SpaceAround => lineInfo.LineU,
                FlexContentJustify.FlexEnd => lineInfo.ItemsU,
                FlexContentJustify.Center => (lineInfo.LineU + lineInfo.ItemsU) * 0.5,
                _ => u
            };
        }
        else
        {
            u = justifyContent switch
            {
                FlexContentJustify.FlexEnd => lineFreeU,
                FlexContentJustify.Center => lineFreeU * 0.5,
                _ => u
            };
        }

        u *= lineInfo.ScaleU;

        var flexGrowUArr = new double[itemCount];
        if (constraintFreeU > 0)
        {
            var ignoreFlexGrow = true;
            var flexGrowSum = .0;

            for (var i = 0; i < itemCount; i++)
            {
                var flexGrow = GetFlexGrow(children[_orderList[i].Index]);
                ignoreFlexGrow &= MathHelper.IsVerySmall(flexGrow);
                flexGrowUArr[i] = flexGrow;
                flexGrowSum += flexGrow;
            }

            if (!ignoreFlexGrow)
            {
                var flexGrowItem = .0;
                if (flexGrowSum > 0)
                {
                    flexGrowItem = constraintFreeU / flexGrowSum;
                    lineInfo.ScaleU = 1;
                    lineFreeU = 0;
                }

                for (var i = 0; i < itemCount; i++)
                {
                    flexGrowUArr[i] *= flexGrowItem;
                }
            }
            else
            {
                flexGrowUArr = new double[itemCount];
            }
        }

        var flexShrinkUArr = new double[itemCount];
        if (constraintFreeU < 0)
        {
            var ignoreFlexShrink = true;
            var flexShrinkSum = .0;

            for (var i = 0; i < itemCount; i++)
            {
                var flexShrink = GetFlexShrink(children[_orderList[i].Index]);
                ignoreFlexShrink &= MathHelper.IsVerySmall(flexShrink - 1);
                flexShrinkUArr[i] = flexShrink;
                flexShrinkSum += flexShrink;
            }

            if (!ignoreFlexShrink)
            {
                var flexShrinkItem = .0;
                if (flexShrinkSum > 0)
                {
                    flexShrinkItem = constraintFreeU / flexShrinkSum;
                    lineInfo.ScaleU = 1;
                    lineFreeU = 0;
                }

                for (var i = 0; i < itemCount; i++)
                {
                    flexShrinkUArr[i] *= flexShrinkItem;
                }
            }
            else
            {
                flexShrinkUArr = new double[itemCount];
            }
        }

        var offsetUArr = new double[itemCount];
        if (lineFreeU > 0)
        {
            if (justifyContent == FlexContentJustify.SpaceBetween)
            {
                var freeItemU = lineFreeU / (itemCount - 1);
                for (var i = 1; i < itemCount; i++)
                {
                    offsetUArr[i] = freeItemU;
                }
            }
            else if (justifyContent == FlexContentJustify.SpaceAround)
            {
                var freeItemU = lineFreeU / itemCount * 0.5;
                offsetUArr[0] = freeItemU;
                for (var i = 1; i < itemCount; i++)
                {
                    offsetUArr[i] = freeItemU * 2;
                }
            }
        }

        for (int i = lineInfo.ItemStartIndex, j = 0; i < lineInfo.ItemEndIndex; i++, j++)
        {
            var child = children[_orderList[i].Index];
            if (child == null) continue;

            var childSize = new UVSize(flexDirection, isHorizontal
                ? new Size(child.DesiredSize.Width * lineInfo.ScaleU, child.DesiredSize.Height)
                : new Size(child.DesiredSize.Width, child.DesiredSize.Height * lineInfo.ScaleU));

            childSize.U += flexGrowUArr[j] + flexShrinkUArr[j];

            if (isReverse)
            {
                u -= childSize.U;
                u -= offsetUArr[j];
            }
            else
            {
                u += offsetUArr[j];
            }

            var v = lineInfo.OffsetV;
            var alignSelf = GetAlignSelf(child);
            var alignment = alignSelf == FlexItemAlignment.Auto ? alignItems : (FlexItemsAlignment) alignSelf;

            switch (alignment)
            {
                case FlexItemsAlignment.Stretch:
                    if (_lineCount == 1 && flexWrap == FlexWrap.NoWrap)
                    {
                        childSize.V = lineInfo.LineV + lineInfo.LineFreeV;
                    }
                    else
                    {
                        childSize.V = lineInfo.LineV;
                    }

                    break;
                case FlexItemsAlignment.FlexEnd:
                    v += lineInfo.LineV - childSize.V;
                    break;
                case FlexItemsAlignment.Center:
                    v += (lineInfo.LineV - childSize.V) * 0.5;
                    break;
            }

            child.Arrange(isHorizontal ? new Rect(u, v, childSize.U, childSize.V) : new Rect(v, u, childSize.V, childSize.U));

            if (!isReverse)
            {
                u += childSize.U;
            }
        }
    }

    private readonly struct FlexItemInfo : IComparable<FlexItemInfo>
    {
        public FlexItemInfo(int index, int order)
        {
            Index = index;
            Order = order;
        }

        private int Order { get; }

        public int Index { get; }

        public int CompareTo(FlexItemInfo other)
        {
            var orderCompare = Order.CompareTo(other.Order);
            if (orderCompare != 0) return orderCompare;
            return Index.CompareTo(other.Index);
        }
    }

    private struct FlexLineInfo
    {
        public double ItemsU { get; set; }

        public double OffsetV { get; set; }

        public double LineU { get; set; }

        public double LineV { get; set; }

        public double LineFreeV { get; set; }

        public int ItemStartIndex { get; set; }

        public int ItemEndIndex { get; set; }

        public double ScaleU { get; set; }
    }

    private struct UVSize
    {
        public UVSize(FlexDirection direction, Size size)
        {
            U = V = 0d;
            FlexDirection = direction;
            Width = size.Width;
            Height = size.Height;
        }

        public UVSize(FlexDirection direction)
        {
            U = V = 0d;
            FlexDirection = direction;
        }

        public double U { get; set; }

        public double V { get; set; }

        private FlexDirection FlexDirection { get; }

        public double Width
        {
            get => FlexDirection == FlexDirection.Row || FlexDirection == FlexDirection.RowReverse ? U : V;
            private set
            {
                if (FlexDirection == FlexDirection.Row || FlexDirection == FlexDirection.RowReverse)
                {
                    U = value;
                }
                else
                {
                    V = value;
                }
            }
        }

        public double Height
        {
            get => FlexDirection == FlexDirection.Row || FlexDirection == FlexDirection.RowReverse ? V : U;
            private set
            {
                if (FlexDirection == FlexDirection.Row || FlexDirection == FlexDirection.RowReverse)
                {
                    V = value;
                }
                else
                {
                    U = value;
                }
            }
        }
    }
}
