using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using HandyControl.Data;

namespace HandyControl.Interactivity
{
    public sealed class FluidMoveBehavior : FluidMoveBehaviorBase
    {
        private static readonly DependencyProperty CacheDuringOverlayProperty =
            DependencyProperty.RegisterAttached("CacheDuringOverlay", typeof(object),
                typeof(FluidMoveBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration",
            typeof(Duration), typeof(FluidMoveBehavior),
            new PropertyMetadata(new Duration(TimeSpan.FromSeconds(1.0))));

        public static readonly DependencyProperty EaseXProperty = DependencyProperty.Register("EaseX",
            typeof(IEasingFunction), typeof(FluidMoveBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty EaseYProperty = DependencyProperty.Register("EaseY",
            typeof(IEasingFunction), typeof(FluidMoveBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty FloatAboveProperty =
            DependencyProperty.Register("FloatAbove", typeof(bool), typeof(FluidMoveBehavior),
                new PropertyMetadata(ValueBoxes.TrueBox));

        private static readonly DependencyProperty HasTransformWrapperProperty =
            DependencyProperty.RegisterAttached("HasTransformWrapper", typeof(bool),
                typeof(FluidMoveBehavior), new PropertyMetadata(ValueBoxes.FalseBox));

        private static readonly DependencyProperty InitialIdentityTagProperty =
            DependencyProperty.RegisterAttached("InitialIdentityTag", typeof(object),
                typeof(FluidMoveBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty InitialTagPathProperty =
            DependencyProperty.Register("InitialTagPath", typeof(string), typeof(FluidMoveBehavior),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty InitialTagProperty =
            DependencyProperty.Register("InitialTag", typeof(TagType), typeof(FluidMoveBehavior),
                new PropertyMetadata(TagType.Element));

        private static readonly DependencyProperty OverlayProperty =
            DependencyProperty.RegisterAttached("Overlay", typeof(object), typeof(FluidMoveBehavior),
                new PropertyMetadata(null));

        private static readonly Dictionary<object, Storyboard> TransitionStoryboardDictionary =
            new Dictionary<object, Storyboard>();

        public Duration Duration
        {
            get =>
                (Duration) GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public IEasingFunction EaseX
        {
            get =>
                (IEasingFunction) GetValue(EaseXProperty);
            set => SetValue(EaseXProperty, value);
        }

        public IEasingFunction EaseY
        {
            get =>
                (IEasingFunction) GetValue(EaseYProperty);
            set => SetValue(EaseYProperty, value);
        }

        public bool FloatAbove
        {
            get =>
                (bool) GetValue(FloatAboveProperty);
            set => SetValue(FloatAboveProperty, value);
        }

        public TagType InitialTag
        {
            get =>
                (TagType) GetValue(InitialTagProperty);
            set => SetValue(InitialTagProperty, value);
        }

        public string InitialTagPath
        {
            get =>
                (string) GetValue(InitialTagPathProperty);
            set => SetValue(InitialTagPathProperty, value);
        }

        protected override bool ShouldSkipInitialLayout
        {
            get
            {
                if (!base.ShouldSkipInitialLayout) return InitialTag == TagType.DataContext;
                return true;
            }
        }

        private static void AddTransform(FrameworkElement child, Transform transform)
        {
            if (!(child.RenderTransform is TransformGroup renderTransform))
            {
                renderTransform = new TransformGroup
                {
                    Children = {child.RenderTransform}
                };
                child.RenderTransform = renderTransform;
                SetHasTransformWrapper(child, true);
            }

            renderTransform.Children.Add(transform);
        }

        private Storyboard CreateTransitionStoryboard(FrameworkElement child, bool usingBeforeLoaded,
            ref Rect layoutRect, ref Rect currentRect)
        {
            var duration = Duration;
            var storyboard = new Storyboard
            {
                Duration = duration
            };
            var num = !usingBeforeLoaded || Math.Abs(layoutRect.Width) < 0.001
                ? 1.0
                : currentRect.Width / layoutRect.Width;
            var num2 = !usingBeforeLoaded || Math.Abs(layoutRect.Height) < 0.001
                ? 1.0
                : currentRect.Height / layoutRect.Height;
            var num3 = currentRect.Left - layoutRect.Left;
            var num4 = currentRect.Top - layoutRect.Top;
            var group = new TransformGroup();
            var transform = new ScaleTransform
            {
                ScaleX = num,
                ScaleY = num2
            };
            group.Children.Add(transform);
            var transform2 = new TranslateTransform
            {
                X = num3,
                Y = num4
            };
            group.Children.Add(transform2);
            AddTransform(child, group);
            var str = "(FrameworkElement.RenderTransform).";
            if (child.RenderTransform is TransformGroup renderTransform && GetHasTransformWrapper(child))
            {
                object obj2 = str;
                str = string.Concat(obj2, "(TransformGroup.Children)[", renderTransform.Children.Count - 1,
                    "].");
            }

            if (usingBeforeLoaded)
            {
                if (Math.Abs(num - 1.0) > 0.001)
                {
                    var element = new DoubleAnimation
                    {
                        Duration = duration,
                        From = num,
                        To = 1.0
                    };
                    Storyboard.SetTarget(element, child);
                    Storyboard.SetTargetProperty(element,
                        new PropertyPath(str + "(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
                    element.EasingFunction = EaseX;
                    storyboard.Children.Add(element);
                }

                if (Math.Abs(num2 - 1.0) > 0.001)
                {
                    var animation3 = new DoubleAnimation
                    {
                        Duration = duration,
                        From = num2,
                        To = 1.0
                    };
                    Storyboard.SetTarget(animation3, child);
                    Storyboard.SetTargetProperty(animation3,
                        new PropertyPath(str + "(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
                    animation3.EasingFunction = EaseY;
                    storyboard.Children.Add(animation3);
                }
            }

            if (Math.Abs(num3) > 0.001)
            {
                var animation5 = new DoubleAnimation
                {
                    Duration = duration,
                    From = num3,
                    To = 0.0
                };
                Storyboard.SetTarget(animation5, child);
                Storyboard.SetTargetProperty(animation5,
                    new PropertyPath(str + "(TransformGroup.Children)[1].(TranslateTransform.X)"));
                animation5.EasingFunction = EaseX;
                storyboard.Children.Add(animation5);
            }

            if (Math.Abs(num4) > 0.001)
            {
                var animation7 = new DoubleAnimation
                {
                    Duration = duration,
                    From = num4,
                    To = 0.0
                };
                Storyboard.SetTarget(animation7, child);
                Storyboard.SetTargetProperty(animation7,
                    new PropertyPath(str + "(TransformGroup.Children)[1].(TranslateTransform.Y)"));
                animation7.EasingFunction = EaseY;
                storyboard.Children.Add(animation7);
            }

            return storyboard;
        }

        protected override void EnsureTags(FrameworkElement child)
        {
            base.EnsureTags(child);
            if (InitialTag == TagType.DataContext &&
                !(child.ReadLocalValue(InitialIdentityTagProperty) is BindingExpression))
                child.SetBinding(InitialIdentityTagProperty, new Binding(InitialTagPath));
        }

        private static bool GetHasTransformWrapper(DependencyObject obj)
        {
            return (bool) obj.GetValue(HasTransformWrapperProperty);
        }

        private static object GetInitialIdentityTag(DependencyObject obj)
        {
            return obj.GetValue(InitialIdentityTagProperty);
        }

        private static object GetOverlay(DependencyObject obj)
        {
            return obj.GetValue(OverlayProperty);
        }

        private static Transform GetTransform(FrameworkElement child)
        {
            if (child.RenderTransform is TransformGroup renderTransform && renderTransform.Children.Count > 0)
                return renderTransform.Children[renderTransform.Children.Count - 1];
            return new TranslateTransform();
        }

        private static bool IsClose(double a, double b)
        {
            return Math.Abs(a - b) < 1E-07;
        }

        private static bool IsEmptyRect(Rect rect)
        {
            if (!rect.IsEmpty && !double.IsNaN(rect.Left)) return double.IsNaN(rect.Top);
            return true;
        }

        private static void RemoveTransform(FrameworkElement child)
        {
            if (child.RenderTransform is TransformGroup renderTransform)
            {
                if (GetHasTransformWrapper(child))
                {
                    child.RenderTransform = renderTransform.Children[0];
                    SetHasTransformWrapper(child, false);
                }
                else
                {
                    renderTransform.Children.RemoveAt(renderTransform.Children.Count - 1);
                }
            }
        }

        private static void SetHasTransformWrapper(DependencyObject obj, bool value)
        {
            obj.SetValue(HasTransformWrapperProperty, value);
        }

        private static void SetOverlay(DependencyObject obj, object value)
        {
            obj.SetValue(OverlayProperty, value);
        }

        private static void TransferLocalValue(FrameworkElement element, DependencyProperty source,
            DependencyProperty dest)
        {
            var obj2 = element.ReadLocalValue(source);
            if (obj2 is BindingExpressionBase base2)
                element.SetBinding(dest, base2.ParentBindingBase);
            else if (obj2 == DependencyProperty.UnsetValue)
                element.ClearValue(dest);
            else
                element.SetValue(dest, element.GetAnimationBaseValue(source));
            element.ClearValue(source);
        }

        internal override void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root,
            object tag, TagData newTagData)
        {
            Rect empty;
            var flag = false;
            var usingBeforeLoaded = false;
            var initialIdentityTag = GetInitialIdentityTag(child);
            var flag3 = TagDictionary.TryGetValue(tag, out var data);
            if (flag3 && data.InitialTag != initialIdentityTag)
            {
                flag3 = false;
                TagDictionary.Remove(tag);
            }

            if (!flag3)
            {
                if (initialIdentityTag != null && TagDictionary.TryGetValue(initialIdentityTag, out var data2))
                {
                    empty = TranslateRect(data2.AppRect, root, newTagData.Parent);
                    flag = true;
                    usingBeforeLoaded = true;
                }
                else
                {
                    empty = Rect.Empty;
                }

                data = new TagData
                {
                    ParentRect = Rect.Empty,
                    AppRect = Rect.Empty,
                    Parent = newTagData.Parent,
                    Child = child,
                    Timestamp = DateTime.Now,
                    InitialTag = initialIdentityTag
                };
                TagDictionary.Add(tag, data);
            }
            else if (!Equals(data.Parent, VisualTreeHelper.GetParent(child)))
            {
                empty = TranslateRect(data.AppRect, root, newTagData.Parent);
                flag = true;
            }
            else
            {
                empty = data.ParentRect;
            }

            var originalChild = child;
            if (!IsEmptyRect(empty) && !IsEmptyRect(newTagData.ParentRect) &&
                (!IsClose(empty.Left, newTagData.ParentRect.Left) ||
                 !IsClose(empty.Top, newTagData.ParentRect.Top)) || !Equals(child, data.Child) &&
                TransitionStoryboardDictionary.ContainsKey(tag))
            {
                var rect = empty;
                var flag4 = false;
                if (TransitionStoryboardDictionary.TryGetValue(tag, out var storyboard))
                {
                    var obj3 = GetOverlay(data.Child);
                    var adorner = (AdornerContainer) obj3;
                    flag4 = obj3 != null;
                    var element = data.Child;
                    if (obj3 != null)
                    {
                        if (adorner.Child is Canvas canvas) element = canvas.Children[0] as FrameworkElement;
                    }

                    if (!usingBeforeLoaded) rect = GetTransform(element).TransformBounds(rect);
                    TransitionStoryboardDictionary.Remove(tag);
                    storyboard.Stop();
                    RemoveTransform(element);
                    if (obj3 != null)
                    {
                        AdornerLayer.GetAdornerLayer(root).Remove(adorner);
                        TransferLocalValue(data.Child, CacheDuringOverlayProperty,
                            UIElement.RenderTransformProperty);
                        SetOverlay(data.Child, null);
                    }
                }

                object overlay = null;
                if (flag4 || flag && FloatAbove)
                {
                    var canvas2 = new Canvas
                    {
                        Width = newTagData.ParentRect.Width,
                        Height = newTagData.ParentRect.Height,
                        IsHitTestVisible = false
                    };
                    var rectangle = new Rectangle
                    {
                        Width = newTagData.ParentRect.Width,
                        Height = newTagData.ParentRect.Height,
                        IsHitTestVisible = false,
                        Fill = new VisualBrush(child)
                    };
                    canvas2.Children.Add(rectangle);
                    var container2 = new AdornerContainer(child)
                    {
                        Child = canvas2
                    };
                    overlay = container2;
                    SetOverlay(originalChild, overlay);
                    AdornerLayer.GetAdornerLayer(root).Add(container2);
                    TransferLocalValue(child, UIElement.RenderTransformProperty, CacheDuringOverlayProperty);
                    child.RenderTransform = new TranslateTransform(-10000.0, -10000.0);
                    canvas2.RenderTransform = new TranslateTransform(10000.0, 10000.0);
                    child = rectangle;
                }

                var parentRect = newTagData.ParentRect;
                var transitionStoryboard =
                    CreateTransitionStoryboard(child, usingBeforeLoaded, ref parentRect, ref rect);
                TransitionStoryboardDictionary.Add(tag, transitionStoryboard);
                transitionStoryboard.Completed += delegate
                {
                    if (TransitionStoryboardDictionary.TryGetValue(tag, out var storyboard1) &&
                        Equals(storyboard1, transitionStoryboard))
                    {
                        TransitionStoryboardDictionary.Remove(tag);
                        transitionStoryboard.Stop();
                        RemoveTransform(child);
                        child.InvalidateMeasure();
                        if (overlay != null)
                        {
                            AdornerLayer.GetAdornerLayer(root).Remove((AdornerContainer) overlay);
                            TransferLocalValue(originalChild, CacheDuringOverlayProperty,
                                UIElement.RenderTransformProperty);
                            SetOverlay(originalChild, null);
                        }
                    }
                };
                transitionStoryboard.Begin();
            }

            data.ParentRect = newTagData.ParentRect;
            data.AppRect = newTagData.AppRect;
            data.Parent = newTagData.Parent;
            data.Child = newTagData.Child;
            data.Timestamp = newTagData.Timestamp;
        }
    }
}