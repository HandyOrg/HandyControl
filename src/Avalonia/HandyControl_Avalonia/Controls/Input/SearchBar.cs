using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using HandyControl.Data;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementSearchButton, Type = typeof(Button))]
public class SearchBar : TextBox
{
    private const string ElementSearchButton = "PART_SearchButton";

    public static readonly RoutedEvent<FunctionEventArgs<string?>> SearchStartedEvent =
        RoutedEvent.Register<SearchBar, FunctionEventArgs<string?>>(nameof(SearchStarted), RoutingStrategies.Bubble);

    public static readonly StyledProperty<bool> IsRealTimeProperty =
        AvaloniaProperty.Register<SearchBar, bool>(nameof(IsRealTime));

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<SearchBar, ICommand?>(nameof(Command));

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<SearchBar, object?>(nameof(CommandParameter));

    public static readonly StyledProperty<IInputElement?> CommandTargetProperty =
        AvaloniaProperty.Register<SearchBar, IInputElement?>(nameof(CommandTarget));

    private Button? _searchButton;

    static SearchBar()
    {
        TextProperty.Changed.AddClassHandler<SearchBar>((searchBar, _) =>
        {
            if (searchBar.IsRealTime)
            {
                searchBar.OnSearchStarted();
            }
        });
    }

    public event EventHandler<FunctionEventArgs<string?>> SearchStarted
    {
        add => AddHandler(SearchStartedEvent, value);
        remove => RemoveHandler(SearchStartedEvent, value);
    }

    public bool IsRealTime
    {
        get => GetValue(IsRealTimeProperty);
        set => SetValue(IsRealTimeProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public IInputElement? CommandTarget
    {
        get => GetValue(CommandTargetProperty);
        set => SetValue(CommandTargetProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_searchButton != null)
        {
            _searchButton.Click -= SearchButtonOnClick;
        }

        _searchButton = e.NameScope.Find<Button>(ElementSearchButton);
        if (_searchButton != null)
        {
            _searchButton.Click += SearchButtonOnClick;
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key == Key.Enter)
        {
            OnSearchStarted();
            e.Handled = true;
        }
    }

    private void SearchButtonOnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OnSearchStarted();
    }

    private void OnSearchStarted()
    {
        RaiseEvent(new FunctionEventArgs<string?>(SearchStartedEvent, this)
        {
            Info = Text
        });

        var command = Command;
        if (command == null)
        {
            return;
        }

        var parameter = CommandParameter ?? Text;
        if (!command.CanExecute(parameter))
        {
            return;
        }

        command.Execute(parameter);
    }
}
