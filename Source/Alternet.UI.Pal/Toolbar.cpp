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

    void Toolbar::SetOwnerWindow(Window* window)
    {
        _ownerWindow = window;
        RecreateWxToolbar(window);
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
        _items[index]->RemoveWxTool(index);
    }

    void Toolbar::InsertItemAt(int index, ToolbarItem* item)
    {
        _items.insert(_items.begin() + index, item);
        InsertWxItem(index);
        item->SetParentToolbar(this, index);
    }

    void Toolbar::RemoveItemAt(int index)
    {
        auto it = _items.begin() + index;
        auto item = *it;
        _items.erase(it);
        RemoveWxItem(index);
        item->SetParentToolbar(nullptr, nullopt);
    }

    void Toolbar::CreateWxToolbar(Window* window)
    {
        if (_wxToolBar != nullptr)
            throwExNoInfo;

        if (_ownerWindow == nullptr)
            return;

        _wxToolBar = window->GetFrame()->CreateToolBar(wxTB_TEXT | wxTB_HORIZONTAL | wxTB_HORZ_TEXT | wxTB_FLAT);

        _wxToolBar->Bind(wxEVT_TOOL, &Toolbar::OnToolbarCommand, this);

        for (int i = 0; i < _items.size(); i++)
            InsertWxItem(i);

        _wxToolBar->Realize();
    }

    void Toolbar::DestroyWxToolbar()
    {
        if (_wxToolBar != nullptr)
        {
            _wxToolBar->Unbind(wxEVT_TOOL, &Toolbar::OnToolbarCommand, this);

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
