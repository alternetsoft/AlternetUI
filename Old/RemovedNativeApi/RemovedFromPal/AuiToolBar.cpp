#include "AuiToolBar.h"

namespace Alternet::UI
{
    class wxAuiToolBar2 : public wxAuiToolBar, public wxWidgetExtender
    {
    public:
        wxAuiToolBar2(wxWindow* parent,
            wxWindowID id = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxAUI_TB_DEFAULT_STYLE)
        {
            Create(parent, id, pos, size, style);
        }

        wxAuiToolBar2(){}
        
        void OnLeaveWindow(wxMouseEvent& evt) 
        {
            wxAuiToolBar::OnLeaveWindow(evt);
        }

        void OnCaptureLost(wxMouseCaptureLostEvent& evt)
        {
            wxAuiToolBar::OnCaptureLost(evt);
        }

        void OnLeftDown(wxMouseEvent& evt)
        {
            wxAuiToolBar::OnLeftDown(evt);
        }

        void OnLeftUp(wxMouseEvent& evt)
        {
            wxAuiToolBar::OnLeftUp(evt);
        }

        void OnRightDown(wxMouseEvent& evt)
        {
            wxAuiToolBar::OnRightDown(evt);
        }

        void OnRightUp(wxMouseEvent& evt)
        {
            wxAuiToolBar::OnRightUp(evt);
        }
        
        void OnMiddleDown(wxMouseEvent& evt)
        {
            wxAuiToolBar::OnMiddleDown(evt);
        }
        
        void OnMiddleUp(wxMouseEvent& evt)
        {
            wxAuiToolBar::OnMiddleUp(evt);
        }
        
        void OnMotion(wxMouseEvent& evt)
        {
            wxAuiToolBar::OnMotion(evt);
        }
    };

    wxWindow* AuiToolBar::CreateWxWindowUnparented()
    {
        return new wxAuiToolBar2();
    }

    void AuiToolBar::DoOnCaptureLost()
    {
        auto window = dynamic_cast<wxAuiToolBar2*>(GetWxWindow());
        wxMouseCaptureLostEvent ev = wxMouseCaptureLostEvent();
        window->OnCaptureLost(ev);
    }

    void AuiToolBar::DoOnLeftUp(int x, int y)
    {
        auto window = dynamic_cast<wxAuiToolBar2*>(GetWxWindow());
        wxMouseEvent ev = wxMouseEvent(wxEVT_LEFT_UP);
        ev.SetX(x);ev.SetY(y);
        window->OnLeftUp(ev);
    }

    void AuiToolBar::DoOnLeftDown(int x, int y)
    {
        auto window = dynamic_cast<wxAuiToolBar2*>(GetWxWindow());
        wxMouseEvent ev = wxMouseEvent(wxEVT_LEFT_DOWN);
        ev.SetX(x);ev.SetY(y);
        window->OnLeftDown(ev);
    }

	AuiToolBar::AuiToolBar()
	{

	}

    AuiToolBar::AuiToolBar(long styles)
    {
        _createStyle = styles;
    }

    /*static*/ void* AuiToolBar::CreateEx(int64_t styles)
    {
        return new AuiToolBar(styles);
    }

    int64_t AuiToolBar::GetCreateStyle()
    {
        return _createStyle;
    }

    void AuiToolBar::SetCreateStyle(int64_t value)
    {
        if (_createStyle == value)
            return;
        _createStyle = value;
        RecreateWxWindowIfNeeded();
    }

