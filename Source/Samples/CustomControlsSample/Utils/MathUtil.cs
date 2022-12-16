#nullable disable

namespace CustomControlsSample
{
    internal static class MathUtil
    {
        public static double MapRanges(double value, double from1, double to1, double from2, double to2) =>
            (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}