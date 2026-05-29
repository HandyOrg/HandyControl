using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

namespace HandyControl.Data;

public class MessageBoxInfo
{
    public string Message { get; set; }

    public string Caption { get; set; }

    public MessageBoxButton Button { get; set; } = MessageBoxButton.OK;

    public Geometry Icon { get; set; }

    public string IconKey { get; set; }

    public Brush IconBrush { get; set; }

    public string IconBrushKey { get; set; }

    public MessageBoxResult DefaultResult { get; set; } = MessageBoxResult.None;

    public ControlTheme ControlTheme { get; set; }

    public string ControlThemeKey { get; set; }

    public Window Owner { get; set; }
}


public enum MessageBoxResult
{
    //
    // Summary:
    //     The message box returns no result.
    None = 0,
    //
    // Summary:
    //     The result value of the message box is OK.
    OK = 1,
    //
    // Summary:
    //     The result value of the message box is Cancel.
    Cancel = 2,
    //
    // Summary:
    //     The result value of the message box is Yes.
    Yes = 6,
    //
    // Summary:
    //     The result value of the message box is No.
    No = 7
}
//
// Summary:
//     Specifies the buttons that are displayed on a message box. Used as an argument
//     of the Overload:MessageBox.Show method.
public enum MessageBoxButton
{
    //
    // Summary:
    //     The message box displays an OK button.
    OK = 0,
    //
    // Summary:
    //     The message box displays OK and Cancel buttons.
    OKCancel = 1,
    //
    // Summary:
    //     The message box displays Yes, No, and Cancel buttons.
    YesNoCancel = 3,
    //
    // Summary:
    //     The message box displays Yes and No buttons.
    YesNo = 4
}

//
// Summary:
//     Specifies the icon that is displayed by a message box.
public enum MessageBoxImage
{
    //
    // Summary:
    //     No icon is displayed.
    None = 0,
    //
    // Summary:
    //     The message box contains a symbol consisting of a white X in a circle with a
    //     red background.
    Hand = 16,
    //
    // Summary:
    //     The message box contains a symbol consisting of a question mark in a circle.
    Question = 32,
    //
    // Summary:
    //     The message box contains a symbol consisting of an exclamation point in a triangle
    //     with a yellow background.
    Exclamation = 48,
    //
    // Summary:
    //     The message box contains a symbol consisting of a lowercase letter i in a circle.
    Asterisk = 64,
    //
    // Summary:
    //     The message box contains a symbol consisting of white X in a circle with a red
    //     background.
    Stop = 16,
    //
    // Summary:
    //     The message box contains a symbol consisting of white X in a circle with a red
    //     background.
    Error = 16,
    //
    // Summary:
    //     The message box contains a symbol consisting of an exclamation point in a triangle
    //     with a yellow background.
    Warning = 48,
    //
    // Summary:
    //     The message box contains a symbol consisting of a lowercase letter i in a circle.
    Information = 64
}
