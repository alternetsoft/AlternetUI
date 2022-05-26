#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class MenuItem;

    class Menu : public Control
    {
#include "Api/Menu.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    protected:
    private:
    };
}
