#include "Toolbar.h"
#include "ToolbarItem.h"
#include "Window.h"
#include "IdManager.h"

namespace Alternet::UI
{
    Toolbar::Toolbar()
    {
    }

    Toolbar::~Toolbar()
    {
        DestroyWxToolbar();
    }

    wxWindow* Toolbar::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxPanel();
    }

    void Toolbar::Realize() 
    {
        _wxToolBar->Realize();
    }

    void Toolbar::SetOwnerWindow(Window* window)
    {
        _ownerWindow = window;
        RecreateWxToolbar(window);
    }

    void Toolbar::OnItemChanged(int index)
    {
    }

    ToolbarItemImageToTextDisplayMode Toolbar::GetImageToTextDisplayMode()
    {
        return _imageToTextDisplayMode;
    }

    void Toolbar::SetImageToTextDisplayMode(ToolbarItemImageToTextDisplayMode value)
    {
        if (_imageToTextDisplayMode == value)
            return;
        
        _imageToTextDisplayMode = value;
        
        if (_ownerWindow != nullptr)
            RecreateWxToolbar(_ownerWindow);
    }

    bool Toolbar::GetItemTextVisible()
    {
        return _itemTextVisible;
    }

    void Toolbar::SetItemTextVisible(bool value)
    {
        if (_itemTextVisible == value)
            return;

        _itemTextVisible = value;
        if (_ownerWindow != nullptr)
            RecreateWxToolbar(_ownerWindow);
    }

    bool Toolbar::GetItemImagesVisible()
    {
        return _itemImagesVisible;
    }

    void Toolbar::SetItemImagesVisible(bool value)
    {
        if (_itemImagesVisible == value)
            return;

        _itemImagesVisible = value;
        if (_ownerWindow != nullptr)
            RecreateWxToolbar(_ownerWindow);
    }

    void Toolbar::ApplyEnabled(bool value)
    {
    }

    void Toolbar::ApplyBounds(const Rect& value)
    {
    }

    void Toolbar::OnToolbarCommand(wxCommandEvent& event)
    {
        auto item = ToolbarItem::GetToolbarItemById(event.GetId());
        if (item != nullptr)
        {
            item->RaiseClick();
        }
    }

    void Toolbar::InsertWxItem(int index)
    {
        _items[index]->InsertWxTool(index);
    }

    wxToolBar* Toolbar::GetWxToolBar()
    {
        return _wxToolBar;
    }

    void Toolbar::RemoveWxItem(int index)
    {
        _items[index]->RemoveWxTool();
    }

    void Toolbar::InsertItemAt(int index, ToolbarItem* item)
    {
        _items.insert(_items.begin() + index, item);
        item->SetParentToolbar(this, index);
        InsertWxItem(index);
        
        if (_wxToolBar != nullptr)
            _wxToolBar->Realize();
    }

    void Toolbar::RemoveItemAt(int index)
    {
        RemoveWxItem(index);
        auto it = _items.begin() + index;
        auto item = *it;
        _items.erase(it);
        item->SetParentToolbar(nullptr, nullopt);

        if (_wxToolBar != nullptr)
            _wxToolBar->Realize();
    }

    void Toolbar::CreateWxToolbar(Window* window)
    {
        if (_wxToolBar != nullptr)
            throwExNoInfo;

        if (_ownerWindow == nullptr)
            return;

        auto getStyle = [&]
        {
            auto style = wxTB_HORIZONTAL | wxTB_FLAT;
            
            if (_itemTextVisible)
                style |= wxTB_TEXT;

            if (!_itemImagesVisible)
                style |= wxTB_NOICONS;

            if (_imageToTextDisplayMode == ToolbarItemImageToTextDisplayMode::Horizontal)
                style |= wxTB_HORZ_LAYOUT;

            return style;
        };

        _wxToolBar = window->GetFrame()->CreateToolBar(getStyle());

        _wxToolBar->Bind(wxEVT_TOOL, &Toolbar::OnToolbarCommand, this);

        for (size_t i = 0; i < _items.size(); i++)
            InsertWxItem(i);

        _wxToolBar->Realize();
    }

    void Toolbar::DestroyWxToolbar()
    {
        if (_wxToolBar != nullptr)
        {
            _wxToolBar->Unbind(wxEVT_TOOL, &Toolbar::OnToolbarCommand, this);

            for (auto item : _items)
            {
                auto menu = item->GetDropDownMenu();
                if (menu != nullptr)
                {
                    menu->DetachAndRecreateWxMenu();
                    item->GetToolInfo()->dropDownMenu = menu->GetWxMenu();
                    menu->Release();
                }
            }

            delete _wxToolBar;
            _wxToolBar = nullptr;
        }
    }

    void Toolbar::RecreateWxToolbar(Window* window)
    {
        DestroyWxToolbar();
        CreateWxToolbar(window);
    }

    int Toolbar::GetItemsCount()
    {
        if (_wxToolBar == nullptr)
            return 0;

        return _wxToolBar->GetToolsCount();
    }
}
