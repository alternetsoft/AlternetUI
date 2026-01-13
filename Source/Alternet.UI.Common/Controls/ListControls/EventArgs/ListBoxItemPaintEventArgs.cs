using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="IListBoxItemPainter.Paint"/> event.
    /// </summary>
    public class ListBoxItemPaintEventArgs : PaintEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItemPaintEventArgs"/> class.
        /// </summary>
        /// <param name="control">Control which owns the item.</param>
        /// <param name="graphics"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="bounds">Bounds of the item.</param>
        /// <param name="itemIndex">Index of the tem.</param>
        public ListBoxItemPaintEventArgs(
            IListControlItemContainer control,
            Graphics graphics,
            RectD bounds,
            int itemIndex)
            : base(() => graphics, bounds)
        {
            ListBox = control;
            this.ItemIndex = itemIndex;
        }

        /// <summary>
        /// Gets index of the item.
        /// </summary>
        public virtual int ItemIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether item is selected.
        /// </summary>
        public virtual bool IsSelected { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current item is selected, taking into account any selection visibility
        /// settings.
        /// </summary>
        /// <remarks>If the associated item has selection visibility disabled, this property returns false
        /// regardless of the actual selection state.</remarks>
        public virtual bool HasSelection
        {
            get
            {
                var hideSelection = Item?.HideSelection ?? false;

                bool isSelected;
                if (hideSelection)
                    isSelected = false;
                else
                    isSelected = IsSelected;
                return isSelected;
            }
        }

        /// <summary>
        /// Gets the rectangle, in client coordinates, that defines the area available for painting the item's content,
        /// excluding any padding. This property uses the padding settings defined by the associated list control to
        /// determine the available painting area. <see cref="PaintEventArgs.ClientRectangle"/> is used as the base rectangle from which
        /// the padding is applied.
        /// </summary>
        /// <remarks>The returned rectangle accounts for the item's padding as determined by the
        /// associated list control. Use this property when performing custom drawing to ensure content is rendered
        /// within the correct bounds.</remarks>
        public virtual RectD PaintRectangle
        {
            get
            {
                Thickness padding = ListControlItem.GetPadding(Item, ListBox);
                var paintRectangle = ClientRectangle;
                paintRectangle.ApplyMargin(padding);
                return paintRectangle;
            }
        }

        /// <summary>
        /// Gets whether item is current.
        /// </summary>
        public virtual bool IsCurrent { get; set; }

        /// <summary>
        /// Gets font of the item.
        /// </summary>
        public virtual Font ItemFont
        {
            get
            {
                var itemFont = ListControlItem.GetFont(Item, ListBox, IsSelected);
                return itemFont;
            }
        }

        /// <summary>
        /// Gets an item.
        /// </summary>
        public virtual ListControlItem? Item
        {
            get
            {
                return ListBox.SafeItem(ItemIndex);
            }
        }

        /// <summary>
        /// Gets foreground color of the item.
        /// </summary>
        public virtual Color? TextColor
        {
            get
            {
                return GetTextColor(IsSelected);
            }
        }

        /// <summary>
        /// Gets text of the item.
        /// </summary>
        public virtual string ItemText
        {
            get
            {
                return ListBox.GetItemText(ItemIndex, false);
            }
        }

        /// <summary>
        /// Gets the text to display for the item, taking into account the current visibility settings.
        /// </summary>
        /// <remarks>If the associated list box is configured to hide text, this property returns an empty
        /// string. Otherwise, it returns the display text for the item. This property is intended for use in UI
        /// scenarios where the visibility of item text may be toggled.</remarks>
        public virtual string VisibleTextForDisplay
        {
            get
            {
                var textVisible = ListBox?.Defaults.TextVisible ?? true;

                var s = textVisible ? ItemTextForDisplay : string.Empty;
                return s;
            }
        }

        /// <summary>
        /// Gets text of the item for display.
        /// </summary>
        public virtual string ItemTextForDisplay
        {
            get
            {
                return ListBox.GetItemText(ItemIndex, true);
            }
        }

        /// <summary>
        /// Gets minimal height of the item.
        /// </summary>
        public virtual Coord ItemMinHeight
        {
            get
            {
                var result = ListControlItem.GetMinHeight(Item, ListBox);
                return result;
            }
        }

        /// <summary>
        /// Gets alignment of the item.
        /// </summary>
        public virtual HVAlignment ItemAlignment
        {
            get
            {
                var result = ListControlItem.GetAlignment(Item, ListBox);
                return result;
            }
        }

        /// <summary>
        /// Gets or sets whether real painting need to be performed. When <see cref="Visible"/>
        /// is False, caller need only <see cref="LabelMetrics"/> without any actual painting.
        /// </summary>
        public virtual bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to use columns for item painting.
        /// </summary>
        public virtual bool UseColumns { get; set; } = false;

        /// <summary>
        /// Gets draw label result returned after painting of the label is performed.
        /// </summary>
        public virtual Graphics.DrawLabelParams LabelMetrics { get; set; }

        /// <summary>
        /// Gets normal and disabled images of the item.
        /// </summary>
        public virtual EnumArrayStateImages ItemImages
        {
            get
            {
                return GetItemImages(Item, ListBox);
            }
        }

        /// <summary>
        /// Gets container control in which item is painted.
        /// </summary>
        public virtual IListControlItemContainer ListBox { get; set; }

        /// <summary>
        /// Retrieves the set of images associated with the specified list control item, taking into account its
        /// selection state and the containing list box.
        /// </summary>
        /// <param name="item">The list control item for which to obtain images.
        /// This parameter can be null to indicate no specific item.</param>
        /// <param name="listBox">The container that holds the list control item.
        /// This parameter can be null if the item is not associated
        /// with a container.</param>
        /// <returns>An object containing the images representing the visual states of the specified item. The returned value
        /// reflects the item's current state within the provided container.</returns>
        public virtual EnumArrayStateImages GetItemImages(ListControlItem? item, IListControlItemContainer? listBox)
        {
            var color = ListControlItem.GetSelectedTextColor(item, listBox);
            return ListControlItem.GetItemImages(item, listBox, color);
        }

        /// <summary>
        /// Retrieves the image associated with the specified list control item, based on its selection and enabled
        /// state.
        /// </summary>
        /// <remarks>The returned image reflects the item's visual state: selected, normal, or disabled,
        /// depending on the parameters provided and the enabled state of the container.</remarks>
        /// <param name="item">The list control item for which to retrieve the image. Can be null if no item is specified.</param>
        /// <param name="listBox">The container that holds the list control item. Can be null if
        /// the item is not associated with a container.</param>
        /// <param name="isSelected">true if the item is currently selected; otherwise, false.</param>
        /// <returns>The image corresponding to the item's current state, or null if no image is available for the specified item
        /// and state.</returns>
        public virtual Image? GetImage(ListControlItem? item, IListControlItemContainer? listBox, bool isSelected)
        {
            var itemImages = GetItemImages(item, listBox);
            var normalImage = itemImages[VisualControlState.Normal];
            var disabledImage = itemImages[VisualControlState.Disabled];
            var selectedImage = itemImages[VisualControlState.Selected];

            var image = ListControlItem.IsContainerEnabled(listBox)
                ? (isSelected ? selectedImage : normalImage) : disabledImage;
            return image;
        }

        /// <summary>
        /// Retrieves the image associated with the current item state, based on whether the item is selected and
        /// enabled.
        /// </summary>
        /// <remarks>If the item is disabled, the disabled image is returned regardless of the selection
        /// state. If the item is enabled, the selected or normal image is returned based on the value of
        /// isSelected.</remarks>
        /// <param name="isSelected">true to retrieve the image for the selected state;
        /// false to retrieve the image for the normal or disabled
        /// state.</param>
        /// <returns>An Image representing the item's visual state. Returns null if no image
        /// is defined for the current state.</returns>
        public virtual Image? GetImage(bool isSelected)
        {
            return GetImage(Item, ListBox, isSelected);
        }

        /// <summary>
        /// Gets text color.
        /// </summary>
        /// <param name="isSelected">Whether to get text color for the selected state.</param>
        /// <returns></returns>
        public virtual Color? GetTextColor(bool isSelected)
        {
            Color? textColor;
            if (isSelected)
            {
                textColor = ListControlItem.GetSelectedTextColor(Item, ListBox);
            }
            else
            {
                textColor = ListControlItem.GetItemTextColor(Item, ListBox);
            }

            return textColor;
        }
    }
}
