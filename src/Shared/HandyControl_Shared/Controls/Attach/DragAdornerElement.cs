using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Controls;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

public static class DragAdornerElement
{
    public static readonly DependencyProperty DragTargetProperty =
        DependencyProperty.RegisterAttached("DragTarget", typeof(UIElement), typeof(DragAdornerElement), null);

    public static void SetDragTarget(DependencyObject element, UIElement value) =>
        element.SetValue(DragTargetProperty, value);

    public static UIElement GetDragTarget(DependencyObject element) =>
        (UIElement) element.GetValue(DragTargetProperty);

    public static readonly DependencyProperty IsDraggableProperty =
        DependencyProperty.RegisterAttached("IsDraggable", typeof(bool), typeof(DragAdornerElement),
            new PropertyMetadata(false, OnIsDraggableChanged));

    public static void SetIsDraggable(DependencyObject element, bool value) =>
        element.SetValue(IsDraggableProperty, value);

    private static void OnIsDraggableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement handle)
        {
            ((FrameworkElement) handle).Cursor = Cursors.SizeAll;
            handle.MouseLeftButtonDown -= OnMouseDown;
            if ((bool) e.NewValue)
                handle.MouseLeftButtonDown += OnMouseDown;
        }
    }

    private static void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not UIElement handle || e.ButtonState != MouseButtonState.Pressed)
            return;

        var dragTarget = GetDragTarget(handle) ?? VisualTreeHelper.GetParent(handle) as UIElement;
        if (dragTarget == null) return;

        var adorner = GetAncestor<Adorner>(dragTarget);
        if (adorner == null) return;

        adorner.SizeChanged -= Adorner_SizeChanged;
        adorner.SizeChanged += Adorner_SizeChanged;

        var adornerLayer = VisualTreeHelper.GetParent(adorner) as FrameworkElement;
        if (adornerLayer == null) return;

        handle.CaptureMouse();

        // Get Or Set TranslateTransform
        TranslateTransform currentTransform;
        if (dragTarget.RenderTransform is TranslateTransform tt)
        {
            currentTransform = tt;
        }
        else
        {
            currentTransform = new TranslateTransform();
            if (dragTarget.RenderTransform == null)
            {
                dragTarget.RenderTransform = currentTransform;
            }
            else
            {
                // TransformGroup
                dragTarget.RenderTransform = new TransformGroup
                {
                    Children = { dragTarget.RenderTransform, currentTransform }
                };
            }
        }

        double currentX = currentTransform.X;
        double currentY = currentTransform.Y;

        // 全局坐标系位置
        var mouseInLayer = e.GetPosition(adornerLayer);
        // 相对坐标系位置
        var relatePos = e.GetPosition(dragTarget);

        // 通过鼠标初始位置与当前偏移量计算出偏移公式
        var gripOffset = new Point(mouseInLayer.X - currentX, mouseInLayer.Y - currentY);

        void OnMouseMove(object s, MouseEventArgs args)
        {
            if (args.LeftButton != MouseButtonState.Pressed || !handle.IsMouseCaptured)
                return;

            // 实时获取当前尺寸（应对布局或窗口大小变化）
            double targetWidth = dragTarget.RenderSize.Width;
            double targetHeight = dragTarget.RenderSize.Height;

            if (targetWidth <= 1)
                targetWidth = (dragTarget as FrameworkElement)?.ActualWidth ?? 100;
            if (targetHeight <= 1)
                targetHeight = (dragTarget as FrameworkElement)?.ActualHeight ?? 100;

            double layerWidth = adornerLayer.ActualWidth;
            double layerHeight = adornerLayer.ActualHeight;

            if (layerWidth <= 0 || layerHeight <= 0)
            {
                var window = System.Windows.Window.GetWindow(adornerLayer);
                layerWidth = window?.ActualWidth ?? SystemParameters.WorkArea.Width;
                layerHeight = window?.ActualHeight ?? SystemParameters.WorkArea.Height;
            }

            // 全局坐标系
            var currentMouse = args.GetPosition(adornerLayer);
            var desiredX = currentMouse.X - gripOffset.X;
            var desiredY = currentMouse.Y - gripOffset.Y;

            // 计算允许的拖动边界
            double minX = relatePos.X - gripOffset.X;
            double maxX = layerWidth - (targetWidth - relatePos.X) - gripOffset.X;
            double minY = relatePos.Y - gripOffset.Y;
            double maxY = layerHeight - (targetHeight - relatePos.Y) - gripOffset.Y;

            var clampedX = Math.Max(minX, Math.Min(maxX, desiredX));
            var clampedY = Math.Max(minY, Math.Min(maxY, desiredY));

            // 调试日志（可选，发布时可移除）
            System.Diagnostics.Debug.WriteLine(
                $"currentMouse=({currentMouse.X:F1},{currentMouse.Y:F1}) | " +
                $"gripOffset=({gripOffset.X:F1},{gripOffset.Y:F1}) | " +
                $"desiredX=({desiredX:F1},{desiredY:F1}) | " +
                $"clampedX=({clampedX:F1},{clampedY:F1}) | " +
                $"X:[{minX:F1}, {maxX:F1}] Y:[{minY:F1}, {maxY:F1}]"
            );

            currentTransform.X = clampedX;
            currentTransform.Y = clampedY;
        }

        void OnMouseUp(object s, MouseButtonEventArgs args)
        {
            handle.ReleaseMouseCapture();
            handle.MouseMove -= OnMouseMove;
            handle.MouseLeftButtonUp -= OnMouseUp;
        }

        // 先解绑再绑定，防止重复注册
        handle.MouseMove -= OnMouseMove;
        handle.MouseLeftButtonUp -= OnMouseUp;
        handle.MouseMove += OnMouseMove;
        handle.MouseLeftButtonUp += OnMouseUp;

        e.Handled = true;
    }

    private static void Adorner_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // 当 Adorner 尺寸变化时，修改拖动目标的位置以保持在边界内
        if (sender is not AdornerContainer adorner) return;
        // 获取dialog实例
        if (adorner.Child is not Dialog dialog)
        {
            return;
        }
        // 获取拖动目标
        var dragTarget = dialog.Content as FrameworkElement;
        if (dragTarget == null) return;
        // 获取当前Transform
        if (dragTarget.RenderTransform is not TransformGroup transformGroup) return;
        TranslateTransform currentTransform = null;
        foreach (var item in transformGroup.Children)
        {
            currentTransform = item as TranslateTransform;
            if (currentTransform != null)
            {
                break;
            }
        }
        if (currentTransform == null)
        {
            return;
        }
        // 父级容器大小变化重置当前dialog偏移为0
        currentTransform.X = 0;
        currentTransform.Y = 0;
    }

    // 辅助方法：向上查找祖先
    private static T GetAncestor<T>(DependencyObject obj) where T : DependencyObject
    {
        while (obj != null)
        {
            if (obj is T result)
                return result;
            obj = VisualTreeHelper.GetParent(obj);
        }
        return null;
    }
}
