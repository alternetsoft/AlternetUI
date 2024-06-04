using Alternet.Drawing;
using Alternet.UI;
using System;
using Alternet.UI.Extensions;

namespace PaintSample
{
    public class FloodFillTool : Tool
    {
        public FloodFillTool(Func<PaintSampleDocument> getDocument, ISelectedColors selectedColors, UndoService undoService) :
            base(getDocument, selectedColors, undoService)
        {
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            var point = Mouse.GetPosition(Canvas);

            if (!new RectD(new PointD(), Document.Bitmap.SizeDip(Canvas)).Contains(point))
                return;

            UndoService.Do(() => Document.Modify(dc => dc.FloodFill(new SolidBrush(SelectedColors.Stroke), point)));
        }

        public override string Name => "Flood Fill";
    }
}