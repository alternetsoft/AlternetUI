#include "Menu.h"
#include "MenuItem.h"

namespace Alternet::UI
{
    /*static*/ Menu::MenusByWxMenusMap Menu::s_menusByWxMenusMap;

    Menu::Menu()
    {
        CreateWxMenu();
    }

    Menu::~Menu()
    {
        UnregisterWxMenu();
        delete _menu;
        _menu = nullptr;
    }

    void* Menu::GetMenuHandle()
    {
        return _menu;
    }

    void Menu::CreateWxMenu()
    {
        _menu = new wxMenu();
        AssociateMenuWithWxMenu(_menu, this);
        _menu->Bind(wxEVT_MENU, &Menu::OnMenuCommand, this);
        _menu->Bind(wxEVT_MENU_OPEN, &Menu::OnMenuOpen, this);
        _menu->Bind(wxEVT_MENU_CLOSE, &Menu::OnMenuClose, this);
        _menu->Bind(wxEVT_MENU_HIGHLIGHT, &Menu::OnMenuHighlight, this);
    }

    void Menu::UnregisterWxMenu()
    {
        _menu->Unbind(wxEVT_MENU, &Menu::OnMenuCommand, this);
        _menu->Unbind(wxEVT_MENU_OPEN, &Menu::OnMenuOpen, this);
        _menu->Unbind(wxEVT_MENU_CLOSE, &Menu::OnMenuClose, this);
        _menu->Unbind(wxEVT_MENU_HIGHLIGHT, &Menu::OnMenuHighlight, this);
        RemoveWxMenuAssociation(_menu);
    }

    void Menu::OnMenuOpen(wxMenuEvent& evt)
    {
        evt.StopPropagation();
        RaiseEvent(MenuEvent::Opened);

        for (auto item : _items)
        {
            item->RaiseMenuOpen();
        }
    }

    void Menu::OnMenuClose(wxMenuEvent& evt)
    {
        evt.StopPropagation();
        RaiseEvent(MenuEvent::Closed);

        for (auto item : _items)
        {
            item->RaiseMenuClose();
        }
    }

    void Menu::OnMenuHighlight(wxMenuEvent& evt)
    {
        evt.StopPropagation();
        auto item = MenuItem::GetMenuItemById(evt.GetMenuId());
        if (item == nullptr)
            return;
        item->RaiseMenuHighlight();
    }

    void Menu::OnMenuCommand(wxCommandEvent& event)
    {
        event.StopPropagation();
        auto item = MenuItem::GetMenuItemById(event.GetId());
        if (item == nullptr)
            return;
        item->RaiseClick();
    }

    MainMenu* Menu::GetParentMainMenu()
    {
        return _parentMainMenu;
    }

    void Menu::SetParent(MainMenu* mainMenu)
    {
        _parentMainMenu = mainMenu;
        _parentMenuItem = nullptr;
    }

    void Menu::SetParent(MenuItem* item)
    {
        _parentMenuItem = item;
        _parentMainMenu = nullptr;
    }

    MainMenu* Menu::FindParentMainMenu()
    {
        if (_parentMainMenu != nullptr)
            return _parentMainMenu;

        if (_parentMenuItem == nullptr)
            return nullptr;

        return _parentMenuItem->FindParentMainMenu();
    }

    std::vector<MenuItem*> Menu::GetItems()
    {
        return _items;
    }

    void Menu::DetachAndRecreateWxMenu()
    {
        UnregisterWxMenu();
        auto oldMenu = _menu;

        CreateWxMenu();

        int index = 0;
        for (auto item : _items)
        {
            auto wxItem = item->GetWxMenuItem();
            oldMenu->Remove(wxItem);
            _menu->Insert(index++, wxItem);
        }
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

    void Menu::ShowContextMenu(Control* control, const Point& position)
    {
        auto wxWindow = control->GetWxWindow();
        wxWindow->PopupMenu(_menu, fromDip(position, wxWindow));
    }

    wxMenu* Menu::GetWxMenu()
    {
        return _menu;
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
