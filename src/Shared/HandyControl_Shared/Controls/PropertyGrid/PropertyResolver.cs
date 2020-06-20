using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using HandyControl.Properties.Langs;

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
            [typeof(bool)] = EditorTypeCode.Switch,
            [typeof(DateTime)] = EditorTypeCode.DateTime,
            [typeof(HorizontalAlignment)] = EditorTypeCode.HorizontalAlignment,
            [typeof(VerticalAlignment)] = EditorTypeCode.VerticalAlignment
        };

        public string ResolveCategory(PropertyDescriptor propertyDescriptor)
        {
            var categoryAttribute = propertyDescriptor.Attributes.OfType<CategoryAttribute>().FirstOrDefault();

            return categoryAttribute == null ? 
                Lang.Miscellaneous :
                string.IsNullOrEmpty(categoryAttribute.Category) ? 
                    Lang.Miscellaneous : 
                    categoryAttribute.Category;
        }

        public string ResolveDisplayName(PropertyDescriptor propertyDescriptor)
        {
            var displayName = propertyDescriptor.DisplayName;
            if (string.IsNullOrEmpty(displayName))
            {
                displayName = propertyDescriptor.Name;
            }

            return displayName;
        }

        public string ResolveDescription(PropertyDescriptor propertyDescriptor) => propertyDescriptor.Description;

        public bool ResolveIsBrowsable(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsBrowsable;

        public bool ResolveIsDisplay(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsLocalizable;

        public bool ResolveIsReadOnly(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsReadOnly;

        public object ResolveDefaultValue(PropertyDescriptor propertyDescriptor)
        {
            var defaultValueAttribute = propertyDescriptor.Attributes.OfType<DefaultValueAttribute>().FirstOrDefault();
            return defaultValueAttribute?.Value;
        }

        public PropertyEditorBase ResolveEditor(PropertyDescriptor propertyDescriptor)
        {
            var editorAttribute = propertyDescriptor.Attributes.OfType<EditorAttribute>().FirstOrDefault();
            var editor = editorAttribute == null || string.IsNullOrEmpty(editorAttribute.EditorTypeName)
                ? CreateDefaultEditor(propertyDescriptor.PropertyType)
                : CreateEditor(Type.GetType(editorAttribute.EditorTypeName));

            return editor;
        }

        public virtual PropertyEditorBase CreateDefaultEditor(Type type) =>
            TypeCodeDic.TryGetValue(type, out var editorType)
                ? editorType switch
                {
                    EditorTypeCode.PlainText => new PlainTextPropertyEditor(),
                    EditorTypeCode.SByteNumber => new NumberPropertyEditor(sbyte.MinValue, sbyte.MaxValue),
                    EditorTypeCode.ByteNumber => new NumberPropertyEditor(byte.MinValue, byte.MaxValue),
                    EditorTypeCode.Int16Number => new NumberPropertyEditor(short.MinValue, short.MaxValue),
                    EditorTypeCode.UInt16Number => new NumberPropertyEditor(ushort.MinValue, ushort.MaxValue),
                    EditorTypeCode.Int32Number => new NumberPropertyEditor(int.MinValue, int.MaxValue),
                    EditorTypeCode.UInt32Number => new NumberPropertyEditor(uint.MinValue, uint.MaxValue),
                    EditorTypeCode.Int64Number => new NumberPropertyEditor(long.MinValue, long.MaxValue),
                    EditorTypeCode.UInt64Number => new NumberPropertyEditor(ulong.MinValue, ulong.MaxValue),
                    EditorTypeCode.SingleNumber => new NumberPropertyEditor(float.MinValue, float.MaxValue),
                    EditorTypeCode.DoubleNumber => new NumberPropertyEditor(double.MinValue, double.MaxValue),
                    EditorTypeCode.Switch => new SwitchPropertyEditor(),
                    EditorTypeCode.DateTime => new DateTimePropertyEditor(),
                    EditorTypeCode.HorizontalAlignment => new HorizontalAlignmentPropertyEditor(),
                    EditorTypeCode.VerticalAlignment => new VerticalAlignmentPropertyEditor(),
                    _ => new ReadOnlyTextPropertyEditor()
                }
                : type.IsSubclassOf(typeof(Enum))
                    ? (PropertyEditorBase) new EnumPropertyEditor()
                    : new ReadOnlyTextPropertyEditor();

        public virtual PropertyEditorBase CreateEditor(Type type) => new ReadOnlyTextPropertyEditor();

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
            DateTime,
            HorizontalAlignment,
            VerticalAlignment
        }
    }
}