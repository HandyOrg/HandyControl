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

        private HandyControl.Controls.TextBox _addTagTextBox;

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

        public static readonly RoutedEvent AddTagTextBoxTextChangedEvent = EventManager.RegisterRoutedEvent("AddTagTextBoxTextChanged", RoutingStrategy.Bubble, typeof(EventHandler<FunctionEventArgs<string>>), typeof(TagPanel));

        public event EventHandler<FunctionEventArgs<string>> AddTagTextBoxTextChanged
        {
            add => AddHandler(AddTagTextBoxTextChangedEvent, value);
            remove => RemoveHandler(AddTagTextBoxTextChangedEvent, value);
        }

        public static readonly DependencyProperty ShowAddButtonProperty = DependencyProperty.Register(
            "ShowAddButton", typeof(bool), typeof(TagPanel), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowAddButton
        {
            get => (bool) GetValue(ShowAddButtonProperty);
            set => SetValue(ShowAddButtonProperty, value);
        }

        public static readonly DependencyProperty ShowAddTextBoxProperty = DependencyProperty.Register(
            "ShowAddTextBox", typeof(bool), typeof(TagPanel), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool ShowAddTextBox
        {
            get => (bool)GetValue(ShowAddTextBoxProperty);
            set => SetValue(ShowAddTextBoxProperty, value);
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

            if (ShowAddTextBox)
            {
                _isInternalAction = true;
                if (!Children.Contains(_addTagTextBox))
                {
                    _addTagTextBox = new HandyControl.Controls.TextBox
                    {
                        Width = 180,
                        Margin = TagMargin
                    };
                    _addTagTextBox.TextChanged += AddTagTextBox_TextChanged;
                    InfoElement.SetPlaceholder(_addTagTextBox, Properties.Langs.Lang.Tag);
                    Children.Add(_addTagTextBox);
                }
                else
                {
                    Children.Remove(_addTagTextBox);
                    Children.Add(_addTagTextBox);
                }
                _isInternalAction = false;
            }
        }

        private void AddTagTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (!string.IsNullOrEmpty(textBox.Text) && textBox.Text.Contains(";") || textBox.Text.Contains(" "))
                {
                    this.Children.Add(new Tag
                    {
                        Selectable = true,
                        Content = textBox.Text.Replace(";", "").Replace(" ", "")
                    });
                    textBox.Text = string.Empty;
                }

            }
            RaiseEvent(new FunctionEventArgs<string>(AddTagTextBoxTextChangedEvent, this) { Info = textBox.Text });
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(AddTagButtonClickEvent, this));
    }
}