using System.Collections.Generic;
using System.Linq;
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
        private string _token;

        private Adorner _container;

        private static readonly Dictionary<string, FrameworkElement> ContainerDic = new Dictionary<string, FrameworkElement>();

        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(Dialog), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsClosed
        {
            get => (bool)GetValue(IsClosedProperty);
            internal set => SetValue(IsClosedProperty, value);
        }

        public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached(
            "Token", typeof(string), typeof(Dialog), new PropertyMetadata(default(string), OnTokenChanged));

        private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (e.NewValue == null)
                {
                    Unregister(element);
                }
                else
                {
                    Register(e.NewValue.ToString(), element);
                }
            }
        }

        public static void SetToken(DependencyObject element, string value)
            => element.SetValue(TokenProperty, value);

        public static string GetToken(DependencyObject element)
            => (string)element.GetValue(TokenProperty);

        public Dialog()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => Close()));
        }

        public static void Register(string token, FrameworkElement element)
        {
            if (string.IsNullOrEmpty(token) || element == null) return;
            ContainerDic[token] = element;
        }

        public static void Unregister(string token, FrameworkElement element)
        {
            if (string.IsNullOrEmpty(token) || element == null) return;

            if (ContainerDic.ContainsKey(token))
            {
                if (ReferenceEquals(ContainerDic[token], element))
                {
                    ContainerDic.Remove(token);
                }
            }
        }

        public static void Unregister(FrameworkElement element)
        {
            if (element == null) return;
            var first = ContainerDic.FirstOrDefault(item => ReferenceEquals(element, item.Value));
            if (!string.IsNullOrEmpty(first.Key))
            {
                ContainerDic.Remove(first.Key);
            }
        }

        public static void Unregister(string token)
        {
            if (string.IsNullOrEmpty(token)) return;

            if (ContainerDic.ContainsKey(token))
            {
                ContainerDic.Remove(token);
            }
        }

        public static Dialog Show<T>(string token = "") where T : new() => Show(new T(), token);

        public static Dialog Show(object content, string token = "")
        {
            var dialog = new Dialog
            {
                _token = token,
                Content = content
            };

            FrameworkElement element;

            if (string.IsNullOrEmpty(token))
            {
                element = WindowHelper.GetActiveWindow();
            }
            else
            {
                ContainerDic.TryGetValue(token, out element);
            }

            if (element != null)
            {
                var decorator = VisualHelper.GetChild<AdornerDecorator>(element);
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
            if (string.IsNullOrEmpty(_token))
            {
                Close(WindowHelper.GetActiveWindow());
            }
            else if (ContainerDic.TryGetValue(_token, out var element))
            {
                Close(element);
            }
        }

        private void Close(DependencyObject element)
        {
            if (element != null)
            {
                var decorator = VisualHelper.GetChild<AdornerDecorator>(element);
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