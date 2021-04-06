#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class Panel : public Control
    {
#include "Api/Panel.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    private:
    
    };
}
