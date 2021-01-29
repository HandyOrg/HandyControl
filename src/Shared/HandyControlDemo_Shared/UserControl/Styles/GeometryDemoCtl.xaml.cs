using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using HandyControl.Controls;
using HandyControl.Tools;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl
{
    public partial class GeometryDemoCtl
    {
        private readonly HashSet<string> _lineSet = new HashSet<string>
        {
            "CheckedGeometry"
        };

        public ObservableCollection<GeometryItemModel> GeometryItems { get; set; } =
            new ObservableCollection<GeometryItemModel>();

        public GeometryDemoCtl()
        {
            InitializeComponent();
            GenerateGeometries();
        }

        public void GenerateGeometries()
        {
            var theme = new Theme();
           
            foreach (var key in theme.MergedDictionaries[0].Keys.OfType<string>().OrderBy(item => item))
            {
                if (!key.EndsWith("Geometry")) continue;
                GeometryItems.Add(new GeometryItemModel
                {
                    Key = key,
                    Data = ResourceHelper.GetResource<Geometry>(key),
                    Line = _lineSet.Contains(key)
                });
            }
        }
    }
}
