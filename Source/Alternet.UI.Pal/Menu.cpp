#include "Menu.h"
#include "IdManager.h"
#include "Application.h"

namespace Alternet::UI
{
    void wxAlternetMenu::OnMenuCommand(wxCommandEvent& event)
    {
        int id = event.GetId();
        wxMenuItem* wxItem = FindItem(id);

        if (wxItem == nullptr)
            return;

        wxAlternetMenuItem* item = wxDynamicCast(wxItem, wxAlternetMenuItem);
        if (item != nullptr)
        {
            Menu::_eventMenuItemId = item->_id;
            Menu::RaiseStaticEvent(Menu::MenuEvent::MenuClick);
        }
    }

    void wxAlternetMenu::OnMenuOpen(wxMenuEvent& event)
    {
        RaiseMenuEvent(event, Menu::MenuEvent::MenuOpened);
    }

    void wxAlternetMenu::RaiseMenuEvent(wxMenuEvent& event, Menu::MenuEvent eventType)
    {
        event.StopPropagation();

        wxAlternetMenu* itemMenu = wxDynamicCast(event.GetMenu(), wxAlternetMenu);
        if (itemMenu != nullptr)
        {
            Menu::_eventMenuItemId = itemMenu->_id;
            Menu::RaiseStaticEvent(eventType);
            return;
        }
    }

    void wxAlternetMenu::RaiseMenuItemEvent(wxMenuEvent& event, Menu::MenuEvent eventType)
    {
        event.StopPropagation();

        wxAlternetMenuItem* item = wxDynamicCast(event.GetMenuItem(), wxAlternetMenuItem);
        if (item != nullptr)
        {
            Menu::_eventMenuItemId = item->_id;
            Menu::RaiseStaticEvent(eventType);
            return;
        }
    }

    void wxAlternetMenu::OnMenuClose(wxMenuEvent& event)
    {
        RaiseMenuEvent(event, Menu::MenuEvent::MenuClosed);
    }

    void wxAlternetMenu::OnMenuHighlight(wxMenuEvent& event)
    {
        RaiseMenuItemEvent(event, Menu::MenuEvent::MenuHighlight);
    }

    void Menu::Show(void* menuHandle, Control* control, const PointD& position)
    {
        auto item = (wxAlternetMenu*)menuHandle;
        auto wxWindow = control->GetWxWindow();

        wxWindow->PopupMenu(item, fromDip(position, wxWindow));
    }

    string Menu::_eventMenuItemId = wxStr("");

    string Menu::GetEventMenuItemId()
    {
        return _eventMenuItemId;
    }

    string Menu::GetMenuId(void* handle)
    {
        wxObject* obj = static_cast<wxObject*>(handle);

        if (wxAlternetMenuItem* item = wxDynamicCast(obj, wxAlternetMenuItem))
        {
			return item->_id;
        }
        else
        if (wxAlternetMenu* menu = wxDynamicCast(obj, wxAlternetMenu))
        {
			return menu->_id;
        }
        else
        if (wxAlternetMenuBar* bar = wxDynamicCast(obj, wxAlternetMenuBar))
        {
			return bar->_id;
        }
        else
        {
			return wxStr("");
        }
    }

    void* Menu::CreateContextMenu(const string& id)
    {
        auto result = new wxAlternetMenu();
		result->_id = id;

		return result;
    }

    void* Menu::CreateMainMenu(const string& id)
    {
        auto result = new wxAlternetMenuBar();
        result->_id = id;
        return result;
    }

    void* Menu::CreateMenuItem(MenuItemType itemType, const string& id)
    {
        wxItemKind kind;
        switch (itemType)
        {
        case MenuItemType::Standard:
        default:
            kind = wxITEM_NORMAL;
            break;
        case MenuItemType::Check:
            kind = wxITEM_CHECK;
            break;
        case MenuItemType::Radio:
            kind = wxITEM_RADIO;
            break;
        case MenuItemType::Separator:
            kind = wxITEM_SEPARATOR;
            break;
        }

        auto result = new wxAlternetMenuItem(
            nullptr,
            kind == wxITEM_SEPARATOR ? wxID_SEPARATOR : IdManager::AllocateId(),
            " ",
            wxEmptyString,
            kind,
            nullptr);
        result->_id = id;
        return result;
    }

