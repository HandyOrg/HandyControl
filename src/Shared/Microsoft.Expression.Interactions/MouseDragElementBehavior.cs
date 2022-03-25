using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Interactivity;

public class MouseDragElementBehavior : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty ConstrainToParentBoundsProperty =
        DependencyProperty.Register("ConstrainToParentBounds", typeof(bool), typeof(MouseDragElementBehavior),
            new PropertyMetadata(ValueBoxes.FalseBox, OnConstrainToParentBoundsChanged));

    public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double),
        typeof(MouseDragElementBehavior), new PropertyMetadata(double.PositiveInfinity, OnXChanged));

    public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double),
        typeof(MouseDragElementBehavior), new PropertyMetadata(double.PositiveInfinity, OnYChanged));

    // Fields
    private Transform _cachedRenderTransform;

    private Point _relativePosition;
    private bool _settingPosition;

    // Properties

    public bool ConstrainToParentBounds
    {
        get => (bool) GetValue(ConstrainToParentBoundsProperty);
        set => SetValue(ConstrainToParentBoundsProperty, ValueBoxes.BooleanBox(value));
    }

    private Rect ElementBounds
    {
        get
        {
            var layoutRect = ExtendedVisualStateManager.GetLayoutRect(AssociatedObject);
            return new Rect(new Point(0.0, 0.0), new Size(layoutRect.Width, layoutRect.Height));
        }
    }

    private FrameworkElement ParentElement =>
        AssociatedObject.Parent as FrameworkElement;

    private Transform RenderTransform
    {
        get
        {
            if (_cachedRenderTransform == null ||
                !ReferenceEquals(_cachedRenderTransform, AssociatedObject.RenderTransform))
            {
                var transform = CloneTransform(AssociatedObject.RenderTransform);
                RenderTransform = transform;
            }
            return _cachedRenderTransform;
        }
        set
        {
            if (!Equals(_cachedRenderTransform, value))
            {
                _cachedRenderTransform = value;
                AssociatedObject.RenderTransform = value;
            }
        }
    }

    private UIElement RootElement
    {
        get
        {
            DependencyObject associatedObject = AssociatedObject;
            for (var obj3 = associatedObject; obj3 != null; obj3 = VisualTreeHelper.GetParent(associatedObject))
                associatedObject = obj3;
            return associatedObject as UIElement;
        }
    }

    public double X
    {
        get =>
            (double) GetValue(XProperty);
        set => SetValue(XProperty, value);
    }

    public double Y
    {
        get =>
            (double) GetValue(YProperty);
        set => SetValue(YProperty, value);
    }

    // Events
    public event MouseEventHandler DragBegun;

    public event MouseEventHandler DragFinished;

    public event MouseEventHandler Dragging;

    // Methods
    private void ApplyTranslation(double x, double y)
    {
        if (ParentElement != null)
        {
            var point = TransformAsVector(RootElement.TransformToVisual(ParentElement), x, y);
            x = point.X;
            y = point.Y;
            if (ConstrainToParentBounds)
            {
                var parentElement = ParentElement;
                var rect = new Rect(0.0, 0.0, parentElement.ActualWidth, parentElement.ActualHeight);
                var transform2 = AssociatedObject.TransformToVisual(parentElement);
                var elementBounds = ElementBounds;
                var rect3 = transform2.TransformBounds(elementBounds);
                rect3.X += x;
                rect3.Y += y;
                if (!RectContainsRect(rect, rect3))
                {
                    if (rect3.X < rect.Left)
                    {
                        var num = rect3.X - rect.Left;
                        x -= num;
                    }
                    else if (rect3.Right > rect.Right)
                    {
                        var num2 = rect3.Right - rect.Right;
                        x -= num2;
                    }
                    if (rect3.Y < rect.Top)
                    {
                        var num3 = rect3.Y - rect.Top;
                        y -= num3;
                    }
                    else if (rect3.Bottom > rect.Bottom)
                    {
                        var num4 = rect3.Bottom - rect.Bottom;
                        y -= num4;
                    }
                }
            }
            ApplyTranslationTransform(x, y);
        }
    }

    internal void ApplyTranslationTransform(double x, double y)
    {
        var renderTransform = RenderTransform;
        var transform2 = renderTransform as TranslateTransform;
        if (transform2 == null)
        {
            var group = renderTransform as TransformGroup;
            var transform3 = renderTransform as MatrixTransform;
            if (group != null)
            {
                if (group.Children.Count > 0)
                    transform2 = group.Children[group.Children.Count - 1] as TranslateTransform;
                if (transform2 == null)
                {
                    transform2 = new TranslateTransform();
                    group.Children.Add(transform2);
                }
            }
            else
            {
                if (transform3 != null)
                {
                    var matrix = transform3.Matrix;
                    matrix.OffsetX += x;
                    matrix.OffsetY += y;
                    var transform4 = new MatrixTransform
                    {
                        Matrix = matrix
                    };
                    RenderTransform = transform4;
                    return;
                }
                var group2 = new TransformGroup();
                transform2 = new TranslateTransform();
                if (renderTransform != null)
                    group2.Children.Add(renderTransform);
                group2.Children.Add(transform2);
                RenderTransform = group2;
            }
        }
        transform2.X += x;
        transform2.Y += y;
    }

    internal static Transform CloneTransform(Transform transform)
    {
        if (transform == null)
            return null;
        if (transform is ScaleTransform transform2)
            return new ScaleTransform
            {
                CenterX = transform2.CenterX,
                CenterY = transform2.CenterY,
                ScaleX = transform2.ScaleX,
                ScaleY = transform2.ScaleY
            };
        if (transform is RotateTransform transform3)
            return new RotateTransform
            {
                Angle = transform3.Angle,
                CenterX = transform3.CenterX,
                CenterY = transform3.CenterY
            };
        if (transform is SkewTransform transform4)
            return new SkewTransform
            {
                AngleX = transform4.AngleX,
                AngleY = transform4.AngleY,
                CenterX = transform4.CenterX,
                CenterY = transform4.CenterY
            };
        if (transform is TranslateTransform transform5)
            return new TranslateTransform
            {
                X = transform5.X,
                Y = transform5.Y
            };
        if (transform is MatrixTransform transform6)
            return new MatrixTransform { Matrix = transform6.Matrix };
        if (!(transform is TransformGroup group))
            return null;
        var group2 = new TransformGroup();
        foreach (var transform12 in group.Children)
            group2.Children.Add(CloneTransform(transform12));
        return group2;
    }

    internal void EndDrag()
    {
        AssociatedObject.MouseMove -= OnMouseMove;
        AssociatedObject.LostMouseCapture -= OnLostMouseCapture;
        AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonUpEvent,
            new MouseButtonEventHandler(OnMouseLeftButtonUp));
    }

    private static Point GetTransformOffset(GeneralTransform transform)
    {
        return transform.Transform(new Point(0.0, 0.0));
    }

    internal void HandleDrag(Point newPositionInElementCoordinates)
    {
        var x = newPositionInElementCoordinates.X - _relativePosition.X;
        var y = newPositionInElementCoordinates.Y - _relativePosition.Y;
        var point = TransformAsVector(AssociatedObject.TransformToVisual(RootElement), x, y);
        _settingPosition = true;
        ApplyTranslation(point.X, point.Y);
        UpdatePosition();
        _settingPosition = false;
    }

    protected override void OnAttached()
    {
        AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent,
            new MouseButtonEventHandler(OnMouseLeftButtonDown), false);
    }

    private static void OnConstrainToParentBoundsChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
        var behavior = (MouseDragElementBehavior) sender;
        behavior.UpdatePosition(new Point(behavior.X, behavior.Y));
    }

    protected override void OnDetaching()
    {
        AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent,
            new MouseButtonEventHandler(OnMouseLeftButtonDown));
    }

    private void OnLostMouseCapture(object sender, MouseEventArgs e)
    {
        EndDrag();
        DragFinished?.Invoke(this, e);
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        StartDrag(e.GetPosition(AssociatedObject));
        DragBegun?.Invoke(this, e);
    }

    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        AssociatedObject.ReleaseMouseCapture();
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        HandleDrag(e.GetPosition(AssociatedObject));
        Dragging?.Invoke(this, e);
    }

    private static void OnXChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
        var behavior = (MouseDragElementBehavior) sender;
        behavior.UpdatePosition(new Point((double) args.NewValue, behavior.Y));
    }

    private static void OnYChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
        var behavior = (MouseDragElementBehavior) sender;
        behavior.UpdatePosition(new Point(behavior.X, (double) args.NewValue));
    }

    private static bool RectContainsRect(Rect rect1, Rect rect2)
    {
        if (rect1.IsEmpty || rect2.IsEmpty)
            return false;
        return rect1.X <= rect2.X && rect1.Y <= rect2.Y && rect1.X + rect1.Width >= rect2.X + rect2.Width &&
               rect1.Y + rect1.Height >= rect2.Y + rect2.Height;
    }

    internal void StartDrag(Point positionInElementCoordinates)
    {
        _relativePosition = positionInElementCoordinates;
        AssociatedObject.CaptureMouse();
        AssociatedObject.MouseMove += OnMouseMove;
        AssociatedObject.LostMouseCapture += OnLostMouseCapture;
        AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent,
            new MouseButtonEventHandler(OnMouseLeftButtonUp), false);
    }

    private static Point TransformAsVector(GeneralTransform transform, double x, double y)
    {
        var point = transform.Transform(new Point(0.0, 0.0));
        var point2 = transform.Transform(new Point(x, y));
        return new Point(point2.X - point.X, point2.Y - point.Y);
    }

    private void UpdatePosition()
    {
        var transformOffset = GetTransformOffset(AssociatedObject.TransformToVisual(RootElement));
        X = transformOffset.X;
        Y = transformOffset.Y;
    }

    private void UpdatePosition(Point point)
    {
        if (!_settingPosition && AssociatedObject != null)
        {
            var transformOffset = GetTransformOffset(AssociatedObject.TransformToVisual(RootElement));
            var x = double.IsNaN(point.X) ? 0.0 : point.X - transformOffset.X;
            var y = double.IsNaN(point.Y) ? 0.0 : point.Y - transformOffset.Y;
            ApplyTranslation(x, y);
        }
    }
}
