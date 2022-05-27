#include "MainMenu.h"
#include "MenuItem.h"

namespace Alternet::UI
{
    MainMenu::MainMenu()
    {
    }
    MainMenu::~MainMenu()
    {
    }

    void MainMenu::ApplyEnabled(bool value)
    {
    }

    void MainMenu::ApplyBounds(const Rect& value)
    {
    }

    int MainMenu::GetItemsCount()
    {
        return GetWxMenuBar()->GetMenuCount();
    }

    void MainMenu::InsertItemAt(int index, Menu* menu, const string& text)
    {
        GetWxMenuBar()->Insert(index, menu->GetWxMenu(), MenuItem::CoerceWxItemText(text));
    }

    void MainMenu::RemoveItemAt(int index)
    {
        GetWxMenuBar()->Remove(index);
    }

    void MainMenu::SetItemText(int index, const string& text)
    {
        GetWxMenuBar()->SetMenuLabel(index, MenuItem::CoerceWxItemText(text));
    }

    wxWindow* MainMenu::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxMenuBar();
    }
    
    wxMenuBar* MainMenu::GetWxMenuBar()
    {
        return dynamic_cast<wxMenuBar*>(GetWxWindow());
    }
}