    void Menu::DestroyMainMenu(void* menuHandle)
    {
		auto item = (wxAlternetMenuBar*)menuHandle;

        if (item->ownerFrame != nullptr)
            return;

        delete item;
    }

    void Menu::DestroyMenuItem(void* menuHandle)
    {
		auto item = (wxAlternetMenuItem*)menuHandle;

        if (item->ownerMenu != nullptr)
            return;

		delete item;
    }

    void Menu::DestroyContextMenu(void* menuHandle)
    {
		auto item = (wxAlternetMenu*)menuHandle;

        if (item->ownerMenuItem != nullptr)
			return;
        if (item->ownerMenuBar != nullptr)
            return;

		delete item;
    }

    MenuItemType Menu::GetMenuItemType(void* handle)
    {
        auto item = (wxAlternetMenuItem*)handle;
        if (item == nullptr)
            return MenuItemType::Standard;

        switch (item->GetKind())
        {
        case wxITEM_NORMAL:
            return MenuItemType::Standard;
        case wxITEM_CHECK:
            return MenuItemType::Check;
        case wxITEM_RADIO:
            return MenuItemType::Radio;
        case wxITEM_SEPARATOR:
            return MenuItemType::Separator;
        default:
            return MenuItemType::Standard;
        }
    }

    void Menu::SetMenuItemBitmap(void* handle, ImageSet* value)
    {
        auto item = (wxAlternetMenuItem*)handle;

        if (value == item->_normalImage)
            return;

        if (item->_normalImage != nullptr)
            item->_normalImage->Release();

        item->_normalImage = value;

        if (item->_normalImage != nullptr)
            item->_normalImage->AddRef();

        item->SetBitmap(ImageSet::BitmapBundle(value));
    }

    void Menu::SetMenuItemEnabled(void* handle, bool value)
    {
        auto item = (wxAlternetMenuItem*)handle;
		item->Enable(value);
    }

    void Menu::SetMenuItemShortcut(void* handle, Key key, ModifierKeys modifierKeys)
    {
        auto item = (wxAlternetMenuItem*)handle;

        if (key == Key::None)
        {
            item->SetAccel(nullptr);
        }
        else
        {
            auto keyboard = Application::GetCurrent()->GetKeyboardInternal();
            auto wxKey = keyboard->KeyToWxKey(key);
            auto acceleratorFlags = keyboard->ModifierKeysToAcceleratorFlags(modifierKeys);

            auto accel = new wxAcceleratorEntry(acceleratorFlags, wxKey, item->GetId());
            item->SetAccel(accel);
        }
    }

    wxString Menu::CoerceMenuText(const string& value)
    {
        auto text = value.empty() ? wxString(" ") : wxStr(value);
        text.Replace("_", "&");
        return text;
    }

    void Menu::SetMenuItemText(void* handle, const string& value, const string& rightValue)
    {
        auto item = (wxAlternetMenuItem*)handle;

        auto text = CoerceMenuText(value);

        if(rightValue.empty())
        {
            item->SetItemLabel(text);
        }
        else
        {
            auto labelText = text + "\t" + wxStr(rightValue);
            item->SetItemLabel(labelText);
        }
    }

    void Menu::SetMenuItemChecked(void* handle, bool value)
    {
        auto item = (wxAlternetMenuItem*)handle;
		if (item->IsCheckable())
		    item->Check(value);
    }

    void Menu::SetMenuItemSubMenu(void* handle, void* subMenuHandle)
    {
        auto item = (wxAlternetMenuItem*)handle;
		auto subMenu = (wxAlternetMenu*)subMenuHandle;

		auto oldValue = dynamic_cast<wxAlternetMenu*>(item->GetSubMenu());

        if(oldValue == subMenu)
			return;

        if (oldValue != nullptr)
        {
            oldValue->ownerMenuItem = nullptr;
        }

        item->SetSubMenu(subMenu);
        
        if (subMenu != nullptr)
        {
            subMenu->ownerMenuItem = item;
		}
    }

    void Menu::MenuAddItem(void* handle, void* itemHandle)
    {
        auto menu = (wxAlternetMenu*)handle;
        auto newItem = (wxAlternetMenuItem*)itemHandle;

        if (menu == nullptr)
			return;

        if (newItem->ownerMenu != nullptr)
			return;

        menu->Append(newItem);
		newItem->ownerMenu = menu;
    }

