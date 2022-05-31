using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace PaintSample
{
    public partial class MainWindow : Window
    {
        private Tools tools;

        private Document document;

        private UndoService undoService;

        CommandButton? undoButton;
        CommandButton? redoButton;

        public MainWindow()
        {
            InitializeComponent();

            border.BorderBrush = Brushes.Black;
            border.Padding = new Thickness();

            document = new Document();

            undoService = new UndoService(document);
            undoService.Changed += UndoService_Changed;

            canvasControl.Document = document;

            tools = new Tools(document, colorSelector, undoService, canvasControl);
            tools.CurrentTool = tools.Pen;
            InitializeToolsMenuItems();

            toolbar.SetTools(tools);

            InitializeCommandButtons();

            UpdateControls();
        }

        private void InitializeToolsMenuItems()
        {
            void ToolMenuItem_Click(object? sender, System.EventArgs e)
            {
                tools.CurrentTool = (Tool)((MenuItem)sender!).Tag!;
            }

            void UpdateCheckedItems()
            {
                foreach (var item in toolsMenu.Items)
                    item.Checked = item.Tag == tools.CurrentTool;
            }

            foreach (var tool in tools.AllTools)
            {
                toolsMenu.Items.Add(new MenuItem(tool.Name, ToolMenuItem_Click) { Tag = tool });
            }

            UpdateCheckedItems();

            tools.CurrentToolChanged += (_, __) => UpdateCheckedItems();
        }

        private void InitializeCommandButtons()
        {
            undoButton = new CommandButton("Undo");
            undoButton.Click += UndoButton_Click;
            toolbar.CommandButtons.Add(undoButton);

            redoButton = new CommandButton("Redo");
            redoButton.Click += RedoButton_Click;
            toolbar.CommandButtons.Add(redoButton);
        }

        private void UndoService_Changed(object? sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            undoButton!.Enabled = undoService.CanUndo;
            redoButton!.Enabled = undoService.CanRedo;
        }

        void UndoButton_Click(object? sender, EventArgs e)
        {
            undoService.Undo();
        }

        void RedoButton_Click(object? sender, EventArgs e)
        {
            undoService.Redo();
        }

        private void ExitMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void AboutMenuItem_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("AlterNET UI Paint Sample Application", "About");
        }
    }
}