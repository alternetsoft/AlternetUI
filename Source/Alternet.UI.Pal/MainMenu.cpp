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
        event.Skip();
        auto item = MenuItem::GetMenuItemById(event.GetId());
        item->RaiseClick();
    }

    void MainMenu::ApplyEnabled(bool value)
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
        _itemTextByMenu[menu] = text;
        _items.insert(_items.begin() + index, menu);
        InsertWxItem(index);
        menu->SetParent(this);
    }

    void MainMenu::RemoveItemAt(int index)
    {
        auto item = _items[index];
        _itemTextByMenu.erase(_itemTextByMenu.find(item));
        item->SetParent((MainMenu*)nullptr);
        RemoveWxItem(index);
        _items.erase(_items.begin() + index);
    }

    void MainMenu::InsertWxItem(int index)
    {
        auto menu = _items[index];
        auto wxIndex = LogicalIndexToWxIndex(index);
        GetWxMenuBar()->Insert(wxIndex, menu->GetWxMenu(),
            MenuItem::CoerceWxItemText(_itemTextByMenu[menu], nullptr));
    }

    void MainMenu::RemoveWxItem(int index)
    {
        auto wxIndex = LogicalIndexToWxIndex(index);
        GetWxMenuBar()->Remove(wxIndex);
    }

    void MainMenu::SetItemText(int index, const string& text)
    {
        auto wxIndex = LogicalIndexToWxIndex(index);
        GetWxMenuBar()->SetMenuLabel(wxIndex, MenuItem::CoerceWxItemText(text, nullptr));
    }

    wxWindow* MainMenu::CreateWxWindowUnparented()
    {
        return new wxMenuBar();
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

    int MainMenu::GetItemLogicalIndex(Menu* item)
    {
        auto it = std::find(_items.begin(), _items.end(), item);
        if (it == _items.end())
            throwExNoInfo;

        return it - _items.begin();
    }

    void MainMenu::SetItemHidden(Menu* item, bool hidden)
    {
        auto index = GetItemLogicalIndex(item);
        bool alreadyHidden = IsItemHidden(index);

        if (alreadyHidden == hidden)
            return;

        if (alreadyHidden)
        {
            InsertWxItem(index);
            _hiddenItemIndices.erase(index);
        }
        else
        {
            _hiddenItemIndices.insert(index);
            RemoveWxItem(index);
        }
    }

    bool MainMenu::IsItemHidden(Menu* item)
    {
        auto index = GetItemLogicalIndex(item);
        return IsItemHidden(index);
    }

    bool MainMenu::IsItemHidden(int index)
    {
        return _hiddenItemIndices.find(index) != _hiddenItemIndices.end();
    }

    int MainMenu::LogicalIndexToWxIndex(int logicalIndex)
    {
        int wxIndex = 0;
        for (size_t i = 0; i < _items.size(); i++)
        {
            if (i == logicalIndex)
                return wxIndex;
            
            if (!IsItemHidden(i))
                wxIndex++;
        }

        throwExNoInfo;
    }

    void MainMenu::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
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
