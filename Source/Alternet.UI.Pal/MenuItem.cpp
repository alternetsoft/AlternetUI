#include "MenuItem.h"
#include "MainMenu.h"
#include "Menu.h"
#include "IdManager.h"
#include "Application.h"

namespace Alternet::UI
{
    ImageSet* MenuItem::GetNormalImage()
    {
        if (_normalImage == nullptr)
            return nullptr;

        _normalImage->AddRef();
        return _normalImage;
    }

    void MenuItem::SetNormalImage(ImageSet* value)
    {
       if (value == _normalImage)
			return;

        if (_normalImage != nullptr)
            _normalImage->Release();

        _normalImage = value;

        if (_normalImage != nullptr)
            _normalImage->AddRef();

        if (_menuItem == nullptr)
            return;
        _menuItem->SetBitmap(ImageSet::BitmapBundle(value));
    }

    ImageSet* MenuItem::GetDisabledImage()
    {
        if (_disabledImage == nullptr)
            return nullptr;

        _disabledImage->AddRef();
        return _disabledImage;
    }

    void MenuItem::SetDisabledImage(ImageSet* value)
    {

        /*
		* This is commented out because there are problems with disabled images
		* on Windows when dark mode is enabled. Also disabled image for menu item
		* is only supported on Windows.
         
       if (value == _disabledImage)
            return;

        if (_disabledImage != nullptr)
            _disabledImage->Release();

        _disabledImage = value;

        if (_disabledImage != nullptr)
            _disabledImage->AddRef();

        if (_menuItem == nullptr)
            return;

#ifdef __WXMSW__
        _menuItem->SetDisabledBitmap(ImageSet::BitmapBundle(value));
#endif
        */
    }

    MenuItem::MenuItem() : _flags(MenuItemFlags::Enabled)
    {
        CreateWxMenuItem();
    }

    MenuItem::~MenuItem()
    {
        DestroyAcceleratorIfNeeded();
        DestroyWxMenuItem();
    }

