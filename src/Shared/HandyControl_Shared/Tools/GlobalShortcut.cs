using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using HandyControl.Data;

namespace HandyControl.Tools
{
    public class GlobalShortcut
    {
        private static ObservableCollection<KeyBinding> KeyBindingCollection;

        private static readonly Dictionary<string, KeyBinding> CommandDic = new Dictionary<string, KeyBinding>();

        private static void KeyboardHook_KeyDown(object sender, KeyboardHookEventArgs e) => HitTest(e.Key);

        static GlobalShortcut()
        {
            KeyboardHook.KeyDown += KeyboardHook_KeyDown;
        }

        private static void HitTest(Key key)
        {
            if (IsModifierKey(key)) return;

            var modifierKeys = Keyboard.Modifiers;
            var keyStr = modifierKeys != ModifierKeys.None ? $"{modifierKeys.ToString()}, {key.ToString()}" : key.ToString();
            ExecuteCommand(keyStr);
        }

        private static void ExecuteCommand(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (CommandDic.ContainsKey(key))
            {
                var keyBinding = CommandDic[key];
                var command = keyBinding.Command;
                if (command != null && command.CanExecute(keyBinding.CommandParameter))
                {
                    command.Execute(keyBinding.CommandParameter);
                }
            }
        }

        private static bool IsModifierKey(Key key)
        {
            return key == Key.LeftCtrl || key == Key.LeftAlt || key == Key.LeftShift || key == Key.LWin ||
                   key == Key.RightCtrl || key == Key.RightAlt || key == Key.RightShift || key == Key.RWin;
        }

        public static void Init(List<KeyBinding> list)
        {
            CommandDic.Clear();

            if (list == null) return;
            KeyBindingCollection = new ObservableCollection<KeyBinding>(list);
            if (KeyBindingCollection.Count == 0) return;
            
            AddKeyBindings(KeyBindingCollection);
            KeyboardHook.Start();
        }

        private static void AddKeyBindings(IEnumerable<KeyBinding> keyBindings)
        {
            foreach (var item in keyBindings)
            {
                if (item.Key == Key.None) continue;

                if (item.Modifiers == ModifierKeys.None)
                {
                    CommandDic[item.Key.ToString()] = item;
                }
                else
                {
                    var keyStr = $"{item.Modifiers.ToString()}, {item.Key.ToString()}";
                    CommandDic[keyStr] = item;
                }
            }
        }

        public static readonly DependencyProperty HostProperty = DependencyProperty.RegisterAttached(
            "Host", typeof(bool), typeof(GlobalShortcut), new PropertyMetadata(ValueBoxes.FalseBox, OnHostChanged));

        private static void OnHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerHelper.IsInDesignMode) return;

            if (KeyBindingCollection != null)
            {
                KeyBindingCollection.CollectionChanged -= KeyBindingCollection_CollectionChanged;
            }
            KeyBindingCollection = GetKeyBindings(d);
            if (KeyBindingCollection != null)
            {
                KeyBindingCollection.CollectionChanged += KeyBindingCollection_CollectionChanged;
            }
        }

        private static void KeyBindingCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            AddKeyBindings(KeyBindingCollection);
            KeyboardHook.Start();
        }

        public static void SetHost(DependencyObject element, bool value)
            => element.SetValue(HostProperty, value);

        public static bool GetHost(DependencyObject element)
            => (bool) element.GetValue(HostProperty);

        public static readonly DependencyProperty KeyBindingsProperty = DependencyProperty.RegisterAttached(
            "KeyBindings", typeof(ObservableCollection<KeyBinding>), typeof(GlobalShortcut), new PropertyMetadata(new ObservableCollection<KeyBinding>()));

        public static void SetKeyBindings(DependencyObject element, ObservableCollection<KeyBinding> value)
            => element.SetValue(KeyBindingsProperty, value);

        public static ObservableCollection<KeyBinding> GetKeyBindings(DependencyObject element)
            => (ObservableCollection<KeyBinding>) element.GetValue(KeyBindingsProperty);
    }
}