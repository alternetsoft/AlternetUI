#include "Control.h"
#include "Application.h"
#include "Screenshot.h"
#include "Window.h"
#include "Popup.h"

namespace Alternet::UI
{
    /*static*/ Control::ControlsByWxWindowsMap Control::s_controlsByWxWindowsMap;

    wxString Control::GetMouseEventDesc(const wxMouseEvent& ev)
    {
        // click event
        wxString button;
        bool dbl, up;
        if (ev.LeftDown() || ev.LeftUp() || ev.LeftDClick())
        {
            button = "Left";
            dbl = ev.LeftDClick();
            up = ev.LeftUp();
        }
        else if (ev.MiddleDown() || ev.MiddleUp() || ev.MiddleDClick())
        {
            button = "Middle";
            dbl = ev.MiddleDClick();
            up = ev.MiddleUp();
        }
        else if (ev.RightDown() || ev.RightUp() || ev.RightDClick())
        {
            button = "Right";
            dbl = ev.RightDClick();
            up = ev.RightUp();
        }
        else if (ev.Aux1Down() || ev.Aux1Up() || ev.Aux1DClick())
        {
            button = "Aux1";
            dbl = ev.Aux1DClick();
            up = ev.Aux1Up();
        }
        else if (ev.Aux2Down() || ev.Aux2Up() || ev.Aux2DClick())
        {
            button = "Aux2";
            dbl = ev.Aux2DClick();
            up = ev.Aux2Up();
        }
        else if (ev.GetWheelRotation())
        {
            return wxString::Format("%s wheel rotation %+d",
                ev.GetWheelAxis() == wxMOUSE_WHEEL_VERTICAL ? "Vertical" : "Horizontal",
                ev.GetWheelRotation());
        }
        else
        {
            return "Unknown mouse event";
        }
        wxASSERT(!(dbl && up));

        return wxString::Format("%s mouse button %s",
            button,
            dbl ? "double clicked"
            : up ? "released" : "clicked");
    }

    bool Control::HasExtraStyle(long extra)
    {
        auto window = GetWxWindow();
        auto style = window->GetExtraStyle();
        return (style & extra) != 0;
    }

    void Control::SetExtraStyle(long extra, bool value)
    {
        auto window = GetWxWindow();
        auto style = window->GetExtraStyle();
        if (value)
            style |= extra;
        else
            style &= ~extra;
        window->SetExtraStyle(style);
    }

    bool Control::GetProcessIdle()
    {
        return HasExtraStyle(wxWS_EX_PROCESS_IDLE);
    }

    void Control::SetProcessIdle(bool value)
    {
        SetExtraStyle(wxWS_EX_PROCESS_IDLE, value);
    }

    int Control::DrawingFromDip(double value, void* window)
    {
        return fromDip(value, (wxWindow*)window);
    }

    double Control::DrawingDPIScaleFactor(void* window)
    {
        return ((wxWindow*)window)->GetDPIScaleFactor();
        //return GetDPIScaleFactor((wxWindow*)window);
    }

    double Control::DrawingToDip(int value, void* window)
    {
        return toDip(value, (wxWindow*)window);
    }

    double Control::DrawingFromDipF(double value, void* window)
    {
        return fromDipF(value, (wxWindow*)window);
    }

    bool Control::GetProcessUIUpdates()
    {
        return HasExtraStyle(wxWS_EX_PROCESS_UI_UPDATES);            
    }

    void Control::SetProcessUIUpdates(bool value)
    {
        SetExtraStyle(wxWS_EX_PROCESS_UI_UPDATES, value);
    }

    Control::Control() :
        _flags(ControlFlags::TabStop),
        _delayedFlags(
            *this,
            DelayedControlFlags::Visible,
            &Control::IsWxWindowCreated,
            {
                {DelayedControlFlags::Visible, std::make_tuple(&Control::RetrieveVisible,
                    &Control::ApplyVisible)},
                {DelayedControlFlags::Frozen, std::make_tuple(&Control::RetrieveFrozen,
                    &Control::ApplyFrozen)},
                {DelayedControlFlags::Enabled, std::make_tuple(&Control::RetrieveEnabled,
                    &Control::ApplyEnabled)},
            }),
            _bounds(*this, Rect(), &Control::IsWxWindowCreated, &Control::RetrieveBounds,
                &Control::ApplyBounds),
            _minimumSize(*this, Size(), &Control::IsWxWindowCreated,
                    &Control::RetrieveMinimumSize, &Control::ApplyMinimumSize),
            _maximumSize(*this, Size(), &Control::IsWxWindowCreated,
                &Control::RetrieveMaximumSize, &Control::ApplyMaximumSize),
            _horizontalScrollBarInfo(*this, ScrollInfo(), &Control::CanSetScrollbar,
                &Control::RetrieveHorizontalScrollBarInfo, &Control::ApplyHorizontalScrollBarInfo),
            _verticalScrollBarInfo(*this, ScrollInfo(), &Control::CanSetScrollbar,
                &Control::RetrieveVerticalScrollBarInfo, &Control::ApplyVerticalScrollBarInfo),
            _delayedValues({&_delayedFlags, &_bounds, 
                &_horizontalScrollBarInfo,
                &_verticalScrollBarInfo,&_minimumSize,&_maximumSize})
    {
    }

    bool Control::GetIsFocusable()
    {
        return GetWxWindow()->IsFocusable();
    }

    bool Control::GetCanAcceptFocus()
    {
        return GetWxWindow()->CanAcceptFocus();
    }

    void Control::ShowPopupMenu(void* menu, double x, double y)
    {
        auto window = GetWxWindow();

        auto byDefault = (x == -1 || y == -1);

        auto sx = byDefault ? -1 : fromDip(x, window);
        auto sy = byDefault ? -1 : fromDip(y, window);
        window->PopupMenu((wxMenu*)menu, sx, sy);
    }

    Size Control::RetrieveMinimumSize()
    {
        return _appliedMinimumSize;
    }

    void Control::LogRectMethod(wxString name, const Rect& value, const wxRect& wx)
    {
        auto dips = value.ToString();
        Int32Rect rect = wx;
        auto pxs = rect.ToString();
        Int32Rect resultRect = GetWxWindow()->GetRect();
        Int32Rect resultScreenRect = GetWxWindow()->GetScreenRect();
        wxString s = name + " dip " + dips + ", px " + pxs + ", result " + resultRect.ToString()
            + ", screen " + resultScreenRect.ToString();
        Application::Log(s);
    }

    void Control::LogSizeMethod(std::string methodName, const Size& value)
    {
        auto window = GetWxWindow();
        /*if (window->GetName() != wxDatePickerCtrlNameStr)
            return;*/
        auto minSize = Size(window->GetMinSize());
        auto maxSize = Size(window->GetMaxSize());
        auto s = methodName + ": " + value.ToString() + ", minSize: " + minSize.ToString() + 
            ", maxSize: " + maxSize.ToString();
        Application::Log(s);
    }

    void Control::ApplyMinimumSize(const Size& value)
    {
        //LogMethod("Before ApplyMinimumSize", value);
        //return; // !!
        auto window = GetWxWindow();
        if (value.Width <= 0 && value.Height <= 0)
        {
            if (window->GetMinSize() != wxDefaultSize)
                window->SetMinSize(wxDefaultSize);
            //LogMethod("After ApplyMinimumSize", value);
            return;
        }

        auto size = fromDip(value, window);
        window->SetMinSize(size);
        //LogMethod("After2 ApplyMinimumSize", value);
    }

    Size Control::GetMinimumSize()
    {
        return _minimumSize.Get();
    }

    void Control::SetMinimumSize(const Size& value)
    {
        //LogMethod("SetMinimumSize", value);
        //return; // !!
        _minimumSize.Set(value);
        _appliedMinimumSize = value;
        auto limited = CoerceSize(GetSize());
         if (limited.has_value())
            SetSize(limited.value());
    }

