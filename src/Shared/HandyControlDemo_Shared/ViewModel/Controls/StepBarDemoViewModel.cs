using System;
using System.Linq;
using System.Windows.Controls;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using GalaSoft.MvvmLight.CommandWpf;
# endif
using HandyControl.Controls;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class StepBarDemoViewModel : DemoViewModelBase<StepBarDemoModel>
    {
        public StepBarDemoViewModel(DataService dataService) => DataList = dataService.GetStepBarDemoDataList();

        private int _stepIndex;

        public int StepIndex
        {
            get => _stepIndex;
#if netle40
            set => Set(nameof(StepIndex), ref _stepIndex, value);
#else
            set => Set(ref _stepIndex, value);
#endif
        }

        /// <summary>
        ///     下一步
        /// </summary>
        public RelayCommand<Panel> NextCmd => new Lazy<RelayCommand<Panel>>(() => new RelayCommand<Panel>(Next)).Value;

        /// <summary>
        ///     上一步
        /// </summary>
        public RelayCommand<Panel> PrevCmd => new Lazy<RelayCommand<Panel>>(() => new RelayCommand<Panel>(Prev)).Value;

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