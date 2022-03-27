namespace HandyControl.Expression.Media;

public interface IArcGeometrySourceParameters : IGeometrySourceParameters
{
    double ArcThickness { get; }

    UnitType ArcThicknessUnit { get; }

    double EndAngle { get; }

    double StartAngle { get; }
}
