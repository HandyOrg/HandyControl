using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Linq;

namespace HandyControl.Controls
{
    public class EnumPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new System.Windows.Controls.ComboBox
        {
            IsEnabled = !propertyItem.IsReadOnly,
            ItemsSource = GetEnumDescription(propertyItem.PropertyType),
        };

        public override DependencyProperty GetDependencyProperty() => Selector.SelectedValueProperty;

        protected override IValueConverter GetConverter(PropertyItem propertyItem) => new EnumDescriptionConverter(propertyItem.PropertyType);

        private string[] GetEnumDescription(Type enumType)
        {
            var values = Enum.GetValues(enumType);
            var enumDescs = new string[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                var fi = enumType.GetField(values.GetValue(i).ToString());
                var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    enumDescs[i] = ((DescriptionAttribute) attrs[0]).Description;
                }
                else
                {
                    enumDescs[i] = fi.Name;
                }
            }
            return enumDescs;
        }

        private class EnumDescriptionConverter : IValueConverter
        {
            private readonly List<KeyValuePair<Enum, string>> cache;

            public EnumDescriptionConverter(Type enumType)
            {
                var values = Enum.GetValues(enumType);
                cache = new List<KeyValuePair<Enum, string>>(values.Length);
                for (var i = 0; i < values.Length; i++)
                {
                    var fi = enumType.GetField(values.GetValue(i).ToString());
                    var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs.Length > 0)
                    {
                        cache.Add(new KeyValuePair<Enum, string>((Enum) values.GetValue(i), ((DescriptionAttribute) attrs[0]).Description));
                    }
                    else
                    {
                        cache.Add(new KeyValuePair<Enum, string>((Enum) values.GetValue(i), fi.Name));
                    }
                }
            }

            object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var myEnum = (Enum)value;
                var pair = cache.First(x => x.Key.Equals(myEnum));
                return pair.Value;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var enumStr = (string)value;
                var pair = cache.First(x => x.Value == enumStr);
                return pair.Key;
            }
        }
    }
}
