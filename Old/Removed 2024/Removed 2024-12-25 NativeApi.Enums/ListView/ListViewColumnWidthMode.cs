using ApiCommon;

namespace NativeApi.Api
{
    /// <summary>
    /// Specifies the list view column width behavior of a list view.
    /// </summary>
    /// <remarks>This enumeration is used by the <see cref="ListView"/> class.</remarks>
    [ManagedExternName("Alternet.UI.ListViewColumnWidthMode")]
    [ManagedName("Alternet.UI.ListViewColumnWidthMode")]
    public enum ListViewColumnWidthMode
    {
        /// <summary>
        /// Fixed column width, using the <see cref="ListViewColumn.Width"/> property value.
        /// </summary>
        Fixed,

        /// <summary>
        /// Adjust the width of the longest item in the column.
        /// </summary>
        AutoSize,

        /// <summary>
        /// Autosize to the width of the column heading
        /// </summary>
        AutoSizeHeader,
    }
}