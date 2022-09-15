using System.Drawing;
using System.IO;

namespace Alternet.UI
{
    /// <summary>
    /// Provides static, predefined Clipboard and Drag and Drop format names.
    /// Use them to identify the format of data that you store in an <see cref="IDataObject"/>.
    /// </summary>
    public static class DataFormats
    {
        /// <summary>
        /// Specifies the text format. This static field is read-only.
        /// </summary>
        public static readonly string Text = "Text";

        /// <summary>
        /// Specifies the files format. This static field is read-only.
        /// </summary>
        public static readonly string Files = "Files";

        /// <summary>
        /// Specifies the bitmap format. This static field is read-only.
        /// </summary>
        public static readonly string Bitmap = "Bitmap";
    }
}