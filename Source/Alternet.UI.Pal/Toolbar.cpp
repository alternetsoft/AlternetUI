#include "Exception.h"
#include "Toolbar.h"
#include "ToolbarItem.h"
#include "Window.h"
#include "IdManager.h"

namespace Alternet::UI
{
    bool Toolbar::GetNoDivider() 
    {
        return _noDivider;
    }

    void Toolbar::SetNoDivider(bool value)
    {
        if (value == _noDivider)
            return;
        _noDivider = value;
        RecreateWxToolbar();
    }

    bool Toolbar::GetIsVertical() 
    {
        return _isVertical;
    }
    
    void Toolbar::SetIsVertical(bool value) 
    {
        if (value == _isVertical)
            return;
        _isVertical = value;
        RecreateWxToolbar();
    }

    void* Toolbar::CreateEx(bool mainToolbar)
    {
        return new Toolbar(mainToolbar);
    }

    Toolbar::Toolbar(bool mainToolbar)
    {
        _mainToolbar = mainToolbar;
    }

    Toolbar::Toolbar()
    {
    }

    Toolbar::~Toolbar()
    {
        DestroyWxToolbar();
    }

    void Toolbar::Realize() 
    {
        if (IsWxWindowCreated())
            GetToolbar()->Realize();
    }

    void Toolbar::SetOwnerWindow(Window* window)
    {
        if (!_mainToolbar)
            throwExNoInfo;
        _ownerWindow = window;
        RecreateWxToolbar();
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
        
        RecreateWxToolbar();
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
        RecreateWxToolbar();
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
        RecreateWxToolbar();
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

    void Toolbar::RemoveWxItem(int index)
    {
        _items[index]->RemoveWxTool();
    }

    void Toolbar::InsertItemAt(int index, ToolbarItem* item)
    {
        _items.insert(_items.begin() + index, item);
        item->SetParentToolbar(this, index);
        InsertWxItem(index);
        
        if (IsWxWindowCreated())
            GetToolbar()->Realize();
    }

    void Toolbar::RemoveItemAt(int index)
    {
        RemoveWxItem(index);
        auto it = _items.begin() + index;
        auto item = *it;
        _items.erase(it);
        item->SetParentToolbar(nullptr, nullopt);

        if (IsWxWindowCreated())
            GetToolbar()->Realize();
    }

    wxWindow* Toolbar::CreateWxWindowCore(wxWindow* parent)
    {
        auto getStyle = [&]
        {
            auto style = 0 | wxTB_FLAT;

            if(_noDivider)
                style |= wxTB_NODIVIDER;

            if(_isVertical)
                style |= wxTB_VERTICAL;
            else
                style |= wxTB_HORIZONTAL;

            if (_itemTextVisible)
                style |= wxTB_TEXT;

            if (!_itemImagesVisible)
                style |= wxTB_NOICONS;

            if (_imageToTextDisplayMode == ToolbarItemImageToTextDisplayMode::Horizontal)
                style |= wxTB_HORZ_LAYOUT;

            return style;
        };

        wxToolBar* result;

        if (_mainToolbar) 
        {
            if (_ownerWindow == nullptr)
                return new wxPanel();
            result = _ownerWindow->GetFrame()->CreateToolBar(getStyle());
        }
        else
            result = new wxToolBar(parent,
                wxID_ANY,
                wxDefaultPosition,
                wxDefaultSize,
                getStyle());
      
        result->Bind(wxEVT_TOOL, &Toolbar::OnToolbarCommand, this);

        for (size_t i = 0; i < _items.size(); i++)
            InsertWxItem(i);

        result->Realize();
        return result;
    }

    wxToolBar* Toolbar::GetToolbar()
    {
        return dynamic_cast<wxToolBar*>(GetWxWindow());
    }

    void Toolbar::DestroyWxToolbar()
    {
        if (!IsWxWindowCreated())
            return;
        auto window = GetWxWindow();

        window->Unbind(wxEVT_TOOL, &Toolbar::OnToolbarCommand, this);

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
    }

    void Toolbar::RecreateWxToolbar()
    {
        DestroyWxToolbar();
        CreateWxWindow();
    }

    int Toolbar::GetItemsCount()
    {
        if (!IsWxWindowCreated())
            return 0;

        return GetToolbar()->GetToolsCount();
    }
}
