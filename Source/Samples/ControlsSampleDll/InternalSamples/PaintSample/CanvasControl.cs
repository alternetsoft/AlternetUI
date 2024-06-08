using Alternet.Drawing;
using Alternet.UI;

namespace PaintSample
{
    public class CanvasControl : Control
    {
        private PaintSampleDocument? document;

        public CanvasControl()
        {
            UserPaint = true;
        }

        public PaintSampleDocument? Document
        {
            get => document;
            set
            {
                if (document == value)
                    return;

                if (document != null)
                    document.Changed -= Document_Changed;
                
                document = value;

                if (document != null)
                    document.Changed += Document_Changed;

                Invalidate();
            }
        }

        private void Document_Changed(object? sender, System.EventArgs e)
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.ClipRectangle);

            if (Document == null)
                return;
            
            Document.Paint(this, e.Graphics);
        }
    }
}