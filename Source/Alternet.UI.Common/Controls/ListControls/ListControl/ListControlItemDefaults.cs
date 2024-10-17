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
        public virtual SizeI? SvgImageSize { get; set; }

        /// <inheritdoc/>
        public virtual bool SelectedItemIsBold { get; set; } = false;

        /// <inheritdoc/>
        public virtual Coord MinItemHeight { get; set; } = VirtualListBox.DefaultMinItemHeight;

        /// <inheritdoc/>
        public virtual LightDarkColor? ItemTextColor { get; set; }

        /// <inheritdoc/>
        public virtual GenericAlignment ItemAlignment { get; set; }
            = ListControlItem.DefaultItemAlignment;

        /// <inheritdoc/>
        public virtual LightDarkColor? SelectedItemTextColor { get; set; }

        /// <inheritdoc/>
        public virtual LightDarkColor? DisabledItemTextColor { get; set; }

        /// <inheritdoc/>
        public virtual bool SelectionVisible { get; set; } = true;

        /// <inheritdoc/>
        public virtual bool CheckBoxVisible { get; set; } = false;

        /// <inheritdoc/>
        public bool CheckBoxThreeState { get; set; } = false;

        /// <inheritdoc/>
        public virtual bool TextVisible { get; set; } = true;

        /// <inheritdoc/>
        public virtual LightDarkColor? SelectedItemBackColor { get; set; }

        /// <inheritdoc/>
        public virtual bool CurrentItemBorderVisible { get; set; } = true;

        /// <inheritdoc/>
        public virtual BorderSettings? CurrentItemBorder { get; set; }

        /// <inheritdoc/>
        public virtual BorderSettings? SelectionBorder { get; set; }
    }
}
