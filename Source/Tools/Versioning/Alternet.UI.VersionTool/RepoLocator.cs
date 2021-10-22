using Alternet.UI;
using System;
using System.IO;

namespace Alternet.UI.VersionTool
{
    static class RepoLocator
    {
        public static string GetRepoRootPath()
        {
            var repoRoot = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? throw new Exception(),
                "../../../../../../../");
            return repoRoot;
        }
    }
}
