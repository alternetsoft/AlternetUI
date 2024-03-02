using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ComboBox"/> descendant for editing <see cref="Color"/> values.
    /// </summary>
    public class ColorComboBox : ComboBox
    {
        /// <summary>
        /// Gets or sets default painter for the <see cref="ColorComboBox"/> items.
        /// </summary>
        public static IComboBoxItemPainter Painter = new DefaultItemPainter();

        /// <summary>
        /// Gets or sets method that initializes items in <see cref="ColorComboBox"/>.
        /// </summary>
        public static Action<ColorComboBox>? InitColors = InitDefaultColors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorComboBox"/> class.
        /// </summary>
        public ColorComboBox()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorComboBox"/> class.
        /// </summary>
        /// <param name="defaultColors">Specifies whether to add default color items
        /// to the control.</param>
        public ColorComboBox(bool defaultColors)
        {
            Initialize(defaultColors);
        }

        /// <summary>
        /// Adds color items to the <see cref="ColorComboBox"/>. This is default
        /// implementation of the initialization method. It is assigned to
        /// <see cref="InitColors"/> property by default.
        /// </summary>
        /// <param name="control">Control to initialize.</param>
        public static void InitDefaultColors(ColorComboBox control)
        {
            ListControlUtils.AddColors(control);
        }

        private void Initialize(bool defaultColors = true)
        {
            if (defaultColors)
            {
                if (InitColors is not null)
                    InitColors(this);
            }

            OwnerDrawItem = true;
            ItemPainter = Painter;
        }

        /// <summary>
        /// Default item painter for the <see cref="ColorComboBox"/> items.
        /// </summary>
        public class DefaultItemPainter : IComboBoxItemPainter
        {
            /// <inheritdoc/>
            public virtual double GetHeight(ComboBox sender, int index, double defaultHeight)
            {
                return -1;
            }

            /// <inheritdoc/>
            public virtual double GetWidth(ComboBox sender, int index, double defaultWidth)
            {
                return -1;
            }

            /// <inheritdoc/>
            public virtual void Paint(ComboBox sender, ComboBoxItemPaintEventArgs e)
            {
                if (e.IsPaintingBackground)
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

                var itemColor = (item as Color) ?? Color.White;

                if (!itemColor.IsOk)
                    itemColor = Color.White;

                e.Graphics.FillRectangle(itemColor.AsBrush, colorRect);

                var itemRect = e.Bounds;
                itemRect.X += size + 2;
                itemRect.Width -= size + 2;
                e.Bounds = itemRect;
                sender.DefaultItemPaint(e);
            }
        }
    }
}
