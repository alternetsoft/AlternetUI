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
    internal interface IAuiToolbarArt : IDisposableObject
    {
        /// <summary>
        /// Gets or sets text orientation.
        /// </summary>
        AuiToolbarTextOrientation TextOrientation { get; set; }

        /// <summary>
        /// Gets toolbar element size.
        /// </summary>
        /// <param name="elementId">Element id.</param>
        /// <remarks>
        /// Note that this function work with the size in DIPs, not physical pixels.
        /// </remarks>
        int GetElementSize(AuiToolBarArtSetting elementId);

        /// <summary>
        /// Gets toolbar element size.
        /// </summary>
        /// <param name="elementId">Element id.</param>
        /// <remarks>
        /// Note that this function work with the size in DIPs, not physical pixels.
        /// </remarks>
        /// <param name="size">New element size.</param>
        void SetElementSize(AuiToolBarArtSetting elementId, int size);

        /// <summary>
        /// Provides opportunity for subclasses to recalculate colors
        /// </summary>
        internal void UpdateColorsFromSystem();

        internal void SetFlags(uint flags);

        internal uint GetFlags();
    }
}
