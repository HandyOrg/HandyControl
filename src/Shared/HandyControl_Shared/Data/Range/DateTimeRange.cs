using System;

namespace HandyControl.Data;

public struct DateTimeRange : IValueRange<DateTime>
{
    public DateTimeRange(DateTime start)
    {
        Start = start;
        End = start;
    }

    public DateTimeRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public double TotalMilliseconds => (End - Start).TotalMilliseconds;
}
