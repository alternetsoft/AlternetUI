using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies hatch styles that can be used to fill shapes.
    /// </summary>
    public enum HatchStyle
    {
        /// <summary>Horizontal lines.</summary>
        Horizontal = 0,

        /// <summary>Vertical lines.</summary>
        Vertical = 1,

        /// <summary>Diagonal lines slanting forward (bottom-left to top-right).</summary>
        ForwardDiagonal = 2,

        /// <summary>Diagonal lines slanting backward (top-left to bottom-right).</summary>
        BackwardDiagonal = 3,

        /// <summary>Crosshatch with horizontal and vertical lines.</summary>
        Cross = 4,

        /// <summary>Crosshatch with forward and backward diagonal lines.</summary>
        DiagonalCross = 5,

        /// <summary>5 percent fill pattern.</summary>
        Percent05 = 6,

        /// <summary>10 percent fill pattern.</summary>
        Percent10 = 7,

        /// <summary>20 percent fill pattern.</summary>
        Percent20 = 8,

        /// <summary>25 percent fill pattern.</summary>
        Percent25 = 9,

        /// <summary>30 percent fill pattern.</summary>
        Percent30 = 10,

        /// <summary>40 percent fill pattern.</summary>
        Percent40 = 11,

        /// <summary>50 percent fill pattern.</summary>
        Percent50 = 12,

        /// <summary>60 percent fill pattern.</summary>
        Percent60 = 13,

        /// <summary>70 percent fill pattern.</summary>
        Percent70 = 14,

        /// <summary>75 percent fill pattern.</summary>
        Percent75 = 15,

        /// <summary>80 percent fill pattern.</summary>
        Percent80 = 16,

        /// <summary>90 percent fill pattern.</summary>
        Percent90 = 17,

        /// <summary>Light downward diagonal lines.</summary>
        LightDownwardDiagonal = 18,

        /// <summary>Light upward diagonal lines.</summary>
        LightUpwardDiagonal = 19,

        /// <summary>Dark downward diagonal lines.</summary>
        DarkDownwardDiagonal = 20,

        /// <summary>Dark upward diagonal lines.</summary>
        DarkUpwardDiagonal = 21,

        /// <summary>Wide downward diagonal lines.</summary>
        WideDownwardDiagonal = 22,

        /// <summary>Wide upward diagonal lines.</summary>
        WideUpwardDiagonal = 23,

        /// <summary>Light vertical lines.</summary>
        LightVertical = 24,

        /// <summary>Light horizontal lines.</summary>
        LightHorizontal = 25,

        /// <summary>Narrow vertical lines.</summary>
        NarrowVertical = 26,

        /// <summary>Narrow horizontal lines.</summary>
        NarrowHorizontal = 27,

        /// <summary>Dark vertical lines.</summary>
        DarkVertical = 28,

        /// <summary>Dark horizontal lines.</summary>
        DarkHorizontal = 29,

        /// <summary>Dashed downward diagonal lines.</summary>
        DashedDownwardDiagonal = 30,

        /// <summary>Dashed upward diagonal lines.</summary>
        DashedUpwardDiagonal = 31,

        /// <summary>Dashed horizontal lines.</summary>
        DashedHorizontal = 32,

        /// <summary>Dashed vertical lines.</summary>
        DashedVertical = 33,

        /// <summary>Small confetti pattern.</summary>
        SmallConfetti = 34,

        /// <summary>Large confetti pattern.</summary>
        LargeConfetti = 35,

        /// <summary>Zigzag lines.</summary>
        ZigZag = 36,

        /// <summary>Wave pattern.</summary>
        Wave = 37,

        /// <summary>Diagonal brick pattern.</summary>
        DiagonalBrick = 38,

        /// <summary>Horizontal brick pattern.</summary>
        HorizontalBrick = 39,

        /// <summary>Weave pattern.</summary>
        Weave = 40,

        /// <summary>Plaid pattern.</summary>
        Plaid = 41,

        /// <summary>Divot pattern.</summary>
        Divot = 42,

        /// <summary>Dotted grid pattern.</summary>
        DottedGrid = 43,

        /// <summary>Dotted diamond pattern.</summary>
        DottedDiamond = 44,

        /// <summary>Shingle pattern.</summary>
        Shingle = 45,

        /// <summary>Trellis pattern.</summary>
        Trellis = 46,

        /// <summary>Sphere pattern.</summary>
        Sphere = 47,

        /// <summary>Small grid pattern.</summary>
        SmallGrid = 48,

        /// <summary>Small checkerboard pattern.</summary>
        SmallCheckerBoard = 49,

        /// <summary>Large checkerboard pattern.</summary>
        LargeCheckerBoard = 50,

        /// <summary>Outlined diamond pattern.</summary>
        OutlinedDiamond = 51,

        /// <summary>Solid diamond pattern.</summary>
        SolidDiamond = 52,

        /// <summary>Large grid pattern (alias for Cross).</summary>
        LargeGrid = Cross,

        /// <summary>Minimum hatch style (alias for Horizontal).</summary>
        Min = Horizontal,

        /// <summary>Maximum hatch style (alias for LargeGrid).</summary>
        Max = LargeGrid,
    }
}