    void Menu::MenuRemoveItem(void* handle, void* itemHandle)
    {
        auto menu = (wxAlternetMenu*)handle;
        auto newItem = (wxAlternetMenuItem*)itemHandle;
        if (menu == nullptr)
            return;
        if( newItem->ownerMenu != menu)
			return;

        menu->Remove(newItem);
		newItem->ownerMenu = nullptr;
    }

    Menu::Menu()
    {
    }

    Menu::~Menu()
    {
    }

    bool Menu::MainMenuAppend(void* menuHandle, void* menu, const string& text)
    {
        auto menuBar = (wxAlternetMenuBar*)menuHandle;
		auto alternetMenu = (wxAlternetMenu*)menu;
        auto coercedText = CoerceMenuText(text);
        auto result = menuBar->Append(alternetMenu, coercedText);
        if (!result)
			return false;
        alternetMenu->ownerMenuBar = menuBar;
		return true;
    }

    int Menu::MainMenuGetCount(void* menuHandle)
    {
        auto menuBar = (wxAlternetMenuBar*)menuHandle;
		return menuBar->GetMenuCount();
    }

    void Menu::MainMenuSetEnabled(void* menuHandle, int pos, bool enable)
    {
        auto menuBar = (wxAlternetMenuBar*)menuHandle;
		menuBar->EnableTop(pos, enable);
    }

    void* Menu::MainMenuGetSubMenu(void* menuHandle, int menuIndex)
    {
        auto menuBar = (wxAlternetMenuBar*)menuHandle;
		auto result = menuBar->GetMenu(menuIndex);
		return result;
    }

    void* Menu::MainMenuRemove(void* menuHandle, int pos)
    {
        auto menuBar = (wxAlternetMenuBar*)menuHandle;
		auto oldSubMenu = menuBar->Remove(pos);
        if (oldSubMenu != nullptr)
        {
            auto alternetSubMenu = (wxAlternetMenu*)oldSubMenu;
            alternetSubMenu->ownerMenuBar = nullptr;
        }

		return oldSubMenu;
    }

    bool Menu::MainMenuInsert(void* menuHandle, int pos, void* menu, const string& title)
    {
        auto menuBar = (wxAlternetMenuBar*)menuHandle;
		auto alternetMenu = (wxAlternetMenu*)menu;
        auto coercedText = CoerceMenuText(title);
        auto result = menuBar->Insert(pos, alternetMenu, coercedText);
        
        if (!result)
            return false;

        alternetMenu->ownerMenuBar = menuBar;
        return true;
    }

    void Menu::SetMainMenu(Window* window, void* menu)
    {
        if (window == nullptr)
            return;
        auto frame = window->GetFrame();
        if (frame == nullptr)
			return;
        auto oldValue = frame->GetMenuBar();

        if (oldValue != nullptr)
        {
            wxAlternetMenuBar* alternetMenu = wxDynamicCast(oldValue, wxAlternetMenuBar);
            if(alternetMenu != nullptr)
				alternetMenu->ownerFrame = nullptr;
        }

        auto menuBar = (wxAlternetMenuBar*)menu;

		window->ApplyMenu(menuBar);

        if (menuBar != nullptr)
        {
            menuBar->ownerFrame = frame;
        }
    }

    void* Menu::MainMenuReplace(void* menuHandle, int pos, void* menu, const string& title)
    {
        auto menuBar = (wxAlternetMenuBar*)menuHandle;
		auto alternetMenu = (wxAlternetMenu*)menu;
        auto coercedText = CoerceMenuText(title);
        auto oldSubMenu = menuBar->Replace(pos, alternetMenu, coercedText);
        if (oldSubMenu != nullptr)
        {
            auto alternetSubMenu = (wxAlternetMenu*)oldSubMenu;
            alternetSubMenu->ownerMenuBar = nullptr;
        }
		alternetMenu->ownerMenuBar = menuBar;
		return oldSubMenu;
    }

    void Menu::MainMenuSetText(void* menuHandle, int pos, const string& label)
    {
        auto menuBar = (wxAlternetMenuBar*)menuHandle;
		auto coercedText = CoerceMenuText(label);
		menuBar->SetMenuLabel(pos, coercedText);
    }
}
