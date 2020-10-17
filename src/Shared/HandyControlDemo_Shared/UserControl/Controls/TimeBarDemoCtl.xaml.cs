#if !NET35
using System;
using HandyControl.Data;
#endif

namespace HandyControlDemo.UserControl
{
    public partial class TimeBarDemoCtl
    {
        public TimeBarDemoCtl()
        {
            InitializeComponent();

#if !NET35
            for (int i = 0; i < 10; i++)
            {
                var hour = 6 * i;
                TimeBarDemo.Hotspots.Add(new DateTimeRange(DateTime.Today.AddHours(hour), DateTime.Today.AddHours(hour + 1)));
                TimeBarDemo.Hotspots.Add(new DateTimeRange(DateTime.Today.AddHours(-hour), DateTime.Today.AddHours(-hour + 1)));
            }
#endif
        }
    }
}
