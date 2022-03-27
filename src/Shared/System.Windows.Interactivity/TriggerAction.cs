using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using HandyControl.Data;

namespace HandyControl.Interactivity;

[DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "MouseLeftButtonDown")]
[DefaultTrigger(typeof(ButtonBase), typeof(EventTrigger), "Click")]
public abstract class TriggerAction : Animatable, IAttachedObject
{
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled",
        typeof(bool), typeof(TriggerAction), new FrameworkPropertyMetadata(ValueBoxes.TrueBox));

    private readonly Type _associatedObjectTypeConstraint;

    private DependencyObject _associatedObject;
    private bool _isHosted;

    internal TriggerAction(Type associatedObjectTypeConstraint)
    {
        _associatedObjectTypeConstraint = associatedObjectTypeConstraint;
    }

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

    public bool IsEnabled
    {
        get => (bool) GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, ValueBoxes.BooleanBox(value));
    }

    internal bool IsHosted
    {
        get
        {
            ReadPreamble();
            return _isHosted;
        }
        set
        {
            WritePreamble();
            _isHosted = value;
            WritePostscript();
        }
    }

    public void Attach(DependencyObject dependencyObject)
    {
        if (!Equals(dependencyObject, AssociatedObject))
        {
            if (AssociatedObject != null)
                throw new InvalidOperationException(ExceptionStringTable
                    .CannotHostTriggerActionMultipleTimesExceptionMessage);
            if (dependencyObject != null &&
                !AssociatedObjectTypeConstraint.IsInstanceOfType(dependencyObject))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    ExceptionStringTable.TypeConstraintViolatedExceptionMessage,
                    new object[]
                        {GetType().Name, dependencyObject.GetType().Name, AssociatedObjectTypeConstraint.Name}));
            WritePreamble();
            _associatedObject = dependencyObject;
            WritePostscript();
            OnAttached();
        }
    }

    public void Detach()
    {
        OnDetaching();
        WritePreamble();
        _associatedObject = null;
        WritePostscript();
    }

    DependencyObject IAttachedObject.AssociatedObject =>
        AssociatedObject;

    internal void CallInvoke(object parameter)
    {
        if (IsEnabled)
            Invoke(parameter);
    }

    protected override Freezable CreateInstanceCore()
    {
        return (Freezable) Activator.CreateInstance(GetType());
    }

    protected abstract void Invoke(object parameter);

    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }
}
