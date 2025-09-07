#include "Menu.h"

namespace Alternet::UI
{
    void Menu::Show(void* menuHandle, Control* control, const PointD& position)
    {
        auto item = (wxAlternetMenu*)menuHandle;
        auto wxWindow = control->GetWxWindow();

        auto byDefault = (position.X == -1 || position.Y == -1);

        auto sx = byDefault ? -1 : fromDip(position.X, wxWindow);
        auto sy = byDefault ? -1 : fromDip(position.Y, wxWindow);

		auto pos = PointD(sx, sy);

        wxWindow->PopupMenu(item, fromDip(pos, wxWindow));
    }

    string Menu::_eventMenuItemId = wxStr("");

    string Menu::GetEventMenuItemId()
    {
        return _eventMenuItemId;
    }

    void* Menu::CreateMainMenu(const string& id)
    {
        auto result = new wxAlternetMenuBar();
        result->_id = id;
        return result;
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

        auto result = new wxAlternetMenuItem(nullptr, wxID_ANY, wxEmptyString, wxEmptyString, kind, nullptr);
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

    void Menu::SetMenuItemText(void* handle, const string& value, const string& rightValue)
    {
        auto item = (wxAlternetMenuItem*)handle;

        auto text = value.empty() ? wxString(" ") : wxStr(value);
		auto rightText = rightValue.empty() ? wxString(" ") : wxStr(rightValue);

        text.Replace("_", "&");

        auto labelText = text + "\t" + rightText;

		item->SetItemLabel(labelText);
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


/*
    _menu->Bind(wxEVT_MENU, &Menu::OnMenuCommand, this);
    _menu->Bind(wxEVT_MENU_OPEN, &Menu::OnMenuOpen, this);
    _menu->Bind(wxEVT_MENU_CLOSE, &Menu::OnMenuClose, this);
    _menu->Bind(wxEVT_MENU_HIGHLIGHT, &Menu::OnMenuHighlight, this);
    _menu->Unbind(wxEVT_MENU, &Menu::OnMenuCommand, this);
    _menu->Unbind(wxEVT_MENU_OPEN, &Menu::OnMenuOpen, this);
    _menu->Unbind(wxEVT_MENU_CLOSE, &Menu::OnMenuClose, this);
    _menu->Unbind(wxEVT_MENU_HIGHLIGHT, &Menu::OnMenuHighlight, this);

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
*/
}
