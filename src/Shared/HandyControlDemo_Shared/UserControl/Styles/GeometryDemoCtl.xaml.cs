using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using HandyControl.Themes;
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
            var uri = new Uri("pack://application:,,,/HandyControl;component/Themes/Basic/Geometries.xaml", UriKind.Absolute);
            var dic = SharedResourceDictionary.SharedDictionaries[uri];
            var keys = dic.Keys.OfType<string>().OrderBy(item => item);

            foreach (var key in keys)
            {
                var data = ResourceHelper.GetResource<Geometry>(key);
                GeometryItems.Add(new GeometryItemModel
                {
                    Key = key,
                    Data = data,
                    Line = _lineSet.Contains(key)
                });
            }
        }
    }
}
