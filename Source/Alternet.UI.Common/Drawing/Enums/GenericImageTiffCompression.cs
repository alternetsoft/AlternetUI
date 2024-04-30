using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates Tiff compression methods for use with <see cref="GenericImage"/>
    /// </summary>
    public enum GenericImageTiffCompression
    {
        /// <summary>
        /// Dump mode.
        /// </summary>
        None = 1,

        /// <summary>
        /// Compression method 'CCITT modified Huffman RLE'.
        /// </summary>
        CCITTRLE = 2,

        /// <summary>
        /// Compression method 'CCITT Group 3 fax encoding'.
        /// </summary>
        CCITTFAX3 = 3,

        /// <summary>
        /// Compression method 'CCITT T.4 (TIFF 6 name)'.
        /// </summary>
#pragma warning disable
        CCITT_T = 3,
#pragma warning restore

        /// <summary>
        /// Compression method 'CCITT Group 4 fax encoding'.
        /// </summary>
        CCITTFAX4 = 4,

        /// <summary>
        /// Compression method 'CCITT T.6 (TIFF 6 name)'.
        /// </summary>
#pragma warning disable
        CCITT_T6 = 4,
#pragma warning restore

        /// <summary>
        /// Compression method 'Lempel-Ziv and Welch'.
        /// </summary>
        LZW = 5,

        /// <summary>
        /// Compression method '!6.0 JPEG'.
        /// </summary>
        OJPEG = 6,

        /// <summary>
        /// Compression method '%JPEG DCT compression'.
        /// </summary>
        JPEG = 7,

        /// <summary>
        /// Compression method '!TIFF/FX T.85 JBIG compression'.
        /// </summary>
        T85 = 9,

        /// <summary>
        /// Compression method '!TIFF/FX T.43 colour by layered JBIG compression'.
        /// </summary>
        T43 = 10,
    }
}