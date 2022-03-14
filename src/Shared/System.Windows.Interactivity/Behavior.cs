using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Animation;

namespace HandyControl.Interactivity;

public abstract class Behavior : Animatable, IAttachedObject
{
    private DependencyObject _associatedObject;
    private readonly Type _associatedType;

    internal Behavior(Type associatedType)
    {
        _associatedType = associatedType;
    }

    protected DependencyObject AssociatedObject
    {
        get
        {
            ReadPreamble();
            return _associatedObject;
        }
    }

    protected Type AssociatedType
    {
        get
        {
            ReadPreamble();
            return _associatedType;
        }
    }

    public void Attach(DependencyObject dependencyObject)
    {
        if (!Equals(dependencyObject, AssociatedObject))
        {
            if (AssociatedObject != null)
                throw new InvalidOperationException(ExceptionStringTable
                    .CannotHostBehaviorMultipleTimesExceptionMessage);
            if (dependencyObject != null && !AssociatedType.IsInstanceOfType(dependencyObject))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    ExceptionStringTable.TypeConstraintViolatedExceptionMessage,
                    new object[] { GetType().Name, dependencyObject.GetType().Name, AssociatedType.Name }));
            WritePreamble();
            _associatedObject = dependencyObject;
            WritePostscript();
            OnAssociatedObjectChanged();
            OnAttached();
        }
    }

    public void Detach()
    {
        OnDetaching();
        WritePreamble();
        _associatedObject = null;
        WritePostscript();
        OnAssociatedObjectChanged();
    }

    DependencyObject IAttachedObject.AssociatedObject =>
        AssociatedObject;

    internal event EventHandler AssociatedObjectChanged;

    protected override Freezable CreateInstanceCore()
    {
        return (Freezable) Activator.CreateInstance(GetType());
    }

    private void OnAssociatedObjectChanged()
    {
        AssociatedObjectChanged?.Invoke(this, new EventArgs());
    }

    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }
}
