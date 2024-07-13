using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControlDemo.Properties.Langs;
using HandyControlDemo.Tools.Converter;

namespace HandyControlDemo.ViewModel;

public class InputElementDemoViewModel : ViewModelBase
{
    private string _email1;
    private string _email2;
    private double _doubleValue1;
    private double _doubleValue2;
    private IList<string> _dataList;
    private IList<string> _selectedDataList;

    public string Email1
    {
        get => _email1;
#if NET40
        set => Set(nameof(Email1), ref _email1, value);
#else
        set => Set(ref _email1, value);
#endif
    }

    public string Email2
    {
        get => _email2;
#if NET40
        set => Set(nameof(Email2), ref _email2, value);
#else
        set => Set(ref _email2, value);
#endif
    }

    public double DoubleValue1
    {
        get => _doubleValue1;
#if NET40
        set => Set(nameof(DoubleValue1), ref _doubleValue1, value);
#else
        set => Set(ref _doubleValue1, value);
#endif
    }

    public double DoubleValue2
    {
        get => _doubleValue2;
#if NET40
        set => Set(nameof(DoubleValue2), ref _doubleValue2, value);
#else
        set => Set(ref _doubleValue2, value);
#endif
    }

    public IList<string> DataList
    {
        get => _dataList;
#if NET40
        set => Set(nameof(DataList), ref _dataList, value);
#else
        set => Set(ref _dataList, value);
#endif
    }

    public IList<string> SelectedDataList
    {
        get => _selectedDataList;
#if NET40
        set => Set(nameof(SelectedDataList), ref _selectedDataList, value);
#else
        set => Set(ref _selectedDataList, value);
#endif
    }

    public RelayCommand<string> SearchCmd => new(Search);

    public InputElementDemoViewModel()
    {
        DataList = GetComboBoxDemoDataList();
        SelectedDataList = DataList.Where((t, i) => i % 2 == 0).ToList();
    }

    private void Search(string key)
    {
        Growl.Info(key);
    }

    private List<string> GetComboBoxDemoDataList()
    {
        var converter = new StringRepeatConverter();
        var list = new List<string>();
        for (var i = 1; i <= 9; i++)
        {
            list.Add($"{converter.Convert(Lang.Text, null, i, CultureInfo.CurrentCulture)}{i}");
        }

        return list;
    }
}
