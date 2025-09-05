#include "Menu.h"
#include "MenuItem.h"

namespace Alternet::UI
{
    string Menu::_eventMenuItemId = wxStr("");

    string Menu::GetEventMenuItemId()
    {
        return _eventMenuItemId;
    }

    void* Menu::CreateMainMenu()
    {
        auto result = new wxAlternetMenuBar();
		return result;
    }

    void* Menu::CreateContextMenu()
    {
        auto result = new wxAlternetMenu();
		return result;
    }

    void* Menu::CreateMenuItem(MenuItemType itemType)
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

    void Menu::SetMenuItemId(void* handle, const string& id)
    {
        auto item = (wxAlternetMenuItem*)handle;
		item->_id = id;
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
