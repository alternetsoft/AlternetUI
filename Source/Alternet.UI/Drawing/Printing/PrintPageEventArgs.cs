using Alternet.UI.Internal.ComponentModel;
using Alternet.UI.Native;
using System;
using System.ComponentModel;

namespace Alternet.Drawing.Printing
{
    public class PrintPageEventArgs : CancelEventArgs
    {
        public DrawingContext DrawingContext { get; set; }

        internal UI.Native.PrintDocument NativePrintDocument { get; private set; }
        public bool HasMorePages { get; set; }
    }
}