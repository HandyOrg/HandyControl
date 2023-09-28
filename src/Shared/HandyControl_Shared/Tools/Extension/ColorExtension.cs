using System.Collections.Generic;
using System.Windows.Media;


namespace HandyControl.Tools.Extension;

/// <summary>
///     颜色扩展类
/// </summary>
public static class ColorExtension
{
    /// <summary>
    ///     将颜色转换为10进制表示（rgb顺序颠倒）
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static int ToInt32(this Color color) => color.R << 16 | color.G << 8 | color.B;

    /// <summary>
    ///     将颜色转换为10进制表示（rgb顺序颠倒）
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static int ToInt32Reverse(this Color color) => color.R | color.G << 8 | color.B << 18;

    internal static List<byte> ToList(this Color color) => new()
    {
        color.R,
        color.G,
        color.B
    };
}
