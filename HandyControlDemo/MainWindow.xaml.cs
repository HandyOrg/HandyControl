using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using HandyControlDemo.Data.Model;

namespace HandyControlDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public static readonly DependencyProperty FruitListProperty = DependencyProperty.Register(
            "FruitList", typeof(ObservableCollection<Fruit>), typeof(MainWindow), new PropertyMetadata(default(ObservableCollection<string>)));

        public ObservableCollection<Fruit> FruitList
        {
            get => (ObservableCollection<Fruit>)GetValue(FruitListProperty);
            set => SetValue(FruitListProperty, value);
        }

        public MainWindow()
        {
            InitializeComponent();

            foreach (var border in StackPanelMain.Children.OfType<Border>())
            {
                border.Show();
            }

            var listZ = new List<CellGateInfo>();

            for (int i = 1; i <= 40; i++)
            {
                var itemC = new CellGateInfo();
                var listC = new List<CellGateInfo>();
                itemC.Name = $"{i}幢";
                for (int j = 1; j <= 2; j++)
                {
                    var itemF = new CellGateInfo();
                    var listF = new List<CellGateInfo>();
                    itemF.Name = $"{itemC.Name}{j}单元";
                    for (int k = 1; k <= 10; k++)
                    {
                        for (int l = 1; l <= 4; l++)
                        {
                            var itemR = new CellGateInfo
                            {
                                Name = $"{itemF.Name}{k}0{l}室"
                            };
                            listF.Add(itemR);
                        }
                    }
                    itemF.InfoList = listF;
                    listC.Add(itemF);
                }
                itemC.InfoList = listC;
                listZ.Add(itemC);
            }

            //TreeViewMain.ItemsSource = listZ;
            //ListBoxMain.ItemsSource = listZ;
            //ButtonContextMenu.ItemsSource = listZ;

            var showRecords = new List<CellGateRecord>
            {
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-1),
                    CellGateId = "1幢1单元A单元门",
                    PersonId = "宋立才",
                    DeviceId = "刷卡",
                    Direction = "进"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-2),
                    CellGateId = "1幢1单元A单元门",
                    PersonId = "宋立才",
                    DeviceId = "刷卡",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-2),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "周弘益",
                    DeviceId = "人脸",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-3),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "周弘益",
                    DeviceId = "人脸",
                    Direction = "进"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-2),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "彭念文",
                    DeviceId = "RFID",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-3),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "彭念文",
                    DeviceId = "RFID",
                    Direction = "进"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "王跃",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "赵兴隆",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "王跃",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "赵兴隆",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "王跃",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "赵兴隆",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "王跃",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "赵兴隆",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "王跃",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "赵兴隆",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "王跃",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "赵兴隆",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "王跃",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "赵兴隆",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "王跃",
                    DeviceId = "指纹",
                    Direction = "出"
                },
                new CellGateRecord
                {
                    CreateTime = DateTime.Now.AddDays(-4),
                    CellGateId = "20幢2单元A单元门",
                    PersonId = "赵兴隆",
                    DeviceId = "指纹",
                    Direction = "出"
                }
            };
            //DataGridMain.ItemsSource = showRecords;

            FruitList = new ObservableCollection<Fruit>
            {
                new Fruit
                {
                    Name = "苹果",
                    Id = 0
                },
                new Fruit
                {
                    Name = "香蕉",
                    Id = 1
                }
            };

        }

        public class Fruit
        {
            public string Name { get; set; }

            public int Id { get; set; }
        }

        //private void ThemesComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem comboBoxItem &&
        //        comboBoxItem.Tag is string tag)
        //    {
        //        var str = File.ReadAllText($"./Files/Themes/{tag}.json");
        //        var jObject = JObject.Parse(str);
        //        foreach (var item in jObject.Properties())
        //        {
        //            Resources.Remove(item.Name);
        //            Resources.Add(item.Name, item.Value.ToObject<SolidColorBrush>());
        //        }
        //    }
        //}

        //private void ButtonStepPrec_OnClick(object sender, RoutedEventArgs e)
        //{
        //    StepsExample.Prev();
        //}


        //private void ButtonStepNext_OnClick(object sender, RoutedEventArgs e)
        //{
        //    StepsExample.Next();
        //}

        //private void ButtonImageBrowser_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var window = new ImageBrowser(new Uri("pack://application:,,,/Resources/demo.png"));
        //    window.Show();
        //}

        private void ColorPicker_OnColorSelected(object sender, FunctionEventArgs<Color> e)
        {
            BlockColor.Text = $"{e.Info}-----{e.Info.ToInt32()}";
            PopupColor.IsOpen = false;
        }

        private void ColorPicker_OnCanceled(object sender, EventArgs e)
        {
            PopupColor.IsOpen = false;
        }

        private void ButtonColor_OnClick(object sender, RoutedEventArgs e)
        {
            PopupColor.IsOpen = true;
        }

        private void ButtonColorPreset_OnClick(object sender, RoutedEventArgs e)
        {
            ColorPicker.SelectedBrush = Brushes.Blue;
            PopupColor.IsOpen = true;
        }
    }
}