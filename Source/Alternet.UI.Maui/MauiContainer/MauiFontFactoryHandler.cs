using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public class MauiFontFactoryHandler : PlessFontFactoryHandler
    {
        public override IFontHandler CreateDefaultFont()
        {
            return new MauiFontHandler(Microsoft.Maui.Graphics.Font.Default);
        }

        public override IFontHandler CreateDefaultMonoFont()
        {
            return CreateDefaultFont();
        }

        public override IFontHandler CreateFont()
        {
            return CreateDefaultFont();
        }
    }
}
