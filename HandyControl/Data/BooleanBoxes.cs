namespace HandyControl.Data
{
    /// <summary>
    ///     装箱后的布尔值（用于提高效率）
    /// </summary>
    internal static class BooleanBoxes
    {
        internal static object TrueBox = true;
        internal static object FalseBox = false;

        internal static object Box(bool value) => value ? TrueBox : FalseBox;
    }
}