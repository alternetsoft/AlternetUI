using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known clipboard data format identifiers.
    /// </summary>
    public enum ClipboardDataFormatId
    {
        /// <summary>
        /// An invalid format - used as default argument for functions taking
        /// a data format argument sometimes.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Text format.
        /// </summary>
        Text = 1,

        /// <summary>
        /// A bitmap.
        /// </summary>
        Bitmap = 2,

        /// <summary>
        /// A metafile (Windows only).
        /// </summary>
        MetaFile = 3,

        /// <summary>
        /// 'Sylk' format.
        /// </summary>
        Sylk = 4,

        /// <summary>
        /// 'Dif' format.
        /// </summary>
        Dif = 5,

        /// <summary>
        /// 'Tiff' format.
        /// </summary>
        Tiff = 6,

        /// <summary>
        /// 'OemText' format.
        /// </summary>
        OemText = 7,

        /// <summary>
        /// 'Dib' format.
        /// </summary>
        Dib = 8,

        /// <summary>
        /// 'Palette' format.
        /// </summary>
        Palette = 9,

        /// <summary>
        /// 'PenData' format.
        /// </summary>
        PenData = 10,

        /// <summary>
        /// 'Riff' format.
        /// </summary>
        Riff = 11,

        /// <summary>
        /// 'Wave' format.
        /// </summary>
        Wave = 12,

        /// <summary>
        /// Unicode text format.
        /// </summary>
        UnicodeText = 13,

        /// <summary>
        /// 'EnhMetaFile' format.
        /// </summary>
        EnhMetaFile = 14,

        /// <summary>
        /// A list of filenames.
        /// </summary>
        Filename = 15,

        /// <summary>
        /// 'Locale' format.
        /// </summary>
        Locale = 16,

        /// <summary>
        /// 'Private' format.
        /// </summary>
        Private = 20,

        /// <summary>
        /// An HTML string. This is currently only valid on Mac and MSW.
        /// </summary>
        Html = 30,

        /// <summary>
        /// A PNG file. This is valid only on MSW.
        /// </summary>
        Png = 31,

        /// <summary>
        /// Special value for the iterators.
        /// </summary>
        Max,
    }
}