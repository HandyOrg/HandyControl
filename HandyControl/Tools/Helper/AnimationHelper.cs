using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace HandyControl.Tools
{
    /// <summary>
    ///     包含一些常用的动画辅助方法
    /// </summary>
    public class AnimationHelper
    {
        /// <summary>
        ///     创建一个Thickness动画
        /// </summary>
        /// <param name="thickness"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static ThicknessAnimation CreateAnimation(Thickness thickness = default(Thickness), double milliseconds = 200)
        {
            return new ThicknessAnimation(thickness, new Duration(TimeSpan.FromMilliseconds(milliseconds)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };
        }

        /// <summary>
        ///     创建一个Double动画
        /// </summary>
        /// <param name="toValue"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DoubleAnimation CreateAnimation(double toValue, double milliseconds = 200)
        {
            return new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(milliseconds)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };
        }
    }
}