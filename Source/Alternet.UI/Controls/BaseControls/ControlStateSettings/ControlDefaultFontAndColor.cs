using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ControlDefaultFontAndColor : IReadOnlyFontAndColor
    {
        private readonly Control control;

        public ControlDefaultFontAndColor(Control control)
        {
            this.control = control;
        }

        public Color? BackgroundColor => control.GetDefaultAttributesBgColor();

        public Color? ForegroundColor => control.GetDefaultAttributesFgColor();

        public Font? Font => control.GetDefaultAttributesFont();
    }
}
