using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
#if NET40
using System.Windows.Threading;
#endif
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    [TemplatePart(Name = AutoCompletePanel, Type = typeof(Panel))]
    [TemplatePart(Name = EditableTextBox, Type = typeof(System.Windows.Controls.TextBox))]
    [TemplatePart(Name = AutoPopupAutoComplete, Type = typeof(Popup))]
    public class ComboBox : System.Windows.Controls.ComboBox, IDataInput
    {
#if NET40

        private string _searchText;

        private DispatcherTimer _autoCompleteTimer;

#endif
        private bool _isAutoCompleteAction = true;

        private Panel _autoCompletePanel;

        private System.Windows.Controls.TextBox _editableTextBox;

        private Popup _autoPopupAutoComplete;

        private const string AutoCompletePanel = "PART_AutoCompletePanel";

        private const string AutoPopupAutoComplete = "PART_Popup_AutoComplete";

        private const string EditableTextBox = "PART_EditableTextBox";

        public ComboBox()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
            {
                SetCurrentValue(SelectedValueProperty, null);
                SetCurrentValue(SelectedItemProperty, null);
                SetCurrentValue(SelectedIndexProperty, -1);
                SetCurrentValue(TextProperty, "");
            }));
        }

        public override void OnApplyTemplate()
        {
            if (_editableTextBox != null)
            {
                BindingOperations.ClearBinding(_editableTextBox, System.Windows.Controls.TextBox.TextProperty);
                _editableTextBox.GotFocus -= EditableTextBox_GotFocus;
                _editableTextBox.LostFocus -= EditableTextBox_LostFocus;
            }

            base.OnApplyTemplate();

            if (IsEditable)
            {
                _editableTextBox = GetTemplateChild(EditableTextBox) as System.Windows.Controls.TextBox;

                if (_editableTextBox != null)
                {
                    _editableTextBox.TextChanged += EditableTextBox_TextChanged;

                    if (AutoComplete)
                    {
#if NET40
                        _autoCompleteTimer = new DispatcherTimer
                        {
                            Interval = TimeSpan.FromMilliseconds(500)
                        };
                        _autoCompleteTimer.Tick += AutoCompleteTimer_Tick;
#endif
                        _autoPopupAutoComplete = GetTemplateChild(AutoPopupAutoComplete) as Popup;
                        _autoCompletePanel = GetTemplateChild(AutoCompletePanel) as Panel;
                        _editableTextBox.SetBinding(System.Windows.Controls.TextBox.TextProperty, new Binding(SearchTextProperty.Name)
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.OneWayToSource,
#if !NET40
                            Delay = 500,
#endif
                            Source = this
                        });
                        _editableTextBox.GotFocus += EditableTextBox_GotFocus;
                        _editableTextBox.LostFocus += EditableTextBox_LostFocus;
                    }
                }
            }
        }

#if NET40

        private void AutoCompleteTimer_Tick(object sender, EventArgs e)
        {
            UpdateSearchItems(_searchText);
            _autoCompleteTimer.Stop();
        }

#endif

        private void EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyData();
        }

        private void EditableTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_autoPopupAutoComplete != null)
            {
                _autoPopupAutoComplete.IsOpen = false;
            }
        }

#if !NET40
        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);

            _isAutoCompleteAction = false;
        }
