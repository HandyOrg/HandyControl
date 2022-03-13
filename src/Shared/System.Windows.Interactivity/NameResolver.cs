using System;
using System.Windows;

namespace HandyControl.Interactivity;

internal sealed class NameResolver
{
    private string _name;
    private FrameworkElement _nameScopeReferenceElement;

    private FrameworkElement ActualNameScopeReferenceElement
    {
        get
        {
            if (NameScopeReferenceElement != null && Interaction.IsElementLoaded(NameScopeReferenceElement))
                return GetActualNameScopeReference(NameScopeReferenceElement);
            return null;
        }
    }

    private bool HasAttempedResolve { get; set; }

    public string Name
    {
        get =>
            _name;
        set
        {
            var oldObject = Object;
            _name = value;
            UpdateObjectFromName(oldObject);
        }
    }

    public FrameworkElement NameScopeReferenceElement
    {
        get =>
            _nameScopeReferenceElement;
        set
        {
            var nameScopeReferenceElement = NameScopeReferenceElement;
            _nameScopeReferenceElement = value;
            OnNameScopeReferenceElementChanged(nameScopeReferenceElement);
        }
    }

    public DependencyObject Object
    {
        get
        {
            if (string.IsNullOrEmpty(Name) && HasAttempedResolve)
                return NameScopeReferenceElement;
            return ResolvedObject;
        }
    }

    private bool PendingReferenceElementLoad { get; set; }

    private DependencyObject ResolvedObject { get; set; }

    public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;

    private FrameworkElement GetActualNameScopeReference(FrameworkElement initialReferenceElement)
    {
        var element = initialReferenceElement;
        if (!IsNameScope(initialReferenceElement))
            return element;
        return initialReferenceElement.Parent as FrameworkElement ?? element;
    }

    private bool IsNameScope(FrameworkElement frameworkElement)
    {
        return frameworkElement.Parent is FrameworkElement parent && parent.FindName(Name) != null;
    }

    private void OnNameScopeReferenceElementChanged(FrameworkElement oldNameScopeReference)
    {
        if (PendingReferenceElementLoad)
        {
            oldNameScopeReference.Loaded -= OnNameScopeReferenceLoaded;
            PendingReferenceElementLoad = false;
        }
        HasAttempedResolve = false;
        UpdateObjectFromName(Object);
    }

    private void OnNameScopeReferenceLoaded(object sender, RoutedEventArgs e)
    {
        PendingReferenceElementLoad = false;
        NameScopeReferenceElement.Loaded -= OnNameScopeReferenceLoaded;
        UpdateObjectFromName(Object);
    }

    private void OnObjectChanged(DependencyObject oldTarget, DependencyObject newTarget)
    {
        ResolvedElementChanged?.Invoke(this, new NameResolvedEventArgs(oldTarget, newTarget));
    }

    private void UpdateObjectFromName(DependencyObject oldObject)
    {
        DependencyObject obj2 = null;
        ResolvedObject = null;
        if (NameScopeReferenceElement != null)
        {
            if (!Interaction.IsElementLoaded(NameScopeReferenceElement))
            {
                NameScopeReferenceElement.Loaded += OnNameScopeReferenceLoaded;
                PendingReferenceElementLoad = true;
                return;
            }
            if (!string.IsNullOrEmpty(Name))
            {
                var actualNameScopeReferenceElement = ActualNameScopeReferenceElement;
                if (actualNameScopeReferenceElement != null)
                    obj2 = actualNameScopeReferenceElement.FindName(Name) as DependencyObject;
            }
        }
        HasAttempedResolve = true;
        ResolvedObject = obj2;
        if (!Equals(oldObject, Object))
            OnObjectChanged(oldObject, Object);
    }
}
