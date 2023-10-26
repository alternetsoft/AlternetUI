using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Math"/> related static methods.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Calculates distance between two points.
        /// </summary>
        /// <param name="x1">X coordinate of the first point.</param>
        /// <param name="y1">Y coordinate of the first point.</param>
        /// <param name="x2">X coordinate of the second point.</param>
        /// <param name="y2">Y coordinate of the second point.</param>
        /// <returns><see cref="double"/> value with distance between point (x1, y1) and
        /// point (x2, y2).</returns>
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        /// <summary>
        /// Calculates distance between two points.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns><see cref="double"/> value with distance between point <paramref name="p1"/>
        /// and point <paramref name="p2"/>.</returns>
        public static double GetDistance(Point p1, Point p2)
        {
            return GetDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        /// <summary>
        /// Compares distance between two points and specified value.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <param name="value">Value to compare distance with.</param>
        /// <returns><c>true</c> if distance between two points is less
        /// than <paramref name="value"/>; <c>false</c> otherwise.</returns>
        public static bool DistanceIsLess(Point p1, Point p2, double value)
        {
            return (Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2)) < Math.Pow(value, 2);
        }

        /// <summary>
        /// Returns the larger of the specified numbers.
        /// </summary>
        /// <param name="values">Array of <see cref="double"/> numbers.</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="values"/> is
        /// an empty array.</exception>
        public static double Max(params double[] values)
        {
            var length = values.Length;

            if (length < 2)
            {
                if(length == 0)
                    throw new ArgumentOutOfRangeException(nameof(values));
                return values[0];
            }

            var result = values[0];

            for(int i = 1; i < values.Length; i++)
            {
                var item = values[i];
                if (item > result)
                    result = item;
            }

            return result;
        }

        /// <summary>
        /// Returns the larger of the specified numbers.
        /// </summary>
        /// <param name="values">Array of <see cref="int"/> numbers.</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="values"/> is
        /// an empty array.</exception>
        public static int Max(params int[] values)
        {
            var length = values.Length;

            if (length < 2)
            {
                if (length == 0)
                    throw new ArgumentOutOfRangeException(nameof(values));
                return values[0];
            }

            var result = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                var item = values[i];
                if (item > result)
                    result = item;
            }

            return result;
        }

        internal static double ApplyMinMax(double value, double? min = null, double? max = null)
        {
            if (min is not null && value < min)
                value = min.Value;
            if (max is not null && value > max)
                value = max.Value;
            return value;
        }
    }
}
