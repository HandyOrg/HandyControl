using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace HandyControl.Interactivity;

public abstract class EventTriggerBase : TriggerBase
{
    public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register("SourceName",
        typeof(string), typeof(EventTriggerBase), new PropertyMetadata(OnSourceNameChanged));

    public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register("SourceObject",
        typeof(object), typeof(EventTriggerBase), new PropertyMetadata(OnSourceObjectChanged));

    private MethodInfo _eventHandlerMethodInfo;

    internal EventTriggerBase(Type sourceTypeConstraint) : base(typeof(DependencyObject))
    {
        SourceTypeConstraint = sourceTypeConstraint;
        SourceNameResolver = new NameResolver();
        RegisterSourceChanged();
    }

    protected sealed override Type AssociatedObjectTypeConstraint
    {
        get
        {
            if (TypeDescriptor.GetAttributes(GetType())[typeof(TypeConstraintAttribute)] is TypeConstraintAttribute attribute)
                return attribute.Constraint;
            return typeof(DependencyObject);
        }
    }

    private bool IsLoadedRegistered { get; set; }

    private bool IsSourceChangedRegistered { get; set; }

    private bool IsSourceNameSet
    {
        get
        {
            if (string.IsNullOrEmpty(SourceName))
                return ReadLocalValue(SourceNameProperty) != DependencyProperty.UnsetValue;
            return true;
        }
    }

