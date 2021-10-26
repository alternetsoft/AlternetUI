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
            var versionFilePath = Path.GetFullPath(Path.Combine(repository.RootPath, "Source/Mastering/Version/Version.props"));
            if (!File.Exists(versionFilePath))
                throw new InvalidOperationException("Cannot locate Version.props file at" + versionFilePath);
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

            foreach (var file in files)
            {
                var filePath = Path.GetFullPath(Path.Combine(repository.RootPath, file));
                if (!File.Exists(filePath))
                    throw new InvalidOperationException("Cannot locate file at" + filePath);
                yield return filePath;
            }
        }
    }
}