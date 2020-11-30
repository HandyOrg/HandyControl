using System.Collections.Generic;
using System.IO;

namespace IKriv.XamlMerge
{
    class AssemblyList
    {
        private readonly Dictionary<string, string> _assemblies = new Dictionary<string, string>();
        private readonly IFileSystem _fs;

        public string Root { get; private set; }

        public AssemblyList(IFileSystem fs)
        {
            _fs = fs;
        }

        public AssemblyList Readfile(string path)
        {
            Root = Path.GetDirectoryName(path);
            return ReadText(_fs.ReadAllLines(path));
        }

        public string this[string name]
        {
            get
            {
                string result;
                if (!_assemblies.TryGetValue(name, out result)) return null;
                return result;
            }
        }

        public bool IsExternal(string name)
        {
            return name != null && this[name] == "@extern";
        }

        private AssemblyList ReadText(string[] lines)
        {
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (trimmed == "") continue;
                if (trimmed[0] == '#') continue; // commment line
                var split = trimmed.Split(new[] {'='}, 2);
                var name = split[0].Trim();
                var path = split.Length > 1 ? split[1].Trim() : name;
                _assemblies[name] = path;
            }

            var newRoot = this["@root"];
            if (newRoot != null) Root = Path.Combine(Root, newRoot);

            return this;
        }
    }
}
