global using Coord = float;
global using CoordI = int;
global using FontScalar = float;

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
        public const Coord Coord72 = 72f;

        /// <summary>
        /// Gets a coordinate which is equal to 96.0.
        /// </summary>
        public const Coord Coord96 = 96f;

        /// <summary>
        /// Gets a coordinate which is equal to 300.0.
        /// </summary>
        public const Coord Coord300 = 300f;

        /// <summary>
        /// Gets a coordinate which is equal to 25.4.
        /// </summary>
        public const Coord Coord25And4 = 25.4f;

        /// <summary>
        /// Gets a coordinate which is equal to 7.5.
        /// </summary>
        public const Coord Coord7And5 = 7.5f;

        /// <summary>
        /// Gets a coordinate which is equal to 4.5.
        /// </summary>
        public const Coord Coord4And5 = 4.5f;

        /// <summary>
        /// Gets a coordinate which is equal to 2.54.
        /// </summary>
        public const Coord Coord2And54 = 2.54f;

        /// <summary>
        /// Gets a coordinate which is equal to 360.0.
        /// </summary>
        public const Coord Coord360 = 360f;
    }
}