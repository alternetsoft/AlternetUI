using System;
using Alternet.Drawing;
using Alternet.UI;
using ApiCommon;

namespace NativeApi.Api
{
    public class PrintDocument
    {
        public string DocumentName { get => throw new Exception(); set => throw new Exception(); }
        public PrinterSettings PrinterSettings { get => throw new Exception(); set => throw new Exception(); }
        public PageSettings DefaultPageSettings { get => throw new Exception(); set => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? PrintPage { add => throw new Exception(); remove => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? BeginPrint { add => throw new Exception(); remove => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? EndPrint { add => throw new Exception(); remove => throw new Exception(); }

        public void Print() => throw new Exception();

        public DrawingContext PrintDrawingContext { get => throw new Exception(); }

        public bool PrintHasMorePages { get => throw new Exception(); }

        public PageSettings PrintPageSettings { get => throw new Exception(); }

        public Thickness PrintPhysicalMarginBounds { get => throw new Exception(); }

        public Rect MarginBounds { get => throw new Exception(); }

        public Rect PhysicalPageBounds { get => throw new Exception(); }

        public Rect PageBounds { get => throw new Exception(); }
    }
}