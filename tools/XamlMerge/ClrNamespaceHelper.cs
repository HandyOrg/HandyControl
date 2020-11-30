
using System.Text.RegularExpressions;

namespace IKriv.XamlMerge
{
    class ClrNamespaceHelper
    {
        private static readonly Regex ElementWithClrNamespace =
            new Regex("<[^>]+xmlns:[^'\"]+=\\s*(\"clr-namespace:[^\"]+\"|'clr-namespace:[^']+')[^>]*>");

        private static readonly Regex ClrNamespace =
            new Regex("xmlns:(?<Prefix>\\w+?)=['\"](?<Namespace>clr-namespace:[^\\s;'\"]+)['\"]");

        /// <summary>
        /// Adds ";assembly=" to all clr namespaces in the xml that don't have assembly specifieid
        /// </summary>
        public string NormalizeLocalNamespaces(string xaml, string assembly)
        {
            var replaced = ElementWithClrNamespace.Replace(xaml,
                match => ClrNamespace.Replace(match.Value,
                    "xmlns:${Prefix}=\"${Namespace};assembly=" + assembly + "\""));

            return replaced;
        }

        /// <summary>
        /// Adds ";assembly=" declaration to Uri if it is a clr-namespace URI and does not have the assembly part already
        /// </summary>
        public string NormalizeNamespaceUri(string uri, string assembly)
        {
            if (assembly == null) return uri;
            if (!uri.StartsWith("clr-namespace")) return uri;
            if (uri.Contains(";assembly=")) return uri;
            return uri + ";assembly=" + assembly;
        }
    }
}
