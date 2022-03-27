using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace HandyControl.Interactivity;

[ContentProperty("Actions")]
public abstract class TriggerBase : Animatable, IAttachedObject
{
    private static readonly DependencyPropertyKey ActionsPropertyKey =
        DependencyProperty.RegisterReadOnly("Actions", typeof(TriggerActionCollection), typeof(TriggerBase),
            new FrameworkPropertyMetadata());

    public static readonly DependencyProperty ActionsProperty = ActionsPropertyKey.DependencyProperty;

    private DependencyObject _associatedObject;
    private readonly Type _associatedObjectTypeConstraint;

    internal TriggerBase(Type associatedObjectTypeConstraint)
    {
        _associatedObjectTypeConstraint = associatedObjectTypeConstraint;
        var actions = new TriggerActionCollection();
        SetValue(ActionsPropertyKey, actions);
    }

    public TriggerActionCollection Actions =>
        (TriggerActionCollection) GetValue(ActionsProperty);

    protected DependencyObject AssociatedObject
    {
        get
        {
            ReadPreamble();
            return _associatedObject;
        }
    }

    protected virtual Type AssociatedObjectTypeConstraint
    {
        get
        {
            ReadPreamble();
            return _associatedObjectTypeConstraint;
        }
    }

    public void Attach(DependencyObject dependencyObject)
    {
        if (!Equals(dependencyObject, AssociatedObject))
        {
            if (AssociatedObject != null)
                throw new InvalidOperationException(ExceptionStringTable
                    .CannotHostTriggerMultipleTimesExceptionMessage);
            if (dependencyObject != null &&
                !AssociatedObjectTypeConstraint.IsInstanceOfType(dependencyObject))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    ExceptionStringTable.TypeConstraintViolatedExceptionMessage,
                    new object[]
                        {GetType().Name, dependencyObject.GetType().Name, AssociatedObjectTypeConstraint.Name}));
            WritePreamble();
            _associatedObject = dependencyObject;
            WritePostscript();
            Actions.Attach(dependencyObject);
            OnAttached();
        }
    }

    public void Detach()
    {
        OnDetaching();
        WritePreamble();
        _associatedObject = null;
        WritePostscript();
        Actions.Detach();
    }

    DependencyObject IAttachedObject.AssociatedObject =>
        AssociatedObject;

    public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

    protected override Freezable CreateInstanceCore()
    {
        return (Freezable) Activator.CreateInstance(GetType());
    }

    protected void InvokeActions(object parameter)
    {
        if (PreviewInvoke != null)
        {
            var e = new PreviewInvokeEventArgs();
            PreviewInvoke(this, e);
            if (e.Cancelling)
                return;
        }
        foreach (var action in Actions)
            action.CallInvoke(parameter);
    }

    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }
}
