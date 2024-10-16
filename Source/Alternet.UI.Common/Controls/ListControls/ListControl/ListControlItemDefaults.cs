using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains defaults used when <see cref="ListControlItem"/> is painted.
    /// Implements <see cref="IListControlItemDefaults"/>.
    /// </summary>
    public class ListControlItemDefaults : IListControlItemDefaults
    {
        /// <inheritdoc/>
        public SizeI? SvgImageSize { get; set; }

        /// <inheritdoc/>
        public bool SelectedItemIsBold { get; set; } = false;

        /// <inheritdoc/>
        public Coord MinItemHeight { get; set; } = VirtualListBox.DefaultMinItemHeight;

        /// <inheritdoc/>
        public Color? ItemTextColor { get; set; }

        /// <inheritdoc/>
        public GenericAlignment ItemAlignment { get; set; }
            = ListControlItem.DefaultItemAlignment;

        /// <inheritdoc/>
        public Color? SelectedItemTextColor { get; set; }

        /// <inheritdoc/>
        public Color? DisabledItemTextColor { get; set; }

        /// <inheritdoc/>
        public bool SelectionVisible { get; set; } = true;

        /// <inheritdoc/>
        public bool CheckBoxVisible { get; set; } = false;
    }
}
