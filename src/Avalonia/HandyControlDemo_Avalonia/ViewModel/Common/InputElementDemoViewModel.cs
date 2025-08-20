using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HandyControlDemo.ViewModel;

public partial class InputElementDemoViewModel : ObservableValidator
{
    [Required] [EmailAddress] [ObservableProperty] [NotifyDataErrorInfo]
    private string? _email1;

    [Required] [EmailAddress] [ObservableProperty] [NotifyDataErrorInfo]
    private string? _email2;

    [Required] [ObservableProperty] [NotifyDataErrorInfo]
    private string? _text1;

    [Required] [ObservableProperty] [NotifyDataErrorInfo]
    private string? _text2;

    [ObservableProperty] private double? _doubleValue1;

    [ObservableProperty] private double? _doubleValue2;
}
