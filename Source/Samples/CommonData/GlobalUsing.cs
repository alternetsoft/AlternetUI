﻿#define CoordIsDouble

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

namespace Alternet.UI.Internal
{
}

#pragma warning disable
namespace Alternet.UI.Threading
{
}
#pragma warning restore
