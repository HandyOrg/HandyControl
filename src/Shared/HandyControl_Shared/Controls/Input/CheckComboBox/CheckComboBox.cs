using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Data;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
    [TemplatePart(Name = ElementSelectAll, Type = typeof(CheckComboBoxItem))]
    public class CheckComboBox : ListBox
    {
        private const string ElementPanel = "PART_Panel";

        private const string ElementSelectAll = "PART_SelectAll";

        private Panel _panel;

        private Popup _popup;

        private CheckComboBoxItem _selectAllItem;

        private bool _isInternalAction;

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

        public CheckComboBox() => AddHandler(Controls.Tag.ClosedEvent, new RoutedEventHandler(Tags_OnClosed));

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

        private void SwitchAllItems(bool selected)
        {
            if (_isInternalAction) return;
            _isInternalAction = true;

            foreach (CheckComboBoxItem item in ItemContainerGenerator.Items)
            {
                item.SetCurrentValue(ListBoxItem.IsSelectedProperty, selected);
            }

            _isInternalAction = false;
            UpdateTags();
        }

        private void SelectAllItem_Selected(object sender, RoutedEventArgs e) => SwitchAllItems(true);

        private void SelectAllItem_Unselected(object sender, RoutedEventArgs e) => SwitchAllItems(false);

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            UpdateTags();
            base.OnSelectionChanged(e);
        }

        private void UpdateTags()
        {
            if (_isInternalAction) return;

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
