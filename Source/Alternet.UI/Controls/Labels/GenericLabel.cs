using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class GenericLabel : UserPaintControl
    {
        private string text = string.Empty;

        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public override string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text == value)
                    return;
                text = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new LabelHandler();
        }

        internal class LabelHandler : ControlHandler<GenericLabel>
        {
            protected override bool NeedsPaint => true;

            public override void OnPaint(DrawingContext drawingContext)
            {
                if (Control.Text != null)
                {
                    drawingContext.DrawText(
                        Control.Text,
                        Control.ChildrenLayoutBounds.Location,
                        Control.Font ?? UI.Control.DefaultFont,
                        Control.ForeColor,
                        Color.Empty);
                }
            }

            public override SizeD GetPreferredSize(SizeD availableSize)
            {
                var text = Control.Text;
                if (text == null)
                    return new SizeD();

                using var dc = Control.CreateDrawingContext();
                var result = dc.GetTextExtent(text, Control.Font ?? UI.Control.DefaultFont, Control);
                return result + Control.Padding.Size;
            }
        }
    }
}
