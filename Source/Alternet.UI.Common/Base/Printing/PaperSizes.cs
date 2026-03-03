using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Provides a collection of predefined paper sizes for standard international (ISO) and North American formats,
    /// supporting both millimeter and inch units.
    /// Use properties <see cref="SizeMillimeters"/>, <see cref="SizeInches"/>, and <see cref="SizeInchesRounded"/>
    /// to access standard paper sizes in the desired units and precision.
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
        /// Initializes a new instance of the PaperSizes class with default values.
        /// Use the static properties <see cref="SizeMillimeters"/>, <see cref="SizeInches"/>, and <see cref="SizeInchesRounded"/>
        /// to access predefined sets of paper sizes.
        /// These properties are initialized lazily, meaning that the actual paper size data is created only when it is first accessed,
        /// allowing for efficient resource usage. The constructor can be used by derived classes to create custom paper size collections if needed.
        /// By default properties with paper sizes are initialized with standard dimensions in millimeters,
        /// and the inch-based properties are created by converting these millimeter sizes using the appropriate conversion methods.
        /// </summary>
        public PaperSizes()
        {
        }

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
            return new ();
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

            foreach (PaperKind kind in Enum.GetValues<PaperKind>())
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

            foreach (PaperKind kind in Enum.GetValues<PaperKind>())
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
            foreach (PaperKind kind in Enum.GetValues<PaperKind>())
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
        /// a valid value in the PaperKind enumeration.</exception>
        public virtual void SetSize(PaperKind kind, SizeD size, Func<SizeD, SizeD>? transform = null)
        {
            SizeD Transform(SizeD s)
            {
                return transform is not null ? transform(s) : s;
            }

            switch (kind)
            {
                case PaperKind.Letter: Letter = Transform(size); break;
                case PaperKind.Legal: Legal = Transform(size); break;
                case PaperKind.A4: A4 = Transform(size); break;
                case PaperKind.C: C = Transform(size); break;
                case PaperKind.D: D = Transform(size); break;
                case PaperKind.E: E = Transform(size); break;
                case PaperKind.LetterSmall: LetterSmall = Transform(size); break;
                case PaperKind.Tabloid: Tabloid = Transform(size); break;
                case PaperKind.Ledger: Ledger = Transform(size); break;
                case PaperKind.Statement: Statement = Transform(size); break;
                case PaperKind.Executive: Executive = Transform(size); break;
                case PaperKind.A3: A3 = Transform(size); break;
                case PaperKind.A4Small: A4Small = Transform(size); break;
                case PaperKind.A5: A5 = Transform(size); break;
                case PaperKind.B4: B4 = Transform(size); break;
                case PaperKind.B5: B5 = Transform(size); break;
                case PaperKind.Folio: Folio = Transform(size); break;
                case PaperKind.Quarto: Quarto = Transform(size); break;
                case PaperKind.Sheet10X14: Sheet10X14 = Transform(size); break;
                case PaperKind.Sheet11X17: Sheet11X17 = Transform(size); break;
                case PaperKind.Note: Note = Transform(size); break;
                case PaperKind.Envelope9: Envelope9 = Transform(size); break;
                case PaperKind.Envelope10: Envelope10 = Transform(size); break;
                case PaperKind.Envelope11: Envelope11 = Transform(size); break;
                case PaperKind.Envelope12: Envelope12 = Transform(size); break;
                case PaperKind.Envelope14: Envelope14 = Transform(size); break;
                case PaperKind.EnvelopeDl: EnvelopeDl = Transform(size); break;
                case PaperKind.EnvelopeC5: EnvelopeC5 = Transform(size); break;
                case PaperKind.EnvelopeC3: EnvelopeC3 = Transform(size); break;
                case PaperKind.EnvelopeC4: EnvelopeC4 = Transform(size); break;
                case PaperKind.EnvelopeC6: EnvelopeC6 = Transform(size); break;
                case PaperKind.EnvelopeC65: EnvelopeC65 = Transform(size); break;
                case PaperKind.EnvelopeB4: EnvelopeB4 = Transform(size); break;
                case PaperKind.EnvelopeB5: EnvelopeB5 = Transform(size); break;
                case PaperKind.EnvelopeB6: EnvelopeB6 = Transform(size); break;
                case PaperKind.EnvelopeItaly: EnvelopeItaly = Transform(size); break;
                case PaperKind.EnvelopeMonarch: EnvelopeMonarch = Transform(size); break;
                case PaperKind.EnvelopePersonal: EnvelopePersonal = Transform(size); break;
                case PaperKind.FanfoldUs: FanfoldUs = Transform(size); break;
                case PaperKind.FanfoldStandardGerman: FanfoldStandardGerman = Transform(size); break;
                case PaperKind.FanfoldLegalGerman: FanfoldLegalGerman = Transform(size); break;
                case PaperKind.IsoB4: IsoB4 = Transform(size); break;
                case PaperKind.JapanesePostcard: JapanesePostcard = Transform(size); break;
                case PaperKind.Sheet9X11: Sheet9X11 = Transform(size); break;
                case PaperKind.Sheet10X11: Sheet10X11 = Transform(size); break;
                case PaperKind.Sheet15X11: Sheet15X11 = Transform(size); break;
                case PaperKind.EnvelopeInvite: EnvelopeInvite = Transform(size); break;
                case PaperKind.LetterExtra: LetterExtra = Transform(size); break;
                case PaperKind.LegalExtra: LegalExtra = Transform(size); break;
                case PaperKind.TabloidExtra: TabloidExtra = Transform(size); break;
                case PaperKind.A4Extra: A4Extra = Transform(size); break;
                case PaperKind.LetterTransverse: LetterTransverse = Transform(size); break;
                case PaperKind.A4Transverse: A4Transverse = Transform(size); break;
                case PaperKind.LetterExtraTransverse: LetterExtraTransverse = Transform(size); break;
                case PaperKind.APlus: APlus = Transform(size); break;
                case PaperKind.BPlus: BPlus = Transform(size); break;
                case PaperKind.LetterPlus: LetterPlus = Transform(size); break;
                case PaperKind.A4Plus: A4Plus = Transform(size); break;
                case PaperKind.A5Transverse: A5Transverse = Transform(size); break;
                case PaperKind.B5Transverse: B5Transverse = Transform(size); break;
                case PaperKind.A3Extra: A3Extra = Transform(size); break;
                case PaperKind.A5Extra: A5Extra = Transform(size); break;
                case PaperKind.B5Extra: B5Extra = Transform(size); break;
                case PaperKind.A2: A2 = Transform(size); break;
                case PaperKind.A3Transverse: A3Transverse = Transform(size); break;
                case PaperKind.A3ExtraTransverse: A3ExtraTransverse = Transform(size); break;
                case PaperKind.DblJapanesePostcard: DblJapanesePostcard = Transform(size); break;
                case PaperKind.A6: A6 = Transform(size); break;
                case PaperKind.JapaneseEnvelopeKaku2: JapaneseEnvelopeKaku2 = Transform(size); break;
                case PaperKind.JapaneseEnvelopeKaku3: JapaneseEnvelopeKaku3 = Transform(size); break;
                case PaperKind.JapaneseEnvelopeChou3: JapaneseEnvelopeChou3 = Transform(size); break;
                case PaperKind.JapaneseEnvelopeChou4: JapaneseEnvelopeChou4 = Transform(size); break;
                case PaperKind.LetterRotated: LetterRotated = Transform(size); break;
                case PaperKind.A3Rotated: A3Rotated = Transform(size); break;
                case PaperKind.A4Rotated: A4Rotated = Transform(size); break;
                case PaperKind.A5Rotated: A5Rotated = Transform(size); break;
                case PaperKind.B4JisRotated: B4JisRotated = Transform(size); break;
                case PaperKind.B5JisRotated: B5JisRotated = Transform(size); break;
                case PaperKind.JapanesePostcardRotated: JapanesePostcardRotated = Transform(size); break;
                case PaperKind.DblJapanesePostcardRotated: DblJapanesePostcardRotated = Transform(size); break;
                case PaperKind.A6Rotated: A6Rotated = Transform(size); break;
                case PaperKind.JapaneseEnvelopeKaku2Rotated: JapaneseEnvelopeKaku2Rotated = Transform(size); break;
                case PaperKind.JapaneseEnvelopeKaku3Rotated: JapaneseEnvelopeKaku3Rotated = Transform(size); break;
                case PaperKind.JapaneseEnvelopeChou3Rotated: JapaneseEnvelopeChou3Rotated = Transform(size); break;
                case PaperKind.JapaneseEnvelopeChou4Rotated: JapaneseEnvelopeChou4Rotated = Transform(size); break;
                case PaperKind.B6Jis: B6Jis = Transform(size); break;
                case PaperKind.B6JisRotated: B6JisRotated = Transform(size); break;
                case PaperKind.Sheet12X11: Sheet12X11 = Transform(size); break;
                case PaperKind.JapaneseEnvelopeYou4: JapaneseEnvelopeYou4 = Transform(size); break;
                case PaperKind.JapaneseEnvelopeYou4Rotated: JapaneseEnvelopeYou4Rotated = Transform(size); break;
                case PaperKind.Prc16k: Prc16k = Transform(size); break;
                case PaperKind.Prc32k: Prc32k = Transform(size); break;
                case PaperKind.Prc32kBig: Prc32kBig = Transform(size); break;
                case PaperKind.PrcEnvelope1: PrcEnvelope1 = Transform(size); break;
                case PaperKind.PrcEnvelope2: PrcEnvelope2 = Transform(size); break;
                case PaperKind.PrcEnvelope3: PrcEnvelope3 = Transform(size); break;
                case PaperKind.PrcEnvelope4: PrcEnvelope4 = Transform(size); break;
                case PaperKind.PrcEnvelope5: PrcEnvelope5 = Transform(size); break;
                case PaperKind.PrcEnvelope6: PrcEnvelope6 = Transform(size); break;
                case PaperKind.PrcEnvelope7: PrcEnvelope7 = Transform(size); break;
                case PaperKind.PrcEnvelope8: PrcEnvelope8 = Transform(size); break;
                case PaperKind.PrcEnvelope9: PrcEnvelope9 = Transform(size); break;
                case PaperKind.PrcEnvelope10: PrcEnvelope10 = Transform(size); break;
                case PaperKind.Prc16kRotated: Prc16kRotated = Transform(size); break;
                case PaperKind.Prc32kRotated: Prc32kRotated = Transform(size); break;
                case PaperKind.Prc32kBigRotated: Prc32kBigRotated = Transform(size); break;
                case PaperKind.PrcEnvelope1Rotated: PrcEnvelope1Rotated = Transform(size); break;
                case PaperKind.PrcEnvelope2Rotated: PrcEnvelope2Rotated = Transform(size); break;
                case PaperKind.PrcEnvelope3Rotated: PrcEnvelope3Rotated = Transform(size); break;
                case PaperKind.PrcEnvelope4Rotated: PrcEnvelope4Rotated = Transform(size); break;
                case PaperKind.PrcEnvelope5Rotated: PrcEnvelope5Rotated = Transform(size); break;
                case PaperKind.PrcEnvelope6Rotated: PrcEnvelope6Rotated = Transform(size); break;
                case PaperKind.PrcEnvelope7Rotated: PrcEnvelope7Rotated = Transform(size); break;
                case PaperKind.PrcEnvelope8Rotated: PrcEnvelope8Rotated = Transform(size); break;
                case PaperKind.PrcEnvelope9Rotated: PrcEnvelope9Rotated = Transform(size); break;
                case PaperKind.PrcEnvelope10Rotated: PrcEnvelope10Rotated = Transform(size); break;
                case PaperKind.A0: A0 = Transform(size); break;
                case PaperKind.A1: A1 = Transform(size); break;

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        /// <summary>
        /// Returns the dimensions of the specified standard paper type.
        /// </summary>
        /// <remarks>Use this method to obtain the standard dimensions for a variety of common paper
        /// sizes. Supplying an invalid or unsupported paper kind will result in an exception.</remarks>
        /// <param name="kind">A value from the PaperKind enumeration that specifies the paper type
        /// for which to retrieve the size.</param>
        /// <returns>A SizeD structure representing the width and height of the specified paper type,
        /// in the appropriate units.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified kind is
        /// not a valid value of the PaperKind enumeration.</exception>
        public virtual SizeD GetSize(PaperKind kind)
        {
            switch (kind)
            {
                case PaperKind.Letter: return Letter;
                case PaperKind.Legal: return Legal;
                case PaperKind.A4: return A4;
                case PaperKind.C: return C;
                case PaperKind.D: return D;
                case PaperKind.E: return E;
                case PaperKind.LetterSmall: return LetterSmall;
                case PaperKind.Tabloid: return Tabloid;
                case PaperKind.Ledger: return Ledger;
                case PaperKind.Statement: return Statement;
                case PaperKind.Executive: return Executive;
                case PaperKind.A3: return A3;
                case PaperKind.A4Small: return A4Small;
                case PaperKind.A5: return A5;
                case PaperKind.B4: return B4;
                case PaperKind.B5: return B5;
                case PaperKind.Folio: return Folio;
                case PaperKind.Quarto: return Quarto;
                case PaperKind.Sheet10X14: return Sheet10X14;
                case PaperKind.Sheet11X17: return Sheet11X17;
                case PaperKind.Note: return Note;
                case PaperKind.Envelope9: return Envelope9;
                case PaperKind.Envelope10: return Envelope10;
                case PaperKind.Envelope11: return Envelope11;
                case PaperKind.Envelope12: return Envelope12;
                case PaperKind.Envelope14: return Envelope14;
                case PaperKind.EnvelopeDl: return EnvelopeDl;
                case PaperKind.EnvelopeC5: return EnvelopeC5;
                case PaperKind.EnvelopeC3: return EnvelopeC3;
                case PaperKind.EnvelopeC4: return EnvelopeC4;
                case PaperKind.EnvelopeC6: return EnvelopeC6;
                case PaperKind.EnvelopeC65: return EnvelopeC65;
                case PaperKind.EnvelopeB4: return EnvelopeB4;
                case PaperKind.EnvelopeB5: return EnvelopeB5;
                case PaperKind.EnvelopeB6: return EnvelopeB6;
                case PaperKind.EnvelopeItaly: return EnvelopeItaly;
                case PaperKind.EnvelopeMonarch: return EnvelopeMonarch;
                case PaperKind.EnvelopePersonal: return EnvelopePersonal;
                case PaperKind.FanfoldUs: return FanfoldUs;
                case PaperKind.FanfoldStandardGerman: return FanfoldStandardGerman;
                case PaperKind.FanfoldLegalGerman: return FanfoldLegalGerman;
                case PaperKind.IsoB4: return IsoB4;
                case PaperKind.JapanesePostcard: return JapanesePostcard;
                case PaperKind.Sheet9X11: return Sheet9X11;
                case PaperKind.Sheet10X11: return Sheet10X11;
                case PaperKind.Sheet15X11: return Sheet15X11;
                case PaperKind.EnvelopeInvite: return EnvelopeInvite;
                case PaperKind.LetterExtra: return LetterExtra;
                case PaperKind.LegalExtra: return LegalExtra;
                case PaperKind.TabloidExtra: return TabloidExtra;
                case PaperKind.A4Extra: return A4Extra;
                case PaperKind.LetterTransverse: return LetterTransverse;
                case PaperKind.A4Transverse: return A4Transverse;
                case PaperKind.LetterExtraTransverse: return LetterExtraTransverse;
                case PaperKind.APlus: return APlus;
                case PaperKind.BPlus: return BPlus;
                case PaperKind.LetterPlus: return LetterPlus;
                case PaperKind.A4Plus: return A4Plus;
                case PaperKind.A5Transverse: return A5Transverse;
                case PaperKind.B5Transverse: return B5Transverse;
                case PaperKind.A3Extra: return A3Extra;
                case PaperKind.A5Extra: return A5Extra;
                case PaperKind.B5Extra: return B5Extra;
                case PaperKind.A2: return A2;
                case PaperKind.A3Transverse: return A3Transverse;
                case PaperKind.A3ExtraTransverse: return A3ExtraTransverse;
                case PaperKind.DblJapanesePostcard: return DblJapanesePostcard;
                case PaperKind.A6: return A6;
                case PaperKind.JapaneseEnvelopeKaku2: return JapaneseEnvelopeKaku2;
                case PaperKind.JapaneseEnvelopeKaku3: return JapaneseEnvelopeKaku3;
                case PaperKind.JapaneseEnvelopeChou3: return JapaneseEnvelopeChou3;
                case PaperKind.JapaneseEnvelopeChou4: return JapaneseEnvelopeChou4;
                case PaperKind.LetterRotated: return LetterRotated;
                case PaperKind.A3Rotated: return A3Rotated;
                case PaperKind.A4Rotated: return A4Rotated;
                case PaperKind.A5Rotated: return A5Rotated;
                case PaperKind.B4JisRotated: return B4JisRotated;
                case PaperKind.B5JisRotated: return B5JisRotated;
                case PaperKind.JapanesePostcardRotated: return JapanesePostcardRotated;
                case PaperKind.DblJapanesePostcardRotated: return DblJapanesePostcardRotated;
                case PaperKind.A6Rotated: return A6Rotated;
                case PaperKind.JapaneseEnvelopeKaku2Rotated: return JapaneseEnvelopeKaku2Rotated;
                case PaperKind.JapaneseEnvelopeKaku3Rotated: return JapaneseEnvelopeKaku3Rotated;
                case PaperKind.JapaneseEnvelopeChou3Rotated: return JapaneseEnvelopeChou3Rotated;
                case PaperKind.JapaneseEnvelopeChou4Rotated: return JapaneseEnvelopeChou4Rotated;
                case PaperKind.B6Jis: return B6Jis;
                case PaperKind.B6JisRotated: return B6JisRotated;
                case PaperKind.Sheet12X11: return Sheet12X11;
                case PaperKind.JapaneseEnvelopeYou4: return JapaneseEnvelopeYou4;
                case PaperKind.JapaneseEnvelopeYou4Rotated: return JapaneseEnvelopeYou4Rotated;
                case PaperKind.Prc16k: return Prc16k;
                case PaperKind.Prc32k: return Prc32k;
                case PaperKind.Prc32kBig: return Prc32kBig;
                case PaperKind.PrcEnvelope1: return PrcEnvelope1;
                case PaperKind.PrcEnvelope2: return PrcEnvelope2;
                case PaperKind.PrcEnvelope3: return PrcEnvelope3;
                case PaperKind.PrcEnvelope4: return PrcEnvelope4;
                case PaperKind.PrcEnvelope5: return PrcEnvelope5;
                case PaperKind.PrcEnvelope6: return PrcEnvelope6;
                case PaperKind.PrcEnvelope7: return PrcEnvelope7;
                case PaperKind.PrcEnvelope8: return PrcEnvelope8;
                case PaperKind.PrcEnvelope9: return PrcEnvelope9;
                case PaperKind.PrcEnvelope10: return PrcEnvelope10;
                case PaperKind.Prc16kRotated: return Prc16kRotated;
                case PaperKind.Prc32kRotated: return Prc32kRotated;
                case PaperKind.Prc32kBigRotated: return Prc32kBigRotated;
                case PaperKind.PrcEnvelope1Rotated: return PrcEnvelope1Rotated;
                case PaperKind.PrcEnvelope2Rotated: return PrcEnvelope2Rotated;
                case PaperKind.PrcEnvelope3Rotated: return PrcEnvelope3Rotated;
                case PaperKind.PrcEnvelope4Rotated: return PrcEnvelope4Rotated;
                case PaperKind.PrcEnvelope5Rotated: return PrcEnvelope5Rotated;
                case PaperKind.PrcEnvelope6Rotated: return PrcEnvelope6Rotated;
                case PaperKind.PrcEnvelope7Rotated: return PrcEnvelope7Rotated;
                case PaperKind.PrcEnvelope8Rotated: return PrcEnvelope8Rotated;
                case PaperKind.PrcEnvelope9Rotated: return PrcEnvelope9Rotated;
                case PaperKind.PrcEnvelope10Rotated: return PrcEnvelope10Rotated;
                case PaperKind.A0: return A0;
                case PaperKind.A1: return A1;
                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
}