    optional<Size> Control::CoerceSize(const Size& value)
    {
        auto minSize = GetMinimumSize();
        auto maxSize = GetMaximumSize();

        bool limitNeeded = false;
        Size limitedSize = value;

        if (minSize.Width > 0 && minSize.Width > value.Width)
        {
            limitedSize.Width = minSize.Width;
            limitNeeded = true;
        }

        if (minSize.Height > 0 && minSize.Height > value.Height)
        {
            limitedSize.Height = minSize.Height;
            limitNeeded = true;
        }

        if (maxSize.Width > 0 && maxSize.Width < value.Width)
        {
            limitedSize.Width = maxSize.Width;
            limitNeeded = true;
        }

        if (maxSize.Height > 0 && maxSize.Height < value.Height)
        {
            limitedSize.Height = maxSize.Height;
            limitNeeded = true;
        }

        if (limitNeeded)
            return limitedSize;
        else
            return nullopt;
    }

    Size Control::GetMaximumSize()
    {
        return _maximumSize.Get();
    }

    void Control::SetMaximumSize(const Size& value)
    {
        //LogMethod("SetMaximumSize", value);
        //return; // !!
        _maximumSize.Set(value);
        _appliedMaximumSize = value;
        auto limited = CoerceSize(GetSize());
        if (limited.has_value())
            SetSize(limited.value());
    }

    Size Control::RetrieveMaximumSize()
    {
        return _appliedMaximumSize;
    }

    void Control::ApplyMaximumSize(const Size& value)
    {
        //LogMethod("ApplyMaximumSize", value);
        //return; // !!
        auto window = GetWxWindow();
        if (value.Width <= 0 && value.Height <= 0)
        {
            if(window->GetMaxSize() != wxDefaultSize)
                window->SetMaxSize(wxDefaultSize);
            //LogMethod("After ApplyMaximumSize", value);
            return;
        }
        auto size = fromDip(value, window);
        window->SetMaxSize(size);
        //LogMethod("After2 ApplyMaximumSize", value);
    }

    int Control::GetId()
    {
        return GetWxWindow()->GetId();
    }

    void Control::SetId(int value)
    {
        GetWxWindow()->SetId(value);
    }

    string Control::GetName()
    {
        return wxStr(GetWxWindow()->GetName());
    }

    void Control::SetName(const string& value)
    {
        GetWxWindow()->SetName(wxStr(value));
    }

    int Control::GetBorderStyle()
    {
        return _borderStyle;
    }
    
    void Control::SetBorderStyle(int value)
    {
        if (_borderStyle == value)
            return;
        _borderStyle = value;
        RecreateWxWindowIfNeeded();
    }

    Control::~Control()
    {
        DestroyDropTarget();
        DestroyWxWindow();
    }

    bool Control::CanSetScrollbar()
    {
        return IsWxWindowCreated() && GetIsScrollable();
    }

    void Control::Destroy()
    {
        DestroyWxWindow();
    }

    bool Control::GetHasWindowCreated()
    {
        return IsWxWindowCreated();
    }

    void* Control::GetHandle()
    {
#ifdef  __WXMSW__
        return GetWxWindow()->GetHWND();
#else
        return nullptr;
#endif
    }

    bool Control::GetIsHandleCreated()
    {
#ifdef  __WXMSW__
        return IsWxWindowCreated() && (GetWxWindow()->GetHWND() !=0);
#else
        return IsWxWindowCreated();
#endif
    }

    bool Control::GetIsWxWidgetCreated()
    {
        return IsWxWindowCreated();
    }


    void Control::SaveScreenshot(const string& fileName)
    {
        UI::SaveScreenshot(GetWxWindow(), fileName);
    }

    void Control::OnDestroy(wxWindowDestroyEvent& event)
    {
        event.Skip();
        auto app = Application::GetCurrent();
        if (app == nullptr || app->GetInUixmlPreviewerMode())
            return; // HACK. This gets invoked by wxWidgets on a dead this pointer.

        auto wxWindow = event.GetWindow();
        if (wxWindow != _wxWindow)
        {
            if (IsRecreatingWxWindow())
                SetRecreatingWxWindow(false);
            return;
        }

        if (!IsRecreatingWxWindow())
            _wxWindow = nullptr;

        wxWindow->Unbind(wxEVT_IDLE, &Control::OnIdle, this);
        wxWindow->Unbind(wxEVT_PAINT, &Control::OnPaint, this);
        //wxWindow->Unbind(wxEVT_ERASE_BACKGROUND, &Control::OnEraseBackground, this);
        wxWindow->Unbind(wxEVT_DESTROY, &Control::OnDestroy, this);
        wxWindow->Unbind(wxEVT_SHOW, &Control::OnVisibleChanged, this);
        wxWindow->Unbind(wxEVT_MOUSE_CAPTURE_LOST, &Control::OnMouseCaptureLost, this);
        wxWindow->Unbind(wxEVT_ENTER_WINDOW, &Control::OnMouseEnter, this);
        wxWindow->Unbind(wxEVT_LEAVE_WINDOW, &Control::OnMouseLeave, this);
        wxWindow->Unbind(wxEVT_ACTIVATE, &Control::OnActivate, this);
        wxWindow->Unbind(wxEVT_SIZE, &Control::OnSizeChanged, this);
        wxWindow->Unbind(wxEVT_SET_FOCUS, &Control::OnGotFocus, this);
        wxWindow->Unbind(wxEVT_KILL_FOCUS, &Control::OnLostFocus, this);
        wxWindow->Unbind(wxEVT_LEFT_UP, &Control::OnMouseLeftUp, this);

        if (bindScrollEvents) 
        {
            wxWindow->Unbind(wxEVT_SCROLLWIN_TOP, &Control::OnScrollTop, this);
            wxWindow->Unbind(wxEVT_SCROLLWIN_BOTTOM, &Control::OnScrollBottom, this);
            wxWindow->Unbind(wxEVT_SCROLLWIN_LINEUP, &Control::OnScrollLineUp, this);
            wxWindow->Unbind(wxEVT_SCROLLWIN_LINEDOWN, &Control::OnScrollLineDown, this);
            wxWindow->Unbind(wxEVT_SCROLLWIN_PAGEUP, &Control::OnScrollPageUp, this);
            wxWindow->Unbind(wxEVT_SCROLLWIN_PAGEDOWN, &Control::OnScrollPageDown, this);
            wxWindow->Unbind(wxEVT_SCROLLWIN_THUMBTRACK, &Control::OnScrollThumbTrack, this);
            wxWindow->Unbind(wxEVT_SCROLLWIN_THUMBRELEASE, &Control::OnScrollThumbRelease, this);
        }

        RemoveWxWindowControlAssociation(wxWindow);

        OnWxWindowDestroyed(wxWindow);
        RaiseEvent(ControlEvent::Destroyed);

        if (IsRecreatingWxWindow())
            SetRecreatingWxWindow(false);
        RaiseEvent(ControlEvent::HandleDestroyed);
    }

    bool Control::GetIsActive()
    {
        return _flags.IsSet(ControlFlags::Active);
    }

    void Control::OnActivate(wxActivateEvent& event)
    {
        event.Skip();
        bool active = event.GetActive();
        _flags.Set(ControlFlags::Active, active);

        if (active)
            RaiseEvent(ControlEvent::Activated);
        else
            RaiseEvent(ControlEvent::Deactivated);
    }

    bool Control::GetIsScrollable()
    {
        return _flags.IsSet(ControlFlags::IsScrollable);
    }

    void Control::SetIsScrollable(bool value)
    {
        if (value == GetIsScrollable())
            return;

        _flags.Set(ControlFlags::IsScrollable, value);
        RecreateWxWindowIfNeeded();
    }

#define ScrollEventType_SmallDecrement 0
#define ScrollEventType_SmallIncrement 1
#define ScrollEventType_LargeDecrement 2
#define ScrollEventType_LargeIncrement 3
#define ScrollEventType_ThumbPosition 4
#define ScrollEventType_ThumbTrack 5
#define ScrollEventType_First 6
#define ScrollEventType_Last 7
#define ScrollEventType_EndScroll 8

