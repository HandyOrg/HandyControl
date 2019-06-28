using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using HandyControl.Data;

namespace HandyControl.Tools
{
    public class GlobalShortcut
    {
        private static readonly Dictionary<string, KeyBinding> CommandDic = new Dictionary<string, KeyBinding>();

        static GlobalShortcut()
        {
            KeyboardHook.KeyDown += KeyboardHook_KeyDown;
        }

        private static void KeyboardHook_KeyDown(object sender, KeyboardHookEventArgs e) => HitTest(e.Key);

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

        public static void Init(DependencyObject host)
        {
            CommandDic.Clear();

            if (host == null) return;

            var keyBindings = GetKeyBindings(host);
            if (keyBindings == null || keyBindings.Count == 0) return;

            foreach (KeyBinding item in keyBindings)
            {
                if (item.Key == Key.None)
                {
                    continue;
                }

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

            KeyboardHook.Start();
        }

        public static readonly DependencyProperty KeyBindingsProperty = DependencyProperty.RegisterAttached(
            "KeyBindings", typeof(InputBindingCollection), typeof(GlobalShortcut), new PropertyMetadata(new InputBindingCollection()));

        public static void SetKeyBindings(DependencyObject element, InputBindingCollection value)
            => element.SetValue(KeyBindingsProperty, value);

        public static InputBindingCollection GetKeyBindings(DependencyObject element)
            => (InputBindingCollection) element.GetValue(KeyBindingsProperty);
    }
}