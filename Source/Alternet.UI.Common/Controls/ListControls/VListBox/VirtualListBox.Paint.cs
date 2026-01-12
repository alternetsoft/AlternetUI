using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class VirtualListBox
    {
        private Color? emptyTextForeColor;
        private DrawItemEventArgs? drawItemArgs;
        private MeasureItemEventArgs? measureItemArgs;

        /// <summary>
        /// Represents the default foreground color used for empty text elements.
        /// </summary>
        public static Color DefaultEmptyTextForeColor = Color.Gray;

        /// <summary>
        /// Gets or sets the foreground color used to render the empty text string.
        /// Default is <see langword="null"/>, which indicates that the
        /// <see cref="DefaultEmptyTextForeColor"/> is used.
        /// </summary>
        public virtual Color? EmptyTextForeColor
        {
            get => emptyTextForeColor;
            set
            {
                if (emptyTextForeColor != value)
                {
                    emptyTextForeColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DrawItemEventArgs"/> used during item drawing.
        /// This property can be used during custom drawing operations.
        /// </summary>
        protected DrawItemEventArgs? DrawItemArgs
        {
            get => drawItemArgs;
            set => drawItemArgs = value;
        }

        /// <summary>
        /// Draws the empty text string, if defined, centered within the specified rectangle using the provided graphics
        /// context.
        /// </summary>
        /// <remarks>If the empty text is null or empty, no text is drawn. The text is rendered using a
        /// gray color and is centered both horizontally and vertically within the specified rectangle.</remarks>
        /// <param name="dc">The graphics context used to render the empty text.</param>
        /// <param name="paintRectangle">The rectangle that defines the area in which to center and draw the empty text.</param>
        protected virtual void PaintEmptyText(Graphics dc, RectD paintRectangle)
        {
            var s = EmptyText;

            if (s is null || s.Length == 0)
                return;

            var size = dc.MeasureText(s, RealFont);

            var alignedRect = AlignUtils.AlignRectInRect(
                (PointD.Empty, size),
                paintRectangle,
                HorizontalAlignment.Center,
                VerticalAlignment.Center,
                false);
            var location = alignedRect.Location;
            dc.DrawText(s, location.ClampToZero(), RealFont, EmptyTextForeColor ?? DefaultEmptyTextForeColor, Color.Empty);
        }

        /// <summary>
        /// Paints a single row in the list control using the specified drawing parameters.
        /// </summary>
        /// <remarks>Override this method to customize the appearance of individual rows. The method
        /// adapts its painting logic based on the current draw mode and selection state. When overriding, ensure that
        /// custom drawing respects the provided drawing context and item state.</remarks>
        /// <param name="dc">The graphics context used to render the row.</param>
        /// <param name="rowIndex">The zero-based index of the row to paint.</param>
        /// <param name="item">The item associated with the row to be painted, or null
        /// if the row does not correspond to a specific item.</param>
        /// <param name="rectRow">The bounding rectangle, in client coordinates, that defines the area of the row to paint.</param>
        public virtual void PaintRow(Graphics dc, int rowIndex, ListControlItem? item, RectD rectRow)
        {
            if (drawItemArgs is null)
            {
                drawItemArgs = new(dc);
            }
            else
            {
                drawItemArgs.Assign(dc);
            }

            var isCurrentItem = IsCurrent(rowIndex);
            var isSelectedItem = IsSelected(rowIndex);

            if (drawMode != DrawMode.Normal)
            {
                drawItemArgs.Bounds = rectRow;
                drawItemArgs.Index = rowIndex;

                DrawItemState state = 0;
                if (isCurrentItem)
                    state |= DrawItemState.Focus;
                if (isSelectedItem)
                    state |= DrawItemState.Selected;

                drawItemArgs.Font = ListControlItem.GetFont(item, this, isSelectedItem);

                drawItemArgs.State = state;

                if (isSelectedItem)
                {
                    drawItemArgs.BackColor
                        = ListControlItem.GetSelectedItemBackColor(item, this)
                        ?? RealBackgroundColor;
                    drawItemArgs.ForeColor
                        = ListControlItem.GetSelectedTextColor(item, this)
                        ?? RealForegroundColor;
                }
                else
                {
                    drawItemArgs.BackColor = RealBackgroundColor;
                    drawItemArgs.ForeColor
                        = ListControlItem.GetItemTextColor(item, this)
                        ?? RealForegroundColor;
                }

                RaiseDrawItem(drawItemArgs);
            }
            else
            {
                if (itemPaintArgs is null)
                {
                    itemPaintArgs = new(this, dc, rectRow, rowIndex);
                }
                else
                {
                    itemPaintArgs.Graphics = dc;
                    itemPaintArgs.ClientRectangle = rectRow;
                    itemPaintArgs.ItemIndex = rowIndex;
                }

                itemPaintArgs.LabelMetrics = new();
                itemPaintArgs.IsCurrent = isCurrentItem;
                itemPaintArgs.IsSelected = isSelectedItem;
                itemPaintArgs.Visible = true;
                itemPaintArgs.UseColumns = HasColumns;

                DrawItemBackground(itemPaintArgs);
                DrawItemForeground(itemPaintArgs);
            }
        }

        /// <summary>
        /// Measures the sizes of rows within the specified index range and returns their dimensions.
        /// </summary>
        /// <remarks>The method measures rows in the range [fromIndex, toIndex), where fromIndex is
        /// inclusive and toIndex is exclusive. If fromIndex or toIndex is not specified, the method uses the visible
        /// row range as determined by GetVisibleBegin() and GetVisibleEnd(). The returned array contains the sizes in
        /// the order corresponding to the measured rows.</remarks>
        /// <param name="dc">The graphics context used to perform the measurement. Cannot be null.</param>
        /// <param name="fromIndex">The zero-based index of the first row to measure.
        /// If null, measurement starts from the first visible row.</param>
        /// <param name="toIndex">The zero-based index after the last row to measure.
        /// If null, measurement continues to the last visible row.</param>
        /// <returns>An array of <see cref="SizeD"/> values representing the measured size of each row in the specified range.
        /// The length of the array equals the number of rows measured.</returns>
        public virtual SizeD[] MeasureRows(Graphics dc, int? fromIndex = null, int? toIndex = null)
        {
            int lineMax = toIndex ?? GetVisibleEnd();
            int lineMin = fromIndex ?? GetVisibleBegin();
            int count = lineMax - lineMin;

            var sizes = new SizeD[count];

            MeasureItemEventArgs.EnsureCreated(ref measureItemArgs, dc);

            for (int i = 0, line = lineMin; line < lineMax; i++, line++)
            {
                measureItemArgs.Index = line;
                MeasureItemSize(measureItemArgs);
                sizes[i] = measureItemArgs.ItemSize;
            }

            return sizes;
        }

        /// <summary>
        /// Paints the visible rows within the specified rectangle using the provided graphics context, applying an
        /// optional width increment to the painting area.
        /// </summary>
        /// <remarks>This method iterates over all visible rows and paints each one according to the
        /// current drawing mode and selection state. If partial row visibility is disabled, only fully visible rows
        /// within the specified rectangle are painted. The method updates the list of items that were last painted.
        /// Override this method to customize row painting behavior.</remarks>
        /// <param name="dc">The graphics context used to render the rows.</param>
        /// <param name="fromIndex">The optional starting index of the rows to paint.
        /// If null, painting starts from the first visible row.</param>
        /// <param name="toIndex">The optional ending index of the rows to paint.
        /// If null, painting ends at the last visible row.</param>
        /// <param name="paintRectangle">The rectangle that defines the area in which rows are painted.</param>
        /// <param name="widthIncrement">The additional width, in device units, to add to the painting
        /// rectangle before rendering the rows.</param>
        public virtual void PaintRows(
            Graphics dc,
            RectD paintRectangle,
            float widthIncrement,
            int? fromIndex = null,
            int? toIndex = null)
        {
            int lineMax = toIndex ?? GetVisibleEnd();
            int lineMin = fromIndex ?? GetVisibleBegin();

            var r = paintRectangle;
            r.Width += widthIncrement;
            var rectRow = r;

            MeasureItemEventArgs.EnsureCreated(ref measureItemArgs, dc);

            itemsLastPainted.Clear();

            var rowSizes = MeasureRows(dc, fromIndex, toIndex);

            for (int line = lineMin; line < lineMax; line++)
            {
                var hRow = rowSizes[line - lineMin].Height;

                rectRow.Height = hRow;

                if (!IsPartialRowVisible)
                {
                    if (!r.Contains(rectRow))
                        continue;
                }

                var item = SafeItem(line);

                PaintRow(dc, line, item, rectRow);

                if (item is not null)
                    itemsLastPainted.Add(item);

                rectRow.Top += hRow;
            }
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            var dc = e.Graphics;

            dc.FillRectangle(RealBackgroundColor.AsBrush, ClientRectangle);

            UpdateInteriorProperties();

            var r = GetPaintRectangle();

            if (Count == 0)
            {
                PaintEmptyText(dc, r);
            }
            else
            {
                InternalPaint();
            }

            DrawInterior(dc);

            void InternalPaint()
            {
                dc.PushAndTranslate(-scrollOffsetX, 0);
                try
                {
                    PaintRows(dc, r, scrollOffsetX);
                }
                finally
                {
                    dc.PopTransform();
                }
            }
        }
    }
}
