using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class PropertyItem : Control
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(object), typeof(PropertyItem), new PropertyMetadata(default(object)));

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            "DisplayName", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            "PropertyName", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
            "PropertyType", typeof(Type), typeof(PropertyItem), new PropertyMetadata(default(Type)));

        public Type PropertyType
        {
            get => (Type) GetValue(PropertyTypeProperty);
            set => SetValue(PropertyTypeProperty, value);
        }

        public static readonly DependencyProperty PropertyTypeNameProperty = DependencyProperty.Register(
            "PropertyTypeName", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public string PropertyTypeName
        {
            get => (string)GetValue(PropertyTypeNameProperty);
            set => SetValue(PropertyTypeNameProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
            "DefaultValue", typeof(object), typeof(PropertyItem), new PropertyMetadata(default(object)));

        public object DefaultValue
        {
            get => GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(
            "Category", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
            "Editor", typeof(PropertyEditorBase), typeof(PropertyItem), new PropertyMetadata(default(PropertyEditorBase)));

        public PropertyEditorBase Editor
        {
            get => (PropertyEditorBase)GetValue(EditorProperty);
            set => SetValue(EditorProperty, value);
        }

        public static readonly DependencyProperty EditorElementProperty = DependencyProperty.Register(
            "EditorElement", typeof(FrameworkElement), typeof(PropertyItem), new PropertyMetadata(default(FrameworkElement)));

        public FrameworkElement EditorElement
        {
            get => (FrameworkElement)GetValue(EditorElementProperty);
            set => SetValue(EditorElementProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsExpandedEnabledProperty = DependencyProperty.Register(
            "IsExpandedEnabled", typeof(bool), typeof(PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsExpandedEnabled
        {
            get => (bool)GetValue(IsExpandedEnabledProperty);
            set => SetValue(IsExpandedEnabledProperty, value);
        }

        public PropertyDescriptor PropertyDescriptor { get; set; }

        public virtual void InitElement()
        {
            if (Editor == null) return;
            EditorElement = Editor.CreateElement(this);
            Editor.CreateBinding(this, EditorElement);
        }
    }
}