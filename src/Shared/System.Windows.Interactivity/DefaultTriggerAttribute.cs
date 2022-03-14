using System;
using System.Collections;
using System.Globalization;

namespace HandyControl.Interactivity;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
public sealed class DefaultTriggerAttribute : Attribute
{
    private readonly object[] _parameters;

    public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter) : this(targetType,
        triggerType, new[] { parameter })
    {
    }

    public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
    {
        if (!typeof(TriggerBase).IsAssignableFrom(triggerType))
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ExceptionStringTable.DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage,
                new object[] { triggerType.Name }));
        TargetType = targetType;
        TriggerType = triggerType;
        _parameters = parameters;
    }

    public IEnumerable Parameters =>
        _parameters;

    public Type TargetType { get; }

    public Type TriggerType { get; }

    public TriggerBase Instantiate()
    {
        object obj2 = null;
        try
        {
            obj2 = Activator.CreateInstance(TriggerType, _parameters);
        }
        catch
        {
            // ignored
        }
        return (TriggerBase) obj2;
    }
}
