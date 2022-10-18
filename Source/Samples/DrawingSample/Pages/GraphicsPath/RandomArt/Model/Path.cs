using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawingSample.RandomArt
{
    internal sealed class Path
    {
        public List<PathSegment> Segments { get; } = new List<PathSegment>();

        public Pen GetPen()
        {
            var type = Segments.LastOrDefault()?.Type ?? PathSegmentType.Curves;
            return type switch
            {
                PathSegmentType.Lines => Pens.Blue,
                PathSegmentType.Curves => Pens.Red,
                _ => throw new Exception(),
            };
        }

        public Brush GetBrush()
        {
            var type = Segments.LastOrDefault()?.Type ?? PathSegmentType.Curves;
            return type switch
            {
                PathSegmentType.Lines => Brushes.LightBlue,
                PathSegmentType.Curves => Brushes.Pink,
                _ => throw new Exception(),
            };
        }
    }
}