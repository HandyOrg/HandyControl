using System;
using System.Globalization;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HandyControl.Media.Animation;

public abstract class GeometryAnimationBase : AnimationTimeline
{
    protected GeometryAnimationBase()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }

    public new GeometryAnimationBase Clone() => (GeometryAnimationBase) base.Clone();

    public sealed override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
    {
        if (defaultOriginValue == null)
        {
            throw new ArgumentNullException(nameof(defaultOriginValue));
        }
        if (defaultDestinationValue == null)
        {
            throw new ArgumentNullException(nameof(defaultDestinationValue));
        }
        return GetCurrentValue((Geometry) defaultOriginValue, (Geometry) defaultDestinationValue, animationClock);
    }

    public override Type TargetPropertyType
    {
        get
        {
            ReadPreamble();

            return typeof(Geometry);
        }
    }

    public Geometry GetCurrentValue(Geometry defaultOriginValue, Geometry defaultDestinationValue, AnimationClock animationClock)
    {
        ReadPreamble();

        if (animationClock == null)
        {
            throw new ArgumentNullException(nameof(animationClock));
        }

        if (animationClock.CurrentState == ClockState.Stopped)
        {
            return defaultDestinationValue;
        }

        return GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
    }

    protected abstract Geometry GetCurrentValueCore(Geometry defaultOriginValue, Geometry defaultDestinationValue, AnimationClock animationClock);
}
