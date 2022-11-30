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
        if (_wxToolBar == nullptr)
            return;

        auto info = _items[index]->GetToolInfo();
        auto tool = _wxToolBar->InsertTool(index, info->id, info->text, info->image, wxBitmapBundle(), info->kind);

        if (info->dropDownMenu != nullptr)
            tool->SetDropdownMenu(info->dropDownMenu);

        //_wxToolBar->AddTool(_items[index]->GetWxTool()->GetId(), "eded", _items[index]->GetWxTool()->GetBitmap());
        //_wxToolBar->AddSeparator();
        //GetWxToolBar()->Realize();
    }

    wxToolBar* Toolbar::GetWxToolBar()
    {
        return _wxToolBar;
    }

    void Toolbar::RemoveWxItem(int index)
    {
    }

    void Toolbar::CreateWxToolbar(Window* window)
    {
        //if (_wxToolBar != nullptr)
        //    throwExNoInfo;

        if (_ownerWindow == nullptr)
            return;

        _wxToolBar = window->GetFrame()->CreateToolBar(wxTB_TEXT | wxTB_HORIZONTAL | wxTB_HORZ_TEXT | wxTB_FLAT);
        //_wxToolBar = new wxToolBar(window->GetFrame(), /*IdManager::AllocateId()*/-1, wxDefaultPosition, wxDefaultSize,
        //    wxTB_TEXT | wxTB_HORIZONTAL | wxTB_HORZ_TEXT | wxTB_FLAT);

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
        //DestroyWxToolbar();
        CreateWxToolbar(window);
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
