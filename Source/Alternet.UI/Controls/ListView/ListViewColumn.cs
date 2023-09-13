using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
        private double width = 80;
        private ListViewColumnWidthMode widthMode;
        private ListView? listView;
        private string title = string.Empty;
        private int? index;

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
        public string Title
        {
            get => title;
            set
            {
                title = value;
                ApplyTitle();
            }
        }

        /// <summary>
        /// Gets the location with the <see cref="ListView"/> control's
        /// <see cref="ListView.Columns"/> of this column.
        /// </summary>
        /// <value>The zero-based index of the column header within the
        /// <see cref="ListView.Columns"/> of the <see cref="ListView"/> control it is contained in.</value>
        /// <remarks>If the <see cref="ListViewColumn"/> is not contained within a
        /// <see cref="ListView"/> control this property returns a value of <c>null</c>.</remarks>
        public int? Index
        {
            get => index;

            internal set
            {
                index = value;
                ApplyAll();
            }
        }

        /// <summary>
        /// Gets the <see cref="ListView"/> control the <see cref="ListViewColumn"/> is located in.
        /// </summary>
        /// <value>A <see cref="ListView"/> control that represents the control that contains
        /// the <see cref="ListViewColumn"/>.</value>
        /// <remarks>
        /// You can use this property to determine which <see cref="ListView"/> control a
        /// specific <see cref="ListViewColumn"/> object is associated with.
        /// </remarks>
        public ListView? ListView
        {
            get => listView;

            internal set
            {
                listView = value;
                ApplyAll();
            }
        }

        /// <summary>
        /// Gets or sets the fixed width of the column, in device-independent units (1/96th inch
        /// per unit).
        /// </summary>
        /// <value>
        /// The fixed width of the column, in device-independent units (1/96th inch per unit).
        /// Default value is 80.
        /// </value>
        public double Width
        {
            get => width;
            set
            {
                width = value;
                ApplyWidth();
            }
        }

        /// <summary>
        /// Gets or set the width sizing behavior for this column.
        /// </summary>
        /// <value>A <see cref="ListViewColumnWidthMode"/> which specifies the width sizing behavior.
        /// Default is <see cref="ListViewColumnWidthMode.Fixed"/>.</value>
        public ListViewColumnWidthMode WidthMode
        {
            get => widthMode;
            set
            {
                widthMode = value;
                ApplyWidth();
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Title))
                return base.ToString() ?? nameof(ListViewColumn);
            else
                return Title;
        }

        private bool TryGetColumnIndex(
            [NotNullWhen(true)] out ListView? listView,
            [NotNullWhen(true)] out int? columnIndex)
        {
            listView = ListView;
            columnIndex = Index;

            return listView != null && columnIndex != null;
        }

        private void ApplyWidth()
        {
            if (TryGetColumnIndex(out var listView, out var columnIndex))
                listView.Handler.SetColumnWidth(columnIndex.Value, Width, WidthMode);
        }

        private void ApplyTitle()
        {
            if (TryGetColumnIndex(out var listView, out var columnIndex))
                listView.Handler.SetColumnTitle(columnIndex.Value, Title);
        }

        private void ApplyAll()
        {
            ApplyWidth();
            ApplyTitle();
        }
    }
}