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

        static MenuItem* GetMenuItemById(int id);

        void RaiseClick();

    protected:

        void ShowCore() override;

    private:

        wxMenuItem* _menuItem;

        inline static std::map<int, MenuItem*> s_itemsByIdsMap;
    };
}
