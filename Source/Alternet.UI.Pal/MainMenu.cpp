#include "MainMenu.h"
#include "MenuItem.h"
#include "MacOSMenu.h"

namespace Alternet::UI
{
    MainMenu::MainMenu()
    {
    }

    MainMenu::~MainMenu()
    {
    }

    void MainMenu::OnMenuCommand(wxCommandEvent& event)
    {
        auto item = MenuItem::GetMenuItemById(event.GetId());
        item->RaiseClick();
    }

    void MainMenu::ApplyEnabled(bool value)
    {
    }

    void MainMenu::ApplyBounds(const Rect& value)
    {
    }

    void MainMenu::ApplyItemRoles()
    {
#ifdef __WXOSX_COCOA__
        MacOSMenu::ApplyItemRoles(this);
#endif
    }

    int MainMenu::GetItemsCount()
    {
        return GetWxMenuBar()->GetMenuCount();
    }

    void MainMenu::InsertItemAt(int index, Menu* menu, const string& text)
    {
        _items.insert(_items.begin() + index, menu);
        menu->SetParent(this);
        GetWxMenuBar()->Insert(index, menu->GetWxMenu(), MenuItem::CoerceWxItemText(text, nullptr));
    }

    void MainMenu::RemoveItemAt(int index)
    {
        _items[index]->SetParent((MainMenu*)nullptr);
        GetWxMenuBar()->Remove(index);
        _items.erase(_items.begin() + index);
    }

    void MainMenu::SetItemText(int index, const string& text)
    {
        GetWxMenuBar()->SetMenuLabel(index, MenuItem::CoerceWxItemText(text, nullptr));
    }

    wxWindow* MainMenu::CreateWxWindowCore(wxWindow* parent)
    {
        auto menuBar = new wxMenuBar();
#ifdef __WXOSX_COCOA__
        menuBar->CallAfter([=]() {MacOSMenu::EnsureHelpItemIsLast(GetWxMenuBar()); });

        auto appMenu = menuBar->OSXGetAppleMenu();
        appMenu->Bind(wxEVT_MENU, &MainMenu::OnMenuCommand, this);
#endif

        return menuBar;
    }
    
    void MainMenu::OnWxWindowDestroyed(wxWindow* window)
    {
#ifdef __WXOSX_COCOA__
        window->Unbind(wxEVT_MENU, &MainMenu::OnMenuCommand, this);
#endif
    }

    wxMenuBar* MainMenu::GetWxMenuBar()
    {
        return dynamic_cast<wxMenuBar*>(GetWxWindow());
    }

    void MainMenu::OnItemRoleChanged(MenuItem* item)
    {
        ApplyItemRoles();
    }

    void MainMenu::OnEndInit()
    {
        ApplyItemRoles();
    }

    std::vector<Menu*> MainMenu::GetItems()
    {
        return _items;
    }
}
