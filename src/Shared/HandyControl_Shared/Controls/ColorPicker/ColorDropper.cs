using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls
{
    internal class ColorDropper : Control
    {
        private Cursor _dropperCursor;

        private readonly ColorPicker _colorPicker;

        public ColorDropper(ColorPicker colorPicker)
        {
            _colorPicker = colorPicker;
        }

        public void Update(bool isShow)
        {
            if (isShow)
            {
                if (_dropperCursor == null)
                {
                    var info = Application.GetResourceStream(
                        new Uri("pack://application:,,,/HandyControl;Component/Resources/dropper.cur"));
                    if (info != null)
                    {
                        _dropperCursor = new Cursor(info.Stream);
                    }
                }

                if (_dropperCursor == null) return;

                Mouse.OverrideCursor = _dropperCursor;
                MouseHook.Start();
                MouseHook.StatusChanged += MouseHook_StatusChanged;
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void MouseHook_StatusChanged(object sender, MouseHookEventArgs e)
        {
            if (!_colorPicker.IsMouseOver && e.Message == MouseHookMessageType.LeftButtonDown)
            {
                var brush = new SolidColorBrush(GetColorAt(e.Point.X, e.Point.Y));
                _colorPicker.SelectedBrush = brush;
                //MouseHook.Stop();
                //MouseHook.StatusChanged -= MouseHook_StatusChanged;
            }
        }

        public static Color GetColorAt(int x, int y)
        {
            var desk = UnsafeNativeMethods.GetDesktopWindow();
            var dc = UnsafeNativeMethods.GetWindowDC(desk);
            var a = (int)UnsafeNativeMethods.GetPixel(dc, x, y);
            UnsafeNativeMethods.ReleaseDC(desk, dc);
            return Color.FromArgb(255, (byte) ((a >> 0) & 0xff), (byte) ((a >> 8) & 0xff), (byte) ((a >> 16) & 0xff));
        }
    }
}