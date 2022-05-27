#include "MenuItem.h"
#include "Menu.h"
#include "IdManager.h"

namespace Alternet::UI
{
    MenuItem::MenuItem()
    {
        CreateWxMenuItem();
    }

    MenuItem::~MenuItem()
    {
        DestroyWxMenuItem();
    }

    void MenuItem::CreateWxMenuItem()
    {
        if (_menuItem != nullptr)
            throwExInvalidOp;

        _menuItem = new wxMenuItem(
                nullptr,
                IdManager::AllocateId(),
                CoerceWxItemText(_text));
        s_itemsByIdsMap[_menuItem->GetId()] = this;
    }

    void MenuItem::DestroyWxMenuItem()
    {
        if (_menuItem == nullptr)
            throwExInvalidOp;

        auto id = _menuItem->GetId();
        s_itemsByIdsMap.erase(id);
        IdManager::FreeId(id);

        delete _menuItem;
        _menuItem = nullptr;
    }

    void MenuItem::RecreateWxMenuItem()
    {
        bool wasCreated = _menuItem != nullptr;

        if (wasCreated)
            DestroyWxMenuItem();
        
        CreateWxMenuItem();

        if (wasCreated)
        {
            auto parent = _parentMenu;
            if (parent != nullptr)
            {
                if (!_indexInParentMenu.has_value())
                    throwExInvalidOp;
                auto index = _indexInParentMenu.value();
                parent->RemoveItemAt(index);
                parent->InsertItemAt(index, this);
            }
        }
    }

    void MenuItem::RecreateWxMenuItemIfNeeded()
    {
#ifdef __WXOSX_COCOA__
        RecreateWxMenuItem();
#endif
    }

    /*static*/ wxString MenuItem::CoerceWxItemText(string value)
    {
        // Have to pass space because the validation check does not allow for empty string.
        auto text = value.empty() ? wxString(" ") : wxStr(value);
        text.Replace("_", "&");
        return text;
    }

    string MenuItem::GetText()
    {
        return _text;
    }

    void MenuItem::SetText(const string& value)
    {
        _text = value;
        _menuItem->SetItemLabel(CoerceWxItemText(value));
        RecreateWxMenuItemIfNeeded();
    }

    void MenuItem::SetParentMenu(Menu* value, optional<int> index)
    {
        _parentMenu = value;
        _indexInParentMenu = index;
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
