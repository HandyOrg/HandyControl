using System.Diagnostics;
using System.Reflection;

namespace HandyControlDemo.Tools;

internal class VersionHelper
{
    internal static string GetVersion()
    {
        var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
#if NET40
        var netVersion = ".NET Framework 4.0";
#elif NET45
        var netVersion = ".NET Framework 4.5";
#elif NET451
        var netVersion = ".NET Framework 4.5.1";
#elif NET452
        var netVersion = ".NET Framework 4.5.2";
#elif NET46
        var netVersion = ".NET Framework 4.6";
#elif NET461
        var netVersion = ".NET Framework 4.6.1";
#elif NET462
        var netVersion = ".NET Framework 4.6.2";
#elif NET47
        var netVersion = ".NET Framework 4.7";
#elif NET471
        var netVersion = ".NET Framework 4.7.1";
#elif NET472
        var netVersion = ".NET Framework 4.7.2";
#elif NET48
        var netVersion = ".NET Framework 4.8";
#elif NET481
        var netVersion = ".NET Framework 4.8.1";
#elif NET5_0
        var netVersion = ".NET 5.0";
#elif NET6_0
        var netVersion = ".NET 6.0";
#elif NET7_0
        var netVersion = ".NET 7.0";
#elif NET8_0
        var netVersion = ".NET 8.0";
#elif NETCOREAPP3_0
        var netVersion = ".NET CORE 3.0";
#elif NETCOREAPP3_1
        var netVersion = ".NET CORE 3.1";
#endif
        return $"v{versionInfo.FileVersion} {netVersion}";
    }

    internal static string GetCopyright() =>
        FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;
}