    void Control::OnScrollTop(wxScrollWinEvent& event)
    {
        ApplyScroll(ScrollEventType_First, event, 0);
        event.Skip();
    }

    void Control::OnScrollBottom(wxScrollWinEvent& event)
    {
        auto scrollInfo = GetScrollInfoDelayedValue(event).Get();
        ApplyScroll(ScrollEventType_Last, event, scrollInfo.maximum);
        event.Skip();
    }

    void Control::OnScrollLineUp(wxScrollWinEvent& event)
    {
        auto scrollInfo = GetScrollInfoDelayedValue(event).Get();
        ApplyScroll(ScrollEventType_SmallDecrement, event, scrollInfo.value - 1);
        event.Skip();
    }

    void Control::OnScrollLineDown(wxScrollWinEvent& event)
    {
        auto scrollInfo = GetScrollInfoDelayedValue(event).Get();
        ApplyScroll(ScrollEventType_SmallIncrement, event, scrollInfo.value + 1);
        event.Skip();
    }

    void Control::OnScrollPageUp(wxScrollWinEvent& event)
    {
        auto scrollInfo = GetScrollInfoDelayedValue(event).Get();
        ApplyScroll(ScrollEventType_LargeDecrement, event, scrollInfo.value - scrollInfo.largeChange);
        event.Skip();
    }

    void Control::OnScrollPageDown(wxScrollWinEvent& event)
    {
        auto scrollInfo = GetScrollInfoDelayedValue(event).Get();
        ApplyScroll(ScrollEventType_LargeIncrement, event, scrollInfo.value + scrollInfo.largeChange);
    }

    void Control::OnScrollThumbTrack(wxScrollWinEvent& event)
    {
        ApplyScroll(ScrollEventType_ThumbTrack, event, event.GetPosition());
        event.Skip();
    }

    void Control::OnScrollThumbRelease(wxScrollWinEvent& event)
    {
        ApplyScroll(ScrollEventType_ThumbPosition, event, event.GetPosition());
        event.Skip();
    }

    DelayedValue<Control, Control::ScrollInfo>& Control::GetScrollInfoDelayedValue(
        const wxScrollWinEvent& event)
    {
        return GetScrollInfoDelayedValue(
            event.GetOrientation() == wxHORIZONTAL ?
            ScrollBarOrientation::Horizontal : ScrollBarOrientation::Vertical);
    }

    void Control::ApplyScroll(int evtKind, wxScrollWinEvent& event, int position)
    {
        auto wxOrientation = event.GetOrientation();
        GetWxWindow()->SetScrollPos(wxOrientation, position);

        auto getEvent = [&]()
        {
            switch (wxOrientation)
            {
            case wxHORIZONTAL:
                return ControlEvent::HorizontalScrollBarValueChanged;
            case wxVERTICAL:
                return ControlEvent::VerticalScrollBarValueChanged;
            default:
                throwExNoInfo;
            }
        };
        _scrollbarEvtKind = evtKind;
        _scrollbarEvtPosition = event.GetPosition();
        RaiseEvent(getEvent());
    }

    int Control::GetScrollBarEvtPosition()
    {
        return _scrollbarEvtPosition;
    }

    int Control::GetScrollBarEvtKind()
    {
        return _scrollbarEvtKind;
    }

    void Control::OnGotFocus(wxFocusEvent& event)
    {
        event.Skip();
        RaiseEvent(ControlEvent::GotFocus);
    }

    void Control::OnLostFocus(wxFocusEvent& event)
    {
        event.Skip();
        RaiseEvent(ControlEvent::LostFocus);
    }

    void Control::DestroyWxWindow()
    {
        if (_wxWindow != nullptr)
        {
            OnBeforeDestroyWxWindow();
            _wxWindow->Destroy();
            _wxWindow = nullptr;
        }
    }

    DrawingContext* Control::OpenPaintDrawingContext()
    {
        //auto dc = new wxAutoBufferedPaintDC(GetWxWindow());
        auto dc = new wxPaintDC(GetWxWindow());
        
        // // todo: work out a proper background brush solution (including transparency, patterns, etc)
        // auto oldBackground = dc->GetBackground();
        // dc->SetBackground(wxSystemSettings::GetColour(wxSystemColour::wxSYS_COLOUR_BTNFACE));
        // dc->Clear();
        // dc->SetBackground(oldBackground);

        return new DrawingContext(dc);
    }

    DrawingContext* Control::OpenClientDrawingContext()
    {
        return new DrawingContext(new wxClientDC(GetWxWindow()));
    }

    DrawingContext* Control::OpenClientDrawingContextForWindow(void* window)
    {
        return new DrawingContext(new wxClientDC((wxWindow*)window));
    }

    DrawingContext* Control::OpenDrawingContextForDC(void* dc, bool deleteDc)
    {
        auto result = new DrawingContext((wxDC*)dc);
        if (!deleteDc)
            result->SetDoNotDeleteDC(true);
        return result;
    }

    DrawingContext* Control::OpenPaintDrawingContextForWindow(void* window)
    {
        return new DrawingContext(new wxPaintDC((wxWindow*)window));
    }

    Control* Control::GetParent()
    {
        return _parent;
    }

    Control* Control::GetParentRefCounted()
    {
        if (_parent != nullptr)
            _parent->AddRef();
        return _parent;
    }

    void Control::BeginInit()
    {
        if (_flags.IsSet(ControlFlags::InitInProgress))
            throwExInvalidOpWithInfo(wxStr("Control::BeginInit"));

        _flags.Set(ControlFlags::InitInProgress, true);

        OnBeginInit();
    }

    void Control::EndInit()
    {
        if (!_flags.IsSet(ControlFlags::InitInProgress))
            throwExInvalidOpWithInfo(wxStr("Control::EndInit"));

        _flags.Set(ControlFlags::InitInProgress, false);

        if (_flags.IsSet(ControlFlags::PostInitWxWindowRecreationPending))
        {
            RecreateWxWindowIfNeeded();
            _flags.Set(ControlFlags::PostInitWxWindowRecreationPending, false);
        }

        for (auto& action : _postInitActions)
        {
            action();
        }

        _postInitActions.clear();

        OnEndInit();
    }

    void Control::OnBeginInit()
    {
    }

    void Control::OnEndInit()
    {
    }

    bool Control::IsInitInProgress()
    {
        return _flags.IsSet(ControlFlags::InitInProgress);
    }

    /*static*/ int Control::GetDoDragDropFlags(DragDropEffects allowedEffects)
    {
        #pragma warning(suppress: 26813)
        if (allowedEffects == DragDropEffects::Copy)
            return wxDrag_CopyOnly; // allow only copying

        if ((allowedEffects & DragDropEffects::Move) != DragDropEffects::None)
            return wxDrag_AllowMove; // allow moving (copying is always allowed)

        return wxDrag_DefaultMove; // the default operation is move, not copy
    }

    DragDropEffects Control::DoDragDrop(UnmanagedDataObject* data,
        DragDropEffects allowedEffects)
    {
        wxDropSource dragSource(GetWxWindow());
        auto dataObjectComposite = data->GetDataObjectComposite();
        dragSource.SetData(*dataObjectComposite);
        auto result = dragSource.DoDragDrop(GetDoDragDropFlags(allowedEffects));
        return GetDragDropEffects(result);
    }

    bool Control::GetAllowDrop()
    {
        return _dropTarget != nullptr;
    }

    void Control::SetAllowDrop(bool value)
    {
        if (value)
            CreateDropTarget();
        else
            DestroyDropTarget();
    }

    void Control::CreateDropTarget()
    {
        if (_dropTarget != nullptr)
            throwExNoInfo;

        _dropTarget = new DropTarget(this);
        GetWxWindow()->SetDropTarget(_dropTarget);
    }

    void Control::Raise()
    {
        GetWxWindow()->Raise();
    }

    void Control::Lower()
    {
        GetWxWindow()->Lower();
    }

