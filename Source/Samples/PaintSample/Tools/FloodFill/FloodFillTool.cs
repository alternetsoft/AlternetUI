using Alternet.Drawing;
using Alternet.UI;
using System;

namespace PaintSample
{
    internal class FloodFillTool : Tool
    {
        public FloodFillTool(Func<Document> getDocument, ISelectedColors selectedColors, UndoService undoService) :
            base(getDocument, selectedColors, undoService)
        {
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            var point = e.GetPosition(Canvas);

            if (!new Rect(new Point(), Document.Bitmap.Size).Contains(point))
                return;

            UndoService.Do(() => Document.Modify(dc => dc.FloodFill(new SolidBrush(SelectedColors.Stroke), point)));
        }

        public override string Name => "Flood Fill";
    }
}