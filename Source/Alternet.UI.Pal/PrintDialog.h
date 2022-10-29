#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Window;
    class PageSettings;
    class PrintDocument;
    class PrinterSettings;

    class PrintDialog : public Object
    {
#include "Api/PrintDialog.inc"
    public:

    private:
    
    };
}
