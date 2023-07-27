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
        wxFont GetWxFont();
        void SetWxFont(wxFont font);
        void SetWxFontInfo(wxFontInfo fontInfo);

    private:
        wxFont _font;

        static wxFontFamily GetWxFontFamily(GenericFontFamily genericFamily);
    };
}
