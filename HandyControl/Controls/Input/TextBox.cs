using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    /// <inheritdoc cref="IDataInput" />
    public class TextBox : System.Windows.Controls.TextBox, IDataInput
    {
        public TextBox()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
            {
                ClearValue(TextProperty);
            }));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste)); //handle paste
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, CanCut)); //surpress cut
        }

        static TextBox()
        {
            TextProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(OnTextChanged));
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            VerifyData();
        }

        public Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        /// <summary>
        ///     数据是否错误
        /// </summary>
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(TextBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsError
        {
            get => (bool)GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, value);
        }

        /// <summary>
        ///     错误提示
        /// </summary>
        public static readonly DependencyProperty ErrorStrProperty = DependencyProperty.Register(
            "ErrorStr", typeof(string), typeof(TextBox), new PropertyMetadata(default(string)));

        public string ErrorStr
        {
            get => (string)GetValue(ErrorStrProperty);
            set => SetValue(ErrorStrProperty, value);
        }

        /// <summary>
        ///     文本类型
        /// </summary>
        public static readonly DependencyProperty TextTypeProperty = DependencyProperty.Register(
            "TextType", typeof(TextType), typeof(TextBox), new PropertyMetadata(default(TextType)));

        public TextType TextType
        {
            get => (TextType)GetValue(TextTypeProperty);
            set => SetValue(TextTypeProperty, value);
        }

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(TextBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        public virtual bool VerifyData()
        {
            OperationResult<bool> result;

            if (VerifyFunc != null)
            {
                result = VerifyFunc.Invoke(Text);
            }
            else
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    if (TextType != TextType.Common)
                    {
                        result = Text.IsKindOf(TextType) ? OperationResult.Success() : OperationResult.Failed(Properties.Langs.Lang.FormatError);
                    }
                    else
                    {
                        result = OperationResult.Success();
                    }
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

        #region Members

        /// <summary>
        /// Flags if the Text and Value properties are in the process of being sync'd
        /// </summary>
        private bool _isSyncingTextAndValueProperties;
        private bool _isInitialized;
        private bool _convertExceptionOccurred = false;

        #endregion //Members

        #region Properties

        protected MaskedTextProvider MaskProvider
        {
            get;
            set;
        }

        #region IncludePrompt

        public static readonly DependencyProperty IncludePromptProperty = DependencyProperty.Register("IncludePrompt", typeof(bool), typeof(TextBox), new UIPropertyMetadata(false, OnIncludePromptPropertyChanged));
        public bool IncludePrompt
        {
            get
            {
                return (bool)GetValue(IncludePromptProperty);
            }
            set
            {
                SetValue(IncludePromptProperty, value);
            }
        }

        private static void OnIncludePromptPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox maskedTextBox = o as TextBox;
            if (maskedTextBox != null)
                maskedTextBox.OnIncludePromptChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected virtual void OnIncludePromptChanged(bool oldValue, bool newValue)
        {
            UpdateMaskProvider(Mask);
        }

        #endregion //IncludePrompt

        #region IncludeLiterals

        public static readonly DependencyProperty IncludeLiteralsProperty = DependencyProperty.Register("IncludeLiterals", typeof(bool), typeof(TextBox), new UIPropertyMetadata(true, OnIncludeLiteralsPropertyChanged));
        public bool IncludeLiterals
        {
            get
            {
                return (bool)GetValue(IncludeLiteralsProperty);
            }
            set
            {
                SetValue(IncludeLiteralsProperty, value);
            }
        }

        private static void OnIncludeLiteralsPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox maskedTextBox = o as TextBox;
            if (maskedTextBox != null)
                maskedTextBox.OnIncludeLiteralsChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected virtual void OnIncludeLiteralsChanged(bool oldValue, bool newValue)
        {
            UpdateMaskProvider(Mask);
        }

        #endregion //IncludeLiterals

        #region Mask

        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register("Mask", typeof(string), typeof(TextBox), new UIPropertyMetadata("<>", OnMaskPropertyChanged));
        public string Mask
        {
            get
            {
                return (string)GetValue(MaskProperty);
            }
            set
            {
                SetValue(MaskProperty, value);
            }
        }

        private static void OnMaskPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox maskedTextBox = o as TextBox;
            if (maskedTextBox != null)
                maskedTextBox.OnMaskChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual void OnMaskChanged(string oldValue, string newValue)
        {
            UpdateMaskProvider(newValue);
            UpdateText(0);
        }

        #endregion //Mask

        #region PromptChar

        public static readonly DependencyProperty PromptCharProperty = DependencyProperty.Register("PromptChar", typeof(char), typeof(TextBox), new UIPropertyMetadata('_', OnPromptCharChanged));
        public char PromptChar
        {
            get
            {
                return (char)GetValue(PromptCharProperty);
            }
            set
            {
                SetValue(PromptCharProperty, value);
            }
        }

        private static void OnPromptCharChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox maskedTextBox = o as TextBox;
            if (maskedTextBox != null)
                maskedTextBox.OnPromptCharChanged((char)e.OldValue, (char)e.NewValue);
        }

        protected virtual void OnPromptCharChanged(char oldValue, char newValue)
        {
            UpdateMaskProvider(Mask);
        }

        #endregion //PromptChar

        #region SelectAllOnGotFocus

        public static readonly DependencyProperty SelectAllOnGotFocusProperty = DependencyProperty.Register("SelectAllOnGotFocus", typeof(bool), typeof(TextBox), new PropertyMetadata(false));
        public bool SelectAllOnGotFocus
        {
            get
            {
                return (bool)GetValue(SelectAllOnGotFocusProperty);
            }
            set
            {
                SetValue(SelectAllOnGotFocusProperty, value);
            }
        }

        #endregion //SelectAllOnGotFocus

        #region Text

        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox inputBase = o as TextBox;
            if (inputBase != null)
                inputBase.OnTextChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual void OnTextChanged(string oldValue, string newValue)
        {
            if (_isInitialized)
                SyncTextAndValueProperties(TextBox.TextProperty, newValue);
        }

        #endregion //Text

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(TextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        public object Value
        {
            get
            {
                return (object)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox maskedTextBox = o as TextBox;
            if (maskedTextBox != null)
                maskedTextBox.OnValueChanged((object)e.OldValue, (object)e.NewValue);
        }

        protected virtual void OnValueChanged(object oldValue, object newValue)
        {
            if (_isInitialized)
                SyncTextAndValueProperties(TextBox.ValueProperty, newValue);

            RoutedPropertyChangedEventArgs<object> args = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue);
            args.RoutedEvent = TextBox.ValueChangedEvent;
            RaiseEvent(args);
        }

        #endregion //Value

        #region ValueType

        public static readonly DependencyProperty ValueTypeProperty = DependencyProperty.Register("ValueType", typeof(Type), typeof(TextBox), new UIPropertyMetadata(typeof(String), OnValueTypeChanged));
        public Type ValueType
        {
            get
            {
                return (Type)GetValue(ValueTypeProperty);
            }
            set
            {
                SetValue(ValueTypeProperty, value);
            }
        }

        private static void OnValueTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TextBox maskedTextBox = o as TextBox;
            if (maskedTextBox != null)
                maskedTextBox.OnValueTypeChanged((Type)e.OldValue, (Type)e.NewValue);
        }

        protected virtual void OnValueTypeChanged(Type oldValue, Type newValue)
        {
            if (_isInitialized)
                SyncTextAndValueProperties(TextBox.TextProperty, Text);
        }

        #endregion //ValueType

        #endregion //Properties

        #region Base Class Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateMaskProvider(Mask);
            UpdateText(0);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (!_isInitialized)
            {
                _isInitialized = true;
                SyncTextAndValueProperties(ValueProperty, Value);
            }
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (SelectAllOnGotFocus)
            {
                SelectAll();
            }

            base.OnGotKeyboardFocus(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                HandlePreviewKeyDown(e);
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (!e.Handled)
            {
                HandlePreviewTextInput(e);
            }

            base.OnPreviewTextInput(e);
        }

        #endregion //Base Class Overrides

        #region Events

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(TextBox));
        public event RoutedPropertyChangedEventHandler<object> ValueChanged
        {
            add
            {
                AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ValueChangedEvent, value);
            }
        }

        #endregion //Events

        #region Methods

        #region Private

        private void UpdateText()
        {
            UpdateText(SelectionStart);
        }

        private void UpdateText(int position)
        {
            MaskedTextProvider provider = MaskProvider;
            if (provider == null)
                throw new InvalidOperationException();

            Text = provider.ToDisplayString();
            SelectionLength = 0;
            SelectionStart = position;
        }

        private int GetNextCharacterPosition(int startPosition)
        {
            int position = MaskProvider.FindEditPositionFrom(startPosition, true);
            return position == -1 ? startPosition : position;
        }

        private void UpdateMaskProvider(string mask)
        {
            //do not create a mask provider if the Mask is empty, which can occur if the IncludePrompt and IncludeLiterals properties
            //are set prior to the Mask.
            if (String.IsNullOrEmpty(mask))
                return;

            MaskProvider = new MaskedTextProvider(mask)
            {
                IncludePrompt = this.IncludePrompt,
                IncludeLiterals = this.IncludeLiterals,
                PromptChar = this.PromptChar,
                ResetOnSpace = false //should make this a property
            };
        }

        private object ConvertTextToValue(string text)
        {
            object convertedValue = null;

            Type dataType = ValueType;

            string valueToConvert = MaskProvider.ToString().Trim();

            try
            {
                if (valueToConvert.GetType() == dataType || dataType.IsInstanceOfType(valueToConvert))
                {
                    convertedValue = valueToConvert;
                }
#if !VS2008
                else if (String.IsNullOrWhiteSpace(valueToConvert))
                {
                    convertedValue = Activator.CreateInstance(dataType);
                }
#else
        else if ( String.IsNullOrEmpty( valueToConvert ) )
        {
          convertedValue = Activator.CreateInstance( dataType );
        }
#endif
                else if (null == convertedValue && valueToConvert is IConvertible)
                {
                    convertedValue = Convert.ChangeType(valueToConvert, dataType);
                }
            }
            catch
            {
                //if an excpetion occurs revert back to original value
                _convertExceptionOccurred = true;
                return Value;
            }

            return convertedValue;
        }

        private string ConvertValueToText(object value)
        {
            if (value == null)
                value = string.Empty;

            if (_convertExceptionOccurred)
            {
                value = Value;
                _convertExceptionOccurred = false;
            }

            //I have only seen this occur while in Blend, but we need it here so the Blend designer doesn't crash.
            if (MaskProvider == null)
                return value.ToString();

            MaskProvider.Set(value.ToString());
            return MaskProvider.ToDisplayString();
        }

        private void SyncTextAndValueProperties(DependencyProperty p, object newValue)
        {
            //prevents recursive syncing properties
            if (_isSyncingTextAndValueProperties)
                return;

            _isSyncingTextAndValueProperties = true;

            //this only occures when the user typed in the value
            if (TextBox.TextProperty == p)
            {
                if (newValue != null)
                    SetValue(TextBox.ValueProperty, ConvertTextToValue(newValue.ToString()));
            }

            SetValue(TextBox.TextProperty, ConvertValueToText(newValue));

            _isSyncingTextAndValueProperties = false;
        }

        private void HandlePreviewTextInput(TextCompositionEventArgs e)
        {
            if (!IsReadOnly)
            {
                this.InsertText(e.Text);
            }

            e.Handled = true;
        }

        private void HandlePreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = IsReadOnly
                         || HandleKeyDownDelete();
            }
            else if (e.Key == Key.Back)
            {
                e.Handled = IsReadOnly
                         || HandleKeyDownBack();
            }
            else if (e.Key == Key.Space)
            {
                if (!IsReadOnly)
                {
                    InsertText(" ");
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                if (!IsReadOnly && AcceptsReturn)
                {
                    this.InsertText("\r");
                }

                // We don't want the OnPreviewTextInput to be triggered for the Return/Enter key
                // when it is not accepted.
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                // We don't want the OnPreviewTextInput to be triggered at all for the Escape key.
                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                if (AcceptsTab)
                {
                    if (!IsReadOnly)
                    {
                        this.InsertText("\t");
                    }

                    e.Handled = true;
                }
            }
        }

        private bool HandleKeyDownDelete()
        {
            ModifierKeys modifiers = Keyboard.Modifiers;
            bool handled = true;

            if (modifiers == ModifierKeys.None)
            {
                if (!RemoveSelectedText())
                {
                    int position = SelectionStart;

                    if (position < Text.Length)
                    {
                        RemoveText(position, 1);
                        UpdateText(position);
                    }
                }
                else
                {
                    UpdateText();
                }
            }
            else if (modifiers == ModifierKeys.Control)
            {
                if (!RemoveSelectedText())
                {
                    int position = SelectionStart;

                    RemoveTextToEnd(position);
                    UpdateText(position);
                }
                else
                {
                    UpdateText();
                }
            }
            else if (modifiers == ModifierKeys.Shift)
            {
                if (RemoveSelectedText())
                {
                    UpdateText();
                }
                else
                {
                    handled = false;
                }
            }
            else
            {
                handled = false;
            }

            return handled;
        }

        private bool HandleKeyDownBack()
        {
            ModifierKeys modifiers = Keyboard.Modifiers;
            bool handled = true;

            if (modifiers == ModifierKeys.None || modifiers == ModifierKeys.Shift)
            {
                if (!RemoveSelectedText())
                {
                    int position = SelectionStart;

                    if (position > 0)
                    {
                        int newPosition = position - 1;

                        RemoveText(newPosition, 1);
                        UpdateText(newPosition);
                    }
                }
                else
                {
                    UpdateText();
                }
            }
            else if (modifiers == ModifierKeys.Control)
            {
                if (!RemoveSelectedText())
                {
                    RemoveTextFromStart(SelectionStart);
                    UpdateText(0);
                }
                else
                {
                    UpdateText();
                }
            }
            else
            {
                handled = false;
            }

            return handled;
        }

        private void InsertText(string text)
        {
            int position = SelectionStart;
            MaskedTextProvider provider = MaskProvider;

            bool textRemoved = this.RemoveSelectedText();

            position = GetNextCharacterPosition(position);

            if (!textRemoved && Keyboard.IsKeyToggled(Key.Insert))
            {
                if (provider.Replace(text, position))
                {
                    position += text.Length;
                }
            }
            else
            {
                if (provider.InsertAt(text, position))
                {
                    position += text.Length;
                }
            }

            position = GetNextCharacterPosition(position);

            this.UpdateText(position);
        }

        private void RemoveTextFromStart(int endPosition)
        {
            RemoveText(0, endPosition);
        }

        private void RemoveTextToEnd(int startPosition)
        {
            RemoveText(startPosition, Text.Length - startPosition);
        }

        private void RemoveText(int position, int length)
        {
            if (length == 0)
                return;

            MaskProvider.RemoveAt(position, position + length - 1);
        }

        private bool RemoveSelectedText()
        {
            int length = SelectionLength;

            if (length == 0)
                return false;

            int position = SelectionStart;

            return MaskProvider.RemoveAt(position, position + length - 1);
        }

        #endregion //Private

        #endregion //Methods

        #region Commands

        private void Paste(object sender, RoutedEventArgs e)
        {
            if (IsReadOnly)
                return;

            object data = Clipboard.GetData(DataFormats.Text);
            if (data != null)
            {
                string text = data.ToString().Trim();
                if (text.Length > 0)
                {
                    int position = SelectionStart;

                    MaskProvider.Set(text);

                    UpdateText(position);
                }
            }
        }

        private void CanCut(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        #endregion //Commands
    }
}