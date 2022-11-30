#include "Toolbar.h"
#include "ToolbarItem.h"
#include "Window.h"
#include "IdManager.h"

namespace Alternet::UI
{
    Toolbar::Toolbar()
    {
        //SetDoNotDestroyWxWindow(true);
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
    }

    void Toolbar::InsertWxItem(int index)
    {
        if (_wxToolBar == nullptr)
            return;

        _wxToolBar->InsertTool(index, _items[index]->GetWxTool());
        //GetWxToolBar()->Realize();
        //wxBitmapBundle bmp;
        //GetWxToolBar()->AddTool(item->GetWxTool()->GetId(), item->GetWxTool()->GetLabel(), bmp);
    }

    wxToolBar* Toolbar::GetWxToolBar()
    {
        return _wxToolBar;
    }

    void Toolbar::RemoveWxItem(int index)
    {
    }

    void Toolbar::CreateWxToolbar()
    {
        //if (_wxToolBar != nullptr)
        //    throwExNoInfo;

        if (_ownerWindow == nullptr)
            return;

        _wxToolBar = new wxToolBar(_ownerWindow->GetFrame(), IdManager::AllocateId());

        for (int i = 0; i < _items.size(); i++)
            InsertWxItem(i);

        _wxToolBar->Realize();
    }

    void Toolbar::DestroyWxToolbar()
    {
        if (_wxToolBar != nullptr)
        {
            delete _wxToolBar;
            _wxToolBar = nullptr;
        }
    }

    void Toolbar::RecreateWxToolbar()
    {
        //DestroyWxToolbar();
        CreateWxToolbar();
    }

    int Toolbar::GetItemsCount()
    {
        if (_wxToolBar == nullptr)
            return 0;

        return _wxToolBar->GetToolsCount();
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
        if (_wxToolBar != nullptr)
            _wxToolBar->RemoveTool(index);
        item->SetParentToolbar(nullptr, nullopt);
    }
}
