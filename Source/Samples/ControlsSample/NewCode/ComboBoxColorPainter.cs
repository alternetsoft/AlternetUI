using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class ComboBoxColorPainter : IComboBoxItemPainter
    {
        public double GetHeight(ComboBox sender, int index, double defaultHeight)
        {
            return -1;
        }

        public double GetWidth(ComboBox sender, int index, double defaultWidth)
        {
            return -1;
        }

        public void Paint(ComboBox sender, ComboBoxItemPaintEventArgs e)
        {
            if(e.IsPaintingBackground)
            {
                e.DefaultPaint();
                return;
            }

            const double offset = 2;

            var size = e.Bounds.Height - (sender.TextMargin.Y * 2) - (offset * 2);
            var colorRect = new RectD(
                e.Bounds.X + sender.TextMargin.X,
                e.Bounds.Y + sender.TextMargin.Y + offset,
                size,
                size);
            DrawingUtils.FillRectangleBorder(e.Graphics, Color.White, colorRect, 1);
            colorRect.Inflate(-1);
            DrawingUtils.FillRectangleBorder(e.Graphics, SystemColors.GrayText, colorRect, 1);
            colorRect.Inflate(-1);

            object? item;

            if (e.IsPaintingControl)
                item = sender.SelectedItem;
            else
                item = sender.Items[e.ItemIndex];

            if (item is ListControlItem item1)
                item = item1.Value;

            var itemColor = (item as Color) ?? Color.Empty;

            e.Graphics.FillRectangle(itemColor.AsBrush, colorRect);

            var itemRect = e.Bounds;
            itemRect.X += size + 2;
            itemRect.Width -= size + 2;
            e.Bounds = itemRect;
            sender.DefaultItemPaint(e);
        }
    }
}
