using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Contains methods and properties with allow with page settings.
    /// </summary>
    public interface IPageSettingsHandler : IDisposable
    {
        /// <inheritdoc cref="PageSettings.Color"/>
        public bool Color { get; set; }

        /// <inheritdoc cref="PageSettings.Landscape"/>
        public bool Landscape { get; set; }

        /// <summary>
        /// Gets or sets left margin.
        /// </summary>
        public Coord MarginLeft { get; set; }

        /// <summary>
        /// Gets or sets top margin.
        /// </summary>
        public Coord MarginTop { get; set; }

        /// <summary>
        /// Gets or sets right margin.
        /// </summary>
        public Coord MarginRight { get; set; }

        /// <summary>
        /// Gets or sets bottom margin.
        /// </summary>
        public Coord MarginBottom { get; set; }

        /// <summary>
        /// Gets or sets custom paper size.
        /// </summary>
        public SizeD CustomPaperSize { get; set; }

        /// <summary>
        /// Gets or sets whether to use custom paper size.
        /// </summary>
        public bool UseCustomPaperSize { get; set; }

        /// <inheritdoc cref="PageSettings.PaperSize"/>
        public PaperKind PaperSize { get; set; }

        /// <inheritdoc cref="PageSettings.PrinterResolution"/>
        public PrinterResolutionKind PrinterResolution { get; set; }
    }
}
