using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines the properties of a pen used for drawing lines and curves.
    /// </summary>
    public struct PenInfo
    {
        /// <inheritdoc cref="Pen.Color"/>.
        public Color Color;

        /// <inheritdoc cref="Pen.DashStyle"/>.
        public DashStyle DashStyle;
        
        /// <inheritdoc cref="Pen.LineCap"/>.
        public LineCap LineCap;
        
        /// <inheritdoc cref="Pen.LineJoin"/>.
        public LineJoin LineJoin;
        
        /// <inheritdoc cref="Pen.Width"/>.
        public Coord Width;
        
        /// <inheritdoc cref="Pen.DashPattern"/>.
        public float[] DashPattern;

        /// <summary>
        /// Operator to compare two <see cref="PenInfo"/> instances for equality.
        /// </summary>
        /// <param name="left">The first <see cref="PenInfo"/> instance to compare.</param>
        /// <param name="right">The second <see cref="PenInfo"/> instance to compare.</param>
        /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Alternet.Drawing.PenInfo left, Alternet.Drawing.PenInfo right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Operator to compare two <see cref="PenInfo"/> instances for inequality.
        /// </summary>
        /// <param name="left">The first <see cref="PenInfo"/> instance to compare.</param>
        /// <param name="right">The second <see cref="PenInfo"/> instance to compare.</param>
        /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PenInfo left, PenInfo right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object? obj)
        {
            if (obj is not PenInfo other)
                return false;
            return Color == other.Color
                && DashStyle == other.DashStyle
                && LineCap == other.LineCap
                && LineJoin == other.LineJoin
                && Width == other.Width
                && ((DashPattern == null && other.DashPattern == null)
                || (DashPattern != null && other.DashPattern != null && DashPattern.SequenceEqual(other.DashPattern)));
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            var hc = new HashCode();

            hc.Add(Color.GetHashCode());
            hc.Add(DashStyle);
            hc.Add(LineCap);
            hc.Add(LineJoin);
            hc.Add(Width);

            if (DashPattern != null)
            {
                foreach (var d in DashPattern)
                    hc.Add(d);
            }

            return hc.ToHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"Pen ({Color}, {Width}, {DashStyle}");

            if (LineCap != LineCap.Flat)
                sb.Append($", LineCap={LineCap}");

            if (LineJoin != LineJoin.Miter)
                sb.Append($", LineJoin={LineJoin}");

            if (DashPattern != null && DashPattern.Length > 0)
                sb.Append($", DashPattern=[{string.Join(",", DashPattern)}]");

            sb.Append(")");

            return sb.ToString();
        }
    }
}