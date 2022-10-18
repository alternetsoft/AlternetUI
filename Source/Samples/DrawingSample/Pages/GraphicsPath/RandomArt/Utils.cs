using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DrawingSample.RandomArt
{
    internal static class Utils
    {
        public static Vector2 GetVector(Point point) => new Vector2((float)point.X, (float)point.Y);

        public static bool IsDistanceGreaterOrEqual(Point p1, Point p2, double distanceThreshold)
        {
            return Vector2.DistanceSquared(GetVector(p1), GetVector(p2)) >= distanceThreshold * distanceThreshold;
        }

        public static Point GetJitteredPointAlongLineSegment(
            Point startPoint,
            Point endPoint,
            double distance,
            double jitter,
            Random random)
        {
            var start = GetVector(startPoint);
            var end = GetVector(endPoint);

            var direction = Vector2.Normalize((end - start));
            var pointOnLine = start + direction * (float)distance;
            var perpendicular = Vector2.Normalize(new Vector2(direction.Y, -direction.X));

            var result = pointOnLine + (perpendicular * random.Next(-(int)jitter, (int)jitter));
            return new Point(result);
        }
    }
}