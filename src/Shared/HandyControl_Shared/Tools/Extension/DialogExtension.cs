using System;
using System.Threading.Tasks;
using System.Windows;
using HandyControl.Controls;

namespace HandyControl.Tools.Extension;

public interface IDialogResultable<T>
{
    T Result { get; set; }

    Action CloseAction { get; set; }
}

public static class DialogExtension
{
    public static Task<TResult> GetResultAsync<TResult>(this Dialog dialog)
    {
        var tcs = new TaskCompletionSource<TResult>();

        try
        {
            if (dialog.IsClosed)
            {
                SetResult();
            }
            else
            {
                dialog.Unloaded += OnUnloaded;
                dialog.GetViewModel<IDialogResultable<TResult>>().CloseAction = dialog.Close;
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
            dialog.Unloaded -= OnUnloaded;
            SetResult();
        }

        void SetResult()
        {
            try
            {
                tcs.TrySetResult(dialog.GetViewModel<IDialogResultable<TResult>>().Result);
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        }
    }

    public static Dialog Initialize<TViewModel>(this Dialog dialog, Action<TViewModel> configure)
    {
        configure?.Invoke(dialog.GetViewModel<TViewModel>());

        return dialog;
    }

    public static TViewModel GetViewModel<TViewModel>(this Dialog dialog)
    {
        if (!(dialog.Content is FrameworkElement frameworkElement))
            throw new InvalidOperationException("The dialog is not a derived class of the FrameworkElement. ");

        if (!(frameworkElement.DataContext is TViewModel viewModel))
            throw new InvalidOperationException($"The view model of the dialog is not the {typeof(TViewModel)} type or its derived class. ");

        return viewModel;
    }
}
