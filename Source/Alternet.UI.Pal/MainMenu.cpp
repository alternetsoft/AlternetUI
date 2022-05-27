#include "MainMenu.h"

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
        GetWxMenuBar()->Insert(index, menu->GetWxMenu(), wxStr(text));
    }

    void MainMenu::RemoveItemAt(int index)
    {
        GetWxMenuBar()->Remove(index);
    }

    void MainMenu::SetItemText(int index, const string& text)
    {
        GetWxMenuBar()->SetMenuLabel(index, wxStr(text));
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
