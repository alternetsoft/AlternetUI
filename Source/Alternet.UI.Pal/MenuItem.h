#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class Menu;

    class MenuItem : public Control
    {
#include "Api/MenuItem.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        wxMenuItem* GetWxMenuItem();

    protected:

        void ShowCore() override;

    private:

        wxMenuItem* _menuItem;
    };
}
