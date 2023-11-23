#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Font.h"

#include <wx/richtext/richtextctrl.h>

namespace Alternet::UI
{
    class TextBoxTextAttr : public Object
    {
#include "Api/TextBoxTextAttr.inc"
    public:
    private:
        static inline wxTextAttr* Attr(void* attr);
        static inline wxRichTextAttr* RichAttr(void* attr);
    };
}
