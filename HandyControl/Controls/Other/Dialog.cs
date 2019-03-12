using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class Dialog : ContentControl
    {
        private Adorner _container;

        public Dialog()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => Close()));
        }

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
                }
            }
        } 
    }
}