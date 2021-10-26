using System;
using System.Collections.Generic;
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
            return GetValidFullPath("Source/Mastering/Version/Version.props");
        }

        string GetValidFullPath(string relativePath)
        {
            var versionFilePath = Path.GetFullPath(Path.Combine(repository.RootPath, relativePath));
            if (!File.Exists(versionFilePath))
                throw new InvalidOperationException($"Cannot locate {Path.GetFileName(versionFilePath)} file at {versionFilePath}");
            return versionFilePath;
        }

        public IEnumerable<string> GetTemplateProjectFiles()
        {
            var files = new[]
            {
                @"Source\Integration\VisualStudio\Templates\AlternetUIApplicationTemplate\ProjectTemplate.csproj",
                @"Source\Integration\VisualStudio\Templates\AlternetUIApplicationTemplate\ProjectTemplate.csproj",
                @"Source\Integration\Templates\CSharp\Application\Alternet.UI.Templates.Application.CSharp.csproj"
            };

            return files.Select(file => GetValidFullPath(file));
        }

        public IEnumerable<string> GetVSPackageManifestFiles()
        {
            var files = new[]
            {
                @"Source\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Manifests\VS2019\source.extension.vsixmanifest",
                @"Source\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Manifests\VS2022\source.extension.vsixmanifest",
            };

            return files.Select(file => GetValidFullPath(file));
        }
    }
}