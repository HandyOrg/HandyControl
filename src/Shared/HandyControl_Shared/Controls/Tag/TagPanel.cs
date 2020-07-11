using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    public class TagPanel : WrapPanel
    {
        private Button _addTagButton;

        private bool _isInternalAction;

        public TagPanel()
        {
            var behavior = new FluidMoveBehavior
            {
                AppliesTo = FluidMoveScope.Children,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EaseY = new PowerEase(),
                EaseX = new PowerEase()
            };
            var collection = Interaction.GetBehaviors(this);
            collection.Add(behavior);

            AddHandler(Controls.Tag.ClosedEvent, new RoutedEventHandler(Tag_OnClosed));
        }

        private void Tag_OnClosed(object sender, RoutedEventArgs e) => Children.Remove(e.OriginalSource as Tag);

        public static readonly RoutedEvent AddTagButtonClickEvent = EventManager.RegisterRoutedEvent("AddTagButtonClick", RoutingStrategy.Bubble, typeof(EventHandler), typeof(TagPanel));

        public event EventHandler AddTagButtonClick
        {
            add => AddHandler(AddTagButtonClickEvent, value);
            remove => RemoveHandler(AddTagButtonClickEvent, value);
        }

        public static readonly DependencyProperty ShowAddButtonProperty = DependencyProperty.Register(
            "ShowAddButton", typeof(bool), typeof(TagPanel), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowAddButton
        {
            get => (bool) GetValue(ShowAddButtonProperty);
            set => SetValue(ShowAddButtonProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty TagMarginProperty = DependencyProperty.Register(
            "TagMargin", typeof(Thickness), typeof(TagPanel), new PropertyMetadata(new Thickness(5)));

        public Thickness TagMargin
        {
            get => (Thickness) GetValue(TagMarginProperty);
            set => SetValue(TagMarginProperty, value);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            if (!(visualAdded is Tag tag)) return;
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            tag.Margin = TagMargin;
            if (_isInternalAction) return;
            if (ShowAddButton)
            {
                _isInternalAction = true;
                if (!Children.Contains(_addTagButton))
                {
                    _addTagButton = new Button
                    {
                        Style = ResourceHelper.GetResource<Style>(ResourceToken.AddTagButtonStyle),
                        Margin = TagMargin
                    };
                    _addTagButton.Click += AddTagButton_Click;
                    Children.Add(_addTagButton);
                }
                else
                {
                    Children.Remove(_addTagButton);
                    Children.Add(_addTagButton);
                }
                _isInternalAction = false;
            }
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(AddTagButtonClickEvent, this));
    }
}