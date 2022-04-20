namespace PaintSample
{
    internal class Tools
    {
        private readonly Document document;
        private readonly ISelectedColors selectedColors;
        private readonly UndoService undoService;

        PenTool? pen;

        public Tools(Document document, ISelectedColors selectedColors, UndoService undoService)
        {
            this.document = document;
            this.selectedColors = selectedColors;
            this.undoService = undoService;
        }

        public PenTool Pen => pen ??= new PenTool(document, selectedColors, undoService);
    }
}