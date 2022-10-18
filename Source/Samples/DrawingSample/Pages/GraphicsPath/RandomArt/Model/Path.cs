using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DrawingSample.RandomArt
{
    internal sealed class Path
    {
        public List<PathSegment> Segments { get; } = new List<PathSegment>();
    }
}