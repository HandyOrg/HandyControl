using System;
using System.Xml.Linq;

namespace IKriv.XamlMerge
{
    class XamlFile
    {
        private readonly IFileSystem _fs;

        public XamlFile(IFileSystem fs, string assembly = null)
        {
            _fs = fs;
            Assembly = assembly;
        }

        public string Path { get; private set; }
        public XDocument Xml { get; private set; }
        public string Assembly { get; private set; }

        public XamlFile Read(string path)
        {
            try
            {
                Path = path;

                if (Assembly == null)
                {
                    Xml = _fs.ReadXml(path);
                }
                else
                {
                    var content = _fs.ReadAllText(path);
                    var normalized = new ClrNamespaceHelper().NormalizeLocalNamespaces(content, Assembly);
                    Xml = XDocument.Parse(normalized);
                }

                return this;

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error reading XAML from '{path}': {ex.Message}", ex);
            }
        }
    }
}
