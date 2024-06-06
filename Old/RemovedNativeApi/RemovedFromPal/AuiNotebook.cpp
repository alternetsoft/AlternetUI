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

        wxAuiNotebook2()
        {
        }
    };

	AuiNotebook::AuiNotebook()
	{

	}

    AuiNotebook::AuiNotebook(long styles)
    {
        _createStyle = styles;
    }

    /*static*/ void* AuiNotebook::CreateEx(int64_t styles)
    {
        return new AuiNotebook(styles);
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
                    &AuiNotebook::OnPageButton, this);
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

    int AuiNotebook::GetEventSelection()
    {
        return _eventSelection;
    }

    int AuiNotebook::GetEventOldSelection()
    {
        return _eventOldSelection;
    }

    void AuiNotebook::FromEventData(wxAuiNotebookEvent& event)
    {
        _eventSelection = event.GetSelection();
        _eventOldSelection = event.GetOldSelection();
    }

    void AuiNotebook::OnPageClose(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        bool c = RaiseEvent(AuiNotebookEvent::PageClose);
        if (!c)
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

    //can Veto
    void AuiNotebook::OnPageChanging(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        bool c = RaiseEvent(AuiNotebookEvent::PageChanging);
        if (!c)
            event.Skip();
    }

    void AuiNotebook::OnPageButton(wxAuiNotebookEvent& event)
    {
        FromEventData(event);
        bool c = RaiseEvent(AuiNotebookEvent::PageButton);
        if (!c)
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

    int64_t AuiNotebook::GetCreateStyle()
    {
        return _createStyle;
    }

    void AuiNotebook::SetCreateStyle(int64_t value)
    {
        if (_createStyle == value)
            return;
        _createStyle = value;
        RecreateWxWindowIfNeeded();
    }

    wxWindow* AuiNotebook::CreateWxWindowUnparented()
    {
        return new wxAuiNotebook2();
    }

    wxWindow* AuiNotebook::CreateWxWindowCore(wxWindow* parent)
    {
        long style = _createStyle;

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
            &AuiNotebook::OnPageButton, this);
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
        GetNotebook()->SetArtProvider((wxAuiTabArt*)art);
    }

    void* AuiNotebook::GetArtProvider()
    {
        return GetNotebook()->GetArtProvider();
    }

    void AuiNotebook::SetUniformBitmapSize(int width, int height)
    {
        GetNotebook()->SetUniformBitmapSize(wxSize(width, height));
    }

    void AuiNotebook::SetTabCtrlHeight(int height)
    {
        GetNotebook()->SetTabCtrlHeight(height);
    }

    bool AuiNotebook::AddPage(void* page, const string& caption,
        bool select, ImageSet* bitmap)
    {
        return GetNotebook()->AddPage((wxWindow*)page, wxStr(caption),
            select, ImageSet::BitmapBundle(bitmap));
    }

    bool AuiNotebook::InsertPage(uint64_t pageIdx, void* page, const string& caption,
        bool select, ImageSet* bitmap)
    {
        return GetNotebook()->InsertPage(pageIdx, (wxWindow*) page, wxStr(caption),
            select, ImageSet::BitmapBundle(bitmap));
    }

    bool AuiNotebook::DeletePage(uint64_t page)
    {
        return GetNotebook()->DeletePage(page);
    }

    bool AuiNotebook::RemovePage(uint64_t page)
    {
        return GetNotebook()->RemovePage(page);
    }

    uint64_t AuiNotebook::GetPageCount()
    {
        return GetNotebook()->GetPageCount();
    }

    void* AuiNotebook::GetPage(uint64_t pageIdx)
    {
        return GetNotebook()->GetPage(pageIdx);
    }

    int AuiNotebook::FindPage(void* page)
    {
        return GetNotebook()->FindPage((wxWindow*) page);
    }

    bool AuiNotebook::SetPageText(uint64_t page, const string& text)
    {
        return GetNotebook()->SetPageText(page, wxStr(text));
    }

    string AuiNotebook::GetPageText(uint64_t pageIdx)
    {
        return wxStr(GetNotebook()->GetPageText(pageIdx));
    }

    bool AuiNotebook::SetPageToolTip(uint64_t page, const string& text)
    {
        return GetNotebook()->SetPageToolTip(page, wxStr(text));
    }

    string AuiNotebook::GetPageToolTip(uint64_t pageIdx)
    {
        return wxStr(GetNotebook()->GetPageToolTip(pageIdx));
    }

    bool AuiNotebook::SetPageBitmap(uint64_t page, ImageSet* bitmap)
    {
        return GetNotebook()->SetPageBitmap(page, ImageSet::BitmapBundle(bitmap));
    }

    int64_t AuiNotebook::SetSelection(uint64_t newPage)
    {
        return GetNotebook()->SetSelection(newPage);
    }

    int64_t AuiNotebook::GetSelection()
    {
        return GetNotebook()->GetSelection();
    }

    int64_t AuiNotebook::ChangeSelection(uint64_t newPage)
    {
        return GetNotebook()->ChangeSelection(newPage);
    }

    void AuiNotebook::AdvanceSelection(bool forward)
    {
        GetNotebook()->AdvanceSelection(forward);
    }

    void AuiNotebook::SetMeasuringFont(Font* font)
    {
        if (font == nullptr)
            return;
        GetNotebook()->SetMeasuringFont(font->GetWxFont());
    }

    void AuiNotebook::SetNormalFont(Font* font)
    {
        if (font == nullptr)
            return;
        GetNotebook()->SetNormalFont(font->GetWxFont());
    }

    void AuiNotebook::SetSelectedFont(Font* font)
    {
        if (font == nullptr)
            return;
        GetNotebook()->SetSelectedFont(font->GetWxFont());
    }

    void AuiNotebook::Split(uint64_t page, int direction)
    {
        GetNotebook()->Split(page, direction);
    }

    int AuiNotebook::GetTabCtrlHeight()
    {
        return GetNotebook()->GetTabCtrlHeight();
    }

    int AuiNotebook::GetHeightForPageHeight(int pageHeight)
    {
        return GetNotebook()->GetHeightForPageHeight(pageHeight);
    }

    bool AuiNotebook::ShowWindowMenu()
    {
        return GetNotebook()->ShowWindowMenu();
    }

    bool AuiNotebook::DeleteAllPages()
    {
        return GetNotebook()->DeleteAllPages();
    }
}
