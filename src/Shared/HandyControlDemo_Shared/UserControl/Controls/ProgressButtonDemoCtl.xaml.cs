using System;
using System.Windows;
using System.Windows.Threading;

namespace HandyControlDemo.UserControl;

public partial class ProgressButtonDemoCtl
{
    public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
        "Progress", typeof(int), typeof(ProgressButtonDemoCtl), new PropertyMetadata(default(int)));

    public int Progress
    {
        get => (int) GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public static readonly DependencyProperty IsUploadingProperty = DependencyProperty.Register(
        "IsUploading", typeof(bool), typeof(ProgressButtonDemoCtl), new PropertyMetadata(default(bool)));

    public bool IsUploading
    {
        get => (bool) GetValue(IsUploadingProperty);
        set => SetValue(IsUploadingProperty, value);
    }

    private readonly DispatcherTimer _timer;

    public ProgressButtonDemoCtl()
    {
        InitializeComponent();

        DataContext = this;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(200)
        };
        _timer.Tick += Timer_Tick;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        Progress++;
        if (Progress == 100)
        {
            Progress = 0;
            _timer.Stop();
            IsUploading = false;
        }
    }

    private void ButtonProgress_OnClick(object sender, RoutedEventArgs e)
    {
        if (_timer.IsEnabled)
        {
            _timer.Stop();
        }
        else
        {
            _timer.Start();
        }
    }
}
