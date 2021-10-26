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
            var repoRoot = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? throw new Exception(),
                "../../../../../../../");
            return new Repository(repoRoot);
        }
    }
}
