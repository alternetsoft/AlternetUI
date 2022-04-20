using Alternet.Drawing;
using Alternet.UI;
using System;

namespace PaintSample
{
    internal class PenTool : Tool
    {
        public double Thickness { get; set; } = 1;

        private Polyline? currentPolyline;

        public PenTool(Document document, ISelectedColors selectedColors, UndoService undoService) :
            base(document, selectedColors, undoService)
        {
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Canvas.CaptureMouse();

            currentPolyline = new Polyline(new Pen(SelectedColors.Stroke, Thickness), e.GetPosition(Canvas));
            Document.AddShape(currentPolyline);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (currentPolyline != null)
            {
                currentPolyline.AddPoint(e.GetPosition(Canvas));
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Canvas.ReleaseMouseCapture();

            if (currentPolyline == null)
                throw new InvalidOperationException();

            Document.RemoveShape(currentPolyline);

            UndoService.Do(new Command(
                () => Document.AddShape(currentPolyline),
                () => Document.RemoveShape(currentPolyline)));

            currentPolyline = null;
        }
    }
}