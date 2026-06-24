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
        private SkiaGraphics? canvas;
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
            var dpi = dc.GetDpi();
            var scaleFactor = GraphicsFactory.ScaleFactorFromDpi(dpi.Width);

            canvas = SkiaUtils.CreateBitmapCanvas(rect.Size, scaleFactor, true);
            canvas.Canvas.Clear(SKColors.Transparent);
            return true;
        }

        public Graphics? GetGraphics()
        {
            return canvas;
        }

        public bool EndPrinting()
        {
            if (canvas == null)
                return false;

            var result = false;

            if (canvas.Bitmap is not null && dc is not null)
            {
                var image = (Image)canvas.Bitmap;
                dc.DrawImageAtPoint((UI.Native.Image)image.Handler, rect.Location, false);

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
