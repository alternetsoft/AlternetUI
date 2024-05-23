#pragma warning disable
using ApiCommon;

namespace NativeApi.Api
{
    [ManagedExternName("Alternet.Drawing.Printing.PaperKind")]
    [ManagedName("Alternet.Drawing.Printing.PaperKind")]
    public enum PaperKind
    {
        /// <summary>
        /// Letter, 8 1/2 by 11 inches.
        /// </summary>
        Letter,

        /// <summary>
        /// Legal, 8 1/2 by 14 inches.
        /// </summary>
        Legal,

        /// <summary>
        /// A4 Sheet, 210 by 297 millimeters.
        /// </summary>
        A4,

        /// <summary>
        /// C Sheet, 17 by 22 inches.
        /// </summary>
        C,

        /// <summary>
        /// D Sheet, 22 by 34 inches.
        /// </summary>
        D,

        /// <summary>
        /// E Sheet, 34 by 44 inches.
        /// </summary>
        E,

        /// <summary>
        /// Letter Small, 8 1/2 by 11 inches.
        /// </summary>
        LetterSmall,

        /// <summary>
        /// Tabloid, 11 by 17 inches.
        /// </summary>
        Tabloid,

        /// <summary>
        /// Ledger, 17 by 11 inches.
        /// </summary>
        Ledger,

        /// <summary>
        /// Statement, 5 1/2 by 8 1/2 inches.
        /// </summary>
        Statement,

        /// <summary>
        /// Executive, 7 1/4 by 10 1/2 inches.
        /// </summary>
        Executive,

        /// <summary>
        /// A3 sheet, 297 by 420 millimeters.
        /// </summary>
        A3,

        /// <summary>
        /// A4 small sheet, 210 by 297 millimeters.
        /// </summary>
        A4mall,

        /// <summary>
        /// A5 sheet, 148 by 210 millimeters.
        /// </summary>
        A5,

        /// <summary>
        /// B4 sheet, 250 by 354 millimeters.
        /// </summary>
        B4,

        /// <summary>
        /// B5 sheet, 182-by-257-millimeter paper.
        /// </summary>
        B5,

        /// <summary>
        /// Folio, 8-1/2-by-13-inch paper.
        /// </summary>
        Folio,

        /// <summary>
        /// Quarto, 215-by-275-millimeter paper.
        /// </summary>
        Quarto,

        /// <summary>
        /// 10-by-14-inch sheet.
        /// </summary>
        Sheet10X14,

        /// <summary>
        /// 11-by-17-inch sheet.
        /// </summary>
        Sheet11X17,

        /// <summary>
        /// Note, 8 1/2 by 11 inches.
        /// </summary>
        Note,

        /// <summary>
        /// #9 Envelope, 3 7/8 by 8 7/8 inches.
        /// </summary>
        Envelope9,

        /// <summary>
        /// #10 Envelope, 4 1/8 by 9 1/2 inches.
        /// </summary>
        Envelope10,

        /// <summary>
        /// #11 Envelope, 4 1/2 by 10 3/8 inches.
        /// </summary>
        Envelope11,

        /// <summary>
        /// #12 Envelope, 4 3/4 by 11 inches.
        /// </summary>
        Envelope12,

        /// <summary>
        /// #14 Envelope, 5 by 11 1/2 inches.
        /// </summary>
        Envelope14,

        /// <summary>
        /// DL Envelope, 110 by 220 millimeters.
        /// </summary>
        EnvelopeDl,

        /// <summary>
        /// C5 Envelope, 162 by 229 millimeters.
        /// </summary>
        EnvelopeC5,

        /// <summary>
        /// C3 Envelope, 324 by 458 millimeters.
        /// </summary>
        EnvelopeC3,

        /// <summary>
        /// C4 Envelope, 229 by 324 millimeters.
        /// </summary>
        EnvelopeC4,

        /// <summary>
        /// C6 Envelope, 114 by 162 millimeters.
        /// </summary>
        EnvelopeC6,

        /// <summary>
        /// C65 Envelope, 114 by 229 millimeters.
        /// </summary>
        EnvelopeC65,

        /// <summary>
        /// B4 Envelope, 250 by 353 millimeters.
        /// </summary>
        EnvelopeB4,

        /// <summary>
        /// B5 Envelope, 176 by 250 millimeters.
        /// </summary>
        EnvelopeB5,

        /// <summary>
        /// B6 Envelope, 176 by 125 millimeters.
        /// </summary>
        EnvelopeB6,

        /// <summary>
        /// Italy Envelope, 110 by 230 millimeters.
        /// </summary>
        EnvelopeItaly,

        /// <summary>
        /// Monarch Envelope, 3 7/8 by 7 1/2 inches.
        /// </summary>
        EnvelopeMonarch,

        /// <summary>
        /// 6 3/4 Envelope, 3 5/8 by 6 1/2 inches.
        /// </summary>
        EnvelopePersonal,

        /// <summary>
        /// US Stanard Fanfold, 14 7/8 by 11 inches.
        /// </summary>
        FanfoldUs,