    void Control::DestroyDropTarget()
    {
        if (_dropTarget != nullptr)
        {
            GetWxWindow()->SetDropTarget(nullptr);
            delete _dropTarget;
            _dropTarget = nullptr;
        }
    }

    int Control::GetLayoutDirection()
    {
        return GetWxWindow()->GetLayoutDirection();
    }
    
    void Control::SetLayoutDirection(int value)
    {
        GetWxWindow()->SetLayoutDirection((wxLayoutDirection)value);
    }

    void* Control::GetWxWidget() 
    {
        return GetWxWindow();
    }

    wxWindow* Control::GetWxWindow()
    {
        if (_wxWindow == nullptr)
            CreateWxWindow();

        if (_wxWindow == nullptr)
            throwExInvalidOpWithInfo(wxStr("Control::GetWxWindow"));

        return _wxWindow;
    }

    void Control::Invalidate()
    {
        if (!IsWxWindowCreated())
            return;

        auto wxWindow = GetWxWindow();
        wxWindow->Refresh();
    }

    void Control::Update()
    {
        if (!IsWxWindowCreated())
            return;

        auto wxWindow = GetWxWindow();
        wxWindow->Update();
    }

    void Control::SendSizeEvent()
    {
        if (!IsWxWindowCreated())
            return;

        auto wxWindow = GetWxWindow();
        wxWindow->SendSizeEvent();
    }

    optional<string> Control::GetToolTip()
    {
        return _toolTip;
    }

    void Control::SetToolTip(optional<string> value)
    {
        _toolTip = value;

        if (IsWxWindowCreated())
            ApplyToolTip();

        OnToolTipChanged();
    }

    void Control::ApplyToolTip()
    {
        if (_wxWindow == nullptr)
            throwExInvalidOpWithInfo(wxStr("Control::ApplyToolTip"));

        if (_toolTip == nullopt)
            _wxWindow->UnsetToolTip();
        else
            _wxWindow->SetToolTip(wxStr(_toolTip.value()));
    }

    void Control::OnParentChanged()
    {
    }

    void Control::OnAnyParentChanged()
    {
    }

    void Control::OnToolTipChanged()
    {
    }

    bool Control::GetAcceptsFocus()
    {
        return _acceptsFocus;
    }

    void Control::SetAcceptsFocus(bool value)
    {
        if (_acceptsFocus == value)
            return;
        _acceptsFocus = value;
        RecreateWxWindowIfNeeded();
    }

    bool Control::GetAcceptsFocusAll()
    {
        return _acceptsFocus && _acceptsFocusFromKeyboard && _acceptsFocusRecursively;
    }

    void Control::SetAcceptsFocusAll(bool value)
    {
        if (_acceptsFocus == value && _acceptsFocusFromKeyboard == value
            && _acceptsFocusRecursively == value)
            return;
        _acceptsFocus = value;
        _acceptsFocusFromKeyboard = value;
        _acceptsFocusRecursively = value;
        RecreateWxWindowIfNeeded();
    }

    bool Control::GetAcceptsFocusFromKeyboard()
    {
        return _acceptsFocusFromKeyboard;
    }

    void Control::SetAcceptsFocusFromKeyboard(bool value)
    {
        if (_acceptsFocusFromKeyboard == value)
            return;
        _acceptsFocusFromKeyboard = value;
        RecreateWxWindowIfNeeded();
    }

    bool Control::GetAcceptsFocusRecursively()
    {
        return _acceptsFocusRecursively;
    }

    void Control::SetAcceptsFocusRecursively(bool value)
    {
        if (_acceptsFocusRecursively == value)
            return;
        _acceptsFocusRecursively = value;
        RecreateWxWindowIfNeeded();
    }

    void Control::CreateWxWindow()
    {
        _flags.Set(ControlFlags::CreatingWxWindow, true);
        _flags.Set(ControlFlags::DestroyingWxWindow, false);
        wxWindow* parentingWxWindow = nullptr;

        if (_parent == nullptr)
        {
            parentingWxWindow = ParkingWindow::GetWindow();
            _wxWindow = CreateWxWindowCore(parentingWxWindow);
        }
        else
        {
            parentingWxWindow = _parent->GetParentingWxWindow(this);
            _wxWindow = CreateWxWindowCore(parentingWxWindow);
        }

        ApplyToolTip();

        if (GetUserPaint())
        {
            _wxWindow->SetDoubleBuffered(true);
            //_wxWindow->SetBackgroundStyle(wxBG_STYLE_PAINT);
        }

        if (!GetTabStop())
            _wxWindow->DisableFocusFromKeyboard();

        _wxWindow->Bind(wxEVT_ACTIVATE, &Control::OnActivate, this);
        _wxWindow->Bind(wxEVT_PAINT, &Control::OnPaint, this);
        //_wxWindow->Bind(wxEVT_ERASE_BACKGROUND, &Control::OnEraseBackground, this);
        _wxWindow->Bind(wxEVT_DESTROY, &Control::OnDestroy, this);
        _wxWindow->Bind(wxEVT_SHOW, &Control::OnVisibleChanged, this);
        _wxWindow->Bind(wxEVT_MOUSE_CAPTURE_LOST, &Control::OnMouseCaptureLost, this);
        _wxWindow->Bind(wxEVT_ENTER_WINDOW, &Control::OnMouseEnter, this);
        _wxWindow->Bind(wxEVT_LEAVE_WINDOW, &Control::OnMouseLeave, this);
        _wxWindow->Bind(wxEVT_SIZE, &Control::OnSizeChanged, this);
        _wxWindow->Bind(wxEVT_SET_FOCUS, &Control::OnGotFocus, this);
        _wxWindow->Bind(wxEVT_KILL_FOCUS, &Control::OnLostFocus, this);
        _wxWindow->Bind(wxEVT_LEFT_UP, &Control::OnMouseLeftUp, this);
        _wxWindow->Bind(wxEVT_IDLE, &Control::OnIdle, this);

        if (bindScrollEvents)
        {
            _wxWindow->Bind(wxEVT_SCROLLWIN_TOP, &Control::OnScrollTop, this);
            _wxWindow->Bind(wxEVT_SCROLLWIN_BOTTOM, &Control::OnScrollBottom, this);
            _wxWindow->Bind(wxEVT_SCROLLWIN_LINEUP, &Control::OnScrollLineUp, this);
            _wxWindow->Bind(wxEVT_SCROLLWIN_LINEDOWN, &Control::OnScrollLineDown, this);
            _wxWindow->Bind(wxEVT_SCROLLWIN_PAGEUP, &Control::OnScrollPageUp, this);
            _wxWindow->Bind(wxEVT_SCROLLWIN_PAGEDOWN, &Control::OnScrollPageDown, this);
            _wxWindow->Bind(wxEVT_SCROLLWIN_THUMBTRACK, &Control::OnScrollThumbTrack, this);
            _wxWindow->Bind(wxEVT_SCROLLWIN_THUMBRELEASE, &Control::OnScrollThumbRelease, this);
        }

        AssociateControlWithWxWindow(_wxWindow, this);

        OnWxWindowCreated();
        _delayedValues.ApplyIfPossible();

        _flags.Set(ControlFlags::CreatingWxWindow, false);

        for (auto child : _children)
            child->UpdateWxWindowParent();
        RaiseEvent(ControlEvent::HandleCreated);
    }

    void Control::RecreateWindow() 
    {
        RecreateWxWindowIfNeeded();
    }

    void Control::BeginIgnoreRecreate() 
    {
        _ignoreRecreate++;
    }

    void Control::EndIgnoreRecreate()
    {
        _ignoreRecreate--;
        if (_ignoreRecreate == 0)
            RecreateWxWindowIfNeeded();
    }

    void Control::RecreateWxWindowIfNeeded()
    {
        if (_ignoreRecreate>0 || !IsWxWindowCreated())
            return;

        if (_disableRecreateCounter > 0)
            Application::Log("Native window recreated");

        SetRecreatingWxWindow(true);
        DestroyWxWindow();
        CreateWxWindow();
    }

    void Control::OnWxWindowCreated()
    {
    }

