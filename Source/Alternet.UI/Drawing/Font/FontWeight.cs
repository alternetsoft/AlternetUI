using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Refers to the density of a typeface, in terms of the
    /// lightness or heaviness of the strokes.
    /// </summary>
    [Flags]
    public enum FontWeight
    {
        /// <summary>
        /// Font weight is not specified or is invalid.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Specifies a "Thin" font weight (100).
        /// </summary>
        Thin = 100,

        /// <summary>
        /// Specifies an "Extra-light" font weight (200).
        /// </summary>
        ExtraLight = 200,

        /// <summary>
        /// Specifies an "Ultra-light" font weight (200).
        /// </summary>
        UltraLight = ExtraLight,

        /// <summary>
        /// Specifies a "Light" font weight (300).
        /// </summary>
        Light = 300,

        /// <summary>
        /// Specifies a "Normal" font weight (400).
        /// </summary>
        Normal = 400,

        /// <summary>
        /// Specifies a "Regular" font weight (400).
        /// </summary>
        Regular = Normal,

        /// <summary>
        /// Specifies a "Medium" font weight.
        /// </summary>
        Medium = 500,

        /// <summary>
        /// Specifies a "Semi-bold" font weight (600).
        /// </summary>
        SemiBold = 600,

        /// <summary>
        /// Specifies a "Demi-bold" font weight (600).
        /// </summary>
        DemiBold = SemiBold,

        /// <summary>
        /// Specifies a "Bold" font weight (700).
        /// </summary>
        Bold = 700,

        /// <summary>
        /// Specifies an "Extra-bold" font weight (800).
        /// </summary>
        ExtraBold = 800,

        /// <summary>
        /// Specifies an "Ultra-bold" font weight (800).
        /// </summary>
        UltraBold = ExtraBold,

        /// <summary>
        /// Specifies a "Heavy" font weight (900).
        /// </summary>
        Heavy = 900,

        /// <summary>
        /// Specifies a "Black" font weight (900).
        /// </summary>
        Black = Heavy,

        /// <summary>
        /// Specifies an "Extra-black" font weight (950).
        /// </summary>
        ExtraBlack = 950,

        /// <summary>
        /// Specifies an "Ultra-black" font weight (950).
        /// </summary>
        UltraBlack = ExtraBlack,

        /// <summary>
        /// Specifies an "Extra-heavy" font weight (100).
        /// </summary>
        ExtraHeavy = 1000,

        /// <summary>
        /// Maximal possible font weight.
        /// </summary>
        Max = ExtraHeavy,
    }
}