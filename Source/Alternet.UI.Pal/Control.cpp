#include "Control.h"

namespace Alternet::UI
{
    Control::Control() :
        _delayedFlags(
            *this,
            DelayedControlFlags::Visible,
            &Control::IsWxWindowCreated,
            {
                {DelayedControlFlags::Visible, std::make_tuple(&Control::RetrieveVisible, &Control::ApplyVisible)},
                {DelayedControlFlags::Frozen, std::make_tuple(&Control::RetrieveFrozen, &Control::ApplyFrozen)},
                {DelayedControlFlags::Enabled, std::make_tuple(&Control::RetrieveEnabled, &Control::ApplyEnabled)},
            }),
            _bounds(*this, Rect(), &Control::IsWxWindowCreated, &Control::RetrieveBounds, &Control::ApplyBounds),
            _backgroundColor(*this, Color(), &Control::IsWxWindowCreated, &Control::RetrieveBackgroundColor, &Control::ApplyBackgroundColor),
            _foregroundColor(*this, Color(), &Control::IsWxWindowCreated, &Control::RetrieveForegroundColor, &Control::ApplyForegroundColor),
            _delayedValues({&_delayedFlags, &_bounds, &_backgroundColor, &_foregroundColor})
    {
    }

    Control::~Control()
    {
        DestroyWxWindow(/*finalDestroy:*/ true);
    }

