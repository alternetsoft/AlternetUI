#include "TabControl.h"

namespace Alternet::UI
{
    TabControl::TabControl()
    {
    }

    TabControl::~TabControl()
    {
        auto window = GetWxWindow();
        if (window != nullptr)
        {
            window->Unbind(wxEVT_NOTEBOOK_PAGE_CHANGED, &TabControl::OnSelectedPageChanged, this);
        }
    }

    int TabControl::GetSelectedPageIndex()
    {
        return GetNotebook()->GetSelection();
    }

    void TabControl::SetSelectedPageIndex(int value)
    {
        GetNotebook()->SetSelection(value);
    }

    wxWindow* TabControl::CreateWxWindowCore(wxWindow* parent)
    {
        auto notebook = new wxNotebook(parent, -1, wxDefaultPosition, wxDefaultSize);
        notebook->Bind(wxEVT_NOTEBOOK_PAGE_CHANGED, &TabControl::OnSelectedPageChanged, this);
        return notebook;
    }

    SizeF TabControl::GetTotalPreferredSizeFromPageSize(const SizeF& pageSize)
    {
        auto notebook = GetNotebook();
        return toDip(notebook->CalcSizeFromPage(fromDip(pageSize, notebook)), notebook);
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

    void TabControl::OnSelectedPageChanged(wxBookCtrlEvent& event)
    {
        RaiseEvent(TabControlEvent::SelectedPageIndexChanged);
    }

    wxNotebook* TabControl::GetNotebook()
    {
        return dynamic_cast<wxNotebook*>(GetWxWindow());
    }
}
