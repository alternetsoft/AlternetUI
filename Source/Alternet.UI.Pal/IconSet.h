#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Image.h"

namespace Alternet::UI
{
    class IconSet : public Object
    {
#include "Api/IconSet.inc"
    public:
    
        static wxIconBundle IconBundle(IconSet* iconBundle)
        {
            if (iconBundle == nullptr)
                return wxIconBundle();
            return iconBundle->_iconBundle;
        }
    private:
        wxIconBundle _iconBundle;
    };
}
