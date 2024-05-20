using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IFontDialogHandler : IDialogHandler
    {
        FontDialogRestrictSelection RestrictSelection { get; set; }

        bool EnableEffects { get; set; }

        Color Color { get; set; }

        FontInfo FontInfo { get; set; }

        bool AllowSymbols { get; set; }

        void SetRange(int minRange, int maxRange);
    }
}
