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

        Graphics IPrintDocumentHandler.DrawingContext
        {
            get
            {
                var bounds1 = ((IPrintDocumentHandler)this).PageBounds;
                var bounds2 = ((IPrintDocumentHandler)this).PrintablePageBounds;
                var clientRect = bounds2;
                var dc = PrintPage_DrawingContext;
                var hdc = dc.GetHandle();
                var dpi = dc.GetDpi();
                var scaleFactor = GraphicsFactory.ScaleFactorFromDpi(dpi.Width);
                var clientRectI = clientRect.PixelFromDip(scaleFactor);

                var canvas = SkiaUtils.CreateBitmapCanvas(clientRect.Size, scaleFactor, true);
                canvas.Canvas.Clear(SKColors.Transparent);

                canvas.Disposed += (s, e) =>
                {
                    if (canvas.Bitmap is null)
                        return;

                    var image = (Image)canvas.Bitmap;
                    dc.DrawImageAtPoint(
                        (UI.Native.Image)image.Handler,
                        clientRectI.Location,
                        false);
                };

                return canvas;
            }
        }

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
    }
}
