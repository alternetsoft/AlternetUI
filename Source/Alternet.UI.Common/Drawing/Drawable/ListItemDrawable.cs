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
        /// Gets or sets whether the item is current in the container.
        /// This property is used to determine the state of the item and to select the appropriate colors for painting.
        /// </summary>
        public virtual bool IsCurrentItem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether item's columns are used when item is painted.
        /// </summary>
        public virtual bool UseColumns { get; set; }

        /// <summary>
        /// Gets or sets whether the item is selected in the container.
        /// This property is used to determine the state of the item and to select the appropriate colors for painting.
        /// </summary>
        public virtual bool IsSelectedItem { get; set; }

        /// <summary>
        /// Gets or sets container of the item.
        /// </summary>
        public virtual IListControlItemContainer? Container { get; set; }

        /// <summary>
        /// Gets preferred size in device-independent units.
        /// </summary>
        /// <returns>Preferred size of the item.</returns>
        public virtual SizeD GetPreferredSize()
        {
            if (Container?.Control is null)
                return SizeD.Empty;

            var result = ListControlItem.DefaultMeasureItemSize(Container, Container.Control.MeasureCanvas, 0);
            result += Item.ForegroundMargin.Size;

            var checkBoxInfo = Item.GetCheckBoxInfo(Container, (PointD.Empty, result));

            result.Width += checkBoxInfo.IsCheckBoxVisible ? checkBoxInfo.TextRect.X : 0;

            return result;
        }

        /// <summary>
        /// Prepares the arguments required for painting the item using the specified graphics context.
        /// </summary>
        /// <remarks>Call this method before performing any custom drawing operations to ensure that the
        /// paint arguments reflect the current state of the item and its container. Returns false if the container is
        /// not set, in which case painting should not proceed.</remarks>
        /// <param name="dc">The graphics context to use for rendering the item.</param>
        /// <returns>true if the paint arguments were successfully prepared; otherwise, false.</returns>
        protected virtual bool PreparePaintArguments(Graphics dc)
        {
            if (Container is null)
                return false;
            if (paintArguments is null)
            {
                paintArguments = new(Container, dc, Bounds, ItemIndex);
            }
            else
            {
                paintArguments.ListBox = Container;
                paintArguments.Graphics = dc;
                paintArguments.ClientRectangle = Bounds;
                paintArguments.ItemIndex = ItemIndex;
            }

            paintArguments.LabelMetrics = new();
            paintArguments.IsCurrent = IsCurrentItem;
            paintArguments.IsSelected = IsSelectedItem;
            paintArguments.Visible = true;
            paintArguments.UseColumns = UseColumns;

            return true;
        }

        /// <inheritdoc/>
        protected override void OnDraw(AbstractControl control, Graphics dc)
        {
            if (item is null || Container is null || !Visible)
                return;
            PreparePaintArguments(dc);
            item.DrawBackground(Container, paintArguments!);
            item.DrawForeground(Container, paintArguments!);
        }
    }
}