    void Control::OnBeforeDestroyWxWindow()
    {
        _flags.Set(ControlFlags::DestroyingWxWindow, true);
        _delayedValues.ReceiveIfPossible();
    }

    void Control::OnWxWindowDestroyed(wxWindow* window)
    {
    }

    DelayedValues& Control::GetDelayedValues()
    {
        return _delayedValues;
    }

    bool Control::IsWxWindowCreated()
    {
        return _wxWindow != nullptr;
    }

    void Control::SetBackgroundColor(const Color& value)
    {
        GetWxWindow()->SetBackgroundColour(value);
    }

    Color Control::GetBackgroundColor()
    {
        return GetWxWindow()->GetBackgroundColour();
    }

    void Control::SetForegroundColor(const Color& value)
    {
        GetWxWindow()->SetForegroundColour(value);
    }

    Color Control::GetForegroundColor()
    {
        return GetWxWindow()->GetForegroundColour();
    }

    void Control::SetMouseCapture(bool value)
    {
        auto wxWindow = GetWxWindow();
        if (value)
        {
            if (!(wxWindow->HasCapture()))
                wxWindow->CaptureMouse();
        }
        else
        {
            if (wxWindow->HasCapture())
                wxWindow->ReleaseMouse();
        }
    }

    void Control::SendMouseDownEvent(int x, int y) 
    {
        auto window = GetWxWindow();
        wxMouseEvent ev = wxMouseEvent(wxEVT_LEFT_DOWN);
        ev.SetX(x);ev.SetY(y);
        window->GetEventHandler()->AddPendingEvent(ev);
    }

    void Control::SendMouseUpEvent(int x, int y)
    {
        auto window = GetWxWindow();
        wxMouseEvent ev = wxMouseEvent(wxEVT_LEFT_UP);
        ev.SetX(x);ev.SetY(y);
        window->GetEventHandler()->AddPendingEvent(ev);
    }

    void Control::NotifyCaptureLost()
    {
        wxWindowBaseUnprotected::NotifyCaptureLost();
    }

    bool Control::GetIsMouseCaptured()
    {
        return GetWxWindow()->HasCapture();
    }

    bool Control::GetIsMouseOver()
    {
        auto wxWindow = GetWxWindow();

        wxPoint pt;
        auto window = wxFindWindowAtPointer(pt);
        while (window != nullptr)
        {
            if (window == wxWindow)
                return true;
            window = window->GetParent();
        }

        return false;
    }

    wxWindow* Control::GetParentingWxWindow(Control* child)
    {
        return GetWxWindow();
    }

    void Control::ScheduleRecreateWxWindow(std::function<void()> postRecreateAction)
    {
        if (!_flags.IsSet(ControlFlags::InitInProgress))
        {
            RecreateWxWindowIfNeeded();
            postRecreateAction();
            return;
        }

        _flags.Set(ControlFlags::PostInitWxWindowRecreationPending, true);

        _postInitActions.push_back([postRecreateAction]
            {
                postRecreateAction();
            });
    }

    void Control::ScheduleRecreateWxWindow()
    {
        ScheduleRecreateWxWindow([] {});
    }

    bool Control::RetrieveVisible()
    {
        return GetWxWindow()->IsShown();
    }

    void Control::CenterOnParent(int orientation)
    {
        GetWxWindow()->CenterOnParent(orientation);
    }

    void Control::ApplyVisible(bool value)
    {
        if (value)
            ShowCore();
        else
            HideCore();
    }

    void Control::ShowCore()
    {
        //if (Application::GetCurrent()->GetInUixmlPreviewerMode())
        //    return;

        GetWxWindow()->Show();
    }

    void Control::HideCore()
    {
        GetWxWindow()->Hide();
    }

    bool Control::IsDestroyingWxWindow()
    {
        return _flags.IsSet(ControlFlags::DestroyingWxWindow);
    }

    bool Control::IsRecreatingWxWindow()
    {
        return _recreatingWxWindowCounter > 0;
    }

    bool Control::RetrieveEnabled()
    {
        return GetWxWindow()->IsEnabled();
    }

    void Control::ApplyEnabled(bool value)
    {
        auto wxWindow = GetWxWindow();
        if (value)
            wxWindow->Enable();
        else
            wxWindow->Disable();
    }

    bool Control::RetrieveFrozen()
    {
        return GetWxWindow()->IsFrozen();
    }

    void Control::Freeze()
    {
        GetWxWindow()->Freeze();
    }

    void Control::Thaw() 
    {
        GetWxWindow()->Thaw();
    }

    void Control::ApplyFrozen(bool value)
    {
        auto window = GetWxWindow();
        if (value)
        {
            if (!window->IsFrozen())
                window->Freeze();
        }
        else
        {
            if (window->IsFrozen())
                window->Thaw();
        }
    }

    std::vector<Control*> Control::GetChildren()
    {
        return _children;
    }

    bool Control::GetDoNotDestroyWxWindow()
    {
        return _flags.IsSet(ControlFlags::DoNotDestroyWxWindow);
    }

    void Control::SetDoNotDestroyWxWindow(bool value)
    {
        _flags.Set(ControlFlags::DoNotDestroyWxWindow, value);
    }

    bool Control::GetIsBold()
    {
        auto w = GetWxWindow();
        auto weight = w->GetFont().GetWeight();
        if (weight > wxFontWeight::wxFONTWEIGHT_NORMAL)
            return true;
        else
            return false;
    }
    
    void Control::SetIsBold(bool value)
    {
        if (GetIsBold() == value)
            return;
        auto w = GetWxWindow();
        if (value)
            w->SetFont(w->GetFont().Bold());
        else
        {
            auto font = w->GetFont();
            font.SetWeight(wxFONTWEIGHT_NORMAL);
            w->SetFont(font);
        }
    }

    void Control::UnsetToolTip()
    {
        GetWxWindow()->UnsetToolTip();
    }

    Font* Control::GetFont()
    {
        return new Font(GetWxWindow()->GetFont());
    }

    void Control::SetFont(Font* value)
    {
        auto w = GetWxWindow();
        if (value == nullptr)
        {
            w->SetFont(wxNullFont);
            return;
        }
        w->SetFont(value->GetWxFont());
    }

    void Control::BeginUpdate()
    {
        _beginUpdateCount++;
        if (_beginUpdateCount == 1)
        {
            _delayedFlags.Set(DelayedControlFlags::Frozen, true);
        }
    }

    void Control::EndUpdate()
    {
        if (_beginUpdateCount <= 0)
            throwEx(u"EndUpdate() without matching BeginUpdate()");

        _beginUpdateCount--;
        if (_beginUpdateCount == 0)
        {
            _delayedFlags.Set(DelayedControlFlags::Frozen, false);
        }
    }

    Size Control::GetClientSize()
    {
        if (!_flags.IsSet(ControlFlags::ClientSizeCacheValid))
        {
            _clientSizeCache = GetClientSizeCore();
            _flags.Set(ControlFlags::ClientSizeCacheValid, true);
        }

        return _clientSizeCache;
    }

    Size Control::GetClientSizeCore()
    {
        return SizeToClientSize(GetBounds().GetSize());
    }

    Size Control::GetDPI() 
    {
        auto window = GetWxWindow();
        auto size = window->GetDPI();
        return Size(size.x, size.y);
    }

    void Control::SetClientSize(const Size& value)
    {
        SetBounds(Rect(GetBounds().GetLocation(), ClientSizeToSize(value)));
    }

    Size Control::ClientSizeToSize(const Size& clientSize)
    {
        // todo: On Linux ClientSize is not reported correctly until the window is shown
        // See https://forums.wxwidgets.org/viewtopic.php?f=1&t=47439
        // So on Linux ClientSizeToSize and SizeToClientSize will return the input value
        // until the window is shown.

        auto window = GetWxWindow();
        return toDip(GetWxWindow()->ClientToWindowSize(fromDip(clientSize, window)), window);
    }

