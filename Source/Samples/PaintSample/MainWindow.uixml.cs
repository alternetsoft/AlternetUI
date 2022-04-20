using Alternet.UI;
using System;

namespace PaintSample
{
    public partial class MainWindow : Window
    {
        private Tools tools;

        private Document document;

        private ColorSelector colorSelector;

        private UndoService undoService;

        private Tool? currentTool;

        public MainWindow()
        {
            InitializeComponent();

            document = new Document();

            colorSelector = new ColorSelector();
            undoService = new UndoService();
            tools = new Tools(document, colorSelector, undoService);

            canvasControl.Document = document;

            CurrentTool = tools.Pen;
        }

        private Tool CurrentTool
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
            }
        }
    }
}