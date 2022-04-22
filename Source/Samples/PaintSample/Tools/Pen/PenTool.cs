using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    internal sealed class PenTool : PenLikeTool
    {
        public PenTool(Document document, ISelectedColors selectedColors, UndoService undoService) :
            base(document, selectedColors, undoService)
        {
        }

        public override Color PenColor => SelectedColors.Stroke;
    }
}