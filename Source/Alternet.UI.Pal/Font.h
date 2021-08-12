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

    private:
        wxFont _font;

        static wxFontFamily GetWxFontFamily(GenericFontFamily genericFamily);
    };
}
