using Alternet.UI;
using System;

namespace PaintSample
{
    public partial class MainWindow : Window
    {
        private Tools tools;

        private Document document;

        private UndoService undoService;


        public MainWindow()
        {
            InitializeComponent();

            document = new Document();

            undoService = new UndoService(document);
            undoService.Changed += UndoService_Changed;

            canvasControl.Document = document;

            tools = new Tools(document, colorSelector, undoService, canvasControl);

            tools.CurrentTool = tools.Pen;

            toolbar.SetTools(tools);

            UpdateControls();
        }

        private void UndoService_Changed(object? sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            undoButton.Enabled = undoService.CanUndo;
            redoButton.Enabled = undoService.CanRedo;
        }

        void UndoButton_Click(object? sender, EventArgs e)
        {
            undoService.Undo();
        }

        void RedoButton_Click(object? sender, EventArgs e)
        {
            undoService.Redo();
        }
    }
}