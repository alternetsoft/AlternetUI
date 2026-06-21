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
        /// <param name="paintRectangle">The rectangle that defines the area in which
        /// to center and draw the empty text.</param>
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
        /// <param name="rectRow">The bounding rectangle, in client coordinates,
        /// that defines the area of the row to paint.</param>
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
        /// Gets preferred content size based on the current items and layout of the control.
        /// </summary>
        /// <param name="dc">The graphics context used to perform the measurement.
        /// If null, the default measurement canvas is used.</param>
        /// <returns>A <see cref="MeasureContentSizeResult"/> representing the preferred content size.</returns>
        public virtual MeasureContentSizeResult GetPreferredContentSize(Graphics? dc = null)
        {
            ListControlItem.MeasureItemSizeParams prm = new();
            prm.UseColumnWidth = false;
            prm.RequestCellSize = true;

            var contentSize = GetContentSize(dc, prm);
            return contentSize;
        }

        /// <summary>
        /// Calculates the total size of all the rows.
        /// </summary>
        /// <param name="dc">The graphics context used to perform the measurement.
        /// If null, the default measurement canvas is used.</param>
        /// <returns>A <see cref="MeasureContentSizeResult"/> representing the total size of all the rows.</returns>
        /// <param name="prm">Optional parameters for additional measurement options.</param>
        public virtual MeasureContentSizeResult GetContentSize(
            Graphics? dc = null,
            ListControlItem.MeasureItemSizeParams? prm = null)
        {
            return GetContentSize(dc ?? MeasureCanvas, 0, Items.Count, prm);
        }

        /// <summary>
        /// Calculates the total size of the rows within the specified index range.
        /// </summary>
        /// <param name="dc">The graphics context used to perform the measurement.
        /// If null, the default measurement canvas is used.</param>
        /// <param name="fromIndex">The zero-based index of the first row to include in the calculation.
        /// If null, calculation starts from the first visible row.</param>
        /// <param name="toIndex">The zero-based index after the last row to include in the calculation.
        /// If null, calculation continues to the last visible row.</param>
        /// <returns>A <see cref="MeasureContentSizeResult"/> representing the total
        /// size of the rows within the specified range.</returns>
        /// <param name="prm">Optional parameters for additional measurement options.</param>
        public virtual MeasureContentSizeResult GetContentSize(
            Graphics? dc,
            int? fromIndex,
            int? toIndex = null,
            ListControlItem.MeasureItemSizeParams? prm = null)
        {
            MeasureContentSizeResult result = new();

            var rowSizes = MeasureRows(dc ?? MeasureCanvas, fromIndex, toIndex, prm);

            result.RowSizes = rowSizes;

            float totalHeight = 0;
            float maxWidth = 0;

            foreach (var size in rowSizes)
            {
                totalHeight += size.Height;
                maxWidth = Math.Max(maxWidth, size.Width);
            }

            result.ContentSize = new SizeD(maxWidth, totalHeight);

            return result;
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
        /// <returns>An array of <see cref="SizeD"/> values representing
        /// the measured size of each row in the specified range.
        /// The length of the array equals the number of rows measured.</returns>
        /// <param name="prm">The parameters used to measure the item sizes. Can be null.</param>
        public virtual ListControlItem.MeasureItemSizeResult[] MeasureRows(
            Graphics dc,
            int? fromIndex = null,
            int? toIndex = null,
            ListControlItem.MeasureItemSizeParams? prm = null)
        {
            int lineMax = toIndex ?? GetVisibleEnd();
            int lineMin = fromIndex ?? GetVisibleBegin();
            int count = lineMax - lineMin;

            var sizes = new ListControlItem.MeasureItemSizeResult[count];

            MeasureItemEventArgs.EnsureCreated(ref measureItemArgs, dc);

            for (int i = 0, line = lineMin; line < lineMax; i++, line++)
            {
                measureItemArgs.Index = line;
                measureItemArgs.MeasureParams = prm;
                MeasureItemSize(measureItemArgs);

                if(measureItemArgs.MeasureResult is null)
                {
                    sizes[i] = new (measureItemArgs.ItemSize);
                }
                else
                {
                    sizes[i] = measureItemArgs.MeasureResult.Value;
                }

                sizes[i].ItemIndex = line;
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

            bool drawHorzLines = HorzGridLines;
            bool isDark = IsDarkBackground;
            bool drawVertLines = HasColumns && VertGridLines;
            Color horzLineColor = GetEffectiveHorzGridLinesColor(isDark);

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

                if (hRow <= 0)
                    continue;

                var item = GetItem(line);

                if (drawHorzLines)
                {
                    var effectiveRowRect = rectRow;
                    effectiveRowRect.Height -= 1;
                    PaintRow(dc, line, item, effectiveRowRect);
                    var p = effectiveRowRect.BottomLeft;
                    dc.DrawHorzLine(horzLineColor.AsBrush, p, rectRow.Width, 1);
                }
                else
                {
                    PaintRow(dc, line, item, rectRow);
                }

                if (item is not null)
                    itemsLastPainted.Add(item);

                rectRow.Top += hRow;
            }

            if (drawVertLines)
            {
                var vertLineColor = GetEffectiveVertGridLinesColor(isDark);

                r = paintRectangle;

                var x = r.Left;

                var halfOfColumnSeparatorWidth = (ListControlItem.GetColumnSeparatorWidth(this) - 1) / 2;

                for (int i = 0; i < Columns.Count; i++)
                {
                    x += Columns[i].SuggestedWidth + halfOfColumnSeparatorWidth;
                    var p1 = new PointD(x, r.Top);
                    dc.DrawVertLine(vertLineColor.AsBrush, p1, r.Height, 1);
                    x += halfOfColumnSeparatorWidth + 1;
                }
            }
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            e.Graphics.DoInsideClipped(ClientRectangle, DoDefaultPaint);

            void DoDefaultPaint()
            {
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

        /// <summary>
        /// Represents the result of measuring the content size of a virtual list box,
        /// including the overall content size and the sizes of individual rows.
        /// </summary>
        public struct MeasureContentSizeResult
        {
            /// <summary>
            /// Gets or sets the overall content size of the virtual list box.
            /// </summary>
            public SizeD ContentSize { get; set; }

            /// <summary>
            /// Gets content width.
            /// </summary>
            public readonly float Width { get => ContentSize.Width; }

            /// <summary>
            /// Gets content height.
            /// </summary>
            public readonly float Height { get => ContentSize.Height; }

            /// <summary>
            /// Gets or sets an array of <see cref="ListControlItem.MeasureItemSizeResult"/> representing
            /// the sizes of individual rows in the virtual list box.
            /// </summary>
            public ListControlItem.MeasureItemSizeResult[] RowSizes { get; set; }

            /// <summary>
            /// Gets content width of the specified column. Returns -1 if
            /// no measurement is available for the column.
            /// </summary>
            /// <param name="column">The column for which to get the content width.</param>
            /// <returns>The content width of the specified column.</returns>
            public readonly float GetContentWidth(ListControlColumn column)
            {
                float result = -1;

                foreach (ListControlItem.MeasureItemSizeResult rowSize in RowSizes)
                {
                    foreach (ListControlItem.MeasureItemSizeResult cellSize in rowSize.Cells)
                    {
                        if (cellSize.Item is null)
                            continue;

                        var columnId = cellSize.Item.ColumnId;

                        if (columnId == column.UniqueId)
                        {
                            result = Math.Max(result, cellSize.Size.Width);
                        }
                    }
                }

                return result;
            }
        }
    }
}
