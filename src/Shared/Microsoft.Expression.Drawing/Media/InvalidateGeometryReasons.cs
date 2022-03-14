using System;

namespace HandyControl.Expression.Media;

[Flags]
public enum InvalidateGeometryReasons
{
    ChildInvalidated = 4,
    IsAnimated = 2,
    ParentInvalidated = 8,
    PropertyChanged = 1,
    TemplateChanged = 0x10
}
