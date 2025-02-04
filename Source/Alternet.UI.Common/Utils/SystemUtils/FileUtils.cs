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
        /// Returns <paramref name="path"/> if such directory exists;
        /// otherwise returns Null.
        /// </summary>
        /// <param name="path">Path to return if it exists.</param>
        /// <returns></returns>
        public static string? ExistingDirOrNull(string? path)
        {
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
                return path;
            return null;
        }

        /// <summary>
        /// Saves string to the specified file only if its contents is different from the saved string.
        /// </summary>
        /// <param name="path">Path to file where string will be saved.</param>
        /// <param name="s">String to save.</param>
        /// <param name="encoding">Encoding to use for saving the string. Optional. If not specified,
        /// <see cref="Encoding.UTF8"/> is used.</param>
        public static void StringToFileIfChanged(string path, string s, Encoding? encoding = null)
        {
            if (!File.Exists(path))
            {
                StringToFile(path, s, encoding);
                return;
            }

            MemoryStream memoryStream = new();
            StreamUtils.StringToStream(memoryStream, s, encoding);
            memoryStream.Seek(0, SeekOrigin.Begin);

            long fileLength = new System.IO.FileInfo(path).Length;
            long streamLength = memoryStream.Length;

            if(fileLength != streamLength)
            {
                DoSave();
                return;
            }

            using var fileStream = File.OpenRead(path);

            var areEqual = StreamUtils.AreEqual(memoryStream, fileStream);

            if (!areEqual)
                DoSave();

            void DoSave()
            {
                File.Delete(path);
                StreamUtils.CopyStream(memoryStream, path);
            }
        }

        /// <summary>
        /// Reads <see cref="string"/> from the file using the specified encoding.
        /// </summary>
        /// <param name="path">Path to file from which string will be loaded.</param>
        /// <param name="encoding">Stream encoding. Optional. If not specified,
        /// <see cref="Encoding.UTF8"/> is used.</param>
        public static string StringFromFile(string path, Encoding? encoding = null)
        {
            using var stream = File.OpenRead(path);
            var result = StreamUtils.StringFromStream(stream, encoding);
            return result;
        }

        /// <summary>
        /// Saves string to file. If file with such name already exists,
        /// it is deleted before saving the string.
        /// </summary>
        /// <param name="path">Path to file where string will be saved.</param>
        /// <param name="s">String to save.</param>
        /// <param name="encoding">Encoding to use for saving the string. Optional. If not specified,
        /// <see cref="Encoding.UTF8"/> is used.</param>
        public static void StringToFile(string path, string s, Encoding? encoding = null)
        {
            File.Delete(path);
            using var stream = File.Create(path);
            StreamUtils.StringToStream(stream, s, encoding);
            stream.Close();
        }

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
        /// Gets whether the specified file name has the same char case as file on disk.
        /// </summary>
        /// <param name="fileName">Path to file.</param>
        /// <returns></returns>
        public static bool RealFileHasSameCase(string fileName)
        {
            fileName = Path.GetFullPath(fileName);

            var onlyFileName = Path.GetFileName(fileName);
            var files = Directory.GetFiles(Path.GetDirectoryName(fileName)!, onlyFileName);

            if (files.Length == 0 || files[0] != fileName)
                return false;
            return true;
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
        /// <param name="appFolderOverride">Path to use instead of app folder. Optional. </param>
        /// <returns>
        /// The full name (including path) for the first found file that match the
        /// specified search pattern. Returns <c>null</c> if no file was found.
        /// </returns>
        public static string? FindFileRecursiveInAppFolder(
            string searchPattern,
            string? appFolderOverride = null)
        {
            var result = FindFirstFile(
                appFolderOverride ?? CommonUtils.GetAppFolder(),
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
                    App.Log("Deleting file: " + s);
                    try
                    {
                        File.Delete(s);
                    }
                    catch (Exception)
                    {
                        App.Log("ERROR deleting file: " + s);
                    }
                }
            }
        }
    }
}
