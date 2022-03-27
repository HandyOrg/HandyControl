using System;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class RunningBorder : Border
{
    private bool _test;

    protected override Size MeasureOverride(Size constraint)
    {
        if (_test)
        {
            _test = false;
            return constraint;
        }

        var child = Child;
        var borderThickness = BorderThickness;
        var padding = Padding;

        if (UseLayoutRounding)
        {
            var dpiScaleX = DpiHelper.DeviceDpiX;
            var dpiScaleY = DpiHelper.DeviceDpiY;

            borderThickness = new Thickness(
                DpiHelper.RoundLayoutValue(borderThickness.Left, dpiScaleX),
                DpiHelper.RoundLayoutValue(borderThickness.Top, dpiScaleY),
                DpiHelper.RoundLayoutValue(borderThickness.Right, dpiScaleX),
                DpiHelper.RoundLayoutValue(borderThickness.Bottom, dpiScaleY));
        }

        var borderSize = ConvertThickness2Size(borderThickness);
        var paddingSize = ConvertThickness2Size(padding);
        var mySize = new Size();

        if (child != null)
        {
            var combined = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);
            var borderConstraint = new Size(Math.Max(0.0, constraint.Width - combined.Width), Math.Max(0.0, constraint.Height - combined.Height));
            var childConstraint = new Size(Math.Max(0.0, double.PositiveInfinity - combined.Width), Math.Max(0.0, double.PositiveInfinity - combined.Height));


            child.Measure(borderConstraint);
            var childSize = child.DesiredSize;

            mySize.Width = childSize.Width + combined.Width;
            mySize.Height = childSize.Height + combined.Height;

            child.Measure(childConstraint);
        }
        else
        {
            mySize = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);
        }

        return mySize;
    }

    private static Size ConvertThickness2Size(Thickness th) => new(th.Left + th.Right, th.Top + th.Bottom);
}
