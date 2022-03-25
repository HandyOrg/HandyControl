namespace HandyControl.Interactivity;

public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
{
    protected EventTriggerBase() : base(typeof(T))
    {
    }

    protected virtual void OnSourceChanged(T oldSource, T newSource)
    {
    }

    internal sealed override void OnSourceChangedImpl(object oldSource, object newSource)
    {
        base.OnSourceChangedImpl(oldSource, newSource);
        OnSourceChanged(oldSource as T, newSource as T);
    }

    public new T Source => (T) base.Source;
}
