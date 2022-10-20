using Alternet.UI;
using Alternet.Drawing;
using System;

namespace DrawingSample
{
    internal static class MathUtils
    {
        public const double DegToRad = Math.PI / 180;

        public static Point GetPointOnCircle(Point center, double radius, double angle)
        {
            return new Point(center.X + radius * Math.Cos(angle * DegToRad), center.Y + radius * Math.Sin(angle * DegToRad));
        }

        public static double MapRanges(double value, double fromLow, double fromHigh, double toLow, double toHigh) =>
            ((value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow)) + toLow;

    }
}