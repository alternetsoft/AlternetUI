#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class MenuItem;
    class MainMenu;

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

        void SetParent(MainMenu* mainMenu);
        void SetParent(MenuItem* item);

        MainMenu* GetParentMainMenu();

        MainMenu* FindParentMainMenu();

        std::vector<MenuItem*> GetItems();

        void DetachAndRecreateWxMenu();
    protected:

        void ApplyBounds(const Rect& value) override;
        void ShowCore() override;

    private:

        wxMenu* _menu;
        
        MainMenu* _parentMainMenu = nullptr;
        MenuItem* _parentMenuItem = nullptr;

        std::vector<MenuItem*> _items;

        static MenusByWxMenusMap s_menusByWxMenusMap;

        static void AssociateMenuWithWxMenu(wxMenu* wxMenu, Menu* menu);
        static void RemoveWxMenuAssociation(wxMenu* wxMenu);

    };
}
