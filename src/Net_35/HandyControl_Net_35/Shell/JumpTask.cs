namespace Microsoft.Windows.Shell
{
    using System;
    using System.Runtime.CompilerServices;

    public class JumpTask : JumpItem
    {
        public string ApplicationPath { get; set; }

        public string Arguments { get; set; }

        public string Description { get; set; }

        public int IconResourceIndex { get; set; }

        public string IconResourcePath { get; set; }

        public string Title { get; set; }

        public string WorkingDirectory { get; set; }
    }
}

