#include "Menu.h"
#include "MenuItem.h"

namespace Alternet::UI
{
    /*static*/ Menu::MenusByWxMenusMap Menu::s_menusByWxMenusMap;

    Menu::Menu() : _menu(new wxMenu())
    {
        AssociateMenuWithWxMenu(_menu, this);
    }

    Menu::~Menu()
    {
        RemoveWxMenuAssociation(_menu);
        delete _menu;
        _menu = nullptr;
    }

    int Menu::GetItemsCount()
    {
        return _menu->GetMenuItemCount();
    }

    void Menu::InsertItemAt(int index, MenuItem* item)
    {
        _menu->Insert(index, item->GetWxMenuItem());
    }

    void Menu::RemoveItemAt(int index)
    {
        _menu->Remove(_menu->GetMenuItems()[index]);
    }

    wxWindow* Menu::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxPanel();
    }
    
    wxMenu* Menu::GetWxMenu()
    {
        return _menu;
    }

    void Menu::ShowCore()
    {
    }

    /*static*/ Menu* Menu::TryFindMenuByWxMenu(wxMenu* wxMenu)
    {
        MenusByWxMenusMap::const_iterator i = s_menusByWxMenusMap.find(wxMenu);
        return i == s_menusByWxMenusMap.end() ? NULL : i->second;
    }

    /*static*/ void Menu::AssociateMenuWithWxMenu(wxMenu* wxMenu, Menu* menu)
    {
        s_menusByWxMenusMap[wxMenu] = menu;
    }
    
    /*static*/ void Menu::RemoveWxMenuAssociation(wxMenu* wxMenu)
    {
        s_menusByWxMenusMap.erase(wxMenu);
    }
}
