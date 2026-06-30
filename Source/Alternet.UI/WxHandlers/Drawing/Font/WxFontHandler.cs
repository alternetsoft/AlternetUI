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
        string IFontHandler.GetName()
        {
            return GetName().ToString();
        }

        string IFontHandler.GetDescription()
        {
            return GetDescription().ToString();
        }

        string IFontHandler.Serialize()
        {
            return Serialize().ToString();
        }

        /// <summary>
        /// Gets whether font is using size in pixels.
        /// </summary>
        bool IFontHandler.IsUsingSizeInPixels(Alternet.Drawing.Font font)
            => IsUsingSizeInPixels();

        void IFontHandler.Update(Alternet.Drawing.Font font, IFontHandler.FontParams prm)
        {
            Alternet.Drawing.Font.CoerceFontParams(prm);
            NativeStringSpan.Invoke(prm.FamilyName, span =>
            {
                Initialize(
                   prm.GenericFamily ?? 0,
                   span,
                   prm.Size,
                   prm.Style);
            });
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
