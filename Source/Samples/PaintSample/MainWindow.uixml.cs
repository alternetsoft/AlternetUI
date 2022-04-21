using Alternet.UI;
using System;

namespace PaintSample
{
    public partial class MainWindow : Window
    {
        private Tools tools;

        private Document document;

        private UndoService undoService;

        private Tool? currentTool;

        public MainWindow()
        {
            InitializeComponent();

            document = new Document();

            undoService = new UndoService(document);
            undoService.Changed += UndoService_Changed;

            tools = new Tools(document, colorSelector, undoService);

            canvasControl.Document = document;

            CurrentTool = tools.Pen;

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