#include "TabControl.h"
namespace Alternet::UI
{
    TabControl::TabControl()
    {
    }

    TabControl::~TabControl()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_NOTEBOOK_PAGE_CHANGED, &TabControl::OnSelectedPageChanged, this);
                window->Unbind(wxEVT_NOTEBOOK_PAGE_CHANGING, &TabControl::OnSelectedPageChanging, this);
            }
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

    class wxNotebook2 : public wxNotebook, public wxWidgetExtender
    {
    public:
        wxNotebook2(){}
        wxNotebook2(wxWindow* parent,
            wxWindowID id,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = 0,
            const wxString& name = wxASCII_STR(wxNotebookNameStr))
        {
            Create(parent, id, pos, size, style, name);
        }
    };

    wxWindow* TabControl::CreateWxWindowUnparented()
    {
        return new wxNotebook2();
    }

    wxWindow* TabControl::CreateWxWindowCore(wxWindow* parent)
    {
        auto notebook = new wxNotebook2(parent, wxID_ANY, wxDefaultPosition,
            wxDefaultSize, GetStyle());
        notebook->Bind(wxEVT_NOTEBOOK_PAGE_CHANGED, 
            &TabControl::OnSelectedPageChanged, this);
        notebook->Bind(wxEVT_NOTEBOOK_PAGE_CHANGING, 
            &TabControl::OnSelectedPageChanging, this);
        return notebook;
    }

    void TabControl::OnWxWindowCreated()
    {
        for (size_t i = 0; i < _pages.size(); i++)
            InsertPage(i, _pages[i]);
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
        _pages[index]->title = wxStr(title);
        GetNotebook()->SetPageText(index, _pages[index]->title);
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

    void TabControl::InsertPage(int index, Control* control, const string& title)
    {
        auto page = new Page(control, wxStr(title));
        _pages.insert(_pages.begin() + index, page);
        InsertPage(index, page);
    }

    void TabControl::RemovePage(int index, Control* pageControl)
    {
        GetNotebook()->RemovePage(index);
        
        auto page = _pages[index];
        
        _pages.erase(_pages.begin() + index);
        delete page;
    }

    void TabControl::OnSelectedPageChanged(wxBookCtrlEvent& event)
    {
        event.Skip();
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
        else
            event.Skip();
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

    void TabControl::InsertPage(int index, Page* page)
    {
        GetNotebook()->InsertPage(index, page->control->GetWxWindow(), page->title);
    }

    // ---------------

    TabControl::Page::Page(Control* control_, const wxString& title_) :
        control(control_), title(title_)
    {
        control->AddRef();
        // Do not explicitly delete the window for a page that is currently managed by wxNotebook..
        // See https://docs.wxwidgets.org/3.0/classwx_notebook.html.
        control->SetDoNotDestroyWxWindow(true);
    }

    TabControl::Page::~Page()
    {
        control->SetDoNotDestroyWxWindow(false); // See the corresponding SetDoNotDestroyWxWindow(true) call.
        control->Release();
        control = nullptr;
    }
}
