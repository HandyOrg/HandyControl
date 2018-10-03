using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using HandyControl.Interactivity;
using HandyControl.Tools;

// ReSharper disable once CheckNamespace
namespace HandyControl.Controls
{
    /// <summary>
    ///     Steps.xaml 的交互逻辑
    /// </summary>
    [ContentProperty("Items")]
    public partial class StepBar
    {
        private bool _isLoaded;

        public StepBar()
        {
            InitializeComponent();

            Loaded += (s1, e1) =>
            {
                if (_isLoaded) return;
                UpdateItems(Items);

                CommandBindings.Add(new CommandBinding(ControlCommands.Next, (s, e) => Next()));
                CommandBindings.Add(new CommandBinding(ControlCommands.Prev, (s, e) => Prev()));

                _isLoaded = true;
            };
        }

        public int StepIndex { get; private set; }

        public List<StepItem> Items { get; set; } = new List<StepItem>();

        private void UpdateItems(IReadOnlyCollection<StepItem> list)
        {
            if (list == null) return;
            UniformGridMain.Children.Clear();

            var index = 1;
            foreach (var item in list)
            {
                item.Index = index;
                item.IndexStr = $"{Properties.Langs.Lang.Step}{index}";
                UniformGridMain.Children.Add(item);
                index++;
            }

            if (list.FirstOrDefault() is StepItem stepItem)
            {
                stepItem.Status = false;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var colCount = UniformGridMain.Children.Count;
            if (colCount <= 0) return;
            ProgressBarBack.Width = (colCount - 1) * (ActualWidth / colCount);
            ProgressBarBack.Value = 0;
            ProgressBarBack.Maximum = colCount - 1;
        }

        public void Next()
        {
            StepIndex++;
            if (StepIndex >= UniformGridMain.Children.Count)
            {
                StepIndex = UniformGridMain.Children.Count - 1;
                return;
            }
            if (UniformGridMain.Children[StepIndex - 1] is StepItem stepItemFinished)
                stepItemFinished.Status = true;
            if (UniformGridMain.Children[StepIndex] is StepItem stepItemSelected)
                stepItemSelected.Status = false;
            ProgressBarBack.BeginAnimation(RangeBase.ValueProperty, AnimationHelper.CreateAnimation(StepIndex));
        }

        public void Prev()
        {
            StepIndex--;
            if (StepIndex < 0)
            {
                StepIndex = 0;
                return;
            }
            if (UniformGridMain.Children[StepIndex + 1] is StepItem stepItemFinished)
                stepItemFinished.Status = null;
            if (UniformGridMain.Children[StepIndex] is StepItem stepItemSelected)
                stepItemSelected.Status = false;
            ProgressBarBack.BeginAnimation(RangeBase.ValueProperty, AnimationHelper.CreateAnimation(StepIndex));
        }
    }
}