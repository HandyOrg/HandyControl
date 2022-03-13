using System;
using System.Windows;

namespace HandyControl.Interactivity;

public class TriggerActionCollection : AttachableCollection<TriggerAction>
{
    internal TriggerActionCollection()
    {
    }

    protected override Freezable CreateInstanceCore()
    {
        return new TriggerActionCollection();
    }

    internal override void ItemAdded(TriggerAction item)
    {
        if (item.IsHosted)
            throw new InvalidOperationException(ExceptionStringTable
                .CannotHostTriggerActionMultipleTimesExceptionMessage);
        if (AssociatedObject != null)
            item.Attach(AssociatedObject);
        item.IsHosted = true;
    }

    internal override void ItemRemoved(TriggerAction item)
    {
        if (((IAttachedObject) item).AssociatedObject != null)
            item.Detach();
        item.IsHosted = false;
    }

    protected override void OnAttached()
    {
        foreach (var action in this)
            action.Attach(AssociatedObject);
    }

    protected override void OnDetaching()
    {
        foreach (var action in this)
            action.Detach();
    }
}
