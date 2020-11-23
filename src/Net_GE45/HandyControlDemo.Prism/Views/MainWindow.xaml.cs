using HandyControl.Controls;
using HandyControl.Tools;
using HandyControlDemo.Data;
using HandyControlDemo.ViewModels;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MessageBox = HandyControl.Controls.MessageBox;

namespace HandyControlDemo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool _drawerCodeUsed;
        private string _currentDemoKey;
        private Dictionary<string, TextEditor> _textEditor;

        private DelegateCommand _GlobalShortcutInfoCmd;
        public DelegateCommand GlobalShortcutInfoCmd =>
            _GlobalShortcutInfoCmd ?? (_GlobalShortcutInfoCmd = new DelegateCommand(() => { Growl.Info("Global Shortcut Info"); }));

        private DelegateCommand _GlobalShortcutWarningCmd;
        public DelegateCommand GlobalShortcutWarningCmd =>
            _GlobalShortcutWarningCmd ?? (_GlobalShortcutWarningCmd = new DelegateCommand(() => { Growl.Warning("Global Shortcut Warning"); }));

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            GlobalShortcut.Init(new List<KeyBinding>
            {
                new KeyBinding(GlobalShortcutInfoCmd, Key.I, ModifierKeys.Control | ModifierKeys.Alt),
                new KeyBinding(GlobalShortcutWarningCmd, Key.E, ModifierKeys.Control | ModifierKeys.Alt)
            });
            Dialog.SetToken(this, "MainWindow");
            WindowAttach.SetIgnoreAltF4(this, true);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (GlobalData.NotifyIconIsShow)
            {
                MessageBox.Info(Properties.Langs.Lang.AppClosingTip, Properties.Langs.Lang.Tip);
                Hide();
                e.Cancel = true;
            }
            else
            {
                base.OnClosing(e);
            }
        }

        //Todo: Not Working
        private void DrawerCode_OnOpened(object sender, RoutedEventArgs e)
        {
            if (!_drawerCodeUsed)
            {
                var textEditorCustomStyle = ResourceHelper.GetResource<Style>("TextEditorCustom");
                _textEditor = new Dictionary<string, TextEditor>
                {
                    ["XAML"] = new TextEditor
                    {
                        Style = textEditorCustomStyle,
                        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML")
                    },
                    ["C#"] = new TextEditor
                    {
                        Style = textEditorCustomStyle,
                        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#")
                    },
                    ["VM"] = new TextEditor
                    {
                        Style = textEditorCustomStyle,
                        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#")
                    }
                };
                BorderCode.Child = new TabControl
                {
                    Style = ResourceHelper.GetResource<Style>("TabControlInLine"),
                    Items =
                    {
                        new TabItem
                        {
                            Header = "XAML",
                            Content = _textEditor["XAML"]
                        },
                        new TabItem
                        {
                            Header = "C#",
                            Content = _textEditor["C#"]
                        },
                        new TabItem
                        {
                            Header = "VM",
                            Content = _textEditor["VM"]
                        }
                    }
                };

                _drawerCodeUsed = true;
            }

            var typeKey = LeftMainContentViewModel.Instance.DemoInfoCurrent.Key;
            var demoKey = LeftMainContentViewModel.Instance.DemoItemCurrent.TargetCtlName;
            if (Equals(_currentDemoKey, demoKey))
            {
                return;
            }

            _currentDemoKey = demoKey;

            if (ContentRegion.Content is FrameworkElement demoCtl)
            {
                var demoCtlTypeName = demoCtl.GetType().Name;
                var xamlPath = $"Views/{typeKey}/{demoCtlTypeName}.xaml";
                var dc = demoCtl.DataContext;
                var dcTypeName = dc.GetType().Name;
                var vmPath = !Equals(dcTypeName, demoCtlTypeName)
                    ? $"ViewModels/{dcTypeName}"
                    : xamlPath;

                _textEditor["XAML"].Text = DemoHelper.GetCode(xamlPath);
                _textEditor["C#"].Text = DemoHelper.GetCode($"{xamlPath}.cs");
                _textEditor["VM"].Text = DemoHelper.GetCode($"{vmPath}.cs");
            }
        }
    }
}
