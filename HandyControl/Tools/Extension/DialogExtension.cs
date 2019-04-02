using System;
using System.Threading.Tasks;
using System.Windows;
using HandyControl.Controls;

namespace HandyControl.Tools.Extension
{
    public interface IDialogResultable
    {
        object Result { get; set; }

        Action ClosureToken { get; set; }
    }

    public static class DialogExtension
    {
        public static Task<TResult> GetResultAsync<TResult>(this Dialog @this)
        {
            var tcs = new TaskCompletionSource<TResult>();

            try
            {
                if (@this.IsClosed)
                {
                    SetResult();
                }
                else
                {
                    @this.Unloaded += OnUnloaded;
                    @this.GetViewModel<IDialogResultable>().ClosureToken = @this.Close;
                }
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }

            return tcs.Task;

            // ReSharper disable once ImplicitlyCapturedClosure
            void OnUnloaded(object sender, RoutedEventArgs args)
            {
                @this.Unloaded -= OnUnloaded;
                SetResult();
            }

            void SetResult()
            {
                try
                {
                    tcs.TrySetResult(ExtractResult<TResult>(@this));
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
            }
        }

        public static Dialog Initialize<TViewModel>(this Dialog @this, Action<TViewModel> configure)
        {
            configure?.Invoke(@this.GetViewModel<TViewModel>());

            return @this;
        }

        public static TViewModel GetViewModel<TViewModel>(this Dialog @this)
        {
            if (!(@this.Content is FrameworkElement frameworkElement))
                throw new InvalidOperationException("The dialog is not a derived class of the FrameworkElement. ");

            if (!(frameworkElement.DataContext is TViewModel viewModel))
                throw new InvalidOperationException($"The view model of the dialog is not the {typeof(TViewModel)} type or its derived class. ");

            return viewModel;
        }

        private static TResult ExtractResult<TResult>(Dialog dialog)
        {
            var viewModelResult = dialog.GetViewModel<IDialogResultable>().Result;

            switch (viewModelResult)
            {
                case TResult result:
                    return result;
                case null when !typeof(TResult).IsValueType:
                    return default(TResult); // The default value of the value type may cause confusion, so should not help the user decide.
                default:
                    throw new InvalidCastException("Could not cast " +
                                                   $"the {(viewModelResult != null ? $"{viewModelResult.GetType()} type" : "null value")} " +
                                                   $"to the {typeof(TResult)} type. ");
            }
        }
    }
}
