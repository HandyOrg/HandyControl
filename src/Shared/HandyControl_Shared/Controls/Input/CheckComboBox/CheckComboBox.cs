using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
    [TemplatePart(Name = ElementSelectAll, Type = typeof(CheckComboBoxItem))]
    public class CheckComboBox : ListBox, IDataInput
    {
        private const string ElementPanel = "PART_Panel";

        private const string ElementSelectAll = "PART_SelectAll";

        private Panel _panel;

        private CheckComboBoxItem _selectAllItem;

        private bool _isInternalAction;

        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(CheckComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsError
        {
            get => (bool) GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(CheckComboBox), new PropertyMetadata(default(string)));

        public string ErrorStr
        {
            get => (string) GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        public static readonly DependencyProperty TextTypeProperty = DependencyProperty.Register(
            "TextType", typeof(TextType), typeof(CheckComboBox), new PropertyMetadata(default(TextType)));

        public TextType TextType
        {
            get => (TextType) GetValue(TextTypeProperty);
            set => SetValue(TextTypeProperty, value);
        }

        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(CheckComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowClearButton
        {
            get => (bool) GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty MaxDropDownHeightProperty =
            System.Windows.Controls.ComboBox.MaxDropDownHeightProperty.AddOwner(typeof(CheckComboBox),
                new FrameworkPropertyMetadata(SystemParameters.PrimaryScreenHeight / 3));

        [Bindable(true), Category("Layout")]
        [TypeConverter(typeof(LengthConverter))]
        public double MaxDropDownHeight
        {
            get => (double) GetValue(MaxDropDownHeightProperty);
            set => SetValue(MaxDropDownHeightProperty, value);
        }

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen", typeof(bool), typeof(CheckComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsDropDownOpen
        {
            get => (bool) GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty TagStyleProperty = DependencyProperty.Register(
            "TagStyle", typeof(Style), typeof(CheckComboBox), new PropertyMetadata(default(Style)));

        public Style TagStyle
        {
            get => (Style) GetValue(TagStyleProperty);
            set => SetValue(TagStyleProperty, value);
        }

        public static readonly DependencyProperty ShowSelectAllButtonProperty = DependencyProperty.Register(
            "ShowSelectAllButton", typeof(bool), typeof(CheckComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowSelectAllButton
        {
            get => (bool) GetValue(ShowSelectAllButtonProperty);
            set => SetValue(ShowSelectAllButtonProperty, ValueBoxes.BooleanBox(value));
        }

        public CheckComboBox()
        {
            AddHandler(Controls.Tag.ClosedEvent, new RoutedEventHandler(Tags_OnClosed));
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) => SelectedItems.Clear()));
        }

        private void Tags_OnClosed(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Tag tag && tag.Tag is CheckComboBoxItem checkComboBoxItem)
            {
                checkComboBoxItem.SetCurrentValue(IsSelectedProperty, false);
            }
        }

        public override void OnApplyTemplate()
        {
            if (_selectAllItem != null)
            {
                _selectAllItem.Selected -= SelectAllItem_Selected;
                _selectAllItem.Unselected -= SelectAllItem_Unselected;
            }

            base.OnApplyTemplate();

            _panel = GetTemplateChild(ElementPanel) as Panel;
            _selectAllItem = GetTemplateChild(ElementSelectAll) as CheckComboBoxItem;
            if (_selectAllItem != null)
            {
                _selectAllItem.Selected += SelectAllItem_Selected;
                _selectAllItem.Unselected += SelectAllItem_Unselected;
            }
        }

        public bool VerifyData()
        {
            OperationResult<bool> result;

            if (VerifyFunc != null)
            {
                result = VerifyFunc.Invoke(null);
            }
            else
            {
                if (SelectedItems.Count > 0)
                {
                    result = OperationResult.Success();
                }
                else if (InfoElement.GetNecessary(this))
                {
                    result = OperationResult.Failed(Properties.Langs.Lang.IsNecessary);
                }
                else
                {
                    result = OperationResult.Success();
                }
            }

            IsError = !result.Data;
            ErrorStr = result.Message;
            return result.Data;
        }

        public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        private void SwitchAllItems(bool selected)
        {
            if (_isInternalAction) return;
            _isInternalAction = true;

            foreach (var item in Items)
            {
                if (ItemContainerGenerator.ContainerFromItem(item) is CheckComboBoxItem checkComboBoxItem)
                {
                    checkComboBoxItem.SetCurrentValue(ListBoxItem.IsSelectedProperty, selected);
                }
            }

            _isInternalAction = false;
            UpdateTags();
        }

        private void SelectAllItem_Selected(object sender, RoutedEventArgs e) => SwitchAllItems(true);

        private void SelectAllItem_Unselected(object sender, RoutedEventArgs e) => SwitchAllItems(false);

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            UpdateTags();
            VerifyData();

            base.OnSelectionChanged(e);
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is CheckComboBoxItem;

        protected override DependencyObject GetContainerForItemOverride() => new CheckComboBoxItem();

        private void UpdateTags()
        {
            if (_panel == null || _isInternalAction) return;

            if (_selectAllItem != null)
            {
                _isInternalAction = true;
                _selectAllItem.SetCurrentValue(IsSelectedProperty, SelectedItems.Count == Items.Count);
                _isInternalAction = false;
            }

            _panel.Children.Clear();

            foreach (var item in SelectedItems)
            {
                if (ItemContainerGenerator.ContainerFromItem(item) is CheckComboBoxItem checkComboBoxItem)
                {
                    var tag = new Tag
                    {
                        Style = TagStyle,
                        Content = checkComboBoxItem.Content,
                        Tag = checkComboBoxItem
                    };
                    _panel.Children.Add(tag);
                }
            }
        }
    }
}
