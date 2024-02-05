using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    public class ControlPainterPreview : UserPaintControl
    {
        private NativeControlPainter.ControlPartKind kind
            = NativeControlPainter.ControlPartKind.PushButton;

        private NativeControlPainter.DrawFlags flags;

        public ControlPainterPreview()
            : base()
        {
        }

        public NativeControlPainter.ControlPartKind Kind
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

        public NativeControlPainter.DrawFlags Flags
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
            var dc = e.DrawingContext;
            var bounds = DrawClientRectangle;

            var brush = this.Background;
            if (brush != null)
                dc.FillRectangle(brush, bounds);

            NativeControlPainter.Default.DrawItem(
                Kind,
                this,
                dc,
                bounds,
                Flags);
        }
    }
}
