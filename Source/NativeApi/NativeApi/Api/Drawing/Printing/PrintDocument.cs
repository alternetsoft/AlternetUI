#pragma warning disable
using System;
using Alternet.Drawing;
using Alternet.UI;
using ApiCommon;

namespace NativeApi.Api
{
    public class PrintDocument
    {
        public bool OriginAtMargins { get => throw new Exception(); set => throw new Exception(); }
        public string DocumentName { get => throw new Exception(); set => throw new Exception(); }
        public PrinterSettings PrinterSettings { get => throw new Exception(); }
        public PageSettings PageSettings { get => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? PrintPage { add => throw new Exception(); remove => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? BeginPrint { add => throw new Exception(); remove => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? EndPrint { add => throw new Exception(); remove => throw new Exception(); }

        public void Print() => throw new Exception();

        public DrawingContext PrintPage_DrawingContext { get => throw new Exception(); }

        public bool PrintPage_HasMorePages { get => throw new Exception(); set => throw new Exception(); }

        public RectD GetPrintPage_MarginBounds() => throw new Exception();

        public RectD GetPrintPage_PhysicalPageBounds() => throw new Exception();

        public RectD GetPrintPage_PageBounds() => throw new Exception();

        public RectD GetPrintPage_PrintablePageBounds() => throw new Exception();

        public int PrintPage_PageNumber { get => throw new Exception(); }
    }
}