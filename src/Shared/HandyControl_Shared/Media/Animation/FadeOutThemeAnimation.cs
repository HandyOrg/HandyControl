using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace HandyControl.Media.Animation
{
    /// <summary>
    /// Represents the preconfigured opacity animation that applies to controls when
    /// they are removed from the UI or hidden.
    /// </summary>
    public sealed class FadeOutThemeAnimation : DoubleAnimation
    {
        private static readonly Duration DefaultDuration = TimeSpan.FromMilliseconds(167);

        static FadeOutThemeAnimation()
        {
            ToProperty.OverrideMetadata(typeof(FadeOutThemeAnimation), new FrameworkPropertyMetadata(0.0));
            DurationProperty.OverrideMetadata(typeof(FadeOutThemeAnimation), new FrameworkPropertyMetadata(DefaultDuration));
            Storyboard.TargetPropertyProperty.OverrideMetadata(typeof(FadeOutThemeAnimation), new FrameworkPropertyMetadata(new PropertyPath(UIElement.OpacityProperty)));
        }

        /// <summary>
        /// Initializes a new instance of the FadeOutThemeAnimation class.
        /// </summary>
        public FadeOutThemeAnimation()
        {
        }

        /// <summary>
        /// Identifies the TargetName dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetNameProperty = Storyboard.TargetNameProperty.AddOwner(typeof(FadeOutThemeAnimation));

        /// <summary>
        /// Gets or sets the reference name of the control element being targeted.
        /// </summary>
        /// <returns>
        /// The reference name. This is typically the **x:Name** of the relevant element
        /// as declared in XAML.
        /// </returns>
        public string TargetName
        {
            get => (string)GetValue(TargetNameProperty);
            set => SetValue(TargetNameProperty, value);
        }

        protected override Freezable CreateInstanceCore()
        {
            return new FadeOutThemeAnimation();
        }
    }
}
