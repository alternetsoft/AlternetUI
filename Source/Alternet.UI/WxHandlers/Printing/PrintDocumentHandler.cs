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
        private SizeD pageScaleFactor = SizeD.One;

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

        public bool StartPage()
        {
            if (canvas != null)
                return false;

            dc = PrintPage_DrawingContext;
            int pageWidthPx = dc.GetSize().Width;
            int pageHeightPx = dc.GetSize().Height;
            int printerPpiX = dc.GetPPI().Width;
            int printerPpiY = dc.GetPPI().Height;

            var info = new SKImageInfo(pageWidthPx, pageHeightPx, SKColorType.Bgra8888, SKAlphaType.Premul);
            var bitmap = new SKBitmap(info);

            graphics = new SkiaGraphics(bitmap);
            canvas = graphics.Canvas;

            float scaleX = (float)printerPpiX / 96f;
            float scaleY = (float)printerPpiY / 96f;
            pageScaleFactor = new SizeD(scaleX, scaleY);
            canvas.Scale(scaleX, scaleY);

            canvas.Clear(SKColors.White);

            return true;
        }

        public Graphics? GetPageGraphics()
        {
            return graphics;
        }

        public bool EndPage()
        {
            if (canvas == null)
                return false;

            var result = false;

            if (graphics?.Bitmap is not null && dc is not null)
            {
                var imageInfo = new SKImageInfo(
                    (int)(graphics.Bitmap.Width / pageScaleFactor.Width),
                    (int)(graphics.Bitmap.Height / pageScaleFactor.Height));

                var sampling = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None);

                var resized = graphics.Bitmap.Resize(imageInfo, sampling);

                var image = (Image)resized;

                /* image.Save("E:\\test.png");*/

                /* SkiaUtils.SaveBitmapToPng(graphics.Bitmap, "E:\\test_skia.png");*/
                
                dc.DrawBitmapAtPointI((UI.Native.Image)image.Handler, 0, 0, false);

                graphics.Dispose();
                graphics = null;
                canvas.Dispose();
                canvas = null;
                dc = null;
                result = true;
            }
            else
            {
            }

            return result;
        }
    }
}
