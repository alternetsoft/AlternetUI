using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DrawingSample.RandomArt
{
    internal abstract class PathSegment
    {
        public PathSegment(Point start)
        {
            Start = start;
        }

        public abstract PathSegmentType Type { get; }

        public Point Start { get; }
        public abstract Point End { get; }

        public abstract void TryAddSegmentParts(Point tipPoint, ToolSettings toolSettings);

        public abstract void Render(GraphicsPath path);

        public double Length => Vector2.Distance(Utils.GetVector(Start), Utils.GetVector(End));
    }
}