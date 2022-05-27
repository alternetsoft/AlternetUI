#include "Menu.h"
#include "MenuItem.h"

namespace Alternet::UI
{
    /*static*/ Menu::MenusByWxMenusMap Menu::s_menusByWxMenusMap;

    Menu::Menu() : _menu(new wxMenu())
    {
        AssociateMenuWithWxMenu(_menu, this);

        _menu->Bind(wxEVT_MENU, &Menu::OnMenuCommand, this);
    }

    Menu::~Menu()
    {
        _menu->Unbind(wxEVT_MENU, &Menu::OnMenuCommand, this);

        RemoveWxMenuAssociation(_menu);
        delete _menu;
        _menu = nullptr;
    }

    void Menu::OnMenuCommand(wxCommandEvent& evt)
    {
        auto item = MenuItem::GetMenuItemById(evt.GetId());
        item->RaiseClick();
    }

    void Menu::ApplyBounds(const Rect& value)
    {
    }

    int Menu::GetItemsCount()
    {
        return _menu->GetMenuItemCount();
    }

    void Menu::InsertItemAt(int index, MenuItem* item)
    {
        _items.insert(_items.begin() + index, item);
        _menu->Insert(index, item->GetWxMenuItem());
        item->SetParentMenu(this, index);
    }

    void Menu::RemoveItemAt(int index)
    {
        auto it = _items.begin() + index;
        auto item = *it;
        _items.erase(it);
        _menu->Remove(item->GetWxMenuItem());
        item->SetParentMenu(nullptr, nullopt);
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
