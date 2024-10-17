#define CoordIsDouble

global using CoordI = int;

#if CoordIsDouble
global using Coord = double;
global using CoordUtils = Alternet.UI.DoubleUtils;
global using FontMeasure = double;
global using FontSize = double;
#else
global using Coord = single;
global using CoordUtils = Alternet.UI.SingleUtils;
global using FontMeasure = single;
global using FontSize = single;
#endif

#pragma warning disable
using System;
using System.Collections.Generic;
using System.Text;
#pragma warning restore

namespace Alternet.Drawing
{
    internal struct CoordD
    {
    }
}
