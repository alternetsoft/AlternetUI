using System;
using System.IO;
using System.Threading;

namespace Alternet.UI.PublicSourceGenerator.Utils
{
    static class FileSystemHelper
    {
        public static void RecreateDirectory(string path, bool doNotDelete = false)
        {
            if (Directory.Exists(path) && !doNotDelete)
                DeleteDirectory(path);

            Directory.CreateDirectory(path);
            var directoryInfo = new DirectoryInfo(path);
            directoryInfo.Attributes = directoryInfo.Attributes | FileAttributes.NotContentIndexed;
        }

        public static void DeleteDirectory(string path)
        {
            Exception? exception = null;

            for (int i = 0; i < 1; i++)
            {
                try
                {
                    Directory.Delete(path, true);
                    return;
                }
                catch (Exception e)
                {
                    exception = e;
                    Thread.Sleep(500);
                }
            }

            throw new Exception(
                string.Format("Unable to delete directory '{0}'", path),
                exception);
        }

        public static string GetRelativePath(string filePath, string directoryPath)
        {
            Uri pathUri = new Uri(filePath);
            // Folders must end in a slash
            if (!directoryPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                directoryPath += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(directoryPath);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static void CopyFiles(string sourceDirectoryPath, string targetDirectoryPath)
        {
            CopyFilesByNameEndings(sourceDirectoryPath, targetDirectoryPath, null, null);
        }

        public static void CopyFilesByNameEndings(
            string sourceDirectoryPath,
            string targetDirectoryPath,
            string[]? allowedFileNameEndings,
            string[]? prohibitedFileNameEndings,
            Func<string, string>? postProcessOutputFilePathFunc = null)
        {
            RecreateDirectory(targetDirectoryPath, true);

            var copier = new DirectoryCopier
            {
                SourceDirectoryPath = sourceDirectoryPath,
                TargetDirectoryPath = targetDirectoryPath,
                PostProcessOutputFilePathFunc = postProcessOutputFilePathFunc,
                FileFilter = (fi) =>
                {
                    if (prohibitedFileNameEndings != null)
                    {
                        foreach (var ending in prohibitedFileNameEndings)
                        {
                            if (fi.Name.EndsWith(ending, StringComparison.OrdinalIgnoreCase))
                                return false;
                        }
                    }

                    if (allowedFileNameEndings == null)
                        return true;

                    foreach (var ending in allowedFileNameEndings)
                    {
                        if (fi.Name.EndsWith(ending, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }

                    return false;
                }
            };

            copier.Run();
        }

        public static void CopyFilesByNameBeginnings(
            string sourceDirectoryPath,
            string targetDirectoryPath,
            string[] allowedFileNameBeginnings,
            string[] prohibitedFileNameBeginnings)
        {
            RecreateDirectory(targetDirectoryPath, true);

            var copier = new DirectoryCopier
            {
                SourceDirectoryPath = sourceDirectoryPath,
                TargetDirectoryPath = targetDirectoryPath,
                FileFilter = (fi) =>
                {
                    if (prohibitedFileNameBeginnings != null)
                    {
                        foreach (var ending in prohibitedFileNameBeginnings)
                        {
                            if (fi.Name.StartsWith(ending, StringComparison.OrdinalIgnoreCase))
                                return false;
                        }
                    }

                    if (allowedFileNameBeginnings == null)
                        return true;

                    foreach (var ending in allowedFileNameBeginnings)
                    {
                        if (fi.Name.StartsWith(ending, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }

                    return false;
                }
            };

            copier.Run();
        }
    }
}
