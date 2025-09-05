#include "MainMenu.h"
#include "MenuItem.h"
#include "MacOSMenu.h"

namespace Alternet::UI
{
    MainMenu::MainMenu()
    {
        menuBar = new wxMenuBar();
#ifdef __WXOSX_COCOA__
        menuBar->CallAfter([=]() {MacOSMenu::EnsureHelpItemIsLast(GetWxMenuBar()); });

        auto appMenu = menuBar->OSXGetAppleMenu();
        appMenu->Bind(wxEVT_MENU, &MainMenu::OnMenuCommand, this);
#endif

    }

    MainMenu::~MainMenu()
    {
        auto menuBar = GetWxMenuBar();
        if (menuBar == nullptr)
            return;

#ifdef __WXOSX_COCOA__
        menuBar->Unbind(wxEVT_MENU, &MainMenu::OnMenuCommand, this);
#endif

        wxWindowList windows = wxTopLevelWindows;
        for (wxWindowList::compatibility_iterator node = windows.GetFirst(); node; node = node->GetNext())
        {
            wxFrame* frame = wxDynamicCast(node->GetData(), wxFrame);
            if (frame && frame->GetMenuBar() == menuBar)
            {
                frame->SetMenuBar(nullptr);
				break;
            }
        }

		delete menuBar;
    }

    void MainMenu::OnMenuCommand(wxCommandEvent& event)
    {
        event.Skip();
        auto item = MenuItem::GetMenuItemById(event.GetId());
        item->RaiseClick();
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

    wxMenuBar* MainMenu::GetWxMenuBar()
    {
        return dynamic_cast<wxMenuBar*>(menuBar);
    }

    void MainMenu::OnItemRoleChanged(MenuItem* item)
    {
        ApplyItemRoles();
    }

    std::vector<Menu*> MainMenu::GetItems()
    {
        return _items;
    }
}
