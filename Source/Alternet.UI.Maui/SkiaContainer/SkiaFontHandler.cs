using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using SkiaSharp;

namespace Alternet.UI
{
    public class SkiaFontHandler : PlessFontHandler
    {
        private readonly SKFont font;

        public SkiaFontHandler()
            : this(new SKFont())
        {
        }

        public SkiaFontHandler(SKFont font)
        {
            this.font = font;
        }
    }
}
