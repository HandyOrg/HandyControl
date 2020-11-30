using System.IO;

namespace IKriv.XamlMerge
{
    class Options
    {
        public string XamlPath { get; set; }
        public string AssembliesListPath { get; set; }
        public string OutPath { get; set; }
        public bool OutputResourcesOnly { get; set; }

        public bool Parse(string[] args, TextWriter stderr)
        {
            if (args.Length < 2) return false;
            XamlPath = args[0];
            AssembliesListPath = args[1];

            for (int i = 2; i < args.Length; ++i)
            {
                switch (args[i])
                {
                    case "-resonly":
                        OutputResourcesOnly = true;
                        break;

                    case "-out":
                        ++i;
                        if (i >= args.Length)
                        {
                            stderr.WriteLine("-out option must be followed by a file name");
                            return false;
                        }
                        OutPath = args[i];
                        break;

                    default:
                        stderr.WriteLine("Invalid option " + args[i]);
                        return false;
                }
            }

            if (OutPath == null) OutPath = Path.ChangeExtension(XamlPath, ".merged.xaml");
            return true;
        }

        public void Usage(TextWriter output)
        {
            output.WriteLine("Usage: XamlMerge.exe file.xaml assemblies.txt [-out merged.xaml] [-resonly]");    
        }
    }
}
