using System;
using Avalonia;
using Avalonia.Threading;
using HandyControlDemo.Properties.Langs;

namespace HandyControlDemo.UserControl;

public partial class ProgressButtonDemo : Avalonia.Controls.UserControl
{
    private readonly DispatcherTimer _timer;

    private int _progress;
    private bool _isUploading;

    public static readonly DirectProperty<ProgressButtonDemo, int> ProgressProperty =
        AvaloniaProperty.RegisterDirect<ProgressButtonDemo, int>(nameof(Progress), o => o.Progress, (o, v) => o.Progress = v);

    public static readonly DirectProperty<ProgressButtonDemo, bool> IsUploadingProperty =
        AvaloniaProperty.RegisterDirect<ProgressButtonDemo, bool>(nameof(IsUploading), o => o.IsUploading, (o, v) => o.IsUploading = v);

    public static readonly DirectProperty<ProgressButtonDemo, string> UploadContentProperty =
        AvaloniaProperty.RegisterDirect<ProgressButtonDemo, string>(nameof(UploadContent), o => o.UploadContent);

    public ProgressButtonDemo()
    {
        InitializeComponent();

        DataContext = this;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(200)
        };
        _timer.Tick += Timer_Tick;
    }

    public int Progress
    {
        get => _progress;
        set
        {
            if (_progress == value)
            {
                return;
            }

            var oldValue = _progress;
            _progress = value;
            RaisePropertyChanged(ProgressProperty, oldValue, value);
        }
    }

    public bool IsUploading
    {
        get => _isUploading;
        set
        {
            if (_isUploading == value)
            {
                return;
            }

            var oldValue = _isUploading;
            var oldUploadContent = UploadContent;
            _isUploading = value;
            RaisePropertyChanged(IsUploadingProperty, oldValue, value);
            RaisePropertyChanged(UploadContentProperty, oldUploadContent, UploadContent);

            if (_isUploading)
            {
                if (!_timer.IsEnabled)
                {
                    _timer.Start();
                }
            }
            else
            {
                if (_timer.IsEnabled)
                {
                    _timer.Stop();
                }

                if (Progress != 0)
                {
                    Progress = 0;
                }
            }
        }
    }

    public string UploadContent
    {
        get
        {
            var values = Lang.UploadStr.Split(';');
            if (values.Length < 2)
            {
                return Lang.UploadStr;
            }

            return (IsUploading ? values[1] : values[0]).Trim();
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        Progress++;
        if (Progress >= 100)
        {
            Progress = 0;
            IsUploading = false;
        }
    }
}
