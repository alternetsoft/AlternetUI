using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Storage;

namespace Alternet.Maui
{
    /// <summary>
    /// Provides utility methods for working with temporary files and directories.
    /// </summary>
    public static class TempFileUtils
    {
        /// <summary>
        /// Gets the path to the temporary folder used by the application.
        /// </summary>
        /// <returns>
        /// A string representing the path to the temporary folder, with a directory separator
        /// character appended.
        /// </returns>
        public static string GetTempFolder()
        {
            // Namespace: Microsoft.Maui.Storage
            // Assembly: Microsoft.Maui.Essentials.dll
            return UI.PathUtils.AddDirectorySeparatorChar(FileSystem.CacheDirectory);
        }

        /// <summary>
        /// Generates a full file path for a temporary file in the application's temporary folder.
        /// </summary>
        /// <param name="filename">The name of the temporary file.</param>
        /// <returns>
        /// A string representing the full path to the temporary file.
        /// </returns>
        public static string GenTempFilePath(string filename)
        {
            string tempFilePath = Path.Combine(GetTempFolder(), filename);
            return tempFilePath;
        }
    }
}
