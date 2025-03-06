using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
#if !NET40
using System.Runtime;
#endif
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControlDemo.Data;
using HandyControlDemo.Properties.Langs;
using HandyControlDemo.Tools;

namespace HandyControlDemo;

public partial class App
{
#pragma warning disable IDE0052
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    private static Mutex AppMutex;
#pragma warning restore IDE0052

    public App()
    {
        EnsureProfileOptimization();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        EnsureSingleton();
        OpenSplashScreen();

        base.OnStartup(e);

        //UpdateRegistry();
        ApplyConfiguration();
        UpdateApp();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        GlobalData.Save();
    }

    internal void UpdateSkin(SkinType skin)
    {
        var skins0 = Resources.MergedDictionaries[0];
        skins0.MergedDictionaries.Clear();
        skins0.MergedDictionaries.Add(ResourceHelper.GetSkin(skin));
        skins0.MergedDictionaries.Add(ResourceHelper.GetSkin(typeof(App).Assembly, "Resources/Themes", skin));

        var skins1 = Resources.MergedDictionaries[1];
        skins1.MergedDictionaries.Clear();
        skins1.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        });
        skins1.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/HandyControlDemo;component/Resources/Themes/Theme.xaml")
        });

        Current.MainWindow?.OnApplyTemplate();
    }

    private void ApplyConfiguration()
    {
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        GlobalData.Init();
        ConfigHelper.Instance.SetLang(GlobalData.Config.Lang);
        LangProvider.Culture = new CultureInfo(GlobalData.Config.Lang);

        if (GlobalData.Config.Skin != SkinType.Default)
        {
            UpdateSkin(GlobalData.Config.Skin);
        }

        ConfigHelper.Instance.SetWindowDefaultStyle();
        ConfigHelper.Instance.SetNavigationWindowDefaultStyle();

#if NET40
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
#else
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif
    }

    private static void OpenSplashScreen()
    {
        var splashScreen = new SplashScreen("Resources/Img/Cover.png");
        splashScreen.Show(true);
    }

    private static void EnsureProfileOptimization()
    {
#if !NET40
        var cachePath = $"{AppDomain.CurrentDomain.BaseDirectory}Cache";
        if (!Directory.Exists(cachePath))
        {
            Directory.CreateDirectory(cachePath);
        }
        ProfileOptimization.SetProfileRoot(cachePath);
        ProfileOptimization.StartProfile("Profile");
#endif
    }

    private void EnsureSingleton()
    {
        AppMutex = new Mutex(true, "HandyControlDemo", out var createdNew);

        if (createdNew)
        {
            return;
        }

        var current = Process.GetCurrentProcess();

        foreach (var process in Process.GetProcessesByName(current.ProcessName))
        {
            if (process.Id != current.Id)
            {
                Win32Helper.SetForegroundWindow(process.MainWindowHandle);
                break;
            }
        }

        Shutdown();
    }

    private static void UpdateRegistry()
    {
        var processModule = Process.GetCurrentProcess().MainModule;
        if (processModule != null)
        {
            var registryFilePath = $"{Path.GetDirectoryName(processModule.FileName)}\\Registry.reg";
            if (!File.Exists(registryFilePath))
            {
                var streamResourceInfo = GetResourceStream(new Uri("pack://application:,,,/Resources/Registry.txt"));
                if (streamResourceInfo != null)
                {
                    using var reader = new StreamReader(streamResourceInfo.Stream);
                    var registryStr = reader.ReadToEnd();
                    var newRegistryStr = registryStr.Replace("#", processModule.FileName.Replace("\\", "\\\\"));
                    File.WriteAllText(registryFilePath, newRegistryStr);
                    Process.Start(new ProcessStartInfo("cmd", $"/c {registryFilePath}")
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                }
            }
        }
    }

    private static void UpdateApp()
    {
        const string api = "https://github.com/handyorg/handycontrol/releases/latest";

        try
        {
            var mainDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
            var updateExePath = Path.Combine(mainDirectory, "Update.exe");

            if (File.Exists(updateExePath))
            {
                Task.Factory.StartNew(() => Process.Start(updateExePath, $"--update={api}"));
            }
        }
        catch
        {
            // ignored
        }
    }
}
