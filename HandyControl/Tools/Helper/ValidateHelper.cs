namespace HandyControl.Tools
{
    /// <summary>
    ///     验证帮助类
    /// </summary>
    internal class ValidateHelper
    {
        /// <summary>
        ///     是否在浮点数范围内
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool IsInRangeOfDouble(object value)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v));
        }
    }
}