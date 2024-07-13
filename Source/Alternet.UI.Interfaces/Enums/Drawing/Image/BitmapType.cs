using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known bitmap types.
    /// </summary>
    public enum BitmapType
    {
        /// <summary>
        /// Invalid bitmap type.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Bitmap type is 'Bmp'.
        /// </summary>
        Bmp = 1,

        /// <summary>
        /// Bitmap type is 'Ico'.
        /// </summary>
        Ico = 3,

        /// <summary>
        /// Bitmap type is 'Cur'.
        /// </summary>
        Cur = 5,

        /// <summary>
        /// Bitmap type is 'Xbm'.
        /// </summary>
        Xbm = 7,

        /// <summary>
        /// Bitmap type is 'XbmData'.
        /// </summary>
        XbmData = 8,

        /// <summary>
        /// Bitmap type is 'Xpm'.
        /// </summary>
        Xpm = 9,

        /// <summary>
        /// Bitmap type is 'XpmData'.
        /// </summary>
        XpmData = 10,

        /// <summary>
        /// Bitmap type is 'Tiff'.
        /// </summary>
        Tiff = 11,

        /// <summary>
        /// Bitmap type is 'Gif'.
        /// </summary>
        Gif = 13,

        /// <summary>
        /// Bitmap type is 'Png'.
        /// </summary>
        Png = 15,

        /// <summary>
        /// Bitmap type is 'Jpeg'.
        /// </summary>
        Jpeg = 17,

        /// <summary>
        /// Bitmap type is 'Pnm'.
        /// </summary>
        Pnm = 19,

        /// <summary>
        /// Bitmap type is 'Pcx'.
        /// </summary>
        Pcx = 21,

        /// <summary>
        /// Bitmap type is 'Pict'.
        /// </summary>
        Pict = 23,

        /// <summary>
        /// Bitmap type is 'Icon'.
        /// </summary>
        Icon = 25,

        /// <summary>
        /// Bitmap type is 'Ani'.
        /// </summary>
        Ani = 27,

        /// <summary>
        /// Bitmap type is 'Iff'.
        /// </summary>
        Iff = 28,

        /// <summary>
        /// Bitmap type is 'Tga'.
        /// </summary>
        Tga = 29,

        /// <summary>
        /// Bitmap type is 'MacCursor'.
        /// </summary>
        MacCursor = 30,

        /// <summary>
        /// Any bitmap type.
        /// </summary>
        Any = 50,

        /// <summary>
        /// Default cursor type for the current operating system.
        /// </summary>
        CursorDefaultType = -1,
    }
}