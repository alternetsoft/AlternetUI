using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Contains common static methods.
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// Returns a new array containing all non-null elements from the specified array of nullable values.
        /// </summary>
        /// <remarks>The returned array will not contain any null elements. If the input array contains no
        /// non-null values, the result is an empty array. The method preserves the order of non-null elements from the
        /// input array.</remarks>
        /// <typeparam name="T">The type of the elements in the input and result arrays.
        /// Must be a non-nullable value or reference type.</typeparam>
        /// <param name="values">An array of nullable values to filter. May be null or empty.</param>
        /// <returns>An array containing all non-null elements from the input array, in their original order. Returns an empty
        /// array if the input is null, empty, or contains only null elements.</returns>
        public static T[] GetNotNull<T>(T?[]? values)
            where T : class
        {
            if (!HasNulls<T>(values))
                return values!;

#pragma warning disable
            if (values == null || values.Length == 0)
                return Array.Empty<T>();
#pragma warning restore

            List<T> result = new(values.Length);

            foreach (var type in values)
            {
                if (type != null)
                    result.Add(type);
            }

#pragma warning disable
            return result.ToArray();
#pragma warning restore
        }

        /// <summary>
        /// Determines whether the specified array contains any null elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array. Must be a nullable value type or a reference type.</typeparam>
        /// <param name="array">The array to check for null elements. Cannot be null.</param>
        /// <returns>true if the array contains at least one null element; otherwise, false.</returns>
        public static bool HasNulls<T>(T?[]? array)
            where T : class
        {
            if (array is null || array.Length == 0)
                return false;

            foreach (var item in array)
            {
                if (item == null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Calls the specified action if condition is <c>true</c>.
        /// </summary>
        /// <param name="condition">Condition to check.</param>
        /// <param name="actionToCall">Action to call.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CallIf(bool condition, Action actionToCall)
        {
            if (condition)
                actionToCall();
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Nop()
        {
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
        /// Replaces spaces with %20, other non url chars with corresponding
        /// html codes.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceNonUrlChars(string s)
        {
            s = s.Replace('\\', '/')
                .Replace(":", "%3A")
                .Replace(" ", "%20");
            return s;
        }

        /// <summary>
        /// Prepends filename with "file" url protocol prefix.
        /// </summary>
        /// <param name="filename">Path to the file.</param>
        /// <returns><see cref="string"/> containing filename with "file" url
        /// protocol prefix.</returns>
        /// <remarks>
        /// Under Windows file path starts with drive letter, so we need
        /// to add additional "/" separator between protocol and file path.
        /// This functions checks for OS and adds "file" url protocol correctly.
        /// Also spaces are replaced with %20, other non url chars are replaced
        /// with corresponding html codes.
        /// </remarks>
        public static string PrepareFileUrl(string filename)
        {
            string url;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                url = "file:///" + ReplaceNonUrlChars(filename);
            else
                url = "file://" + ReplaceNonUrlChars(filename);
            return url;
        }

        /// <summary>
        /// Prepares zip url in the form "{schemeName}:///{arcPath};protocol={fileInArchivePath}"
        /// </summary>
        /// <param name="schemeName">Name of the theme, any string.</param>
        /// <param name="arcPath">Path to archive file.</param>
        /// <param name="fileInArchivePath">Path to file in archive.</param>
        /// <returns></returns>
        public static string PrepareZipUrl(string schemeName, string arcPath, string fileInArchivePath)
        {
            static string PrepareUrl(string s)
            {
                s = s.Replace('\\', '/');
                return s;
            }

            string url = schemeName + "://" + PrepareUrl(arcPath) + ";protocol=zip/" +
                PrepareUrl(fileInArchivePath);
            return url;
        }

        /// <summary>
        /// Gets path to the exe file of the application.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetAppExePath()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
        }

        /// <summary>
        /// Returns path to the application folder.
        /// </summary>
        /// <returns><see cref="string"/> containing path to the application folder
        /// with directory separator char at the end.</returns>
        public static string GetAppFolder()
        {
            string location = App.ExecutingAssemblyLocation;
            string s = Path.GetDirectoryName(location)!;
            return PathUtils.AddDirectorySeparatorChar(s);
        }

        /// <summary>
        /// Gets Samples folder of the UI Installation.
        /// </summary>
        public static string? GetSamplesFolder(string suffix = "Samples")
        {
            bool ValidFolder(string s)
            {
                var trimmed = TrimEndDirectorySeparator(s);
                var result = trimmed is not null && trimmed.EndsWith(suffix);
                return result;
            }

            string folder1 = GetAppFolder() + @"../../../../";
            string folder2 = folder1 + @"../";
            folder1 = Path.GetFullPath(folder1);
            folder2 = Path.GetFullPath(folder2);

            if (ValidFolder(folder1))
                return folder1;

            if (ValidFolder(folder2))
                return folder2;

            return null;
        }

        /// <summary>
        /// Gets Alternet.UI.Pal folder.
        /// </summary>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string? GetPalFolder(string suffix = "Samples")
        {
            var samplesFolder = GetSamplesFolder(suffix);
            if (samplesFolder is null)
                return null;
            string relativePath = Path.Combine(samplesFolder, "..", "Alternet.UI.Pal");
            string result = Path.GetFullPath(relativePath);
            return result;
        }

        /// <summary>
        /// Gets Alternet.UI folder.
        /// </summary>
        public static string? GetUIFolder(string suffix = "Samples")
        {
            var samplesFolder = GetSamplesFolder(suffix);
            if (samplesFolder is null)
                return null;
            string relativePath = Path.Combine(samplesFolder, "..", "Alternet.UI");
            string result = Path.GetFullPath(relativePath);
            return result;
        }
    }
}
