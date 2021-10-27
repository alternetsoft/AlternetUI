using System;
using System.IO;
using System.IO.Compression;

namespace Alternet.UI.PublicSourceGenerator.Generators
{
    class TempDirectory : IDisposable
    {
        string? path;

        public string Path
        {
            get => path ?? throw new ObjectDisposedException(GetType().FullName);
        }

        public TempDirectory(string generatorName)
        {
            path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "Alternet.UI\\PublicSourceGenerator",
                generatorName,
                Guid.NewGuid().ToString("N"));

            Directory.CreateDirectory(Path);
        }

        public void Pack(string targetArchiveFilePath)
        {
            if (File.Exists(targetArchiveFilePath))
                File.Delete(targetArchiveFilePath);

            var targetFileDirectory = System.IO.Path.GetDirectoryName(targetArchiveFilePath);
            if (!Directory.Exists(targetFileDirectory))
                Directory.CreateDirectory(targetFileDirectory);

            ZipFile.CreateFromDirectory(Path, targetArchiveFilePath, CompressionLevel.Optimal, includeBaseDirectory: false);
        }

        public void Dispose()
        {
            if (path == null)
                throw new ObjectDisposedException(GetType().FullName);

            if (Path != null && Directory.Exists(Path))
                Directory.Delete(Path, true);

            path = null;
        }
    }
}