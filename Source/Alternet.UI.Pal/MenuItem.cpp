#include "MenuItem.h"
#include "Menu.h"

namespace Alternet::UI
{
    MenuItem::MenuItem() : _menuItem(new wxMenuItem())
    {
    }

    MenuItem::~MenuItem()
    {
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

    void MenuItem::ShowCore()
    {
    }
}
