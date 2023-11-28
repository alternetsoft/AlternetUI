﻿using System;
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
        /// Deletes all files in the 'bin' and 'obj' subfolders of the specified solution folder.
        /// </summary>
        /// <param name="path">Path to folder.</param>
        /// <remarks>
        /// This method works recursively. First it finds all '*.csproj' and '*.vcxproj' project files.
        /// For the each found project file it deletes all files in the 'bin' and 'obj' subfolders
        /// located in the project file path.
        /// </remarks>
        public static void DeleteBinObjFiles(string path)
        {
            path = Path.GetFullPath(path);

            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.EndsWith(".csproj") || s.EndsWith(".vcxproj"));

            foreach (string projFile in files)
            {
                var projPath = Path.GetDirectoryName(projFile);

                var projPathBin = Path.Combine(projPath!, "bin");
                var projPathObj = Path.Combine(projPath!, "obj");
                var filesToDelete = new List<string>();

                if (Directory.Exists(projPathBin))
                {
                    var projPathBinFiles =
                        Directory.EnumerateFiles(projPathBin, "*.*", SearchOption.AllDirectories);
                    filesToDelete.AddRange(projPathBinFiles);
                }

                if (Directory.Exists(projPathObj))
                {
                    var projPathObjFiles =
                        Directory.EnumerateFiles(projPathObj, "*.*", SearchOption.AllDirectories);
                    filesToDelete.AddRange(projPathObjFiles);
                }

                foreach (var s in filesToDelete)
                {
                    Application.Log("Deleting file: " + s);
                    try
                    {
                        File.Delete(s);
                    }
                    catch (Exception)
                    {
                        Application.Log("ERROR deleting file: " + s);
                    }
                }
            }
        }

        /// <summary>
        /// Removes a <see cref="Path.DirectorySeparatorChar"/> and
        /// <see cref="Path.AltDirectorySeparatorChar"/> characters from the end
        /// of the specified string.
        /// </summary>
        /// <param name="path">Path to be trimmed.</param>
        /// <returns>Trimmed string.</returns>
        public static string? TrimEndDirectorySeparator(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            var s = path?.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return s;
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
        public static string GetAppFolder()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string s = Path.GetDirectoryName(location)!;
            return PathUtils.AddDirectorySeparatorChar(s);
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
