using Alternet.UI;
using Alternet.UI.Versioning;
using System;
using System.IO;

namespace Alternet.UI.VersionTool
{
    static class RepositoryLocator
    {
        public static Repository GetRepository()
        {
            var asmLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var repoRoot = Path.Combine(
                Path.GetDirectoryName(asmLocation) ?? throw new Exception(),
                "../../../../../../../");

            repoRoot = Path.GetFullPath(repoRoot);
            return new Repository(repoRoot);
        }
    }
}
