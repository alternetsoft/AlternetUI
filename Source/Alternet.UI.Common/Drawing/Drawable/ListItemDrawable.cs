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

        /// <inheritdoc/>
        protected override void OnDraw(AbstractControl control, Graphics dc)
        {
            if (item is null || Container is null || !Visible)
                return;

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

            item.DrawBackground(Container, paintArguments);
            item.DrawForeground(Container, paintArguments);
        }
    }
}