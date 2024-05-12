using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class ListViewColumn : BaseControlItem
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
        /// Initializes a new instance of the <see cref="ListViewColumn"/> class with the specified
        /// column title.
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
                if (title == value)
                    return;
                title = value;
                ApplyTitle();
            }
        }

        /// <summary>
        /// Gets the location with the <see cref="ListView"/> control's
        /// <see cref="ListView.Columns"/> of this column.
        /// </summary>
        /// <value>The zero-based index of the column header within the
        /// <see cref="ListView.Columns"/> of the <see cref="ListView"/> control it is contained
        /// in.</value>
        /// <remarks>If the <see cref="ListViewColumn"/> is not contained within a
        /// <see cref="ListView"/> control this property returns a value of <c>null</c>.</remarks>
        [Browsable(false)]
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
        [Browsable(false)]
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
                if (width == value)
                    return;
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
                if (widthMode == value)
                    return;
                widthMode = value;
                ApplyWidth();
            }
        }

        /// <summary>
        /// Creates copy of this <see cref="ListViewColumn"/>.
        /// </summary>
        public ListViewColumn Clone()
        {
            var result = new ListViewColumn();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Assigns properties from another <see cref="ListViewColumn"/>.
        /// </summary>
        /// <param name="item">Source of the properties to assign.</param>
        public void Assign(ListViewColumn item)
        {
            Tag = item.Tag;

            bool applyWidth = (widthMode != item.WidthMode) || (width != item.Width);
            bool applyTitle = title != item.Title;

            if (applyTitle)
            {
                title = item.Title;
                ApplyTitle();
            }

            if (applyWidth)
            {
                widthMode = item.WidthMode;
                width = item.Width;
                ApplyWidth();
            }
        }

        /// <inheritdoc cref="ListControlItem.ToString"/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Title))
                return base.ToString() ?? nameof(ListViewColumn);
            else
                return Title;
        }

        public void InternalSetListViewAndIndex(ListView? control, int? newIndex)
        {
            var changed = listView != control || index != newIndex;
            if (changed)
            {
                listView = control;
                index = newIndex;
                ApplyAll();
            }
        }

        internal void ApplyWidth()
        {
            if (TryGetColumnIndex(out var listView, out var columnIndex))
                listView.Handler.SetColumnWidth(columnIndex.Value, Width, WidthMode);
        }

        internal void ApplyTitle()
        {
            if (TryGetColumnIndex(out var listView, out var columnIndex))
                listView.Handler.SetColumnTitle(columnIndex.Value, Title);
        }

        internal void ApplyAll()
        {
            ApplyTitle();
            ApplyWidth();
        }

        private bool TryGetColumnIndex(
            [NotNullWhen(true)] out ListView? listView,
            [NotNullWhen(true)] out int? columnIndex)
        {
            listView = ListView;
            columnIndex = Index;

            return listView != null && columnIndex != null;
        }
    }
}