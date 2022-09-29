using Alternet.Drawing;
using Alternet.UI;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PaintSample
{
    public partial class MainWindow : Window
    {
        private const string FileDialogImageFilesFilter = "Image files(*.png; *.jpg)|*.png;*.jpg|All files(*.*)|*.*";
        private Tools tools;

        private Document? document;

        private UndoService undoService;

        CommandButton? undoButton;
        CommandButton? redoButton;

        string baseTitle;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialize()
        {
            baseTitle = Title;

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

        void UpdateTitle()
        {
            var title = new StringBuilder();


            title.Append(baseTitle);

            title.Append(" - ");

            if (Document.FileName != null)
                title.Append(Path.GetFileName(Document.FileName));
            else
                title.Append("Untitled");

            if (Document.Dirty)
                title.Append('*');

            Title = title.ToString();
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

                if (document != null)
                    document.Changed -= Document_Changed;

                document = value;

                if (document != null)
                    document.Changed += Document_Changed;

                undoService.Document = value;
                canvasControl.Document = value;
                UpdateControls();
            }
        }

        private void Document_Changed(object? sender, EventArgs e)
        {
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
            undoMenuItem!.Enabled = undoButton!.Enabled = undoService.CanUndo;
            redoMenuItem!.Enabled = redoButton!.Enabled = undoService.CanRedo;
            saveMenuItem!.Enabled = Document.Dirty;

            UpdateTitle();
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
            PromptToSaveDocument(out var cancel);
            if (cancel)
                return;

            CreateNewDocument();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            PromptToSaveDocument(out var cancel);
            if (cancel)
                return;

            using var dialog = new OpenFileDialog
            {
                Filter = FileDialogImageFilesFilter
            };

            if (dialog.ShowModal(this) != ModalResult.Accepted || dialog.FileName == null)
                return;

            Document = new Document(dialog.FileName);
        }

        string? PromptForSaveFileName()
        {
            using var dialog = new SaveFileDialog
            {
                Filter = FileDialogImageFilesFilter
            };

            if (dialog.ShowModal(this) != ModalResult.Accepted || dialog.FileName == null)
                return null;

            return dialog.FileName;
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            var fileName = Document.FileName;
            if (fileName == null)
                fileName = PromptForSaveFileName();
            if (fileName == null)
                return;

            Document.Save(fileName);
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            var fileName = PromptForSaveFileName();
            if (fileName == null)
                return;

            Document.Save(fileName);
        }

        private void PromptToSaveDocument(out bool cancel)
        {
            cancel = false;

            if (!Document.Dirty)
                return;

            var result = MessageBox.Show(
                this,
                "The document has been modified. Save?",
                "Paint Sample",
                MessageBoxButtons.YesNoCancel,
                defaultButton: MessageBoxDefaultButton.Cancel);

            if (result == MessageBoxResult.Cancel)
            {
                cancel = true;
                return;
            }

            if (result == MessageBoxResult.Yes)
                Save();
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            PromptToSaveDocument(out var cancel);
            e.Cancel = cancel;
            base.OnClosing(e);
        }
    }
}