        /// <summary>
        /// German Stanard Fanfold, 8 1/2 by 12 inches.
        /// </summary>
        FanfoldStanardGerman,

        /// <summary>
        /// German Legal Fanfold, 8 1/2 by 13 inches.
        /// </summary>
        FanfoldLegalGerman,

        /// <summary>
        /// B4 (ISO) 250 x 353 mm.
        /// </summary>
        IsoB4,

        /// <summary>
        /// Japanese Postcard 100 x 148 mm.
        /// </summary>
        JapanesePostcard,

        /// <summary>
        /// 9 x 11 in.
        /// </summary>
        Sheet9X11,

        /// <summary>
        /// 10 x 11 in.
        /// </summary>
        Sheet10X11,

        /// <summary>
        /// 15 x 11 in.
        /// </summary>
        Sheet15X11,

        /// <summary>
        /// Envelope Invite 220 x 220 mm.
        /// </summary>
        EnvelopeInvite,

        /// <summary>
        /// Letter Extra 9 \275 x 12 in.
        /// </summary>
        LetterExtra,

        /// <summary>
        /// Legal Extra 9 \275 x 15 in.
        /// </summary>
        LegalExtra,

        /// <summary>
        /// Tabloid Extra 11.69 x 18 in.
        /// </summary>
        TabloidExtra,

        /// <summary>
        /// A4 Extra 9.27 x 12.69 in.
        /// </summary>
        A4Extra,

        /// <summary>
        /// Letter Transverse 8 \275 x 11 in.
        /// </summary>
        LetterTransverse,

        /// <summary>
        /// A4 Transverse 210 x 297 mm.
        /// </summary>
        A4Transverse,

        /// <summary>
        /// Letter Extra Transverse 9\275 x 12 in.
        /// </summary>
        LetterExtraTransverse,

        /// <summary>
        /// SuperA/SuperA/A4 227 x 356 mm.
        /// </summary>
        APlus,

        /// <summary>
        /// SuperB/SuperB/A3 305 x 487 mm.
        /// </summary>
        BPlus,

        /// <summary>
        /// Letter Plus 8.5 x 12.69 in.
        /// </summary>
        LetterPlus,

        /// <summary>
        /// A4 Plus 210 x 330 mm.
        /// </summary>
        A4Plus,

        /// <summary>
        /// A5 Transverse 148 x 210 mm.
        /// </summary>
        A5Transverse,

        /// <summary>
        /// B5 (JIS) Transverse 182 x 257 mm.
        /// </summary>
        B5Transverse,

        /// <summary>
        /// A3 Extra 322 x 445 mm.
        /// </summary>
        A3Extra,

        /// <summary>
        /// A5 Extra 174 x 235 mm.
        /// </summary>
        A5Extra,

        /// <summary>
        /// B5 (ISO) Extra 201 x 276 mm.
        /// </summary>
        B5Extra,

        /// <summary>
        /// A2 420 x 594 mm.
        /// </summary>
        A2,

        /// <summary>
        /// A3 Transverse 297 x 420 mm.
        /// </summary>
        A3Transverse,

        /// <summary>
        /// A3 Extra Transverse 322 x 445 mm.
        /// </summary>
        A3ExtraTransverse,

        /// <summary>
        /// Japanese Double Postcard 200 x 148 mm.
        /// </summary>
        DblJapanesePostcard,

        /// <summary>
        /// A6 105 x 148 mm.
        /// </summary>
        A6,

        /// <summary>
        /// Japanese Envelope Kaku #2.
        /// </summary>
        JapaneseEnvelopeKaku2,

        /// <summary>
        /// Japanese Envelope Kaku #3.
        /// </summary>
        JapaneseEnvelopeKaku3,

        /// <summary>
        /// Japanese Envelope Chou #3.
        /// </summary>
        JapaneseEnvelopeChou3,

        /// <summary>
        /// Japanese Envelope Chou #4.
        /// </summary>
        JapaneseEnvelopeChou4,

        /// <summary>
        /// Letter Rotated 11 x 8 1/2 in.
        /// </summary>
        LetterRotated,

        /// <summary>
        /// A3 Rotated 420 x 297 mm.
        /// </summary>
        A3Rotated,

        /// <summary>
        /// A4 Rotated 297 x 210 mm.
        /// </summary>
        A4Rotated,

        /// <summary>
        /// A5 Rotated 210 x 148 mm.
        /// </summary>
        A5Rotated,

        /// <summary>
        /// B4 (JIS) Rotated 364 x 257 mm.
        /// </summary>
        B4JisRotated,

        /// <summary>
        /// B5 (JIS) Rotated 257 x 182 mm.
        /// </summary>
        B5JisRotated,

        /// <summary>
        /// Japanese Postcard Rotated 148 x 100 mm.
        /// </summary>
        JapanesePostcardRotated,

        /// <summary>
        /// Double Japanese Postcard Rotated 148 x 200 mm.
        /// </summary>
        DblJapanesePostcardRotated,

