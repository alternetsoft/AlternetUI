using System.IO;

namespace ApiGenerator
{
    internal class Paths
    {
        public Paths(string repoRootPath)
        {
            RepoRootPath = repoRootPath;

            NativeSourcePath = Path.Combine(RepoRootPath, @"Source\Alternet.UI.Pal\Api");
            ManagedSourcePath = Path.Combine(RepoRootPath, @"Source\Alternet.UI\Native");
        }

        public string RepoRootPath { get; }

        public string NativeSourcePath { get; }

        public string ManagedSourcePath { get; }
    }
}