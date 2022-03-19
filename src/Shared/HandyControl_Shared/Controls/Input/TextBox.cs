using System.Windows.Input;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

public class TextBox : System.Windows.Controls.TextBox
{
    public TextBox()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
        {
            SetCurrentValue(TextProperty, string.Empty);
        }));
    }
}