        /// <summary>
        /// A6 Rotated 148 x 105 mm.
        /// </summary>
        A6Rotated,

        /// <summary>
        /// Japanese Envelope Kaku #2 Rotated.
        /// </summary>
        JapaneseEnvelopeKaku2Rotated,

        /// <summary>
        /// Japanese Envelope Kaku #3 Rotated.
        /// </summary>
        JapaneseEnvelopeKaku3Rotated,

        /// <summary>
        /// Japanese Envelope Chou #3 Rotated.
        /// </summary>
        JapaneseEnvelopeChou3Rotated,

        /// <summary>
        /// Japanese Envelope Chou #4 Rotated.
        /// </summary>
        JapaneseEnvelopeChou4Rotated,

        /// <summary>
        /// B6 (JIS) 128 x 182 mm.
        /// </summary>
        B6Jis,

        /// <summary>
        /// B6 (JIS) Rotated 182 x 128 mm.
        /// </summary>
        B6JisRotated,

        /// <summary>
        /// 12 x 11 in.
        /// </summary>
        Sheet12X11,

        /// <summary>
        /// Japanese Envelope You #4.
        /// </summary>
        JapaneseEnvelopeYou4,

        /// <summary>
        /// Japanese Envelope You #4 Rotated.
        /// </summary>
        JapaneseEnvelopeYou4Rotated,

        /// <summary>
        /// PRC 16K 146 x 215 mm.
        /// </summary>
        P16k,

        /// <summary>
        /// PRC 32K 97 x 151 mm.
        /// </summary>
        P32k,

        /// <summary>
        /// PRC 32K(Big) 97 x 151 mm.
        /// </summary>
        P32kbig,

        /// <summary>
        /// PRC Envelope #1 102 x 165 mm.
        /// </summary>
        PrcEnvelope1,

        /// <summary>
        /// PRC Envelope #2 102 x 176 mm.
        /// </summary>
        PrcEnvelope2,

        /// <summary>
        /// PRC Envelope #3 125 x 176 mm.
        /// </summary>
        PrcEnvelope3,

        /// <summary>
        /// PRC Envelope #4 110 x 208 mm.
        /// </summary>
        PrcEnvelope4,

        /// <summary>
        /// PRC Envelope #5 110 x 220 mm.
        /// </summary>
        PrcEnvelope5,

        /// <summary>
        /// PRC Envelope #6 120 x 230 mm.
        /// </summary>
        PrcEnvelope6,

        /// <summary>
        /// PRC Envelope #7 160 x 230 mm.
        /// </summary>
        PrcEnvelope7,

        /// <summary>
        /// PRC Envelope #8 120 x 309 mm.
        /// </summary>
        PrcEnvelope8,

        /// <summary>
        /// PRC Envelope #9 229 x 324 mm.
        /// </summary>
        PrcEnvelope9,

        /// <summary>
        /// PRC Envelope #10 324 x 458 mm.
        /// </summary>
        PrcEnvelope10,

        /// <summary>
        /// PRC 16K Rotated.
        /// </summary>
        P16kRotated,

        /// <summary>
        /// PRC 32K Rotated.
        /// </summary>
        P32kRotated,

        /// <summary>
        /// PRC 32K(Big) Rotated.
        /// </summary>
        P32kbigRotated,

        /// <summary>
        /// PRC Envelope #1 Rotated 165 x 102 mm.
        /// </summary>
        PrcEnvelope1Rotated,

        /// <summary>
        /// PRC Envelope #2 Rotated 176 x 102 mm.
        /// </summary>
        PrcEnvelope2Rotated,

        /// <summary>
        /// PRC Envelope #3 Rotated 176 x 125 mm.
        /// </summary>
        PrcEnvelope3Rotated,

        /// <summary>
        /// PRC Envelope #4 Rotated 208 x 110 mm.
        /// </summary>
        PrcEnvelope4Rotated,

        /// <summary>
        /// PRC Envelope #5 Rotated 220 x 110 mm.
        /// </summary>
        PrcEnvelope5Rotated,

        /// <summary>
        /// PRC Envelope #6 Rotated 230 x 120 mm.
        /// </summary>
        PrcEnvelope6Rotated,

        /// <summary>
        /// PRC Envelope #7 Rotated 230 x 160 mm.
        /// </summary>
        PrcEnvelope7Rotated,

        /// <summary>
        /// PRC Envelope #8 Rotated 309 x 120 mm.
        /// </summary>
        PrcEnvelope8Rotated,

        /// <summary>
        /// PRC Envelope #9 Rotated 324 x 229 mm.
        /// </summary>
        PrcEnvelope9Rotated,

        /// <summary>
        /// PRC Envelope #10 Rotated 458 x 324 m.
        /// </summary>
        PrcEnvelope10Rotated,

        /// <summary>
        /// A0 Sheet 841 x 1189 mm.
        /// </summary>
        A0,

        /// <summary>
        /// A1 Sheet 594 x 841 mm.
        /// </summary>
        A1,
    }
}