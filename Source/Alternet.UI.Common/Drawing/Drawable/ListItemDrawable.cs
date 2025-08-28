using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements <see cref="ListControlItem"/> painting.
    /// </summary>
    public class ListItemDrawable : BaseDrawable
    {
        private ListControlItem? item;
        private ListBoxItemPaintEventArgs? paintArguments;

        /// <summary>
        /// Gets or sets item to paint.
        /// </summary>
        public virtual ListControlItem Item
        {
            get => item ??= new();
            set => item = value;
        }

        /// <summary>
        /// Gets or sets index of the item in the container.
        /// </summary>
        public virtual int ItemIndex { get; set; }

        /// <summary>
        /// Gets or sets container of the item.
        /// </summary>
        public virtual IListControlItemContainer? Container { get; set; }

        /// <inheritdoc/>
        protected override void OnDraw(AbstractControl control, Graphics dc)
        {
            if (item is null || Container is null || !Visible)
                return;

            if(paintArguments is null)
            {
                paintArguments = new(Container, dc, Bounds, ItemIndex);
            }
            else
            {
                paintArguments.ListBox = Container;
                paintArguments.Graphics = dc;
                paintArguments.ClipRectangle = Bounds;
                paintArguments.ItemIndex = ItemIndex;
            }

            item.DrawBackground(Container, paintArguments);
        }
    }
}
