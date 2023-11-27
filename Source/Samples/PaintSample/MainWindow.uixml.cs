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
        /*private const string FileDialogImageFilesFilter =
            "Image files (*.png; *.jpg)|*.png;*.jpg|All files (*.*)|*.*";*/
        private readonly Tools? tools;

        private Document? document;

        private readonly UndoService? undoService;

        private readonly CanvasControl canvasControl;
        private readonly Toolbar toolbar;
        private readonly ColorSelector colorSelector;
        private readonly Border border;
        private readonly Grid mainGrid;

        CommandButton? undoButton;
        CommandButton? redoButton;

        private readonly MenuItem toolsMenu;
        private readonly MenuItem helpMainMenu;
        private readonly MenuItem aboutMenuItem;
        private readonly MenuItem editMainMenu;
        private readonly MenuItem undoMenuItem;
        private readonly MenuItem redoMenuItem;
        private readonly MenuItem copyMenuItem;
        private readonly MenuItem pasteMenuItem;
        private readonly MenuItem fileMainMenu;
        private readonly MenuItem newMenuItem;
        private readonly MenuItem openMenuItem;
        private readonly MenuItem saveMenuItem;
        private readonly MenuItem saveAsMenuItem;
        private readonly MenuItem exitMenuItem;

        private readonly string? baseTitle;

        public MainWindow()
        {
            InitializeComponent();

            Title = "AlterNET UI Paint Sample";
            Width = 750;
            Height = 700;
            StartLocation = WindowStartLocation.CenterScreen;

            var menu = Menu!;

            fileMainMenu = "_File";
            menu.Items.Add(fileMainMenu);

            newMenuItem = new("_New", NewMenuItem_Click, "Ctrl+N");
            fileMainMenu.Items.Add(newMenuItem);
            openMenuItem = new("_Open...", OpenMenuItem_Click, "Ctrl+O");
            fileMainMenu.Items.Add(openMenuItem);
            fileMainMenu.Items.Add("-");
            saveMenuItem = new("_Save", SaveMenuItem_Click, "Ctrl+S");
            fileMainMenu.Items.Add(saveMenuItem);
            saveAsMenuItem = new(
                "_Save As...", 
                SaveAsMenuItem_Click, 
                "Ctrl+Shift+S");
            fileMainMenu.Items.Add(saveAsMenuItem);
            fileMainMenu.Items.Add("-");
            exitMenuItem = new("E_xit", ExitMenuItem_Click);
            fileMainMenu.Items.Add(exitMenuItem);

            editMainMenu = "_Edit";
            menu.Items.Add(editMainMenu);

            undoMenuItem = new("_Undo", UndoMenuItem_Click, "Ctrl+Z");
            editMainMenu.Items.Add(undoMenuItem);
            redoMenuItem = new("_Redo", RedoMenuItem_Click, "Ctrl+Y");
            editMainMenu.Items.Add(redoMenuItem);
            editMainMenu.Items.Add("-");
            copyMenuItem = new("_Copy", CopyMenuItem_Click, "Ctrl+C");
            editMainMenu.Items.Add(copyMenuItem);
            pasteMenuItem = new("_Paste", PasteMenuItem_Click, "Ctrl+V");
            editMainMenu.Items.Add(pasteMenuItem);

            toolsMenu = new("_Tools");
            menu.Items.Add(toolsMenu);

            helpMainMenu = new("_Help");            
            menu.Items.Add(helpMainMenu);

            aboutMenuItem = new("_About", AboutMenuItem_Click);
            helpMainMenu.Items.Add(aboutMenuItem);

            mainGrid = new();
            Children.Add(mainGrid);

            mainGrid.RowDefinitions.Add(new RowDefinition { 
                Height = new GridLength() });
            mainGrid.RowDefinitions.Add(new RowDefinition { 
                Height = new GridLength(100, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { 
                Height = new GridLength() });

            Icon = new("embres:PaintSample.Sample.ico");

            border = new();
            mainGrid.Children.Add(border);
            Alternet.UI.Grid.SetRowColumn(border, 1, 0);

            canvasControl = new();
            border.Children.Add(canvasControl);

            toolbar = new();
            colorSelector = new();

            Alternet.UI.Grid.SetRow(toolbar, 0);
            Alternet.UI.Grid.SetColumn(toolbar, 0);

            Alternet.UI.Grid.SetRow(colorSelector, 2);
            Alternet.UI.Grid.SetColumn(colorSelector, 0);

            mainGrid.Children.Add(toolbar);
            mainGrid.Children.Add(colorSelector);

            baseTitle = Title;

            InitializeCommandButtons();

            undoService = new UndoService();
            undoService.Changed += UndoService_Changed;

            CreateNewDocument();

            tools = new Tools(() => Document, colorSelector, undoService, canvasControl);
            Tools.CurrentTool = Tools.Pen;
            InitializeToolsMenuItems();

            toolbar.SetTools(Tools);

            UpdateControls();

            PerformLayout();
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

        UndoService UndoService => undoService ?? throw new Exception();
        Tools Tools => tools ?? throw new Exception();

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

                UndoService.Document = value;
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
                Tools.CurrentTool = (Tool)((MenuItem)sender!).Tag!;
            }

            void UpdateCheckedItems()
            {
                var myTool = Tools.CurrentTool;

                foreach (var item in toolsMenu.Items)
                    item.Checked = item.Tag == myTool;
            }

            foreach (var tool in Tools.AllTools)
            {
                toolsMenu.Items.Add(new MenuItem(tool.Name, ToolMenuItem_Click) { Tag = tool });
            }

            UpdateCheckedItems();

            Tools.CurrentToolChanged += (_, __) => UpdateCheckedItems();
        }

        private void InitializeCommandButtons()
        {
            undoButton = new("Undo")
            {
                ToolTip = "Undo"
            };
            undoButton.Click += UndoButton_Click;
            toolbar.CommandButtons.Add(undoButton);

            redoButton = new("Redo")
            {
                ToolTip = "Redo"
            };
            redoButton.Click += RedoButton_Click;
            toolbar.CommandButtons.Add(redoButton);
        }

        private void UndoService_Changed(object? sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            undoMenuItem!.Enabled = undoButton!.Enabled = UndoService.CanUndo;
            redoMenuItem!.Enabled = redoButton!.Enabled = UndoService.CanRedo;
            saveMenuItem!.Enabled = Document.Dirty;

            UpdateTitle();
        }

        void UndoButton_Click(object? sender, EventArgs e)
        {
            Undo();
        }

        private void Undo()
        {
            UndoService.Undo();
        }

        void RedoButton_Click(object? sender, EventArgs e)
        {
            Redo();
        }

        private void Redo()
        {
            UndoService.Redo();
        }

        private void ExitMenuItem_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void UndoMenuItem_Click(object? sender, EventArgs e)
        {
            Undo();
        }

        private void RedoMenuItem_Click(object? sender, EventArgs e)
        {
            Redo();
        }

        private void CopyMenuItem_Click(object? sender, EventArgs e)
        {
            Clipboard.SetBitmap(Document.Bitmap);
            UpdateControls();
        }

        private void PasteMenuItem_Click(object? sender, EventArgs e)
        {
            if (Clipboard.ContainsBitmap)
            {
                var bitmap = Clipboard.GetBitmap();
                if (bitmap != null)
                    Document.Bitmap = bitmap;
            }
        }

        private void AboutMenuItem_Click(object? sender, System.EventArgs e)
        {
            MessageBox.Show("AlterNET UI Paint Sample Application", "About");
        }

        private void NewMenuItem_Click(object? sender, EventArgs e)
        {
            PromptToSaveDocument(out var cancel);
            if (cancel)
                return;

            CreateNewDocument();
        }

        private void OpenMenuItem_Click(object? sender, EventArgs e)
        {
            PromptToSaveDocument(out var cancel);
            if (cancel)
                return;

            var s = PathUtils.GetAppSubFolder("SampleImages");

            using var dialog = new OpenFileDialog
            {
                Filter = FileMaskUtils.GetFileDialogFilterForImageOpen(false),
                InitialDirectory = s,
            };            

            if (dialog.ShowModal(this) != ModalResult.Accepted || dialog.FileName == null)
                return;

            Document = new Document(dialog.FileName);
        }

        string? PromptForSaveFileName()
        {
            using var dialog = new SaveFileDialog
            {
                Filter = FileMaskUtils.GetFileDialogFilterForImageSave(),
                InitialDirectory = PathUtils.GetAppSubFolder("SampleImages"),
            };

            if (dialog.ShowModal(this) != ModalResult.Accepted || dialog.FileName == null)
                return null;

            return dialog.FileName;
        }

        private void SaveMenuItem_Click(object? sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            var fileName = Document.FileName;
            fileName ??= PromptForSaveFileName();
            if (fileName == null)
                return;

            Document.Save(fileName);
        }

        private void SaveAsMenuItem_Click(object? sender, EventArgs e)
        {
            var fileName = PromptForSaveFileName();
            if (fileName == null)
                return;

            Document.Save(fileName);
        }

        private void PromptToSaveDocument(out bool cancel)
        {
            cancel = false;
            if (document == null)
                return;

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
            if (!cancel) 
            {
                this.Hide();
                Alternet.UI.Application.Current.Exit();
            }

            base.OnClosing(e);
        }
    }
}