    Size Control::SizeToClientSize(const Size& size)
    {
        auto window = GetWxWindow();
        if (window->GetHandle() == nullptr)
        {
            // On macOS, in case where the peer is not created, calling WindowToClientSize causes a crash.
            return size;
        }
        
        return toDip(window->WindowToClientSize(fromDip(size, window)), window);
    }

    Thickness Control::GetIntrinsicLayoutPadding()
    {
        return Thickness();
    }

    Thickness Control::GetIntrinsicPreferredSizePadding()
    {
        return Thickness();
    }

    bool Control::EventsSuspended()
    {
        return _flags.IsSet(ControlFlags::CreatingWxWindow);
    }

    void Control::OnIdle(wxIdleEvent& event)
    {
        event.Skip();
        RaiseEvent(ControlEvent::Idle);
    }

    void Control::OnPaint(wxPaintEvent& event)
    {
        event.Skip();

        /*if (GetUserPaint())
        {
            wxPaintDC dc(GetWxWindow());
            
            auto background = GetBackgroundColor();
            wxColor color;
            if (background.IsEmpty())
                color = wxSystemSettings::GetColour(wxSystemColour::wxSYS_COLOUR_BTNFACE);
            else
                color = background;
            
            dc.SetBackground(wxBrush(color));
            dc.Clear();
        }*/

        RaiseEvent(ControlEvent::Paint);
    }

    void Control::OnEraseBackground(wxEraseEvent& event)
    {
        event.Skip();
    }

    void Control::OnMouseCaptureLost(wxEvent& event)
    {
        RaiseEvent(ControlEvent::MouseCaptureLost);
    }

    void Control::OnMouseEnter(wxMouseEvent& event)
    {
        event.Skip();
        RaiseEvent(ControlEvent::MouseEnter);

        auto window = GetParent();
        while (window != nullptr)
        {
            if (window->GetIsMouseOver())
                window->RaiseEvent(ControlEvent::MouseEnter);

            window = window->GetParent();
        }
    }

    void Control::OnMouseLeftUp(wxMouseEvent& event)
    {
        event.Skip();
    }

    void Control::OnMouseLeave(wxMouseEvent& event)
    {
        event.Skip();
        RaiseEvent(ControlEvent::MouseLeave);

        auto window = GetParent();
        while (window != nullptr)
        {
            if(!window->GetIsMouseOver())
                window->RaiseEvent(ControlEvent::MouseLeave);

            window = window->GetParent();
        }
    }

    void Control::OnVisibleChanged(wxShowEvent& event)
    {
        event.Skip();

        // For some reason this event is being called while destroying the window, despite the Unbind call prior to destruction.
        auto window = dynamic_cast<wxWindow*>(event.GetEventObject());
        if (window->IsBeingDeleted())
            return;

        RaiseEvent(ControlEvent::VisibleChanged);
    }

    void Control::OnSizeChanged(wxSizeEvent& event)
    {
        event.Skip();

        _flags.Set(ControlFlags::ClientSizeCacheValid, false);

        RaiseEvent(ControlEvent::SizeChanged);
    }

    void Control::UpdateWxWindowParent()
    {
        auto wxWindow = GetWxWindow();
        auto parkingWindow = ParkingWindow::GetWindow();

        auto parent = GetParent();
        if (parent == nullptr)
        {
            if (wxWindow->GetParent() != parkingWindow)
                SetWxWindowParent(parkingWindow);
        }
        else
        {
            auto parentWxWindow = parent->GetParentingWxWindow(this);
            auto oldParent = wxWindow->GetParent();
            if (oldParent != parentWxWindow)
                SetWxWindowParent(parentWxWindow);
        }

        OnParentChanged();
        NotifyAllChildrenOnParentChange();
    }

    void Control::NotifyAllChildrenOnParentChange()
    {
        OnAnyParentChanged();
        for (auto child : _children)
            child->NotifyAllChildrenOnParentChange();
    }

    void Control::SetRecreatingWxWindow(bool value)
    {
        if (value)
            _recreatingWxWindowCounter++;
        else if (_recreatingWxWindowCounter > 0)
            _recreatingWxWindowCounter--;
    }

    void Control::SetWxWindowParent(wxWindow* parent)
    {
        auto wxWindow = GetWxWindow();
        if (wxWindow != nullptr)
            wxWindow->Reparent(parent);
    }

    Size Control::GetSize()
    {
        return GetBounds().GetSize();
    }

    void Control::SetSize(const Size& value)
    {
        SetBounds(Rect(GetLocation(), value));
    }

    Point Control::GetLocation()
    {
        return GetBounds().GetLocation();
    }

    void Control::SetLocation(const Point& value)
    {
        SetBounds(Rect(value, GetSize()));
    }

    void Control::DisableRecreate()
    {
        auto wxWindow = GetWxWindow();
        _disableRecreateCounter++;
    }

    void Control::EnableRecreate()
    {
        _disableRecreateCounter--;
    }

    Rect Control::RetrieveBounds()
    {
        auto wxWindow = GetWxWindow();
        return toDip(wxWindow->GetRect(), wxWindow);
    }

    void Control::ApplyBounds(const Rect& value)
    {
        if (value.IsEmpty())
            return;

        auto wxWindow = GetWxWindow();
        wxRect rect(fromDip(value, wxWindow));
        wxWindow->SetSize(rect);
        wxWindow->Refresh();
    }

    Rect Control::GetBounds()
    {
        return _bounds.Get();
    }

    void Control::SetBounds(const Rect& value)
    {
        _bounds.Set(value);
    }

    void Control::SetBoundsEx(const Rect& value, int flags)
    {
        auto wxWindow = GetWxWindow();
        wxWindow->SetSize(fromDip(value, wxWindow), flags);
    }

    void Control::RefreshRect(const Rect& rect, bool eraseBackground)
    {
        auto wxWindow = GetWxWindow();
        wxWindow->RefreshRect(fromDip(rect, wxWindow), eraseBackground);
    }

    bool Control::GetVisible()
    {
        return _delayedFlags.Get(DelayedControlFlags::Visible);
    }

    void Control::SetVisible(bool value)
    {
        _delayedFlags.Set(DelayedControlFlags::Visible, value);
    }

    bool Control::GetEnabled()
    {
        return _delayedFlags.Get(DelayedControlFlags::Enabled);
    }

    void Control::SetEnabled(bool value)
    {
        _delayedFlags.Set(DelayedControlFlags::Enabled, value);
    }

    void Control::AddChild(Control* control)
    {
        verifyNonNull(control);

        _children.push_back(control);
        control->_parent = this;
        control->UpdateWxWindowParent();
    }

    void Control::RemoveChild(Control* control)
    {
        _children.erase(std::find(_children.begin(), _children.end(), control));
        control->_parent = nullptr;
        control->UpdateWxWindowParent();
    }

    Size Control::GetPreferredSize(const Size& availableSize)
    {
        auto wxWindow = GetWxWindow();
        return toDip(wxWindow->GetBestSize(), wxWindow);
    }

    /*static*/ Control* Control::TryFindControlByWxWindow(wxWindow* wxWindow)
    {
        wxWidgetExtender* extender = wxWidgetExtender::AsExtender(wxWindow);
        if (extender != nullptr)
            return extender->_palControl;

        ControlsByWxWindowsMap::const_iterator i = s_controlsByWxWindowsMap.find(wxWindow);
        return i == s_controlsByWxWindowsMap.end() ? NULL : i->second;
    }

    /*static*/ void Control::AssociateControlWithWxWindow(
        wxWindow* wxWindow, Control* control)
    {
        wxWidgetExtender* extender = wxWidgetExtender::AsExtender(wxWindow);
        if (extender == nullptr)
            s_controlsByWxWindowsMap[wxWindow] = control;
        else
            extender->_palControl = control;
    }

    /*static*/ void Control::RemoveWxWindowControlAssociation(wxWindow* wxWindow)
    {
        wxWidgetExtender* extender = wxWidgetExtender::AsExtender(wxWindow);
        if (extender == nullptr)
            s_controlsByWxWindowsMap.erase(wxWindow);
        else
            extender->_palControl = nullptr;
    }

