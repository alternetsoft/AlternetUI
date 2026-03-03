using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    public partial class PaperSizes
    {
        /// <summary>
        /// Gets or sets the default number of decimal places used when representing inch measurements.
        /// This value is used in methods that convert millimeters to inches or when formatting inch-based sizes for display.
        /// Adjusting this value allows you to control the precision of inch measurements across the application,
        /// ensuring that they are displayed with the desired number of decimal places for clarity and consistency.
        /// </summary>
        /// <remarks>This value determines the precision applied to inch-based measurements in
        /// applications that require decimal formatting. Adjust this value to control the number of digits displayed
        /// after the decimal point for inch values.</remarks>
        public static int DefaultInchesDecimals
        {
            get => defaultInchesDecimals;

            set
            {
                if (value == defaultInchesDecimals)
                    return;
                defaultInchesDecimals = value;
                sizeInchesRounded = null;
            }
        }

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
        /// Gets or sets the paper sizes with dimensions rounded to the specific number of decimal places
        /// which is determined by the <see cref="DefaultInchesDecimals"/> property.
        /// </summary>
        public static PaperSizes SizeInchesRounded
        {
            get
            {
                sizeInchesRounded ??= CreateSizeInchesRounded();
                return sizeInchesRounded;
            }

            set
            {
                sizeInchesRounded = value;
            }
        }

        /// <summary>
        /// Gets or sets paper size for the "4A0" format.
        /// </summary>
        public SizeD A0x4 { get; set; } = (1682, 2378);

        /// <summary>
        /// Gets or sets paper size for the "2A0" format.
        /// </summary>
        public SizeD A0x2 { get; set; } = (1189, 1682);

        /// <summary>
        /// Gets or sets paper size for the "A0" format.
        /// Title: "A0 sheet, 841 x 1189 mm".
        /// </summary>
        public SizeD A0 { get; set; } = (841, 1189);

        /// <summary>
        /// Gets or sets paper size for the "A1" format.
        /// Title: "A1 sheet, 594 x 841 mm".
        /// </summary>
        public SizeD A1 { get; set; } = (594, 841);

        /// <summary>
        /// Gets or sets paper size for the "A2" format.
        /// Title: "A2 420 x 594 mm".
        /// </summary>
        public SizeD A2 { get; set; } = (420, 594);

        /// <summary>
        /// Gets or sets paper size for the "A3" format.
        /// Title: "A3 sheet, 297 x 420 mm".
        /// </summary>
        public SizeD A3 { get; set; } = (297, 420);

        /// <summary>
        /// Gets or sets paper size for the "A4" format.
        /// Title: "A4 sheet, 210 x 297 mm".
        /// </summary>
        public SizeD A4 { get; set; } = (210, 297);

        /// <summary>
        /// Gets or sets paper size for the "A5" format.
        /// Title: "A5 sheet, 148 x 210 mm".
        /// </summary>
        public SizeD A5 { get; set; } = (148, 210);

        /// <summary>
        /// Gets or sets paper size for the "A6" format.
        /// Title: "A6 105 x 148 mm".
        /// </summary>
        public SizeD A6 { get; set; } = (105, 148);

        /// <summary>
        /// Gets or sets paper size for the "A7" format.
        /// </summary>
        public SizeD A7 { get; set; } = (74, 105);

        /// <summary>
        /// Gets or sets paper size for the "A8" format.
        /// </summary>
        public SizeD A8 { get; set; } = (52, 74);

        /// <summary>
        /// Gets or sets paper size for the "A9" format.
        /// </summary>
        public SizeD A9 { get; set; } = (37, 52);

        /// <summary>
        /// Gets or sets paper size for the "A10" format.
        /// </summary>
        public SizeD A10 { get; set; } = (26, 37);

        /// <summary>
        /// Gets or sets paper size for the "Half Letter" format.
        /// </summary>
        public SizeD HalfLetter { get; set; } = (140, 216);

        /// <summary>
        /// Gets or sets paper size for the "Government Letter" format.
        /// </summary>
        public SizeD GovernmentLetter { get; set; } = (203, 254);

        /// <summary>
        /// Gets or sets paper size for the "Letter" format.
        /// Title: "Letter, 8 1/2 x 11 in".
        /// </summary>
        public SizeD Letter { get; set; } = (215.9f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "Junior Legal" format.
        /// </summary>
        public SizeD JuniorLegal { get; set; } = (127, 203);

        /// <summary>
        /// Gets or sets paper size for the "Government Legal" format.
        /// </summary>
        public SizeD GovernmentLegal { get; set; } = (216, 330);

        /// <summary>
        /// Gets or sets paper size for the "Legal" format.
        /// Title: "Legal, 8 1/2 x 14 in".
        /// </summary>
        public SizeD Legal { get; set; } = (215.9f, 355.6f);

        /// <summary>
        /// Gets or sets paper size for the "Tabloid" format.
        /// Title: "Tabloid, 11 x 17 in".
        /// </summary>
        public SizeD Tabloid { get; set; } = (279.4f, 431.8f);

        /// <summary>
        /// Gets or sets paper size for the "B10" format.
        /// </summary>
        public SizeD B10 { get; set; } = (31, 44);

        /// <summary>
        /// Gets or sets paper size for the "B9" format.
        /// </summary>
        public SizeD B9 { get; set; } = (44, 62);

        /// <summary>
        /// Gets or sets paper size for the "B8" format.
        /// </summary>
        public SizeD B8 { get; set; } = (62, 88);

        /// <summary>
        /// Gets or sets paper size for the "B7" format.
        /// </summary>
        public SizeD B7 { get; set; } = (88, 125);

        /// <summary>
        /// Gets or sets paper size for the "B6" format.
        /// </summary>
        public SizeD B6 { get; set; } = (125, 176);

        /// <summary>
        /// Gets or sets paper size for the "B5" format.
        /// </summary>
        public SizeD B5 { get; set; } = (176, 250);

        /// <summary>
        /// Gets or sets paper size for the "B4" format.
        /// Title: "B4 (ISOf) 250 x 353 mm".
        /// </summary>
        public SizeD B4 { get; set; } = (250, 353);

        /// <summary>
        /// Gets or sets paper size for the "B3" format.
        /// </summary>
        public SizeD B3 { get; set; } = (353, 500);

        /// <summary>
        /// Gets or sets paper size for the "B2" format.
        /// </summary>
        public SizeD B2 { get; set; } = (500, 707);

        /// <summary>
        /// Gets or sets paper size for the "B1" format.
        /// </summary>
        public SizeD B1 { get; set; } = (707, 1000);

        /// <summary>
        /// Gets or sets paper size for the "B0" format.
        /// </summary>
        public SizeD B0 { get; set; } = (1000, 1414);

        /// <summary>
        /// Gets or sets paper size for the "C10" format.
        /// </summary>
        public SizeD C10 { get; set; } = (28, 40);

        /// <summary>
        /// Gets or sets paper size for the "C9" format.
        /// </summary>
        public SizeD C9 { get; set; } = (40, 57);

        /// <summary>
        /// Gets or sets paper size for the "C8" format.
        /// </summary>
        public SizeD C8 { get; set; } = (57, 81);

        /// <summary>
        /// Gets or sets paper size for the "C7" format.
        /// </summary>
        public SizeD C7 { get; set; } = (81, 114);

        /// <summary>
        /// Gets or sets paper size for the "C6" format.
        /// </summary>
        public SizeD C6 { get; set; } = (114, 162);

        /// <summary>
        /// Gets or sets paper size for the "C5" format.
        /// </summary>
        public SizeD C5 { get; set; } = (162, 229);

        /// <summary>
        /// Gets or sets paper size for the "C4" format.
        /// </summary>
        public SizeD C4 { get; set; } = (229, 324);

        /// <summary>
        /// Gets or sets paper size for the "C3" format.
        /// </summary>
        public SizeD C3 { get; set; } = (324, 458);

        /// <summary>
        /// Gets or sets paper size for the "C2" format.
        /// </summary>
        public SizeD C2 { get; set; } = (458, 648);

        /// <summary>
        /// Gets or sets paper size for the "C1" format.
        /// </summary>
        public SizeD C1 { get; set; } = (648, 917);

        /// <summary>
        /// Gets or sets paper size for the "C0" format.
        /// </summary>
        public SizeD C0 { get; set; } = (917, 1297);

        /// <summary>
        /// Gets or sets paper size for the "C" format.
        /// Title: "C sheet, 17 x 22 in".
        /// </summary>
        public SizeD C { get; set; } = (431.8f, 558.8f);

        /// <summary>
        /// Gets or sets paper size for the "D" format.
        /// Title: "D sheet, 22 x 34 in".
        /// </summary>
        public SizeD D { get; set; } = (558.8f, 863.6f);

        /// <summary>
        /// Gets or sets paper size for the "E" format.
        /// Title: "E sheet, 34 x 44 in".
        /// </summary>
        public SizeD E { get; set; } = (863.6f, 1117.6f);

        /// <summary>
        /// Gets or sets paper size for the "Letter Small" format.
        /// Title: "Letter Small, 8 1/2 x 11 in".
        /// </summary>
        public SizeD LetterSmall { get; set; } = (215.9f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "Ledger" format.
        /// Title: "Ledger, 17 x 11 in".
        /// </summary>
        public SizeD Ledger { get; set; } = (431.8f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "Statement" format.
        /// Title: "Statement, 5 1/2 x 8 1/2 in".
        /// </summary>
        public SizeD Statement { get; set; } = (139.7f, 215.9f);

        /// <summary>
        /// Gets or sets paper size for the "Executive" format.
        /// Title: "Executive, 7 1/4 x 10 1/2 in".
        /// </summary>
        public SizeD Executive { get; set; } = (184.2f, 266.7f);

        /// <summary>
        /// Gets or sets paper size for the "A4 Small" format.
        /// Title: "A4 small sheet, 210 x 297 mm".
        /// </summary>
        public SizeD A4Small { get; set; } = (210.0f, 297.0f);

        /// <summary>
        /// Gets or sets paper size for the "Folio" format.
        /// Title: "Folio, 8 1/2 x 13 in".
        /// </summary>
        public SizeD Folio { get; set; } = (215.9f, 330.2f);

        /// <summary>
        /// Gets or sets paper size for the "Quarto" format.
        /// Title: "Quarto, 215 x 275 mm".
        /// </summary>
        public SizeD Quarto { get; set; } = (215.0f, 275.0f);

        /// <summary>
        /// Gets or sets paper size for the "10x14" sheet format.
        /// Title: "10 x 14 in".
        /// </summary>
        public SizeD Sheet10X14 { get; set; } = (254.0f, 355.6f);

        /// <summary>
        /// Gets or sets paper size for the "11x17" sheet format.
        /// Title: "11 x 17 in".
        /// </summary>
        public SizeD Sheet11X17 { get; set; } = (279.4f, 431.8f);

        /// <summary>
        /// Gets or sets paper size for the "Note" format.
        /// Title: "Note, 8 1/2 x 11 in".
        /// </summary>
        public SizeD Note { get; set; } = (215.9f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "#9" envelope format.
        /// Title: "#9 Envelope, 3 7/8 x 8 7/8 in".
        /// </summary>
        public SizeD Envelope9 { get; set; } = (98.4f, 225.4f);

        /// <summary>
        /// Gets or sets paper size for the "#10" envelope format.
        /// Title: "#10 Envelope, 4 1/8 x 9 1/2 in".
        /// </summary>
        public SizeD Envelope10 { get; set; } = (104.8f, 241.3f);

        /// <summary>
        /// Gets or sets paper size for the "#11" envelope format.
        /// Title: "#11 Envelope, 4 1/2 x 10 3/8 in".
        /// </summary>
        public SizeD Envelope11 { get; set; } = (114.3f, 263.5f);

        /// <summary>
        /// Gets or sets paper size for the "#12" envelope format.
        /// Title: "#12 Envelope, 4 3/4 x 11 in".
        /// </summary>
        public SizeD Envelope12 { get; set; } = (120.6f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "#14" envelope format.
        /// Title: "#14 Envelope, 5 x 11 1/2 in".
        /// </summary>
        public SizeD Envelope14 { get; set; } = (127.0f, 292.1f);

        /// <summary>
        /// Gets or sets paper size for the "DL" envelope format.
        /// Title: "DL Envelope, 110 x 220 mm".
        /// </summary>
        public SizeD EnvelopeDl { get; set; } = (110.0f, 220.0f);

        /// <summary>
        /// Gets or sets paper size for the "C5" envelope format.
        /// Title: "C5 Envelope, 162 x 229 mm".
        /// </summary>
        public SizeD EnvelopeC5 { get; set; } = (162.0f, 229.0f);

        /// <summary>
        /// Gets or sets paper size for the "C3" envelope format.
        /// Title: "C3 Envelope, 324 x 458 mm".
        /// </summary>
        public SizeD EnvelopeC3 { get; set; } = (324.0f, 458.0f);

        /// <summary>
        /// Gets or sets paper size for the "C4" envelope format.
        /// Title: "C4 Envelope, 229 x 324 mm".
        /// </summary>
        public SizeD EnvelopeC4 { get; set; } = (229.0f, 324.0f);

        /// <summary>
        /// Gets or sets paper size for the "C6" envelope format.
        /// Title: "C6 Envelope, 114 x 162 mm".
        /// </summary>
        public SizeD EnvelopeC6 { get; set; } = (114.0f, 162.0f);

        /// <summary>
        /// Gets or sets paper size for the "C6/5" envelope format.
        /// Title: "C65 Envelope, 114 x 229 mm".
        /// </summary>
        public SizeD EnvelopeC65 { get; set; } = (114.0f, 229.0f);

        /// <summary>
        /// Gets or sets paper size for the "B4" envelope format.
        /// Title: "B4 Envelope, 250 x 353 mm".
        /// </summary>
        public SizeD EnvelopeB4 { get; set; } = (250.0f, 353.0f);

        /// <summary>
        /// Gets or sets paper size for the "B5" envelope format.
        /// Title: "B5 Envelope, 176 x 250 mm".
        /// </summary>
        public SizeD EnvelopeB5 { get; set; } = (176.0f, 250.0f);

        /// <summary>
        /// Gets or sets paper size for the "B6" envelope format.
        /// Title: "B6 Envelope, 176 x 125 mm".
        /// </summary>
        public SizeD EnvelopeB6 { get; set; } = (176.0f, 125.0f);

        /// <summary>
        /// Gets or sets paper size for the "Italy" envelope format.
        /// Title: "Italy Envelope, 110 x 230 mm".
        /// </summary>
        public SizeD EnvelopeItaly { get; set; } = (110.0f, 230.0f);

        /// <summary>
        /// Gets or sets paper size for the "Monarch" envelope format.
        /// Title: "Monarch Envelope, 3 7/8 x 7 1/2 in".
        /// </summary>
        public SizeD EnvelopeMonarch { get; set; } = (98.4f, 190.5f);

        /// <summary>
        /// Gets or sets paper size for the "Personal" envelope format.
        /// Title: "6 3/4 Envelope, 3 5/8 x 6 1/2 in".
        /// </summary>
        public SizeD EnvelopePersonal { get; set; } = (92.1f, 165.1f);

        /// <summary>
        /// Gets or sets paper size for the "US Standard Fanfold" format.
        /// Title: "US Std Fanfold, 14 7/8 x 11 in".
        /// </summary>
        public SizeD FanfoldUs { get; set; } = (377.8f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "German Standard Fanfold" format.
        /// Title: "German Std Fanfold, 8 1/2 x 12 in".
        /// </summary>
        public SizeD FanfoldStandardGerman { get; set; } = (215.9f, 304.8f);

        /// <summary>
        /// Gets or sets paper size for the "German Legal Fanfold" format.
        /// Title: "German Legal Fanfold, 8 1/2 x 13 in".
        /// </summary>
        public SizeD FanfoldLegalGerman { get; set; } = (215.9f, 330.2f);

        /// <summary>
        /// Gets or sets paper size for the "ISO B4" format.
        /// Title: "B4 sheet, 250 x 354 mm".
        /// </summary>
        public SizeD IsoB4 { get; set; } = (250.0f, 354.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Postcard" format.
        /// Title: "Japanese Postcard 100 x 148 mm".
        /// </summary>
        public SizeD JapanesePostcard { get; set; } = (100.0f, 148.0f);

        /// <summary>
        /// Gets or sets paper size for the "9x11" sheet format.
        /// Title: "9 x 11 in".
        /// </summary>
        public SizeD Sheet9X11 { get; set; } = (228.6f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "10x11" sheet format.
        /// Title: "10 x 11 in".
        /// </summary>
        public SizeD Sheet10X11 { get; set; } = (254.0f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "15x11" sheet format.
        /// Title: "15 x 11 in".
        /// </summary>
        public SizeD Sheet15X11 { get; set; } = (381.0f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "Invitation" envelope format.
        /// Title: "Envelope Invite 220 x 220 mm".
        /// </summary>
        public SizeD EnvelopeInvite { get; set; } = (220.0f, 220.0f);

        /// <summary>
        /// Gets or sets paper size for the "Letter Extra" format.
        /// Title: "Letter Extra 9 1/2 x 12 in".
        /// </summary>
        public SizeD LetterExtra { get; set; } = (241.3f, 304.8f);

        /// <summary>
        /// Gets or sets paper size for the "Legal Extra" format.
        /// Title: "Legal Extra 9 1/2 x 15 in".
        /// </summary>
        public SizeD LegalExtra { get; set; } = (241.3f, 381.0f);

        /// <summary>
        /// Gets or sets paper size for the "Tabloid Extra" format.
        /// Title: "Tabloid Extra 11.69 x 18 in".
        /// </summary>
        public SizeD TabloidExtra { get; set; } = (296.9f, 457.2f);

        /// <summary>
        /// Gets or sets paper size for the "A4 Extra" format.
        /// Title: "A4 Extra 9.27 x 12.69 in".
        /// </summary>
        public SizeD A4Extra { get; set; } = (235.5f, 322.3f);

        /// <summary>
        /// Gets or sets paper size for the "Letter Transverse" format.
        /// Title: "Letter Transverse 8 1/2 x 11 in".
        /// </summary>
        public SizeD LetterTransverse { get; set; } = (215.9f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "A4 Transverse" format.
        /// Title: "A4 Transverse 210 x 297 mm".
        /// </summary>
        public SizeD A4Transverse { get; set; } = (210.0f, 297.0f);

        /// <summary>
        /// Gets or sets paper size for the "Letter Extra Transverse" format.
        /// Title: "Letter Extra Transverse 9.275 x 12 in".
        /// </summary>
        public SizeD LetterExtraTransverse { get; set; } = (235.5f, 304.8f);

        /// <summary>
        /// Gets or sets paper size for the "A Plus" format.
        /// Title: "SuperA/SuperA/A4 227 x 356 mm".
        /// </summary>
        public SizeD APlus { get; set; } = (227.0f, 356.0f);

        /// <summary>
        /// Gets or sets paper size for the "B Plus" format.
        /// Title: "SuperB/SuperB/A3 305 x 487 mm".
        /// </summary>
        public SizeD BPlus { get; set; } = (305.0f, 487.0f);

        /// <summary>
        /// Gets or sets paper size for the "Letter Plus" format.
        /// Title: "Letter Plus 8 1/2 x 12.69 in".
        /// </summary>
        public SizeD LetterPlus { get; set; } = (215.9f, 322.3f);

        /// <summary>
        /// Gets or sets paper size for the "A4 Plus" format.
        /// Title: "A4 Plus 210 x 330 mm".
        /// </summary>
        public SizeD A4Plus { get; set; } = (210.0f, 330.0f);

        /// <summary>
        /// Gets or sets paper size for the "A5 Transverse" format.
        /// Title: "A5 Transverse 148 x 210 mm".
        /// </summary>
        public SizeD A5Transverse { get; set; } = (148.0f, 210.0f);

        /// <summary>
        /// Gets or sets paper size for the "B5 Transverse" format.
        /// Title: "B5 (JISf) Transverse 182 x 257 mm".
        /// </summary>
        public SizeD B5Transverse { get; set; } = (182.0f, 257.0f);

        /// <summary>
        /// Gets or sets paper size for the "A3 Extra" format.
        /// Title: "A3 Extra 322 x 445 mm".
        /// </summary>
        public SizeD A3Extra { get; set; } = (322.0f, 445.0f);

        /// <summary>
        /// Gets or sets paper size for the "A5 Extra" format.
        /// Title: "A5 Extra 174 x 235 mm".
        /// </summary>
        public SizeD A5Extra { get; set; } = (174.0f, 235.0f);

        /// <summary>
        /// Gets or sets paper size for the "B5 Extra" format.
        /// Title: "B5 (ISOf) Extra 201 x 276 mm".
        /// </summary>
        public SizeD B5Extra { get; set; } = (201.0f, 276.0f);

        /// <summary>
        /// Gets or sets paper size for the "A3 Transverse" format.
        /// Title: "A3 Transverse 297 x 420 mm".
        /// </summary>
        public SizeD A3Transverse { get; set; } = (297.0f, 420.0f);

        /// <summary>
        /// Gets or sets paper size for the "A3 Extra Transverse" format.
        /// Title: "A3 Extra Transverse 322 x 445 mm".
        /// </summary>
        public SizeD A3ExtraTransverse { get; set; } = (322.0f, 445.0f);

        /// <summary>
        /// Gets or sets paper size for the "Double Japanese Postcard" format.
        /// Title: "Japanese Double Postcard 200 x 148 mm".
        /// </summary>
        public SizeD DblJapanesePostcard { get; set; } = (200.0f, 148.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope Kaku #2" format.
        /// Title: "Japanese Envelope Kaku #2".
        /// </summary>
        public SizeD JapaneseEnvelopeKaku2 { get; set; } = (240.0f, 332.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope Kaku #3" format.
        /// Title: "Japanese Envelope Kaku #3".
        /// </summary>
        public SizeD JapaneseEnvelopeKaku3 { get; set; } = (216.0f, 277.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope Chou #3" format.
        /// Title: "Japanese Envelope Chou #3".
        /// </summary>
        public SizeD JapaneseEnvelopeChou3 { get; set; } = (120.0f, 235.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope Chou #4" format.
        /// Title: "Japanese Envelope Chou #4".
        /// </summary>
        public SizeD JapaneseEnvelopeChou4 { get; set; } = (90.0f, 205.0f);

        /// <summary>
        /// Gets or sets paper size for the "Letter Rotated" format.
        /// Title: "Letter Rotated 11 x 8 1/2 in".
        /// </summary>
        public SizeD LetterRotated { get; set; } = (279.4f, 215.9f);

        /// <summary>
        /// Gets or sets paper size for the "A3 Rotated" format.
        /// Title: "A3 Rotated 420 x 297 mm".
        /// </summary>
        public SizeD A3Rotated { get; set; } = (420.0f, 297.0f);

        /// <summary>
        /// Gets or sets paper size for the "A4 Rotated" format.
        /// Title: "A4 Rotated 297 x 210 mm".
        /// </summary>
        public SizeD A4Rotated { get; set; } = (297.0f, 210.0f);

        /// <summary>
        /// Gets or sets paper size for the "A5 Rotated" format.
        /// Title: "A5 Rotated 210 x 148 mm".
        /// </summary>
        public SizeD A5Rotated { get; set; } = (210.0f, 148.0f);

        /// <summary>
        /// Gets or sets paper size for the "B4 JIS Rotated" format.
        /// Title: "B4 (JISf) Rotated 364 x 257 mm".
        /// </summary>
        public SizeD B4JisRotated { get; set; } = (364.0f, 257.0f);

        /// <summary>
        /// Gets or sets paper size for the "B5 JIS Rotated" format.
        /// Title: "B5 (JISf) Rotated 257 x 182 mm".
        /// </summary>
        public SizeD B5JisRotated { get; set; } = (257.0f, 182.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Postcard Rotated" format.
        /// Title: "Japanese Postcard Rotated 148 x 100 mm".
        /// </summary>
        public SizeD JapanesePostcardRotated { get; set; } = (148.0f, 100.0f);

        /// <summary>
        /// Gets or sets paper size for the "Double Japanese Postcard Rotated" format.
        /// Title: "Double Japanese Postcard Rotated 148 x 200 mm".
        /// </summary>
        public SizeD DblJapanesePostcardRotated { get; set; } = (148.0f, 200.0f);

        /// <summary>
        /// Gets or sets paper size for the "A6 Rotated" format.
        /// Title: "A6 Rotated 148 x 105 mm".
        /// </summary>
        public SizeD A6Rotated { get; set; } = (148.0f, 105.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope Kaku #2 Rotated" format.
        /// Title: "Japanese Envelope Kaku #2 Rotated".
        /// </summary>
        public SizeD JapaneseEnvelopeKaku2Rotated { get; set; } = (332.0f, 240.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope Kaku #3 Rotated" format.
        /// Title: "Japanese Envelope Kaku #3 Rotated".
        /// </summary>
        public SizeD JapaneseEnvelopeKaku3Rotated { get; set; } = (277.0f, 216.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope Chou #3 Rotated" format.
        /// Title: "Japanese Envelope Chou #3 Rotated".
        /// </summary>
        public SizeD JapaneseEnvelopeChou3Rotated { get; set; } = (235.0f, 120.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope Chou #4 Rotated" format.
        /// Title: "Japanese Envelope Chou #4 Rotated".
        /// </summary>
        public SizeD JapaneseEnvelopeChou4Rotated { get; set; } = (205.0f, 90.0f);

        /// <summary>
        /// Gets or sets paper size for the "B6 JIS" format.
        /// Title: "B6 (JISf) 128 x 182 mm".
        /// </summary>
        public SizeD B6Jis { get; set; } = (128.0f, 182.0f);

        /// <summary>
        /// Gets or sets paper size for the "B6 JIS Rotated" format.
        /// Title: "B6 (JISf) Rotated 182 x 128 mm".
        /// </summary>
        public SizeD B6JisRotated { get; set; } = (182.0f, 128.0f);

        /// <summary>
        /// Gets or sets paper size for the "12x11" sheet format.
        /// Title: "12 x 11 in".
        /// </summary>
        public SizeD Sheet12X11 { get; set; } = (304.8f, 279.4f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope You #4" format.
        /// Title: "Japanese Envelope You #4".
        /// </summary>
        public SizeD JapaneseEnvelopeYou4 { get; set; } = (235.0f, 105.0f);

        /// <summary>
        /// Gets or sets paper size for the "Japanese Envelope You #4 Rotated" format.
        /// Title: "Japanese Envelope You #4 Rotated".
        /// </summary>
        public SizeD JapaneseEnvelopeYou4Rotated { get; set; } = (105.0f, 235.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC 16K" format.
        /// Title: "PRC 16K 146 x 215 mm".
        /// </summary>
        public SizeD Prc16k { get; set; } = (146.0f, 215.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC 32K" format.
        /// Title: "PRC 32K 97 x 151 mm".
        /// </summary>
        public SizeD Prc32k { get; set; } = (97.0f, 151.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC 32K Big" format.
        /// Title: "PRC 32K(Bigf) 97 x 151 mm".
        /// </summary>
        public SizeD Prc32kBig { get; set; } = (97.0f, 151.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #1" format.
        /// Title: "PRC Envelope #1 102 x 165 mm".
        /// </summary>
        public SizeD PrcEnvelope1 { get; set; } = (102.0f, 165.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #2" format.
        /// Title: "PRC Envelope #2 102 x 176 mm".
        /// </summary>
        public SizeD PrcEnvelope2 { get; set; } = (102.0f, 176.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #3" format.
        /// Title: "PRC Envelope #3 125 x 176 mm".
        /// </summary>
        public SizeD PrcEnvelope3 { get; set; } = (125.0f, 176.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #4" format.
        /// Title: "PRC Envelope #4 110 x 208 mm".
        /// </summary>
        public SizeD PrcEnvelope4 { get; set; } = (110.0f, 208.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #5" format.
        /// Title: "PRC Envelope #5 110 x 220 mm".
        /// </summary>
        public SizeD PrcEnvelope5 { get; set; } = (110.0f, 220.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #6" format.
        /// Title: "PRC Envelope #6 120 x 230 mm".
        /// </summary>
        public SizeD PrcEnvelope6 { get; set; } = (120.0f, 230.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #7" format.
        /// Title: "PRC Envelope #7 160 x 230 mm".
        /// </summary>
        public SizeD PrcEnvelope7 { get; set; } = (160.0f, 230.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #8" format.
        /// Title: "PRC Envelope #8 120 x 309 mm".
        /// </summary>
        public SizeD PrcEnvelope8 { get; set; } = (120.0f, 309.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #9" format.
        /// Title: "PRC Envelope #9 229 x 324 mm".
        /// </summary>
        public SizeD PrcEnvelope9 { get; set; } = (229.0f, 324.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #10" format.
        /// Title: "PRC Envelope #10 324 x 458 mm".
        /// </summary>
        public SizeD PrcEnvelope10 { get; set; } = (324.0f, 458.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC 16K Rotated" format.
        /// Title: "PRC 16K Rotated".
        /// </summary>
        public SizeD Prc16kRotated { get; set; } = (215.0f, 146.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC 32K Rotated" format.
        /// Title: "PRC 32K Rotated".
        /// </summary>
        public SizeD Prc32kRotated { get; set; } = (151.0f, 97.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC 32K Big Rotated" format.
        /// Title: "PRC 32K(Bigf) Rotated".
        /// </summary>
        public SizeD Prc32kBigRotated { get; set; } = (151.0f, 97.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #1 Rotated" format.
        /// Title: "PRC Envelope #1 Rotated 165 x 102 mm".
        /// </summary>
        public SizeD PrcEnvelope1Rotated { get; set; } = (165.0f, 102.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #2 Rotated" format.
        /// Title: "PRC Envelope #2 Rotated 176 x 102 mm".
        /// </summary>
        public SizeD PrcEnvelope2Rotated { get; set; } = (176.0f, 102.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #3 Rotated" format.
        /// Title: "PRC Envelope #3 Rotated 176 x 125 mm".
        /// </summary>
        public SizeD PrcEnvelope3Rotated { get; set; } = (176.0f, 125.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #4 Rotated" format.
        /// Title: "PRC Envelope #4 Rotated 208 x 110 mm".
        /// </summary>
        public SizeD PrcEnvelope4Rotated { get; set; } = (208.0f, 110.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #5 Rotated" format.
        /// Title: "PRC Envelope #5 Rotated 220 x 110 mm".
        /// </summary>
        public SizeD PrcEnvelope5Rotated { get; set; } = (220.0f, 110.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #6 Rotated" format.
        /// Title: "PRC Envelope #6 Rotated 230 x 120 mm".
        /// </summary>
        public SizeD PrcEnvelope6Rotated { get; set; } = (230.0f, 120.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #7 Rotated" format.
        /// Title: "PRC Envelope #7 Rotated 230 x 160 mm".
        /// </summary>
        public SizeD PrcEnvelope7Rotated { get; set; } = (230.0f, 160.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #8 Rotated" format.
        /// Title: "PRC Envelope #8 Rotated 309 x 120 mm".
        /// </summary>
        public SizeD PrcEnvelope8Rotated { get; set; } = (309.0f, 120.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #9 Rotated" format.
        /// Title: "PRC Envelope #9 Rotated 324 x 229 mm".
        /// </summary>
        public SizeD PrcEnvelope9Rotated { get; set; } = (324.0f, 229.0f);

        /// <summary>
        /// Gets or sets paper size for the "PRC Envelope #10 Rotated" format.
        /// Title: "PRC Envelope #10 Rotated 458 x 324 mm".
        /// </summary>
        public SizeD PrcEnvelope10Rotated { get; set; } = (458.0f, 324.0f);
    }
}
