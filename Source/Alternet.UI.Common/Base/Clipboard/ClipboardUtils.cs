using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static properties and methods related to the clipboard.
    /// </summary>
    public static class ClipboardUtils
    {
        /// <summary>
        /// Sets data transformation.
        /// </summary>
        /// <param name="format">Data format.</param>
        /// <param name="data">Object with data.</param>
        /// <returns></returns>
        public static object SetDataTransform(string format, object data)
        {
            if (format.Equals(DataFormats.Files, StringComparison.Ordinal))
            {
                return data switch
                {
                    FileInfo[] x => x.Select(x => x.FullName).ToArray(),
                    _ => data
                };
            }

            return data;
        }

        /// <summary>
        /// Detects format of the specified object with data.
        /// </summary>
        /// <param name="data">Object with data.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Raised if format of the data can't be detected.</exception>
        public static string DetectFormatFromData(object data) => data switch
        {
            string _ => DataFormats.Text,
            FileInfo[] _ => DataFormats.Files,
            Image _ => DataFormats.Bitmap,
            _ => data.GetType().FullName ?? throw new InvalidOperationException(),
        };
    }
}