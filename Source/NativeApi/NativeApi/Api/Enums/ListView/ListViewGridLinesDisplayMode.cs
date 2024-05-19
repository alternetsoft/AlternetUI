using ApiCommon;

namespace NativeApi.Api
{
    /// <summary>
    /// Specifies the grid line display mode for a list view.
    /// </summary>
    /// <remarks>This enumeration is used by the <see cref="ListView"/> class.</remarks>
    [ManagedExternName("Alternet.UI.ListViewGridLinesDisplayMode")]
    [ManagedName("Alternet.UI.ListViewGridLinesDisplayMode")]
    public enum ListViewGridLinesDisplayMode
    {
        /// <summary>
        /// No grid lines visible.
        /// </summary>
        None,

        /// <summary>
        /// Vertical grid lines visible.
        /// </summary>
        Vertical,

        /// <summary>
        /// Horizontal grid lines visible.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Vertical and horizontal grid lines visible.
        /// </summary>
        VerticalAndHorizontal,
    }
}