#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class PrinterSettings;
    class PageSettings;
    class DrawingContext;

    class PrintDocument : public Object
    {
#include "Api/PrintDocument.inc"
    public:

    private:
    
    };
}
