#include "MenuItem.h"
#include "Menu.h"
#include "IdManager.h"

namespace Alternet::UI
{
    MenuItem::MenuItem() : _flags(MenuItemFlags::Enabled)
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

        bool checked = _flags.IsSet(MenuItemFlags::Checked);

        _menuItem = new wxMenuItem(
                nullptr,
                IdManager::AllocateId(),
                CoerceWxItemText(_text),
                wxEmptyString,
                checked ? wxITEM_CHECK : wxITEM_NORMAL);

        _menuItem->Enable(_flags.IsSet(MenuItemFlags::Enabled));
        
        if (checked)
            _menuItem->Check(true);

        s_itemsByIdsMap[_menuItem->GetId()] = this;
    }

    void MenuItem::ApplyBounds(const Rect& value)
    {
    }

    Size MenuItem::SizeToClientSize(const Size& size)
    {
        return size;
    }

    bool MenuItem::GetEnabled()
    {
        return _flags.IsSet(MenuItemFlags::Enabled);
    }

    void MenuItem::SetEnabled(bool value)
    {
        _flags.Set(MenuItemFlags::Enabled, value);
        if (_menuItem != nullptr)
            _menuItem->Enable(value);
    }

    bool MenuItem::GetChecked()
    {
        return _flags.IsSet(MenuItemFlags::Checked);
    }

    void MenuItem::SetChecked(bool value)
    {
        _flags.Set(MenuItemFlags::Checked, value);

        if (value && !_menuItem->IsCheckable())
        {
            RecreateWxMenuItem();
        }

        if (_menuItem->IsCheckable())
            _menuItem->Check(value);
    }


    void MenuItem::DestroyWxMenuItem()
    {
        if (_menuItem == nullptr)
            throwExInvalidOp;

        if (_parentMenu != nullptr)
        {
            _parentMenu->RemoveItemAt(_indexInParentMenu.value());
            _parentMenu = nullptr;
        }

        auto id = _menuItem->GetId();
        s_itemsByIdsMap.erase(id);
        IdManager::FreeId(id);

        delete _menuItem;
        _menuItem = nullptr;
    }

    void MenuItem::RecreateWxMenuItem()
    {
        bool wasCreated = _menuItem != nullptr;

        auto parent = _parentMenu;
        auto index = _indexInParentMenu;
        if (wasCreated)
            DestroyWxMenuItem();
        
        CreateWxMenuItem();

        if (wasCreated)
        {
            if (parent != nullptr)
            {
                if (!index.has_value())
                    throwExInvalidOp;
                parent->InsertItemAt(index.value(), this);
            }
        }
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

#ifdef __WXOSX_COCOA__
        RecreateWxMenuItem();
#endif
    }

    void MenuItem::SetParentMenu(Menu* value, optional<int> index)
    {
        _parentMenu = value;
        _indexInParentMenu = index;
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
