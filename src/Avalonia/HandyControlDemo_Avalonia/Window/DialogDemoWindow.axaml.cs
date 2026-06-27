using System;
using Avalonia.Controls;
using HandyControl.Controls;
using HandyControlDemo.Data;

namespace HandyControlDemo.Window;

public partial class DialogDemoWindow : Avalonia.Controls.Window
{
    public string DialogToken { get; }

    public DialogDemoWindow()
    {
        InitializeComponent();

        DialogToken = $"{MessageToken.DialogDemoWindow}+{DateTime.Now:yyyyMMddHHmmssfff}";
        Dialog.SetToken(this, DialogToken);
    }
}
