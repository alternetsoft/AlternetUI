using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    public sealed class PenTool : PenLikeTool
    {
        public PenTool(Func<Document> getDocument, ISelectedColors selectedColors, UndoService undoService) :
            base(getDocument, selectedColors, undoService)
        {
        }

        public override Color PenColor => SelectedColors.Stroke;

        public override string Name => "Pen";
    }
}