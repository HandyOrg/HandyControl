using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Expression.Media;

namespace HandyControl.Expression.Shapes;

public abstract class PrimitiveShape : Shape, IGeometrySourceParameters, IShape
{
    private IGeometrySource _geometrySource;

    static PrimitiveShape()
    {
        StretchProperty.OverrideMetadata(typeof(PrimitiveShape),
            new DrawingPropertyMetadata(Stretch.Fill, DrawingPropertyMetadataOptions.AffectsRender));
        StrokeThicknessProperty.OverrideMetadata(typeof(PrimitiveShape),
            new DrawingPropertyMetadata(ValueBoxes.Double1Box, DrawingPropertyMetadataOptions.AffectsRender));
    }

    protected sealed override Geometry DefiningGeometry =>
        GeometrySource.Geometry ?? Geometry.Empty;

    private IGeometrySource GeometrySource => _geometrySource ?? (_geometrySource = CreateGeometrySource());

    Stretch IGeometrySourceParameters.Stretch => Stretch;

    Brush IGeometrySourceParameters.Stroke => Stroke;

    double IGeometrySourceParameters.StrokeThickness => StrokeThickness;

    public event EventHandler RenderedGeometryChanged;

    public void InvalidateGeometry(InvalidateGeometryReasons reasons)
    {
        if (GeometrySource.InvalidateGeometry(reasons)) InvalidateArrange();
    }

    public Thickness GeometryMargin => GeometrySource.LogicalBounds.Subtract(RenderedGeometry.Bounds);

    Brush IShape.Fill
    {
        get => Fill;
        set => Fill = value;
    }

    Stretch IShape.Stretch
    {
        get => Stretch;
        set => Stretch = value;
    }

    Brush IShape.Stroke
    {
        get => Stroke;
        set => Stroke = value;
    }

    double IShape.StrokeThickness
    {
        get => StrokeThickness;
        set => StrokeThickness = value;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (GeometrySource.UpdateGeometry(this, finalSize.Bounds())) RealizeGeometry();
        base.ArrangeOverride(finalSize);
        return finalSize;
    }

    protected abstract IGeometrySource CreateGeometrySource();

    protected override Size MeasureOverride(Size availableSize) => new(base.StrokeThickness, base.StrokeThickness);

    private void RealizeGeometry() => RenderedGeometryChanged?.Invoke(this, EventArgs.Empty);
}