    wxWindow* AuiToolBar::CreateWxWindowCore(wxWindow* parent)
    {
        long style = _createStyle;

        auto toolbar = new wxAuiToolBar2(parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        //toolbar->Bind(wxEVT_LEFT_DOWN, &AuiToolBar::OnLeftDown, this);
        toolbar->Bind(wxEVT_TOOL, &AuiToolBar::OnToolbarCommand, this);
        toolbar->Bind(wxEVT_AUITOOLBAR_TOOL_DROPDOWN, &AuiToolBar::OnToolDropDown, this);
        toolbar->Bind(wxEVT_AUITOOLBAR_OVERFLOW_CLICK, &AuiToolBar::OnOverflowClick, this);
        toolbar->Bind(wxEVT_AUITOOLBAR_RIGHT_CLICK, &AuiToolBar::OnRightClick, this);
        toolbar->Bind(wxEVT_AUITOOLBAR_MIDDLE_CLICK, &AuiToolBar::OnMiddleClick, this);
        toolbar->Bind(wxEVT_AUITOOLBAR_BEGIN_DRAG, &AuiToolBar::OnBeginDrag, this);
        return toolbar;
    }

    void AuiToolBar::OnToolbarCommand(wxCommandEvent& event)
    {
        event.Skip();
        _eventToolId = event.GetId();
        RaiseEvent(AuiToolBarEvent::ToolCommand);
        event.StopPropagation();
    }

    int AuiToolBar::GetEventToolId() 
    {
        return _eventToolId;
    }

    bool AuiToolBar::GetEventIsDropDownClicked()
    {
        return _eventIsDropDownClicked;
    }

    Int32Point AuiToolBar::GetEventClickPoint()
    {
        return _eventClickPoint;
    }

    Int32Rect AuiToolBar::GetEventItemRect()
    {
        return _eventItemRect;
    }

    void AuiToolBar::FromEventData(wxAuiToolBarEvent& event)
    {
        _eventToolId = event.GetToolId();
        _eventIsDropDownClicked = event.IsDropDownClicked();
        _eventClickPoint = Int32Point(event.GetClickPoint());
        _eventItemRect = Int32Rect(event.GetItemRect());
    }

    void AuiToolBar::OnLeftDown(wxMouseEvent& evt)
    {
        evt.Skip();
    }

    void AuiToolBar::OnToolDropDown(wxAuiToolBarEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiToolBarEvent::ToolDropDown);
       /* if (_eventIsDropDownClicked)
        {
            wxCommandEvent* ev = new wxCommandEvent(wxEVT_TOOL);
            ev->SetId(event.GetToolId());
            GetToolbar()->GetEventHandler()->QueueEvent(ev);
        }*/
        event.Skip();
    }

