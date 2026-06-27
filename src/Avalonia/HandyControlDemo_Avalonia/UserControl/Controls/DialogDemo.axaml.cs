using HandyControl.Controls;
using HandyControlDemo.Data;
using HandyControlDemo.ViewModel;

namespace HandyControlDemo.UserControl;

public partial class DialogDemo : Avalonia.Controls.UserControl
{
    public DialogDemo()
    {
        InitializeComponent();

        DataContext = ViewModelLocator.Instance.DialogDemo;
        Dialog.SetToken(this, MessageToken.DialogContainer);
    }
}
