using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls;

internal class ColorDropper
{
    private bool _cursorIsSetted;

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
                var info = Application.GetResourceStream(new Uri("pack://application:,,,/HandyControl;Component/Resources/dropper.cur"));
                if (info != null)
                {
                    _dropperCursor = new Cursor(info.Stream);
                }
            }

            if (_dropperCursor == null) return;

            MouseHook.Start();
            MouseHook.StatusChanged += MouseHook_StatusChanged;
        }
        else
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            MouseHook.Stop();
            MouseHook.StatusChanged -= MouseHook_StatusChanged;
        }
    }

    private void MouseHook_StatusChanged(object sender, MouseHookEventArgs e)
    {
        var window = System.Windows.Window.GetWindow(_colorPicker);
        if (window == null)
        {
            UpdateCursor(false);

            return;
        }

        if (!_colorPicker.IsMouseOver && window.IsMouseOver)
        {
            UpdateCursor(true);
            if (e.MessageType == MouseHookMessageType.LeftButtonDown)
            {
                var brush = new SolidColorBrush(GetColorAt(e.Point.X, e.Point.Y));
                _colorPicker.SelectedBrush = brush;
            }
        }
        else
        {
            UpdateCursor(false);
        }
    }

    private void UpdateCursor(bool isDropper)
    {
        if (isDropper)
        {
            Mouse.Captured?.ReleaseMouseCapture();

            if (!_cursorIsSetted)
            {
                Mouse.OverrideCursor = _dropperCursor;
                _cursorIsSetted = true;
            }
        }
        else
        {
            if (_cursorIsSetted)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                _cursorIsSetted = false;
            }
        }
    }

    public static Color GetColorAt(int x, int y)
    {
        var desk = InteropMethods.GetDesktopWindow();
        var dc = InteropMethods.GetWindowDC(desk);
        var a = (int) InteropMethods.GetPixel(dc, x, y);
        InteropMethods.ReleaseDC(desk, dc);
        return Color.FromArgb(255, (byte) ((a >> 0) & 0xff), (byte) ((a >> 8) & 0xff), (byte) ((a >> 16) & 0xff));
    }
}