#endif

        private void EditableTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_autoPopupAutoComplete != null && _editableTextBox != null &&
                !string.IsNullOrEmpty(_editableTextBox.Text))
            {
                _autoPopupAutoComplete.IsOpen = true;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            _isAutoCompleteAction = false;
            base.OnSelectionChanged(e);
            VerifyData();
#if NET40
            _isAutoCompleteAction = true;
#endif
        }

        /// <summary>
        ///     数据验证委托
        /// </summary>
        public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        /// <summary>
        ///     数据搜索委托
        /// </summary>
        public Func<ItemCollection, object, IEnumerable<object>> SearchFunc { get; set; }

        /// <summary>
        ///     数据是否错误
        /// </summary>
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(ComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     数据是否错误
        /// </summary>
        public bool IsError
        {
            get => (bool)GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, value);
        }

        /// <summary>
        ///     错误提示
        /// </summary>
        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(ComboBox), new PropertyMetadata(default(string)));

        /// <summary>
        ///     错误提示
        /// </summary>
        public string ErrorStr
        {
            get => (string)GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        /// <summary>
        ///     文本类型
        /// </summary>
        public static readonly DependencyPropertyKey TextTypePropertyKey =
            DependencyProperty.RegisterReadOnly("TextType", typeof(TextType), typeof(ComboBox),
                new PropertyMetadata(default(TextType)));

        /// <summary>
        ///     文本类型
        /// </summary>
        public static readonly DependencyProperty TextTypeProperty = TextTypePropertyKey.DependencyProperty;

        /// <summary>
        ///     文本类型
        /// </summary>
        public TextType TextType
        {
            get => (TextType)GetValue(TextTypeProperty);
            set => SetValue(TextTypeProperty, value);
        }

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(ComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        /// <summary>
        ///     是否自动完成输入
        /// </summary>
        public static readonly DependencyProperty AutoCompleteProperty = DependencyProperty.Register(
            "AutoComplete", typeof(bool), typeof(ComboBox), new PropertyMetadata(ValueBoxes.FalseBox, OnAutoCompleteChanged));

        private static void OnAutoCompleteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ComboBox)d;
            if (ctl._editableTextBox != null)
            {
                ctl.UpdateSearchItems(ctl._editableTextBox.Text);
            }
        }

        /// <summary>
        ///     更新搜索的项目
        /// </summary>
        /// <param name="key"></param>
        private void UpdateSearchItems(string key)
        {
            if (_editableTextBox != null && _autoPopupAutoComplete != null)
            {
                _autoPopupAutoComplete.IsOpen = !string.IsNullOrEmpty(key);
                _autoCompletePanel.Children.Clear();

                if (SearchFunc == null)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        foreach (var item in Items)
                        {
                            var content = item?.ToString();
                            if (content == null) continue;
                            if (!content.Contains(key)) continue;

                            _autoCompletePanel.Children.Add(CreateSearchItem(item));
                        }
                    }
                }
                else
                {
                    foreach (var item in SearchFunc.Invoke(Items, key))
                    {
                        _autoCompletePanel.Children.Add(CreateSearchItem(item));
                    }
                }
            }
        }

        private ComboBoxItem CreateSearchItem(object content)
        {
            var item = new ComboBoxItem
            {
                Content = content,
                Style = ItemContainerStyle,
                ContentTemplate = ItemTemplate
            };

            item.PreviewMouseLeftButtonDown += AutoCompleteItem_PreviewMouseLeftButtonDown;

            return item;
        }

        private void AutoCompleteItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ComboBoxItem comboBoxItem)
            {
                if (_autoPopupAutoComplete != null)
                {
                    _autoPopupAutoComplete.IsOpen = false;
                }

                _isAutoCompleteAction = false;
                SelectedValue = comboBoxItem.Content;
            }
        }

        /// <summary>
        ///     搜索文本
        /// </summary>
        internal static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
            "SearchText", typeof(string), typeof(ComboBox), new PropertyMetadata(default(string), OnSearchTextChanged));

        private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ComboBox)d;
            if (ctl._isAutoCompleteAction)
            {
#if NET40
                ctl._searchText = e.NewValue as string;
                ctl._autoCompleteTimer.Stop();
                ctl._autoCompleteTimer.Start();
#else
                ctl.UpdateSearchItems(e.NewValue as string);
#endif
            }

            ctl._isAutoCompleteAction = true;
        }

        /// <summary>
        ///     搜索文本
        /// </summary>
        internal string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        /// <summary>
        ///     是否自动完成输入
        /// </summary>
        public bool AutoComplete
        {
            get => (bool)GetValue(AutoCompleteProperty);
            set => SetValue(AutoCompleteProperty, value);
        }

        /// <summary>
        ///     验证数据
        /// </summary>
        /// <returns></returns>
        public virtual bool VerifyData()
        {
            OperationResult<bool> result;

            var value = _editableTextBox == null ? Text : _editableTextBox.Text;

            if (VerifyFunc != null)
            {
                result = VerifyFunc.Invoke(value);
            }
            else
            {
                if (!string.IsNullOrEmpty(value))
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
    }
}