using System;
using System.IO;
using System.Text;
using Alternet.Drawing;
using Alternet.UI;

namespace PaintSample
{
    public partial class MainWindow : Window
    {
        private readonly Tools? tools;

        private Document? document;

        private readonly UndoService? undoService;

        private readonly CanvasControl canvasControl;
        private readonly Toolbar toolbar;
        private readonly ColorSelector colorSelector;
        private readonly Border border;
        private readonly Grid mainGrid;

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
        private readonly MenuItem testMenu;

        private readonly string? baseTitle;

        private CommandButton? undoButton;
        private CommandButton? redoButton;

        public MainWindow()
        {
            InitializeComponent();

            Title = "AlterNET UI Paint Sample";
            Size = (750, 700);
            StartLocation = WindowStartLocation.CenterScreen;

            var menu = Menu!;

            fileMainMenu = menu.Add("_File");
            editMainMenu = menu.Add("_Edit");

            newMenuItem = new("_New", NewMenuItem_Click, "Ctrl+N");
            fileMainMenu.Add(newMenuItem);
            openMenuItem = new("_Open...", OpenMenuItem_Click, "Ctrl+O");
            fileMainMenu.Add(openMenuItem);
            fileMainMenu.Add("-");
            saveMenuItem = new("_Save", SaveMenuItem_Click, "Ctrl+S");
            fileMainMenu.Add(saveMenuItem);

            saveAsMenuItem = new(
                "_Save As...",
                SaveAsMenuItem_Click,
                "Ctrl+Shift+S");
            fileMainMenu.Add(saveAsMenuItem);
            fileMainMenu.Add("-");
            exitMenuItem = fileMainMenu.Add(new("E_xit", ExitMenuItem_Click));

            undoMenuItem = new("_Undo", UndoMenuItem_Click, "Ctrl+Z");
            editMainMenu.Add(undoMenuItem);
            redoMenuItem = new("_Redo", RedoMenuItem_Click, "Ctrl+Y");
            editMainMenu.Add(redoMenuItem);
            editMainMenu.Add("-");
            copyMenuItem = new("_Copy", CopyMenuItem_Click, "Ctrl+C");
            editMainMenu.Add(copyMenuItem);
            pasteMenuItem = new("_Paste", PasteMenuItem_Click, "Ctrl+V");
            editMainMenu.Add(pasteMenuItem);

            toolsMenu = menu.Add("_Tools");

            testMenu = menu.Add("Actions");

            testMenu.Add("Lightness (GenericImage.ChangeLightness)", DoChangeLightness);
            testMenu.Add("Gen sample image (GenericImage.GetData)", DoGenImageUseGetData);
            testMenu.Add("Lightness (GenericImage.GetData)", DoChangeLightnessUseGetData);
            testMenu.Add("Fill red (new GenericImage with native data)", DoFillRedUseSetData);
            testMenu.Add("Make file grey...", DoMakeFileGray);
            testMenu.Add("Sample draw", DoDrawOnBitmap);
            testMenu.Add("Rotate", DoRotate);

            helpMainMenu = new("_Help");
            menu.Add(helpMainMenu);

            aboutMenuItem = new("_About", AboutMenuItem_Click);
            helpMainMenu.Add(aboutMenuItem);

            mainGrid = new();
            Children.Add(mainGrid);

            mainGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength()
            });
            mainGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(100, GridUnitType.Star)
            });
            mainGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength()
            });

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
            Document = new Document(this);
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
            if (Clipboard.ContainsBitmap())
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

            Document = new Document(this, dialog.FileName);
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
                defaultButton: MessageBoxDefaultButton.Cancel,
                icon: MessageBoxIcon.None);

            if (result == DialogResult.Cancel)
            {
                cancel = true;
                return;
            }

            if (result == DialogResult.Yes)
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

        public void DoChangeLightness()
        {
            var result = DialogFactory.GetNumberFromUser(null, "Lightness (0..200)", null, 100, 0, 200);
            if (result is null)
                return;
            Application.Log($"Image.ChangeLightness: {result}");
            var bitmap = Document.Bitmap;
            GenericImage image = (GenericImage)bitmap;
            var converted = image.ChangeLightness((int)result.Value);
            Document.Bitmap = (Bitmap)converted;
        }

        public void DoChangeLightnessUseGetData()
        {
            var result = DialogFactory.GetNumberFromUser(null, "Lightness (0..200)", null, 100, 0, 200);
            if (result is null)
                return;
            Application.Log($"Change lightness using GenericImage.GetData");
            var bitmap = Document.Bitmap;
            GenericImage image = (GenericImage)bitmap;
            var lightness = (int)result.Value;
            image.ForEachPixel(Color.ChangeLightness, lightness);
            Document.Bitmap = (Bitmap)image;
        }

        public unsafe void DoFillRedUseSetData()
        {
            var result = DialogFactory.GetNumberFromUser(null, "Transparency (0..255)", null, 100, 0, 255);
            if (result is null)
                return;

            var alpha = (byte)result.Value;
            Application.Log($"Fill red color (alpha = {alpha}) using new image with native data");

            SizeI size = (600, 600);
            var pixelCount = size.PixelCount;
            var height = size.Height;
            var width = size.Width;
            RGBValue rgb = Color.Red;
            byte r = rgb.R, g = rgb.G, b = rgb.B;

            var alphaData = BaseMemory.Alloc(pixelCount);
            BaseMemory.Fill(alphaData, alpha, pixelCount);

            var dataPtr = BaseMemory.Alloc(pixelCount * 3);
            byte* data = (byte*)dataPtr;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    *data++ = r;
                    *data++ = g;
                    *data++ = b;
                }
            }

            GenericImage image = new(size.Width, size.Height, dataPtr, alphaData, false);
            Document.Bitmap = (Bitmap)image;
        }

        public unsafe void DoMakeFileGray()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Images | *.bmp; *.png; *.jpg; *.jpeg"
            };

            if (dialog.ShowModal(this) != ModalResult.Accepted || dialog.FileName == null)
                return;

            string ext = Path.GetExtension(dialog.FileName);
            var bm = new Bitmap();
            bm.Load(dialog.FileName, BitmapType.Any);
            var image = (GenericImage)bm;
            image.ChangeToGrayScale();
            var greyBm = (Bitmap)image;
            greyBm.Save(dialog.FileName.Replace(ext, "_Gray" + ext));
            Document.Bitmap = greyBm;
        }

        public void DoRotate()
        {
            var bitmap = Document.Bitmap;
            GenericImage image = (GenericImage)bitmap;
            var newImage = image.Rotate90(); 
            Document.Bitmap = (Bitmap)newImage;
        }

        public unsafe void DoGenImageUseGetData()
        {
            Application.Log($"Generate sample image using GenericImage.GetData");
            var bitmap = Document.Bitmap;
            GenericImage image = (GenericImage)bitmap;
            image.InitAlpha();
            var ndata = image.GetNativeData();
            var nalpha = image.GetNativeAlphaData();

            byte* data = (byte*)ndata;
            byte* alpha = (byte*)nalpha;

            var height = image.Height;
            var width = image.Width;

            for (int y = 0; y < height; y++)
            {
                byte r = 0, g = 0, b = 0;
                if (y < height / 3)
                    r = 0xff;
                else if (y < (2 * height) / 3)
                    g = 0xff;
                else
                    b = 0xff;

                for (int x = 0; x < width; x++)
                {
                    *alpha++ = (byte)x;
                    *data++ = r;
                    *data++ = g;
                    *data++ = b;
                }
            }

            Document.Bitmap = (Bitmap)image;
        }

        public void DoDrawOnBitmap()
        {
            CreateNewDocument();
            var bitmap = Document.Bitmap;
            var dc = bitmap.Canvas;

            DrawSample(dc, (15, 15));
        }

        public void DrawSample(Graphics dc, PointD location)
        {
            Application.LogNameValue("dc.DPI", dc.GetDPI());
            Application.LogNameValue("window.DPI", GetDPI());

            var s = "Hello text";

            var font = Control.DefaultFont.Scaled(5);
            var measure = dc.MeasureText(s, font);

            var size = dc.GetTextExtent(
                s,
                font,
                out double descent,
                out double externalLeading,
                null);

            dc.DestroyClippingRegion();
            dc.SetClippingRegion((location.X + size.Width / 2, location.Y, size.Width, size.Height));
            dc.DrawText(s, location, font, Color.Black, Color.Empty);

            dc.DestroyClippingRegion();
            dc.SetClippingRegion((location.X, location.Y, size.Width / 2, size.Height));
            dc.DrawText(s, location, font, Color.Green, Color.Empty);
            dc.DestroyClippingRegion();

            dc.DrawRectangle(Color.DarkRed.AsPen, (location.X, location.Y, measure.Width, measure.Height));


            dc.DrawRectangle(Color.Red.AsPen, (location.X, location.Y, size.Width, size.Height));



            var y = location.Y - descent + size.Height;
            dc.DrawLine(Color.RosyBrown.AsPen, (location.X, y), (location.X + size.Width, y));

            dc.DrawWave((location.X, location.Y, size.Width, size.Height), Color.Green);

            var size1 = dc.MeasureText("x", Font.Default);

            var size2 = dc.GetTextExtent(
                "x",
                Font.Default,
                out _,
                out _,
                null);

            // <param name="descent">Dimension from the baseline of the font to
            // the bottom of the descender (the size of the tail below the baseline).</param>
            // <param name="externalLeading">Any extra vertical space added to the
            // font by the font designer (inter-line interval).</param>
        }
    }
}