    void Control::DestroyWxWindow(bool finalDestroy/* = false*/)
    {
        if (_wxWindow != nullptr)
        {
            if (!finalDestroy)
                _delayedValues.ReceiveIfPossible();

            OnWxWindowDestroying();

            _wxWindow->Unbind(wxEVT_PAINT, &Control::OnPaint, this);
            //_wxWindow->Unbind(wxEVT_ERASE_BACKGROUND, &Control::OnEraseBackground, this);
            _wxWindow->Unbind(wxEVT_SHOW, &Control::OnVisibleChanged, this);
            _wxWindow->Unbind(wxEVT_MOTION, &Control::OnMouseMove, this);
            _wxWindow->Unbind(wxEVT_MOUSE_CAPTURE_LOST, &Control::OnMouseCaptureLost, this);
            _wxWindow->Unbind(wxEVT_ENTER_WINDOW, &Control::OnMouseEnter, this);
            _wxWindow->Unbind(wxEVT_LEAVE_WINDOW, &Control::OnMouseLeave, this);
            _wxWindow->Unbind(wxEVT_LEFT_DOWN, &Control::OnMouseLeftButtonDown, this);
            _wxWindow->Unbind(wxEVT_LEFT_UP, &Control::OnMouseLeftButtonUp, this);
            _wxWindow->Unbind(wxEVT_SIZE, &Control::OnSizeChanged, this);

            if (!GetDoNotDestroyWxWindow())
                _wxWindow->Destroy();
            _wxWindow = nullptr;

            OnWxWindowDestroyed();
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

    Control* Control::GetParent()
    {
        return _parent;
    }

    wxWindow* Control::GetWxWindow()
    {
        if (_wxWindow == nullptr)
            CreateWxWindow();

        wxASSERT(_wxWindow);
        return _wxWindow;
    }

    void Control::Update()
    {
        if (!IsWxWindowCreated())
            return;
        
        auto wxWindow = GetWxWindow();
        wxWindow->Refresh();
        wxWindow->Update();
    }

    void Control::CreateWxWindow()
    {
        SetFlag(ControlFlags::CreatingWxWindow, true);
        wxWindow* parentingWxWindow = nullptr;
        if (_parent != nullptr)
            parentingWxWindow = _parent->GetParentingWxWindow();
        else
            parentingWxWindow = ParkingWindow::GetWindow();
        
        _wxWindow = CreateWxWindowCore(parentingWxWindow);
        //_wxWindow->SetDoubleBuffered(true); // todo: this removes flicker on TextBoxes, but causes it on custom composite controls
        //_wxWindow->SetBackgroundStyle(wxBG_STYLE_PAINT);

        _wxWindow->Bind(wxEVT_PAINT, &Control::OnPaint, this);
        //_wxWindow->Bind(wxEVT_ERASE_BACKGROUND, &Control::OnEraseBackground, this);
        _wxWindow->Bind(wxEVT_SHOW, &Control::OnVisibleChanged, this);
        _wxWindow->Bind(wxEVT_MOTION, &Control::OnMouseMove, this);
        _wxWindow->Bind(wxEVT_MOUSE_CAPTURE_LOST, &Control::OnMouseCaptureLost, this);
        _wxWindow->Bind(wxEVT_ENTER_WINDOW, &Control::OnMouseEnter, this);
        _wxWindow->Bind(wxEVT_LEAVE_WINDOW, &Control::OnMouseLeave, this);
        _wxWindow->Bind(wxEVT_LEFT_DOWN, &Control::OnMouseLeftButtonDown, this);
        _wxWindow->Bind(wxEVT_LEFT_UP, &Control::OnMouseLeftButtonUp, this);
        _wxWindow->Bind(wxEVT_SIZE, &Control::OnSizeChanged, this);

        OnWxWindowCreated();
        _delayedValues.ApplyIfPossible();

        SetFlag(ControlFlags::CreatingWxWindow, false);

        for (auto child : _children)
            child->UpdateWxWindowParent();
    }

    void Control::RecreateWxWindowIfNeeded()
    {
        if (!IsWxWindowCreated())
            return;

        DestroyWxWindow();
        CreateWxWindow();
    }

    void Control::OnWxWindowCreated()
    {
    }

    void Control::OnWxWindowDestroying()
    {
    }

    void Control::OnWxWindowDestroyed()
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
        _backgroundColor.Set(value);
    }

    Color Control::GetBackgroundColor()
    {
        return _backgroundColor.Get();
    }

    void Control::SetForegroundColor(const Color& value)
    {
        _foregroundColor.Set(value);
    }

    Color Control::GetForegroundColor()
    {
        return _foregroundColor.Get();
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

    wxWindow* Control::GetParentingWxWindow()
    {
        return GetWxWindow();
    }

    bool Control::RetrieveVisible()
    {
        return GetWxWindow()->IsShown();
    }

    void Control::ApplyVisible(bool value)
    {
        auto wxWindow = GetWxWindow();
        if (value)
            wxWindow->Show();
        else
            wxWindow->Hide();
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

    Color Control::RetrieveBackgroundColor()
    {
#ifndef __WXMSW__
        // This is a workaround for non-Windows systems returning wrong background color when recreating a wxWindow.
        // For example, see ListBox on SelectionMode change.
        // Later this wrong color is reapplied to the recreated control. See also RetrieveForegroundColor().
        return _backgroundColor.GetDelayed();
#else
        return GetWxWindow()->GetBackgroundColour();
#endif
    }

    void Control::ApplyBackgroundColor(const Color& value)
    {
        if (!value.IsEmpty())
            GetWxWindow()->SetBackgroundColour(value);
    }

    Color Control::RetrieveForegroundColor()
    {
#ifndef __WXMSW__
        return _foregroundColor.GetDelayed();
#else
        return GetWxWindow()->GetForegroundColour();
#endif
    }

    void Control::ApplyForegroundColor(const Color& value)
    {
        if (!value.IsEmpty())
            GetWxWindow()->SetForegroundColour(value);
    }

    std::vector<Control*> Control::GetChildren()
    {
        return _children;
    }

    bool Control::GetDoNotDestroyWxWindow()
    {
        return GetFlag(ControlFlags::DoNotDestroyWxWindow);
    }

    void Control::SetDoNotDestroyWxWindow(bool value)
    {
        SetFlag(ControlFlags::DoNotDestroyWxWindow, value);
    }

    Font* Control::GetFont()
    {
        return nullptr;
    }

    void Control::SetFont(Font* value)
    {
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
        wxASSERT_MSG(_beginUpdateCount > 0, "EndUpdate() without matching BeginUpdate()");
        _beginUpdateCount--;
        if (_beginUpdateCount == 0)
        {
            _delayedFlags.Set(DelayedControlFlags::Frozen, false);
        }
    }

    Size Control::GetClientSize()
    {
        if (!GetFlag(ControlFlags::ClientSizeCacheValid))
        {
            _clientSizeCache = GetClientSizeCore();
            SetFlag(ControlFlags::ClientSizeCacheValid, true);
        }

        return _clientSizeCache;
    }

    Size Control::GetClientSizeCore()
    {
        return SizeToClientSize(GetBounds().GetSize());
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
        return toDip(GetWxWindow()->WindowToClientSize(fromDip(size, window)), window);
    }

    Thickness Control::GetIntrinsicLayoutPadding()
    {
        return Thickness();
    }

    Thickness Control::GetIntrinsicPreferredSizePadding()
    {
        return Thickness();
    }

    bool Control::GetFlag(ControlFlags flag)
    {
        return (_flags & flag) != ControlFlags::None;
    }

    void Control::SetFlag(ControlFlags flag, bool value)
    {
        if (value)
            _flags |= flag;
        else
            _flags &= ~flag;
    }

    Rect Control::RetrieveBounds()
    {
        auto wxWindow = GetWxWindow();
        return toDip(wxWindow->GetRect(), wxWindow);
    }

    void Control::ApplyBounds(const Rect& value)
    {
        auto wxWindow = GetWxWindow();
        wxRect rect(fromDip(value, wxWindow));
        wxWindow->SetPosition(rect.GetPosition());
        wxWindow->SetSize(rect.GetSize());
        wxWindow->Refresh();
    }

    bool Control::EventsSuspended()
    {
        return GetFlag(ControlFlags::CreatingWxWindow);
    }

    void Control::OnPaint(wxPaintEvent& event)
    {
        event.Skip();
        RaiseEvent(ControlEvent::Paint);
    }

    void Control::OnEraseBackground(wxEraseEvent& event)
    {
    }

    void Control::OnMouseCaptureLost(wxEvent& event)
    {
    }

    void Control::OnMouseMove(wxMouseEvent& event)
    {
        event.Skip();
        RaiseEvent(ControlEvent::MouseMove);
    }

    void Control::OnMouseEnter(wxMouseEvent& event)
    {
        RaiseEvent(ControlEvent::MouseEnter);

        auto window = GetParent();
        while (window != nullptr)
        {
            if (window->GetIsMouseOver())
                window->RaiseEvent(ControlEvent::MouseEnter);

            window = window->GetParent();
        }
    }

    void Control::OnMouseLeave(wxMouseEvent& event)
    {
        RaiseEvent(ControlEvent::MouseLeave);

        auto window = GetParent();
        while (window != nullptr)
        {
            if(!window->GetIsMouseOver())
                window->RaiseEvent(ControlEvent::MouseLeave);

            window = window->GetParent();
        }
    }

    void Control::OnMouseLeftButtonDown(wxMouseEvent& event)
    {
        event.Skip();
        RaiseEvent(ControlEvent::MouseLeftButtonDown);
    }

    void Control::OnMouseLeftButtonUp(wxMouseEvent& event)
    {
        event.Skip();
        RaiseEvent(ControlEvent::MouseLeftButtonUp);
        GetWxWindow()->CallAfter([=]() {RaiseEvent(ControlEvent::MouseClick); });
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

        SetFlag(ControlFlags::ClientSizeCacheValid, false);
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
            auto parentWxWindow = parent->GetParentingWxWindow();
            auto oldParent = wxWindow->GetParent();
            if (oldParent != parentWxWindow)
                SetWxWindowParent(parentWxWindow);
        }
    }

    void Control::SetWxWindowParent(wxWindow* parent)
    {
        GetWxWindow()->Reparent(parent);
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

    Rect Control::GetBounds()
    {
        return _bounds.Get();
    }

    void Control::SetBounds(const Rect& value)
    {
        _bounds.Set(value);
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
}