    /*static*/ DragDropEffects Control::GetDragDropEffects(wxDragResult input)
    {
        switch (input)
        {
        case wxDragError:
        case wxDragNone:
        case wxDragCancel:
            return DragDropEffects::None;
        case wxDragCopy:
            return DragDropEffects::Copy;
        case wxDragMove:
            return DragDropEffects::Move;
        case wxDragLink:
            return DragDropEffects::Link;
        default:
            throwExNoInfo;
        }
    }

    wxDragResult Control::GetDragResult(DragDropEffects input)
    {
        switch (input)
        {
        case DragDropEffects::None:
            return wxDragResult::wxDragNone;
        case DragDropEffects::Copy:
            return wxDragResult::wxDragCopy;
        case DragDropEffects::Move:
            return wxDragResult::wxDragMove;
        case DragDropEffects::Link:
            return wxDragResult::wxDragLink;
        default:
            throwExNoInfo;
        }
    }

    wxDragResult Control::RaiseDragAndDropEvent(
        const wxPoint& location,
        wxDragResult defaultDragResult,
        wxDataObjectComposite* dataObjectComposite,
        ControlEvent event)
    {
        auto clientPoint = toDip(location, GetWxWindow());

        DragEventData data =
        {
            new UnmanagedDataObject(dataObjectComposite) /*data*/,
            clientPoint.X /*mouseClientLocationX*/,
            clientPoint.Y /*mouseClientLocationY*/,
            GetDragDropEffects(defaultDragResult) /*effect*/,
        };

        return GetDragResult((DragDropEffects)(size_t)RaiseEventWithPointerResult(event, &data));
    }


    wxDragResult Control::RaiseDragOver(const wxPoint& location, wxDragResult defaultDragResult, wxDataObjectComposite* dataObjectComposite)
    {
        return RaiseDragAndDropEvent(location, defaultDragResult, dataObjectComposite, ControlEvent::DragOver);
    }

    wxDragResult Control::RaiseDragEnter(const wxPoint& location, wxDragResult defaultDragResult, wxDataObjectComposite* dataObjectComposite)
    {
        return RaiseDragAndDropEvent(location, defaultDragResult, dataObjectComposite, ControlEvent::DragEnter);
    }

    wxDragResult Control::RaiseDragDrop(const wxPoint& location, wxDragResult defaultDragResult, wxDataObjectComposite* dataObjectComposite)
    {
        return RaiseDragAndDropEvent(location, defaultDragResult, dataObjectComposite, ControlEvent::DragDrop);
    }

    void Control::RaiseDragLeave()
    {
        RaiseEvent(ControlEvent::DragLeave);
    }

    Window* Control::GetParentWindow()
    {
        Control* parent = this;
        
        while (parent != nullptr)
        {
            auto window = dynamic_cast<Window*>(parent);
            if (window != nullptr)
                return window;
            parent = parent->GetParent();
        }
        
        return nullptr;
    }

    bool Control::GetTabStop()
    {
        return _flags.IsSet(ControlFlags::TabStop);
    }

    void Control::SetTabStop(bool value)
    {
        _flags.Set(ControlFlags::TabStop, value);
        RecreateWxWindowIfNeeded();
    }

    bool Control::GetIsFocused()
    {
        if (_wxWindow == nullptr)
            return false;

        return _wxWindow->HasFocus();
    }

    void Control::FocusNextControl(bool forward, bool nested)
    {
        if (_wxWindow == nullptr)
            return;
        
        int flags = forward ? wxNavigationKeyEvent::IsForward : wxNavigationKeyEvent::IsBackward;

        if (nested)
            _wxWindow->Navigate(flags);
        else
            _wxWindow->NavigateIn(flags);
    }

    wxOrientation Control::GetWxScrollOrientation(ScrollBarOrientation orientation)
    {
        switch (orientation)
        {
        case ScrollBarOrientation::Vertical:
        default:
            return wxOrientation::wxVERTICAL;
        case ScrollBarOrientation::Horizontal:
            return wxOrientation::wxHORIZONTAL;
        }
    }

    ScrollBarOrientation Control::GetScrollOrientation(wxOrientation orientation)
    {
        switch (orientation)
        {
        case wxHORIZONTAL:
            return ScrollBarOrientation::Horizontal;
        case wxVERTICAL:
        default:
            return ScrollBarOrientation::Vertical;
        }
    }

    DelayedValue<Control, Control::ScrollInfo>& Control::GetScrollInfoDelayedValue(ScrollBarOrientation orientation)
    {
        switch (orientation)
        {
        case ScrollBarOrientation::Vertical:
            return _verticalScrollBarInfo;
        case ScrollBarOrientation::Horizontal:
            return _horizontalScrollBarInfo;
        default:
            throwExNoInfo;
        }
    }

    Control::ScrollInfo Control::GetScrollInfo(ScrollBarOrientation orientation)
    {
        auto window = GetWxWindow();
        auto wxOrientation = GetWxScrollOrientation(orientation);
        
        ScrollInfo info;
        info.value = window->GetScrollPos(wxOrientation);
        info.largeChange = window->GetScrollThumb(wxOrientation);
        info.maximum = window->GetScrollRange(wxOrientation);
        info.visible = info.maximum > 0;

        return info;
    }

    void Control::SetScrollInfo(ScrollBarOrientation orientation, const ScrollInfo& value)
    {
        auto window = GetWxWindow();
        auto wxOrientation = GetWxScrollOrientation(orientation);

        if (!value.visible)
        {
            window->SetScrollbar(wxOrientation, 0, 0, 0);
            return;
        }

        window->SetScrollbar(wxOrientation, value.value, value.largeChange, value.maximum);
    }

    Control::ScrollInfo Control::RetrieveVerticalScrollBarInfo()
    {
        return GetScrollInfo(ScrollBarOrientation::Vertical);
    }

    void Control::ApplyVerticalScrollBarInfo(const ScrollInfo& value)
    {
        SetScrollInfo(ScrollBarOrientation::Vertical, value);
    }

    Control::ScrollInfo Control::RetrieveHorizontalScrollBarInfo()
    {
        return GetScrollInfo(ScrollBarOrientation::Horizontal);
    }

    void Control::ApplyHorizontalScrollBarInfo(const ScrollInfo& value)
    {
        SetScrollInfo(ScrollBarOrientation::Horizontal, value);
    }

    void Control::SetScrollBar(ScrollBarOrientation orientation, bool visible, int value, int largeChange, int maximum)
    {
        auto& delayedValue = GetScrollInfoDelayedValue(orientation);
        
        ScrollInfo info;
        info.value = value;
        info.largeChange = largeChange;
        info.maximum = maximum;
        info.visible = visible;

        delayedValue.Set(info);
    }

    bool Control::IsScrollBarVisible(ScrollBarOrientation orientation)
    {
        return GetScrollInfoDelayedValue(orientation).Get().visible;
    }

    int Control::GetScrollBarValue(ScrollBarOrientation orientation)
    {
        return GetScrollInfoDelayedValue(orientation).Get().value;
    }

    int Control::GetScrollBarLargeChange(ScrollBarOrientation orientation)
    {
        return GetScrollInfoDelayedValue(orientation).Get().largeChange;
    }

    int Control::GetScrollBarMaximum(ScrollBarOrientation orientation)
    {
        return GetScrollInfoDelayedValue(orientation).Get().maximum;
    }

    bool Control::GetUserPaint()
    {
        return _flags.IsSet(ControlFlags::UserPaint);
    }

    void Control::SetUserPaint(bool value)
    {
        _flags.Set(ControlFlags::UserPaint, value);
        RecreateWxWindowIfNeeded();
    }

