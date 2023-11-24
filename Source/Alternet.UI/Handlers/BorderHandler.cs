using System.Diagnostics;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class BorderHandler : ControlHandler<Border>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext dc)
        {
            Control.DefaultPaint(dc, DrawClientRectangle);
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            return base.GetPreferredSize(availableSize) +
                new Size(
                    Control.BorderWidth.Horizontal,
                    Control.BorderWidth.Vertical);
        }
    }
}