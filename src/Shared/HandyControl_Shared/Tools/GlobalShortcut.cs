using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HandyControl.Data;

namespace HandyControl.Tools
{
    public class GlobalShortcut
    {
        private static readonly HashSet<InputBindingCollection> InputBindingCollectionSet = new HashSet<InputBindingCollection>();

        static GlobalShortcut()
        {
            KeyboardHook.KeyDown += KeyboardHook_KeyDown;
            KeyboardHook.Start();
        }

        private static void KeyboardHook_KeyDown(object sender, KeyboardHookEventArgs e) => HitTest(e.Key);

        private static void HitTest(Key key)
        {
            if (IsModifierKey(key)) return;

            var keyBindings = InputBindingCollectionSet
                .SelectMany(inputBindingCollection => inputBindingCollection.OfType<KeyBinding>())
                .Where(keyBinding => keyBinding.Key != Key.None && keyBinding.Modifiers == Keyboard.Modifiers && keyBinding.Key == key);
            keyBindings.ToList().ForEach(keyBinding =>
            {
                if (keyBinding.Command?.CanExecute(keyBinding.CommandParameter) == true)
                    keyBinding.Command?.Execute(keyBinding.CommandParameter);
            });
        }

        private static bool IsModifierKey(Key key)
        {
            return key == Key.LeftCtrl || key == Key.LeftAlt || key == Key.LeftShift || key == Key.LWin ||
                   key == Key.RightCtrl || key == Key.RightAlt || key == Key.RightShift || key == Key.RWin;
        }

        [Obsolete]
        public static void Init(DependencyObject host)
        {
        }

        public static readonly DependencyProperty KeyBindingsProperty = DependencyProperty.RegisterAttached(
            "KeyBindings", typeof(InputBindingCollection), typeof(GlobalShortcut), new PropertyMetadata(new InputBindingCollection()), OnValidateValue);

        private static bool OnValidateValue(object value)
        {
            if (value is InputBindingCollection inputBindingCollection && !InputBindingCollectionSet.Contains(inputBindingCollection))
                InputBindingCollectionSet.Add(inputBindingCollection);
            return true;
        }

        public static void SetKeyBindings(DependencyObject element, InputBindingCollection value)
            => element.SetValue(KeyBindingsProperty, value);

        public static InputBindingCollection GetKeyBindings(DependencyObject element)
            => (InputBindingCollection)element.GetValue(KeyBindingsProperty);
    }
}