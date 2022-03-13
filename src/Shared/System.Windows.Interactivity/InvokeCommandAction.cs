using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace HandyControl.Interactivity;

public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
{
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(InvokeCommandAction), null);

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(InvokeCommandAction),
            null);

    private string _commandName;

    public string CommandName
    {
        get
        {
            ReadPreamble();
            return _commandName;
        }
        set
        {
            if (CommandName == value)
                return;
            WritePreamble();
            _commandName = value;
            WritePostscript();
        }
    }

    public ICommand Command
    {
        get => (ICommand) GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    protected override void Invoke(object parameter)
    {
        if (AssociatedObject == null)
            return;
        var command = ResolveCommand();
        if (command == null || !command.CanExecute(CommandParameter))
            return;
        command.Execute(CommandParameter);
    }

    private ICommand ResolveCommand()
    {
        var command = (ICommand) null;
        if (Command != null)
            command = Command;
        else if (AssociatedObject != null)
            foreach (var property in AssociatedObject.GetType()
                         .GetProperties(BindingFlags.Instance | BindingFlags.Public))
                if (typeof(ICommand).IsAssignableFrom(property.PropertyType) &&
                    string.Equals(property.Name, CommandName, StringComparison.Ordinal))
                    command = (ICommand) property.GetValue(AssociatedObject, null);
        return command;
    }
}
