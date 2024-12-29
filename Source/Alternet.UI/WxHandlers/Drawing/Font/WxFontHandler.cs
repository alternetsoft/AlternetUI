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
        /// <summary>
        /// Gets whether font is using size in pixels.
        /// </summary>
        bool IFontHandler.IsUsingSizeInPixels(Alternet.Drawing.Font font)
            => IsUsingSizeInPixels();

        void IFontHandler.Update(Alternet.Drawing.Font font, IFontHandler.FontParams prm)
        {
            Alternet.Drawing.Font.CoerceFontParams(prm);
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

        int IFontHandler.GetPixelSize(Alternet.Drawing.Font font)
        {
            return GetPixelSize().Height;
        }

        FontEncoding IFontHandler.GetEncoding(Alternet.Drawing.Font font)
        {
            return (FontEncoding)GetEncoding();
        }

        int IFontHandler.GetNumericWeight(Alternet.Drawing.Font font)
        {
            return GetNumericWeight();
        }

        bool IFontHandler.IsFixedWidth(Alternet.Drawing.Font font)
        {
            return IsFixedWidth();
        }
    }
}
