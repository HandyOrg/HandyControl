using System;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

public class DashedBorder : Decorator
{
    public static readonly StyledProperty<double> BorderDashThicknessProperty =
        AvaloniaProperty.Register<DashedBorder, double>(nameof(BorderDashThickness));

    public static readonly StyledProperty<AvaloniaList<double>?> BorderDashArrayProperty =
        AvaloniaProperty.Register<DashedBorder, AvaloniaList<double>?>(nameof(BorderDashArray));

    public static readonly StyledProperty<PenLineCap> BorderDashCapProperty =
        AvaloniaProperty.Register<DashedBorder, PenLineCap>(nameof(BorderDashCap));

    public static readonly StyledProperty<double> BorderDashOffsetProperty =
        AvaloniaProperty.Register<DashedBorder, double>(nameof(BorderDashOffset));

    public static readonly StyledProperty<Thickness> BorderThicknessProperty =
        Border.BorderThicknessProperty.AddOwner<DashedBorder>();

    public static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
        Border.CornerRadiusProperty.AddOwner<DashedBorder>();

    public static readonly StyledProperty<IBrush?> BorderBrushProperty =
        Border.BorderBrushProperty.AddOwner<DashedBorder>();

    public static readonly StyledProperty<IBrush?> BackgroundProperty =
        Border.BackgroundProperty.AddOwner<DashedBorder>();

    private Thickness? _layoutThickness;
    private double _scale;
    private bool _useComplexRendering;
    private StreamGeometry? _backgroundGeometryCache;
    private StreamGeometry? _borderGeometryCache;
    private Pen? _leftPenCache;
    private Pen? _rightPenCache;
    private Pen? _topPenCache;
    private Pen? _bottomPenCache;
    private Pen? _geometryPenCache;

    static DashedBorder()
    {
        AffectsMeasure<DashedBorder>(
            BorderDashThicknessProperty,
            BorderThicknessProperty
        );

        AffectsRender<DashedBorder>(
            BorderDashArrayProperty,
            BorderDashCapProperty,
            BorderDashOffsetProperty,
            CornerRadiusProperty,
            BorderBrushProperty,
            BackgroundProperty
        );

        BorderThicknessProperty.Changed.AddClassHandler<DashedBorder>(OnClearPenCache);
        BorderDashThicknessProperty.Changed.AddClassHandler<DashedBorder>(OnClearPenCache);
        BorderBrushProperty.Changed.AddClassHandler<DashedBorder>(OnClearPenCache);
        BackgroundProperty.Changed.AddClassHandler<DashedBorder>(OnClearPenCache);
        BorderDashArrayProperty.Changed.AddClassHandler<DashedBorder>(OnClearPenCache);
        BorderDashCapProperty.Changed.AddClassHandler<DashedBorder>(OnClearPenCache);
        BorderDashOffsetProperty.Changed.AddClassHandler<DashedBorder>(OnClearPenCache);
    }

    private static void OnClearPenCache(AvaloniaObject element, AvaloniaPropertyChangedEventArgs e)
    {
        var border = (DashedBorder)element;
        border._leftPenCache = null;
        border._rightPenCache = null;
        border._topPenCache = null;
        border._bottomPenCache = null;
        border._geometryPenCache = null;
        border._backgroundGeometryCache = null;
        border._borderGeometryCache = null;
    }

    public double BorderDashThickness
    {
        get => GetValue(BorderDashThicknessProperty);
        set => SetValue(BorderDashThicknessProperty, value);
    }

    public AvaloniaList<double>? BorderDashArray
    {
        get => GetValue(BorderDashArrayProperty);
        set => SetValue(BorderDashArrayProperty, value);
    }

    public PenLineCap BorderDashCap
    {
        get => GetValue(BorderDashCapProperty);
        set => SetValue(BorderDashCapProperty, value);
    }

    public double BorderDashOffset
    {
        get => GetValue(BorderDashOffsetProperty);
        set => SetValue(BorderDashOffsetProperty, value);
    }

