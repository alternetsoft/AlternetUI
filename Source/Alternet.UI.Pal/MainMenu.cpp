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
        GetWxMenuBar()->Insert(index, menu->GetWxMenu(), MenuItem::CoerceWxItemText(text, nullptr));
    }

    void MainMenu::RemoveItemAt(int index)
    {
        GetWxMenuBar()->Remove(index);
    }

    void MainMenu::SetItemText(int index, const string& text)
    {
        GetWxMenuBar()->SetMenuLabel(index, MenuItem::CoerceWxItemText(text, nullptr));
    }

    wxWindow* MainMenu::CreateWxWindowCore(wxWindow* parent)
    {
        auto menuBar = new wxMenuBar();
#ifdef __WXOSX_COCOA__
        menuBar->CallAfter([=]() {AdjustItemsOrder(); });
#endif
        return menuBar;
    }
    
    wxMenuBar* MainMenu::GetWxMenuBar()
    {
        return dynamic_cast<wxMenuBar*>(GetWxWindow());
    }

    void MainMenu::AdjustItemsOrder()
    {
#ifdef __WXOSX_COCOA__
        // macOS appends "Window" menu to the main menu automatically.
        // If our menu has "Help" menu, move it to the end, after the "Window" menu.
        // This only needs to be done when there is no "Window" menu explicitly added by the user.

        auto menuBar = GetWxMenuBar();

        auto windowMenuIndex = menuBar->FindMenu("Window");
        if (windowMenuIndex != wxNOT_FOUND)
            return;
        
        auto helpMenuIndex = menuBar->FindMenu("Help");
        if (helpMenuIndex == wxNOT_FOUND)
            return;

        auto helpMenu = menuBar->GetMenu(helpMenuIndex);
        auto helpMenuLabel = menuBar->GetMenuLabel(helpMenuIndex);

        menuBar->Remove(helpMenuIndex);
        menuBar->Append(helpMenu, helpMenuLabel);
#endif
    }
}
