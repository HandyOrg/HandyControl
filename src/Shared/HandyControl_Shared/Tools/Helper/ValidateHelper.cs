namespace HandyControl.Tools
{
    /// <summary>
    ///     验证帮助类
    /// </summary>
    public class ValidateHelper
    {
        /// <summary>
        ///     是否在浮点数范围内
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfDouble(object value)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v));
        }

        /// <summary>
        ///     是否在正浮点数范围内
        /// </summary>
        /// <param name="value"></param>
        /// <param name="includeZero"></param>
        /// <returns></returns>
        public static bool IsInRangeOfPosDouble(object value, bool includeZero = false)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v)) && (includeZero ? v >= 0 : v > 0);
        }

        /// <summary>
        ///     是否在正浮点数范围内
        /// </summary>
        /// <param name="value"></param>
        /// <param name="includeZero"></param>
        /// <returns></returns>
        public static bool IsInRangeOfNegDouble(object value, bool includeZero = false)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v)) && (includeZero ? v <= 0 : v < 0);
        }
    }
}