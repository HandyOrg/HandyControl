using System.Collections;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HandyControl.Media.Animation
{
    [ContentProperty("KeyFrames")]
    public class GeometryAnimationUsingKeyFrames : GeometryAnimationBase, IKeyFrameAnimation, IAddChild
    {
        protected override Freezable CreateInstanceCore()
        {
            throw new System.NotImplementedException();
        }

        protected override Geometry GetCurrentValueCore(Geometry defaultOriginValue, Geometry defaultDestinationValue,
            AnimationClock animationClock)
        {
            throw new System.NotImplementedException();
        }

        public IList KeyFrames { get; set; }
        public void AddChild(object value)
        {
            throw new System.NotImplementedException();
        }

        public void AddText(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}