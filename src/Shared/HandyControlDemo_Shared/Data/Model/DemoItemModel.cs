using GalaSoft.MvvmLight;

namespace HandyControlDemo.Data;

public class DemoItemModel : ObservableObject
{
    private bool _isVisible = true;
    private string _queriesText = string.Empty;

    public string Name { get; set; }

    public string GroupName { get; set; }

    public string TargetCtlName { get; set; }

    public string ImageName { get; set; }

    public bool IsNew { get; set; }

    public string QueriesText
    {
        get => _queriesText;
#if NET40
        set => Set(nameof(QueriesText), ref _queriesText, value);
#else
        set => Set(ref _queriesText, value);
#endif
    }

    public bool IsVisible
    {
        get => _isVisible;
#if NET40
        set => Set(nameof(IsVisible), ref _isVisible, value);
#else
        set => Set(ref _isVisible, value);
#endif
    }
}
