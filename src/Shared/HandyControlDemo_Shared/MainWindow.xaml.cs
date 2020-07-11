using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using HandyControl.Tools;
using HandyControlDemo.Data;
using HandyControlDemo.Tools;
using HandyControlDemo.UserControl;
using HandyControlDemo.ViewModel;

namespace HandyControlDemo
{
    public partial class MainWindow
    {
        public MainWindow() => InitializeComponent();

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            DataContext = ViewModelLocator.Instance.Main;
            NonClientAreaContent = new NoUserContent();
            ControlMain.Content = new MainWindowContent();

            GlobalShortcut.Init(new List<KeyBinding>
            {
                new KeyBinding(ViewModelLocator.Instance.Main.GlobalShortcutInfoCmd, Key.I, ModifierKeys.Control | ModifierKeys.Alt),
                new KeyBinding(ViewModelLocator.Instance.Main.GlobalShortcutWarningCmd, Key.E, ModifierKeys.Control | ModifierKeys.Alt)
            });

            Dialog.SetToken(this, MessageToken.MainWindow);
            WindowAttach.SetIgnoreAltF4(this, true);

            Messenger.Default.Send(true, MessageToken.FullSwitch);
            Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{MessageToken.PracticalDemo}"), MessageToken.LoadShowContent);
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
    }
}