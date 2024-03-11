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

        /// <summary>
        /// Initializes a new instance of the <see cref="VListBox"/> class.
        /// </summary>
        public VListBox()
        {
            UserPaint = true;
            SuggestedSize = 200;
        }

        /// <summary>
        /// Gets or sets disabled item text color.
        /// </summary>
        public Color? DisabledItemTextColor
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
        public bool SelectedItemIsBold
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
        public Color? SelectedItemTextColor
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
        public Color? SelectedItemBackColor
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
        public Color? ItemTextColor
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

            var font = GetItemFont(itemIndex).AsBold;
            var size = MeasureCanvas.MeasureText(s, font);
            size.Width += ItemMargin.Horizontal;
            size.Height += ItemMargin.Vertical;
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

            if (Enabled)
            {
                if (isSelected)
                    dc.FillRectangle(GetSelectedItemBackColor(e.ItemIndex), rect);
                if (isCurrent && Focused)
                    dc.FillRectangleBorder(Color.Black, rect);
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
        /// Default method which draws items. Called from <see cref="DrawItem"/>.
        /// </summary>
        public virtual void DefaultDrawItem(ListBoxItemPaintEventArgs e)
        {
            e.Graphics.DrawText(
                e.ItemText,
                e.ItemFont,
                e.TextColor.AsBrush,
                e.ClipRectangle);
        }

        /// <summary>
        /// Gets selected item text color. Default is <see cref="SelectedItemTextColor"/>
        /// (if it is not <c>null</c>) or <see cref="DefaultSelectedItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetSelectedItemTextColor(int itemIndex)
        {
            if (Enabled)
                return SelectedItemTextColor ?? DefaultSelectedItemTextColor;
            else
                return GetDisabledItemTextColor(itemIndex);
        }

        /// <summary>
        /// Gets item text color. Default is <see cref="ItemTextColor"/> (if it is not <c>null</c>)
        /// or <see cref="DefaultItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetItemTextColor(int itemIndex)
        {
            if (Enabled)
                return ForegroundColor ?? ItemTextColor ?? DefaultItemTextColor;
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
            if (Enabled)
                return selectedItemBackColor ?? DefaultSelectedItemBackColor;
            else
                return GetDisabledItemTextColor(itemIndex);
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
