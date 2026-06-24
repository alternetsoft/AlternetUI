using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing.Printing
{
    internal class PrintDocumentHandler : UI.Native.PrintDocument, IPrintDocumentHandler
    {
        private SkiaGraphics? graphics;
        private SKCanvas? canvas;
        private UI.Native.DrawingContext? dc;
        private RectD rect;

        RectD IPrintDocumentHandler.MarginBounds => GetPrintPage_MarginBounds();

        RectD IPrintDocumentHandler.PhysicalPageBounds => GetPrintPage_PhysicalPageBounds();

        RectD IPrintDocumentHandler.PageBounds => GetPrintPage_PageBounds();

        RectD IPrintDocumentHandler.PrintablePageBounds => GetPrintPage_PrintablePageBounds();

        int IPrintDocumentHandler.PrintedPageNumber => PrintPage_PageNumber;

        bool IPrintDocumentHandler.HasMorePages
        {
            get => PrintPage_HasMorePages;
            set => PrintPage_HasMorePages = value;
        }

        IPrinterSettingsHandler IPrintDocumentHandler.PrinterSettings => PrinterSettings;

        IPageSettingsHandler IPrintDocumentHandler.PageSettings => PageSettings;

        void IPrintDocumentHandler.SetDocumentName(string name)
        {
            NativeStringSpan.Invoke(name, span =>
            {
                SetDocumentName(span);
            });
        }

        string IPrintDocumentHandler.GetDocumentName()
        {
            return GetDocumentName().ToString();
        }

        public bool StartPrinting(RectD rect)
        {
            if (canvas != null)
                return false;

            this.rect = rect;
            dc = PrintPage_DrawingContext;

            // Suppose you have printer page size and DPI
            int pageWidthPx = dc.GetSize().Width;   // printer pixels
            int pageHeightPx = dc.GetSize().Height;
            int printerDpiX = dc.GetPPI().Width;    // e.g. 600
            int printerDpiY = dc.GetPPI().Height;

            // Choose target DPI for bitmap
            int targetDpi = 300;

            // Compute bitmap size
            int bmpW = pageWidthPx * targetDpi / printerDpiX;
            int bmpH = pageHeightPx * targetDpi / printerDpiY;

            var info = new SKImageInfo(bmpW, bmpH, SKColorType.Bgra8888, SKAlphaType.Premul);
            var bitmap = new SKBitmap(info);

            graphics = new SkiaGraphics(bitmap);
            canvas = graphics.Canvas;

            canvas.Clear(SKColors.White);

            // Apply scale so drawing in printer units maps to bitmap
            float scaleX = (float)bmpW / pageWidthPx;
            float scaleY = (float)bmpH / pageHeightPx;
            canvas.Scale(scaleX, scaleY);

            return true;
        }

        public Graphics? GetGraphics()
        {
            return graphics;
        }

        public bool EndPrinting()
        {
            if (canvas == null)
                return false;

            var result = false;

            if (graphics?.Bitmap is not null && dc is not null)
            {
                var image = (Image)graphics.Bitmap;

                var ppi = dc.GetPPI();        // printer DPI, e.g. 600x600
                var pageSize = dc.GetSize();  // full page in printer pixels

                int targetDpi = 300;
                int bmpW = pageSize.Width * targetDpi / ppi.Width;
                int bmpH = pageSize.Height * targetDpi / ppi.Height;

                var r = new RectD(0, 0, bmpW, bmpH);

                dc.DrawImageAtRect((UI.Native.Image)image.Handler, r, false);

                graphics.Dispose();
                graphics = null;
                canvas.Dispose();
                canvas = null;
                dc = null;
                rect = default;
                result = true;
            }
            else
            {
            }

            return result;
        }
    }
}
