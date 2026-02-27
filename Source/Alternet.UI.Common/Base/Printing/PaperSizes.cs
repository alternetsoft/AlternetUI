using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Provides a collection of predefined paper sizes for standard international (ISO) and North American formats,
    /// supporting both millimeter and inch units.
    /// </summary>
    /// <remarks>The PaperSizes class offers convenient access to a wide range of commonly used paper sizes,
    /// including A, B, and C series, as well as US-specific formats such as Letter, Legal, and Tabloid. It enables
    /// applications to retrieve, customize, and transform paper size definitions for use in printing, layout, or
    /// document formatting scenarios. Static properties are available for accessing standard sets of sizes in
    /// millimeters or inches, and instance members allow for further customization or extension as needed.</remarks>
    public partial class PaperSizes
    {
        private static PaperSizes? sizeMillimeters;
        private static PaperSizes? sizeInches;
        private static PaperSizes? sizeInchesRounded;
        private static int defaultInchesDecimals = 2;

        static PaperSizes()
        {
        }

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
        /// Converts a measurement from millimeters to inches and rounds
        /// the result to the specified number of decimal places.
        /// </summary>
        /// <remarks>Uses a conversion factor of 25.4 millimeters per inch.</remarks>
        /// <param name="mm">The measurement in millimeters to convert.</param>
        /// <param name="decimals">The number of decimal places to which the result is rounded.
        /// Defaults to 2 if not specified.</param>
        /// <returns>The equivalent measurement in inches, rounded to the specified number of decimal places.</returns>
        public static float MmToInches(float mm, int decimals)
        {
            const float mmPerInch = 25.4f;
            return MathF.Round(mm / mmPerInch, decimals);
        }

        /// <summary>
        /// Converts a measurement from millimeters to the equivalent value in inches.
        /// </summary>
        /// <remarks>This method uses a conversion factor of 25.4 millimeters per inch. Supplying a
        /// negative value for the parameter may result in an invalid conversion.</remarks>
        /// <param name="mm">The length in millimeters to convert. Must be a non-negative value.</param>
        /// <returns>The corresponding length in inches as a single-precision floating-point number.</returns>
        public static float MmToInches(float mm)
        {
            const float mmPerInch = 25.4f;
            return mm / mmPerInch;
        }

        /// <summary>
        /// Creates a new instance of the PaperSizes class with standard ISO and US paper sizes defined in inches.
        /// This method converts the standard millimeter sizes to inches using the (<see cref="MmToInches(float, int)"/>) method,
        /// applying rounding to the specified number of decimal places for inch measurements <see cref="DefaultInchesDecimals"/> .
        /// </summary>
        /// <remarks>Use this method when you require paper size definitions in inches for printing,
        /// layout, or document formatting tasks. The returned object includes both international and US standard sizes
        /// for convenience.</remarks>
        /// <returns>A PaperSizes object containing predefined dimensions for common ISO (A, B, C series) and US (Letter, Legal,
        /// Tabloid, etc.) paper sizes, with all measurements specified in inches.</returns>
        public static PaperSizes CreateSizeInchesRounded()
        {
            var result = CreateSizeMillimeters();
            SizeD Transforms(SizeD s) => (MmToInches(s.Width, DefaultInchesDecimals), MmToInches(s.Height, DefaultInchesDecimals));
            result.Transform(Transforms);
            return result;
        }

        /// <summary>
        /// Creates a new instance of the PaperSizes class with dimensions specified in inches.
        /// </summary>
        /// <remarks>Use this method when you require paper size measurements in inches rather than
        /// millimeters. The returned object represents the same paper size as CreateSizeMillimeters, but with
        /// dimensions converted to inches.</remarks>
        /// <returns>A PaperSizes object whose width and height are expressed in inches.</returns>
        public static PaperSizes CreateSizeInches()
        {
            var result = CreateSizeMillimeters();
            SizeD Transforms(SizeD s) => (MmToInches(s.Width), MmToInches(s.Height));
            result.Transform(Transforms);
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

        /// <summary>
        /// Logs the available paper sizes in both millimeters and inches.
        /// </summary>
        /// <remarks>This method calls the Log method on two different size creators, one for millimeters
        /// and one for inches, to output the respective paper sizes.</remarks>
        public static void Log()
        {
            CreateSizeMillimeters().Log("PaperSizes (mm)");
            CreateSizeInches().Log("PaperSizes (inches)");
        }

        /// <summary>
        /// Logs the sizes of all known paper kinds to the specified log writer.
        /// </summary>
        /// <remarks>This method begins a logging section labeled "PaperSizes", writes the size of each
        /// known paper kind, and then ends the section. The log writer must support sectioned logging and line
        /// writing.</remarks>
        /// <param name="log">The log writer to which the paper sizes are written. If null, the default debug log writer is used.</param>
        /// <param name="title">An optional title for the logging section. If null, "PaperSizes" is used.</param>
        public virtual void Log(string? title = null, ILogWriter? log = null)
        {
            log ??= LogWriter.Debug;
            log.BeginSection(title ?? "PaperSizes");

            foreach (KnownPaperKind kind in Enum.GetValues<KnownPaperKind>())
            {
                log.WriteLine($"{kind}: {GetSize(kind)}");
            }

            log.EndSection();
        }

        /// <summary>
        /// Copies all paper sizes from the specified instance, optionally applying a transformation to each size before
        /// assignment.
        /// </summary>
        /// <remarks>This method iterates through all known paper kinds and assigns their sizes from the
        /// specified instance. Use the <paramref name="transform"/> parameter to modify each size during assignment,
        /// such as for unit conversion or scaling.</remarks>
        /// <param name="other">The source <see cref="PaperSizes"/> instance from which to copy paper sizes. Cannot
        /// be <see langword="null"/>.</param>
        /// <param name="transform">An optional function that transforms each <see cref="SizeD"/> value before it is assigned.
        /// If <see langword="null"/>, sizes are assigned directly.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <see langword="null"/>.</exception>
        public virtual void Assign(PaperSizes other, Func<SizeD, SizeD>? transform = null)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            foreach (KnownPaperKind kind in Enum.GetValues<KnownPaperKind>())
            {
                SetSize(kind, other.GetSize(kind), transform);
            }
        }

        /// <summary>
        /// Applies a transformation function to the sizes of all known paper kinds.
        /// </summary>
        /// <remarks>This method iterates through all known paper kinds and applies the specified
        /// transformation to their sizes. Use this method to uniformly adjust or modify paper sizes according to custom
        /// logic.</remarks>
        /// <param name="transform">A function that defines the transformation to apply to each paper size.
        /// The function takes the current size
        /// as input and returns the transformed size.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="transform"/> is <see langword="null"/>.</exception>
        public virtual void Transform(Func<SizeD, SizeD> transform)
        {
            if (transform is null)
                throw new ArgumentNullException(nameof(transform));
            foreach (KnownPaperKind kind in Enum.GetValues<KnownPaperKind>())
            {
                SetSize(kind, GetSize(kind), transform);
            }
        }

        /// <summary>
        /// Sets the size for the specified known paper kind.
        /// </summary>
        /// <remarks>Use this method to customize the dimensions of predefined paper types at runtime.
        /// Ensure that the provided size is appropriate for the intended paper kind.</remarks>
        /// <param name="kind">The paper type for which to assign a new size. Must be a valid value
        /// from the KnownPaperKind enumeration.</param>
        /// <param name="size">The dimensions to assign to the specified paper kind. Represents the
        /// width and height in device-independent units.</param>
        /// <param name="transform">An optional function to transform the size before assignment. If provided,
        /// this function will be applied to the size before setting it.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified kind is not
        /// a valid value in the KnownPaperKind enumeration.</exception>
        public virtual void SetSize(KnownPaperKind kind, SizeD size, Func<SizeD, SizeD>? transform = null)
        {
            SizeD Transform(SizeD s)
            {
                return transform is not null ? transform(s) : s;
            }

            switch (kind)
            {
                case KnownPaperKind.A0x4: A0x4 = Transform(size); break;
                case KnownPaperKind.A0x2: A0x2 = Transform(size); break;
                case KnownPaperKind.A0: A0 = Transform(size); break;
                case KnownPaperKind.A1: A1 = Transform(size); break;
                case KnownPaperKind.A2: A2 = Transform(size); break;
                case KnownPaperKind.A3: A3 = Transform(size); break;
                case KnownPaperKind.A4: A4 = Transform(size); break;
                case KnownPaperKind.A5: A5 = Transform(size); break;
                case KnownPaperKind.A6: A6 = Transform(size); break;
                case KnownPaperKind.A7: A7 = Transform(size); break;
                case KnownPaperKind.A8: A8 = Transform(size); break;
                case KnownPaperKind.A9: A9 = Transform(size); break;
                case KnownPaperKind.A10: A10 = Transform(size); break;
                case KnownPaperKind.B0: B0 = Transform(size); break;
                case KnownPaperKind.B1: B1 = Transform(size); break;
                case KnownPaperKind.B2: B2 = Transform(size); break;
                case KnownPaperKind.B3: B3 = Transform(size); break;
                case KnownPaperKind.B4: B4 = Transform(size); break;
                case KnownPaperKind.B5: B5 = Transform(size); break;
                case KnownPaperKind.B6: B6 = Transform(size); break;
                case KnownPaperKind.B7: B7 = Transform(size); break;
                case KnownPaperKind.B8: B8 = Transform(size); break;
                case KnownPaperKind.B9: B9 = Transform(size); break;
                case KnownPaperKind.B10: B10 = Transform(size); break;
                case KnownPaperKind.C0: C0 = Transform(size); break;
                case KnownPaperKind.C1: C1 = Transform(size); break;
                case KnownPaperKind.C2: C2 = Transform(size); break;
                case KnownPaperKind.C3: C3 = Transform(size); break;
                case KnownPaperKind.C4: C4 = Transform(size); break;
                case KnownPaperKind.C5: C5 = Transform(size); break;
                case KnownPaperKind.C6: C6 = Transform(size); break;
                case KnownPaperKind.C7: C7 = Transform(size); break;
                case KnownPaperKind.C8: C8 = Transform(size); break;
                case KnownPaperKind.C9: C9 = Transform(size); break;
                case KnownPaperKind.C10: C10 = Transform(size); break;
                case KnownPaperKind.HalfLetter: HalfLetter = Transform(size); break;
                case KnownPaperKind.GovernmentLetter: GovernmentLetter = Transform(size); break;
                case KnownPaperKind.Letter: Letter = Transform(size); break;
                case KnownPaperKind.JuniorLegal: JuniorLegal = Transform(size); break;
                case KnownPaperKind.GovernmentLegal: GovernmentLegal = Transform(size); break;
                case KnownPaperKind.Legal: Legal = Transform(size); break;
                case KnownPaperKind.Tabloid: Tabloid = Transform(size); break;
                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        /// <summary>
        /// Returns the dimensions of the specified standard paper type.
        /// </summary>
        /// <remarks>Use this method to obtain the standard dimensions for a variety of common paper
        /// sizes. Supplying an invalid or unsupported paper kind will result in an exception.</remarks>
        /// <param name="kind">A value from the KnownPaperKind enumeration that specifies the paper type
        /// for which to retrieve the size.</param>
        /// <returns>A SizeD structure representing the width and height of the specified paper type,
        /// in the appropriate units.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified kind is
        /// not a valid value of the KnownPaperKind enumeration.</exception>
        public virtual SizeD GetSize(KnownPaperKind kind)
        {
            switch (kind)
            {
                case KnownPaperKind.A0x4: return A0x4;
                case KnownPaperKind.A0x2: return A0x2;
                case KnownPaperKind.A0: return A0;
                case KnownPaperKind.A1: return A1;
                case KnownPaperKind.A2: return A2;
                case KnownPaperKind.A3: return A3;
                case KnownPaperKind.A4: return A4;
                case KnownPaperKind.A5: return A5;
                case KnownPaperKind.A6: return A6;
                case KnownPaperKind.A7: return A7;
                case KnownPaperKind.A8: return A8;
                case KnownPaperKind.A9: return A9;
                case KnownPaperKind.A10: return A10;
                case KnownPaperKind.B0: return B0;
                case KnownPaperKind.B1: return B1;
                case KnownPaperKind.B2: return B2;
                case KnownPaperKind.B3: return B3;
                case KnownPaperKind.B4: return B4;
                case KnownPaperKind.B5: return B5;
                case KnownPaperKind.B6: return B6;
                case KnownPaperKind.B7: return B7;
                case KnownPaperKind.B8: return B8;
                case KnownPaperKind.B9: return B9;
                case KnownPaperKind.B10: return B10;
                case KnownPaperKind.C0: return C0;
                case KnownPaperKind.C1: return C1;
                case KnownPaperKind.C2: return C2;
                case KnownPaperKind.C3: return C3;
                case KnownPaperKind.C4: return C4;
                case KnownPaperKind.C5: return C5;
                case KnownPaperKind.C6: return C6;
                case KnownPaperKind.C7: return C7;
                case KnownPaperKind.C8: return C8;
                case KnownPaperKind.C9: return C9;
                case KnownPaperKind.C10: return C10;
                case KnownPaperKind.HalfLetter: return HalfLetter;
                case KnownPaperKind.GovernmentLetter: return GovernmentLetter;
                case KnownPaperKind.Letter: return Letter;
                case KnownPaperKind.JuniorLegal: return JuniorLegal;
                case KnownPaperKind.GovernmentLegal: return GovernmentLegal;
                case KnownPaperKind.Legal: return Legal;
                case KnownPaperKind.Tabloid: return Tabloid;
                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
    }
    }
}