    void AuiToolBar::OnBeginDrag(wxAuiToolBarEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiToolBarEvent::BeginDrag);
        event.Skip();
    }

    void AuiToolBar::OnMiddleClick(wxAuiToolBarEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiToolBarEvent::ToolMiddleClick);
        event.Skip();
    }

    void AuiToolBar::OnOverflowClick(wxAuiToolBarEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiToolBarEvent::OverflowClick);
        event.Skip();
    }

    void AuiToolBar::OnRightClick(wxAuiToolBarEvent& event)
    {
        FromEventData(event);
        RaiseEvent(AuiToolBarEvent::ToolRightClick);
        event.Skip();
    }

    wxAuiToolBar* AuiToolBar::GetToolbar()
    {
        return dynamic_cast<wxAuiToolBar2*>(GetWxWindow());
    }

	AuiToolBar::~AuiToolBar()
	{
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                //window->Unbind(wxEVT_LEFT_DOWN, &AuiToolBar::OnLeftDown, this);
                window->Unbind(wxEVT_TOOL, &AuiToolBar::OnToolbarCommand, this);
                window->Unbind(wxEVT_AUITOOLBAR_TOOL_DROPDOWN,
                    &AuiToolBar::OnToolDropDown, this);
                window->Unbind(wxEVT_AUITOOLBAR_OVERFLOW_CLICK,
                    &AuiToolBar::OnOverflowClick, this);
                window->Unbind(wxEVT_AUITOOLBAR_RIGHT_CLICK,
                    &AuiToolBar::OnRightClick, this);
                window->Unbind(wxEVT_AUITOOLBAR_MIDDLE_CLICK,
                    &AuiToolBar::OnMiddleClick, this);
                window->Unbind(wxEVT_AUITOOLBAR_BEGIN_DRAG,
                    &AuiToolBar::OnBeginDrag, this);
            }
        }
    }

    void AuiToolBar::SetArtProvider(void* art)
    {
        GetToolbar()->SetArtProvider((wxAuiToolBarArt*)art);
    }

    void* AuiToolBar::GetArtProvider()
    {
        return GetToolbar()->GetArtProvider();
    }

    void* AuiToolBar::AddTool(int toolId, const string& label, ImageSet* bitmapBundle,
        const string& shortHelpString, int itemKind)
    {
        return GetToolbar()->AddTool(toolId, wxStr(label), ImageSet::BitmapBundle(bitmapBundle),
            wxStr(shortHelpString), (wxItemKind)itemKind);
    }

    void* AuiToolBar::AddTool2(int toolId, const string& label, ImageSet* bitmapBundle,
        ImageSet* disabledBitmapBundle, int itemKind, const string& shortHelpString,
        const string& longHelpString, void* clientData)
    {
        return GetToolbar()->AddTool(toolId, wxStr(label), ImageSet::BitmapBundle(bitmapBundle),
            ImageSet::BitmapBundle(disabledBitmapBundle), (wxItemKind)itemKind,
            wxStr(shortHelpString),
            wxStr(longHelpString), (wxObject*) clientData);
    }

    void* AuiToolBar::AddTool3(int toolId, ImageSet* bitmapBundle,
        ImageSet* disabledBitmapBundle, bool toggle, void* clientData,
        const string& shortHelpString, const string& longHelpString)
    {
        return GetToolbar()->AddTool(toolId, ImageSet::BitmapBundle(bitmapBundle),
            ImageSet::BitmapBundle(disabledBitmapBundle), toggle, (wxObject*) clientData,
            wxStr(shortHelpString), wxStr(longHelpString));
    }

    void* AuiToolBar::AddLabel(int toolId, const string& label, int width)
    {
        return GetToolbar()->AddLabel(toolId, wxStr(label), width);
    }

    void* AuiToolBar::AddControl(int toolId, void* control, const string& label)
    {
        auto result = GetToolbar()->AddControl((wxControl*) control, wxStr(label));
        result->SetId(toolId);
        return result;
    }

    void* AuiToolBar::AddSeparator(int toolId)
    {
        auto result = GetToolbar()->AddSeparator();
        result->SetId(toolId);
        return result;
    }

    void* AuiToolBar::AddSpacer(int toolId, int pixels)
    {
        auto result = GetToolbar()->AddSpacer(pixels);
        result->SetId(toolId);
        return result;
    }

    void* AuiToolBar::AddStretchSpacer(int toolId, int proportion)
    {
        auto result = GetToolbar()->AddStretchSpacer(proportion);
        result->SetId(toolId);
        return result;
    }

    bool AuiToolBar::Realize()
    {
        return GetToolbar()->Realize();
    }

    void* AuiToolBar::FindControl(int windowId)
    {
        return GetToolbar()->FindControl(windowId);
    }

    void* AuiToolBar::FindToolByPosition(int x, int y)
    {
        return GetToolbar()->FindToolByPosition(x, y);
    }

    void* AuiToolBar::FindToolByIndex(int idx)
    {
        return GetToolbar()->FindToolByIndex(idx);
    }

    void* AuiToolBar::FindTool(int toolId)
    {
        return GetToolbar()->FindTool(toolId);
    }

    void AuiToolBar::Clear()
    {
        GetToolbar()->Clear();
    }

    bool AuiToolBar::DestroyTool(int toolId)
    {
        return GetToolbar()->DestroyTool(toolId);
    }

    bool AuiToolBar::DestroyToolByIndex(int idx)
    {
        return GetToolbar()->DestroyToolByIndex(idx);
    }

    bool AuiToolBar::DeleteTool(int toolId)
    {
        return GetToolbar()->DeleteTool(toolId);
    }

    bool AuiToolBar::DeleteByIndex(int toolId)
    {
        return GetToolbar()->DeleteByIndex(toolId);
    }

    int AuiToolBar::GetToolKind(int toolId) 
    {
        auto tool = GetToolbar()->FindTool(toolId);
        return tool->GetKind();
    }

    int AuiToolBar::GetToolIndex(int toolId)
    {
        return GetToolbar()->GetToolIndex(toolId);
    }

    bool AuiToolBar::GetToolFits(int toolId)
    {
        return GetToolbar()->GetToolFits(toolId);
    }

    Rect AuiToolBar::GetToolRect(int toolId)
    {
        auto wxWindow = GetToolbar();
        auto rect = wxWindow->GetToolRect(toolId);
        auto dipRect = toDip(rect, wxWindow);
        return dipRect;
    }

    bool AuiToolBar::GetToolFitsByIndex(int toolId)
    {
        return GetToolbar()->GetToolFitsByIndex(toolId);
    }

    bool AuiToolBar::GetToolBarFits()
    {
        return GetToolbar()->GetToolBarFits();
    }

    void AuiToolBar::SetToolBitmapSizeInPixels(const Int32Size& size)
    {
        GetToolbar()->SetToolBitmapSize(size);
    }

    Int32Size AuiToolBar::GetToolBitmapSizeInPixels()
    {
        auto size = GetToolbar()->GetToolBitmapSize();
        return size;
    }

    bool AuiToolBar::GetOverflowVisible()
    {
        return GetToolbar()->GetOverflowVisible();
    }

    void AuiToolBar::SetOverflowVisible(bool visible)
    {
        GetToolbar()->SetOverflowVisible(visible);
    }

    bool AuiToolBar::GetGripperVisible()
    {
        return GetToolbar()->GetGripperVisible();
    }

    void AuiToolBar::SetGripperVisible(bool visible)
    {
        GetToolbar()->SetGripperVisible(visible);
    }

    void AuiToolBar::ToggleTool(int toolId, bool state)
    {
        GetToolbar()->ToggleTool(toolId, state);
    }

    bool AuiToolBar::GetToolToggled(int toolId)
    {
        return GetToolbar()->GetToolToggled(toolId);
    }

    void AuiToolBar::SetMargins(int left, int right, int top, int bottom)
    {
        GetToolbar()->SetMargins(left, right, top, bottom);
    }

    void AuiToolBar::EnableTool(int toolId, bool state)
    {
        GetToolbar()->EnableTool(toolId, state);
    }

    bool AuiToolBar::GetToolEnabled(int toolId)
    {
        return GetToolbar()->GetToolEnabled(toolId);
    }

    Int32Size AuiToolBar::GetToolMinSize(int tool_id)
    {
        wxAuiToolBarItem* item = GetToolbar()->FindTool(tool_id);
        if (!item)
            return Int32Size(-1,-1);
        auto& result = item->GetMinSize();
        return Int32Size(result.x, result.y);
    }

    void AuiToolBar::SetAlignment(int tool_id, int l)
    {
        wxAuiToolBarItem* item = GetToolbar()->FindTool(tool_id);
        if (!item)
            return;
        return item->SetAlignment(l);
    }

    int AuiToolBar::GetAlignment(int tool_id)
    {
        wxAuiToolBarItem* item = GetToolbar()->FindTool(tool_id);
        if (!item)
            return -1;
        return item->GetAlignment();
    }

    void AuiToolBar::SetToolMinSize(int tool_id, int width, int height)
    {
        wxAuiToolBarItem* item = GetToolbar()->FindTool(tool_id);
        if (!item)
            return;
        item->SetMinSize(wxSize(width, height));
    }

    void AuiToolBar::SetToolDropDown(int toolId, bool dropdown)
    {
        GetToolbar()->SetToolDropDown(toolId, dropdown);
    }

    bool AuiToolBar::GetToolDropDown(int toolId)
    {
        return GetToolbar()->GetToolDropDown(toolId);
    }

    void AuiToolBar::SetToolBorderPadding(int padding)
    {
        GetToolbar()->SetToolBorderPadding(padding);
    }

    int AuiToolBar::GetToolBorderPadding()
    {
        return GetToolbar()->GetToolBorderPadding();
    }

    void AuiToolBar::SetToolTextOrientation(int orientation)
    {
        GetToolbar()->SetToolTextOrientation(orientation);
    }

    int AuiToolBar::GetToolTextOrientation()
    {
        return GetToolbar()->GetToolTextOrientation();
    }

    void AuiToolBar::SetToolPacking(int packing)
    {
        GetToolbar()->SetToolPacking(packing);
    }

    int AuiToolBar::GetToolPacking()
    {
        return GetToolbar()->GetToolPacking();
    }

    void AuiToolBar::SetToolProportion(int toolId, int proportion)
    {
        GetToolbar()->SetToolProportion(toolId, proportion);
    }

    int AuiToolBar::GetToolProportion(int toolId)
    {
        return GetToolbar()->GetToolProportion(toolId);
    }

    void AuiToolBar::SetToolSeparation(int separation)
    {
        GetToolbar()->SetToolSeparation(separation);
    }

    int AuiToolBar::GetToolSeparation()
    {
        return GetToolbar()->GetToolSeparation();
    }

    void AuiToolBar::SetToolSticky(int toolId, bool sticky)
    {
        GetToolbar()->SetToolSticky(toolId, sticky);
    }

    bool AuiToolBar::GetToolSticky(int toolId)
    {
        return GetToolbar()->GetToolSticky(toolId);
    }

    string AuiToolBar::GetToolLabel(int toolId)
    {
        return wxStr(GetToolbar()->GetToolLabel(toolId));
    }

    void AuiToolBar::SetToolLabel(int toolId, const string& label)
    {
        GetToolbar()->SetToolLabel(toolId, wxStr(label));
    }

    string AuiToolBar::GetToolShortHelp(int toolId)
    {
        return wxStr(GetToolbar()->GetToolShortHelp(toolId));
    }

    void AuiToolBar::SetToolShortHelp(int toolId, const string& helpString)
    {
        GetToolbar()->SetToolShortHelp(toolId, wxStr(helpString));
    }

    string AuiToolBar::GetToolLongHelp(int toolId)
    {
        return wxStr(GetToolbar()->GetToolLongHelp(toolId));
    }

    void AuiToolBar::SetToolLongHelp(int toolId, const string& helpString)
    {
        GetToolbar()->SetToolLongHelp(toolId, wxStr(helpString));
    }

    uint64_t AuiToolBar::GetToolCount()
    {
        return GetToolbar()->GetToolCount();
    }

    void AuiToolBar::SetToolBitmap(int toolId, ImageSet* bitmapBundle)
    {
        GetToolbar()->SetToolBitmap(toolId, ImageSet::BitmapBundle(bitmapBundle));
    }
}
