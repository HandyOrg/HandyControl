using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
    ///     步骤条
    /// </summary>
    [ContentProperty("Items")]
    [TemplatePart(Name = ElementProgressBarBack, Type = typeof(ProgressBar))]
    [TemplatePart(Name = ElementUniformGridMain, Type = typeof(Panel))]
    public class StepBar : Control
    {
        private bool _isLoaded;

        private ProgressBar _progressBarBack;

        private Panel _panelMain;

        #region Constants

        private const string ElementProgressBarBack = "PART_ProgressBarBack";
        private const string ElementUniformGridMain = "PART_UniformGridMain";

        #endregion Constants

        public StepBar()
        {
            Loaded += (s1, e1) =>
            {
                if (_isLoaded) return;
                UpdateItems(Items);

                CommandBindings.Add(new CommandBinding(ControlCommands.Next, (s, e) => Next()));
                CommandBindings.Add(new CommandBinding(ControlCommands.Prev, (s, e) => Prev()));

                _isLoaded = true;
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _progressBarBack = GetTemplateChild(ElementProgressBarBack) as ProgressBar;
            _panelMain = GetTemplateChild(ElementUniformGridMain) as Panel;

            CheckNull();
        }

        private void CheckNull()
        {
            if (_panelMain == null || _progressBarBack == null) throw new Exception();
        }

        public int StepIndex { get; private set; }

        public List<StepItem> Items { get; set; } = new List<StepItem>();

        private void UpdateItems(IReadOnlyCollection<StepItem> list)
        {
            if (list == null) return;
            _panelMain.Children.Clear();

            var index = 1;
            foreach (var item in list)
            {
                item.Index = index;
                item.IndexStr = $"{Properties.Langs.Lang.Step}{index}";
                _panelMain.Children.Add(item);
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

            var colCount = _panelMain.Children.Count;
            if (colCount <= 0) return;
            _progressBarBack.Width = (colCount - 1) * (ActualWidth / colCount);
            _progressBarBack.Value = 0;
            _progressBarBack.Maximum = colCount - 1;
        }

        public void Next()
        {
            StepIndex++;
            if (StepIndex >= _panelMain.Children.Count)
            {
                StepIndex = _panelMain.Children.Count - 1;
                return;
            }
            if (_panelMain.Children[StepIndex - 1] is StepItem stepItemFinished)
                stepItemFinished.Status = true;
            if (_panelMain.Children[StepIndex] is StepItem stepItemSelected)
                stepItemSelected.Status = false;
            _progressBarBack.BeginAnimation(RangeBase.ValueProperty, AnimationHelper.CreateAnimation(StepIndex));
        }

        public void Prev()
        {
            StepIndex--;
            if (StepIndex < 0)
            {
                StepIndex = 0;
                return;
            }
            if (_panelMain.Children[StepIndex + 1] is StepItem stepItemFinished)
                stepItemFinished.Status = null;
            if (_panelMain.Children[StepIndex] is StepItem stepItemSelected)
                stepItemSelected.Status = false;
            _progressBarBack.BeginAnimation(RangeBase.ValueProperty, AnimationHelper.CreateAnimation(StepIndex));
        }
    }
}