using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    public sealed class EraserTool : PenLikeTool
    {
        public EraserTool(Func<PaintSampleDocument> getDocument, ISelectedColors selectedColors, UndoService undoService) :
            base(getDocument, selectedColors, undoService)
        {
        }

        public override Color PenColor => Document.BackgroundColor;

        public override string Name => "Eraser";
    }
}