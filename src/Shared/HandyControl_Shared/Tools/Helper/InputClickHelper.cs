using System;
using System.Windows;
using System.Windows.Input;

namespace HandyControl.Tools;

/// <summary>
///     输入层点击帮助类
/// </summary>
public static class InputClickHelper
{
    private static readonly DependencyProperty InputInfoProperty = DependencyProperty.RegisterAttached(
        "InputInfo", typeof(InputInfo), typeof(InputClickHelper), new PropertyMetadata(default(InputInfo)));

    private static void SetInputInfo(DependencyObject element, InputInfo value) => element.SetValue(InputInfoProperty, value);

    private static InputInfo GetInputInfo(DependencyObject element) => (InputInfo) element.GetValue(InputInfoProperty);

    /// <summary>
    ///     将 MouseDown MouseMove MouseUp 封装为点击事件
    /// </summary>
    /// <param name="element">要被附加的元素</param>
    /// <param name="clickEventHandler">点击的事件</param>
    /// <param name="dragStarted">因为拖动而结束点击时触发</param>
    public static void AttachMouseDownMoveUpToClick(UIElement element, EventHandler clickEventHandler,
        EventHandler dragStarted = null)
    {
        var inputInfo = GetOrCreateInputInfo(element);
        inputInfo.ClickEventHandler += clickEventHandler;

        inputInfo.DragStarted += dragStarted;

        element.MouseDown -= Element_MouseDown;
        element.MouseDown += Element_MouseDown;
        element.MouseMove -= Element_MouseMove;
        element.MouseMove += Element_MouseMove;
        element.MouseUp -= Element_MouseUp;
        element.MouseUp += Element_MouseUp;
        element.LostMouseCapture -= Element_LostMouseCapture;
        element.LostMouseCapture += Element_LostMouseCapture;
    }

    /// <summary>
    ///     去掉对 <paramref name="element" /> 的点击时间的监听
    /// </summary>
    /// <param name="element"></param>
    /// <param name="clickEventHandler">点击的事件</param>
    /// <param name="dragStarted">因为拖动而结束点击时触发的事件</param>
    public static void DetachMouseDownMoveUpToClick(UIElement element, EventHandler clickEventHandler,
        EventHandler dragStarted = null)
    {
        var inputInfo = GetInputInfo(element);
        if (inputInfo == null) return;

        inputInfo.ClickEventHandler -= clickEventHandler;
        inputInfo.DragStarted -= dragStarted;

        if (inputInfo.IsEmpty())
        {
            element.ClearValue(InputInfoProperty);
            element.MouseDown -= Element_MouseDown;
            element.MouseMove -= Element_MouseMove;
            element.MouseUp -= Element_MouseUp;
            element.LostMouseCapture -= Element_LostMouseCapture;
        }
    }

    private static void Element_LostMouseCapture(object sender, MouseEventArgs e)
    {
        var element = (UIElement) sender;
        GetInputInfo(element)?.LostCapture();
    }

    private static void Element_MouseUp(object sender, MouseButtonEventArgs e)
    {
        var element = (UIElement) sender;
        GetInputInfo(element)?.Up(e.GetPosition(element));
    }

    private static void Element_MouseMove(object sender, MouseEventArgs e)
    {
        var element = (UIElement) sender;
        GetInputInfo(element)?.Move(e.GetPosition(element));
    }

    private static void Element_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var element = (UIElement) sender;
        GetInputInfo(element)?.Down(e.GetPosition(element));
    }

    private static InputInfo GetOrCreateInputInfo(UIElement element)
    {
        var inputInfo = GetInputInfo(element);
        if (inputInfo == null)
        {
            inputInfo = new InputInfo();
            SetInputInfo(element, inputInfo);
        }

        return inputInfo;
    }

    private class InputInfo
    {
        private const double ToleranceSquared = 0.01;

        private Point _downedPosition;

        private bool _isClick;

        public event EventHandler ClickEventHandler;

        public event EventHandler DragStarted;

        public void Down(Point position)
        {
            _downedPosition = position;
            _isClick = true;
        }

        public void Move(Point position)
        {
            if (!_isClick) return;

            if ((position - _downedPosition).LengthSquared > ToleranceSquared)
            {
                _isClick = false;
                DragStarted?.Invoke(null, EventArgs.Empty);
            }
        }

        public void Up(Point position)
        {
            _isClick = _isClick && (position - _downedPosition).LengthSquared <= ToleranceSquared;

            if (!_isClick) return;

            ClickEventHandler?.Invoke(null, EventArgs.Empty);

            _isClick = false;
        }

        public void LostCapture() => _isClick = false;

        public bool IsEmpty() => ClickEventHandler is null && DragStarted is null;
    }
}
