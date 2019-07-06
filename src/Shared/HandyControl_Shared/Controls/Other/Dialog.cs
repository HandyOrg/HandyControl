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

        private static readonly Dictionary<string, System.Windows.Window> WindowDic = new Dictionary<string, System.Windows.Window>();

        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(Dialog), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsClosed
        {
            get => (bool) GetValue(IsClosedProperty);
            internal set => SetValue(IsClosedProperty, value);
        }

        public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached(
            "Token", typeof(string), typeof(Dialog), new PropertyMetadata(default(string), OnTokenChanged));

        private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is System.Windows.Window window)
            {
                if (e.NewValue == null)
                {
                    Unregister(window);
                }
                else
                {
                    Register(e.NewValue.ToString(), window);
                }
            }
        }

        public static void SetToken(DependencyObject element, string value)
            => element.SetValue(TokenProperty, value);

        public static string GetToken(DependencyObject element)
            => (string) element.GetValue(TokenProperty);

        public Dialog()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => Close()));
        }

        public static void Register(string token, System.Windows.Window window)
        {
            if (string.IsNullOrEmpty(token) || window == null) return;
            WindowDic[token] = window;
        }

        public static void Unregister(string token, System.Windows.Window window)
        {
            if (string.IsNullOrEmpty(token) || window == null) return;

            if (WindowDic.ContainsKey(token))
            {
                if (ReferenceEquals(WindowDic[token], window))
                {
                    WindowDic.Remove(token);
                }
            }
        }

        public static void Unregister(System.Windows.Window window)
        {
            if (window == null) return;
            var first = WindowDic.FirstOrDefault(item => ReferenceEquals(window, item.Value));
            if (!string.IsNullOrEmpty(first.Key))
            {
                WindowDic.Remove(first.Key);
            }
        }

        public static void Unregister(string token)
        {
            if (string.IsNullOrEmpty(token)) return;

            if (WindowDic.ContainsKey(token))
            {
                WindowDic.Remove(token);
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

            System.Windows.Window window;

            if (string.IsNullOrEmpty(token))
            {
                window = VisualHelper.GetActiveWindow();
            }
            else
            {
                WindowDic.TryGetValue(token, out window);
            }

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
            if (string.IsNullOrEmpty(_token))
            {
                Close(VisualHelper.GetActiveWindow());
            }
            else if (WindowDic.TryGetValue(_token, out var window))
            {
                Close(window);
            }
        }

        private void Close(System.Windows.Window window)
        {
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