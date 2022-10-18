using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DrawingSample.RandomArt
{
    internal static class SegmentFactory
    {
        public static PathSegment CreateSegment(PathSegmentType type, Point start)
        {
            return new LinesPathSegment(start);
        }
    }
}