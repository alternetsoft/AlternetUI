using System;
using Alternet.Drawing;
using Alternet.Drawing.Printing;
using Alternet.UI;

namespace PrintingSample
{
    public partial class PrintingMainWindow : Window
    {
        private static readonly Font font = new(FontFamily.GenericSerif, 20);
        private static readonly Pen thickGrayPen = Color.Gray.GetAsPen(4);

        private readonly ToolBar toolBar = new()
        {
        };

        public PrintingMainWindow()
        {
            toolBar.Parent = this;
            toolBar.SetVisibleBorders(false, false, false, true);
            toolBar.Padding = 10;

            toolBar.AddTextBtn("Print Immediately", null, PrintImmediatelyMenuItem_Click);
            toolBar.AddSpacer();
            toolBar.AddTextBtn("Print...", null, PrintMenuItem_Click);
            toolBar.AddSpacer();
            toolBar.AddTextBtn("Page Setup...", null, PageSetupMenuItem_Click);
            toolBar.AddSpacer();
            toolBar.AddTextBtn("Print Preview...", null, PrintPreviewMenuItem_Click);

            toolBar.SpeedButtons.SetUseTheme(SpeedButton.KnownTheme.RoundBorder);

            Icon = App.DefaultIcon;
            InitializeComponent();

            DrawingArea.UserPaint = true;

            // This is important if OS style is BlackOnWhite.
            DrawingArea.BackgroundColor = Color.White;
        }

        private void DrawingArea_Paint(object? sender, PaintEventArgs e)
        {
            var dc = e.Graphics;
            var bounds = e.ClientRectangle;

            if (originAtMarginCheckBox.IsChecked)
            {
                var margins = TryGetPageMargins();
                if (margins != null)
                    bounds.Inflate(margins.Value.Size * -1);
            }

            DrawFirstPage(dc, bounds);
        }

        public static void DrawFirstPage(Graphics dc, RectD bounds, bool forScreen = true, int pageIndex = 1)
        {
            string TextToDraw = $"The brown fox jumps over the lazy dog. Page {pageIndex}";

            dc.DrawRectangle(Pens.Blue, bounds);

            var cornerRectSize = new SizeD(50, 50);
            var cornerRectLeft = new RectD(bounds.Location, cornerRectSize);
            var cornerRectRight =
                new RectD(bounds.TopRight - new SizeD(cornerRectSize.Width, 0), cornerRectSize);

            dc.DrawRectangle(Pens.Red, cornerRectLeft);
            dc.DrawRectangle(Pens.Red, cornerRectRight);

            var drawTextBounds = bounds.InflatedBy(-50, -50);

            var dtbWidth = drawTextBounds.Width;

            var wrappedText = DrawingUtils.WrapTextToMultipleLines(
                TextToDraw,
                dtbWidth,
                font,
                dc);

            dc.DrawText(
                wrappedText,
                font,
                Brushes.Black,
                drawTextBounds.Location);

            dc.FillEllipse(Brushes.Gold, cornerRectLeft.InflatedBy(-5, -5));
            dc.FillEllipse(Brushes.Gold, cornerRectRight.InflatedBy(-5, -5));

            dc.DrawEllipse(Pens.Goldenrod, cornerRectLeft.InflatedBy(-5, -5));
            dc.DrawEllipse(Pens.Goldenrod, cornerRectRight.InflatedBy(-5, -5));

            dc.DrawLine(thickGrayPen, cornerRectLeft.Center, cornerRectRight.Center);

            var A4Size = GraphicsUnitConverter.ConvertSize(
                GraphicsUnit.Inch,
                GraphicsUnit.Dip,
                dc.GetDPI(),
                PaperSizes.SizeInches.A4);

            if (forScreen)
            {
                dc.DrawVertLine(Brushes.Red, (A4Size.Width, 0), A4Size.Height, 1);
                dc.DrawHorzLine(Brushes.Red, (0, A4Size.Height), A4Size.Width, 1);
            }
        }

