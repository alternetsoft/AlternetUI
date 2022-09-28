using Alternet.Drawing;
using Alternet.UI;

namespace PaintSample
{
    internal class CanvasControl : Control
    {
        private Document? document;

        public CanvasControl()
        {
            UserPaint = true;
        }

        public Document? Document
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
            e.DrawingContext.FillRectangle(Brushes.LightGray, e.Bounds);

            if (Document == null)
                return;
            
            Document.Paint(e.DrawingContext);
        }
    }
}