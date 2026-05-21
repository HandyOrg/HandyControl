using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using HandyControl.Data;

namespace HandyControl.Controls;

public class SearchBar : TextBox, ICommandSource
{
    public static readonly RoutedEvent SearchStartedEvent = RoutedEvent.Register<SearchBar, FunctionEventArgs<string>>("SearchStarted", RoutingStrategies.Bubble);

    public event EventHandler<FunctionEventArgs<string>> SearchStarted
    {
        add => AddHandler(SearchStartedEvent, value);
        remove => RemoveHandler(SearchStartedEvent, value);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key == Key.Enter)
        {
            OnSearchStarted();
        }
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);

        if (IsRealTime)
        {
            OnSearchStarted();
        }
    }

    private void OnSearchStarted()
    {
        RaiseEvent(new FunctionEventArgs<string>(SearchStartedEvent, this)
        {
            Info = Text
        });

        switch (Command)
        {
            case null:
                return;
            default:
                Command.Execute(CommandParameter);
                break;
        }
    }

    /// <summary>
    ///     是否实时搜索
    /// </summary>
    public static readonly StyledProperty<bool> IsRealTimeProperty = AvaloniaProperty.Register<SearchBar, bool>(nameof(IsRealTime), false);


    /// <summary>
    ///     是否实时搜索
    /// </summary>
    public bool IsRealTime
    {
        get => (bool)GetValue(IsRealTimeProperty);
        set => SetValue(IsRealTimeProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly StyledProperty<ICommand> CommandProperty = AvaloniaProperty.Register<SearchBar, ICommand>(nameof(Command));


    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == CommandProperty)
        {
            OnCommandChanged(change);
        }
    }

    private void OnCommandChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ICommand oldCommand)
        {
            oldCommand.CanExecuteChanged -= CanExecuteChanged;
        }
        if (e.NewValue is ICommand newCommand)
        {
            newCommand.CanExecuteChanged += CanExecuteChanged;
        }
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object?> CommandParameterProperty = AvaloniaProperty.Register<SearchBar, object?>(nameof(CommandParameter));


    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public void CanExecuteChanged(object sender, EventArgs e)
    {
        if (Command == null) return;

        IsEnabled = Command.CanExecute(CommandParameter);
    }

}
