#pragma once
#include "Common.h"
#include "ApiTypes.h"

namespace Alternet::UI
{
    wxPaperSize GetWxPaperSize(PaperKind kind)
    {
        switch (kind)
        {
        case PaperKind::Letter: return wxPAPER_LETTER;
        case PaperKind::Legal: return wxPAPER_LEGAL;
        case PaperKind::A4: return wxPAPER_A4;
        case PaperKind::C: return wxPAPER_CSHEET;
        case PaperKind::D: return wxPAPER_DSHEET;
        case PaperKind::E: return wxPAPER_ESHEET;
        case PaperKind::LetterSmall: return wxPAPER_LETTERSMALL;
        case PaperKind::Tabloid: return wxPAPER_TABLOID;
        case PaperKind::Ledger: return wxPAPER_LEDGER;
        case PaperKind::Statement: return wxPAPER_STATEMENT;
        case PaperKind::Executive: return wxPAPER_EXECUTIVE;
        case PaperKind::A3: return wxPAPER_A3;
        case PaperKind::A4mall: return wxPAPER_A4SMALL;
        case PaperKind::A5: return wxPAPER_A5;
        case PaperKind::B4: return wxPAPER_B4;
        case PaperKind::B5: return wxPAPER_B5;
        case PaperKind::Folio: return wxPAPER_FOLIO;
        case PaperKind::Quarto: return wxPAPER_QUARTO;
        case PaperKind::Sheet10X14: return wxPAPER_10X14;
        case PaperKind::Sheet11X17: return wxPAPER_11X17;
        case PaperKind::Note: return wxPAPER_NOTE;
        case PaperKind::Envelope9: return wxPAPER_ENV_9;
        case PaperKind::Envelope10: return wxPAPER_ENV_10;
        case PaperKind::Envelope11: return wxPAPER_ENV_11;
        case PaperKind::Envelope12: return wxPAPER_ENV_12;
        case PaperKind::Envelope14: return wxPAPER_ENV_14;
        case PaperKind::EnvelopeDl: return wxPAPER_ENV_DL;
        case PaperKind::EnvelopeC5: return wxPAPER_ENV_C5;
        case PaperKind::EnvelopeC3: return wxPAPER_ENV_C3;
        case PaperKind::EnvelopeC4: return wxPAPER_ENV_C4;
        case PaperKind::EnvelopeC6: return wxPAPER_ENV_C6;
        case PaperKind::EnvelopeC65: return wxPAPER_ENV_C65;
        case PaperKind::EnvelopeB4: return wxPAPER_ENV_B4;
        case PaperKind::EnvelopeB5: return wxPAPER_ENV_B5;
        case PaperKind::EnvelopeB6: return wxPAPER_ENV_B6;
        case PaperKind::EnvelopeItaly: return wxPAPER_ENV_ITALY;
        case PaperKind::EnvelopeMonarch: return wxPAPER_ENV_MONARCH;
        case PaperKind::EnvelopePersonal: return wxPAPER_ENV_PERSONAL;
        case PaperKind::FanfoldUs: return wxPAPER_FANFOLD_US;
        case PaperKind::FanfoldStanardGerman: return wxPAPER_FANFOLD_STD_GERMAN;
        case PaperKind::FanfoldLegalGerman: return wxPAPER_FANFOLD_LGL_GERMAN;
        case PaperKind::IsoB4: return wxPAPER_ISO_B4;
        case PaperKind::JapanesePostcard: return wxPAPER_JAPANESE_POSTCARD;
        case PaperKind::Sheet9X11: return wxPAPER_9X11;
        case PaperKind::Sheet10X11: return wxPAPER_10X11;
        case PaperKind::Sheet15X11: return wxPAPER_15X11;
        case PaperKind::EnvelopeInvite: return wxPAPER_ENV_INVITE;
        case PaperKind::LetterExtra: return wxPAPER_LETTER_EXTRA;
        case PaperKind::LegalExtra: return wxPAPER_LEGAL_EXTRA;
        case PaperKind::TabloidExtra: return wxPAPER_TABLOID_EXTRA;
        case PaperKind::A4Extra: return wxPAPER_A4_EXTRA;
        case PaperKind::LetterTransverse: return wxPAPER_LETTER_TRANSVERSE;
        case PaperKind::A4Transverse: return wxPAPER_A4_TRANSVERSE;
        case PaperKind::LetterExtraTransverse: return wxPAPER_LETTER_EXTRA_TRANSVERSE;
        case PaperKind::APlus: return wxPAPER_A_PLUS;
        case PaperKind::BPlus: return wxPAPER_B_PLUS;
        case PaperKind::LetterPlus: return wxPAPER_LETTER_PLUS;
        case PaperKind::A4Plus: return wxPAPER_A4_PLUS;
        case PaperKind::A5Transverse: return wxPAPER_A5_TRANSVERSE;
        case PaperKind::B5Transverse: return wxPAPER_B5_TRANSVERSE;
        case PaperKind::A3Extra: return wxPAPER_A3_EXTRA;
        case PaperKind::A5Extra: return wxPAPER_A5_EXTRA;
        case PaperKind::B5Extra: return wxPAPER_B5_EXTRA;
        case PaperKind::A2: return wxPAPER_A2;
        case PaperKind::A3Transverse: return wxPAPER_A3_TRANSVERSE;
        case PaperKind::A3ExtraTransverse: return wxPAPER_A3_EXTRA_TRANSVERSE;
        case PaperKind::DblJapanesePostcard: return wxPAPER_DBL_JAPANESE_POSTCARD;
        case PaperKind::A6: return wxPAPER_A6;
        case PaperKind::JapaneseEnvelopeKaku2: return wxPAPER_JENV_KAKU2;
        case PaperKind::JapaneseEnvelopeKaku3: return wxPAPER_JENV_KAKU3;
        case PaperKind::JapaneseEnvelopeChou3: return wxPAPER_JENV_CHOU3;
        case PaperKind::JapaneseEnvelopeChou4: return wxPAPER_JENV_CHOU4;
        case PaperKind::LetterRotated: return wxPAPER_LETTER_ROTATED;
        case PaperKind::A3Rotated: return wxPAPER_A3_ROTATED;
        case PaperKind::A4Rotated: return wxPAPER_A4_ROTATED;
        case PaperKind::A5Rotated: return wxPAPER_A5_ROTATED;
        case PaperKind::B4JisRotated: return wxPAPER_B4_JIS_ROTATED;
        case PaperKind::B5JisRotated: return wxPAPER_B5_JIS_ROTATED;
        case PaperKind::JapanesePostcardRotated: return wxPAPER_JAPANESE_POSTCARD_ROTATED;
        case PaperKind::DblJapanesePostcardRotated: return wxPAPER_DBL_JAPANESE_POSTCARD_ROTATED;
        case PaperKind::A6Rotated: return wxPAPER_A6_ROTATED;
        case PaperKind::JapaneseEnvelopeKaku2Rotated: return wxPAPER_JENV_KAKU2_ROTATED;
        case PaperKind::JapaneseEnvelopeKaku3Rotated: return wxPAPER_JENV_KAKU3_ROTATED;
        case PaperKind::JapaneseEnvelopeChou3Rotated: return wxPAPER_JENV_CHOU3_ROTATED;
        case PaperKind::JapaneseEnvelopeChou4Rotated: return wxPAPER_JENV_CHOU4_ROTATED;
        case PaperKind::B6Jis: return wxPAPER_B6_JIS;
        case PaperKind::B6JisRotated: return wxPAPER_B6_JIS_ROTATED;
        case PaperKind::Sheet12X11: return wxPAPER_12X11;
        case PaperKind::JapaneseEnvelopeYou4: return wxPAPER_JENV_YOU4;
        case PaperKind::JapaneseEnvelopeYou4Rotated: return wxPAPER_JENV_YOU4_ROTATED;
        case PaperKind::P16k: return wxPAPER_P16K;
        case PaperKind::P32k: return wxPAPER_P32K;
        case PaperKind::P32kbig: return wxPAPER_P32KBIG;
        case PaperKind::PrcEnvelope1: return wxPAPER_PENV_1;
        case PaperKind::PrcEnvelope2: return wxPAPER_PENV_2;
        case PaperKind::PrcEnvelope3: return wxPAPER_PENV_3;
        case PaperKind::PrcEnvelope4: return wxPAPER_PENV_4;
        case PaperKind::PrcEnvelope5: return wxPAPER_PENV_5;
        case PaperKind::PrcEnvelope6: return wxPAPER_PENV_6;
        case PaperKind::PrcEnvelope7: return wxPAPER_PENV_7;
        case PaperKind::PrcEnvelope8: return wxPAPER_PENV_8;
        case PaperKind::PrcEnvelope9: return wxPAPER_PENV_9;
        case PaperKind::PrcEnvelope10: return wxPAPER_PENV_10;
        case PaperKind::P16kRotated: return wxPAPER_P16K_ROTATED;
        case PaperKind::P32kRotated: return wxPAPER_P32K_ROTATED;
        case PaperKind::P32kbigRotated: return wxPAPER_P32KBIG_ROTATED;
        case PaperKind::PrcEnvelope1Rotated: return wxPAPER_PENV_1_ROTATED;
        case PaperKind::PrcEnvelope2Rotated: return wxPAPER_PENV_2_ROTATED;
        case PaperKind::PrcEnvelope3Rotated: return wxPAPER_PENV_3_ROTATED;
        case PaperKind::PrcEnvelope4Rotated: return wxPAPER_PENV_4_ROTATED;
        case PaperKind::PrcEnvelope5Rotated: return wxPAPER_PENV_5_ROTATED;
        case PaperKind::PrcEnvelope6Rotated: return wxPAPER_PENV_6_ROTATED;
        case PaperKind::PrcEnvelope7Rotated: return wxPAPER_PENV_7_ROTATED;
        case PaperKind::PrcEnvelope8Rotated: return wxPAPER_PENV_8_ROTATED;
        case PaperKind::PrcEnvelope9Rotated: return wxPAPER_PENV_9_ROTATED;
        case PaperKind::PrcEnvelope10Rotated: return wxPAPER_PENV_10_ROTATED;
        case PaperKind::A0: return wxPAPER_A0;
        case PaperKind::A1: return wxPAPER_A1;
        default:
            throwExNoInfo;
        }
    }
}
