using System.Xml.Linq;

namespace IKriv.XamlMerge
{
    public interface IFileSystem
    {
        string ReadAllText(string path);
        string[] ReadAllLines(string path);
        XDocument ReadXml(string path);
        void WriteAllText(string path, string text);
    }
}
