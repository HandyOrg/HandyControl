using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Tools.Helper;

namespace HandyControl.Controls
{
    [TemplatePart(Name = SearchTextBox, Type = typeof(System.Windows.Controls.TextBox))]
    public class AutoCompleteTextBox : ComboBox
    {
        private const string SearchTextBox = "PART_SearchTextBox";

        private bool ignoreTextChanging;

        private System.Windows.Controls.TextBox _searchTextBox;

        private object _selectedItem;

        static AutoCompleteTextBox()
        {
            TextProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        }

        public override void OnApplyTemplate()
        {
            if (_searchTextBox != null)
            {
                _searchTextBox.GotFocus -= SearchTextBoxGotFocus;
                _searchTextBox.KeyDown -= SearchTextBoxKeyDown;
                _searchTextBox.TextChanged -= SearchTextBoxTextChanged;
            }

            base.OnApplyTemplate();

            _searchTextBox = GetTemplateChild(SearchTextBox) as System.Windows.Controls.TextBox;
            if (_searchTextBox != null)
            {
                _searchTextBox.GotFocus += SearchTextBoxGotFocus;
                _searchTextBox.PreviewKeyDown += SearchTextBoxKeyDown;
                _searchTextBox.TextChanged += SearchTextBoxTextChanged;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _selectedItem = e.AddedItems[0];
            }
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);

            if (ItemContainerGenerator.ContainerFromItem(_selectedItem) is AutoCompleteTextBoxItem boxItem)
            {
                ignoreTextChanging = true;

                _searchTextBox.Text = BindingHelper.GetString(boxItem.Content, DisplayMemberPath);
                _searchTextBox.CaretIndex = _searchTextBox.Text.Length;

                ignoreTextChanging = true;

                Text = _searchTextBox.Text;

                ignoreTextChanging = false;
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is AutoCompleteTextBoxItem;

        protected override DependencyObject GetContainerForItemOverride() => new AutoCompleteTextBoxItem();

        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            _selectedItem = null;
            SelectedIndex = -1;

            if (ignoreTextChanging)
            {
                ignoreTextChanging = false;
                return;
            }

            Text = _searchTextBox.Text;
            if (string.IsNullOrEmpty(Text))
            {
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.FalseBox);
                _searchTextBox.Focus();
            }
            else if (_searchTextBox.IsFocused)
            {
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.TrueBox);
            }
        }

        private void SearchTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                var index = SelectedIndex - 1;
                if (index < 0)
                {
                    index = Items.Count - 1;
                }

                UpdateTextBoxBySelectedIndex(index);
            }
            else if (e.Key == Key.Down)
            {
                var index = SelectedIndex + 1;
                if (index >= Items.Count)
                {
                    index = 0;
                }

                UpdateTextBoxBySelectedIndex(index);
            }
            else if (e.Key == Key.Enter)
            {
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.FalseBox);
                e.Handled = true;
            }
        }

        private void UpdateTextBoxBySelectedIndex(int selectedIndex)
        {
            ignoreTextChanging = true;

            if (ItemContainerGenerator.ContainerFromIndex(selectedIndex) is AutoCompleteTextBoxItem boxItem)
            {
                _searchTextBox.Text = BindingHelper.GetString(boxItem.Content, DisplayMemberPath);
                _searchTextBox.CaretIndex = _searchTextBox.Text.Length;

                SelectedIndex = selectedIndex;
            }
        }

        private void SearchTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.TrueBox);
            }
        }
    }
}
