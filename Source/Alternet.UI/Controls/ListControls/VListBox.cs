using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Implements virtual ListBox control. This is experimental and will be changed at any time.
    /// </summary>
    public class VListBox : ListBox
    {
        /// <summary>
        /// Gets or sets default current item border.
        /// </summary>
        public static BorderSettings DefaultCurrentItemBorder;

        /// <summary>
        /// Gets or sets default item margin.
        /// </summary>
        public static Thickness DefaultItemMargin = 2;

        /// <summary>
        /// Gets or sets default selected item text color.
        /// </summary>
        public static Color DefaultSelectedItemTextColor = SystemColors.HighlightText;

        /// <summary>
        /// Gets or sets default selected item background color.
        /// </summary>
        public static Color DefaultSelectedItemBackColor = SystemColors.Highlight;

        /// <summary>
        /// Gets or sets default disabled item text color.
        /// </summary>
        public static Color DefaultDisabledItemTextColor = SystemColors.GrayText;

        /// <summary>
        /// Gets or sets default item text color.
        /// </summary>
        public static Color DefaultItemTextColor = SystemColors.WindowText;

        private Thickness itemMargin = DefaultItemMargin;
        private Color? selectedItemTextColor;
        private Color? itemTextColor;
        private Color? selectedItemBackColor;
        private bool selectedIsBold;
        private Color? disabledItemTextColor;
        private IListBoxItemPainter? painter;
        private ListBoxItemPaintEventArgs? itemPaintArgs;
        private double minItemHeight = 24;
        private bool textVisible = true;
        private bool currentItemBorderVisible = true;
        private bool selectionVisible = true;
        private BorderSettings? currentItemBorder;
        private BorderSettings? selectionBorder;

        private GenericAlignment itemAlignment
            = GenericAlignment.CenterVertical | GenericAlignment.Left;

        static VListBox()
        {
            DefaultCurrentItemBorder = new();
            DefaultCurrentItemBorder.Color = Color.Black;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VListBox"/> class.
        /// </summary>
        public VListBox()
        {
            UserPaint = true;
            SuggestedSize = 200;
        }

        /// <summary>
        /// Gets or sets current item border. If it is <c>null</c> (default value),
        /// <see cref="DefaultCurrentItemBorder"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual BorderSettings? CurrentItemBorder
        {
            get
            {
                return currentItemBorder;
            }

            set
            {
                if (currentItemBorder == value)
                    return;
                currentItemBorder = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets selection border.
        /// </summary>
        [Browsable(false)]
        public virtual BorderSettings? SelectionBorder
        {
            get
            {
                return selectionBorder;
            }

            set
            {
                if (selectionBorder == value)
                    return;
                selectionBorder = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether selection background is visible.
        /// </summary>
        public virtual bool SelectionVisible
        {
            get
            {
                return selectionVisible;
            }

            set
            {
                if (selectionVisible == value)
                    return;
                selectionVisible = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether current item border is visible.
        /// </summary>
        public virtual bool CurrentItemBorderVisible
        {
            get
            {
                return currentItemBorderVisible;
            }

            set
            {
                if (currentItemBorderVisible == value)
                    return;
                currentItemBorderVisible = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override bool UserPaint
        {
            get => base.UserPaint;
            set => base.UserPaint = true;
        }

        /// <summary>
        /// Gets or sets whether item text is displayed.
        /// </summary>
        public virtual bool TextVisible
        {
            get
            {
                return textVisible;
            }

            set
            {
                if (textVisible == value)
                    return;
                textVisible = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets minimal height of the items. Default is 24 dip.
        /// </summary>
        public virtual double MinItemHeight
        {
            get
            {
                return minItemHeight;
            }

            set
            {
                if (minItemHeight == value)
                    return;
                minItemHeight = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets disabled item text color.
        /// </summary>
        public virtual Color? DisabledItemTextColor
        {
            get
            {
                return disabledItemTextColor;
            }

            set
            {
                if (disabledItemTextColor == value)
                    return;
                disabledItemTextColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets item painter associated with the control.
        /// </summary>
        [Browsable(false)]
        public virtual IListBoxItemPainter? ItemPainter
        {
            get
            {
                return painter;
            }

            set
            {
                if (painter == value)
                    return;
                painter = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="ListControl.SelectedItem"/> has bold font.
        /// </summary>
        public virtual bool SelectedItemIsBold
        {
            get
            {
                return selectedIsBold;
            }

            set
            {
                if (selectedIsBold == value)
                    return;
                selectedIsBold = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets selected item text color.
        /// </summary>
        public virtual Color? SelectedItemTextColor
        {
            get
            {
                return selectedItemTextColor;
            }

            set
            {
                if (selectedItemTextColor == value)
                    return;
                selectedItemTextColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets selected item text color.
        /// </summary>
        public virtual Color? SelectedItemBackColor
        {
            get
            {
                return selectedItemBackColor;
            }

            set
            {
                if (selectedItemBackColor == value)
                    return;
                selectedItemBackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets item text color.
        /// </summary>
        public virtual Color? ItemTextColor
        {
            get
            {
                return itemTextColor;
            }

            set
            {
                if (itemTextColor == value)
                    return;
                itemTextColor = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override int? SelectedIndex
        {
            get
            {
                if (SelectionMode == ListBoxSelectionMode.Single)
                {
                    var result = NativeControl.GetSelection();
                    if (result < 0)
                        return null;
                    return result;
                }
                else
                {
                    var result = NativeControl.GetFirstSelected();
                    if (result < 0)
                        return null;
                    return result;
                }
            }

            set
            {
                if (SelectedIndex == value)
                    return;
                if (SelectionMode == ListBoxSelectionMode.Single)
                {
                    NativeControl.SetSelection(value ?? -1);
                }
                else
                {
                    NativeControl.ClearSelected();
                    if (value is not null && value >= 0)
                        NativeControl.SetSelected(value.Value, true);
                }
            }
        }

        /// <summary>
        /// Gets or sets default alignment of the items.
        /// </summary>
        /// <remarks>
        /// In order to set individual item alignment, item must be <see cref="ListControlItem"/>
        /// descendant, it has <see cref="ListControlItem.Alignment"/> property.
        /// </remarks>
        public virtual GenericAlignment ItemAlignment
        {
            get
            {
                return itemAlignment;
            }

            set
            {
                if (itemAlignment == value)
                    return;
                itemAlignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets item margin.
        /// </summary>
        public virtual Thickness ItemMargin
        {
            get
            {
                return itemMargin;
            }

            set
            {
                if (itemMargin == value)
                    return;
                itemMargin = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override int Count
        {
            get
            {
                var result = NativeControl.ItemsCount;
                return result;
            }

            set
            {
                NativeControl.ItemsCount = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="VListBoxHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new VListBoxHandler Handler
        {
            get
            {
                return (VListBoxHandler)base.Handler;
            }
        }

        /// <summary>
        /// Gets <see cref="NativeControl"/> attached to this control.
        /// </summary>
        internal new Native.VListBox NativeControl => (Native.VListBox)base.NativeControl;

        /// <summary>
        /// Gets item font. It must not be <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public virtual Font GetItemFont(int itemIndex)
        {
            var result = Font ?? UI.Control.DefaultFont;
            if (IsBold)
                result = result.AsBold;

            var item = SafeItem(itemIndex);
            if (item is not null)
            {
                var itemFont = item.Font;
                if (itemFont is not null)
                    result = itemFont;
                if (item.FontStyle is not null)
                    result = result.GetWithStyle(item.FontStyle.Value);
            }

            return result;
        }

        /// <summary>
        /// Measures item size. If <see cref="ItemPainter"/> is assigned, uses
        /// <see cref="IListBoxItemPainter.GetSize"/>, otherwise calls
        /// <see cref="DefaultMeasureItemSize"/>.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual SizeD MeasureItemSize(int itemIndex)
        {
            if (painter is null)
                return DefaultMeasureItemSize(itemIndex);
            var result = painter.GetSize(this, itemIndex);
            if(result == SizeD.MinusOne)
                return DefaultMeasureItemSize(itemIndex);
            return result;
        }

        /// <summary>
        /// Default method which measures item size. Called from <see cref="MeasureItemSize"/>.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual SizeD DefaultMeasureItemSize(int itemIndex)
        {
            var s = GetItemText(itemIndex);
            if(string.IsNullOrEmpty(s))
                s = "Wy";

            var (normal, disabled, selected) = GetItemImages(itemIndex);
            var maxHeightI =
                MathUtils.Max(normal?.Size.Height, disabled?.Size.Height, selected?.Size.Height);
            var maxHeightD = PixelToDip(maxHeightI);

            var font = GetItemFont(itemIndex).AsBold;
            var size = MeasureCanvas.MeasureText(s, font);
            size.Height = Math.Max(size.Height, maxHeightD);
            size.Width += ItemMargin.Horizontal;
            size.Height += ItemMargin.Vertical;
            size.Height = Math.Max(size.Height, GetItemMinHeight(itemIndex));
            return size;
        }

        /// <summary>
        /// Gets whether item with the specified index is selected.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public virtual bool IsSelected(int index)
        {
            return NativeControl.IsSelected(index);
        }

        /// <summary>
        /// Gets whether item with the specified index is visible.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public virtual bool IsVisible(int index)
        {
            return NativeControl.IsVisible(index);
        }

        /// <summary>
        /// Scroll by the specified number of rows which may be positive (to scroll down)
        /// or negative (to scroll up).
        /// </summary>
        /// <param name="rows">Number of items to scroll.</param>
        /// <returns><c>true</c> if the control was scrolled, <c>false</c> otherwise
        /// (for example, if we're trying to scroll down but we are already
        /// showing the last row).</returns>
        public virtual bool ScrollRows(int rows)
        {
            return NativeControl.ScrollRows(rows);
        }

        /// <summary>
        /// Scroll by the specified number of pages which may be positive
        /// (to scroll down) or negative (to scroll up).
        /// </summary>
        /// <param name="pages">Number of pages to scroll.</param>
        /// <returns><c>true</c> if the control was scrolled, <c>false</c> otherwise
        /// (for example, if we're trying to scroll down but we are already
        /// showing the last row).</returns>
        public virtual bool ScrollRowPages(int pages)
        {
            return NativeControl.ScrollRowPages(pages);
        }

        /// <summary>
        /// Triggers a refresh for just the given row's area of the control if it's visible.
        /// </summary>
        /// <param name="row">Item index.</param>
        public virtual void RefreshRow(int row)
        {
            NativeControl.RefreshRow(row);
        }

        /// <summary>
        /// Triggers a refresh for the area between the specified range of rows given (inclusively).
        /// </summary>
        /// <param name="from">First item index.</param>
        /// <param name="to">Last item index.</param>
        public virtual void RefreshRows(int from, int to)
        {
            NativeControl.RefreshRows(from, to);
        }

        /// <summary>
        /// Gets whether item with the specified index is current.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public virtual bool IsCurrent(int index)
        {
            return NativeControl.IsCurrent(index);
        }

        /// <summary>
        /// Draws background for the item with the specified index.
        /// If <see cref="ItemPainter"/> is assigned, uses
        /// <see cref="IListBoxItemPainter.PaintBackground"/>, otherwise calls
        /// <see cref="DefaultDrawItemBackground"/>.
        /// </summary>
        /// <param name="e">Draw parameters.</param>
        public virtual void DrawItemBackground(ListBoxItemPaintEventArgs e)
        {
            var result = painter?.PaintBackground(this, e) ?? false;
            if(!result)
                DefaultDrawItemBackground(e);
        }

        /// <summary>
        /// Draws default background for the item with the specified index.
        /// Used inside <see cref="DrawItemBackground"/>.
        /// </summary>
        /// <param name="e">Draw parameters.</param>
        public virtual void DefaultDrawItemBackground(ListBoxItemPaintEventArgs e)
        {
            var rect = e.ClipRectangle;
            var dc = e.Graphics;

            var isSelected = e.IsSelected;
            var isCurrent = e.IsCurrent;

            var item = SafeItem(e.ItemIndex);

            if (Enabled)
            {
                dc.FillBorderRectangle(
                    rect,
                    item?.BackgroundColor?.AsBrush,
                    item?.Border,
                    true,
                    this);

                if (isSelected && selectionVisible)
                {
                    dc.FillBorderRectangle(
                        rect,
                        GetSelectedItemBackColor(e.ItemIndex).AsBrush,
                        selectionBorder,
                        true,
                        this);
                }

                if (isCurrent && Focused && currentItemBorderVisible)
                {
                    var border = CurrentItemBorder ?? DefaultCurrentItemBorder;
                    border?.Draw(this, e.Graphics, rect);
                }
            }
            else
            {
                var border = item?.Border?.ToGrayScale();
                border?.Draw(this, e.Graphics, rect);
            }
        }

        /// <summary>
        /// Draws item with the specified index.  If <see cref="ItemPainter"/> is assigned, uses
        /// <see cref="IListBoxItemPainter.Paint"/>, otherwise calls
        /// <see cref="DefaultDrawItem"/>.
        /// </summary>
        /// <param name="e">Draw parameters.</param>
        public virtual void DrawItem(ListBoxItemPaintEventArgs e)
        {
            e.ClipRectangle = e.ClipRectangle.WithMargin(ItemMargin);

            if (painter is null)
            {
                DefaultDrawItem(e);
                return;
            }

            painter.Paint(this, e);
        }

        /// <summary>
        /// Gets index of the first visible item.
        /// </summary>
        /// <returns></returns>
        public virtual int GetVisibleBegin()
        {
            return NativeControl.GetVisibleBegin();
        }

        /// <summary>
        /// Gets index of the last visible item.
        /// </summary>
        /// <returns></returns>
        public virtual int GetVisibleEnd()
        {
            return NativeControl.GetVisibleEnd();
        }

        /// <summary>
        /// Gets suggested rectangles of the item's image and text.
        /// </summary>
        /// <param name="e">Item painting paramaters.</param>
        /// <returns></returns>
        public virtual (RectD ImageRect, RectD TextRect) GetItemImageRect(
            ListBoxItemPaintEventArgs e)
        {
            Thickness textMargin = Thickness.Empty;

            var offset = ComboBox.DefaultImageVerticalOffset;

            var size = e.ClipRectangle.Height - textMargin.Vertical - (offset * 2);
            var imageRect = new RectD(
                e.ClipRectangle.X + textMargin.Left,
                e.ClipRectangle.Y + textMargin.Top + offset,
                size,
                size);

            var itemRect = e.ClipRectangle;
            itemRect.X += imageRect.Width + ComboBox.DefaultImageTextDistance;
            itemRect.Width -= imageRect.Width + ComboBox.DefaultImageTextDistance;

            return (imageRect, itemRect);
        }

        /// <summary>
        /// Gets item alignment.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns></returns>
        public virtual GenericAlignment GetItemAlignment(int itemIndex)
        {
            var item = SafeItem(itemIndex);
            if(item is null)
                return ItemAlignment;
            return item.Alignment;
        }

        /// <summary>
        /// Gets item minimal height.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns></returns>
        public virtual double GetItemMinHeight(int itemIndex)
        {
            var item = SafeItem(itemIndex);
            if (item is null)
                return MinItemHeight;
            return Math.Max(item.MinHeight, MinItemHeight);
        }

        /// <summary>
        /// Gets item image.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns></returns>
        public virtual (Image? Normal, Image? Disabled, Image? Selected) GetItemImages(int itemIndex)
        {
            var item = SafeItem(itemIndex);
            if (item is null)
                return (null, null, null);
            return (
                item.Image,
                item.DisabledImage ?? item.Image,
                item.SelectedImage ?? item.Image);
        }

        /// <summary>
        /// Default method which draws items. Called from <see cref="DrawItem"/>.
        /// </summary>
        public virtual void DefaultDrawItem(ListBoxItemPaintEventArgs e)
        {
            var (normalImage, disabledImage, selectedImage) = e.ItemImages;
            var image = Enabled ? (e.IsSelected ? selectedImage : normalImage) : disabledImage;

            var s = textVisible ? e.ItemText.Trim() : string.Empty;

            if (image is not null && s != string.Empty)
                s = $" {s}";

            e.Graphics.DrawLabel(
                s,
                e.ItemFont,
                e.TextColor,
                Color.Empty,
                image,
                e.ClipRectangle,
                e.ItemAlignment);
        }

        /// <summary>
        /// Gets selected item text color. Default is <see cref="SelectedItemTextColor"/>
        /// (if it is not <c>null</c>) or <see cref="DefaultSelectedItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetSelectedItemTextColor(int itemIndex)
        {
            if (selectionVisible)
            {
                if (Enabled)
                    return SelectedItemTextColor ?? DefaultSelectedItemTextColor;
                else
                    return GetDisabledItemTextColor(itemIndex);
            }
            else
                return GetItemTextColor(itemIndex);
        }

        /// <summary>
        /// Gets item text color. Default is <see cref="ItemTextColor"/> (if it is not <c>null</c>)
        /// or <see cref="DefaultItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetItemTextColor(int itemIndex)
        {
            if (Enabled)
            {
                var itemColor = SafeItem(itemIndex)?.ForegroundColor;
                return itemColor ?? ForegroundColor ?? ItemTextColor ?? DefaultItemTextColor;
            }
            else
                return GetDisabledItemTextColor(itemIndex);
        }

        /// <summary>
        /// Gets selected item back color. Default is <see cref="SelectedItemBackColor"/>
        /// (if it is not <c>null</c>) or <see cref="DefaultSelectedItemBackColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetSelectedItemBackColor(int itemIndex)
        {
            if (Enabled && selectionVisible)
                return selectedItemBackColor ?? DefaultSelectedItemBackColor;
            else
                return RealBackgroundColor;
        }

        /// <summary>
        /// Gets disabled item text color.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetDisabledItemTextColor(int itemIndex)
        {
            return DisabledItemTextColor ?? DefaultDisabledItemTextColor;
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return new VListBoxHandler();
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var clientSize = ClientSize;

            var dc = e.Graphics;

            var rectUpdate = GetUpdateClientRect();

            dc.FillRectangle(RealBackgroundColor.AsBrush, rectUpdate);

            RectD rectRow = RectD.Empty;
            rectRow.Width = clientSize.Width;

            int lineMax = NativeControl.GetVisibleEnd();
            for (int line = NativeControl.GetVisibleBegin(); line < lineMax; line++)
            {
                var hRow = MeasureItemSize(line).Height;

                rectRow.Height = hRow;

                if (rectRow.IntersectsWith(rectUpdate))
                {
                    /*wxDCClipper clip(*dc, rectRow);*/

                    if (itemPaintArgs is null)
                        itemPaintArgs = new(this, dc, rectRow, line);
                    else
                    {
                        itemPaintArgs.Graphics = dc;
                        itemPaintArgs.ClipRectangle = rectRow;
                        itemPaintArgs.ItemIndex = line;
                    }

                    DrawItemBackground(itemPaintArgs);
                    DrawItem(itemPaintArgs);
                }
                else
                {
                    if (rectRow.Top > rectUpdate.Bottom)
                        break;
                }

                rectRow.Top += hRow;
            }
        }

        /// <inheritdoc/>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }
    }
}
