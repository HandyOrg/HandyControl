using System.Windows.Media;

namespace HandyControl.Expression.Drawing;

internal static class PathGeometryHelper
{
    public static bool IsStroked(this PathSegment pathSegment) => pathSegment.IsStroked;

    public static PathGeometry AsPathGeometry(this Geometry original)
    {
        if (original == null)
        {
            return null;
        }

        if (!(original is PathGeometry geometry))
        {
            return PathGeometry.CreateFromGeometry(original);
        }
        return geometry;
    }

    internal static Geometry FixPathGeometryBoundary(Geometry geometry) => geometry;
}
