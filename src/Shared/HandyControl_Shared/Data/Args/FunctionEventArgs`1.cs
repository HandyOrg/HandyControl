using System.Windows;


namespace HandyControl.Data;

public class FunctionEventArgs<T> : RoutedEventArgs
{
    public FunctionEventArgs(T info)
    {
        Info = info;
    }

    public FunctionEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
    {
    }

    public T Info { get; set; }
}
