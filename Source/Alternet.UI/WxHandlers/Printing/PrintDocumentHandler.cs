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
            int pageWidthPx = dc.GetSize().Width;   // printer pixels
            int pageHeightPx = dc.GetSize().Height;
            int printerPpiX = dc.GetPPI().Width;    // e.g. 600
            int printerPpiY = dc.GetPPI().Height;

            var widthInches = (float)pageWidthPx / printerPpiX;
            var heightInches = (float)pageHeightPx / printerPpiY;

            var width96 = widthInches * 96;
            var height96 = heightInches * 96;        

            var info = new SKImageInfo((int)width96, (int)height96, SKColorType.Bgra8888, SKAlphaType.Premul);
            var bitmap = new SKBitmap(info);

            graphics = new SkiaGraphics(bitmap);
            canvas = graphics.Canvas;
            canvas.Clear(SKColors.White);

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

                image.Save("E:\\test.png");

                SkiaUtils.SaveBitmapToPng(graphics.Bitmap, "E:\\test_skia.png");

                dc.DrawBitmapAtPointI((UI.Native.Image)image.Handler, 0, 0, false);

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
