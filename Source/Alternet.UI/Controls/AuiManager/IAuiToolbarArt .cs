using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the color and other settings of the art provider for the
    /// <see cref="AuiToolbar"/>.
    /// </summary>
    public interface IAuiToolbarArt : IDisposableObject
    {
        void SetFlags(uint flags);

        uint GetFlags();

        void SetTextOrientation(AuiToolbarTextOrientation orientation);

        AuiToolbarTextOrientation GetTextOrientation();

        // Note that these functions work with the size in DIPs, not physical pixels.
        int GetElementSize(int elementId);

        void SetElementSize(int elementId, int size);

        // Provide opportunity for subclasses to recalculate colors
        void UpdateColorsFromSystem();
    }
}
