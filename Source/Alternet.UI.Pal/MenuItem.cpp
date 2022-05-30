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

        bool separator = IsSeparator();

        _menuItem = new wxMenuItem(
                nullptr,
                separator ? wxID_SEPARATOR : IdManager::AllocateId(),
                CoerceWxItemText(_text),
                wxEmptyString,
                checked ? wxITEM_CHECK : wxITEM_NORMAL);

        if (!separator)
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
        if (_menuItem != nullptr && _parentMenu != nullptr)
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

        bool checked = _flags.IsSet(MenuItemFlags::Checked);
        if (_menuItem != nullptr && _menuItem->IsCheckable() && _parentMenu != nullptr)
            _menuItem->Check(checked);
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

        if (!IsSeparator())
        {
            auto id = _menuItem->GetId();
            s_itemsByIdsMap.erase(id);
            IdManager::FreeId(id);
        }

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

    bool MenuItem::IsSeparator()
    {
        return _text == u"-";
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
        bool wasSeparator = IsSeparator();
        _text = value;
        _menuItem->SetItemLabel(CoerceWxItemText(value));

        if (wasSeparator != IsSeparator())
        {
            RecreateWxMenuItem();
        }
        else
        {
#ifdef __WXOSX_COCOA__
            RecreateWxMenuItem();
#endif
        }
    }

    void MenuItem::SetParentMenu(Menu* value, optional<int> index)
    {
        _parentMenu = value;
        _indexInParentMenu = index;

        if (value != nullptr)
        {
            _menuItem->Enable(_flags.IsSet(MenuItemFlags::Enabled));

            bool checked = _flags.IsSet(MenuItemFlags::Checked);
            if (_menuItem->IsCheckable())
                _menuItem->Check(checked);
        }
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
