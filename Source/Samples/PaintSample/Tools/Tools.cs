using System;
using System.Collections.Generic;

namespace PaintSample
{
    internal class Tools
    {
        private readonly Func<Document> getDocument;
        private readonly ISelectedColors selectedColors;
        private readonly UndoService undoService;
        private readonly CanvasControl canvasControl;

        PenTool? pen;
        EraserTool? eraser;
        FloodFillTool? floodFill;
        AirbrushTool? airbrush;

        public Tools(Func<Document> getDocument, ISelectedColors selectedColors, UndoService undoService, CanvasControl canvasControl)
        {
            this.getDocument = getDocument;
            this.selectedColors = selectedColors;
            this.undoService = undoService;
            this.canvasControl = canvasControl;
        }

        public PenTool Pen => pen ??= new PenTool(getDocument, selectedColors, undoService);
        public EraserTool Eraser => eraser ??= new EraserTool(getDocument, selectedColors, undoService);
        public FloodFillTool FloodFill => floodFill ??= new FloodFillTool(getDocument, selectedColors, undoService);
        public AirbrushTool Airbrush => airbrush ??= new AirbrushTool(getDocument, selectedColors, undoService);

        public IEnumerable<Tool> AllTools
        {
            get
            {
                yield return Pen;
                yield return Eraser;
                yield return FloodFill;
                yield return Airbrush;
            }
        }

        private Tool? currentTool;

        public Tool CurrentTool
        {
            get => currentTool ?? throw new InvalidOperationException();

            set
            {
                if (currentTool == value)
                    return;

                if (currentTool != null)
                    currentTool.Deactivate();

                currentTool = value;

                if (currentTool != null)
                    currentTool.Activate(canvasControl);

                CurrentToolChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? CurrentToolChanged;
    }
}