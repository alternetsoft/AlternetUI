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

        wxMenu* GetWxMenu();

        static Menu* TryFindMenuByWxMenu(wxMenu* wxMenu);

        WX_DECLARE_HASH_MAP(wxMenu*, Menu*,
            wxPointerHash, wxPointerEqual,
            MenusByWxMenusMap);

        void OnMenuCommand(wxCommandEvent& evt);

    protected:

        void ShowCore() override;

    private:

        wxMenu* _menu;


        static MenusByWxMenusMap s_menusByWxMenusMap;

        static void AssociateMenuWithWxMenu(wxMenu* wxMenu, Menu* menu);
        static void RemoveWxMenuAssociation(wxMenu* wxMenu);

    };
}
