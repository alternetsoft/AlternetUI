using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /*
        Microsoft.Maui.FontManager
        Microsoft.Maui.FontRegistrar
    */

    public class MauiFontFactoryHandler : PlessFontFactoryHandler
    {
        public override IFontHandler CreateDefaultFontHandler()
        {
            return new MauiFontHandler(Microsoft.Maui.Graphics.Font.Default);
        }

        public override IFontHandler CreateDefaultMonoFontHandler()
        {
            return CreateDefaultFontHandler();
        }

        public override IFontHandler CreateFontHandler()
        {
            return CreateDefaultFontHandler();
        }
    }
}
