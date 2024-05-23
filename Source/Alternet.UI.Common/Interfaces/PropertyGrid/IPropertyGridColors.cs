using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <inheritdoc cref="PropertyGridColors"/>
    public interface IPropertyGridColors
    {
        /// <inheritdoc cref="PropertyGridColors.CaptionBackgroundColor"/>
        Color? CaptionBackgroundColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.CaptionForegroundColor"/>
        Color? CaptionForegroundColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.CellBackgroundColor"/>
        Color? CellBackgroundColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.CellDisabledTextColor"/>
        Color? CellDisabledTextColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.CellTextColor"/>
        Color? CellTextColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.EmptySpaceColor"/>
        Color? EmptySpaceColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.LineColor"/>
        Color? LineColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.MarginColor"/>
        Color? MarginColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.SelectionBackgroundColor"/>
        Color? SelectionBackgroundColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.SelectionForegroundColor"/>
        Color? SelectionForegroundColor { get; set; }

        /// <inheritdoc cref="PropertyGridColors.ResetColors"/>
        bool ResetColors { get; set; }
    }
}
