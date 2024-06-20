namespace Alternet.UI
{
    /// <summary>
    /// Specifies the list view column width behavior of a list view.
    /// </summary>
    public enum ListViewColumnWidthMode
    {
        /// <summary>
        /// Fixed column width, using the Width property value.
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