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
            window->Unbind(wxEVT_NOTEBOOK_PAGE_CHANGING, &TabControl::OnSelectedPageChanging, this);
        }
    }

    int TabControl::GetSelectedPageIndex()
    {
        return GetNotebook()->GetSelection();
    }

    void TabControl::SetSelectedPageIndex(int value)
    {
        if (value == -1)
            return; // wxWidgets doesnt allow "no selection" in a tab control.

        GetNotebook()->SetSelection(value);
    }

    wxWindow* TabControl::CreateWxWindowCore(wxWindow* parent)
    {
        auto notebook = new wxNotebook(parent, -1, wxDefaultPosition, wxDefaultSize, GetStyle());
        notebook->Bind(wxEVT_NOTEBOOK_PAGE_CHANGED, &TabControl::OnSelectedPageChanged, this);
        notebook->Bind(wxEVT_NOTEBOOK_PAGE_CHANGING, &TabControl::OnSelectedPageChanging, this);
        return notebook;
    }

    TabAlignment TabControl::GetTabAlignment()
    {
        return _tabAlignment;
    }

    void TabControl::SetTabAlignment(TabAlignment value)
    {
        if (_tabAlignment == value)
            return;

        _tabAlignment = value;
        RecreateWxWindowIfNeeded();
    }

    void TabControl::SetPageTitle(int index, const string& title)
    {
        GetNotebook()->SetPageText(index, wxStr(title));
    }

    Size TabControl::GetTotalPreferredSizeFromPageSize(const Size& pageSize)
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

    void TabControl::OnSelectedPageChanging(wxBookCtrlEvent& event)
    {
        // This is not usable now because of the bug in wx:
        // https://github.com/wxWidgets/Phoenix/issues/1163
        // https://stackoverflow.com/questions/52008385/how-to-get-the-users-new-page-selection-in-a-notebook-control-under-windows
        // event.GetOldSelection(), event.GetSelection() are the same.

        TabPageSelectionEventData data{ event.GetOldSelection(), event.GetSelection() };
        if (RaiseEventWithPointerResult(TabControlEvent::SelectedPageIndexChanging, &data) != 0)
            event.Veto();
    }

    wxNotebook* TabControl::GetNotebook()
    {
        return dynamic_cast<wxNotebook*>(GetWxWindow());
    }
    
    long TabControl::GetStyle()
    {
        auto getTabAlignment = [&]()
        {
            switch (_tabAlignment)
            {
            case TabAlignment::Top:
                return wxNB_TOP;
            case TabAlignment::Bottom:
                return wxNB_BOTTOM;
            case TabAlignment::Left:
                return wxNB_LEFT;
            case TabAlignment::Right:
                return wxNB_RIGHT;
            default:
                throwExNoInfo;
            }
        };

        return getTabAlignment();
    }
}
