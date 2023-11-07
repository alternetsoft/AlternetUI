using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public enum BitmapType
    {
        Invalid = 0,

        Bmp,

        BmpResource,

        Resource = BmpResource,

        Ico,

        IcoResource,

        Cur,

        CurResource,

        Xbm,

        XbmData,

        Xpm,

        XpmData,

        Tiff,

        TiffResource,

        Gif,

        GifResource,

        Png,

        PngResource,

        Jpeg,

        JpegResource,

        Pnm,

        PnmResource,

        Pcx,

        PcxResource,

        Pict,

        PictResource,

        Icon,

        IconResource,

        Ani,

        Iff,

        Tga,

        MacCursor,

        MacCursorResource,

        Max,

        Any = 50,
    }
}
