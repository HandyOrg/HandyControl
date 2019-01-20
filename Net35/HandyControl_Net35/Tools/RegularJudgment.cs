using System;
using System.Text.RegularExpressions;
using HandyControl.Data;

namespace HandyControl.Tools
{
    /// <summary>
    ///     包含一些正则验证操作
    /// </summary>
    public static class RegularJudgment
    {
        private static readonly RegularPatterns RegularPatterns = new RegularPatterns();

        /// <summary>
        ///     判断字符串格式是否符合某种要求
        /// </summary>
        /// <param name="str">需要判断的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static bool IsKindOf(this string str, string pattern)
        {
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        ///     判断字符串是否满足指定的格式
        /// </summary>
        /// <param name="text">需要判断的字符串</param>
        /// <param name="textType">指定格式的文本</param>
        /// <returns></returns>
        public static bool IsKindOf(this string text, TextType textType)
        {
            if (textType == TextType.Common) return true;
            return Regex.IsMatch(text,
                RegularPatterns.GetValue(Enum.GetName(typeof(TextType), textType) + "Pattern").ToString());
        }

        /// <summary>
        ///     判断字符串格式是否为电子邮件
        /// </summary>
        /// <param name="email">需要判断的Email字符串</param>
        /// <returns>方法返回布尔值</returns>
        public static bool IsEmail(this string email)
        {
            return Regex.IsMatch(email, RegularPatterns.MailPattern);
        }

        /// <summary>
        ///     判断字符串格式是否为指定类型的IP地址
        /// </summary>
        /// <param name="ip">需要判断的IP字符串</param>
        /// <param name="ipType">指定的IP类型</param>
        /// <returns>方法返回布尔值</returns>
        public static bool IsIp(this string ip, IpType ipType)
        {
            switch (ipType)
            {
                case IpType.A: return Regex.IsMatch(ip, RegularPatterns.IpAPattern);
                case IpType.B: return Regex.IsMatch(ip, RegularPatterns.IpBPattern);
                case IpType.C: return Regex.IsMatch(ip, RegularPatterns.IpCPattern);
                case IpType.D: return Regex.IsMatch(ip, RegularPatterns.IpDPattern);
                case IpType.E: return Regex.IsMatch(ip, RegularPatterns.IpEPattern);
                default: return false;
            }
        }

        /// <summary>
        ///     判断字符串格式是否为IP地址
        /// </summary>
        /// <param name="ip">需要判断的IP字符串</param>
        /// <returns>方法返回布尔值</returns>
        public static bool IsIp(this string ip)
        {
            return Regex.IsMatch(ip, RegularPatterns.IpPattern);
        }

        /// <summary>
        ///     判断字符串格式是否为单个汉字
        /// </summary>
        /// <param name="str">需要判断的单个汉字字符串</param>
        /// <returns>方法返回布尔值</returns>
        public static bool IsChinese(this string str)
        {
            return Regex.IsMatch(str, RegularPatterns.ChinesePattern);
        }

        /// <summary>
        ///     判断字符串格式是否为url
        /// </summary>
        /// <param name="str">需要判断的url字符串</param>
        /// <returns>方法返回布尔值</returns>
        public static bool IsUrl(this string str)
        {
            return Regex.IsMatch(str, RegularPatterns.UrlPattern);
        }
    }
}