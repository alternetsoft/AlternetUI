#include "MenuItem.h"
#include "Menu.h"
#include "IdManager.h"

namespace Alternet::UI
{
    MenuItem::MenuItem() :
        _menuItem(
            new wxMenuItem(
                nullptr,
                IdManager::AllocateId(),
                " ")) // have to pass space because the validation check does not allow for empty string.
    {
        s_itemsByIdsMap[_menuItem->GetId()] = this;
    }

    MenuItem::~MenuItem()
    {
        auto id = _menuItem->GetId();
        s_itemsByIdsMap.erase(id);
        IdManager::FreeId(id);

        delete _menuItem;
        _menuItem = nullptr;
    }

    string MenuItem::GetText()
    {
        return wxStr(_menuItem->GetItemLabel());
    }

    void MenuItem::SetText(const string& value)
    {
        _menuItem->SetItemLabel(wxStr(value));
    }

    bool MenuItem::GetChecked()
    {
        return _menuItem->IsChecked();
    }

    void MenuItem::SetChecked(bool value)
    {
        if (value)
        {
            if (!_menuItem->IsCheckable())
                _menuItem->SetCheckable(true);
        }

        _menuItem->Check(value);
    }

    Key MenuItem::GetShortcut()
    {
        return Key();
    }

    void MenuItem::SetShortcut(Key value)
    {
    }

    Menu* MenuItem::GetSubmenu()
    {
        auto wxMenu = _menuItem->GetSubMenu();
        if (wxMenu == nullptr)
            return nullptr;

        auto menu = Menu::TryFindMenuByWxMenu(wxMenu);
        if (menu == nullptr)
            throwExInvalidOp;

        menu->AddRef();

        return menu;
    }

    void MenuItem::SetSubmenu(Menu* value)
    {
        _menuItem->SetSubMenu(value->GetWxMenu());
    }

    wxWindow* MenuItem::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxPanel();
    }
    
    wxMenuItem* MenuItem::GetWxMenuItem()
    {
        return _menuItem;
    }

    /*static*/ MenuItem* MenuItem::GetMenuItemById(int id)
    {
        auto it = s_itemsByIdsMap.find(id);
        if (it == s_itemsByIdsMap.end())
            throwEx(u"Cannot find menu item by id");

        return it->second;
    }

    void MenuItem::RaiseClick()
    {
        RaiseEvent(MenuItemEvent::Click);
    }

    void MenuItem::ShowCore()
    {
    }
}
