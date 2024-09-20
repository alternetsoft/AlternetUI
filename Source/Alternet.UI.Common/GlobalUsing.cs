#define CoordIsDouble

#if CoordIsDouble
global using Coord = double;
global using CoordUtils = Alternet.UI.DoubleUtils;
#else
global using Coord = single;
global using CoordUtils = Alternet.UI.SingleUtils;
#endif

global using FontMeasure = Coord;
global using FontSize = Coord;

namespace Alternet.UI.Internal
{
}

#pragma warning disable
namespace Alternet.UI.Threading
{
}
#pragma warning restore
