using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class VirtualListBox
    {
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
                PaintEmptyText();
            }
            else
            {
                InternalPaint();
            }

            DrawInterior(dc);

            void PaintEmptyText()
            {
                var s = EmptyText;

                if (s is null || s.Length == 0)
                    return;

                var size = dc.MeasureText(s, RealFont);

                var alignedRect = AlignUtils.AlignRectInRect(
                    (PointD.Empty, size),
                    r,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center,
                    false);
                var location = alignedRect.Location;
                dc.DrawText(s, location.ClampToZero(), RealFont, Color.Gray, Color.Empty);
            }

            void InternalPaint()
            {
                dc.PushAndTranslate(-scrollOffsetX, 0);
                try
                {
                    PaintRows();
                }
                finally
                {
                    dc.PopTransform();
                }
            }

            void PaintRows()
            {
                r.Width += scrollOffsetX;

                var rectRow = r;

                int lineMax = GetVisibleEnd();

                MeasureItemEventArgs measureItemArgs = new(dc, 0);
                DrawItemEventArgs drawItemArgs = new(dc);

                itemsLastPainted.Clear();

                for (int line = GetVisibleBegin(); line < lineMax; line++)
                {
                    measureItemArgs.Index = line;
                    MeasureItemSize(measureItemArgs);

                    var hRow = measureItemArgs.ItemHeight;

                    rectRow.Height = hRow;

                    if (!IsPartialRowVisible)
                    {
                        if (!r.Contains(rectRow))
                            continue;
                    }

                    var isCurrentItem = IsCurrent(line);
                    var isSelectedItem = IsSelected(line);
                    var item = SafeItem(line);

                    if (item is not null)
                        itemsLastPainted.Add(item);

                    if (drawMode != DrawMode.Normal)
                    {
                        drawItemArgs.Bounds = rectRow;
                        drawItemArgs.Index = line;

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
                            itemPaintArgs = new(this, dc, rectRow, line);
                        }
                        else
                        {
                            itemPaintArgs.Graphics = dc;
                            itemPaintArgs.ClientRectangle = rectRow;
                            itemPaintArgs.ItemIndex = line;
                        }

                        itemPaintArgs.LabelMetrics = new();
                        itemPaintArgs.IsCurrent = isCurrentItem;
                        itemPaintArgs.IsSelected = isSelectedItem;
                        itemPaintArgs.Visible = true;

                        DrawItemBackground(itemPaintArgs);
                        DrawItemForeground(itemPaintArgs);
                    }

                    rectRow.Top += hRow;
                }
            }
        }
    }
}
