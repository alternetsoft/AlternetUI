using Alternet.Drawing;
using Alternet.UI;

namespace PaintSample
{
    internal class FloodFillTool : Tool
    {
        public FloodFillTool(Document document, ISelectedColors selectedColors, UndoService undoService) :
            base(document, selectedColors, undoService)
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
    }
}