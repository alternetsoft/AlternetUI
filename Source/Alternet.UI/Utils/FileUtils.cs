using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to files.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Returns the full file name that match a search pattern in a specified path,
        /// and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search.
        /// This string is not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of files in path.
        /// This parameter can contain a combination of valid literal path and
        /// wildcard (* and ?) characters. It doesn't support regular expressions.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or should include all
        /// subdirectories. The default value is <see cref="SearchOption.TopDirectoryOnly"/>.
        /// </param>
        /// <returns>
        /// The full name (including path) for the first found file in the
        /// directory specified by <paramref name="path"/> and that match the
        /// specified search pattern and search option. Returns <c>null</c> if no file was found.
        /// </returns>
        public static string? FindFirstFile(
            string path,
            string searchPattern,
            SearchOption searchOption)
        {
            var result = Directory.EnumerateFiles(
                path,
                searchPattern,
                searchOption).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Returns the full file name that match a search pattern in the application folder.
        /// Searches in all subdirectories.
        /// </summary>
        /// <param name="searchPattern">
        /// The search string to match against the names of files in path.
        /// This parameter can contain a combination of valid literal path and
        /// wildcard (* and ?) characters. It doesn't support regular expressions.
        /// </param>
        /// <returns>
        /// The full name (including path) for the first found file that match the
        /// specified search pattern. Returns <c>null</c> if no file was found.
        /// </returns>
        public static string? FindFileRecursiveInAppFolder(string searchPattern)
        {
            var result = FindFirstFile(
                CommonUtils.GetAppFolder(),
                searchPattern,
                SearchOption.AllDirectories);
            return result;
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
    }
}
