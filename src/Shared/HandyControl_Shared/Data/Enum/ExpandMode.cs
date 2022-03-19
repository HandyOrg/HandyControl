namespace HandyControl.Data;

public enum ExpandMode
{
    /// <summary>
    ///     最多只能显示一项，且不可折叠
    /// </summary>
    ShowOne,

    /// <summary>
    ///     显示所有项，且不可折叠
    /// </summary>
    ShowAll,

    /// <summary>
    ///     类似ShowOne，但是控件的尺寸不随项的数量而改变
    /// </summary>
    Accordion,

    /// <summary>
    ///     没有任何限制
    /// </summary>
    Freedom
}
