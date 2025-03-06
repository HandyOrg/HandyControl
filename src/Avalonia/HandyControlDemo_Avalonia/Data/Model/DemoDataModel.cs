using System.Collections.Generic;


namespace HandyControlDemo.Data;

public class DemoDataModel
{
    public int Index { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsSelected { get; set; }

    public string Remark { get; set; } = string.Empty;

    public DemoType Type { get; set; }

    public string ImgPath { get; set; } = string.Empty;

    public List<DemoDataModel> DataList { get; set; } = [];
}
