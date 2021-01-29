namespace HandyControl.ThemeManager
{
    /// <summary>
    /// Represents a method that handles general events.
    /// </summary>
    /// <typeparam name="TSender"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="sender">The event source.</param>
    /// <param name="args">The event data. If there is no event data, this parameter will be null.</param>
    public delegate void TypedEventHandler<TSender, TResult>(TSender sender, TResult args);
}