    public object Source
    {
        get
        {
            object associatedObject = AssociatedObject;
            if (SourceObject != null)
                return SourceObject;
            if (IsSourceNameSet)
            {
                associatedObject = SourceNameResolver.Object;
                if (associatedObject != null && !SourceTypeConstraint.IsInstanceOfType(associatedObject))
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                        ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage, GetType().Name,
                        associatedObject.GetType(), SourceTypeConstraint, "Source"));
            }
            return associatedObject;
        }
    }

    public string SourceName
    {
        get =>
            (string) GetValue(SourceNameProperty);
        set => SetValue(SourceNameProperty, value);
    }

    private NameResolver SourceNameResolver { get; }

    public object SourceObject
    {
        get =>
            GetValue(SourceObjectProperty);
        set => SetValue(SourceObjectProperty, value);
    }

    protected Type SourceTypeConstraint { get; }

    protected abstract string GetEventName();

    private static bool IsValidEvent(EventInfo eventInfo)
    {
        var eventHandlerType = eventInfo.EventHandlerType;
        if (!typeof(Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
            return false;
        var parameters = eventHandlerType.GetMethod("Invoke")?.GetParameters();
        return parameters != null && parameters.Length == 2 && typeof(object).IsAssignableFrom(parameters[0].ParameterType) && typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        var associatedObject = AssociatedObject;
        var behavior = associatedObject as Behavior;
        var element = associatedObject as FrameworkElement;
        RegisterSourceChanged();
        if (behavior != null)
        {
            behavior.AssociatedObjectChanged += OnBehaviorHostChanged;
        }
        else if (SourceObject != null || element == null)
        {
            try
            {
                OnSourceChanged(null, Source);
            }
            catch (InvalidOperationException)
            {
            }
        }
        else
        {
            SourceNameResolver.NameScopeReferenceElement = element;
        }
        if (string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) == 0 && element != null &&
            !Interaction.IsElementLoaded(element))
            RegisterLoaded(element);
    }

    private void OnBehaviorHostChanged(object sender, EventArgs e)
    {
        SourceNameResolver.NameScopeReferenceElement =
            ((IAttachedObject) sender).AssociatedObject as FrameworkElement;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        var associatedObject = AssociatedObject as Behavior;
        var associatedElement = AssociatedObject as FrameworkElement;
        try
        {
            OnSourceChanged(Source, null);
        }
        catch (InvalidOperationException)
        {
        }
        UnregisterSourceChanged();
        if (associatedObject != null)
            associatedObject.AssociatedObjectChanged -= OnBehaviorHostChanged;
        SourceNameResolver.NameScopeReferenceElement = null;
        if (string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) == 0 && associatedElement != null)
            UnregisterLoaded(associatedElement);
    }

    protected virtual void OnEvent(EventArgs eventArgs)
    {
        InvokeActions(eventArgs);
    }

    private void OnEventImpl(object sender, EventArgs eventArgs)
    {
        OnEvent(eventArgs);
    }

    internal void OnEventNameChanged(string oldEventName, string newEventName)
    {
        if (AssociatedObject != null)
        {
            var source = Source as FrameworkElement;
            if (source != null && string.Compare(oldEventName, "Loaded", StringComparison.Ordinal) == 0)
                UnregisterLoaded(source);
            else if (!string.IsNullOrEmpty(oldEventName))
                UnregisterEvent(Source, oldEventName);
            if (source != null && string.Compare(newEventName, "Loaded", StringComparison.Ordinal) == 0)
                RegisterLoaded(source);
            else if (!string.IsNullOrEmpty(newEventName))
                RegisterEvent(Source, newEventName);
        }
    }

    private void OnSourceChanged(object oldSource, object newSource)
    {
        if (AssociatedObject != null)
            OnSourceChangedImpl(oldSource, newSource);
    }

    internal virtual void OnSourceChangedImpl(object oldSource, object newSource)
    {
        if (!string.IsNullOrEmpty(GetEventName()) &&
            string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) != 0)
        {
            if (oldSource != null && SourceTypeConstraint.IsInstanceOfType(oldSource))
                UnregisterEvent(oldSource, GetEventName());
            if (newSource != null && SourceTypeConstraint.IsInstanceOfType(newSource))
                RegisterEvent(newSource, GetEventName());
        }
    }

    private static void OnSourceNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var base2 = (EventTriggerBase) obj;
        base2.SourceNameResolver.Name = (string) args.NewValue;
    }

    private void OnSourceNameResolverElementChanged(object sender, NameResolvedEventArgs e)
    {
        if (SourceObject == null)
            OnSourceChanged(e.OldObject, e.NewObject);
    }

    private static void OnSourceObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var base2 = (EventTriggerBase) obj;
        object newSource = base2.SourceNameResolver.Object;
        if (args.NewValue == null)
        {
            base2.OnSourceChanged(args.OldValue, newSource);
        }
        else
        {
            if (args.OldValue == null && newSource != null)
                base2.UnregisterEvent(newSource, base2.GetEventName());
            base2.OnSourceChanged(args.OldValue, args.NewValue);
        }
    }

    private void RegisterEvent(object obj, string eventName)
    {
        var eventInfo = obj.GetType().GetEvent(eventName);
        if (eventInfo == null)
        {
            if (SourceObject != null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    ExceptionStringTable.EventTriggerCannotFindEventNameExceptionMessage,
                    new object[] { eventName, obj.GetType().Name }));
        }
        else if (!IsValidEvent(eventInfo))
        {
            if (SourceObject != null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    ExceptionStringTable.EventTriggerBaseInvalidEventExceptionMessage,
                    new object[] { eventName, obj.GetType().Name }));
        }
        else
        {
            _eventHandlerMethodInfo =
                typeof(EventTriggerBase).GetMethod("OnEventImpl", BindingFlags.NonPublic | BindingFlags.Instance);
            eventInfo.AddEventHandler(obj,
                Delegate.CreateDelegate(eventInfo.EventHandlerType, this, _eventHandlerMethodInfo ?? throw new InvalidOperationException()));
        }
    }

    private void RegisterLoaded(FrameworkElement associatedElement)
    {
        if (!IsLoadedRegistered && associatedElement != null)
        {
            associatedElement.Loaded += OnEventImpl;
            IsLoadedRegistered = true;
        }
    }

    private void RegisterSourceChanged()
    {
        if (!IsSourceChangedRegistered)
        {
            SourceNameResolver.ResolvedElementChanged += OnSourceNameResolverElementChanged;
            IsSourceChangedRegistered = true;
        }
    }

    private void UnregisterEvent(object obj, string eventName)
    {
        if (string.Compare(eventName, "Loaded", StringComparison.Ordinal) == 0)
        {
            if (obj is FrameworkElement associatedElement)
                UnregisterLoaded(associatedElement);
        }
        else
        {
            UnregisterEventImpl(obj, eventName);
        }
    }

    private void UnregisterEventImpl(object obj, string eventName)
    {
        var type = obj.GetType();
        if (_eventHandlerMethodInfo != null)
        {
            var info = type.GetEvent(eventName);
            info.RemoveEventHandler(obj,
                Delegate.CreateDelegate(info.EventHandlerType, this, _eventHandlerMethodInfo));
            _eventHandlerMethodInfo = null;
        }
    }

    private void UnregisterLoaded(FrameworkElement associatedElement)
    {
        if (IsLoadedRegistered && associatedElement != null)
        {
            associatedElement.Loaded -= OnEventImpl;
            IsLoadedRegistered = false;
        }
    }

    private void UnregisterSourceChanged()
    {
        if (IsSourceChangedRegistered)
        {
            SourceNameResolver.ResolvedElementChanged -= OnSourceNameResolverElementChanged;
            IsSourceChangedRegistered = false;
        }
    }
}
