using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ControlStaticDefaultFontAndColor : IReadOnlyFontAndColor
    {
        private readonly ControlTypeId controlType;
        private readonly ControlRenderSizeVariant renderSize;

        public ControlStaticDefaultFontAndColor(
            ControlTypeId controlType,
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
}
