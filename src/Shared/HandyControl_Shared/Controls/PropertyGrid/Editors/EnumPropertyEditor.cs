using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using HandyControl.Data;

namespace HandyControl.Controls;

public class EnumPropertyEditor : PropertyEditorBase
{
    public override FrameworkElement CreateElement(PropertyItem propertyItem) => new System.Windows.Controls.ComboBox
    {
        IsEnabled = !propertyItem.IsReadOnly,
        ItemsSource = CreateEnumItems(propertyItem.PropertyType),
        DisplayMemberPath = nameof(EnumItem.Description),
        SelectedValuePath = nameof(EnumItem.Value)
    };

    public override DependencyProperty GetDependencyProperty() => Selector.SelectedValueProperty;

    private static IEnumerable<EnumItem> CreateEnumItems(Type enumType)
    {
        foreach (Enum value in Enum.GetValues(enumType))
        {
            var fieldInfo = enumType.GetField(value.ToString());
            var description = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), true) is DescriptionAttribute[] { Length: > 0 } attributes
                ? attributes[0].Description
                : value.ToString();

            yield return new EnumItem
            {
                Description = description,
                Value = value
            };
        }
    }
}