    void MenuItem::CreateWxMenuItem()
    {
        if (_menuItem != nullptr)
            throwExInvalidOpWithInfo(wxStr("MenuItem::CreateWxMenuItem"));
        
        bool checked = _flags.IsSet(MenuItemFlags::Checked);

        bool separator = IsSeparator();

        _menuItem = new wxMenuItem(
                nullptr,
                separator ? wxID_SEPARATOR : IdManager::AllocateId(),
                CoerceWxItemText(_text, this),
                wxEmptyString,
                checked ? wxITEM_CHECK : wxITEM_NORMAL);

        if (!separator)
            s_itemsByIdsMap[_menuItem->GetId()] = this;

        if (_accelerator != nullptr)
            _menuItem->SetAccel(_accelerator);

        if (_normalImage != nullptr)
        {
            _menuItem->SetBitmap(ImageSet::BitmapBundle(_normalImage));
        }

#ifdef __WXMSW__
        if (_disabledImage != nullptr)
        {
            _menuItem->SetDisabledBitmap(ImageSet::BitmapBundle(_disabledImage));
        }
#endif
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

    MainMenu* MenuItem::FindParentMainMenu()
    {
        if (_parentMenu == nullptr)
            return nullptr;

        return _parentMenu->FindParentMainMenu();
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
            throwExInvalidOpWithInfo(wxStr("MenuItem::DestroyWxMenuItem"));
        
        if (_parentMenu != nullptr)
        {
            if (!_roleBasedOverrideData.has_value())
            {
                auto parentWxMenu = _parentMenu->GetWxMenu();
                if (parentWxMenu->GetMenuItemCount() > _indexInParentMenu.value() &&
                    parentWxMenu->FindItemByPosition(_indexInParentMenu.value()) == _menuItem)
                    _parentMenu->RemoveItemAt(_indexInParentMenu.value());
            }

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

    string MenuItem::GetManagedCommandId()
    {
        return _managedCommandId;
    }

    void MenuItem::SetManagedCommandId(const string& value)
    {
        _managedCommandId = value;
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
                    throwExInvalidOpWithInfo(wxStr("MenuItem::RecreateWxMenuItem()"));                
                parent->InsertItemAt(index.value(), this);
            }
        }
    }

    void MenuItem::DestroyAcceleratorIfNeeded()
    {
        if (_accelerator == nullptr)
            return;

        IdManager::FreeId(_accelerator->GetCommand());
        delete _accelerator;
        _accelerator = nullptr;
    }

    bool MenuItem::IsSeparator()
    {
        return _text == u"-";
    }

    /*static*/ wxString MenuItem::CoerceWxItemText(string value, MenuItem* menuItem)
    {
        // Have to pass space because the validation check does not allow
        // for empty string.
        auto text = value.empty() ? wxString(" ") : wxStr(value);
        text.Replace("_", "&");
        
        if (menuItem != nullptr)
        {
            auto accelerator = menuItem->_accelerator;
            if (accelerator != nullptr)
                text += "\t" + accelerator->ToRawString();
        }

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
        _menuItem->SetItemLabel(CoerceWxItemText(value, this));

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

    string MenuItem::GetRole()
    {
        return _role;
    }

    void MenuItem::SetRole(const string& value)
    {
        if (_role == value)
            return;

        _role = value;

        auto mainMenu = FindParentMainMenu();
        if (mainMenu == nullptr)
            return;

        if (mainMenu->IsInitInProgress())
            return;

        mainMenu->OnItemRoleChanged(this);
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

    void MenuItem::SetRoleBasedOverrideData(optional<MenuItem::RoleBasedOverrideData> value)
    {
        _roleBasedOverrideData = value;
    }

    optional<MenuItem::RoleBasedOverrideData> MenuItem::GetRoleBasedOverrideData()
    {
        return _roleBasedOverrideData;
    }

    Menu* MenuItem::GetParentMenu()
    {
        return _parentMenu;
    }

    Key MenuItem::GetShortcutKey()
    {
        return _shortcutKey;
    }

    ModifierKeys MenuItem::GetShortcutModifierKeys()
    {
        return _shortcutModifierKeys;
    }

    void MenuItem::SetShortcut(Key key, ModifierKeys modifierKeys)
    {
        DestroyAcceleratorIfNeeded();

        _shortcutKey = key;
        _shortcutModifierKeys = modifierKeys;

        if (key == Key::None)
        {
            _menuItem->SetAccel(nullptr);
        }
        else
        {
            auto keyboard = Application::GetCurrent()->GetKeyboardInternal();
            auto wxKey = keyboard->KeyToWxKey(key);
            auto acceleratorFlags = keyboard->ModifierKeysToAcceleratorFlags(modifierKeys);

            _accelerator = new wxAcceleratorEntry(acceleratorFlags, wxKey, IdManager::AllocateId());

            _menuItem->SetAccel(_accelerator);
        }

#ifdef __WXOSX_COCOA__
            RecreateWxMenuItem();
#endif
    }

    Menu* MenuItem::GetSubmenu()
    {
        auto wxMenu = _menuItem->GetSubMenu();
        if (wxMenu == nullptr)
            return nullptr;

        auto menu = Menu::TryFindMenuByWxMenu(wxMenu);
        if (menu == nullptr)
            throwExInvalidOpWithInfo(wxStr("MenuItem::GetSubmenu"));        

        menu->AddRef();

        return menu;
    }

    void MenuItem::SetSubmenu(Menu* value)
    {
        value->SetParent(this);
        _menuItem->SetSubMenu(value->GetWxMenu());
    }

    wxMenuItem* MenuItem::GetWxMenuItem()
    {
        return _menuItem;
    }

    /*static*/ MenuItem* MenuItem::GetMenuItemById(int id)
    {
        auto it = s_itemsByIdsMap.find(id);
        if (it == s_itemsByIdsMap.end())
            return nullptr;
            // throwEx(u"Cannot find menu item by id");

        return it->second;
    }

    void MenuItem::RaiseMenuOpen()
    {
        RaiseEvent(MenuItemEvent::Opened);
    }

    void MenuItem::RaiseMenuClose()
    {
        RaiseEvent(MenuItemEvent::Closed);
    }

    void MenuItem::RaiseMenuHighlight()
    {
        RaiseEvent(MenuItemEvent::Highlight);
    }

    void MenuItem::RaiseClick()
    {
        if (_menuItem->IsCheckable())
            _flags.Set(MenuItemFlags::Checked, !_flags.IsSet(MenuItemFlags::Checked));

        RaiseEvent(MenuItemEvent::Click);
    }
}
