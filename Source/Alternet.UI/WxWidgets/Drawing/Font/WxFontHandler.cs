using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.UI.Native
{
    internal partial class Font : IFontHandler
    {
        void IFontHandler.Update(IFontHandler.FontParams prm)
        {
            Alternet.Drawing.Font.CoerceFontParams(ref prm);
            Initialize(
               prm.GenericFamily ?? 0,
               prm.FamilyName,
               prm.Size,
               prm.Style);
        }

        bool IFontHandler.Equals(Alternet.Drawing.Font font)
        {
            if (font.Handler is not UI.Native.Font handler)
                return false;
            return IsEqualTo(handler);
        }

        FontWeight IFontHandler.GetWeight()
        {
            return (FontWeight)GetWeight();
        }

        int IFontHandler.GetPixelSize()
        {
            return GetPixelSize().Height;
        }

        FontEncoding IFontHandler.GetEncoding()
        {
            return (FontEncoding)GetEncoding();
        }
    }
}
