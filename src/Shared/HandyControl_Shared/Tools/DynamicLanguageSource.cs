using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;
namespace HandyControl.Tools
{
    public class DynamicLanguageSource : INotifyPropertyChanged
    {
        public static DynamicLanguageSource Instance { get; } = new DynamicLanguageSource();

        private readonly Dictionary<string, ResourceManager> resourceManagerDictionary = new Dictionary<string, ResourceManager>();

        public string this[string key]
        {
            get
            {
                Tuple<string, string> tuple = SplitName(key);
                string translation = null;
                if (resourceManagerDictionary.ContainsKey(tuple.Item1))
                {
                    translation = resourceManagerDictionary[tuple.Item1].GetString(tuple.Item2);
                }

                return translation ?? key;
            }
        }

        private string language = Thread.CurrentThread.CurrentUICulture.Name;
        public string Language
        {
            get => language;
            set
            {
                if (language != null)
                {
                    language = value;

                    CultureInfo cultureInfo = new CultureInfo(value);
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;
                    Thread.CurrentThread.CurrentCulture = cultureInfo;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                }
            }
        }

        // WPF bindings register PropertyChanged event if the object supports it and update themselves when it is raised
        public event PropertyChangedEventHandler PropertyChanged;

        public void AddResourceManager(ResourceManager resourceManager)
        {
            if (!resourceManagerDictionary.ContainsKey(resourceManager.BaseName))
            {
                resourceManagerDictionary.Add(resourceManager.BaseName, resourceManager);
            }
        }

        public static Tuple<string, string> SplitName(string local)
        {
            int idx = local.ToString().LastIndexOf(".");
            Tuple<string, string> tuple = new Tuple<string, string>(local.Substring(0, idx), local.Substring(idx + 1));
            return tuple;
        }
    }

    public class DynamicLanguage : DependencyObject
    {
        public static readonly DependencyProperty ResourceManagerProperty =
            DependencyProperty.RegisterAttached("ResourceManager", typeof(ResourceManager), typeof(DynamicLanguage));

        public static ResourceManager GetResourceManager(DependencyObject dependencyObject)
        {
            return (ResourceManager)dependencyObject.GetValue(ResourceManagerProperty);
        }

        public static void SetResourceManager(DependencyObject dependencyObject, ResourceManager value)
        {
            dependencyObject.SetValue(ResourceManagerProperty, value);
        }
    }

    public class LocExtension : MarkupExtension
    {
        public string StringName { get; }

        public LocExtension(string stringName)
        {
            StringName = stringName;
        }

        private ResourceManager GetResourceManager(object control)
        {
            if (control is DependencyObject dependencyObject)
            {
                object localValue = dependencyObject.ReadLocalValue(DynamicLanguage.ResourceManagerProperty);

                // does this control have a "Translation.ResourceManager" attached property with a set value?
                if (localValue != DependencyProperty.UnsetValue)
                {
                    if (localValue is ResourceManager resourceManager)
                    {
                        DynamicLanguageSource.Instance.AddResourceManager(resourceManager);

                        return resourceManager;
                    }
                }
            }

            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // targetObject is the control that is using the LocExtension
            object targetObject = (serviceProvider as IProvideValueTarget)?.TargetObject;

            if (targetObject?.GetType().Name == "SharedDp") // is extension used in a control template?
            {
                return targetObject; // required for template re-binding
            }

            string baseName = GetResourceManager(targetObject)?.BaseName ?? string.Empty;

            if (string.IsNullOrEmpty(baseName))
            {
                // rootObject is the root control of the visual tree (the top parent of targetObject)
                object rootObject = (serviceProvider as IRootObjectProvider)?.RootObject;
                baseName = GetResourceManager(rootObject)?.BaseName ?? string.Empty;
            }

            if (string.IsNullOrEmpty(baseName)) // template re-binding
            {
                if (targetObject is FrameworkElement frameworkElement)
                {
                    baseName = GetResourceManager(frameworkElement.TemplatedParent)?.BaseName ?? string.Empty;
                }
            }

            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath($"[{baseName}.{StringName}]"),
                Source = DynamicLanguageSource.Instance,
                FallbackValue = StringName
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
