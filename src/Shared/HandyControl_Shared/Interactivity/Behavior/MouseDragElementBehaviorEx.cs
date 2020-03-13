using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Interactivity
{
    /// <summary>
    ///     鼠标拖动行为，该类是反编译微软的System.Windows.Interactivity程序集得到的，并对其做了些扩展
    /// </summary>
    internal class MouseDragElementBehaviorEx : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(double),
            typeof(MouseDragElementBehaviorEx), new PropertyMetadata(double.NaN, OnXChanged));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y), typeof(double),
            typeof(MouseDragElementBehaviorEx), new PropertyMetadata(double.NaN, OnYChanged));

        public static readonly DependencyProperty ConstrainToParentBoundsProperty =
            DependencyProperty.Register(nameof(ConstrainToParentBounds), typeof(bool),
                typeof(MouseDragElementBehaviorEx), new PropertyMetadata(ValueBoxes.FalseBox, OnConstrainToParentBoundsChanged));

        private Transform _cachedRenderTransform;
        private Point _relativePosition;
        private bool _settingPosition;

        /// <summary>
        ///     是否固定住Y轴
        /// </summary>
        public bool LockY { get; set; }

        /// <summary>
        ///     是否固定住X轴
        /// </summary>
        public bool LockX { get; set; }

        public double X
        {
            get => (double)GetValue(MouseDragElementBehavior.XProperty);
            set => SetValue(MouseDragElementBehavior.XProperty, value);
        }

        public double Y
        {
            get => (double)GetValue(MouseDragElementBehavior.YProperty);
            set => SetValue(MouseDragElementBehavior.YProperty, value);
        }

        public bool ConstrainToParentBounds
        {
            get => (bool)GetValue(MouseDragElementBehavior.ConstrainToParentBoundsProperty);
            set => SetValue(MouseDragElementBehavior.ConstrainToParentBoundsProperty, value);
        }

        private Rect ElementBounds
        {
            get
            {
                var layoutRect = ArithmeticHelper.GetLayoutRect(AssociatedObject);
                return new Rect(new Point(0.0, 0.0), new Size(layoutRect.Width, layoutRect.Height));
            }
        }

        private FrameworkElement ParentElement => AssociatedObject.Parent as FrameworkElement;

        private UIElement RootElement
        {
            get
            {
                DependencyObject reference = AssociatedObject;
                for (var dependencyObject = reference;
                    dependencyObject != null;
                    dependencyObject = VisualTreeHelper.GetParent(reference))
                    reference = dependencyObject;
                return reference as UIElement;
            }
        }

        private Transform RenderTransform
        {
            get
            {
                if (_cachedRenderTransform == null ||
                    !ReferenceEquals(_cachedRenderTransform, AssociatedObject.RenderTransform))
                    RenderTransform = CloneTransform(AssociatedObject.RenderTransform);
                return _cachedRenderTransform;
            }
            set
            {
                if (Equals(_cachedRenderTransform, value))
                    return;
                _cachedRenderTransform = value;
                AssociatedObject.RenderTransform = value;
            }
        }

        public event MouseEventHandler DragBegun;

        public event MouseEventHandler Dragging;

        public event MouseEventHandler DragFinished;

        private static void OnXChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var dragElementBehavior = (MouseDragElementBehaviorEx)sender;
            dragElementBehavior.UpdatePosition(new Point((double)args.NewValue, dragElementBehavior.Y));
        }

        private static void OnYChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var dragElementBehavior = (MouseDragElementBehaviorEx)sender;
            dragElementBehavior.UpdatePosition(new Point(dragElementBehavior.X, (double)args.NewValue));
        }

        private static void OnConstrainToParentBoundsChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var dragElementBehavior = (MouseDragElementBehaviorEx)sender;
            dragElementBehavior.UpdatePosition(new Point(dragElementBehavior.X, dragElementBehavior.Y));
        }

        private void UpdatePosition(Point point)
        {
            if (_settingPosition || AssociatedObject == null)
                return;
            var transformOffset = GetTransformOffset(AssociatedObject.TransformToVisual(RootElement));
            ApplyTranslation(double.IsNaN(point.X) ? 0.0 : point.X - transformOffset.X,
                double.IsNaN(point.Y) ? 0.0 : point.Y - transformOffset.Y);
        }

        private void ApplyTranslation(double x, double y)
        {
            if (ParentElement == null)
                return;
            var point = TransformAsVector(RootElement.TransformToVisual(ParentElement), x, y);
            x = point.X;
            y = point.Y;
            if (ConstrainToParentBounds)
            {
                var parentElement = ParentElement;
                var rect1 = new Rect(0.0, 0.0, parentElement.ActualWidth, parentElement.ActualHeight);
                var rect2 = AssociatedObject.TransformToVisual(parentElement).TransformBounds(ElementBounds);
                rect2.X += x;
                rect2.Y += y;
                if (!RectContainsRect(rect1, rect2))
                {
                    if (rect2.X < rect1.Left)
                    {
                        var num = rect2.X - rect1.Left;
                        x -= num;
                    }
                    else if (rect2.Right > rect1.Right)
                    {
                        var num = rect2.Right - rect1.Right;
                        x -= num;
                    }
                    if (rect2.Y < rect1.Top)
                    {
                        var num = rect2.Y - rect1.Top;
                        y -= num;
                    }
                    else if (rect2.Bottom > rect1.Bottom)
                    {
                        var num = rect2.Bottom - rect1.Bottom;
                        y -= num;
                    }
                }
            }
            ApplyTranslationTransform(x, y);
        }

        internal void ApplyTranslationTransform(double x, double y)
        {
            var renderTransform = RenderTransform;
            var translateTransform = renderTransform as TranslateTransform;
            if (translateTransform == null)
            {
                var matrixTransform = renderTransform as MatrixTransform;
                if (renderTransform is TransformGroup transformGroup1)
                {
                    if (transformGroup1.Children.Count > 0)
                        translateTransform =
                            transformGroup1.Children[transformGroup1.Children.Count - 1] as TranslateTransform;
                    if (translateTransform == null)
                    {
                        translateTransform = new TranslateTransform();
                        transformGroup1.Children.Add(translateTransform);
                    }
                }
                else
                {
                    if (matrixTransform != null)
                    {
                        var matrix = matrixTransform.Matrix;
                        //在该处对微软的类进行了修改
                        if (!LockX)
                        {
                            matrix.OffsetX += x;
                        }
                        if (!LockY)
                        {
                            matrix.OffsetY += y;
                        }
                        //修改结束
                        RenderTransform = new MatrixTransform
                        {
                            Matrix = matrix
                        };
                        return;
                    }
                    var transformGroup2 = new TransformGroup();
                    translateTransform = new TranslateTransform();
                    if (renderTransform != null)
                        transformGroup2.Children.Add(renderTransform);
                    transformGroup2.Children.Add(translateTransform);
                    RenderTransform = transformGroup2;
                }
            }
            //在该处对微软的类进行了修改
            if (!LockX)
            {
                translateTransform.X += x;
            }
            if (!LockY)
            {
                translateTransform.Y += y;
            }
            //修改结束
        }

        internal static Transform CloneTransform(Transform transform)
        {
            if (transform == null)
                return null;
            if (transform is ScaleTransform scaleTransform)
                return new ScaleTransform
                {
                    CenterX = scaleTransform.CenterX,
                    CenterY = scaleTransform.CenterY,
                    ScaleX = scaleTransform.ScaleX,
                    ScaleY = scaleTransform.ScaleY
                };
            if (transform is RotateTransform rotateTransform)
                return new RotateTransform
                {
                    Angle = rotateTransform.Angle,
                    CenterX = rotateTransform.CenterX,
                    CenterY = rotateTransform.CenterY
                };
            if (transform is SkewTransform skewTransform)
                return new SkewTransform
                {
                    AngleX = skewTransform.AngleX,
                    AngleY = skewTransform.AngleY,
                    CenterX = skewTransform.CenterX,
                    CenterY = skewTransform.CenterY
                };
            if (transform is TranslateTransform translateTransform)
                return new TranslateTransform
                {
                    X = translateTransform.X,
                    Y = translateTransform.Y
                };
            if (transform is MatrixTransform matrixTransform)
                return new MatrixTransform
                {
                    Matrix = matrixTransform.Matrix
                };
            if (!(transform is TransformGroup transformGroup1))
                return null;
            var transformGroup2 = new TransformGroup();
            foreach (var child in transformGroup1.Children)
                transformGroup2.Children.Add(CloneTransform(child));
            return transformGroup2;
        }

        private void UpdatePosition()
        {
            var transformOffset = GetTransformOffset(AssociatedObject.TransformToVisual(RootElement));
            X = transformOffset.X;
            Y = transformOffset.Y;
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

        internal void HandleDrag(Point newPositionInElementCoordinates)
        {
            var point = TransformAsVector(AssociatedObject.TransformToVisual(RootElement),
                newPositionInElementCoordinates.X - _relativePosition.X,
                newPositionInElementCoordinates.Y - _relativePosition.Y);
            _settingPosition = true;
            ApplyTranslation(point.X, point.Y);
            UpdatePosition();
            _settingPosition = false;
        }

        internal void EndDrag()
        {
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.LostMouseCapture -= OnLostMouseCapture;
            AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonUpEvent,
                new MouseButtonEventHandler(OnMouseLeftButtonUp));
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartDrag(e.GetPosition(AssociatedObject));
            DragBegun?.Invoke(this, e);
        }

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            EndDrag();
            DragFinished?.Invoke(this, e);
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

        private static bool RectContainsRect(Rect rect1, Rect rect2)
        {
            if (rect1.IsEmpty || rect2.IsEmpty || rect1.X > rect2.X || rect1.Y > rect2.Y ||
                rect1.X + rect1.Width < rect2.X + rect2.Width)
                return false;
            return rect1.Y + rect1.Height >= rect2.Y + rect2.Height;
        }

        private static Point TransformAsVector(GeneralTransform transform, double x, double y)
        {
            var point1 = transform.Transform(new Point(0.0, 0.0));
            var point2 = transform.Transform(new Point(x, y));
            return new Point(point2.X - point1.X, point2.Y - point1.Y);
        }

        private static Point GetTransformOffset(GeneralTransform transform)
        {
            return transform.Transform(new Point(0.0, 0.0));
        }

        protected override void OnAttached()
        {
            AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler(OnMouseLeftButtonDown), false);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler(OnMouseLeftButtonDown));
        }
    }

}