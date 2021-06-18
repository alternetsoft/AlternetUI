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
        // Do not explicitly delete the window for a page that is currently managed by wxNotebook..
        // See https://docs.wxwidgets.org/3.0/classwx_notebook.html.
        page->SetDoNotDestroyWxWindow(true);

        GetNotebook()->InsertPage(index, page->GetWxWindow(), wxStr(title));
    }

    void TabControl::RemovePage(int index, Control* page)
    {
        GetNotebook()->RemovePage(index);
        page->SetDoNotDestroyWxWindow(false); // See the corresponding SetDoNotDestroyWxWindow(true) call.
    }

    wxNotebook* TabControl::GetNotebook()
    {
        return dynamic_cast<wxNotebook*>(GetWxWindow());
    }
}
