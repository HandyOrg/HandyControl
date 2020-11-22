using System.Diagnostics;
using System.Reflection;

namespace HandyControlDemo.Tools
{
    internal class VersionHelper
    {
        internal static string GetVersion()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            var netVersion = "NET 50";
            return $"v {versionInfo.FileVersion} {netVersion}";
        }

        internal static string GetCopyright() =>
            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;
    }
}