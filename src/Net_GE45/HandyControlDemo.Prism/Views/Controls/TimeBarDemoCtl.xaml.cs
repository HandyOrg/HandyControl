using HandyControl.Data;
using System;

namespace HandyControlDemo.Views
{
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
}
