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
    }

    void Toolbar::RemoveItemAt(int index)
    {
    }
}
