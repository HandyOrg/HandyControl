using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace HandyControl.Expression.Media;

public sealed class GeometryEffectConverter : TypeConverter
{
    private static readonly Dictionary<string, GeometryEffect> RegisteredEffects;

    static GeometryEffectConverter()
    {
        var dictionary = new Dictionary<string, GeometryEffect>
        {
            {
                "None",
                GeometryEffect.DefaultGeometryEffect
            },
            {
                "Sketch",
                new SketchGeometryEffect()
            }
        };
        RegisteredEffects = dictionary;
    }

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return typeof(string).IsAssignableFrom(sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return typeof(string).IsAssignableFrom(destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string key && RegisteredEffects.TryGetValue(key, out var effect))
            return effect.CloneCurrentValue();
        return null;
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
        Type destinationType)
    {
        if (typeof(string).IsAssignableFrom(destinationType))
        {
            if (value is string) return value;
            foreach (var pair in RegisteredEffects)
                if (pair.Value?.Equals(value as GeometryEffect) ?? value == null)
                    return pair.Key;
        }

        return null;
    }
}
