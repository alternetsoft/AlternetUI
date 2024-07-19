using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with the font dialog window.
    /// </summary>
    public interface IFontDialogHandler : IDialogHandler
    {
        /// <inheritdoc cref="FontDialog.RestrictSelection"/>
        FontDialogRestrictSelection RestrictSelection { get; set; }

        /// <inheritdoc cref="FontDialog.EnableEffects"/>
        bool EnableEffects { get; set; }

        /// <inheritdoc cref="FontDialog.Color"/>
        Color Color { get; set; }

        /// <inheritdoc cref="FontDialog.FontInfo"/>
        FontInfo FontInfo { get; set; }

        /// <inheritdoc cref="FontDialog.AllowSymbols"/>
        bool AllowSymbols { get; set; }

        /// <inheritdoc cref="FontDialog.SetRange"/>
        void SetRange(int minRange, int maxRange);
    }
}
