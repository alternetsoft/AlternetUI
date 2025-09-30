using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Path"/> related static methods.
    /// </summary>
    public static class PathUtils
    {
        private static ConcurrentStack<string>? pushedFolders;

        /// <summary>
        /// Gets whether directory is empty (contains no files or sub-folders).
        /// </summary>
        /// <param name="path">Path to directory.</param>
        /// <returns></returns>
        public static bool DirectoryIsEmpty(string path) => !DirectoryHasEntries(path);

        /// <summary>
        /// Gets the path to the desktop directory for the current user.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> containing the full path to the desktop directory.
        /// </returns>
        public static string GetDesktopPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        /// <summary>
        /// Gets the path to the user's Downloads folder.
        /// </summary>
        /// <returns>The full path to the Downloads folder as a string,
        /// or <see langword="null"/> if the path cannot be resolved.</returns>
        public static string GetDownloadsPath()
        {
            if (MswKnownFolders.TryResolvePath(MswKnownFolders.Downloads, out var path))
            {
                if (!string.IsNullOrEmpty(path))
                    return path;
            }

            string downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            return downloadsPath;
        }

        /// <summary>
        /// Generates a unique filename in the user's Downloads folder based
        /// on the specified filename template.
        /// </summary>
        /// <remarks>This method ensures that the generated filename is unique by checking for conflicts
        /// in the Downloads folder. The filename template should be designed to allow for
        /// uniqueness, such as including
        /// placeholders for numbering.</remarks>
        /// <param name="fileNameTemplate">The template for the filename, which may include
        /// placeholders or a base name.  For example, "file_{0}.txt"
        /// can be used to generate filenames like "file_1.txt", "file_2.txt", etc.</param>
        /// <returns>A unique filename within the Downloads folder that does not conflict
        /// with existing files.</returns>
        public static string GenerateUniqueFilenameInDownloads(string fileNameTemplate)
        {
            var path = GetDownloadsPath();
            var result = GenerateUniqueFilename(path, fileNameTemplate);
            return result;
        }

        /// <summary>
        /// Generates a unique file name in the specified folder based on the provided
        /// file name template.
        /// </summary>
        /// <param name="pathToFolder">The path to the folder where the file will be created.</param>
        /// <param name="fileNameTemplate">The template for the file name,
        /// including extension.</param>
        /// <returns>
        /// A <see cref="string"/> containing a unique file name in the specified folder.
        /// </returns>
        public static string GenerateUniqueFilename(string pathToFolder, string fileNameTemplate)
        {
            string filePath = Path.Combine(pathToFolder, fileNameTemplate);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileNameTemplate);
            string fileExt = Path.GetExtension(fileNameTemplate);

            int counter = 1;
            while (File.Exists(filePath))
            {
                filePath = Path.Combine(pathToFolder, $"{fileNameWithoutExt}_{counter}{fileExt}");
                counter++;
            }

            return filePath;
        }

        /// <summary>
        /// Checks if the specified path has the given file extension.
        /// </summary>
        /// <param name="path">The path to check for the extension.</param>
        /// <param name="ext">The file extension to check for.</param>
        /// <param name="comparison">The StringComparison to use for the check.</param>
        /// <returns><c>true</c> if the path has the specified extension;
        /// otherwise, <c>false</c>.</returns>
        public static bool HasExtension(
            string? path,
            string ext,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension))
                return false;
            return string.Equals(extension, ext, comparison);
        }

        /// <summary>
        /// Generates a file path on the desktop for the specified file name.
        /// </summary>
        /// <param name="fileNameWithoutPath">The name of the file without the path.</param>
        /// <param name="uniqueName">
        /// A boolean value indicating whether to generate a unique file name if
        /// a file with the same name already exists. Optional. Default is <c>false</c>.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> containing the full file path on the desktop.
        /// </returns>
        public static string GenFilePathOnDesktop(string fileNameWithoutPath, bool uniqueName = false)
        {
            string result;

            if (uniqueName)
                result = GenerateUniqueFilename(GetDesktopPath(), fileNameWithoutPath);
            else
                result = Path.Combine(GetDesktopPath(), fileNameWithoutPath);

            return result;
        }

        /// <summary>
        /// Gets whether directory is not empty (contains files or sub-folders).
        /// </summary>
        /// <param name="path">Path to directory.</param>
        /// <returns></returns>
        public static bool DirectoryHasEntries(string path)
        {
            if (!Directory.Exists(path))
                return false;
            return Directory.EnumerateFileSystemEntries(path).Any();
        }

        /// <summary>
        /// Saves current directory path to memory and changes it to the specified path.
        /// Use <see cref="PopDirectory"/> to restore saved folder.
        /// This method does the same as console command "pushd".
        /// </summary>
        /// <param name="path">New current directory path.</param>
        public static void PushDirectory(string path)
        {
            pushedFolders ??= new();
            var current = Directory.GetCurrentDirectory();
            pushedFolders.Push(current);
            Directory.SetCurrentDirectory(path);
        }

        /// <summary>
        /// Restores current directory path previously saved with <see cref="PushDirectory"/>.
        /// This method does the same as console command "popd".
        /// </summary>
        public static void PopDirectory()
        {
            pushedFolders ??= new();
            if (pushedFolders.TryPop(out var path))
                Directory.SetCurrentDirectory(path);
            else
                throw new IOException("Invalid call to 'PopDirectory' without call to 'PushDirectory'");
        }

        /// <summary>
        /// Returns an absolute path from a relative path and a fully qualified base path.
        /// </summary>
        /// <param name="path">A relative path to concatenate to basePath.</param>
        /// <param name="basePath">The beginning of a fully qualified path. Path must exist.</param>
        /// <returns></returns>
        public static string GetFullPath(string path, string basePath)
        {
#if NETSTANDARD2_1_OR_GREATER
            var result = Path.GetFullPath(path, basePath);
            return result;
#else
            try
            {
                PushDirectory(basePath);
                var result = Path.GetFullPath(path);
                return result;
            }
            finally
            {
                PopDirectory();
            }
#endif
        }

        /// <summary>
        /// Returns the extension of a file path in the lower case without leading period.
        /// </summary>
        /// <param name="path">The file path from which to get the extension.</param>
        /// <returns>
        /// The extension of the specified path (without the period, "."), or
        /// an empty string if path does not have extension information.
        /// </returns>
        public static string GetExtensionLower(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            var result = System.IO.Path.GetExtension(path).Trim('.').ToLower();
            return result;
        }

        /// <summary>
        /// Returns directory separator char which is used in given path.
        /// </summary>
        /// <param name="path">Path in which directory separator
        /// char is searched.</param>
        /// <returns><see cref="Path.DirectorySeparatorChar"/> or
        /// <see cref="Path.AltDirectorySeparatorChar"/>.</returns>
        public static char ExtractDirectorySeparatorChar(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return Path.DirectorySeparatorChar;

            if (path.Contains(Path.AltDirectorySeparatorChar))
                return Path.AltDirectorySeparatorChar;
            return Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Returns boolean value indicating whether path ends with
        /// directory separator char.
        /// </summary>
        /// <param name="path">Path which will be checked.</param>
        /// <returns><c>true</c> if path ends with directory separator char;
        /// <c>false</c> otherwise.</returns>
        public static bool EndsWithDirectorySeparator(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            int index = path!.Length - 1;
            char c = path[index];
            if (c != Path.DirectorySeparatorChar)
                return c == Path.AltDirectorySeparatorChar;
            return true;
        }

        /// <summary>
        /// Gets sub-folder path in the application folder.
        /// </summary>
        /// <param name="subFolder">Name of the sub folder.</param>
        /// <param name="create">Specifies whether to create sub-folder if it doesn't exist.</param>
        /// <remarks>
        /// Application folder is combined with the <paramref name="subFolder"/>
        /// using <see cref="Path.Combine(string, string)"/>
        /// and converted to the full path using <see cref="Path.GetFullPath(string)"/>.
        /// If result path doesn't exist, an application folder is returned.
        /// </remarks>
        /// <returns></returns>
        public static string GetAppSubFolder(string subFolder, bool create = false)
        {
            var appFolder = GetAppFolder();
            var result = Path.Combine(appFolder, subFolder);
            result = PathUtils.AddDirectorySeparatorChar(result);
            result = Path.GetFullPath(result);

            if (Directory.Exists(result))
                return result;
            else
            if (create)
            {
                Directory.CreateDirectory(result);
                return result;
            }

            return appFolder;
        }

        /// <summary>
        /// Gets path to the 'Temp' sub-folder of the application folder.
        /// </summary>
        /// <returns></returns>
        public static string GetTempAppSubFolder() => GetAppSubFolder("Temp", true);

        /// <summary>
        /// Returns path to the application folder.
        /// </summary>
        /// <returns><see cref="string"/> containing path to the application folder
        /// with directory separator char at the end.</returns>
        public static string GetAppFolder() => CommonUtils.GetAppFolder();

        /// <summary>
        /// Compares two specified paths by file name
        /// and returns an
        /// integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">First item to compare.</param>
        /// <param name="y">Second item to compare.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relationship between the two
        /// comparands. Result value less than 0 means that <paramref name="x"/> precedes
        /// <paramref name="y"/> in the sort order.
        /// Result value equal to 0 means <paramref name="x"/> occurs in the same
        /// position as <paramref name="y"/>
        /// in the sort order. Result value greater than 0 means that <paramref name="x"/> follows
        /// <paramref name="y"/> in the sort order.
        /// </returns>
        public static int CompareByFileName(string x, string y)
        {
            var xFileName = Path.GetFileName(x);
            var yFileName = Path.GetFileName(y);
            return string.Compare(xFileName, yFileName);
        }

        /// <summary>
        /// Creates file with the path and name of the executing assembly
        /// and with the specified extension.
        /// </summary>
        /// <param name="ext">Extension of the created file.</param>
        /// <param name="data">Created file data.</param>
        public static void CreateExecutingAssemblyWithNewExt(string ext, string data = "")
        {
            var s = GetExecutingAssemblyWithNewExt(ext);
            var streamWriter = File.CreateText(s);
            streamWriter.WriteLine(data);
            streamWriter.Close();
        }

        /// <summary>
        /// Gets file path and name of the executing assembly with the specified extension.
        /// </summary>
        /// <param name="ext">Extension to use.</param>
        /// <returns></returns>
        public static string GetExecutingAssemblyWithNewExt(string ext)
        {
            string sPath1 = App.ExecutingAssemblyLocation;
            string sPath2 = Path.ChangeExtension(sPath1, ext);
            return sPath2;
        }

        /// <summary>
        /// Removes file with path and name of the executing assembly with the specified extension.
        /// </summary>
        /// <param name="ext">Extension to use.</param>
        public static void RemoveExecutingAssemblyWithExt(string ext)
        {
            var s = GetExecutingAssemblyWithNewExt(ext);
            if (File.Exists(s))
                File.Delete(s);
        }

        /// <summary>
        /// Adds directory separator char to the path if it's needed. If path
        /// already has directory separator char at the end, no changes are
        /// performed.
        /// </summary>
        /// <param name="path">Path to which directory separator
        /// char will be added.</param>
        /// <returns><see cref="string"/> containing path with added
        /// directory separator char. </returns>
        /// <exception cref="ArgumentNullException">Path is null.</exception>
        public static string AddDirectorySeparatorChar(string? path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            path = path.TrimEnd();
            if (EndsWithDirectorySeparator(path))
                return path;
            return path + ExtractDirectorySeparatorChar(path);
        }
    }
}
