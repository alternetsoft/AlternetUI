using System;
using System.IO;
using System.Text;
using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace PaintSample
{
    public partial class PaintMainWindow : Window
    {
        private readonly Tools? tools;

        private PaintSampleDocument? document;

        private readonly UndoService? undoService;

        private readonly CanvasControl canvasControl;
        private readonly ToolBar toolbar = new();
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
        private readonly HorizontalStackPanel optionsPlaceholder = new ();

        private readonly string? baseTitle;

        private ObjectUniqueId undoId;
        private ObjectUniqueId redoId;

        public PaintMainWindow()
        {
            toolbar.SetMargins(true, true, true, true);

            var optionsId = toolbar.AddControl(optionsPlaceholder);
            toolbar.SetToolAlignCenter(optionsId, true);

            Title = "AlterNET UI Paint Sample";
            Size = (750, 700);
            StartLocation = WindowStartLocation.CenterScreen;

            var menu = new MainMenu();

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

            testMenu.Add("Change Lightness...", DoChangeLightness);
            testMenu.Add("Generate sample image", DoGenImageUseGetData);
            testMenu.Add("Fill red (new with rgb and alpha arrays)", DoFillRedUseSetData);
            testMenu.Add("Fill green (new with pixel array)", DoFillGreenUseSkiaColors);
            testMenu.Add("Make file grey...", DoMakeFileGray);
            testMenu.Add("Load Toucan image", DoLoadToucanImage);
            testMenu.Add("Load Toucan image on background", DoLoadToucanOnBackground);
            testMenu.Add("Convert To Disabled", DoConvertToDisabled);
            testMenu.Add("Convert To Disabled (Skia)", DoConvertToDisabledSkia);

            /*
            if (!App.IsLinuxOS)
                testMenu.Add("Sample draw", DoDrawOnBitmap);
            */

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

            Icon = App.DefaultIcon;

            border = new();
            border.BorderColor = Splitter.DefaultBackColor;
            mainGrid.Children.Add(border);
            Alternet.UI.Grid.SetRowColumn(border, 1, 0);

            canvasControl = new();
            border.Children.Add(canvasControl);

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

            ToolBarSetTools(Tools);

            UpdateControls();

            this.Menu = menu;

            PerformLayout();
        }

        public void ToolBarSetTools(Tools tools)
        {
            toolbar.ItemSize = 32;

            foreach (var tool in tools.AllTools)
            {
                var url = AssemblyUtils.GetImageUrlInAssembly(
                    GetType().Assembly,
                    "Resources.ToolIcons." + tool.GetType().Name.Replace("Tool", "") + ".svg");

                var svgSize = ToolBarUtils.GetDefaultImageSize(this);
                var svg = new MonoSvgImage(url);

                var buttonId = toolbar.AddSpeedBtn(tool.Name, svg);

                void ClickMe()
                {
                    var sticky = toolbar.GetToolSticky(buttonId);
                    if (sticky)
                        return;
                    Tools.CurrentTool = tool;
                    var stickyGroup = toolbar.GetToolsWithCustomFlag("Tool");
                    toolbar.SetToolSticky(stickyGroup, false);
                    toolbar.SetToolSticky(buttonId, true);

                    toolbar.DoInsideLayout(() =>
                    {
                        optionsPlaceholder.Children.Clear();

                        var optionsControl = tools.CurrentTool?.OptionsControl;

                        if (optionsControl != null)
                            optionsPlaceholder.Children.Add(optionsControl);
                    });
                }

                toolbar.SetToolAction(buttonId, ClickMe);
                toolbar.GetToolCustomFlags(buttonId)?.SetFlag("Tool", true);
                if (tool == Tools.CurrentTool)
                    ClickMe();
            }
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
            Document = new PaintSampleDocument(this);
        }

        UndoService UndoService => undoService ?? throw new Exception();
        Tools Tools => tools ?? throw new Exception();

        PaintSampleDocument Document
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
            undoId = toolbar.AddSpeedBtn(KnownButton.Undo);
            redoId = toolbar.AddSpeedBtn(KnownButton.Redo);
            toolbar.SetToolAlignRight(undoId, true);
            toolbar.SetToolAlignRight(redoId, true);
            toolbar.AddToolAction(undoId, UndoButton_Click);
            toolbar.AddToolAction(redoId, RedoButton_Click);
        }

        private void UndoService_Changed(object? sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            undoMenuItem!.Enabled = UndoService.CanUndo;
            redoMenuItem!.Enabled = UndoService.CanRedo;
            saveMenuItem!.Enabled = Document.Dirty;
            toolbar.SetToolEnabled(undoId, UndoService.CanUndo);
            toolbar.SetToolEnabled(redoId, UndoService.CanRedo);

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
            PromptToSaveDocument(
                (cancel) =>
                {
                    if (!cancel)
                    {
                        CreateNewDocument();
                    }
                });
        }

        private void OpenMenuItem_Click(object? sender, EventArgs e)
        {
            PromptToSaveDocument(
                (cancel) =>
                {
                    if (!cancel)
                    {
                        var s = PathUtils.GetAppSubFolder("SampleImages");

                        using var dialog = new OpenFileDialog
                        {
                            Filter = FileMaskUtils.GetFileDialogFilterForImageOpen(false),
                            InitialDirectory = s,
                            FileMustExist = true,
                        };

                        dialog.ShowAsync(() =>
                        {
                            if (dialog.FileName == null)
                                return;
                            Document = new PaintSampleDocument(this, dialog.FileName);
                        });
                    }
                });
        }

        void PromptForSaveFileName(Action<string?> onResult)
        {
            var dialog = SaveFileDialog.Default;
            
            dialog.Filter = FileMaskUtils.GetFileDialogFilterForImageSave();
            dialog.InitialDirectory = PathUtils.GetAppSubFolder("SampleImages");

            dialog.ShowAsync(this, (result) =>
            {
                if (!result || dialog.FileName == null)
                    onResult(null);
                onResult(dialog.FileName);
            });
        }

        private void SaveMenuItem_Click(object? sender, EventArgs e)
        {
            Save();
        }

        private void SaveAs()
        {
            PromptForSaveFileName((fileName) =>
            {
                if (fileName == null)
                    return;
                Document.Save(fileName);
            });
        }

        private void Save()
        {
            var fileName = Document.FileName;

            if(fileName is null)
            {
                SaveAs();
            }
            else
            {
                Document.Save(fileName);
            }
        }

        private void SaveAsMenuItem_Click(object? sender, EventArgs e)
        {
            SaveAs();
        }

        private void PromptToSaveDocument(Action<bool> onClose)
        {
            if (document == null || !Document.Dirty)
            {
                onClose(false);
                return;
            }

            MessageBox.Show(
                "The document has been modified. Save?",
                "Paint Sample",
                MessageBoxButtons.YesNoCancel,
                defaultButton: MessageBoxDefaultButton.Cancel,
                icon: MessageBoxIcon.None,
                onClose: (e) =>
                {
                    if (e.Result == DialogResult.Cancel)
                    {
                        onClose(true);
                        return;
                    }

                    if (e.Result == DialogResult.Yes)
                    {
                        Save();

                        onClose(false);
                    }

                    onClose(false);
                });
        }

        public override bool CanClose(bool askOwned)
        {
            App.AddIdleTask(() =>
            {
                PromptToSaveDocument(
                    (cancel) =>
                    {
                        if (!cancel)
                        {
                            SendDispose();
                        }
                    });
            });

            return false;
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);
        }

        public void DoChangeLightness()
        {
            DialogFactory.AskLightnessAsync((lightness) =>
            {
                App.Log($"Image.ChangeLightness: {lightness}");
                Document.Bitmap = Document.Bitmap.ChangeLightness(lightness);
            });
        }

        public void DoConvertToDisabledSkia()
        {
            var result = SkiaUtils.ConvertToGrayscale((SKBitmap)Document.Bitmap);
            Document.Bitmap = (Bitmap)result;
        }

        public void DoConvertToDisabled()
        {
            DialogFactory.AskBrightnessAsync((value) =>
            {
                App.Log($"Image.ConvertToDisabled: {value}");
                Document.Bitmap = Document.Bitmap.ConvertToDisabled(value);
            });
        }

        public unsafe void DoFillGreenUseSkiaColors()
        {
            DialogFactory.AskTransparencyAsync((alpha) =>
            {
                App.Log($"Fill green color (alpha = {alpha}) using new image with native data");

                var height = 600;
                var width = 600;

                var pixels = GenericImage.CreatePixels(width, height, Color.Green.WithAlpha(alpha));
                Document.Bitmap = Bitmap.Create(width, height, pixels);
            }, 100);
        }

        public void DoFillRedUseSetData()
        {
            DialogFactory.AskTransparencyAsync((alpha) =>
            {
                App.Log($"Fill red color (alpha = {alpha}) using new image with native data");
                Document.Bitmap = Bitmap.Create(600, 600, Color.Red.WithAlpha(alpha));
            }, 100);
        }

        public unsafe void DoMakeFileGray()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Images | *.bmp; *.png; *.jpg; *.jpeg",
                FileMustExist = true,
            };

            dialog.ShowAsync(() =>
            {
                if (dialog.FileName is null)
                    return;

                string ext = Path.GetExtension(dialog.FileName);
                var bm = new Bitmap();
                bm.Load(dialog.FileName, BitmapType.Any);
                var image = (GenericImage)bm;
                image.ChangeToGrayScale();
                var greyBm = (Bitmap)image;
                greyBm.Save(dialog.FileName.Replace(ext, "_Gray" + ext));
                Document.Bitmap = greyBm;
            });
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
            App.Log($"Generate sample image using GenericImage.GetData");
            var bitmap = Document.Bitmap;

            var width = bitmap.Width;
            var height = bitmap.Height;
            var pixels = GenericImage.CreatePixels(width, height);

            fixed(SKColor* ptr = pixels)
            {
                var p = ptr;

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
                        var color = new SKColor(r, g, b, (byte)x);
                        *p = color;
                        p++;
                    }
                }
            }

            Document.Bitmap = Bitmap.Create(width, height, pixels);
        }

        public void DoLoadToucanImage()
        {
            Bitmap toucan = new("SampleImages/toucan.png");
            toucan.Rescale(toucan.Size * 3);

            Document.Bitmap = toucan;
        }

        public void DoLoadToucanOnBackground()
        {
            Bitmap toucan = new("SampleImages/toucan.png");
            toucan.Rescale(toucan.Size * 3);

            var backgroundSize = (toucan.Size * 1.5f).ToSize();

            var background = Image.Create(backgroundSize.Width, backgroundSize.Height, Color.LightGreen);

            background.Canvas.DrawImage(toucan, (50, 50));

            Document.Bitmap = background;
        }

        /*
        public void DoDrawOnBitmap()
        {
            CreateNewDocument();
            var bitmap = new Bitmap(1000, 800);
            bitmap.ScaleFactor = this.ScaleFactor;
            var dc = bitmap.Canvas;

            DrawSample(dc, (15, 15), bitmap.Bounds);

            Document.Bitmap = bitmap;
        }
        */

        /*
        public void DrawSample(Graphics dc, PointD location, RectD rect)
        {
            dc.FillRectangle(Color.WhiteSmoke.AsBrush, rect);

            App.LogNameValue("dc.DPI", dc.GetDPI());
            App.LogNameValue("window.DPI", GetDPI());

            var s = "Hello text";

            var font = AbstractControl.DefaultFont.Scaled(5);
            var measure = dc.MeasureText(s, font);

            var size = dc.GetTextExtent(s, font);

            App.Log($"GetTextExtent: {measure}, {size}");

            RectD r1 = (location.X, location.Y, measure.Width, measure.Height);
            RectD r2 = (location.X, location.Y, size.Width, size.Height);

            DrawingUtils.DrawBorderWithBrush(dc, Color.DarkRed.AsBrush, r1, 1);

            DrawingUtils.DrawBorderWithBrush(dc, Color.Red.AsBrush, r2, 1);

            dc.DrawWave((location.X, location.Y, size.Width, size.Height), Color.Green);

            dc.DestroyClippingRegion();
            dc.SetClippingRegion((location.X + size.Width / 2, location.Y, size.Width, size.Height));
            dc.DrawText(s, location, font, Color.Black, Color.Empty);

            dc.DestroyClippingRegion();
            dc.SetClippingRegion((location.X, location.Y, size.Width / 2, size.Height));
            dc.DrawText(s, location, font, Color.Green, Color.Empty);
            dc.DestroyClippingRegion();
        }
        */
    }
}