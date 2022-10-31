using System;
using Alternet.Drawing;
using Alternet.UI;

namespace NativeApi.Api
{
    public class PageSettings
    {
        public bool Color { get; set; }
        public bool Landscape { get; set; }
        public Thickness Margins { get; set; }
        public Size CustomPaperSize { get => throw new Exception(); set => throw new Exception(); }
        public bool UseCustomPaperSize { get => throw new Exception(); set => throw new Exception(); }
        public PaperKind PaperSize { get => throw new Exception(); set => throw new Exception(); }
        public PrinterResolutionKind PrinterResolution { get => throw new Exception(); set => throw new Exception(); }
    }
}