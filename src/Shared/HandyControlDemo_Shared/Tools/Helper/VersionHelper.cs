using System.Diagnostics;
using System.Reflection;

namespace HandyControlDemo.Tools
{
    internal class VersionHelper
    {
        internal static string GetVersion()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
#if NET40
            var netVersion = "NET 40";
#elif NET45
            var netVersion = "NET 45";
#elif NET451
            var netVersion = "NET 451";
#elif NET452
            var netVersion = "NET 452";
#elif NET46
            var netVersion = "NET 46";
#elif NET461
            var netVersion = "NET 461";
#elif NET462
            var netVersion = "NET 462";
#elif NET47
            var netVersion = "NET 47";
#elif NET471
            var netVersion = "NET 471";
#elif NET472
            var netVersion = "NET 472";
#elif NET48
            var netVersion = "NET 48";
#elif NET5_0
            var netVersion = "NET 50";
#elif NETCOREAPP3_0
            var netVersion = "CORE 30";
#elif NETCOREAPP3_1
            var netVersion = "CORE 31";
#endif
            return $"v {versionInfo.FileVersion} {netVersion}";
        }

        internal static string GetCopyright() =>
            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;
    }
}
