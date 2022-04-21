using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    internal sealed class EraserTool : PenLikeTool
    {
        public EraserTool(Document document, ISelectedColors selectedColors, UndoService undoService) :
            base(document, selectedColors, undoService)
        {
        }

        public override Color PenColor => Document.BackgroundColor;
    }
}