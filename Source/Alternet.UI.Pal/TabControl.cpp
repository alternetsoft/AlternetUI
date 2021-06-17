#include "TabControl.h"

namespace Alternet::UI
{
    TabControl::TabControl()
    {
    }

    TabControl::~TabControl()
    {
    }

    wxWindow* TabControl::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxNotebook(parent, -1, wxDefaultPosition, wxDefaultSize);
    }

    int TabControl::GetPageCount()
    {
        return GetNotebook()->GetPageCount();
    }

    void TabControl::InsertPage(int index, Control* page, const string& title)
    {
        GetNotebook()->InsertPage(index, page->GetWxWindow(), wxStr(title));
    }

    void TabControl::RemovePage(int index)
    {
        GetNotebook()->RemovePage(index);
    }

    wxNotebook* TabControl::GetNotebook()
    {
        return dynamic_cast<wxNotebook*>(GetWxWindow());
    }
}