    wxWindow* wxFindWindowAtPoint(wxWindow* win, const wxPoint& pt)
    {
        if (!win->IsShown())
            return NULL;

        wxWindowList::compatibility_iterator node = win->GetChildren().GetLast();
        while (node)
        {
            wxWindow* child = node->GetData();
            wxWindow* foundWin = wxFindWindowAtPoint(child, pt);
            if (foundWin)
            return foundWin;
            node = node->GetPrevious();
        }

        wxPoint pos = win->GetPosition();
        wxSize sz = win->GetSize();
        if ( !win->IsTopLevel() && win->GetParent() )
        {
            pos = win->GetParent()->ClientToScreen(pos);
        }

        wxRect rect(pos, sz);
        if (rect.Contains(pt))
            return win;

        return NULL;
    }

    /*static*/ Control* Control::HitTest(const Point& screenPoint)
    {
#if __WXOSX__
        /*
        for (auto popupWindow : Popup::GetVisiblePopupWindows())
        {
            auto window = wxFindWindowAtPoint(popupWindow, fromDip(screenPoint, nullptr));
            if (window == nullptr)
                continue;
        
            auto control = TryFindControlByWxWindow(window);
            if (control != nullptr)
            {
                control->AddRef();
                return control;
            }
        }
        */
#endif

        auto window = wxFindWindowAtPoint(fromDip(screenPoint, nullptr));
        if (window == nullptr)
            return nullptr;
    
        auto control = TryFindControlByWxWindow(window);
        if (control != nullptr)
            control->AddRef();

        return control;
    }

    Point Control::ClientToScreen(const Point& point)
    {
        auto window = GetWxWindow();
        auto screenPixelPoint = window->ClientToScreen(fromDip(point, window));
        return toDip(screenPixelPoint, window);
    }

    Point Control::ScreenToClient(const Point& point)
    {
        auto window = GetWxWindow();
        auto screenClientPoint = window->ScreenToClient(fromDip(point, window));
        return toDip(screenClientPoint, window);
    }

    Int32Point Control::ScreenToDevice(const Point& point)
    {
        auto p = fromDip(point, GetWxWindow());
        return Int32Point(p.x, p.y);
    }

    Point Control::DeviceToScreen(const Int32Point& point)
    {
        return toDip(wxPoint(point.X, point.Y), GetWxWindow());
    }

    void* Control::GetContainingSizer() 
    {
        return GetWxWindow()->GetContainingSizer();
    }

    void* Control::GetSizer()
    {
        return GetWxWindow()->GetSizer();
    }

    void Control::SetSizer(void* sizer, bool deleteOld)
    {
        GetWxWindow()->SetSizer((wxSizer*)sizer, deleteOld);
    }

    void Control::SetSizerAndFit(void* sizer, bool deleteOld)
    {
        GetWxWindow()->SetSizerAndFit((wxSizer*)sizer, deleteOld);
    }

    void Control::ResetBackgroundColor()
    {
        auto window = GetWxWindow();
        auto attr = window->GetDefaultAttributes();
        auto& color = attr.colBg;
        if(color.IsOk())
            window->SetBackgroundColour(color);
        else
            window->SetBackgroundColour(wxNullColour);
    } 

    void Control::ResetForegroundColor()
    {
        auto window = GetWxWindow();
        auto attr = window->GetDefaultAttributes();
        auto& color = attr.colFg;
        if (color.IsOk())
            window->SetForegroundColour(color);
        else
            window->SetForegroundColour(wxNullColour);
    }

    bool Control::SetFocus()
    {
        auto window = GetWxWindow();
        if (!window->AcceptsFocus() || !window->CanAcceptFocus() || !window->CanBeFocused())
            return false;

        window->SetFocus();
        return window->HasFocus();
    }

    void Control::SetCursor(void* handle)
    {
        if(handle == nullptr)
            GetWxWindow()->SetCursor(wxNullCursor);
        else
        {
            auto cursor = (wxCursor*)handle;
            GetWxWindow()->SetCursor(wxCursor(*cursor));
        }
    }

    /*static*/ Control* Control::GetFocusedControl()
    {
        auto focusedWxWindow = wxWindow::FindFocus();
        if (focusedWxWindow == nullptr)
            return nullptr;

        auto control = TryFindControlByWxWindow(focusedWxWindow);
        if (control != nullptr)
            control->AddRef();

        return control;
    }

    bool Control::IsTransparentBackgroundSupported()
    {
        return GetWxWindow()->IsTransparentBackgroundSupported();
    }

    bool Control::SetBackgroundStyle(int style)
    {
        return GetWxWindow()->SetBackgroundStyle((wxBackgroundStyle)style);
    }

    int Control::GetBackgroundStyle()
    {
        return GetWxWindow()->GetBackgroundStyle();
    }

    void Control::AlwaysShowScrollbars(bool hflag, bool vflag)
    {
        return GetWxWindow()->AlwaysShowScrollbars(hflag, vflag);
    }

    Color Control::GetDefaultAttributesBgColor()
    {
        return GetWxWindow()->GetDefaultAttributes().colBg;
    }

    Color Control::GetDefaultAttributesFgColor()
    {
        return GetWxWindow()->GetDefaultAttributes().colFg;
    }

    Font* Control::GetDefaultAttributesFont()
    {
        return new Font(GetWxWindow()->GetDefaultAttributes().font);
    }

    enum ControlId
    {
        ControlId_Control = 0,
        ControlId_RichTextBox = 1,
        ControlId_AnimationPlayer = 2,
        ControlId_Calendar = 3,
        ControlId_Button = 4,
        ControlId_Border = 5,
        ControlId_ContextMenu = 6,
        ControlId_MainMenu = 7,
        ControlId_MenuItem = 8,
        ControlId_CheckBox = 9,
        ControlId_RadioButton = 10,
        ControlId_PropertyGrid = 11,
        ControlId_ColorPicker = 12,
        ControlId_DateTimePicker = 13,
        ControlId_Grid = 14,
        ControlId_Panel = 15,
        ControlId_GroupBox = 16,
        ControlId_Label = 17,
        ControlId_LinkLabel = 18,
        ControlId_LayoutPanel = 19,
        ControlId_ComboBox = 20,
        ControlId_ListBox = 21,
        ControlId_CheckListBox = 22,
        ControlId_ListView = 23,
        ControlId_Menu = 24,
        ControlId_NumericUpDown = 25,
        ControlId_PictureBox = 26,
        ControlId_Popup = 27,
        ControlId_ProgressBar = 28,
        ControlId_ScrollViewer = 29,
        ControlId_Slider = 30,
        ControlId_SplitterPanel = 31,
        ControlId_StackPanel = 32,
        ControlId_StatusBar = 33,
        ControlId_StatusBarPanel = 34,
        ControlId_TabControl = 35,
        ControlId_TabPage = 36,
        ControlId_TextBox = 37,
        ControlId_Toolbar = 38,
        ControlId_AuiToolbar = 39,
        ControlId_AuiNotebook = 40,
        ControlId_ToolbarItem = 41,
        ControlId_TreeView = 42,
        ControlId_UserPaintControl = 43,
        ControlId_WebBrowser = 44,
        ControlId_Other = 45,
        ControlId_Window = 46,
        ControlId_MultilineTextBox = 47,
    };


    static wxVisualAttributes NullVisualAttributes;

    static wxVisualAttributes GetClassDefaultAttributes(int controlType, int windowVariant)
    {
        switch (controlType)
        {
        case ControlId_ListBox:
            return wxListCtrl::GetClassDefaultAttributes((wxWindowVariant)windowVariant);
        case ControlId_TextBox:
            return wxTextCtrl::GetClassDefaultAttributes((wxWindowVariant)windowVariant);
        default:
            return NullVisualAttributes;
        }
    }

    Color Control::GetClassDefaultAttributesBgColor(int controlType, int windowVariant)
    {
        return GetClassDefaultAttributes(controlType, windowVariant).colBg;
    }

    Color Control::GetClassDefaultAttributesFgColor(int controlType, int windowVariant)
    {
        return GetClassDefaultAttributes(controlType, windowVariant).colFg;
    }

    Font* Control::GetClassDefaultAttributesFont(int controlType, int windowVariant)
    {
        return new Font(GetClassDefaultAttributes(controlType, windowVariant).font);
    }
}