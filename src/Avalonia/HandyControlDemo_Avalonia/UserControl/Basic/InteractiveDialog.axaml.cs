using Avalonia.Controls;
using HandyControlDemo.ViewModel;

namespace HandyControlDemo.UserControl;

public partial class InteractiveDialog : Border
{
    public InteractiveDialog()
    {
        InitializeComponent();

        DataContext = new InteractiveDialogViewModel();
    }
}
