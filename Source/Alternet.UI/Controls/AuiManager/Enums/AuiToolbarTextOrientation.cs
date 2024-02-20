using System;

namespace Alternet.UI
{
    /// <summary>
    /// Defines possible text orientations for <see cref="AuiToolbar"/> items.
    /// </summary>
    internal enum AuiToolbarTextOrientation
    {
        /// <summary>
        /// Text in toolbar items is left aligned, currently unused/unimplemented.
        /// </summary>
        Left = 0,

        /// <summary>
        /// Text in toolbar items is right aligned.
        /// </summary>
        Right = 1,

        /// <summary>
        /// Text in toolbar items is top aligned, currently unused/unimplemented.
        /// </summary>
        Top = 2,

        /// <summary>
        /// Text in toolbar items is bottom aligned.
        /// </summary>
        Bottom = 3,
    }
}