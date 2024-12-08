using System;
using HandyControl.Data;

namespace HandyControlDemo.UserControl;

public partial class TimeBarDemoCtl
{
    public TimeBarDemoCtl()
    {
        InitializeComponent();

        for (int i = 0; i < 10; i++)
        {
            var hour = 6 * i;
            TimeBarDemo.Hotspots.Add(new DateTimeRange(DateTime.Today.AddHours(hour), DateTime.Today.AddHours(hour + 1)));
            TimeBarDemo.Hotspots.Add(new DateTimeRange(DateTime.Today.AddHours(-hour), DateTime.Today.AddHours(-hour + 1)));
        }
    }
}
