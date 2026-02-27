using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies standard paper sizes used for printing and document formatting.
    /// </summary>
    /// <remarks>This enumeration includes a range of internationally recognized paper sizes, such as
    /// A-series, B-series, C-series, and common North American formats like Letter and Legal. These values can be
    /// used to select or identify paper sizes in printing operations and document layout scenarios.</remarks>
    public enum KnownPaperKind
    {
        /// <summary>ISO A0 paper size (841 x 1189 mm).</summary>
        A0,

        /// <summary>ISO A1 paper size (594 x 841 mm).</summary>
        A1,

        /// <summary>ISO A2 paper size (420 x 594 mm).</summary>
        A2,

        /// <summary>ISO A3 paper size (297 x 420 mm).</summary>
        A3,

        /// <summary>ISO A4 paper size (210 x 297 mm).</summary>
        A4,

        /// <summary>ISO A5 paper size (148 x 210 mm).</summary>
        A5,

        /// <summary>ISO A6 paper size (105 x 148 mm).</summary>
        A6,

        /// <summary>ISO A7 paper size (74 x 105 mm).</summary>
        A7,

        /// <summary>ISO A8 paper size (52 x 74 mm).</summary>
        A8,

        /// <summary>ISO A9 paper size (37 x 52 mm).</summary>
        A9,

        /// <summary>ISO A10 paper size (26 x 37 mm).</summary>
        A10,

        /// <summary>Four times A0 paper size (1682 x 2378 mm).</summary>
        A0x4,

        /// <summary>Two times A0 paper size (1189 x 1682 mm).</summary>
        A0x2,

        /// <summary>ISO B0 paper size (1000 x 1414 mm).</summary>
        B0,

        /// <summary>ISO B1 paper size (707 x 1000 mm).</summary>
        B1,

        /// <summary>ISO B2 paper size (500 x 707 mm).</summary>
        B2,

        /// <summary>ISO B3 paper size (353 x 500 mm).</summary>
        B3,

        /// <summary>ISO B4 paper size (250 x 353 mm).</summary>
        B4,

        /// <summary>ISO B5 paper size (176 x 250 mm).</summary>
        B5,

        /// <summary>ISO B6 paper size (125 x 176 mm).</summary>
        B6,

        /// <summary>ISO B7 paper size (88 x 125 mm).</summary>
        B7,

        /// <summary>ISO B8 paper size (62 x 88 mm).</summary>
        B8,

        /// <summary>ISO B9 paper size (44 x 62 mm).</summary>
        B9,

        /// <summary>ISO B10 paper size (31 x 44 mm).</summary>
        B10,

        /// <summary>ISO C0 envelope size (917 x 1297 mm).</summary>
        C0,

        /// <summary>ISO C1 envelope size (648 x 917 mm).</summary>
        C1,

        /// <summary>ISO C2 envelope size (458 x 648 mm).</summary>
        C2,

        /// <summary>ISO C3 envelope size (324 x 458 mm).</summary>
        C3,

        /// <summary>ISO C4 envelope size (229 x 324 mm).</summary>
        C4,

        /// <summary>ISO C5 envelope size (162 x 229 mm).</summary>
        C5,

        /// <summary>ISO C6 envelope size (114 x 162 mm).</summary>
        C6,

        /// <summary>ISO C7 envelope size (81 x 114 mm).</summary>
        C7,

        /// <summary>ISO C8 envelope size (57 x 81 mm).</summary>
        C8,

        /// <summary>ISO C9 envelope size (40 x 57 mm).</summary>
        C9,

        /// <summary>ISO C10 envelope size (28 x 40 mm).</summary>
        C10,

        /// <summary>Half Letter paper size (5.5 x 8.5 inches).</summary>
        HalfLetter,

        /// <summary>Government Letter paper size (8 x 10.5 inches).</summary>
        GovernmentLetter,

        /// <summary>US Letter paper size (8.5 x 11 inches).</summary>
        Letter,

        /// <summary>Junior Legal paper size (8 x 5 inches).</summary>
        JuniorLegal,

        /// <summary>Government Legal paper size (8.5 x 13 inches).</summary>
        GovernmentLegal,

        /// <summary>US Legal paper size (8.5 x 14 inches).</summary>
        Legal,

        /// <summary>Tabloid paper size (11 x 17 inches).</summary>
        Tabloid,
    }
}
