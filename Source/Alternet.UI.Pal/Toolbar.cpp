#include "Toolbar.h"
#include "ToolbarItem.h"

namespace Alternet::UI
{
    wxWindow* Toolbar::CreateWxWindowCore(wxWindow* parent)
    {
        return nullptr;
    }
    wxToolBar* Toolbar::GetWxToolBar()
    {
        return nullptr;
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
        return 0;
    }
    void Toolbar::InsertItemAt(int index, ToolbarItem* item)
    {
    }
    void Toolbar::RemoveItemAt(int index)
    {
    }
    Toolbar::Toolbar()
    {
    }
    Toolbar::~Toolbar()
    {
    }
}
