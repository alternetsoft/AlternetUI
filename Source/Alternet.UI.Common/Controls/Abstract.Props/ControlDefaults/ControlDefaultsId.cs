using System;

namespace Alternet.UI
{
    /// <summary>
    /// Defines ids for platform related default properties.
    /// </summary>
    public enum ControlDefaultsId
    {
        /// <summary>
        /// Margin property minimal value.
        /// </summary>
        MinMargin,

        /// <summary>
        /// Specifies whether control has border when color scheme is white.
        /// </summary>
        HasBorderOnWhite,

        /// <summary>
        /// Specifies whether control has border when color scheme is black.
        /// </summary>
        HasBorderOnBlack,

        /// <summary>
        /// Padding property minimal value.
        /// </summary>
        MinPadding,

        /// <summary>
        /// Enumeration element with maximal index.
        /// </summary>
        MaxValue = MinPadding,
    }
}