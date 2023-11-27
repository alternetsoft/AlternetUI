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
        /// Gets "All Files (*.*)|*.*" string.
        /// </summary>
        public static string FileDialogFilterAllFiles => $"{StrAllFiles} (*.*)|*.*";

        /// <summary>
        /// Gets string like "Image Files (*.png; *.jpg)|*.png;*.jpg", but with all supported
        /// image format extensions for the open file dialog.
        /// </summary>
        public static string GetFileDialogFilterForImageOpen(bool addAllFiles = false)
        {
            return GetFileDialogFilter(Image.GetExtensionsForOpen(), addAllFiles);
        }

        /// <summary>
        /// Gets string like "Image Files (*.png; *.jpg)|*.png;*.jpg", but with all supported
        /// image format extensions for the save file dialog.
        /// </summary>
        public static string GetFileDialogFilterForImageSave()
        {
            return GetFileDialogFilter(Image.GetExtensionsForSave());
        }

        /// <summary>
        /// Generates file dialog filter.
        /// Result example:"Image Files (*.png; *.jpg)|*.png;*.jpg|All Files (*.*)|*.*".  
        /// </summary>
        /// <param name="extensions">Array of extensions. Each item is an extension with "."
        /// in the beginning (like ".exe").</param>
        /// <param name="addAllFiles"></param>
        /// <returns></returns>
        public static string GetFileDialogFilter(IEnumerable<string> extensions, bool addAllFiles = false)
        {
            var withAsterix = EnumerableUtils.InsertPrefix(extensions, "*");

            var extensionStr = StringUtils.ToString(withAsterix, string.Empty, string.Empty, ";");

            var result = $"{StrImageFiles} ({extensionStr})|{extensionStr}";

            if (addAllFiles)
                result = $"{result}|{FileDialogFilterAllFiles}";
            return result;
        }
    }
}