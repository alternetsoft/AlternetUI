#include "MenuItem.h"

namespace Alternet::UI
{
    MenuItem::MenuItem()
    {
    }

    MenuItem::~MenuItem()
    {
    }

    string MenuItem::GetText()
    {
        return string();
    }

    void MenuItem::SetText(const string& value)
    {
    }

    bool MenuItem::GetChecked()
    {
        return false;
    }

    void MenuItem::SetChecked(bool value)
    {
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
        return nullptr;
    }

    void MenuItem::SetSubmenu(Menu* value)
    {
    }

    wxWindow* MenuItem::CreateWxWindowCore(wxWindow* parent)
    {
        return nullptr;
    }
}
