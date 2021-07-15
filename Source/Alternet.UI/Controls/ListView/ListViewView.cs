namespace Alternet.UI
{
    /// <summary>
    /// Specifies how list items are displayed in a <see cref="ListView"/> control.
    /// </summary>
    /// <remarks>Use the members of this enumeration to set the value of the <see cref="ListView.View"/> property of the <see cref="ListView"/> control.</remarks>
    public enum ListViewView
    {
        /// <summary>
        /// Each item appears as a text label with an optional icon. Items are arranged
        /// in columns with no column headers. Columns are computed automatically.
        /// </summary>
        List,

        /// <summary>
        /// Each item appears on a separate line with further information about each item arranged in columns.
        /// The left-most column can contain an optional small icon.
        /// A column can display a header with a caption for the column. The user can resize each column.
        /// </summary>
        Details,

        /// <summary>
        /// Each item appears as a small icon with a text label.
        /// </summary>
        SmallIcon,

        /// <summary>
        /// Each item appears as a full-sized icon with a text label.
        /// </summary>
        LargeIcon
    }
}