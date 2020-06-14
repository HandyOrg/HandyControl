using System;
using System.Reflection;

namespace HandyControl.Tools.Extension
{
    internal static class CustomAttributeExtension
    {
        public static Attribute GetCustomAttribute(this MemberInfo element, Type attributeType) => Attribute.GetCustomAttribute(element, attributeType);

        public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute => (T)GetCustomAttribute(element, typeof(T));
    }
}