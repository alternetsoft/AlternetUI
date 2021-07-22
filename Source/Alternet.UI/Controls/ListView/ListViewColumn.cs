using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a column in a <see cref="ListView"/> control to be used in
    /// the <see cref="ListViewView.Details"/> <see cref="ListView.View"/>.
    /// </summary>
    /// <remarks>
    /// A column is an item in a <see cref="ListView"/> control that contains column title text in the
    /// <see cref="ListViewView.Details"/> <see cref="ListView.View"/>.
    /// <see cref="ListViewColumn"/> objects can be added to a <see cref="ListView"/> using the
    /// <see cref="ICollection{ListViewColumn}.Add"/> method of the collection returned
    /// by <see cref="ListView.Columns"/> property.
    /// </remarks>
    public class ListViewColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewColumn"/> class with default values.
        /// </summary>
        public ListViewColumn()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewColumn"/> class with the specified column title.
        /// </summary>
        /// <param name="title">The text displayed in the column header.</param>
        public ListViewColumn(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Gets or sets the title text displayed in the column header.
        /// </summary>
        /// <value>The text displayed in the column header.</value>
        public string Title { get; set; } = "";

        /// <summary>
        /// Gets the location with the <see cref="ListView"/> control's <see cref="ListView.Columns"/> of this column.
        /// </summary>
        /// <value>The zero-based index of the column header within the <see cref="ListView.Columns"/> of the <see cref="ListView"/> control it is contained in.</value>
        /// <remarks>If the <see cref="ListViewColumn"/> is not contained within a <see cref="ListView"/> control this property returns a value of <c>null</c>.</remarks>
        public int? Index { get; internal set; }

        /// <summary>
        /// Gets the <see cref="ListView"/> control the <see cref="ListViewColumn"/> is located in.
        /// </summary>
        /// <value>A <see cref="ListView"/> control that represents the control that contains the <see cref="ListViewColumn"/>.</value>
        /// <remarks>
        /// You can use this property to determine which <see cref="ListView"/> control a specific <see cref="ListViewColumn"/> object is associated with.
        /// </remarks>
        public ListView? ListView { get; internal set; }
    }
}