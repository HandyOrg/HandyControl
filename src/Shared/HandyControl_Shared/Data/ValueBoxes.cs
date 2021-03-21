namespace HandyControl.Data
{
    /// <summary>
    ///     装箱后的值类型（用于提高效率）
    /// </summary>
    internal static class ValueBoxes
    {
        internal static object TrueBox = true;

        internal static object FalseBox = false;

        internal static object Double0Box = .0;

        internal static object Double01Box = .1;

        internal static object Double1Box = 1.0;

        internal static object Double10Box = 10.0;

        internal static object Double20Box = 20.0;

        internal static object Double100Box = 100.0;

        internal static object Double200Box = 200.0;

        internal static object Double300Box = 300.0;

        internal static object DoubleNeg1Box = -1.0;

        internal static object Decimal0Box = .0m;

        internal static object Decimal01Box = .1m;

        internal static object Decimal1Box = 1.0m;

        internal static object Decimal10Box = 10.0m;

        internal static object Decimal20Box = 20.0m;

        internal static object Decimal100Box = 100.0m;

        internal static object Decimal200Box = 200.0m;

        internal static object Decimal300Box = 300.0m;

        internal static object DecimalNeg1Box = -1.0m;

        internal static object Int0Box = 0;

        internal static object Int1Box = 1;

        internal static object Int2Box = 2;

        internal static object Int5Box = 5;

        internal static object Int99Box = 99;

        internal static object BooleanBox(bool value) => value ? TrueBox : FalseBox;
    }
}
