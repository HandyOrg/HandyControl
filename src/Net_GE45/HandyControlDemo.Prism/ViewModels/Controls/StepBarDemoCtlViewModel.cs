using HandyControl.Controls;
using HandyControlDemo.Data;
using HandyControlDemo.Service;
using Prism.Commands;
using System.Linq;
using System.Windows.Controls;

namespace HandyControlDemo.ViewModels
{
    public class StepBarDemoCtlViewModel : DemoViewModelBase<StepBarDemoModel>
    {
        public StepBarDemoCtlViewModel(DataService dataService) => DataList = dataService.GetStepBarDemoDataList();

        private int _stepIndex;

        public int StepIndex
        {
            get => _stepIndex;
            set => SetProperty(ref _stepIndex, value);
        }

        private DelegateCommand<Panel> _NextCmd;
        public DelegateCommand<Panel> NextCmd =>
            _NextCmd ?? (_NextCmd = new DelegateCommand<Panel>(Next));

        private DelegateCommand<Panel> _PrevCmd;
        public DelegateCommand<Panel> PrevCmd =>
            _PrevCmd ?? (_PrevCmd = new DelegateCommand<Panel>(Prev));

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
