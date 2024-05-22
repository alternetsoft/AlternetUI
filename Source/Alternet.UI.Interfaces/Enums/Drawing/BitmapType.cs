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
        Bmp,

        /// <summary>
        /// Bitmap type is 'BmpResource'.
        /// </summary>
        BmpResource,

        /// <summary>
        /// Bitmap type is 'Resource' (same as 'BmpResource').
        /// </summary>
        Resource = BmpResource,

        /// <summary>
        /// Bitmap type is 'Ico'.
        /// </summary>
        Ico,

        /// <summary>
        /// Bitmap type is 'IcoResource'.
        /// </summary>
        IcoResource,

        /// <summary>
        /// Bitmap type is 'Cur'.
        /// </summary>
        Cur,

        /// <summary>
        /// Bitmap type is 'CurResource'.
        /// </summary>
        CurResource,

        /// <summary>
        /// Bitmap type is 'Xbm'.
        /// </summary>
        Xbm,

        /// <summary>
        /// Bitmap type is 'XbmData'.
        /// </summary>
        XbmData,

        /// <summary>
        /// Bitmap type is 'Xpm'.
        /// </summary>
        Xpm,

        /// <summary>
        /// Bitmap type is 'XpmData'.
        /// </summary>
        XpmData,

        /// <summary>
        /// Bitmap type is 'Tiff'.
        /// </summary>
        Tiff,

        /// <summary>
        /// Bitmap type is 'TiffResource'.
        /// </summary>
        TiffResource,

        /// <summary>
        /// Bitmap type is 'Gif'.
        /// </summary>
        Gif,

        /// <summary>
        /// Bitmap type is 'GifResource'.
        /// </summary>
        GifResource,

        /// <summary>
        /// Bitmap type is 'Png'.
        /// </summary>
        Png,

        /// <summary>
        /// Bitmap type is 'PngResource'.
        /// </summary>
        PngResource,

        /// <summary>
        /// Bitmap type is 'Jpeg'.
        /// </summary>
        Jpeg,

        /// <summary>
        /// Bitmap type is 'JpegResource'.
        /// </summary>
        JpegResource,

        /// <summary>
        /// Bitmap type is 'Pnm'.
        /// </summary>
        Pnm,

        /// <summary>
        /// Bitmap type is 'PnmResource'.
        /// </summary>
        PnmResource,

        /// <summary>
        /// Bitmap type is 'Pcx'.
        /// </summary>
        Pcx,

        /// <summary>
        /// Bitmap type is 'PcxResource'.
        /// </summary>
        PcxResource,

        /// <summary>
        /// Bitmap type is 'Pict'.
        /// </summary>
        Pict,

        /// <summary>
        /// Bitmap type is 'PictResource'.
        /// </summary>
        PictResource,

        /// <summary>
        /// Bitmap type is 'Icon'.
        /// </summary>
        Icon,

        /// <summary>
        /// Bitmap type is 'IconResource'.
        /// </summary>
        IconResource,

        /// <summary>
        /// Bitmap type is 'Ani'.
        /// </summary>
        Ani,

        /// <summary>
        /// Bitmap type is 'Iff'.
        /// </summary>
        Iff,

        /// <summary>
        /// Bitmap type is 'Tga'.
        /// </summary>
        Tga,

        /// <summary>
        /// Bitmap type is 'MacCursor'.
        /// </summary>
        MacCursor,

        /// <summary>
        /// Bitmap type is 'MacCursorResource'.
        /// </summary>
        MacCursorResource,

        /// <summary>
        /// Max possible value in <see cref="BitmapType"/>.
        /// </summary>
        Max,

        /// <summary>
        /// Any bitmap type.
        /// </summary>
        Any = 50,
    }
}
