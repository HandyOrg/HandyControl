using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Interactivity;

public abstract class FluidMoveBehaviorBase : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty AppliesToProperty = DependencyProperty.Register("AppliesTo",
        typeof(FluidMoveScope), typeof(FluidMoveBehaviorBase), new PropertyMetadata(FluidMoveScope.Self));

    protected static readonly DependencyProperty IdentityTagProperty =
        DependencyProperty.RegisterAttached("IdentityTag", typeof(object), typeof(FluidMoveBehaviorBase),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive",
        typeof(bool), typeof(FluidMoveBehaviorBase), new PropertyMetadata(ValueBoxes.TrueBox));

    private static DateTime LastPurgeTick = DateTime.MinValue;

    private static readonly TimeSpan MinTickDelta = TimeSpan.FromSeconds(0.5);

    private static DateTime NextToLastPurgeTick = DateTime.MinValue;

    internal static Dictionary<object, TagData> TagDictionary = new();

    public static readonly DependencyProperty TagPathProperty = DependencyProperty.Register("TagPath",
        typeof(string), typeof(FluidMoveBehaviorBase), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TagProperty = DependencyProperty.Register("Tag",
        typeof(TagType), typeof(FluidMoveBehaviorBase), new PropertyMetadata(TagType.Element));

    public FluidMoveScope AppliesTo
    {
        get =>
            (FluidMoveScope) GetValue(AppliesToProperty);
        set => SetValue(AppliesToProperty, value);
    }

    public bool IsActive
    {
        get =>
            (bool) GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, ValueBoxes.BooleanBox(value));
    }

    protected virtual bool ShouldSkipInitialLayout =>
        Tag == TagType.DataContext;

    public TagType Tag
    {
        get =>
            (TagType) GetValue(TagProperty);
        set => SetValue(TagProperty, value);
    }

    public string TagPath
    {
        get =>
            (string) GetValue(TagPathProperty);
        set => SetValue(TagPathProperty, value);
    }

    private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
    {
        if (IsActive)
        {
            if (DateTime.Now - LastPurgeTick >= MinTickDelta)
            {
                List<object> list = null;
                foreach (var pair in TagDictionary)
                    if (pair.Value.Timestamp < NextToLastPurgeTick)
                    {
                        if (list == null) list = new List<object>();
                        list.Add(pair.Key);
                    }

                if (list != null)
                    foreach (var obj2 in list)
                        TagDictionary.Remove(obj2);
                NextToLastPurgeTick = LastPurgeTick;
                LastPurgeTick = DateTime.Now;
            }

            if (AppliesTo == FluidMoveScope.Self)
            {
                UpdateLayoutTransition(AssociatedObject);
            }
            else
            {
                if (AssociatedObject is Panel associatedObject)
                    foreach (FrameworkElement element in associatedObject.Children)
                        UpdateLayoutTransition(element);
            }
        }
    }

    protected virtual void EnsureTags(FrameworkElement child)
    {
        if (Tag == TagType.DataContext &&
            !(child.ReadLocalValue(IdentityTagProperty) is BindingExpression))
            child.SetBinding(IdentityTagProperty, new Binding(TagPath));
    }

    protected static object GetIdentityTag(DependencyObject obj)
    {
        return obj.GetValue(IdentityTagProperty);
    }

    private static FrameworkElement GetVisualRoot(FrameworkElement child)
    {
        while (true)
        {
            if (!(VisualTreeHelper.GetParent(child) is FrameworkElement parent)) return child;
            if (AdornerLayer.GetAdornerLayer(parent) == null) return child;
            child = parent;
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
    }

    protected static void SetIdentityTag(DependencyObject obj, object value)
    {
        obj.SetValue(IdentityTagProperty, value);
    }

    internal static Rect TranslateRect(Rect rect, FrameworkElement from, FrameworkElement to)
    {
        if (from == null || to == null) return rect;
        var point = new Point(rect.Left, rect.Top);
        point = from.TransformToVisual(to).Transform(point);
        return new Rect(point.X, point.Y, rect.Width, rect.Height);
    }

    private void UpdateLayoutTransition(FrameworkElement child)
    {
        if (child.Visibility != Visibility.Collapsed && child.IsLoaded || !ShouldSkipInitialLayout)
        {
            var visualRoot = GetVisualRoot(child);
            var newTagData = new TagData
            {
                Parent = VisualTreeHelper.GetParent(child) as FrameworkElement,
                ParentRect = ExtendedVisualStateManager.GetLayoutRect(child),
                Child = child,
                Timestamp = DateTime.Now
            };
            try
            {
                newTagData.AppRect = TranslateRect(newTagData.ParentRect, newTagData.Parent, visualRoot);
            }
            catch (ArgumentException)
            {
                if (ShouldSkipInitialLayout) return;
            }

            EnsureTags(child);
            var identityTag = GetIdentityTag(child) ?? child;
            UpdateLayoutTransitionCore(child, visualRoot, identityTag, newTagData);
        }
    }

    internal abstract void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root,
        object tag, TagData newTagData);


    internal class TagData
    {
        public Rect AppRect { get; set; }

        public FrameworkElement Child { get; set; }

        public object InitialTag { get; set; }

        public FrameworkElement Parent { get; set; }

        public Rect ParentRect { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
