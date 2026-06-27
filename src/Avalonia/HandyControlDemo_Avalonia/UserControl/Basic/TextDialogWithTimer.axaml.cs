using System;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;
using HandyControl.Controls;
using HandyControlDemo.ViewModel;

namespace HandyControlDemo.UserControl;

public partial class TextDialogWithTimer : Border
{
    public TextDialogWithTimer()
    {
        InitializeComponent();

        DataContext = new InteractiveDialogViewModel();

        var startTime = DateTime.Now;
        var timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle)
        {
            Interval = TimeSpan.FromMilliseconds(50)
        };

        timer.Tick += (_, _) =>
        {
            var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
            var progress = Math.Min(elapsed / 5000.0 * 100, 100);
            ProgressBarTimer.Value = progress;

            if (progress >= 100)
            {
                timer.Stop();
                this.FindAncestorOfType<Dialog>()?.Close();
            }
        };
        timer.Start();

        CloseButton.Click += (_, _) =>
        {
            timer.Stop();
            this.FindAncestorOfType<Dialog>()?.Close();
        };
    }
}
