using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.Drawing;

namespace PropertyGridSample
{
    [ControlCategory("Tests")]
    public class ControlPainterPreview : Panel
    {
        private static readonly WxControlPainterHandler Painter = new();

        private WxControlPainterHandler.ControlPartKind kind
            = WxControlPainterHandler.ControlPartKind.PushButton;

        private WxControlPainterHandler.DrawFlags flags;

        public ControlPainterPreview()
        {
            Padding = 10;
        }

        public WxControlPainterHandler.ControlPartKind Kind
        {
            get
            {
                return kind;
            }

            set
            {
                if (kind == value)
                    return;
                kind = value;
                Invalidate();
            }
        }

        public WxControlPainterHandler.DrawFlags Flags
        {
            get
            {
                return flags;
            }

            set
            {
                if (flags == value)
                    return;
                flags = value;
                Invalidate();
            }
        }

        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);

            var dc = e.Graphics;
            var bounds = e.ClientRectangle;

            var brush = this.Background;
            if (brush != null)
                dc.FillRectangle(brush, bounds);

            bounds = bounds.DeflatedWithPadding(Padding);

            Painter.DrawItem(
                Kind,
                this,
                dc,
                bounds,
                Flags);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }
    }
}
