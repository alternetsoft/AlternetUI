#include "Toolbar.h"
#include "ToolbarItem.h"

namespace Alternet::UI
{
    Toolbar::Toolbar()
    {
    }

    Toolbar::~Toolbar()
    {
    }

    wxWindow* Toolbar::CreateWxWindowCore(wxWindow* parent)
    {
        auto toolBar = new wxToolBar();
        return toolBar;
    }

    wxToolBar* Toolbar::GetWxToolBar()
    {
        return dynamic_cast<wxToolBar*>(GetWxWindow());
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
    }

    void Toolbar::RemoveWxItem(int index)
    {
    }

    int Toolbar::GetItemsCount()
    {
        return GetWxToolBar()->GetToolsCount();
    }

    void Toolbar::InsertItemAt(int index, ToolbarItem* item)
    {
//        GetWxToolBar()->InsertTool(index, item->GetWxTool());
        wxBitmapBundle bmp;
        // wxToolBarTool
        GetWxToolBar()->AddTool(item->GetWxTool()->GetId(), item->GetWxTool()->GetLabel(), bmp);
        item->SetParentToolbar(this, index);
    }

    void Toolbar::RemoveItemAt(int index)
    {
        auto it = _items.begin() + index;
        auto item = *it;
        _items.erase(it);
        GetWxToolBar()->RemoveTool(index);
        item->SetParentToolbar(nullptr, nullopt);
    }
}
