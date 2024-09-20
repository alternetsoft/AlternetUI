using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines delegate for <see cref="GraphicsUnitConverter.Convert"/> handler.
    /// </summary>
    /// <param name="from"><see cref="GraphicsUnit"/> of the value which is converted.</param>
    /// <param name="to"><see cref="GraphicsUnit"/> of the result.</param>
    /// <param name="dpi">Number of dots per inch.</param>
    /// <param name="nSrc">Value to convert.</param>
    /// <param name="type">Type of the graphics.</param>
    /// <returns></returns>
    public delegate Coord ConvertGraphicsUnit(
            GraphicsUnit from,
            GraphicsUnit to,
            Coord dpi,
            Coord nSrc,
            GraphicsUnitConverter.GraphicsType type = GraphicsUnitConverter.GraphicsType.Undefined);

    /// <summary>
    /// Contains conversion methods related to <see cref="GraphicsUnit"/>.
    /// </summary>
    public static class GraphicsUnitConverter
    {
        /// <summary>
        /// Gets or set convertion event handler method.
        /// </summary>
        public static ConvertGraphicsUnit Convert = DefaultConvert;

        /// <summary>
        /// Type of the graphics.
        /// </summary>
        public enum GraphicsType
        {
            /// <summary>
            /// Undefined.
            /// </summary>
            Undefined,

            /// <summary>
            /// X11
            /// </summary>
            X11Drawable,

            /// <summary>
            /// Memory bitmap.
            /// </summary>
            MemoryBitmap,

            /// <summary>
            /// MacOs.
            /// </summary>
            OSXDrawable,

            /// <summary>
            /// Printer.
            /// </summary>
            PostScript,
        }

        /// <summary>
        /// Converts <see cref="RectD"/> value to other <see cref="GraphicsUnit"/>.
        /// </summary>
        /// <param name="from"><see cref="GraphicsUnit"/> of the value which is converted.</param>
        /// <param name="to"><see cref="GraphicsUnit"/> of the result.</param>
        /// <param name="dpi">Number of dots per inch.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type of the graphics.</param>
        /// <returns></returns>
        public static RectD ConvertRect(
            GraphicsUnit from,
            GraphicsUnit to,
            SizeD dpi,
            RectD value,
            GraphicsType type = GraphicsType.Undefined)
        {
            if (from == to)
                return value;
            var x = Convert(from, to, dpi.Width, value.X, type);
            var y = Convert(from, to, dpi.Height, value.Y, type);
            var width = Convert(from, to, dpi.Width, value.Width, type);
            var height = Convert(from, to, dpi.Height, value.Height, type);
            return new(x, y, width, height);
        }

        /// <summary>
        /// Converts <see cref="PointD"/> value to other <see cref="GraphicsUnit"/>.
        /// </summary>
        /// <param name="from"><see cref="GraphicsUnit"/> of the value which is converted.</param>
        /// <param name="to"><see cref="GraphicsUnit"/> of the result.</param>
        /// <param name="dpi">Number of dots per inch.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type of the graphics.</param>
        /// <returns></returns>
        public static PointD ConvertPoint(
            GraphicsUnit from,
            GraphicsUnit to,
            SizeD dpi,
            PointD value,
            GraphicsType type = GraphicsType.Undefined)
        {
            if (from == to)
                return value;
            var x = Convert(from, to, dpi.Width, value.X, type);
            var y = Convert(from, to, dpi.Height, value.Y, type);
            return new(x, y);
        }

        /// <summary>
        /// Converts <see cref="SizeD"/> value to other <see cref="GraphicsUnit"/>.
        /// </summary>
        /// <param name="from"><see cref="GraphicsUnit"/> of the value which is converted.</param>
        /// <param name="to"><see cref="GraphicsUnit"/> of the result.</param>
        /// <param name="dpi">Number of dots per inch.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type of the graphics.</param>
        /// <returns></returns>
        public static SizeD ConvertSize(
            GraphicsUnit from,
            GraphicsUnit to,
            SizeD dpi,
            SizeD value,
            GraphicsType type = GraphicsType.Undefined)
        {
            if (from == to)
                return value;
            var width = Convert(from, to, dpi.Width, value.Width, type);
            var height = Convert(from, to, dpi.Height, value.Height, type);
            return new(width, height);
        }

        /// <summary>
        /// Default convertion method.
        /// </summary>
        /// <param name="from"><see cref="GraphicsUnit"/> of the value which is converted.</param>
        /// <param name="to"><see cref="GraphicsUnit"/> of the result.</param>
        /// <param name="dpi">Number of dots per inch.</param>
        /// <param name="nSrc">Value to convert.</param>
        /// <param name="type">Type of the graphics.</param>
        /// <returns></returns>
        public static Coord DefaultConvert(
            GraphicsUnit from,
            GraphicsUnit to,
            Coord dpi,
            Coord nSrc,
            GraphicsType type = GraphicsType.Undefined)
        {
            Coord inchs;

            if (from == to)
                return nSrc;

            switch (from)
            {
                case GraphicsUnit.Document: // Each unit is 1/300 inch.
                    inchs = nSrc / 300.0;
                    break;
                case GraphicsUnit.Inch:
                    inchs = nSrc;
                    break;
                case GraphicsUnit.Millimeter:
                    inchs = nSrc / 25.4;
                    break;
                case GraphicsUnit.Display:
                    if (type == GraphicsType.PostScript)
                    { /* Uses 1/100th on printers */
                        inchs = nSrc / 100;
                    }
                    else
                    { /* Pixel for video display */
                        inchs = nSrc / dpi;
                    }

                    break;
                case GraphicsUnit.Pixel:
                case GraphicsUnit.World:
                    inchs = nSrc / dpi;
                    break;
                case GraphicsUnit.Point:
                    inchs = nSrc / 72.0;
                    break;
                case GraphicsUnit.Dip:
                    inchs = nSrc / 96.0;
                    break;
                default:
                    return nSrc;
            }

            switch (to)
            {
                case GraphicsUnit.Document:
                    return inchs * 300.0;
                case GraphicsUnit.Inch:
                    return inchs;
                case GraphicsUnit.Millimeter:
                    return inchs * 25.4;
                case GraphicsUnit.Display:
                    if (type == GraphicsType.PostScript)
                    { /* Uses 1/100th on printers */
                        return inchs * 100;
                    }
                    else
                    { /* Pixel for video display */
                        return inchs * dpi;
                    }

                case GraphicsUnit.Pixel:
                case GraphicsUnit.World:
                    return inchs * dpi;
                case GraphicsUnit.Point:
                    return inchs * 72.0;
                case GraphicsUnit.Dip:
                    return inchs * 96.0;
                default:
                    return nSrc;
            }
        }
    }
}
