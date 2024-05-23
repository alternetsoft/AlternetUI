#include "Toolbar.h"
#include "ToolbarItem.h"
#include "Window.h"
#include "IdManager.h"

namespace Alternet::UI
{
    class wxToolBar2 : public wxToolBar, public wxWidgetExtender
    {
    public:
        wxToolBar2(){}
        wxToolBar2(wxWindow* parent,
            wxWindowID id,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxTB_DEFAULT_STYLE,
            const wxString& name = wxASCII_STR(wxToolBarNameStr))
        {
            Create(parent, id, pos, size, style, name);
        }
    };

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

    wxWindow* Toolbar::CreateWxWindowUnparented()
    {
        return new wxDummyPanel("toolbar");
    }

    wxWindow* Toolbar::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxDummyPanel("toolbar");
    }

    void Toolbar::SetOwnerWindow(Window* window)
    {
        _ownerWindow = window;
        RecreateWxToolbar();
    }

    void Toolbar::OnItemChanged(int index)
    {
    }

    ImageToText Toolbar::GetImageToTextDisplayMode()
    {
        return _imageToTextDisplayMode;
    }

    void Toolbar::SetImageToTextDisplayMode(ImageToText value)
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
        event.Skip();
        auto id = event.GetId();
        auto item = ToolbarItem::GetToolbarItemById(id);
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

    void Toolbar::Realize()
    {
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

    void Toolbar::CreateWxToolbar()
    {
        if (_wxToolBar != nullptr)
            throwExNoInfo;

        if (_ownerWindow == nullptr)
            return;

        auto getStyle = [&]
        {
            auto style = 0 | wxTB_FLAT;

            if (_noDivider)
                style |= wxTB_NODIVIDER;

            if (_isVertical) 
            {
                style |= wxTB_VERTICAL;
                if (_isRight)
                    style |= wxTB_RIGHT;
            }
            else
            {
                style |= wxTB_HORIZONTAL;
                if (_isBottom)
                    style |= wxTB_BOTTOM;
            }

            if (_itemTextVisible)
                style |= wxTB_TEXT;

            if (!_itemImagesVisible)
                style |= wxTB_NOICONS;

            if (_imageToTextDisplayMode == ImageToText::Horizontal)
                style |= wxTB_HORZ_LAYOUT;

            return style;
        };

        _wxToolBar = _ownerWindow->GetFrame()->CreateToolBar(getStyle());

        _wxToolBar->Bind(wxEVT_TOOL, &Toolbar::OnToolbarCommand, this);

        for (size_t i = 0; i < _items.size(); i++)
            InsertWxItem(i);

        _wxToolBar->Realize();
    }

    bool Toolbar::GetIsBottom()
    {
        return _isBottom;
    }

    void Toolbar::SetIsBottom(bool value) 
    {
        if (_isBottom == value)
            return;
        _isBottom = value;
        RecreateWxToolbar();
    }


    bool Toolbar::GetIsRight() 
    {
        return _isRight;
    }
    
    void Toolbar::SetIsRight(bool value)
    {
        if (_isRight == value)
            return;
        _isRight = value;
        RecreateWxToolbar();
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

    void Toolbar::RecreateWxToolbar()
    {
        if (_ownerWindow == nullptr)
            return;

        DestroyWxToolbar();
        CreateWxToolbar();
    }

    int Toolbar::GetItemsCount()
    {
        if (_wxToolBar == nullptr)
            return 0;

        return _wxToolBar->GetToolsCount();
    }
}