﻿#define CoordIsDouble

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
    /// <summary>
    /// Contains coordinate related properties and methods.
    /// </summary>
    public struct CoordD
    {
#pragma warning disable
        /// <summary>
        /// Gets an empty coordinate which is equal to 0.
        /// </summary>
        public const Coord Empty = 0D;

        /// <summary>
        /// Gets a coordinate which is equal to -1.
        /// </summary>
        public const Coord MinusOne = -1D;

        /// <summary>
        /// Gets a coordinate which is equal to 1.
        /// </summary>
        public const Coord One = 1D;

        /// <summary>
        /// Gets a coordinate which is equal to 1.
        /// </summary>
        public const Coord Two = 2D;
#pragma warning enable
    }
}
