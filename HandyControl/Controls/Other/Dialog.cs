using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public interface IDialogResult
    {
        object Result { get; set; }
    }

    public class Dialog : ContentControl
    {
        private Adorner _container;

        public Dialog()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => Close()));
        }

        public static Task<TResult> ShowAsync<TView, TResult>() where TView : class, new() =>
            ShowAsync<TResult>(new TView());

        public static Task<TResult> ShowAsync<TResult>(object content)
        {
            var tcs = new TaskCompletionSource<TResult>();

            var dialog = Show(content);
            dialog.Unloaded += OnUnloaded;

            return tcs.Task;

            void OnUnloaded(object sender, RoutedEventArgs args)
            {
                try
                {
                    dialog.Unloaded -= OnUnloaded;
                    tcs.SetResult(GetResult(dialog));
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }

            TResult GetResult(Dialog embeddedDialog)
            {
                if (!(embeddedDialog.Content is FrameworkElement frameworkElement))
                    throw new InvalidOperationException("The dialog is not a derived class of the FrameworkElement. ");

                if (!(frameworkElement.DataContext is IDialogResult dialogResult))
                    throw new InvalidOperationException("The view model of the dialog is not implement the IDialogResult interface. ");

                if (!(dialogResult.Result is TResult result))
                    throw new InvalidCastException("Could not cast " +
                                                   $"the {(dialogResult.Result != null ? $"{dialogResult.Result.GetType()} type" : "null value")} " +
                                                   $"to the {typeof(TResult)} type. ");

                return result;
            }
        }

        public static Dialog Show<T>() where T : class, new() => Show(new T());

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