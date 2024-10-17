﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Defines paper sizes.
    /// </summary>
    public class PaperSizes
    {
        private static PaperSizes? sizeMillimeters;
        private static PaperSizes? sizeInches;

        /// <summary>
        /// Gets or sets paper sizes (millimeters).
        /// </summary>
        public static PaperSizes SizeMillimeters
        {
            get
            {
                sizeMillimeters ??= GetSizeMillimeters();
                return sizeMillimeters;
            }

            set
            {
                sizeMillimeters = value;
            }
        }

        /// <summary>
        /// Gets A6 paper size in device-independent units.
        /// </summary>
        public static SizeD A6InDips => PaperSizes.SizeInches.A6.InchesToDips();

        /// <summary>
        /// Gets A5 paper size in device-independent units.
        /// </summary>
        public static SizeD A5InDips => PaperSizes.SizeInches.A5.InchesToDips();

        /// <summary>
        /// Gets A4 paper size in device-independent units.
        /// </summary>
        public static SizeD A4InDips => PaperSizes.SizeInches.A4.InchesToDips();

        /// <summary>
        /// Gets or sets paper sizes (inches).
        /// </summary>
        public static PaperSizes SizeInches
        {
            get
            {
                sizeInches ??= GetSizeInches();
                return sizeInches;
            }

            set
            {
                sizeInches = value;
            }
        }

        /// <summary>
        /// Gets or sets paper size for the "4A0" format.
        /// </summary>
        public SizeD A0x4 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "2A0" format.
        /// </summary>
        public SizeD A0x2 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A0" format.
        /// </summary>
        public SizeD A0 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A1" format.
        /// </summary>
        public SizeD A1 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A2" format.
        /// </summary>
        public SizeD A2 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A3" format.
        /// </summary>
        public SizeD A3 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A4" format.
        /// </summary>
        public SizeD A4 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A5" format.
        /// </summary>
        public SizeD A5 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A6" format.
        /// </summary>
        public SizeD A6 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A7" format.
        /// </summary>
        public SizeD A7 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A8" format.
        /// </summary>
        public SizeD A8 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A9" format.
        /// </summary>
        public SizeD A9 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "A10" format.
        /// </summary>
        public SizeD A10 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "Half Letter" format.
        /// </summary>
        public SizeD HalfLetter { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "Government Letter" format.
        /// </summary>
        public SizeD GovernmentLetter { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "Letter" format.
        /// </summary>
        public SizeD Letter { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "Junior Legal" format.
        /// </summary>
        public SizeD JuniorLegal { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "Government Legal" format.
        /// </summary>
        public SizeD GovernmentLegal { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "Legal" format.
        /// </summary>
        public SizeD Legal { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "Tabloid" format.
        /// </summary>
        public SizeD Tabloid { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B10" format.
        /// </summary>
        public SizeD B10 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B9" format.
        /// </summary>
        public SizeD B9 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B8" format.
        /// </summary>
        public SizeD B8 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B7" format.
        /// </summary>
        public SizeD B7 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B6" format.
        /// </summary>
        public SizeD B6 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B5" format.
        /// </summary>
        public SizeD B5 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B4" format.
        /// </summary>
        public SizeD B4 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B3" format.
        /// </summary>
        public SizeD B3 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B2" format.
        /// </summary>
        public SizeD B2 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B1" format.
        /// </summary>
        public SizeD B1 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "B0" format.
        /// </summary>
        public SizeD B0 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C10" format.
        /// </summary>
        public SizeD C10 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C9" format.
        /// </summary>
        public SizeD C9 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C8" format.
        /// </summary>
        public SizeD C8 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C7" format.
        /// </summary>
        public SizeD C7 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C6" format.
        /// </summary>
        public SizeD C6 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C5" format.
        /// </summary>
        public SizeD C5 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C4" format.
        /// </summary>
        public SizeD C4 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C3" format.
        /// </summary>
        public SizeD C3 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C2" format.
        /// </summary>
        public SizeD C2 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C1" format.
        /// </summary>
        public SizeD C1 { get; set; }

        /// <summary>
        /// Gets or sets paper size for the "C0" format.
        /// </summary>
        public SizeD C0 { get; set; }

        private static PaperSizes GetSizeInches()
        {
            PaperSizes result = new()
            {
                A0x4 = (66.2, 93.6),
                A0x2 = (46.8, 66.2),
                A0 = (33.1, 46.8),
                A1 = (23.4, 33.1),
                A2 = (16.5, 23.4),
                A3 = (11.7, 16.5),
                A4 = (8.3, 11.7),
                A5 = (5.8, 8.3),
                A6 = (4.1, 5.8),
                A7 = (2.9, 4.1),
                A8 = (2.0, 2.9),
                A9 = (1.5, 2.0),
                A10 = (1.0, 1.5),
                B0 = (39.4, 55.7),
                B1 = (27.8, 39.4),
                B2 = (19.7, 27.8),
                B3 = (13.9, 19.7),
                B4 = (9.8, 13.9),
                B5 = (6.9, 9.8),
                B6 = (4.9, 6.9),
                B7 = (3.5, 4.9),
                B8 = (2.4, 3.5),
                B9 = (1.7, 2.4),
                B10 = (1.2, 1.7),
                C0 = (36.1, 51.5),
                C1 = (25.5, 36.1),
                C2 = (18.0, 25.5),
                C3 = (12.8, 18.0),
                C4 = (9.0, 12.8),
                C5 = (6.4, 9.0),
                C6 = (4.5, 6.4),
                C7 = (3.2, 4.5),
                C8 = (2.2, 3.2),
                C9 = (1.6, 2.2),
                C10 = (1.1, 1.6),
                HalfLetter = (5.5, 8.5),
                GovernmentLetter = (8.0, 10.0),
                Letter = (8.5, 11.0),
                JuniorLegal = (5.0, 8.0),
                GovernmentLegal = (8.5, 13.0),
                Legal = (8.5, 14.0),
                Tabloid = (11.0, 17.0),
            };

            return result;
        }

        private static PaperSizes GetSizeMillimeters()
        {
            // Source of the data: https://www.papersizes.org/a-paper-sizes.htm#finder
            PaperSizes result = new()
            {
                A0x4 = (1682, 2378),
                A0x2 = (1189, 1682),
                A0 = (841, 1189),
                A1 = (594, 841),
                A2 = (420, 594),
                A3 = (297, 420),
                A4 = (210, 297),
                A5 = (148, 210),
                A6 = (105, 148),
                A7 = (74, 105),
                A8 = (52, 74),
                A9 = (37, 52),
                A10 = (26, 37),
                B0 = (1000, 1414),
                B1 = (707, 1000),
                B2 = (500, 707),
                B3 = (353, 500),
                B4 = (250, 353),
                B5 = (176, 250),
                B6 = (125, 176),
                B7 = (88, 125),
                B8 = (62, 88),
                B9 = (44, 62),
                B10 = (31, 44),
                C0 = (917, 1297),
                C1 = (648, 917),
                C2 = (458, 648),
                C3 = (324, 458),
                C4 = (229, 324),
                C5 = (162, 229),
                C6 = (114, 162),
                C7 = (81, 114),
                C8 = (57, 81),
                C9 = (40, 57),
                C10 = (28, 40),
                HalfLetter = (140, 216),
                GovernmentLetter = (203, 254),
                Letter = (216, 279),
                JuniorLegal = (127, 203),
                GovernmentLegal = (216, 330),
                Legal = (216, 356),
                Tabloid = (279, 432),
            };

            return result;
        }
    }
}