using System;
using Alternet.Drawing;
using Alternet.UI;

namespace NativeApi.Api
{
    public class PageSettings
    {
        public Rect Bounds { get; }
        public bool Color { get; set; }
        public bool Landscape { get; set; }
        public Thickness Margins { get; set; }
        public PrinterSettings PrinterSettings { get => throw new Exception(); set => throw new Exception(); }
        public PaperKind PaperSize { get => throw new Exception(); set => throw new Exception(); }
        public PrinterResolutionKind PrinterResolution { get => throw new Exception(); set => throw new Exception(); }
    }
}