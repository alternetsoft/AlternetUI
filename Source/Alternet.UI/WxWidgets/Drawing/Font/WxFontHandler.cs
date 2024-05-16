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
            if (prm.Unit != GraphicsUnit.Point)
            {
                prm.Size = GraphicsUnitConverter.Convert(
                    prm.Unit,
                    GraphicsUnit.Point,
                    Display.Primary.DPI.Height,
                    prm.Size);
            }

            if (prm.GenericFamily == null && prm.FamilyName == null)
            {
                BaseApplication.LogError("Font name and family are null, using default font.");
                prm.GenericFamily = Alternet.Drawing.GenericFontFamily.Default;
            }

            prm.Size = Alternet.Drawing.Font.CheckSize(prm.Size);

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
    }
}
