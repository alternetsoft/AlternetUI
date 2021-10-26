using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Alternet.UI.Versioning
{
    internal class FileLocator
    {
        Repository repository;

        public FileLocator(Repository repository)
        {
            this.repository = repository;
        }

        public string GetMasterVersionFile()
        {
            var versionFilePath = Path.GetFullPath(Path.Combine(repository.RootPath, "Source/Mastering/Version/Version.props"));
            if (!File.Exists(versionFilePath))
                throw new InvalidOperationException("Cannot locate Version.props file at" + versionFilePath);
            return versionFilePath;
        }
    }
}