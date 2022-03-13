using System.Windows;
using System.Windows.Media;
using HandyControl.Expression.Drawing;

namespace HandyControl.Expression.Media;

public abstract class GeometrySource<TParameters> : IGeometrySource
    where TParameters : IGeometrySourceParameters
{
    private bool _geometryInvalidated;

    protected Geometry CachedGeometry;

    public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
    {
        if ((reasons & InvalidateGeometryReasons.TemplateChanged) != 0) CachedGeometry = null;
        if (!_geometryInvalidated)
        {
            _geometryInvalidated = true;
            return true;
        }

        return false;
    }

    public bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds)
    {
        var force = false;
        if (parameters is TParameters parameters1)
        {
            var rect = ComputeLogicalBounds(layoutBounds, parameters1);
            force |= LayoutBounds != layoutBounds || LogicalBounds != rect;
            if (_geometryInvalidated || force)
            {
                LayoutBounds = layoutBounds;
                LogicalBounds = rect;
                force |= UpdateCachedGeometry(parameters1);
                force |= ApplyGeometryEffect(parameters1, force);
            }
        }

        _geometryInvalidated = false;
        return force;
    }

    public Geometry Geometry { get; private set; }

    public Rect LayoutBounds { get; private set; }

    public Rect LogicalBounds { get; private set; }

    private bool ApplyGeometryEffect(IGeometrySourceParameters parameters, bool force)
    {
        var flag = false;
        var cachedGeometry = CachedGeometry;
        var geometryEffect = parameters.GetGeometryEffect();
        if (geometryEffect != null)
        {
            if (force)
            {
                flag = true;
                geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.ParentInvalidated);
            }

            if (geometryEffect.ProcessGeometry(CachedGeometry))
            {
                flag = true;
                cachedGeometry = geometryEffect.OutputGeometry;
            }
        }

        if (!Equals(Geometry, cachedGeometry))
        {
            flag = true;
            Geometry = cachedGeometry;
        }

        return flag;
    }

    protected virtual Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
    {
        return GeometryHelper.Inflate(layoutBounds, -parameters.GetHalfStrokeThickness());
    }

    protected abstract bool UpdateCachedGeometry(TParameters parameters);
}
