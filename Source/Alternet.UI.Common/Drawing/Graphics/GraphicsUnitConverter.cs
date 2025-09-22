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
        /// Gets or set conversion event handler method.
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
        /// Default conversion method.
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
            Coord inches;

            if (from == to)
                return nSrc;

            switch (from)
            {
                case GraphicsUnit.Document: // Each unit is 1/300 inch.
                    inches = nSrc / CoordD.Coord300;
                    break;
                case GraphicsUnit.Inch:
                    inches = nSrc;
                    break;
                case GraphicsUnit.Millimeter:
                    inches = nSrc / CoordD.Coord25And4;
                    break;
                case GraphicsUnit.Display:
                    if (type == GraphicsType.PostScript)
                    { /* Uses 1/100th on printers */
                        inches = nSrc / 100;
                    }
                    else
                    { /* Pixel for video display */
                        inches = nSrc / dpi;
                    }

                    break;
                case GraphicsUnit.Pixel:
                case GraphicsUnit.World:
                    inches = nSrc / dpi;
                    break;
                case GraphicsUnit.Point:
                    inches = nSrc / CoordD.Coord72;
                    break;
                case GraphicsUnit.Dip:
                    inches = nSrc / CoordD.Coord96;
                    break;
                default:
                    return nSrc;
            }

            switch (to)
            {
                case GraphicsUnit.Document:
                    return inches * CoordD.Coord300;
                case GraphicsUnit.Inch:
                    return inches;
                case GraphicsUnit.Millimeter:
                    return inches * CoordD.Coord25And4;
                case GraphicsUnit.Display:
                    if (type == GraphicsType.PostScript)
                    { /* Uses 1/100th on printers */
                        return inches * 100;
                    }
                    else
                    { /* Pixel for video display */
                        return inches * dpi;
                    }

                case GraphicsUnit.Pixel:
                case GraphicsUnit.World:
                    return inches * dpi;
                case GraphicsUnit.Point:
                    return inches * CoordD.Coord72;
                case GraphicsUnit.Dip:
                    return inches * CoordD.Coord96;
                default:
                    return nSrc;
            }
        }
    }
}
