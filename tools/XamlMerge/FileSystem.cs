using System.IO;
using System.Xml.Linq;

namespace IKriv.XamlMerge
{
    class FileSystem : IFileSystem
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public XDocument ReadXml(string path)
        {
            return XDocument.Load(path);
        }

        public void WriteAllText(string path, string text)
        {
            File.WriteAllText(path, text);
        }
    }
}
