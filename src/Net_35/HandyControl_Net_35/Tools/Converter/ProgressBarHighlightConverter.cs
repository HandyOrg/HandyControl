using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HandyControl.Tools.Converter
{
    /// <summary>
    ///     The ProgressBarHighlightConverter class
    /// </summary>
    public class ProgressBarHighlightConverter : IMultiValueConverter
    {
        /// <summary>
        ///     Creates the brush for the ProgressBar
        /// </summary>
        /// <param name="values">ForegroundBrush, IsIndeterminate, Width, Height</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Brush for the ProgressBar</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //
            // Parameter Validation
            //
            var doubleType = typeof(double);
            if (values == null ||
                values.Length != 3 ||
                values[0] == null ||
                values[1] == null ||
                values[2] == null ||
                !(values[0] is Brush) ||
                !doubleType.IsInstanceOfType(values[1]) ||
                !doubleType.IsInstanceOfType(values[2]))
                return null;

            //
            // Conversion
            //

            var brush = (Brush) values[0];
            var width = (double) values[1];
            var height = (double) values[2];

            // if an invalid height, return a null brush
            if (width <= 0.0 || double.IsInfinity(width) || double.IsNaN(width) ||
                height <= 0.0 || double.IsInfinity(height) || double.IsNaN(height))
                return null;

            var newBrush = new DrawingBrush();

            // Create a Drawing Brush that is 2x longer than progress bar track
            //
            // +-------------+..............
            // | highlight   | empty       :  
            // +-------------+.............:
            //
            //  This brush will animate to the right.

            var twiceWidth = width * 2.0;

            // Set the viewport and viewbox to the 2*size of the progress region
            newBrush.Viewport = newBrush.Viewbox = new Rect(-width, 0, twiceWidth, height);
            newBrush.ViewportUnits = newBrush.ViewboxUnits = BrushMappingMode.Absolute;

            newBrush.TileMode = TileMode.None;
            newBrush.Stretch = Stretch.None;

            var myDrawing = new DrawingGroup();
            var myDrawingContext = myDrawing.Open();

            // Draw the highlight
            myDrawingContext.DrawRectangle(brush, null, new Rect(-width, 0, width, height));


            // Animate the Translation

            var translateTime = TimeSpan.FromSeconds(twiceWidth / 200.0); // travel at 200px /second
            var pauseTime = TimeSpan.FromSeconds(1.0); // pause 1 second between animations

            var animation = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.Zero,
                Duration = new Duration(translateTime + pauseTime),
                RepeatBehavior = RepeatBehavior.Forever
            };
            animation.KeyFrames.Add(new LinearDoubleKeyFrame(twiceWidth, translateTime));

            var translation = new TranslateTransform();

            // Set the animation to the XProperty
            translation.BeginAnimation(TranslateTransform.XProperty, animation);

            // Set the animated translation on the brush
            newBrush.Transform = translation;


            myDrawingContext.Close();
            newBrush.Drawing = myDrawing;

            return newBrush;
        }

        /// <summary>
        ///     Not Supported
        /// </summary>
        /// <param name="value">value, as produced by target</param>
        /// <param name="targetTypes">target types</param>
        /// <param name="parameter">converter parameter</param>
        /// <param name="culture">culture information</param>
        /// <returns>Nothing</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}