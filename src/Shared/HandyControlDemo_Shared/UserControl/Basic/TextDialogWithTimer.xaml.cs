using System;
using System.Windows.Controls.Primitives;
using HandyControl.Tools;

namespace HandyControlDemo.UserControl;

public partial class TextDialogWithTimer
{
    public TextDialogWithTimer()
    {
        InitializeComponent();

        var animation = AnimationHelper.CreateAnimation(100, 5000);
        animation.EasingFunction = null;
        animation.Completed += Animation_Completed;
        ProgressBarTimer.BeginAnimation(RangeBase.ValueProperty, animation);
    }

    private void Animation_Completed(object sender, EventArgs e)
    {
        ButtonClose.Command.Execute(null);
    }
}
