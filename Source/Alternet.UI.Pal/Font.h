#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Font : public Object
    {
#include "Api/Font.inc"
    public:
        Font(wxFont font);

        wxFont GetWxFont();
        void SetWxFont(wxFont font);
        void SetWxFontInfo(wxFontInfo fontInfo);

        static FontStyle GetFontStyle(wxFont font);
        static wxFont InitializeWxFont(GenericFontFamily genericFamily,
            optional<string> familyName, Coord emSize, FontStyle style);
        static wxFontFamily GetWxFontFamily(GenericFontFamily genericFamily);
    private:
        wxFont _font;
    };
}
