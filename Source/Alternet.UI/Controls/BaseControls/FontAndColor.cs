using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class FontAndColor
    {
        internal class ControlStaticDefaultFontAndColor : IReadOnlyFontAndColor
        {
            private readonly ControlId controlType;
            private readonly ControlRenderSizeVariant renderSize;

            public ControlStaticDefaultFontAndColor(
                ControlId controlType,
                ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
            {
                this.controlType = controlType;
                this.renderSize = renderSize;
            }

            public Color? BackgroundColor =>
                Control.GetClassDefaultAttributesBgColor(controlType, renderSize);

            public Color? ForegroundColor =>
                Control.GetClassDefaultAttributesFgColor(controlType, renderSize);

            public Font? Font =>
                Control.GetClassDefaultAttributesFont(controlType, renderSize);
        }

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
}
