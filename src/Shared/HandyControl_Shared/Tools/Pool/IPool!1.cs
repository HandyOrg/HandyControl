namespace HandyControl.Tools
{
    public interface IPool<T>
    {
        T Acquire();

        bool Release(T instance);
    }
}
