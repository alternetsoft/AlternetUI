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
