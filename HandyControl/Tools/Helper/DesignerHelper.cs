using System;
using System.Linq;
using System.Reflection;

namespace HandyControl.Tools
{
    public class DesignerHelper
    {
        public static bool IsInDesignMode()
        {
            try
            {
                var type1 = Type.GetType(
                    "System.ComponentModel.DesignerProperties, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                if (type1 == null)
                    return false;
                var obj1 = type1.GetTypeInfo().GetDeclaredField("IsInDesignModeProperty").GetValue(null);
                var type2 = Type.GetType(
                    "System.ComponentModel.DependencyPropertyDescriptor, WindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                var type3 = Type.GetType(
                    "System.Windows.FrameworkElement, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                if (type2 == null || type3 == null)
                    return false;
                var list = type2.GetTypeInfo().GetDeclaredMethods("FromProperty").ToList();
                if (list.Count == 0)
                    return false;
                var methodInfo = list.FirstOrDefault(mi =>
                {
                    if (mi.IsPublic && mi.IsStatic)
                        return mi.GetParameters().Length == 2;
                    return false;
                });
                if (methodInfo == null)
                    return false;
                var obj2 = methodInfo.Invoke(null, new[]
                {
                    obj1,
                    type3
                });
                if (obj2 == null)
                    return false;
                var declaredProperty1 = type2.GetTypeInfo().GetDeclaredProperty("Metadata");
                if (declaredProperty1 == null)
                    return false;
                var obj3 = declaredProperty1.GetValue(obj2, null);
                var type4 = Type.GetType(
                    "System.Windows.PropertyMetadata, WindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                if (obj3 == null || type4 == null)
                    return false;
                var declaredProperty2 = type4.GetTypeInfo().GetDeclaredProperty("DefaultValue");
                if (declaredProperty2 == null)
                    return false;
                return (bool) declaredProperty2.GetValue(obj3, null);
            }
            catch
            {
                return false;
            }
        }
    }
}