using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;
using HandyControl.Properties.Langs;
#if NET40
using HandyControl.Tools.Extension;
#endif

namespace HandyControl.Controls
{
    public class PropertyResolver
    {
        private static readonly Dictionary<Type, EditorTypeCode> TypeCodeDic = new Dictionary<Type, EditorTypeCode>
        {
            [typeof(string)] = EditorTypeCode.PlainText,
            [typeof(sbyte)] = EditorTypeCode.SByteNumber,
            [typeof(byte)] = EditorTypeCode.ByteNumber,
            [typeof(short)] = EditorTypeCode.Int16Number,
            [typeof(ushort)] = EditorTypeCode.UInt16Number,
            [typeof(int)] = EditorTypeCode.Int32Number,
            [typeof(uint)] = EditorTypeCode.UInt32Number,
            [typeof(long)] = EditorTypeCode.Int64Number,
            [typeof(ulong)] = EditorTypeCode.UInt64Number,
            [typeof(float)] = EditorTypeCode.SingleNumber,
            [typeof(double)] = EditorTypeCode.DoubleNumber,
            [typeof(bool)] = EditorTypeCode.Switch
        };

        public string ResolveCategory(PropertyInfo propertyInfo)
        {
            var categoryAttribute = propertyInfo.GetCustomAttribute<CategoryAttribute>();
            string category;
            if (categoryAttribute != null && !string.IsNullOrEmpty(categoryAttribute.Category))
            {
                category = categoryAttribute.Category;
            }
            else
            {
                category = Lang.Miscellaneous;
            }

            return category;
        }

        public string ResolveDisplayName(PropertyInfo propertyInfo)
        {
            var displayNameAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();
            string displayName;
            if (displayNameAttribute != null && !string.IsNullOrEmpty(displayNameAttribute.DisplayName))
            {
                displayName = displayNameAttribute.DisplayName;
            }
            else
            {
                displayName = propertyInfo.Name;
            }

            return displayName;
        }

        public string ResolveDescription(PropertyInfo propertyInfo)
        {
            var descriptionAttribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            string description;
            if (descriptionAttribute != null && !string.IsNullOrEmpty(descriptionAttribute.Description))
            {
                description = descriptionAttribute.Description;
            }
            else
            {
                description = string.Empty;
            }

            return description;
        }

        public bool ResolveBrowsable(PropertyInfo propertyInfo)
        {
            var browsableAttribute = propertyInfo.GetCustomAttribute<BrowsableAttribute>();
            return browsableAttribute == null || browsableAttribute.Browsable;
        }

        public bool ResolveIsReadOnly(PropertyInfo propertyInfo)
        {
            var isReadOnlyAttribute = propertyInfo.GetCustomAttribute<ReadOnlyAttribute>();
            return isReadOnlyAttribute != null && isReadOnlyAttribute.IsReadOnly;
        }

        public object ResolveDefaultValue(PropertyInfo propertyInfo)
        {
            var defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
            return defaultValueAttribute?.Value;
        }

        public IValueConverter ResolveConverter(PropertyInfo propertyInfo)
        {
            var typeConverterAttribute = propertyInfo.GetCustomAttribute<TypeConverterAttribute>();
            if (typeConverterAttribute != null && !string.IsNullOrEmpty(typeConverterAttribute.ConverterTypeName))
            {
                return CreateTypeConverter(Type.GetType(typeConverterAttribute.ConverterTypeName));
            }

            return null;
        }

        public virtual IValueConverter CreateTypeConverter(Type type) => Activator.CreateInstance(type) as IValueConverter;

        public PropertyEditorBase ResolveEditor(PropertyInfo propertyInfo)
        {
            var editorAttribute = propertyInfo.GetCustomAttribute<EditorAttribute>();
            var editor = editorAttribute == null || string.IsNullOrEmpty(editorAttribute.EditorTypeName)
                ? CreateDefaultEditor(propertyInfo.PropertyType)
                : CreateEditor(Type.GetType(editorAttribute.EditorTypeName));

            return editor;
        }

        public virtual PropertyEditorBase CreateDefaultEditor(Type type) =>
            TypeCodeDic.TryGetValue(type, out var editorType)
                ? (PropertyEditorBase) (editorType switch
                {
                    EditorTypeCode.PlainText => new StringPropertyEditor(),
                    EditorTypeCode.SByteNumber => new IntegerPropertyEditor(sbyte.MinValue, sbyte.MaxValue),
                    EditorTypeCode.ByteNumber => new IntegerPropertyEditor(byte.MinValue, byte.MaxValue),
                    EditorTypeCode.Int16Number => new IntegerPropertyEditor(short.MinValue, short.MaxValue),
                    EditorTypeCode.UInt16Number => new IntegerPropertyEditor(ushort.MinValue, ushort.MaxValue),
                    EditorTypeCode.Int32Number => new IntegerPropertyEditor(int.MinValue, int.MaxValue),
                    EditorTypeCode.UInt32Number => new IntegerPropertyEditor(uint.MinValue, uint.MaxValue),
                    EditorTypeCode.Int64Number => new IntegerPropertyEditor(long.MinValue, long.MaxValue),
                    EditorTypeCode.UInt64Number => new IntegerPropertyEditor(ulong.MinValue, ulong.MaxValue),
                    EditorTypeCode.SingleNumber => new IntegerPropertyEditor(float.MinValue, float.MaxValue),
                    EditorTypeCode.DoubleNumber => new IntegerPropertyEditor(double.MinValue, double.MaxValue),
                    EditorTypeCode.Switch => new BooleanPropertyEditor(),
                    EditorTypeCode.DateTime => new DateTimePropertyEditor(),
                    _ => new StringPropertyEditor()
                })
                : new StringPropertyEditor();

        public virtual PropertyEditorBase CreateEditor(Type type) => new StringPropertyEditor();

        private enum EditorTypeCode
        {
            PlainText,
            SByteNumber,
            ByteNumber,
            Int16Number,
            UInt16Number,
            Int32Number,
            UInt32Number,
            Int64Number,
            UInt64Number,
            SingleNumber,
            DoubleNumber,
            Switch,
            DateTime
        }
    }
}