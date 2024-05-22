using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known flags for the save and load operations in the <see cref="RichTextBox"/>.
    /// </summary>
    /// <remarks>
    /// You can use these flags in <see cref="RichTextBox.SetFileHandlerFlags"/> method
    /// in order to customize save and load operations.
    /// </remarks>
    [Flags]
    public enum RichTextHandlerFlags
    {
        /// <summary>
        /// Include style sheet when loading and saving.
        /// </summary>
        IncludeStylesheet = 0x0001,

        /// <summary>
        /// Save images to memory file system in HTML handler.
        /// </summary>
        SaveImagesToMemory = 0x0010,

        /// <summary>
        /// Save images to files in HTML handler.
        /// </summary>
        SaveImagesToFiles = 0x0020,

        /// <summary>
        /// Save images as inline base64 data in HTML handler.
        /// </summary>
        SaveImagesToBase64 = 0x0040,

        /// <summary>
        /// Don't write header and footer (or BODY, HEAD, HTML tags), so we can include the fragment
        /// in a larger document.
        /// </summary>
        NoHeaderFooter = 0x0080,

        /// <summary>
        /// Convert the more common face names to names that will work on the current platform
        /// in a larger document.
        /// </summary>
        ConvertFacenames = 0x0100,

        /// <summary>
        /// Use CSS where possible in the HTML handler, otherwise use workarounds that will
        /// show in internal html viewer.
        /// </summary>
        UseCss = 0x1000,
    }
}