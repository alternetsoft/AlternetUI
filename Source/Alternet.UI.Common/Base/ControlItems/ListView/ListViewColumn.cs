using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a column in a <see cref="ListView"/> control to be used in
    /// the <see cref="ListViewView.Details"/> <see cref="ListView.View"/>.
    /// </summary>
    /// <remarks>
    /// A column is an item in a <see cref="ListView"/> control that contains column
    /// title text in the
    /// <see cref="ListViewView.Details"/> <see cref="ListView.View"/>.
    /// <see cref="ListViewColumn"/> objects can be added to a <see cref="ListView"/> using the
    /// <see cref="ICollection{ListViewColumn}.Add"/> method of the collection returned
    /// by <see cref="ListView.Columns"/> property.
    /// </remarks>
    public class ListViewColumn : BaseControlItem
    {
        private Coord width = 80;
        private ListViewColumnWidthMode widthMode;
        private ListView? listView;
        private string title = string.Empty;
        private int? index;
        private Coord? minAutoWidth = -10;
        private Coord? maxAutoWidth;

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
        /// Enumerates known column events.
        /// </summary>
        public enum ColumnEventType
        {
            /// <summary>
            /// No event.
            /// </summary>
            None,

            /// <summary>
            /// Column title is changed.
            /// </summary>
            TitleChanged,

            /// <summary>
            /// Column width is changed.
            /// </summary>
            WidthChanged,

            /// <summary>
            /// Column changed, which included width and title changed.
            /// </summary>
            AllChanged,
        }

        /// <summary>
        /// Gets or sets the title text displayed in the column header.
        /// </summary>
        /// <value>The text displayed in the column header.</value>
        public virtual string Title
        {
            get => title;
            set
            {
                if (title == value)
                    return;
                title = value;
                RaiseChanged(ColumnEventType.TitleChanged);
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
                RaiseChanged(ColumnEventType.AllChanged);
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
                RaiseChanged(ColumnEventType.AllChanged);
            }
        }

        /// <summary>
        /// Gets or sets the fixed width of the column, in device-independent units.
        /// </summary>
        /// <value>
        /// The fixed width of the column, in device-independent units.
        /// Default value is 80.
        /// </value>
        public virtual Coord Width
        {
            get => width;
            set
            {
                if (width == value)
                    return;
                width = value;
                RaiseChanged(ColumnEventType.WidthChanged);
            }
        }

        /// <summary>
        /// Gets or sets the minimal auto-width of the column, in device-independent units.
        /// This property is used when column width is specified in percent of the container's width.
        /// If it is negative, minimal width is calculated from title's width plus the absolute
        /// of the specified value.
        /// </summary>
        public virtual Coord? MinAutoWidth
        {
            get => minAutoWidth;
            set
            {
                if (minAutoWidth == value)
                    return;
                minAutoWidth = value;
                if(WidthMode == ListViewColumnWidthMode.FixedInPercent)
                    RaiseChanged(ColumnEventType.WidthChanged);
            }
        }

        /// <summary>
        /// Gets or sets the maximal auto-width of the column, in device-independent units.
        /// This property is used when column width is specified in percent of the
        /// container's width.
        /// If it equals 0, it is not used.
        /// </summary>
        public virtual Coord? MaxAutoWidth
        {
            get => maxAutoWidth;

            set
            {
                if (maxAutoWidth == value)
                    return;
                maxAutoWidth = value;
                if (WidthMode == ListViewColumnWidthMode.FixedInPercent)
                    RaiseChanged(ColumnEventType.WidthChanged);
            }
        }

        /// <summary>
        /// Gets size of the title's text string in device-independent
        /// units if column is attached to the container.
        /// </summary>
        public virtual SizeD? TitleSize
        {
            get
            {
                if (ListView is null)
                    return null;
                var result = ListView.MeasureCanvas.MeasureText(Title, ListView.RealFont);
                return result;
            }
        }

        /// <summary>
        /// Gets whether column is attached to the container and it's column index is specified.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsAttached
        {
            get
            {
                return listView != null && index != null;
            }
        }

        /// <summary>
        /// Gets or set the width sizing behavior for this column.
        /// </summary>
        /// <value>A <see cref="ListViewColumnWidthMode"/> which specifies
        /// the width sizing behavior.
        /// Default is <see cref="ListViewColumnWidthMode.Fixed"/>.</value>
        public virtual ListViewColumnWidthMode WidthMode
        {
            get => widthMode;
            set
            {
                if (widthMode == value)
                    return;
                widthMode = value;
                RaiseChanged(ColumnEventType.WidthChanged);
            }
        }

        /// <summary>
        /// Converts column width in percent (0..100) of the owner control's width to
        /// real width in device-independent units.
        /// If <see cref="ListView"/> is not assigned returns null.
        /// </summary>
        /// <param name="widthInPercent">The column width in percent of the
        /// owner control's width.</param>
        /// <param name="minWidth">Minimal possible width of the column. If negative value
        /// is specified, minimal possible width is calculated from the width of the
        /// column title's text plus absolute value of the <paramref name="minWidth"/>.</param>
        /// <param name="maxWidth">Maximal possible width of the column.</param>
        public virtual Coord? WidthInPercentToDips(
            int widthInPercent, Coord? minWidth = null, Coord? maxWidth = null)
        {
            if (!IsAttached || ListView!.IsDisposed)
                return null;

            if (widthInPercent <= 0)
                widthInPercent = 0;

            var width = ListView.ClientSize.Width;
            var columnWidth = (width / 100) * widthInPercent;

            if(minWidth < 0)
            {
                var titleWidth = TitleSize?.Width;

                if (titleWidth is not null)
                    minWidth = titleWidth.Value - minWidth;
                else
                    minWidth = 0;
            }

            var adjustedWidth = MathUtils.ApplyMinMax(columnWidth, minWidth, maxWidth);

            return adjustedWidth;
        }

        /// <summary>
        /// Creates copy of this <see cref="ListViewColumn"/>.
        /// </summary>
        public virtual ListViewColumn Clone()
        {
            var result = new ListViewColumn();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Assigns properties from another <see cref="ListViewColumn"/>.
        /// </summary>
        /// <param name="item">Source of the properties to assign.</param>
        public virtual void Assign(ListViewColumn item)
        {
            Tag = item.Tag;

            bool applyWidth = (widthMode != item.WidthMode) || (width != item.Width);
            bool applyTitle = title != item.Title;

            title = item.Title;
            widthMode = item.WidthMode;
            width = item.Width;

            if (applyTitle && applyWidth)
                RaiseChanged(ColumnEventType.AllChanged);
            else
            if(applyTitle)
                RaiseChanged(ColumnEventType.TitleChanged);
            if (applyWidth)
                RaiseChanged(ColumnEventType.WidthChanged);
        }

        /// <inheritdoc cref="ListControlItem.ToString"/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Title))
                return base.ToString() ?? nameof(ListViewColumn);
            else
                return Title;
        }

        /// <summary>
        /// Sets owner control and column index. Do not call directly.
        /// </summary>
        /// <param name="control">Owner control.</param>
        /// <param name="newIndex">New column index.</param>
        public virtual void InternalSetListViewAndIndex(ListView? control, int? newIndex)
        {
            var changed = listView != control || index != newIndex;
            if (changed)
            {
                listView = control;
                index = newIndex;
                RaiseChanged(ColumnEventType.AllChanged);
            }
        }

        /// <summary>
        /// Notifies container about the column changes.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        public virtual void RaiseChanged(ColumnEventType eventType = ColumnEventType.AllChanged)
        {
            if (!IsAttached)
                return;
            listView!.HandleColumnChanged(this, eventType);
        }
    }
}