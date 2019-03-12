using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    public class SearchBar : TextBox
    {
        public SearchBar()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Search, (s, e) => OnSearchStarted()));
        }

        public static readonly RoutedEvent SearchStartedEvent =
            EventManager.RegisterRoutedEvent("SearchStarted", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<string>>), typeof(SearchBar));

        public event EventHandler<FunctionEventArgs<string>> SearchStarted
        {
            add => AddHandler(SearchStartedEvent, value);
            remove => RemoveHandler(SearchStartedEvent, value);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Enter)
            {
                OnSearchStarted();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (IsRealTime)
            {
                OnSearchStarted();
            }
            VerifyData();
        }

        private void OnSearchStarted()
        {
            RaiseEvent(new FunctionEventArgs<string>(SearchStartedEvent, this)
            {
                Info = Text
            });
        }

        /// <summary>
        ///     是否实时搜索
        /// </summary>
        public static readonly DependencyProperty IsRealTimeProperty = DependencyProperty.Register(
            "IsRealTime", typeof(bool), typeof(SearchBar), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否实时搜索
        /// </summary>
        public bool IsRealTime
        {
            get => (bool)GetValue(IsRealTimeProperty);
            set => SetValue(IsRealTimeProperty, value);
        }
    }
}
