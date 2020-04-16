using HandyControl.Properties.Langs;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace HandyControl.Tools.Extension
{
    public class LangExtension : MarkupExtension
    {
        public LangExtension()
        {

        }

        public LangExtension(string key) => Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget pvt))
                return null;

            if (!(pvt.TargetObject is DependencyObject targetObject))
                return null;

            if (!(pvt.TargetProperty is DependencyProperty targetProperty))
                return null;

            var binding = new Binding(Key)
            {
                Source = LangSource,
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(targetObject, targetProperty, binding);
            return binding.ProvideValue(serviceProvider);
        }

        public string Key { get; set; }

        public virtual object LangSource => LangDecorator.Instance;
    }
}