﻿using System;
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
        /// Gets count of the items.
        /// </summary>
        /// <returns></returns>
        int GetItemCount();
    }
}