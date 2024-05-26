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
#if NET6_0_OR_GREATER
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
        /// <remarks>
        /// Application folder is combined with the <paramref name="subFolder"/>
        /// using <see cref="Path.Combine(string, string)"/>
        /// and converted to the full path using <see cref="Path.GetFullPath(string)"/>.
        /// If result path doesn't exist, an application folder is returned.
        /// </remarks>
        /// <returns></returns>
        public static string GetAppSubFolder(string subFolder)
        {
            var appFolder = GetAppFolder();
            var result = Path.Combine(appFolder, subFolder);
            result = PathUtils.AddDirectorySeparatorChar(result);
            result = Path.GetFullPath(result);

            if(Directory.Exists(result))
                return result;

            return appFolder;
        }

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
