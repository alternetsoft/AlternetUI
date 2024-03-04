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
    /// <remarks>
    /// Items in this control have <see cref="ListControlItem"/> type where
    /// <see cref="ListControlItem.Value"/> is <see cref="Color"/> and
    /// <see cref="ListControlItem.Text"/> is label of the color.
    /// </remarks>
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
        /// Gets or sets method that paints color image in the item. Borders around
        /// color image are also painted by this method.
        /// </summary>
        public static Action<Graphics, RectD, Color> PaintColorImage = PaintDefaultColorImage;

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

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool IsEditable
        {
            get => false;
            set
            {
            }
        }

        /// <summary>
        /// Paints color image in the item with the default style. Borders around
        /// color image are also painted by this method.
        /// This is default value of the <see cref="PaintColorImage"/> field.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="rect"><see cref="RectD"/> where drawing is performed.</param>
        /// <param name="color">Color value.</param>
        public static void PaintDefaultColorImage(Graphics canvas, RectD rect, Color color)
        {
            RectD colorRect = DrawingUtils.DrawDoubleBorder(
                canvas,
                rect,
                Color.Empty,
                ComboBox.DefaultImageBorderColor);

            canvas.FillRectangle(color.AsBrush, colorRect);
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

                object? item;

                if (e.IsPaintingControl)
                {
                    item = sender.SelectedItem;
                }
                else
                {
                    item = sender.Items[e.ItemIndex];
                }

                if (item is ListControlItem item1)
                    item = item1.Value;

                var itemColor = (item as Color) ?? Color.White;

                if (!itemColor.IsOk)
                    itemColor = Color.White;

                var (colorRect, itemRect) = sender.GetItemImageRect(e);
                PaintColorImage(e.Graphics, colorRect, itemColor);
                e.ClipRectangle = itemRect;
                sender.DefaultItemPaint(e);
            }
        }
    }
}
