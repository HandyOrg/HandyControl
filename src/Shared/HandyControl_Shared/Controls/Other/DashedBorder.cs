using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class DashedBorder : Decorator
{
    private bool _useComplexRenderCodePath;

    private Pen GeometryPenCache { get; set; }

    private Pen LeftPenCache { get; set; }

    private Pen RightPenCache { get; set; }

    private Pen TopPenCache { get; set; }

    private Pen BottomPenCache { get; set; }

    private StreamGeometry BackgroundGeometryCache { get; set; }

    private StreamGeometry BorderGeometryCache { get; set; }

    private static void OnClearPenCache(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var border = (DashedBorder) d;
        border.LeftPenCache = null;
        border.RightPenCache = null;
        border.TopPenCache = null;
        border.BottomPenCache = null;
        border.GeometryPenCache = null;
    }

    public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
        nameof(BorderThickness), typeof(Thickness), typeof(DashedBorder), new FrameworkPropertyMetadata(default(Thickness),
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnClearPenCache));

    public Thickness BorderThickness
    {
        get => (Thickness) GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public static readonly DependencyProperty BorderDashThicknessProperty = DependencyProperty.Register(
        nameof(BorderDashThickness), typeof(double), typeof(DashedBorder), new FrameworkPropertyMetadata(ValueBoxes.Double0Box,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnClearPenCache));

    public double BorderDashThickness
    {
        get => (double) GetValue(BorderDashThicknessProperty);
        set => SetValue(BorderDashThicknessProperty, value);
    }

    public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
        nameof(Padding), typeof(Thickness), typeof(DashedBorder), new FrameworkPropertyMetadata(default(Thickness),
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    public Thickness Padding
    {
        get => (Thickness) GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius), typeof(CornerRadius), typeof(DashedBorder), new FrameworkPropertyMetadata(default(CornerRadius),
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius) GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
        nameof(BorderBrush), typeof(Brush), typeof(DashedBorder), new FrameworkPropertyMetadata(default(Brush),
            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
            OnClearPenCache));

    public Brush BorderBrush
    {
        get => (Brush) GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
        nameof(Background), typeof(Brush), typeof(DashedBorder), new FrameworkPropertyMetadata(default(Brush),
            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
            OnClearPenCache));

    public Brush Background
    {
        get => (Brush) GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static readonly DependencyProperty BorderDashArrayProperty = DependencyProperty.Register(
        nameof(BorderDashArray), typeof(DoubleCollection), typeof(DashedBorder), new FrameworkPropertyMetadata(default(DoubleCollection),
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnClearPenCache));

    public DoubleCollection BorderDashArray
    {
        get => (DoubleCollection) GetValue(BorderDashArrayProperty);
        set => SetValue(BorderDashArrayProperty, value);
    }

    public static readonly DependencyProperty BorderDashCapProperty = DependencyProperty.Register(
        nameof(BorderDashCap), typeof(PenLineCap), typeof(DashedBorder), new FrameworkPropertyMetadata(default(PenLineCap),
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnClearPenCache));

    public PenLineCap BorderDashCap
    {
        get => (PenLineCap) GetValue(BorderDashCapProperty);
        set => SetValue(BorderDashCapProperty, value);
    }

    public static readonly DependencyProperty BorderDashOffsetProperty = DependencyProperty.Register(
        nameof(BorderDashOffset), typeof(double), typeof(DashedBorder), new FrameworkPropertyMetadata(ValueBoxes.Double0Box,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnClearPenCache));

    public double BorderDashOffset
    {
        get => (double) GetValue(BorderDashOffsetProperty);
        set => SetValue(BorderDashOffsetProperty, value);
    }

    private static Size ConvertThickness2Size(Thickness th) => new(th.Left + th.Right, th.Top + th.Bottom);

    private static Rect DeflateRect(Rect rt, Thickness thick) => new(rt.Left + thick.Left,
        rt.Top + thick.Top,
        Math.Max(0.0, rt.Width - thick.Left - thick.Right),
        Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));

    private static bool AreUniformCorners(CornerRadius borderRadii)
    {
        var topLeft = borderRadii.TopLeft;
        return MathHelper.AreClose(topLeft, borderRadii.TopRight) &&
               MathHelper.AreClose(topLeft, borderRadii.BottomLeft) &&
               MathHelper.AreClose(topLeft, borderRadii.BottomRight);
    }

    private static void GenerateGeometry(StreamGeometryContext ctx, Rect rect, in Radii radii)
    {
        var topLeft = new Point(radii.LeftTop, 0);
        var topRight = new Point(rect.Width - radii.RightTop, 0);
        var rightTop = new Point(rect.Width, radii.TopRight);
        var rightBottom = new Point(rect.Width, rect.Height - radii.BottomRight);
        var bottomRight = new Point(rect.Width - radii.RightBottom, rect.Height);
        var bottomLeft = new Point(radii.LeftBottom, rect.Height);
        var leftBottom = new Point(0, rect.Height - radii.BottomLeft);
        var leftTop = new Point(0, radii.TopLeft);

        //  top edge
        if (topLeft.X > topRight.X)
        {
            var v = radii.LeftTop / (radii.LeftTop + radii.RightTop) * rect.Width;
            topLeft.X = v;
            topRight.X = v;
        }

        //  right edge
        if (rightTop.Y > rightBottom.Y)
        {
            var v = radii.TopRight / (radii.TopRight + radii.BottomRight) * rect.Height;
            rightTop.Y = v;
            rightBottom.Y = v;
        }

        //  bottom edge
        if (bottomRight.X < bottomLeft.X)
        {
            var v = radii.LeftBottom / (radii.LeftBottom + radii.RightBottom) * rect.Width;
            bottomRight.X = v;
            bottomLeft.X = v;
        }

        //  left edge
        if (leftBottom.Y < leftTop.Y)
        {
            var v = radii.TopLeft / (radii.TopLeft + radii.BottomLeft) * rect.Height;
            leftBottom.Y = v;
            leftTop.Y = v;
        }

        //  add on offsets
        var offset = new Vector(rect.TopLeft.X, rect.TopLeft.Y);
        topLeft += offset;
        topRight += offset;
        rightTop += offset;
        rightBottom += offset;
        bottomRight += offset;
        bottomLeft += offset;
        leftBottom += offset;
        leftTop += offset;

        //  create the border geometry
        ctx.BeginFigure(topLeft, true, true);

        //  Top line
        ctx.LineTo(topRight, true, false);

        //  Upper-right corner
        var radiusX = rect.TopRight.X - topRight.X;
        var radiusY = rightTop.Y - rect.TopRight.Y;
        if (!MathHelper.IsZero(radiusX) || !MathHelper.IsZero(radiusY))
        {
            ctx.ArcTo(rightTop, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
        }

        //  Right line
        ctx.LineTo(rightBottom, true, false);

        //  Lower-right corner
        radiusX = rect.BottomRight.X - bottomRight.X;
        radiusY = rect.BottomRight.Y - rightBottom.Y;
        if (!MathHelper.IsZero(radiusX) || !MathHelper.IsZero(radiusY))
        {
            ctx.ArcTo(bottomRight, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
        }

        //  Bottom line
        ctx.LineTo(bottomLeft, true, false);

        //  Lower-left corner
        radiusX = bottomLeft.X - rect.BottomLeft.X;
        radiusY = rect.BottomLeft.Y - leftBottom.Y;
        if (!MathHelper.IsZero(radiusX) || !MathHelper.IsZero(radiusY))
        {
            ctx.ArcTo(leftBottom, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
        }

        //  Left line
        ctx.LineTo(leftTop, true, false);

        //  Upper-left corner
        radiusX = topLeft.X - rect.TopLeft.X;
        radiusY = leftTop.Y - rect.TopLeft.Y;
        if (!MathHelper.IsZero(radiusX) || !MathHelper.IsZero(radiusY))
        {
            ctx.ArcTo(topLeft, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
        }
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var child = Child;
        var borderThickness = BorderThickness;
        var padding = Padding;

        if (UseLayoutRounding)
        {
            var dpiScaleX = DpiHelper.DeviceDpiX;
            var dpiScaleY = DpiHelper.DeviceDpiY;

            borderThickness = new Thickness(
                DpiHelper.RoundLayoutValue(borderThickness.Left, dpiScaleX),
                DpiHelper.RoundLayoutValue(borderThickness.Top, dpiScaleY),
                DpiHelper.RoundLayoutValue(borderThickness.Right, dpiScaleX),
                DpiHelper.RoundLayoutValue(borderThickness.Bottom, dpiScaleY));
        }

        var borderSize = ConvertThickness2Size(borderThickness);
        var paddingSize = ConvertThickness2Size(padding);
        var mySize = new Size();

        if (child != null)
        {
            var combined = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);
            var childConstraint = new Size(Math.Max(0.0, constraint.Width - combined.Width), Math.Max(0.0, constraint.Height - combined.Height));

            child.Measure(childConstraint);
            var childSize = child.DesiredSize;

            mySize.Width = childSize.Width + combined.Width;
            mySize.Height = childSize.Height + combined.Height;
        }
        else
        {
            mySize = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);
        }

        return mySize;
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        var borderThickness = BorderThickness;

        if (UseLayoutRounding)
        {
            var dpiScaleX = DpiHelper.DeviceDpiX;
            var dpiScaleY = DpiHelper.DeviceDpiY;

            borderThickness = new Thickness(
                DpiHelper.RoundLayoutValue(borderThickness.Left, dpiScaleX),
                DpiHelper.RoundLayoutValue(borderThickness.Top, dpiScaleY),
                DpiHelper.RoundLayoutValue(borderThickness.Right, dpiScaleX),
                DpiHelper.RoundLayoutValue(borderThickness.Bottom, dpiScaleY));
        }

        var boundRect = new Rect(arrangeSize);
        var innerRect = DeflateRect(boundRect, borderThickness);

        var child = Child;
        if (child != null)
        {
            var padding = Padding;
            var childRect = DeflateRect(innerRect, padding);
            child.Arrange(childRect);
        }

        var radii = CornerRadius;
        var borderBrush = BorderBrush;
        var uniformCorners = AreUniformCorners(radii);

        _useComplexRenderCodePath = !uniformCorners;
        if (!_useComplexRenderCodePath && borderBrush != null)
        {
            _useComplexRenderCodePath = !MathHelper.IsZero(radii.TopLeft) && !borderThickness.IsUniform();
        }

        if (_useComplexRenderCodePath)
        {
            var innerRadii = new Radii(radii, borderThickness, false);
            StreamGeometry backgroundGeometry = null;

            if (!MathHelper.IsZero(innerRect.Width) && !MathHelper.IsZero(innerRect.Height))
            {
                backgroundGeometry = new StreamGeometry();

                using (var ctx = backgroundGeometry.Open())
                {
                    GenerateGeometry(ctx, innerRect, innerRadii);
                }

                backgroundGeometry.Freeze();
                BackgroundGeometryCache = backgroundGeometry;
            }
            else
            {
                BackgroundGeometryCache = null;
            }

            if (!MathHelper.IsZero(boundRect.Width) && !MathHelper.IsZero(boundRect.Height))
            {
                var outerRadii = new Radii(radii, borderThickness, true);
                var borderGeometry = new StreamGeometry();

                using (var ctx = borderGeometry.Open())
                {
                    GenerateGeometry(ctx, boundRect, outerRadii);
                }

                borderGeometry.Freeze();
                BorderGeometryCache = borderGeometry;
            }
            else
            {
                BorderGeometryCache = null;
            }
        }
        else
        {
            BackgroundGeometryCache = null;
            BorderGeometryCache = null;
        }

        return arrangeSize;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        var background = Background;
        var borderBrush = BorderBrush;
        var useLayoutRounding = UseLayoutRounding;

        if (_useComplexRenderCodePath)
        {
            var borderGeometry = BorderGeometryCache;
            if (borderGeometry != null && borderBrush != null)
            {
                var pen = GeometryPenCache;
                if (pen == null)
                {
                    pen = new Pen
                    {
                        Brush = borderBrush,
                        Thickness = BorderDashThickness,
                        DashCap = BorderDashCap,
                        DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                    };

                    if (borderBrush.IsFrozen)
                    {
                        pen.Freeze();
                    }

                    GeometryPenCache = pen;
                }

                drawingContext.DrawGeometry(null, pen, borderGeometry);
            }

            var backgroundGeometry = BackgroundGeometryCache;
            if (backgroundGeometry != null && background != null)
            {
                drawingContext.DrawGeometry(background, null, backgroundGeometry);
            }
        }
        else
        {
            var dpiScaleX = DpiHelper.DeviceDpiX;
            var dpiScaleY = DpiHelper.DeviceDpiY;

            var borderThickness = BorderThickness;
            var cornerRadius = CornerRadius;
            var outerCornerRadius = cornerRadius.TopLeft;
            var roundedCorners = !MathHelper.IsZero(outerCornerRadius);

            if (!borderThickness.IsZero() && borderBrush != null)
            {
                var pen = LeftPenCache;
                if (pen == null)
                {
                    pen = new Pen
                    {
                        Brush = borderBrush,
                        Thickness = useLayoutRounding
                            ? DpiHelper.RoundLayoutValue(borderThickness.Left, dpiScaleX)
                            : borderThickness.Left,
                        DashCap = BorderDashCap,
                        DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                    };

                    if (borderBrush.IsFrozen)
                    {
                        pen.Freeze();
                    }

                    LeftPenCache = pen;
                }

                double halfThickness;
                var renderSize = RenderSize;

                if (borderThickness.IsUniform())
                {
                    halfThickness = pen.Thickness * 0.5;
                    var rect = new Rect(new Point(halfThickness, halfThickness), new Point(renderSize.Width - halfThickness, renderSize.Height - halfThickness));

                    if (roundedCorners)
                    {
                        drawingContext.DrawRoundedRectangle(null, pen, rect, outerCornerRadius, outerCornerRadius);
                    }
                    else
                    {
                        drawingContext.DrawRectangle(null, pen, rect);
                    }
                }
                else
                {
                    if (MathHelper.GreaterThan(borderThickness.Left, 0))
                    {
                        halfThickness = pen.Thickness * 0.5;
                        drawingContext.DrawLine(pen, new Point(halfThickness, 0), new Point(halfThickness, renderSize.Height));
                    }

                    if (MathHelper.GreaterThan(borderThickness.Right, 0))
                    {
                        pen = RightPenCache;
                        if (pen == null)
                        {
                            pen = new Pen
                            {
                                Brush = borderBrush,
                                Thickness = useLayoutRounding
                                    ? DpiHelper.RoundLayoutValue(borderThickness.Right, dpiScaleX)
                                    : borderThickness.Right,
                                DashCap = BorderDashCap,
                                DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                            };

                            if (borderBrush.IsFrozen)
                            {
                                pen.Freeze();
                            }

                            RightPenCache = pen;
                        }

                        halfThickness = pen.Thickness * 0.5;
                        drawingContext.DrawLine(pen, new Point(renderSize.Width - halfThickness, 0), new Point(renderSize.Width - halfThickness, renderSize.Height));
                    }

                    if (MathHelper.GreaterThan(borderThickness.Top, 0))
                    {
                        pen = TopPenCache;
                        if (pen == null)
                        {
                            pen = new Pen
                            {
                                Brush = borderBrush,
                                Thickness = useLayoutRounding
                                    ? DpiHelper.RoundLayoutValue(borderThickness.Top, dpiScaleY)
                                    : borderThickness.Top,
                                DashCap = BorderDashCap,
                                DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                            };

                            if (borderBrush.IsFrozen)
                            {
                                pen.Freeze();
                            }

                            TopPenCache = pen;
                        }

                        halfThickness = pen.Thickness * 0.5;
                        drawingContext.DrawLine(pen, new Point(0, halfThickness), new Point(renderSize.Width, halfThickness));
                    }

                    if (MathHelper.GreaterThan(borderThickness.Bottom, 0))
                    {
                        pen = BottomPenCache;
                        if (pen == null)
                        {
                            pen = new Pen
                            {
                                Brush = borderBrush,
                                Thickness = useLayoutRounding
                                    ? DpiHelper.RoundLayoutValue(borderThickness.Bottom, dpiScaleY)
                                    : borderThickness.Bottom,
                                DashCap = BorderDashCap,
                                DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                            };

                            if (borderBrush.IsFrozen)
                            {
                                pen.Freeze();
                            }

                            BottomPenCache = pen;
                        }

                        halfThickness = pen.Thickness * 0.5;
                        drawingContext.DrawLine(pen, new Point(0, renderSize.Height - halfThickness), new Point(renderSize.Width, renderSize.Height - halfThickness));
                    }
                }
            }

            if (background != null)
            {
                Point ptTL, ptBR;

                if (useLayoutRounding)
                {
                    ptTL = new Point(DpiHelper.RoundLayoutValue(borderThickness.Left, dpiScaleX), DpiHelper.RoundLayoutValue(borderThickness.Top, dpiScaleY));
                    ptBR = new Point(RenderSize.Width - DpiHelper.RoundLayoutValue(borderThickness.Right, dpiScaleX),
                        RenderSize.Height - DpiHelper.RoundLayoutValue(borderThickness.Bottom, dpiScaleY));
                }
                else
                {
                    ptTL = new Point(borderThickness.Left, borderThickness.Top);
                    ptBR = new Point(RenderSize.Width - borderThickness.Right, RenderSize.Height - borderThickness.Bottom);
                }

                if (ptBR.X > ptTL.X && ptBR.Y > ptTL.Y)
                {
                    if (roundedCorners)
                    {
                        var innerRadii = new Radii(cornerRadius, borderThickness, false);
                        var innerCornerRadius = innerRadii.TopLeft;
                        drawingContext.DrawRoundedRectangle(background, null, new Rect(ptTL, ptBR), innerCornerRadius, innerCornerRadius);
                    }
                    else
                    {
                        drawingContext.DrawRectangle(background, null, new Rect(ptTL, ptBR));
                    }
                }
            }
        }
    }

    private readonly struct Radii
    {
        internal Radii(CornerRadius radii, Thickness borders, bool outer)
        {
            var left = 0.5 * borders.Left;
            var top = 0.5 * borders.Top;
            var right = 0.5 * borders.Right;
            var bottom = 0.5 * borders.Bottom;

            if (outer)
            {
                if (MathHelper.IsZero(radii.TopLeft))
                {
                    LeftTop = TopLeft = 0.0;
                }
                else
                {
                    LeftTop = radii.TopLeft + left;
                    TopLeft = radii.TopLeft + top;
                }
                if (MathHelper.IsZero(radii.TopRight))
                {
                    TopRight = RightTop = 0.0;
                }
                else
                {
                    TopRight = radii.TopRight + top;
                    RightTop = radii.TopRight + right;
                }
                if (MathHelper.IsZero(radii.BottomRight))
                {
                    RightBottom = BottomRight = 0.0;
                }
                else
                {
                    RightBottom = radii.BottomRight + right;
                    BottomRight = radii.BottomRight + bottom;
                }
                if (MathHelper.IsZero(radii.BottomLeft))
                {
                    BottomLeft = LeftBottom = 0.0;
                }
                else
                {
                    BottomLeft = radii.BottomLeft + bottom;
                    LeftBottom = radii.BottomLeft + left;
                }
            }
            else
            {
                LeftTop = Math.Max(0.0, radii.TopLeft - left);
                TopLeft = Math.Max(0.0, radii.TopLeft - top);
                TopRight = Math.Max(0.0, radii.TopRight - top);
                RightTop = Math.Max(0.0, radii.TopRight - right);
                RightBottom = Math.Max(0.0, radii.BottomRight - right);
                BottomRight = Math.Max(0.0, radii.BottomRight - bottom);
                BottomLeft = Math.Max(0.0, radii.BottomLeft - bottom);
                LeftBottom = Math.Max(0.0, radii.BottomLeft - left);
            }
        }

        internal readonly double LeftTop;
        internal readonly double TopLeft;
        internal readonly double TopRight;
        internal readonly double RightTop;
        internal readonly double RightBottom;
        internal readonly double BottomRight;
        internal readonly double BottomLeft;
        internal readonly double LeftBottom;
    }
}
