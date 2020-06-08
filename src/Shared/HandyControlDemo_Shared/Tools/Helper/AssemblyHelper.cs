using System;
using System.Collections.Generic;

namespace HandyControlDemo.Tools
{
    internal class AssemblyHelper
    {
        private static readonly string NameSpaceStr = typeof(AssemblyHelper).Assembly.GetName().Name;

        private static readonly Dictionary<string, object> CacheDic = new Dictionary<string, object>();

        public static void Register(string name, object instance) => CacheDic[name] = instance;

        public static object ResolveByKey(string key)
        {
            if (CacheDic.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }

        public static object CreateInternalInstance(string className)
        {
            try
            {
                var type = Type.GetType($"{NameSpaceStr}.{className}");
                return type == null ? null : Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }
    }
}