using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Enums
{
    internal enum ClipboardDataFormatId
    {
        Invalid = 0,
        Text = 1,  /* Cf_Text */
        Bitmap = 2,  /* Cf_Bitmap */
        MetaFile = 3,  /* Cf_Metafilepict */
        Sylk = 4,
        Dif = 5,
        Tiff = 6,
        OemText = 7,  /* Cf_Oemtext */
        Dib = 8,  /* Cf_Dib */
        Palette = 9,
        PenData = 10,
        Riff = 11,
        Wave = 12,
        UnicodeText = 13,
        EnhMetaFile = 14,
        Filename = 15, /* Cf_Hdrop */
        Locale = 16,
        Private = 20,
        Html = 30, /* Note: Does Not Correspond To Cf_ Constant */
        Png = 31, /* Note: Does Not Correspond To Cf_ Constant */
        Max,
    }
}