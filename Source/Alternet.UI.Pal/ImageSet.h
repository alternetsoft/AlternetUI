#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Image.h"

namespace Alternet::UI
{
    class ImageSet : public Object
    {
#include "Api/ImageSet.inc"
    public:
        wxIconBundle* GetIconBundle();

    private:
        wxIconBundle _bundle;
    };
}
