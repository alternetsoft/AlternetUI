namespace Alternet.UI
{
    /// <summary>
    /// Specifies the list view column width behavior of a list view.
    /// </summary>
    public enum ListViewColumnWidthMode
    {
        /// <summary>
        /// Fixed column width, using the Width property value of the column.
        /// </summary>
        Fixed,

        /// <summary>
        /// Adjust the width to the longest item in the column.
        /// </summary>
        AutoSize,

        /// <summary>
        /// Autosize column to the width of the column header text.
        /// </summary>
        AutoSizeHeader,

        /// <summary>
        /// Fixed column width in percents of the owner control's width, using
        /// the Width property value of the column.
        /// </summary>
        FixedInPercent,
    }
}