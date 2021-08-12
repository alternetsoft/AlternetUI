#pragma once
#include "Common.h"
#include "ApiTypes.h"

namespace Alternet::UI
{
    class Font
    {
#include "Api/Font.inc"
    public:
        wxFont GetWxFont();

    private:
        wxFont _font;

        static wxFontFamily GetWxFontFamily(GenericFontFamily genericFamily);
    };
}
