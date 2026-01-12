using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow <see cref="ListControlItem"/>
    /// to get information about it's container.
    /// </summary>
    public interface IListControlItemContainer
    {
        /// <summary>
        /// Gets the unchecked state image for the checkbox.
        /// </summary>
        SvgImage? CheckImageUnchecked { get; }

        /// <summary>
        /// Gets the format provider to use for culture-specific formatting operations.
        /// </summary>
        IFormatProvider? FormatProvider { get; }

        /// <summary>
        /// Gets the checked state image for the checkbox.
        /// </summary>
        SvgImage? CheckImageChecked { get; }

        /// <summary>
        /// Gets the indeterminate state image for the checkbox.
        /// </summary>
        SvgImage? CheckImageIndeterminate { get; }

        /// <summary>
        /// Gets the image list associated with the container.
        /// </summary>
        ImageList? ImageList { get; }

        /// <summary>
        /// Gets unique id of the container.
        /// </summary>
        ObjectUniqueId UniqueId { get; }

        /// <summary>
        /// Gets control used by the container.
        /// </summary>
        AbstractControl? Control { get; }

        /// <summary>
        /// Gets defaults used when item is painted.
        /// </summary>
        IListControlItemDefaults Defaults { get; }

        /// <summary>
        /// Gets whether container is focused.
        /// </summary>
        bool Focused { get; }

        /// <summary>
        /// Gets <see cref="ListControlItem"/> item with the specified index.
        /// If index of the item is invalid or item is not <see cref="ListControlItem"/>,
        /// returns <c>null</c>.
        /// </summary>
        ListControlItem? SafeItem(int index);

        /// <summary>
        /// Returns the text representation of item with the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Item index from which to get the contents to display.</param>
        /// <param name="forDisplay">The flag which specifies whether to get text
        /// for display purposes or the real value.</param>
        string GetItemText(int index, bool forDisplay);

        /// <summary>
        /// Returns the text representation of the specified list control item.
        /// </summary>
        /// <param name="item">The list control item to retrieve the text for. If null, the method returns an empty string.</param>
        /// <param name="forDisplay">true to format the text for display purposes; otherwise, false to return the raw value.</param>
        /// <returns>A string containing the text of the item. Returns an empty string if item is null.</returns>
        string GetItemText(ListControlItem? item, bool forDisplay);

        /// <summary>
        /// Gets count of the items.
        /// </summary>
        /// <returns></returns>
        int GetItemCount();

        /// <summary>
        /// Gets a value indicating whether the container has columns.
        /// </summary>
        bool HasColumns { get; }

        /// <summary>
        /// Gets the width of the column separator, if defined.
        /// If not specified, it is suggested to use the default width, which is specified in
        /// <see cref="ListControlColumn.DefaultColumnSeparatorWidth"/>.
        /// </summary>
        Coord? ColumnSeparatorWidth { get; }

        /// <summary>
        /// Gets the collection of columns within the list control.
        /// </summary>
        IReadOnlyList<ListControlColumn> Columns { get; }
    }
}