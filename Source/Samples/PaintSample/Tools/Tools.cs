using System;
using System.Collections.Generic;

namespace PaintSample
{
    internal class Tools
    {
        private readonly Document document;
        private readonly ISelectedColors selectedColors;
        private readonly UndoService undoService;
        private readonly CanvasControl canvasControl;

        PenTool? pen;
        EraserTool? eraser;
        FloodFillTool? floodFill;
        AirbrushTool? airbrush;

        public Tools(Document document, ISelectedColors selectedColors, UndoService undoService, CanvasControl canvasControl)
        {
            this.document = document;
            this.selectedColors = selectedColors;
            this.undoService = undoService;
            this.canvasControl = canvasControl;
        }

        public PenTool Pen => pen ??= new PenTool(document, selectedColors, undoService);
        public EraserTool Eraser => eraser ??= new EraserTool(document, selectedColors, undoService);
        public FloodFillTool FloodFill => floodFill ??= new FloodFillTool(document, selectedColors, undoService);
        public AirbrushTool Airbrush => airbrush ??= new AirbrushTool(document, selectedColors, undoService);

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