#nullable disable

using Alternet.Drawing;

namespace CustomControlsSample
{
    internal static class MathUtil
    {
        public static double MapRanges(double value, double from1, double to1, double from2, double to2) =>
            (value - from1) / (to1 - from1) * (to2 - from2) + from2;

        public static double GetDistanceSquared(Point p1, Point p2) =>
            (double)(p1.X - p2.X) * (double)(p1.X - p2.X) + (double)(p1.Y - p2.Y) * (double)(p1.Y - p2.Y);

        public static bool IsPointInCircle(Point p, Point center, double radius)
        {
            return GetDistanceSquared(p, center) <= radius * radius;
        }
    }
}