using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the file masks.
    /// </summary>
    public class FileMaskUtils
    {
        /// <summary>
        /// Gets or sets localization for "All Files" string.
        /// </summary>
        public static string StrAllFiles = "All Files";

        /// <summary>
        /// Gets or sets localization for "Image Files" string.
        /// </summary>
        public static string StrImageFiles = "Image Files";

        /// <summary>
        /// Gets or sets localization for "Image Files" string.
        /// </summary>
        public static string StrLibraryFiles = "Library Files";

        /// <summary>
        /// Gets "Library Files (*.*)|*.*" string.
        /// </summary>
        public static string FileDialogFilterLibraryFiles => $"{StrLibraryFiles} (*.dll)|*.dll";

        /// <summary>
        /// Gets "All Files (*.*)|*.*" string.
        /// </summary>
        public static string FileDialogFilterAllFiles => $"{StrAllFiles} (*.*)|*.*";

        /// <summary>
        /// Gets string like "Image Files (*.png; *.jpg)|*.png;*.jpg", but with all supported
        /// image format extensions for the open file dialog.
        /// </summary>
        public static string GetFileDialogFilterForImageOpen(bool addAllFiles = false)
        {
            return GetFileDialogFilterForImages(Image.ExtensionsForLoad, addAllFiles);
        }

        /// <summary>
        /// Builds string for the <see cref="FileDialog.Filter"/> property
        /// of <see cref="OpenFileDialog"/> and <see cref="SaveFileDialog"/>.
        /// Sample: "PNG Files (*.png)|*.png".
        /// </summary>
        /// <param name="text">Text part of the filter.</param>
        /// <param name="ext">File extension used in the filter. Can be specified
        /// with or without '.' character at the beginning.</param>
        /// <returns></returns>
        public static string GetFileDialogFilter(string text, string ext)
        {
            var extensionStr = $"*.{ext.TrimStart('.')}";
            var result = $"{text} ({extensionStr})|{extensionStr}";
            return result;
        }

        /// <summary>
        /// Builds a file dialog filter string from an array of
        /// <see cref="FileDialogFilterItem"/> objects.
        /// </summary>
        /// <param name="filters">An array of filter items, each representing a file type
        /// description and pattern.</param>
        /// <returns>
        /// A combined string suitable for use with file dialogs, where individual filters
        /// are separated by a vertical bar ('|').
        /// </returns>
        /// <remarks>
        /// This utility simplifies creating filter strings
        /// like: <c>"Images (*.png)|*.png|Text Files (*.txt)|*.txt"</c>.
        /// </remarks>
        public static string ToFileDialogFilter(params FileDialogFilterItem[] filters)
        {
            string result = string.Empty;

            foreach (var filter in filters)
            {
                var s = filter?.ToString();

                if (s is null)
                    continue;

                if (result.Length == 0)
                    result = s;
                else
                    result += "|" + s;
            }

            return result;
        }

        /// <summary>
        /// Gets string like "Image Files (*.png; *.jpg)|*.png;*.jpg", but with all supported
        /// image format extensions for the save file dialog.
        /// </summary>
        public static string GetFileDialogFilterForImageSave()
        {
            return GetFileDialogFilterForImages(Image.ExtensionsForSave);
        }

        /// <summary>
        /// Generates file dialog filter.
        /// Result example:"Image Files (*.png; *.jpg)|*.png;*.jpg".
        /// </summary>
        /// <param name="title">The title for the filter item.</param>
        /// <param name="extensions">Array of file extensions. Each item is an extension
        /// with or without "." in the beginning. Example: ".exe", "exe".</param>
        /// <param name="addAllFiles"></param>
        /// <returns></returns>
        public static string GetFileDialogFilter(
            string title,
            IEnumerable<string> extensions,
            bool addAllFiles = false)
        {
            var withoutDot = EnumerableUtils.RemovePrefix(extensions, ".");
            var withAsterix = EnumerableUtils.InsertPrefix(withoutDot, "*.");

            var extensionStr = StringUtils.ToString(withAsterix, string.Empty, string.Empty, ";");

            var result = $"{title} ({extensionStr})|{extensionStr}";

            if (addAllFiles)
                result = $"{result}|{FileDialogFilterAllFiles}";
            return result;
        }

        /// <summary>
        /// Generates file dialog filter for the images.
        /// Result example:"Image Files (*.png; *.jpg)|*.png;*.jpg|All Files (*.*)|*.*".
        /// </summary>
        /// <param name="extensions">Array of extensions. Each item is an extension with "."
        /// in the beginning (like ".exe").</param>
        /// <param name="addAllFiles"></param>
        /// <returns></returns>
        public static string GetFileDialogFilterForImages(
            IEnumerable<string> extensions,
            bool addAllFiles = false)
        {
            var result = GetFileDialogFilter(StrImageFiles, extensions, addAllFiles);
            return result;
        }
    }
}