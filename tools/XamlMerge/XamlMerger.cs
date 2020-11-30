using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace IKriv.XamlMerge
{
    class XamlMerger
    {
        private readonly XamlFile _xaml;
        private readonly AssemblyList _assemblyList;
        private readonly TextWriter _log;
        private readonly IFileSystem _fs;
        private XmlInserter _resourceInserter;
        private XmlInserter _mdInserter;
        private readonly Options _options;
        private readonly HashSet<string> _mergedDictionaries = new HashSet<string>();
        private NamespaceManager _nsManager;
        private int _warnings;
        private int _errors;

        private static readonly XNamespace Wpf = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        private static readonly XName ResourceDictionaryName = Wpf + "ResourceDictionary";
        private static readonly XName MergedDictionariesName = Wpf + "ResourceDictionary.MergedDictionaries";

        public XamlMerger(Options options, AssemblyList assemblyList, TextWriter log, IFileSystem fs)
        {
            _assemblyList = assemblyList;
            _options = options;
            _log = log;
            _fs = fs;
            _xaml = new XamlFile(fs).Read(options.XamlPath);
        }

        public bool Run()
        {
            var outputNode = ProcessMainFile();
            OutputResult(outputNode);
            OutputSummary();
            return _errors == 0;
        }

        private class XmlInserter
        {
            private XNode _insertPoint;

            public XmlInserter(XNode insertPoint)
            {
                _insertPoint = insertPoint;
            }

            public void Insert(XNode node)
            {
                _insertPoint.AddAfterSelf(node);
                _insertPoint = _insertPoint.NextNode;
            }
        }

        private class SourceUri
        {
            private readonly Regex _packRegex = new Regex("^(pack://.*)?/([^/]+);component/(.*)$");

            public readonly string Uri;
            public readonly string Assembly; // null for relative URIs
            public readonly string Path;
            public bool IsRelative => Assembly == null;

            public SourceUri(string uri)
            {
                Uri = uri;
                var match = _packRegex.Match(uri);
                if (match.Success)
                {
                    // pack URI
                    Assembly = match.Groups[2].Value;
                    Path = match.Groups[3].Value.Replace("/", "\\");
                }
                else
                {
                    // relative path
                    Path = uri;
                }
            }
        }

        private void Warning(string indent, string message)
        {
            ++_warnings;
            _log.WriteLine(indent + "WARNING: " + message);
        }

        private void Error(string indent, string message)
        {
            ++_errors;
            _log.WriteLine(indent + "ERROR: " + message);
        }

        private XNode ProcessMainFile()
        {
            _log.WriteLine(_xaml.Path);

            var rd = _xaml.Xml.Descendants(ResourceDictionaryName).FirstOrDefault();
            var outputNode = _options.OutputResourcesOnly ? (XNode) rd : _xaml.Xml;

            if (rd == null)
            {
                Warning("", "<ResourceDictionary> element not found, nothing to prosess");
                return outputNode;
            }

            var nsRoot = _options.OutputResourcesOnly ? rd : _xaml.Xml.Root;
            _nsManager = new NamespaceManager(nsRoot, _xaml.Path);
            if (nsRoot == rd) _nsManager.CopyNamespaces(_xaml);

            var firstChild = rd.Elements().FirstOrDefault();
            if (firstChild == null)
            {
                Warning("", "Found empty <ResourceDictionary> element, nothing to process");
                return outputNode;
            }

            // this is a simplification: we assume <MergedDictionaries> is always the fisrt child
            if (firstChild.Name != MergedDictionariesName || !firstChild.HasElements) 
            {
                Warning("", "No merged dictionaries found");
                return outputNode;
            }

            var mdListElement = firstChild;
            _resourceInserter = new XmlInserter(mdListElement); // merged resources go here
            _mdInserter = new XmlInserter(mdListElement.LastNode); // external merged dictionaries go here

            ProcessMergedDictionariesElement(_xaml, mdListElement, " ");
            var mdCount = mdListElement.Elements().Count();
            if (mdCount == 0)
            {
                mdListElement.Remove();
            }

            return outputNode;
        }

        private bool IsKnownAssembly(string assembly)
        {
            return assembly == null || _assemblyList[assembly] != null;
        }

        private void ProcessMergedDictionariesElement(XamlFile xamlFile, XElement list, string indent)
        {
            int nMergedDictionary = 0;
            var children = list.Elements().ToArray();

            foreach (var child in children)
            {
                ++nMergedDictionary;
                _log.Write(indent + nMergedDictionary + ": ");

                var source = child.Attribute("Source")?.Value.Trim();
                if (source == null)
                {
                    MergeCustomDictionary(child);
                }
                else
                {
                    _log.Write("'" + source + "' => ");
                    var sourceUri = new SourceUri(source);

                    if (!IsKnownAssembly(sourceUri.Assembly))
                    {
                        _log.WriteLine("UNKNOWN PATH");
                        Error(indent,
                            $"Assembly {sourceUri.Assembly} is not listed. Please specify its path in the assembly list or add line {sourceUri.Assembly}=@xternal");
                        continue;
                    }

                    MergeDictionary(xamlFile, child, sourceUri, indent);
                }
                child.Remove();
            }
        }

        private void MergeCustomDictionary(XElement mdElement)
        {
            _log.WriteLine($"{mdElement.Name} has no Source attribute, including verbatim");
            _mdInserter.Insert(mdElement);
        }

        private void MergeDictionary(XamlFile parentXaml, XNode mdElement, SourceUri source, string indent)
        {
            if (_assemblyList.IsExternal(source.Assembly))
            {
                // external assembly; add this merged dictionary to the md list of the root file
                _log.WriteLine("external dictionary");

                if (IsAlreadyMerged(source.Uri, indent)) return;
                _mdInserter.Insert(mdElement);
            }
            else
            {
                // known assembly; read XAML file and copy its contents to the root resource dictionary
                var path = Path.GetFullPath(GetPathFromSourceUri(parentXaml.Path, source));
                _log.WriteLine(path);

                if (IsAlreadyMerged(path, indent)) return;
                var assembly = source.Assembly ?? parentXaml.Assembly;
                var mergedXaml = new XamlFile(_fs, assembly).Read(path);
                MergeXamlFile(mergedXaml, indent + " ");
            }
        }

        private bool IsAlreadyMerged(string uri, string indent)
        {
            if (_mergedDictionaries.Contains(uri))
            {
                _log.WriteLine(indent + " Dictionary already merged: " + uri);
                return true;
            }
            _mergedDictionaries.Add(uri);
            return false;
        }

        private void MergeXamlFile(XamlFile mergedXaml, string indent)
        {
            _log.WriteLine(indent + mergedXaml.Path);
            try
            {
                var documentRoot = mergedXaml.Xml.Root;
                if (documentRoot == null)
                {
                    Warning(indent, "No root element");
                    return;
                }

                if (documentRoot.Name != ResourceDictionaryName)
                {
                    Error(indent, $"Root element is not resource dictionarty: <{documentRoot.Name.LocalName}>");
                    return;
                }

                var firstChild = documentRoot.Elements().FirstOrDefault();
                if (firstChild == null)
                {
                    Warning(indent, "No child elements. Empty dictionary?");
                    return;
                }

                _nsManager.CopyNamespaces(mergedXaml);

                XNode toMerge = firstChild;

                if (firstChild.Name == MergedDictionariesName)
                {
                    ProcessMergedDictionariesElement(mergedXaml, firstChild, indent + " "); // recursive call
                    toMerge = firstChild.NextNode;
                }

                // merge content of the file into the root file
                if (toMerge != null)
                {
                    _resourceInserter.Insert(new XComment(" Merged from " + mergedXaml.Path + " "));

                    while (toMerge != null)
                    {
                        _resourceInserter.Insert(toMerge);
                        toMerge = toMerge.NextNode;
                    }

                    _resourceInserter.Insert(new XComment(" End merged from " + mergedXaml.Path + " "));
                }
            }
            catch (NamespaceManager.ClashException ex)
            {
                Error(indent, ex.Message);
            }
        }

        private string GetPathFromSourceUri(string xamlFilePath, SourceUri sourceUri)
        {
            if (sourceUri.IsRelative)
            {
                var dir = Path.GetDirectoryName(xamlFilePath) ?? "";
                return Path.Combine(dir, sourceUri.Path);
            }
            else
            {
                var assemblyPath = _assemblyList[sourceUri.Assembly];
                if (assemblyPath == null) throw new InvalidOperationException("Unknown assembly " + sourceUri.Assembly);
                return Path.Combine(_assemblyList.Root, assemblyPath, sourceUri.Path);
            }
        }

        private void OutputSummary()
        {
            _log.WriteLine($"{_errors} error(s)");
            _log.WriteLine($"{_warnings} warning(s)");
        }

        private void OutputResult(XNode node)
        {
            if (node == null)
            {
                Error("", "The output is empty");
                return;
            }

            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                IndentChars = "  ",
                NamespaceHandling = NamespaceHandling.OmitDuplicates
            };

            // quite expensive way to insert newline before each xmlns, but it works
            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, settings))
                {
                    node.WriteTo(writer);
                }

                var content = AddNewLineAfterXmlns(sw.ToString());
                _fs.WriteAllText(_options.OutPath, content);
            }
        }

        private static string AddNewLineAfterXmlns(string input)
        {
            var xmlElement = new Regex("<[^>]+>");
            var xmlns = new Regex("(?<decl>xmlns(:[\\w\\d]+)?=('[^']*'|\"[^\"]*\"))\\s+");

            return xmlElement.Replace(input, 
                m => xmlns.Replace(m.Value, decl=>decl.Groups["decl"].Value + Environment.NewLine),
                1);
        }
    }
}
