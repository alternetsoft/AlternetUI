#include "AuiNotebook.h"

namespace Alternet::UI
{
    class wxAuiNotebook2 : public wxAuiNotebook, public wxWidgetExtender
    {
    public:
        wxAuiNotebook2(wxWindow* parent,
            wxWindowID id = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxAUI_NB_DEFAULT_STYLE)
        {
            Create(parent, id, pos, size, style);
        }
    };

	AuiNotebook::AuiNotebook()
	{

	}

	AuiNotebook::~AuiNotebook()
	{
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_AUINOTEBOOK_PAGE_CLOSE,
                    &AuiNotebook::OnPageClose, this);
                window->Unbind(wxEVT_AUINOTEBOOK_PAGE_CLOSED,
                    &AuiNotebook::OnPageClosed, this);
                window->Unbind(wxEVT_AUINOTEBOOK_PAGE_CHANGED,
                    &AuiNotebook::OnPageChanged, this);
                window->Unbind(wxEVT_AUINOTEBOOK_PAGE_CHANGING,
                    &AuiNotebook::OnPageChanging, this);
                window->Unbind(wxEVT_AUINOTEBOOK_BUTTON,
                    &AuiNotebook::OnWindowListButton, this);
                window->Unbind(wxEVT_AUINOTEBOOK_BEGIN_DRAG,
                    &AuiNotebook::OnBeginDrag, this);
                window->Unbind(wxEVT_AUINOTEBOOK_END_DRAG,
                    &AuiNotebook::OnEndDrag, this);
                window->Unbind(wxEVT_AUINOTEBOOK_DRAG_MOTION,
                    &AuiNotebook::OnDragMotion, this);
                window->Unbind(wxEVT_AUINOTEBOOK_ALLOW_DND,
                    &AuiNotebook::OnAllowTabDrop, this);
                window->Unbind(wxEVT_AUINOTEBOOK_DRAG_DONE,
                    &AuiNotebook::OnDragDone, this);
                window->Unbind(wxEVT_AUINOTEBOOK_TAB_MIDDLE_DOWN,
                    &AuiNotebook::OnTabMiddleMouseDown, this);
                window->Unbind(wxEVT_AUINOTEBOOK_TAB_MIDDLE_UP,
                    &AuiNotebook::OnTabMiddleMouseUp, this);
                window->Unbind(wxEVT_AUINOTEBOOK_TAB_RIGHT_DOWN,
                    &AuiNotebook::OnTabRightMouseDown, this);
                window->Unbind(wxEVT_AUINOTEBOOK_TAB_RIGHT_UP,
                    &AuiNotebook::OnTabRightMouseUp, this);
                window->Unbind(wxEVT_AUINOTEBOOK_BG_DCLICK,
                    &AuiNotebook::OnBgDclickMouse, this);
            }
        }
    }

    void AuiNotebook::FromEventData(wxAuiNotebookEvent& event)
    {
        int _eventSelection = event.GetSelection();
        int _eventOldSelection = event.GetOldSelection();
    }

    void AuiNotebook::OnPageClose(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::PageClose);
        event.Skip();
    }

    void AuiNotebook::OnPageClosed(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::PageClosed);
        event.Skip();
    }

    void AuiNotebook::OnPageChanged(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::PageChanged);
        event.Skip();
    }

    void AuiNotebook::OnPageChanging(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::PageChanging);
        event.Skip();
    }

    void AuiNotebook::OnWindowListButton(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::WindowListButton);
        event.Skip();
    }

    void AuiNotebook::OnBeginDrag(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::BeginDrag);
        event.Skip();
    }

    void AuiNotebook::OnEndDrag(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::EndDrag);
        event.Skip();
    }
        
    void AuiNotebook::OnDragMotion(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::DragMotion);
        event.Skip();
    }

    void AuiNotebook::OnAllowTabDrop(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::AllowTabDrop);
        event.Skip();
    }

    void AuiNotebook::OnDragDone(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::DragDone);
        event.Skip();
    }
    
    void AuiNotebook::OnTabMiddleMouseDown(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::TabMiddleMouseDown);
        event.Skip();
    }
        
    void AuiNotebook::OnTabMiddleMouseUp(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::TabMiddleMouseUp);
        event.Skip();
    }

    void AuiNotebook::OnTabRightMouseDown(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::TabRightMouseDown);
        event.Skip();
    }

    void AuiNotebook::OnTabRightMouseUp(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::TabRightMouseUp);
        event.Skip();
    }
    
    void AuiNotebook::OnBgDclickMouse(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiNotebookEvent::BgDclickMouse);
        event.Skip();
    }

    wxWindow* AuiNotebook::CreateWxWindowCore(wxWindow* parent)
    {
        long style = wxAUI_NB_DEFAULT_STYLE;

        auto window = new wxAuiNotebook2(parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        window->Bind(wxEVT_AUINOTEBOOK_PAGE_CLOSE,
            &AuiNotebook::OnPageClose, this);
        window->Bind(wxEVT_AUINOTEBOOK_PAGE_CLOSED,
            &AuiNotebook::OnPageClosed, this);
        window->Bind(wxEVT_AUINOTEBOOK_PAGE_CHANGED,
            &AuiNotebook::OnPageChanged, this);
        window->Bind(wxEVT_AUINOTEBOOK_PAGE_CHANGING,
            &AuiNotebook::OnPageChanging, this);
        window->Bind(wxEVT_AUINOTEBOOK_BUTTON,
            &AuiNotebook::OnWindowListButton, this);
        window->Bind(wxEVT_AUINOTEBOOK_BEGIN_DRAG,
            &AuiNotebook::OnBeginDrag, this);
        window->Bind(wxEVT_AUINOTEBOOK_END_DRAG,
            &AuiNotebook::OnEndDrag, this);
        window->Bind(wxEVT_AUINOTEBOOK_DRAG_MOTION,
            &AuiNotebook::OnDragMotion, this);
        window->Bind(wxEVT_AUINOTEBOOK_ALLOW_DND,
            &AuiNotebook::OnAllowTabDrop, this);
        window->Bind(wxEVT_AUINOTEBOOK_DRAG_DONE,
            &AuiNotebook::OnDragDone, this);
        window->Bind(wxEVT_AUINOTEBOOK_TAB_MIDDLE_DOWN,
            &AuiNotebook::OnTabMiddleMouseDown, this);
        window->Bind(wxEVT_AUINOTEBOOK_TAB_MIDDLE_UP,
            &AuiNotebook::OnTabMiddleMouseUp, this);
        window->Bind(wxEVT_AUINOTEBOOK_TAB_RIGHT_DOWN,
            &AuiNotebook::OnTabRightMouseDown, this);
        window->Bind(wxEVT_AUINOTEBOOK_TAB_RIGHT_UP,
            &AuiNotebook::OnTabRightMouseUp, this);
        window->Bind(wxEVT_AUINOTEBOOK_BG_DCLICK,
            &AuiNotebook::OnBgDclickMouse, this);
        return window;
    }

    wxAuiNotebook* AuiNotebook::GetNotebook()
    {
        return dynamic_cast<wxAuiNotebook*>(GetWxWindow());
    }

    void AuiNotebook::SetArtProvider(void* art)
    {
    }

    void* AuiNotebook::GetArtProvider()
    {
        return nullptr;
    }

    void AuiNotebook::SetUniformBitmapSize(int width, int height)
    {
    }

    void AuiNotebook::SetTabCtrlHeight(int height)
    {
    }

    bool AuiNotebook::AddPage(void* page, const string& caption,
        bool select, ImageSet* bitmap)
    {
        return false;
    }

    bool AuiNotebook::InsertPage(uint64_t pageIdx, void* page, const string& caption,
        bool select, ImageSet* bitmap)
    {
        return false;
    }

    bool AuiNotebook::DeletePage(uint64_t page)
    {
        return false;
    }

    bool AuiNotebook::RemovePage(uint64_t page)
    {
        return false;
    }

    uint64_t AuiNotebook::GetPageCount()
    {
        return 0;
    }

    void* AuiNotebook::GetPage(uint64_t pageIdx)
    {
        return nullptr;
    }

    int AuiNotebook::FindPage(void* page)
    {
        return 0;
    }

    bool AuiNotebook::SetPageText(uint64_t page, const string& text)
    {
        return false;
    }

    string AuiNotebook::GetPageText(uint64_t pageIdx)
    {
        return wxStr(wxEmptyString);
    }

    bool AuiNotebook::SetPageToolTip(uint64_t page, const string& text)
    {
        return false;
    }

    string AuiNotebook::GetPageToolTip(uint64_t pageIdx)
    {
        return wxStr(wxEmptyString);
    }

    bool AuiNotebook::SetPageBitmap(uint64_t page, ImageSet* bitmap)
    {
        return false;
    }

    int AuiNotebook::SetSelection(uint64_t newPage)
    {
        return 0;
    }

    int AuiNotebook::GetSelection()
    {
        return 0;
    }

    void AuiNotebook::Split(uint64_t page, int direction)
    {
    }

    int AuiNotebook::GetTabCtrlHeight()
    {
        return 0;
    }

    int AuiNotebook::GetHeightForPageHeight(int pageHeight)
    {
        return 0;
    }

    bool AuiNotebook::ShowWindowMenu()
    {
        return false;
    }

    bool AuiNotebook::DeleteAllPages()
    {
        return false;
    }
}
