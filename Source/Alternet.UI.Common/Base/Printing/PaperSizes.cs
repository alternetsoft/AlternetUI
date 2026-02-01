using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Defines paper sizes.
    /// </summary>
    public partial class PaperSizes
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
                sizeMillimeters ??= CreateSizeMillimeters();
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
                sizeInches ??= CreateSizeInches();
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

        /// <summary>
        /// Creates a new instance of the PaperSizes class with standard ISO and US paper sizes defined in inches.
        /// </summary>
        /// <remarks>Use this method when you require paper size definitions in inches for printing,
        /// layout, or document formatting tasks. The returned object includes both international and US standard sizes
        /// for convenience.</remarks>
        /// <returns>A PaperSizes object containing predefined dimensions for common ISO (A, B, C series) and US (Letter, Legal,
        /// Tabloid, etc.) paper sizes, with all measurements specified in inches.</returns>
        public static PaperSizes CreateSizeInches()
        {
            PaperSizes result = new()
            {
                A0x4 = (66.2f, 93.6f),
                A0x2 = (46.8f, 66.2f),
                A0 = (33.1f, 46.8f),
                A1 = (23.4f, 33.1f),
                A2 = (16.5f, 23.4f),
                A3 = (11.7f, 16.5f),
                A4 = (8.3f, 11.7f),
                A5 = (5.8f, 8.3f),
                A6 = (4.1f, 5.8f),
                A7 = (2.9f, 4.1f),
                A8 = (2.0f, 2.9f),
                A9 = (1.5f, 2.0f),
                A10 = (1.0f, 1.5f),
                B0 = (39.4f, 55.7f),
                B1 = (27.8f, 39.4f),
                B2 = (19.7f, 27.8f),
                B3 = (13.9f, 19.7f),
                B4 = (9.8f, 13.9f),
                B5 = (6.9f, 9.8f),
                B6 = (4.9f, 6.9f),
                B7 = (3.5f, 4.9f),
                B8 = (2.4f, 3.5f),
                B9 = (1.7f, 2.4f),
                B10 = (1.2f, 1.7f),
                C0 = (36.1f, 51.5f),
                C1 = (25.5f, 36.1f),
                C2 = (18.0f, 25.5f),
                C3 = (12.8f, 18.0f),
                C4 = (9.0f, 12.8f),
                C5 = (6.4f, 9.0f),
                C6 = (4.5f, 6.4f),
                C7 = (3.2f, 4.5f),
                C8 = (2.2f, 3.2f),
                C9 = (1.6f, 2.2f),
                C10 = (1.1f, 1.6f),
                HalfLetter = (5.5f, 8.5f),
                GovernmentLetter = (8.0f, 10.0f),
                Letter = (8.5f, 11.0f),
                JuniorLegal = (5.0f, 8.0f),
                GovernmentLegal = (8.5f, 13.0f),
                Legal = (8.5f, 14.0f),
                Tabloid = (11.0f, 17.0f),
            };

            return result;
        }

        /// <summary>
        /// Creates a new instance of the PaperSizes class with standard international and North American paper sizes defined in
        /// millimeters.
        /// </summary>
        /// <remarks>The returned PaperSizes instance includes common ISO A, B, and C series sizes, as well as North
        /// American formats such as Letter, Legal, and Tabloid. This method is useful for applications that require accurate
        /// paper size measurements in millimeters for layout, printing, or conversion purposes.</remarks>
        /// <returns>A PaperSizes object containing predefined paper size dimensions, where each size is specified in millimeters.</returns>
        public static PaperSizes CreateSizeMillimeters()
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