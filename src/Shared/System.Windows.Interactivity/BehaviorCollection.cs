using System.Windows;

namespace HandyControl.Interactivity;

public sealed class BehaviorCollection : AttachableCollection<Behavior>
{
    internal BehaviorCollection()
    {
    }

    protected override void OnAttached()
    {
        foreach (var behavior in this)
            behavior.Attach(AssociatedObject);
    }

    protected override void OnDetaching()
    {
        foreach (var behavior in this)
            behavior.Detach();
    }

    internal override void ItemAdded(Behavior item)
    {
        if (item == null || AssociatedObject == null)
            return;
        item.Attach(AssociatedObject);
    }

    internal override void ItemRemoved(Behavior item)
    {
        if (((IAttachedObject) item)?.AssociatedObject == null)
            return;
        item.Detach();
    }

    protected override Freezable CreateInstanceCore()
    {
        return new BehaviorCollection();
    }
}
