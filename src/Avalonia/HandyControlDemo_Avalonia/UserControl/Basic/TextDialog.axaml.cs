using Avalonia.Controls;
using Avalonia.VisualTree;
using HandyControl.Controls;

namespace HandyControlDemo.UserControl;

public partial class TextDialog : Border
{
    public TextDialog()
    {
        InitializeComponent();

        CloseButton.Click += (_, _) => this.FindAncestorOfType<Dialog>()?.Close();
    }
}
