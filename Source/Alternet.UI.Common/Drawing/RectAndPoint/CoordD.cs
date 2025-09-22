global using CoordI = int;

#if CoordIsDouble
global using Coord = double;
global using FontScalar = double;
#else
global using Coord = float;
global using FontScalar = float;
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
        /// <summary>
        /// Gets an empty coordinate which is equal to 0.
        /// </summary>
        public const Coord Empty = 0f;

        /// <summary>
        /// Gets a coordinate which is equal to -1.
        /// </summary>
        public const Coord MinusOne = -1f;

        /// <summary>
        /// Gets a coordinate which is equal to 1.
        /// </summary>
        public const Coord One = 1f;

        /// <summary>
        /// Gets a coordinate which is equal to 2.
        /// </summary>
        public const Coord Two = 2f;

        /// <summary>
        /// Gets a coordinate which is equal to 72.0.
        /// </summary>
#if CoordIsDouble
        public const Coord Coord72 = 72.0;
#else
        public const Coord Coord72 = 72f;
#endif

        /// <summary>
        /// Gets a coordinate which is equal to 96.0.
        /// </summary>
#if CoordIsDouble
        public const Coord Coord96 = 96.0;
#else
        public const Coord Coord96 = 96f;
#endif

        /// <summary>
        /// Gets a coordinate which is equal to 300.0.
        /// </summary>
#if CoordIsDouble
        public const Coord Coord300 = 300.0;
#else
        public const Coord Coord300 = 300f;
#endif

        /// <summary>
        /// Gets a coordinate which is equal to 25.4.
        /// </summary>
#if CoordIsDouble
        public const Coord Coord25And4 = 25.4;
#else
        public const Coord Coord25And4 = 25.4f;
#endif

        /// <summary>
        /// Gets a coordinate which is equal to 7.5.
        /// </summary>
#if CoordIsDouble
        public const Coord Coord7And5 = 7.5;
#else
        public const Coord Coord7And5 = 7.5f;
#endif

        /// <summary>
        /// Gets a coordinate which is equal to 4.5.
        /// </summary>
#if CoordIsDouble
        public const Coord Coord4And5 = 4.5;
#else
        public const Coord Coord4And5 = 4.5f;
#endif

        /// <summary>
        /// Gets a coordinate which is equal to 2.54.
        /// </summary>
#if CoordIsDouble
        public const Coord Coord2And54 = 2.54;
#else
        public const Coord Coord2And54 = 2.54f;
#endif

        /// <summary>
        /// Gets a coordinate which is equal to 360.0.
        /// </summary>
#if CoordIsDouble
        public const Coord Coord360 = 360.0;
#else
        public const Coord Coord360 = 360f;
#endif
    }
}