        private void PrintImmediatelyMenuItem_Click(object? sender, System.EventArgs e)
        {
            var document = CreatePrintDocument();
            document.PrintPage += Document_PrintPage;
            document.Print();
        }

        Thickness? TryGetPageMargins()
        {
            if (Thickness.TryParse(pageMarginTextBox.Text, out var thickness))
                return thickness;
            return null;
        }

        private PrintDocument CreatePrintDocument()
        {
            var document = new PrintDocument
            {
                OriginAtMargins = originAtMarginCheckBox.IsChecked,
                DocumentName = printDocumentNameTextBox.Text,
            };

            document.PrinterSettings.FromPage = 1;
            document.PrinterSettings.MinimumPage = 1;
            
            var maxPage = additionalPagesCountNumericUpDown.Value + 1;
            document.PrinterSettings.MaximumPage = maxPage;
            document.PrinterSettings.ToPage = maxPage;

            document.PageSettings.Color = printInColorCheckBox.IsChecked;
            document.PageSettings.Margins = TryGetPageMargins() ?? new();

            return document;
        }

        private void PrintMenuItem_Click(object? sender, System.EventArgs e)
        {
            var dialog = new PrintDialog();
            var document = CreatePrintDocument();

            dialog.Document = document;
            dialog.AllowSelection = true;
            dialog.AllowSomePages = false;

            document.PrintPage += Document_PrintPage;

            dialog.ShowAsync(() =>
            {
                document.Print();
            });
        }

        private void PageSetupMenuItem_Click(object? sender, System.EventArgs e)
        {
            var pageSetupDialog = new PageSetupDialog();
            var document = CreatePrintDocument();

            pageSetupDialog.Document = document;
            //setupDlg.AllowMargins = false;
            //setupDlg.AllowOrientation = false;
            //setupDlg.AllowPaper = false;
            //setupDlg.AllowPrinter = false;

            pageSetupDialog.ShowAsync(() =>
            {
                //document.DefaultPageSettings = pageSetupDialog.PageSettings;
                //document.PrinterSettings = pageSetupDialog.PrinterSettings;

                document.PrintPage += Document_PrintPage;
                document.Print();
            });
        }

        private void PrintPreviewMenuItem_Click(object? sender, System.EventArgs e)
        {
            var dialog = new PrintPreviewDialog();
            var document = CreatePrintDocument();

            document.PrintPage += Document_PrintPage;
            
            dialog.Document = document;
            dialog.ShowAsync();
        }

        private void Document_PrintPage(object? sender, PrintPageEventArgs e)
        {
            int pageNumber = e.PageNumber;

            var bounds = new RectD(new PointD(), originAtMarginCheckBox.IsChecked
                ? e.MarginBounds.Size : e.PrintablePageBounds.Size);

            e.DrawingContext.Save();

            if (pageNumber <= 2)
            {
                DrawFirstPage(e.DrawingContext, bounds, false, pageNumber);
            }
            else
            {
                DrawAdditionalPage(e.DrawingContext, pageNumber, bounds);
            }

            e.DrawingContext.Restore();

            var v = additionalPagesCountNumericUpDown.Value;

            e.HasMorePages = pageNumber - 1 < v;
        }

        public static void DrawAdditionalPage(Graphics dc, int pageNumber, RectD bounds)
        {
            dc.DrawText(
                "Additional page #" + pageNumber,
                font,
                Brushes.Black,
                bounds.Location + new SizeD(10, 10));
        }

        private void AboutMenuItem_Click(object sender, System.EventArgs e) =>
            MessageBox.Show("AlterNET UI Printing Sample Application.", "About");

        private void ExitMenuItem_Click(object sender, System.EventArgs e) => Close();

        private void OriginAtMarginCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            DrawingArea.Invalidate();
        }

        private void PageMarginTextBox_TextChanged(object? sender, EventArgs e)
        {
            DrawingArea.Invalidate();
        }
    }
}