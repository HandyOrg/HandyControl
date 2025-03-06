using System;
using HandyControl.Data;

namespace HandyControlDemo.UserControl;

public partial class TimeBarDemo
{
    public TimeBarDemo()
    {
        InitializeComponent();

        for (int i = 0; i < 10; i++)
        {
            var hour = 6 * i;
            timeBarDemo.Hotspots.Add(new DateTimeRange(DateTime.Today.AddHours(hour), DateTime.Today.AddHours(hour + 1)));
            timeBarDemo.Hotspots.Add(new DateTimeRange(DateTime.Today.AddHours(-hour), DateTime.Today.AddHours(-hour + 1)));
        }
    }
}
