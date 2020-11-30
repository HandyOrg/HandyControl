using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace IKriv.XamlMerge
{
    class NamespaceManager
    {
        public class NamespaceInfo
        {
            public string Prefix;
            public string Uri;
            public string OriginFilePath;
        }

        private readonly Dictionary<string, NamespaceInfo> _namespaces = new Dictionary<string, NamespaceInfo>();
        private readonly XElement _root;
        private readonly string _rootFilePath;

        public NamespaceManager(XElement element, string originFilePath)
        {
            _root = element;
            _rootFilePath = originFilePath;
            LoadNamespaces(_root, null, originFilePath, false);
        }

        public class ClashException : Exception
        {
            public ClashException(NamespaceInfo first, NamespaceInfo second)
                :
                base(GetMessage(first, second))
            {
            }

            private static string GetMessage(NamespaceInfo first, NamespaceInfo second)
            {
                return
                    $"Namespace conflict: '{first.Prefix}' is mapped to '{first.Uri}'\r\n" +
                    $"in {first.OriginFilePath}\r\n" +
                    $"and to '{second.Uri}'\r\n" +
                    $"in {second.OriginFilePath}";
            }
        }

        public void CopyNamespaces(XamlFile xaml)
        {
            LoadNamespaces(xaml.Xml.Root, xaml.Assembly, xaml.Path, true);
        }

        private void LoadNamespaces(XElement source, string assembly, string originFilePath, bool addToRoot)
        {
            var nsDeclarations = source.Attributes().Where(attr => attr.IsNamespaceDeclaration);
            var clrNamespaceHelper = new ClrNamespaceHelper();
            foreach (var declaration in nsDeclarations)
            {
                var uri = clrNamespaceHelper.NormalizeNamespaceUri(declaration.Value, assembly);
                var info = new NamespaceInfo
                {
                    Prefix = GetNsPrefix(declaration),
                    Uri = uri,
                    OriginFilePath = originFilePath
                };

                NamespaceInfo existing;
                if (_namespaces.TryGetValue(info.Prefix, out existing))
                {
                    if (existing.Uri != info.Uri)
                    {
                        throw new ClashException(existing, info);
                    }
                }
                else
                {
                    _namespaces[info.Prefix] = info;

                    if (addToRoot)
                    {
                        // cannot add new "xmlns" delaration to root element, this would change namespaces of all elements in the document
                        if (info.Prefix == "")
                        {
                            var defaultNamespace = _root.GetDefaultNamespace();
                            if (info.Uri != defaultNamespace)
                            {
                                info.Prefix = "xmlns";
                                throw new ClashException(GetDefaultNamespaceInfo(defaultNamespace.NamespaceName), info);
                            }
                        }
                        else
                        {
                            _root.Add(new XAttribute(XNamespace.Xmlns + info.Prefix, info.Uri));
                        }
                    }
                }
            }
        }

        private NamespaceInfo GetDefaultNamespaceInfo(string uri)
        {
            return new NamespaceInfo
            {
                Prefix = "xmlns",
                Uri = uri,
                OriginFilePath = _rootFilePath
            };
        }

        private static string GetNsPrefix(XAttribute nsDeclaration)
        {
            if (nsDeclaration.Name.NamespaceName == "" && nsDeclaration.Name.LocalName == "xmlns") return "";
            return nsDeclaration.Name.LocalName;
        }
    }
}
