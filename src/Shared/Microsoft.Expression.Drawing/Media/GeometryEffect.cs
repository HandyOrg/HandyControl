using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace HandyControl.Expression.Media;

[TypeConverter(typeof(GeometryEffectConverter))]
public abstract class GeometryEffect : Freezable
{
    // ReSharper disable once InconsistentNaming
    private static GeometryEffect defaultGeometryEffect;

    public static readonly DependencyProperty GeometryEffectProperty =
        DependencyProperty.RegisterAttached("GeometryEffect", typeof(GeometryEffect),
            typeof(GeometryEffect),
            new DrawingPropertyMetadata(DefaultGeometryEffect,
                DrawingPropertyMetadataOptions.AffectsRender, OnGeometryEffectChanged));

    protected Geometry CachedGeometry;

    private bool _effectInvalidated;

    static GeometryEffect()
    {
        DrawingPropertyMetadata.DrawingPropertyChanged +=
            delegate (object sender, DrawingPropertyChangedEventArgs args)
            {
                if (sender is GeometryEffect effect && args.Metadata.AffectsRender)
                    effect.InvalidateGeometry(InvalidateGeometryReasons.PropertyChanged);
            };
    }

    public static GeometryEffect DefaultGeometryEffect =>
        defaultGeometryEffect ?? (defaultGeometryEffect = new NoGeometryEffect());

    public Geometry OutputGeometry =>
        CachedGeometry;

    protected internal DependencyObject Parent { get; private set; }

    protected internal virtual void Attach(DependencyObject obj)
    {
        if (Parent != null) Detach();
        _effectInvalidated = true;
        CachedGeometry = null;
        if (InvalidateParent(obj)) Parent = obj;
    }

    public new GeometryEffect CloneCurrentValue()
    {
        return (GeometryEffect) base.CloneCurrentValue();
    }

    protected override Freezable CreateInstanceCore()
    {
        return (Freezable) Activator.CreateInstance(GetType());
    }

    protected abstract GeometryEffect DeepCopy();

    protected internal virtual void Detach()
    {
        _effectInvalidated = true;
        CachedGeometry = null;
        if (Parent != null)
        {
            InvalidateParent(Parent);
            Parent = null;
        }
    }

    public abstract bool Equals(GeometryEffect geometryEffect);

    public static GeometryEffect GetGeometryEffect(DependencyObject obj)
    {
        return (GeometryEffect) obj.GetValue(GeometryEffectProperty);
    }

    public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
    {
        if (_effectInvalidated) return false;
        _effectInvalidated = true;
        if (reasons != InvalidateGeometryReasons.ParentInvalidated) InvalidateParent(Parent);
        return true;
    }

    private static bool InvalidateParent(DependencyObject parent)
    {
        if (parent is IShape shape)
        {
            shape.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
            return true;
        }

        if (parent is GeometryEffect effect)
        {
            effect.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
            return true;
        }

        return false;
    }

    private static void OnGeometryEffectChanged(DependencyObject obj,
        DependencyPropertyChangedEventArgs e)
    {
        var oldValue = e.OldValue as GeometryEffect;
        var newValue = e.NewValue as GeometryEffect;
        if (!Equals(oldValue, newValue))
        {
            if (oldValue != null && obj.Equals(oldValue.Parent)) oldValue.Detach();
            if (newValue != null)
            {
                if (newValue.Parent != null)
                {
                    obj.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var effect = newValue.CloneCurrentValue();
                        obj.SetValue(GeometryEffectProperty, effect);
                    }), DispatcherPriority.Send, null);
                }
                else
                {
                    newValue.Attach(obj);
                }
            }
        }
    }

    public bool ProcessGeometry(Geometry input)
    {
        var flag = false;
        if (_effectInvalidated)
        {
            flag |= UpdateCachedGeometry(input);
            _effectInvalidated = false;
        }

        return flag;
    }

    public static void SetGeometryEffect(DependencyObject obj, GeometryEffect value)
    {
        obj.SetValue(GeometryEffectProperty, value);
    }

    protected abstract bool UpdateCachedGeometry(Geometry input);


    private class NoGeometryEffect : GeometryEffect
    {
        protected override GeometryEffect DeepCopy()
        {
            return new NoGeometryEffect();
        }

        public override bool Equals(GeometryEffect geometryEffect)
        {
            if (geometryEffect != null) return geometryEffect is NoGeometryEffect;
            return true;
        }

        protected override bool UpdateCachedGeometry(Geometry input)
        {
            CachedGeometry = input;
            return false;
        }
    }
}
