using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using HandyControlDemo.Data;
using HandyControlDemo.ViewModel;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;


namespace HandyControlDemo.UserControl
{
    /// <summary>
    ///     主内容
    /// </summary>
    public partial class MainContent
    {
        private bool _isFull;

        private string _currentDemoKey;

        private bool _drawerCodeUsed;

        private Dictionary<string, TextEditor> _textEditor;

        public MainContent()
        {
            InitializeComponent();

            Messenger.Default.Register<bool>(this, MessageToken.FullSwitch, FullSwitch);
        }

        private void FullSwitch(bool isFull)
        {
            if (_isFull == isFull) return;
            _isFull = isFull;
            if (_isFull)
            {
                BorderEffect.Collapse();
                BorderTitle.Collapse();
                GridMain.HorizontalAlignment = HorizontalAlignment.Stretch;
                GridMain.VerticalAlignment = VerticalAlignment.Stretch;
                GridMain.Margin = new Thickness();
                PresenterMain.Margin = new Thickness();
            }
            else
            {
                BorderEffect.Show();
                BorderTitle.Show();
                GridMain.HorizontalAlignment = HorizontalAlignment.Center;
                GridMain.VerticalAlignment = VerticalAlignment.Center;
                GridMain.Margin = new Thickness(16);
                PresenterMain.Margin = new Thickness(0, 0, 0, 10);
            }
        }

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

            var typeKey = ViewModelLocator.Instance.Main.DemoInfoCurrent.Key;
            var demoKey = ViewModelLocator.Instance.Main.DemoItemCurrent.TargetCtlName;
            if (Equals(_currentDemoKey, demoKey)) return;
            _currentDemoKey = demoKey;

            if (ViewModelLocator.Instance.Main.SubContent is FrameworkElement demoCtl)
            {
                var demoCtlTypeName = demoCtl.GetType().Name;
                var xamlPath = $"UserControl/{typeKey}/{demoCtlTypeName}.xaml";
                var dc = demoCtl.DataContext;
                var dcTypeName = dc.GetType().Name;
                var vmPath = !Equals(dcTypeName, demoCtlTypeName)
                    ? $"ViewModel/{dcTypeName}"
                    : xamlPath;

                _textEditor["XAML"].Text = DemoHelper.GetCode(xamlPath);
                _textEditor["C#"].Text = DemoHelper.GetCode($"{xamlPath}.cs");
                _textEditor["VM"].Text = DemoHelper.GetCode($"{vmPath}.cs");
            }
        }
    }
}