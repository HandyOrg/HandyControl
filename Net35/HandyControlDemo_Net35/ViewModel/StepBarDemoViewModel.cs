using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class StepBarDemoViewModel : ViewModelBase
    {
        /// <summary>
        ///     数据列表
        /// </summary>
        private List<StepBarDemoModel> _dataList;

        /// <summary>
        ///     数据列表
        /// </summary>
        public List<StepBarDemoModel> DataList
        {
            get => _dataList;
            set => Set(nameof(DataList), ref _dataList, value);
        }

        public StepBarDemoViewModel(DataService dataService)
        {
            DataList = dataService.GetStepBarDemoDataList();
        }

        private RelayCommand<Panel> _nextCmd;

        /// <summary>
        ///     下一步
        /// </summary>
        public RelayCommand<Panel> NextCmd => _nextCmd ?? (_nextCmd = new RelayCommand<Panel>(Next));

        private RelayCommand<Panel> _prevCmd;

        /// <summary>
        ///     上一步
        /// </summary>
        public RelayCommand<Panel> PrevCmd => _prevCmd ?? (_prevCmd = new RelayCommand<Panel>(Prev));

        private void Next(Panel panel)
        {
            foreach (var stepBar in panel.Children.OfType<StepBar>())
            {
                stepBar.Next();
            }
        }

        private void Prev(Panel panel)
        {
            foreach (var stepBar in panel.Children.OfType<StepBar>())
            {
                stepBar.Prev();
            }
        }
    }
}