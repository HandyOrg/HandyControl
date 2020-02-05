using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools
{
    internal class ScreenHelper
    {
        internal static void FindMaximumSingleMonitorRectangle(Rect windowRect, out Rect screenSubRect, out Rect monitorRect)
        {
            var windowRect2 = new InteropValues.RECT(windowRect);
            FindMaximumSingleMonitorRectangle(windowRect2, out var rect, out var rect2);
            screenSubRect = new Rect(rect.Position, rect.Size);
            monitorRect = new Rect(rect2.Position, rect2.Size);
        }

        private static void FindMaximumSingleMonitorRectangle(InteropValues.RECT windowRect, out InteropValues.RECT screenSubRect, out InteropValues.RECT monitorRect)
        {
            var rects = new List<InteropValues.RECT>();
            InteropMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                delegate(IntPtr hMonitor, IntPtr hdcMonitor, ref InteropValues.RECT rect, IntPtr lpData)
                {
                    var monitorInfo = default(InteropValues.MONITORINFO);
                    monitorInfo.cbSize = (uint) Marshal.SizeOf(typeof(InteropValues.MONITORINFO));
                    InteropMethods.GetMonitorInfo(hMonitor, ref monitorInfo);
                    rects.Add(monitorInfo.rcWork);
                    return true;
                }, IntPtr.Zero);
            
            var num = 0L;
            
            screenSubRect = new InteropValues.RECT
            {
                Left = 0,
                Right = 0,
                Top = 0,
                Bottom = 0
            };
            
            monitorRect = new InteropValues.RECT
            {
                Left = 0,
                Right = 0,
                Top = 0,
                Bottom = 0
            };
            
            foreach (var current in rects)
            {
                var rect = current;
                InteropMethods.IntersectRect(out var rECT2, ref rect, ref windowRect);
                var num2 = (long) (rECT2.Width * rECT2.Height);
                if (num2 > num)
                {
                    screenSubRect = rECT2;
                    monitorRect = current;
                    num = num2;
                }
            }
        }

        internal static void FindMonitorRectsFromPoint(Point point, out Rect monitorRect, out Rect workAreaRect)
        {
            var intPtr = InteropMethods.MonitorFromPoint(new InteropValues.POINT
            {
                X = (int) point.X,
                Y = (int) point.Y
            }, 2);
            
            monitorRect = new Rect(0.0, 0.0, 0.0, 0.0);
            workAreaRect = new Rect(0.0, 0.0, 0.0, 0.0);
            
            if (intPtr != IntPtr.Zero)
            {
                InteropValues.MONITORINFO monitorInfo = default;
                monitorInfo.cbSize = (uint) Marshal.SizeOf(typeof(InteropValues.MONITORINFO));
                InteropMethods.GetMonitorInfo(intPtr, ref monitorInfo);
                monitorRect = new Rect(monitorInfo.rcMonitor.Position, monitorInfo.rcMonitor.Size);
                workAreaRect = new Rect(monitorInfo.rcWork.Position, monitorInfo.rcWork.Size);
            }
        }
    }
}