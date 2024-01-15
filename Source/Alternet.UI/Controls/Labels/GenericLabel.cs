using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements generic label control.
    /// </summary>
    public class GenericLabel : UserPaintControl
    {
        private string text = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLabel"/> class.
        /// </summary>
        /// <param name="text">Value of the <see cref="Text"/> property.</param>
        public GenericLabel(string text)
        {
            this.text = text ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLabel"/> class.
        /// </summary>
        public GenericLabel()
        {
        }

        /// <inheritdoc/>
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
                text = value ?? string.Empty;
                RaiseTextChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new LabelHandler();
        }

        /// <summary>
        /// Gets <see cref="Font"/> which is used to draw labels text.
        /// </summary>
        /// <returns></returns>
        protected virtual Font GetLabelFont()
        {
            var result = Font ?? UI.Control.DefaultFont;
            if (IsBold)
                result = result.AsBold;
            return result;
        }

        internal class LabelHandler : ControlHandler<GenericLabel>
        {
            protected override bool NeedsPaint => true;

            public override void OnPaint(Graphics drawingContext)
            {
                if (Control.Text != null)
                {
                    drawingContext.DrawText(
                        Control.Text,
                        Control.ChildrenLayoutBounds.Location,
                        Control.GetLabelFont(),
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
                var result = dc.GetTextExtent(text, Control.GetLabelFont(), Control);
                return result + Control.Padding.Size;
            }
        }
    }
}
