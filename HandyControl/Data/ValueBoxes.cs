namespace HandyControl.Data
{
    /// <summary>
    ///     装箱后的布尔值（用于提高效率）
    /// </summary>
    internal static class ValueBoxes
    {
        internal static object TrueBox = true;

        internal static object FalseBox = false;

        internal static object Double0Box = .0;

        internal static object Double1Box = 1.0;

        internal static object Double100Box = 100.0;

        internal static object DoubleNeg1Box = -1.0;

        internal static object BooleanBox(bool value) => value ? TrueBox : FalseBox;
    }
}