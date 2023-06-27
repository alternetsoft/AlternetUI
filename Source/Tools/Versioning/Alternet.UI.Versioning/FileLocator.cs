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
            return GetValidFullFilePath("Source/Version/Version.props");
        }

        string GetValidFullFilePath(string relativePath)
        {
            var path = Path.GetFullPath(Path.Combine(repository.RootPath, relativePath));
            if (!File.Exists(path))
                throw new InvalidOperationException($"Cannot locate {Path.GetFileName(path)} file at {path}");
            return path;
        }

        string GetValidFullDirectoryPath(string relativePath)
        {
            var path = Path.GetFullPath(Path.Combine(repository.RootPath, relativePath));
            if (!Directory.Exists(path))
                throw new InvalidOperationException($"Cannot locate {Path.GetFileName(path)} directory at {path}");
            return path;
        }

        public IEnumerable<string> GetTemplateProjectFiles()
        {
            var files = new[]
            {
                "Source/Integration/VisualStudio/Templates/AlternetUIApplicationTemplate.VS2022/ProjectTemplate.csproj",
                "Source/Integration/Templates/CSharp/Application/Alternet.UI.Templates.Application.CSharp.csproj"
            };

            return files.Select(file => GetValidFullFilePath(file));
        }

        public IEnumerable<string> GetDocumentationExamplesProjectFiles()
        {
            return Directory.GetFiles(
                GetValidFullDirectoryPath(@"Documentation\Alternet.UI.Documentation"),
                "*.Examples.*.csproj",
                SearchOption.AllDirectories);
        }

        public IEnumerable<string> GetVSPackageManifestFiles()
        {
            var files = new[]
            {
                "Source/Integration/VisualStudio/Alternet.UI.Integration.VisualStudio/Manifests/VS2022/source.extension.vsixmanifest",
            };

            return files.Select(file => GetValidFullFilePath(file));
        }
    }
}