    public Thickness BorderThickness
    {
        get => GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public IBrush? BorderBrush
    {
        get => GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    private Thickness LayoutThickness
    {
        get
        {
            VerifyScale();

            if (_layoutThickness.HasValue)
            {
                return _layoutThickness.Value;
            }

            Thickness thickness = BorderThickness;
            if (UseLayoutRounding)
            {
                thickness = LayoutHelper.RoundLayoutThickness(thickness, _scale, _scale);
            }

            _layoutThickness = thickness;

            return _layoutThickness.Value;
        }
    }

    private void VerifyScale()
    {
        double layoutScale = LayoutHelper.GetLayoutScale(this);
        if (MathHelper.AreClose(layoutScale, this._scale))
        {
            return;
        }

        _scale = layoutScale;
        _layoutThickness = new Thickness?();
    }

    public override void Render(DrawingContext context)
    {
        var radii = CornerRadius;
        var borderBrush = BorderBrush;
        var background = Background;
        var borderThickness = BorderThickness;
        bool useLayoutRounding = UseLayoutRounding;
        bool uniformCorners = AreUniformCorners(radii);

        _useComplexRendering = !uniformCorners;
        if (!_useComplexRendering && borderBrush != null)
        {
            _useComplexRendering = !MathHelper.IsZero(radii.TopLeft) && !borderThickness.IsUniform;
        }

        if (_useComplexRendering)
        {
            var boundRect = new Rect(Bounds.Size);
            var innerRect = boundRect.Deflate(borderThickness);
            var innerRadii = new Radii(radii, borderThickness, false);

            if (_backgroundGeometryCache == null &&
                !MathHelper.IsZero(innerRect.Width) &&
                !MathHelper.IsZero(innerRect.Height))
            {
                var backgroundGeometry = new StreamGeometry();

                using (var ctx = backgroundGeometry.Open())
                {
                    GenerateGeometry(ctx, innerRect, innerRadii);
                }

                _backgroundGeometryCache = backgroundGeometry;
            }
            else
            {
                _backgroundGeometryCache = null;
            }

            if (_borderGeometryCache == null &&
                !MathHelper.IsZero(boundRect.Width) &&
                !MathHelper.IsZero(boundRect.Height))
            {
                var outerRadii = new Radii(radii, borderThickness, true);
                var borderGeometry = new StreamGeometry();

                using (var ctx = borderGeometry.Open())
                {
                    GenerateGeometry(ctx, boundRect, outerRadii);
                }

                _borderGeometryCache = borderGeometry;
            }
            else
            {
                _borderGeometryCache = null;
            }
        }
        else
        {
            _backgroundGeometryCache = null;
            _borderGeometryCache = null;
        }

        if (_useComplexRendering)
        {
            if (_borderGeometryCache != null && borderBrush != null)
            {
                _geometryPenCache ??= new Pen
                {
                    Brush = borderBrush,
                    Thickness = BorderDashThickness,
                    LineCap = BorderDashCap,
                    DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                };

                context.DrawGeometry(null, _geometryPenCache, _borderGeometryCache);
            }

            if (_backgroundGeometryCache != null && background != null)
            {
                context.DrawGeometry(background, null, _backgroundGeometryCache);
            }
        }
        else
        {
            var cornerRadius = CornerRadius;
            double outerCornerRadius = cornerRadius.TopLeft;
            bool roundedCorners = !MathHelper.IsZero(outerCornerRadius);

            if (!borderThickness.IsZero() && borderBrush != null)
            {
                _leftPenCache ??= new Pen
                {
                    Brush = borderBrush,
                    Thickness = LayoutThickness.Left,
                    LineCap = BorderDashCap,
                    DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                };

                double halfThickness;
                var renderSize = Bounds.Size;

                if (borderThickness.IsUniform())
                {
                    halfThickness = _leftPenCache.Thickness * 0.5;
                    var rect = new Rect(
                        new Point(halfThickness, halfThickness),
                        new Point(renderSize.Width - halfThickness, renderSize.Height - halfThickness)
                    );

                    if (roundedCorners)
                    {
                        context.DrawRectangle(null, _leftPenCache, rect, outerCornerRadius, outerCornerRadius);
                    }
                    else
                    {
                        context.DrawRectangle(null, _leftPenCache, rect);
                    }
                }
                else
                {
                    if (MathHelper.GreaterThan(borderThickness.Left, 0))
                    {
                        halfThickness = _leftPenCache.Thickness * 0.5;
                        context.DrawLine(
                            _leftPenCache,
                            new Point(halfThickness, 0),
                            new Point(halfThickness, renderSize.Height)
                        );
                    }

                    if (MathHelper.GreaterThan(borderThickness.Right, 0))
                    {
                        _rightPenCache ??= new Pen
                        {
                            Brush = borderBrush,
                            Thickness = LayoutThickness.Right,
                            LineCap = BorderDashCap,
                            DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                        };

                        halfThickness = _rightPenCache.Thickness * 0.5;
                        context.DrawLine(
                            _rightPenCache,
                            new Point(renderSize.Width - halfThickness, 0),
                            new Point(renderSize.Width - halfThickness, renderSize.Height)
                        );
                    }

                    if (MathHelper.GreaterThan(borderThickness.Top, 0))
                    {
                        _topPenCache ??= new Pen
                        {
                            Brush = borderBrush,
                            Thickness = LayoutThickness.Top,
                            LineCap = BorderDashCap,
                            DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                        };

                        halfThickness = _topPenCache.Thickness * 0.5;
                        context.DrawLine(
                            _topPenCache,
                            new Point(0, halfThickness),
                            new Point(renderSize.Width, halfThickness)
                        );
                    }

                    if (MathHelper.GreaterThan(borderThickness.Bottom, 0))
                    {
                        _bottomPenCache ??= new Pen
                        {
                            Brush = borderBrush,
                            Thickness = LayoutThickness.Bottom,
                            LineCap = BorderDashCap,
                            DashStyle = new DashStyle(BorderDashArray, BorderDashOffset)
                        };

                        halfThickness = _bottomPenCache.Thickness * 0.5;
                        context.DrawLine(
                            _bottomPenCache,
                            new Point(0, renderSize.Height - halfThickness),
                            new Point(renderSize.Width, renderSize.Height - halfThickness)
                        );
                    }
                }
            }

            if (background != null)
            {
                Point pointLeftTop, pointRightBottom;

                if (useLayoutRounding)
                {
                    pointLeftTop = new Point(LayoutThickness.Left, LayoutThickness.Top);
                    pointRightBottom = new Point(
                        Bounds.Size.Width - LayoutThickness.Right,
                        Bounds.Size.Height - LayoutThickness.Bottom
                    );
                }
                else
                {
                    pointLeftTop = new Point(borderThickness.Left, borderThickness.Top);
                    pointRightBottom = new Point(
                        Bounds.Size.Width - borderThickness.Right,
                        Bounds.Size.Height - borderThickness.Bottom
                    );
                }

                if (pointRightBottom.X > pointLeftTop.X && pointRightBottom.Y > pointLeftTop.Y)
                {
                    if (roundedCorners)
                    {
                        var innerRadii = new Radii(cornerRadius, borderThickness, false);
                        double innerCornerRadius = innerRadii._topLeft;
                        context.DrawRectangle(
                            brush: background,
                            pen: null,
                            rect: new Rect(pointLeftTop, pointRightBottom),
                            radiusX: innerCornerRadius,
                            radiusY: innerCornerRadius
                        );
                    }
                    else
                    {
                        context.DrawRectangle(
                            brush: background,
                            pen: null,
                            rect: new Rect(pointLeftTop, pointRightBottom)
                        );
                    }
                }
            }
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return LayoutHelper.MeasureChild(Child, availableSize, Padding, BorderThickness);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        return LayoutHelper.ArrangeChild(Child, finalSize, Padding, BorderThickness);
    }

    private static bool AreUniformCorners(CornerRadius borderRadii)
    {
        double topLeft = borderRadii.TopLeft;
        return MathHelper.AreClose(topLeft, borderRadii.TopRight) &&
               MathHelper.AreClose(topLeft, borderRadii.BottomLeft) &&
               MathHelper.AreClose(topLeft, borderRadii.BottomRight);
    }

    private static void GenerateGeometry(StreamGeometryContext ctx, Rect rect, Radii radii)
    {
        var topLeft = new Point(radii._leftTop, 0);
        var topRight = new Point(rect.Width - radii._rightTop, 0);
        var rightTop = new Point(rect.Width, radii._topRight);
        var rightBottom = new Point(rect.Width, rect.Height - radii._bottomRight);
        var bottomRight = new Point(rect.Width - radii._rightBottom, rect.Height);
        var bottomLeft = new Point(radii._leftBottom, rect.Height);
        var leftBottom = new Point(0, rect.Height - radii._bottomLeft);
        var leftTop = new Point(0, radii._topLeft);

        //  top edge
        if (topLeft.X > topRight.X)
        {
            double v = radii._leftTop / (radii._leftTop + radii._rightTop) * rect.Width;
            topLeft = topLeft.WithX(v);
            topRight = topRight.WithX(v);
        }

        //  right edge
        if (rightTop.Y > rightBottom.Y)
        {
            double v = radii._topRight / (radii._topRight + radii._bottomRight) * rect.Height;
            rightTop = rightTop.WithY(v);
            rightBottom = rightBottom.WithY(v);
        }

        //  bottom edge
        if (bottomRight.X < bottomLeft.X)
        {
            double v = radii._leftBottom / (radii._leftBottom + radii._rightBottom) * rect.Width;
            bottomRight = bottomRight.WithX(v);
            bottomLeft = bottomLeft.WithX(v);
        }

        //  left edge
        if (leftBottom.Y < leftTop.Y)
        {
            double v = radii._topLeft / (radii._topLeft + radii._bottomLeft) * rect.Height;
            leftBottom = leftBottom.WithY(v);
            leftTop = leftTop.WithY(v);
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
        ctx.BeginFigure(topLeft, true);

        //  Top line
        ctx.LineTo(topRight, true);

        //  Upper-right corner
        double radiusX = rect.TopRight.X - topRight.X;
        double radiusY = rightTop.Y - rect.TopRight.Y;
        if (!MathHelper.IsZero(radiusX) || !MathHelper.IsZero(radiusY))
        {
            ctx.ArcTo(rightTop, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true);
        }

        //  Right line
        ctx.LineTo(rightBottom, true);

        //  Lower-right corner
        radiusX = rect.BottomRight.X - bottomRight.X;
        radiusY = rect.BottomRight.Y - rightBottom.Y;
        if (!MathHelper.IsZero(radiusX) || !MathHelper.IsZero(radiusY))
        {
            ctx.ArcTo(bottomRight, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true);
        }

        //  Bottom line
        ctx.LineTo(bottomLeft, true);

        //  Lower-left corner
        radiusX = bottomLeft.X - rect.BottomLeft.X;
        radiusY = rect.BottomLeft.Y - leftBottom.Y;
        if (!MathHelper.IsZero(radiusX) || !MathHelper.IsZero(radiusY))
        {
            ctx.ArcTo(leftBottom, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true);
        }

        //  Left line
        ctx.LineTo(leftTop, true);

        //  Upper-left corner
        radiusX = topLeft.X - rect.TopLeft.X;
        radiusY = leftTop.Y - rect.TopLeft.Y;
        if (!MathHelper.IsZero(radiusX) || !MathHelper.IsZero(radiusY))
        {
            ctx.ArcTo(topLeft, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true);
        }

        ctx.EndFigure(true);
    }

    private readonly struct Radii
    {
        internal Radii(CornerRadius radii, Thickness borders, bool outer)
        {
            double left = 0.5 * borders.Left;
            double top = 0.5 * borders.Top;
            double right = 0.5 * borders.Right;
            double bottom = 0.5 * borders.Bottom;

            if (outer)
            {
                if (MathHelper.IsZero(radii.TopLeft))
                {
                    _leftTop = _topLeft = 0.0;
                }
                else
                {
                    _leftTop = radii.TopLeft + left;
                    _topLeft = radii.TopLeft + top;
                }

                if (MathHelper.IsZero(radii.TopRight))
                {
                    _topRight = _rightTop = 0.0;
                }
                else
                {
                    _topRight = radii.TopRight + top;
                    _rightTop = radii.TopRight + right;
                }

                if (MathHelper.IsZero(radii.BottomRight))
                {
                    _rightBottom = _bottomRight = 0.0;
                }
                else
                {
                    _rightBottom = radii.BottomRight + right;
                    _bottomRight = radii.BottomRight + bottom;
                }

                if (MathHelper.IsZero(radii.BottomLeft))
                {
                    _bottomLeft = _leftBottom = 0.0;
                }
                else
                {
                    _bottomLeft = radii.BottomLeft + bottom;
                    _leftBottom = radii.BottomLeft + left;
                }
            }
            else
            {
                _leftTop = Math.Max(0.0, radii.TopLeft - left);
                _topLeft = Math.Max(0.0, radii.TopLeft - top);
                _topRight = Math.Max(0.0, radii.TopRight - top);
                _rightTop = Math.Max(0.0, radii.TopRight - right);
                _rightBottom = Math.Max(0.0, radii.BottomRight - right);
                _bottomRight = Math.Max(0.0, radii.BottomRight - bottom);
                _bottomLeft = Math.Max(0.0, radii.BottomLeft - bottom);
                _leftBottom = Math.Max(0.0, radii.BottomLeft - left);
            }
        }

        internal readonly double _leftTop;
        internal readonly double _topLeft;
        internal readonly double _topRight;
        internal readonly double _rightTop;
        internal readonly double _rightBottom;
        internal readonly double _bottomRight;
        internal readonly double _bottomLeft;
        internal readonly double _leftBottom;
    }
}
