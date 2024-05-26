using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.Drawing;

namespace PropertyGridSample
{
    public class ControlPainterPreview : UserControl
    {
        private static readonly WxControlPainterHandler Painter = new();

        private WxControlPainterHandler.ControlPartKind kind
            = WxControlPainterHandler.ControlPartKind.PushButton;

        private WxControlPainterHandler.DrawFlags flags;

        public ControlPainterPreview()
            : base()
        {
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

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.Graphics;
            var bounds = DrawClientRectangle;

            var brush = this.Background;
            if (brush != null)
                dc.FillRectangle(brush, bounds);

            Painter.DrawItem(
                Kind,
                this,
                dc,
                bounds,
                Flags);
        }
    }
}
