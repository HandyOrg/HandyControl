using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class Dialog : ContentControl
    {
        private Adorner _container;

        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(Dialog), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsClosed
        {
            get => (bool) GetValue(IsClosedProperty);
            internal set => SetValue(IsClosedProperty, value);
        }

        public Dialog()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => Close()));
        }

        public static Dialog Show<T>() where T : new() => Show(new T());

        public static Dialog Show(object content)
        {
            var dialog = new Dialog
            {
                Content = content
            };

            var window = VisualHelper.GetActiveWindow();
            if (window != null)
            {
                var decorator = VisualHelper.GetChild<AdornerDecorator>(window);
                if (decorator != null)
                {
                    if (decorator.Child != null)
                    {
                        decorator.Child.IsEnabled = false;
                    }
                    var layer = decorator.AdornerLayer;
                    if (layer != null)
                    {
                        var container = new AdornerContainer(layer)
                        {
                            Child = dialog
                        };
                        dialog._container = container;
                        dialog.IsClosed = false;
                        layer.Add(container);
                    }
                }
            }

            return dialog;
        }

        public void Close()
        {
            var window = VisualHelper.GetActiveWindow();
            if (window != null)
            {
                var decorator = VisualHelper.GetChild<AdornerDecorator>(window);
                if (decorator != null)
                {
                    if (decorator.Child != null)
                    {
                        decorator.Child.IsEnabled = true;
                    }
                    var layer = decorator.AdornerLayer;
                    layer?.Remove(_container);
                    IsClosed = true;
                }
            }
        }
    }
}