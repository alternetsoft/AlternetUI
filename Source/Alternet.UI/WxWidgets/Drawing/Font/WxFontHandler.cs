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
        Alternet.Drawing.FontStyle IFontHandler.Style
        {
            get
            {
                return (Alternet.Drawing.FontStyle)Style;
            }
        }

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
               ToNativeGenericFamily(prm.GenericFamily),
               prm.FamilyName,
               prm.Size,
               (UI.Native.FontStyle)prm.Style);

            static UI.Native.GenericFontFamily ToNativeGenericFamily(
                Alternet.Drawing.GenericFontFamily? value)
            {
                return value == null ?
                    UI.Native.GenericFontFamily.None :
                    (UI.Native.GenericFontFamily)value;
            }
        }

        bool IFontHandler.Equals(Alternet.Drawing.Font font)
        {
            if (font.Handler is not Font handler)
                return false;
            return IsEqualTo(handler);
        }

        FontWeight IFontHandler.GetWeight()
        {
            return (FontWeight)GetWeight();
        }
    }
}
