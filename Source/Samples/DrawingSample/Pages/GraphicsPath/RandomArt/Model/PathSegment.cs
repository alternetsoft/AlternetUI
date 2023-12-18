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
        public PathSegment(PointD start)
        {
            Start = start;
        }

        public abstract PathSegmentType Type { get; }

        public PointD Start { get; }
        public abstract PointD End { get; }

        public abstract void TryAddSegmentParts(PointD tipPoint, ToolSettings toolSettings);

        public abstract void Render(GraphicsPath path);

        public double Length => Vector2.Distance(Utils.GetVector(Start), Utils.GetVector(End));

        public bool IsClosed { get; private set; }

        public void CloseFigure()
        {
            IsClosed = true;
        }
    }
}