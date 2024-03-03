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
            RectD colorRect;

            if (ColorImageParams.SmartBorder)
            {
                Color borderColor;
                if (color.IsDark())
                    borderColor = ColorImageParams.DarkBorderColor;
                else
                    borderColor = ColorImageParams.LightBorderColor;
                colorRect = DrawingUtils.DrawDoubleBorder(
                    canvas,
                    rect,
                    Color.Empty,
                    borderColor);
            }
            else
            {
                colorRect = DrawingUtils.DrawDoubleBorder(
                    canvas,
                    rect,
                    ColorImageParams.InnerBorderColor,
                    ColorImageParams.OuterBorderColor);
            }

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
        /// Defines style of the color image painting. If <see cref="SmartBorder"/>
        /// is <c>true</c>, a single border is painted using <see cref="DarkBorderColor"/>
        /// or <see cref="LightBorderColor"/> depending on the <see cref="Color.IsDark"/>
        /// of the color image. Otherwise double border is painted using
        /// <see cref="InnerBorderColor"/> and <see cref="OuterBorderColor"/>.
        /// </summary>
        public static class ColorImageParams
        {
            /// <summary>
            /// Gets or sets inner border of the color image.
            /// </summary>
            /// <remarks>
            /// Each color image is painted with inner and outer borders. If border color
            /// is <see cref="Color.Empty"/> it is not painted.
            /// </remarks>
            public static Color InnerBorderColor = SystemColors.GrayText;

            /// <summary>
            /// Gets or sets outer border of the color image.
            /// </summary>
            /// <remarks>
            /// Each color image is painted with inner and outer borders. If border color
            /// is <see cref="Color.Empty"/> it is not painted.
            /// </remarks>
            public static Color OuterBorderColor = Color.White;

            /// <summary>
            /// Gets or sets vertical offset of the color image.
            /// </summary>
            public static double VerticalOffset = 3;

            /// <summary>
            /// Gets or sets border of the color image when it is dark.
            /// </summary>
            public static Color DarkBorderColor = SystemColors.GrayText;

            /// <summary>
            /// Gets or sets border of the color image when it is light.
            /// </summary>
            public static Color LightBorderColor = SystemColors.GrayText;

            /// <summary>
            /// Gets or sets whether to paint single smart border or double border.
            /// </summary>
            public static bool SmartBorder = true;
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

                var offset = ColorImageParams.VerticalOffset;
                if (e.IsPaintingControl)
                    offset++;

                var size = e.Bounds.Height - (sender.TextMargin.Y * 2) - (offset * 2);
                var colorRect = new RectD(
                    e.Bounds.X + sender.TextMargin.X,
                    e.Bounds.Y + sender.TextMargin.Y + offset,
                    size,
                    size);

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

                PaintColorImage(e.Graphics, colorRect, itemColor);

                var itemRect = e.Bounds;
                itemRect.X += size + 2;
                itemRect.Width -= size + 2;
                e.Bounds = itemRect;
                sender.DefaultItemPaint(e);
            }
        }
    }
}
