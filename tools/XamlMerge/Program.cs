using System;

namespace IKriv.XamlMerge
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                return Go(args);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }
        }

        private static int Go(string[] args)
        {
            var options = new Options();
            if (!options.Parse(args, Console.Error))
            {
                options.Usage(Console.Error);
                return 2;
            }

            var fs = new FileSystem();
            var assemblyList = new AssemblyList(fs).Readfile(options.AssembliesListPath);
            var merger = new XamlMerger(options, assemblyList, Console.Out, fs);
            return merger.Run() ? 0 : 3;
        }
    }
}
