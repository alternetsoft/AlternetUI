using System;
using Alternet.Drawing;
using Alternet.Drawing.Printing;
using Alternet.UI;

namespace PrintingSample
{
    public partial class MainWindow : Window
    {
        private readonly Font font = new(FontFamily.GenericSerif, 25);

        public MainWindow()
        {
            Icon = new("embres:PrintingSample.Sample.ico");
            InitializeComponent();
            DrawingArea.UserPaint = true;

            // This is important if OS style is BlackOnWhite.
            DrawingArea.BackgroundColor = Color.White;
        }

        private void DrawingArea_Paint(object? sender, PaintEventArgs e)
        {
            var dc = e.DrawingContext;
            var bounds = e.Bounds;

            if (originAtMarginCheckBox.IsChecked)
            {
                var margins = TryGetPageMargins();
                if (margins != null)
                    bounds.Inflate(margins.Value.Size * -1);
            }

            DrawFirstPage(dc, bounds);
        }

        private readonly Pen thickGrayPen = new(Color.Gray, 4);

        private void DrawFirstPage(Graphics dc, RectD bounds)
        {
            dc.DrawRectangle(Pens.Blue, bounds);

            var cornerRectSize = new SizeD(50, 50);
            var cornerRectLeft = new RectD(bounds.Location, cornerRectSize);
            var cornerRectRight =
                new RectD(bounds.TopRight - new SizeD(cornerRectSize.Width, 0), cornerRectSize);

            dc.DrawRectangle(Pens.Red, cornerRectLeft);
            dc.DrawRectangle(Pens.Red, cornerRectRight);

            var drawTextBounds = bounds.InflatedBy(-50, -50);

            dc.DrawText(
                "The quick brown fox jumps over the lazy dog.",
                font,
                Brushes.Black,
                drawTextBounds,
                new TextFormat { Wrapping = TextWrapping.Word });

            dc.FillEllipse(Brushes.Gold, cornerRectLeft.InflatedBy(-5, -5));
            dc.FillEllipse(Brushes.Gold, cornerRectRight.InflatedBy(-5, -5));

            dc.DrawEllipse(Pens.Goldenrod, cornerRectLeft.InflatedBy(-5, -5));
            dc.DrawEllipse(Pens.Goldenrod, cornerRectRight.InflatedBy(-5, -5));

            dc.DrawLine(thickGrayPen, cornerRectLeft.Center, cornerRectRight.Center);
        }

        private void PrintImmediatelyMenuItem_Click(object sender, System.EventArgs e)
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

        private void PrintMenuItem_Click(object sender, System.EventArgs e)
        {
            var dialog = new PrintDialog();
            var document = CreatePrintDocument();

            dialog.Document = document;
            dialog.AllowSelection = true;
            dialog.AllowSomePages = false;

            document.PrintPage += Document_PrintPage;

            if (dialog.ShowModal() == ModalResult.Accepted)
                document.Print();
        }

        private void PageSetupMenuItem_Click(object sender, System.EventArgs e)
        {
            var pageSetupDialog = new PageSetupDialog();
            var document = CreatePrintDocument();

            pageSetupDialog.Document = document;
            //setupDlg.AllowMargins = false;
            //setupDlg.AllowOrientation = false;
            //setupDlg.AllowPaper = false;
            //setupDlg.AllowPrinter = false;

            if (pageSetupDialog.ShowModal() == ModalResult.Accepted)
            {
                //document.DefaultPageSettings = pageSetupDialog.PageSettings;
                //document.PrinterSettings = pageSetupDialog.PrinterSettings;

                document.PrintPage += Document_PrintPage;
                document.Print();
            }
        }

        private void PrintPreviewMenuItem_Click(object sender, System.EventArgs e)
        {
            var dialog = new PrintPreviewDialog();
            var document = CreatePrintDocument();

            document.PrintPage += Document_PrintPage;
            
            dialog.Document = document;
            dialog.ShowModal();
        }

        private void Document_PrintPage(object? sender, PrintPageEventArgs e)
        {
            /*var pb = e.PageBounds;
            var ppb = e.PrintablePageBounds;
            var phpb = e.PhysicalPageBounds;
            var mb = e.MarginBounds;*/

            int pageNumber = e.PageNumber;

            var bounds = new RectD(new PointD(), originAtMarginCheckBox.IsChecked
                ? e.MarginBounds.Size : e.PrintablePageBounds.Size);

            if (pageNumber == 1)
            {
                DrawFirstPage(
                    e.DrawingContext,
                    bounds);
            }
            else
            {
                DrawAdditionalPage(e.DrawingContext, pageNumber, bounds);
            }

            var v = additionalPagesCountNumericUpDown.Value;

            e.HasMorePages = pageNumber - 1 < v;
        }

        private void DrawAdditionalPage(Graphics dc, int pageNumber, RectD bounds)
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