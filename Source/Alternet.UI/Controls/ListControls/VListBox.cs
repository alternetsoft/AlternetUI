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
        public virtual Font GetItemFont()
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

            var font = GetItemFont().AsBold;
            var size = MeasureCanvas.MeasureText(s, font);
            size.Width += ItemMargin.Horizontal;
            size.Height += ItemMargin.Vertical;
            return size;
        }

        /*public virtual bool IsSelected(int index)
        {
            if (SelectionMode == ListBoxSelectionMode.Single)
                return NativeControl.GetSelection() == index;

            var selCount = NativeControl.GetSelectedCount();

            if (selCount == 0)
                return false;

            var firstSelected = NativeControl.GetFirstSelected();
            if (firstSelected == index)
                return true;

            while (true)
            {
                var selected = NativeControl.GetNextSelected();
                if (selected == index)
                    return true;
                if (selected < 0)
                    return false;
            }
        }*/

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
        /// </summary>
        /// <param name="dc">The <see cref="Graphics" /> surface on which to draw.</param>
        /// <param name="rect">Rectangle in which to draw the item.</param>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual void DrawItemBackground(Graphics dc, RectD rect, int itemIndex)
        {
            var isSelected = IsSelected(itemIndex);
            var isCurrent = IsCurrent(itemIndex);

            if (Enabled)
            {
                if (isSelected)
                    dc.FillRectangle(GetSelectedItemBackColor(), rect);
                if (isCurrent && Focused)
                    dc.FillRectangleBorder(Color.Black, rect);
            }
        }

        /// <summary>
        /// Draws item with the specified index.  If <see cref="ItemPainter"/> is assigned, uses
        /// <see cref="IListBoxItemPainter.Paint"/>, otherwise calls
        /// <see cref="DefaultDrawItem"/>.
        /// </summary>
        /// <param name="dc">The <see cref="Graphics" /> surface on which to draw.</param>
        /// <param name="rect">Rectangle in which to draw the item.</param>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual void DrawItem(Graphics dc, RectD rect, int itemIndex)
        {
            rect.ApplyMargin(ItemMargin);

            if (itemPaintArgs is null)
                itemPaintArgs = new(this, dc, rect, itemIndex);
            else
            {
                itemPaintArgs.Graphics = dc;
                itemPaintArgs.ClipRectangle = rect;
                itemPaintArgs.ItemIndex = itemIndex;
            }

            if (painter is null)
            {
                DefaultDrawItem(itemPaintArgs);
                return;
            }

            painter.Paint(this, itemPaintArgs);
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
        public virtual Color GetSelectedItemTextColor()
        {
            if (Enabled)
                return SelectedItemTextColor ?? DefaultSelectedItemTextColor;
            else
                return GetDisabledItemTextColor();
        }

        /// <summary>
        /// Gets item text color. Default is <see cref="ItemTextColor"/> (if it is not <c>null</c>)
        /// or <see cref="DefaultItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetItemTextColor()
        {
            if (Enabled)
                return ForegroundColor ?? ItemTextColor ?? DefaultItemTextColor;
            else
                return GetDisabledItemTextColor();
        }

        /// <summary>
        /// Gets selected item back color. Default is <see cref="SelectedItemBackColor"/>
        /// (if it is not <c>null</c>) or <see cref="DefaultSelectedItemBackColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetSelectedItemBackColor()
        {
            if (Enabled)
                return selectedItemBackColor ?? DefaultSelectedItemBackColor;
            else
                return GetDisabledItemTextColor();
        }

        /// <summary>
        /// Gets disabled item text color.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetDisabledItemTextColor()
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

            var rectUpdate = e.ClipRectangle; // GetUpdateClientRect();

            dc.FillRectangle(RealBackgroundColor.AsBrush, rectUpdate);

            // the bounding rectangle of the current line
            RectD rectRow = RectD.Empty;
            rectRow.Width = clientSize.Width;

            // iterate over all visible lines
            int lineMax = NativeControl.GetVisibleEnd();
            for (int line = NativeControl.GetVisibleBegin(); line < lineMax; line++)
            {
                var hRow = MeasureItemSize(line).Height;

                rectRow.Height = hRow;

                if (rectRow.IntersectsWith(rectUpdate))
                {
                    /*don't allow drawing outside of the lines rectangle
                    wxDCClipper clip(*dc, rectRow);*/

                    DrawItemBackground(dc, rectRow, line);
                    DrawItem(dc, rectRow, line);
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
