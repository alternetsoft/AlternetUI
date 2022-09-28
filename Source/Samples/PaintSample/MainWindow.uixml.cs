using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace PaintSample
{
    public partial class MainWindow : Window
    {
        private Tools tools;

        private Document? document;

        private UndoService undoService;

        CommandButton? undoButton;
        CommandButton? redoButton;

        public MainWindow()
        {
            InitializeComponent();

            border.BorderBrush = Brushes.Black;
            border.Padding = new Thickness();

            InitializeCommandButtons();

            undoService = new UndoService();
            undoService.Changed += UndoService_Changed;

            CreateNewDocument();

            tools = new Tools(() => Document, colorSelector, undoService, canvasControl);
            tools.CurrentTool = tools.Pen;
            InitializeToolsMenuItems();

            toolbar.SetTools(tools);

            UpdateControls();
        }

        private void CreateNewDocument()
        {
            Document = new Document();
        }

        Document Document
        {
            get => document ?? throw new Exception();

            set
            {
                if (document == value)
                    return;

                document = value;
                undoService.Document = value;
                canvasControl.Document = value;
            }
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
            undoMenuItem!.Enabled = undoButton!.Enabled = undoService.CanUndo;
            redoMenuItem!.Enabled = redoButton!.Enabled = undoService.CanRedo;
            saveMenuItem!.Enabled = Document.Dirty;
        }

        void UndoButton_Click(object? sender, EventArgs e)
        {
            Undo();
        }

        private void Undo()
        {
            undoService.Undo();
        }

        void RedoButton_Click(object? sender, EventArgs e)
        {
            Redo();
        }

        private void Redo()
        {
            undoService.Redo();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UndoMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void RedoMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void AboutMenuItem_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("AlterNET UI Paint Sample Application", "About");
        }

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewDocument();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}