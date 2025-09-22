#pragma warning disable
using System;
using Alternet.Drawing;
using Alternet.UI;

using ApiCommon;

namespace NativeApi.Api
{
    [ManagedInterface("Alternet.Drawing.Printing.IPageSettingsHandler")]
    public class PageSettings
    {
        public bool Color { get; set; }
        public bool Landscape { get; set; }
        public float MarginLeft { get; set; }

        public float MarginRight { get; set; }

        public float MarginTop { get; set; }

        public float MarginBottom { get; set; }

        public SizeD CustomPaperSize { get; set; }
        public bool UseCustomPaperSize { get => throw new Exception(); set => throw new Exception(); }
        public PaperKind PaperSize { get => throw new Exception(); set => throw new Exception(); }
        public PrinterResolutionKind PrinterResolution { get => throw new Exception(); set => throw new Exception(); }
    }
}