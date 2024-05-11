using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    public interface IPageSettingsHandler : IDisposable
    {
        public bool Color { get; set; }

        public bool Landscape { get; set; }

        public Thickness Margins { get; set; }

        public SizeD CustomPaperSize { get; set; }

        public bool UseCustomPaperSize { get; set; }

        public PaperKind PaperSize { get; set; }

        public PrinterResolutionKind PrinterResolution { get; set; }
    }
}
