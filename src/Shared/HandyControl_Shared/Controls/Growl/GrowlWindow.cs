using System;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls;

public sealed class GrowlWindow : Window
{
    internal Panel GrowlPanel { get; set; }

    internal GrowlWindow()
    {
        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;

        Growl.SetTransitionStoryboard(this, Growl.GetTransitionStoryboard(Application.Current.MainWindow));
        GrowlPanel = new ReversibleStackPanel();
        Content = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
            IsInertiaEnabled = true,
            Content = GrowlPanel
        };
    }

    internal void UpdatePosition(TransitionMode transitionMode)
    {
        var desktopWorkingArea = SystemParameters.WorkArea;
        Height = desktopWorkingArea.Height;
        Top = 0;

        var panelHorizontalAlignment = Growl.GetPanelHorizontalAlignment(transitionMode);
        Left = panelHorizontalAlignment switch
        {
            HorizontalAlignment.Right => desktopWorkingArea.Right - Width,
            HorizontalAlignment.Left => desktopWorkingArea.Left,
            HorizontalAlignment.Center => desktopWorkingArea.Left + (desktopWorkingArea.Width - Width) * 0.5,
            _ => desktopWorkingArea.Right - Width
        };

        Growl.SetTransitionMode(this, transitionMode);
        GrowlPanel.SetValue(ReversibleStackPanel.ReverseOrderProperty,
            transitionMode is TransitionMode.Bottom2Top or TransitionMode.Bottom2TopWithFade);
    }

    protected override void OnSourceInitialized(EventArgs e)
        => InteropMethods.IntDestroyMenu(this.GetHwndSource().CreateHandleRef());
}
