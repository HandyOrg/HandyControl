using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using HandyControlDemo.Properties.Langs;
using HandyControlDemo.Tools.Converter;

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

    [ObservableProperty] private IList<string>? _dataList;

    public InputElementDemoViewModel()
    {
        DataList = GetComboBoxDemoDataList();
    }

    private static List<string> GetComboBoxDemoDataList()
    {
        var converter = new StringRepeatConverter();
        var list = new List<string>();
        for (int i = 1; i <= 9; i++)
        {
            list.Add($"{converter.Convert(Lang.Text, null!, i, CultureInfo.CurrentCulture)}{i}");
        }

        return list;
    }
}
