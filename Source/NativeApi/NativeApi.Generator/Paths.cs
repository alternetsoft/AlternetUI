using System.IO;

namespace ApiGenerator
{
    internal class Paths
    {
        public Paths(string repoRootPath)
        {
            RepoRootPath = repoRootPath;

            NativeSourcePath = Path.Combine(RepoRootPath, @"Source\Alternet.UI.Pal");
            NativeApiSourcePath = Path.Combine(NativeSourcePath, "Api");
            ManagedApiSourcePath = Path.Combine(RepoRootPath, @"Source\Alternet.UI\Native");
            ManagedServerApiSourcePath = Path.Combine(ManagedApiSourcePath, @"ManagedServers");
        }

        public string RepoRootPath { get; }

        public string NativeSourcePath { get; }

        public string NativeApiSourcePath { get; }

        public string ManagedApiSourcePath { get; }

        public string ManagedServerApiSourcePath { get; }
    }
}