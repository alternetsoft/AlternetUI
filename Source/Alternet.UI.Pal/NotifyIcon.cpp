#include "NotifyIcon.h"

namespace Alternet::UI
{
    NotifyIcon::TaskBarIcon::TaskBarIcon(NotifyIcon* owner) : _owner(owner)
    {
    }

    wxMenu* NotifyIcon::TaskBarIcon::GetPopupMenu()
    {
        return nullptr;
    }

    wxMenu* NotifyIcon::TaskBarIcon::CreatePopupMenu()
    {
        if(_menu != nullptr)
		PopupMenu(_menu);
        return nullptr;
#ifdef __WXOSX__
#else
#endif
    }

    //===========================================================

    NotifyIcon::NotifyIcon()
    {
    }

    NotifyIcon::~NotifyIcon()
    {
        DeleteTaskBarIcon();
    }

    void NotifyIcon::OnLeftMouseButtonDown(wxTaskBarIconEvent& event)
    {
        event.Skip();
        RaiseEvent(NotifyIconEvent::LeftMouseButtonDown);
	}

    void NotifyIcon::OnRightMouseButtonDown(wxTaskBarIconEvent& event)
    {
        event.Skip();
        RaiseEvent(NotifyIconEvent::RightMouseButtonDown);
    }
    
    void NotifyIcon::OnRightMouseButtonUp(wxTaskBarIconEvent& event)
    {
        event.Skip();
        RaiseEvent(NotifyIconEvent::RightMouseButtonUp);
    }

    void NotifyIcon::OnRightMouseButtonDoubleClick(wxTaskBarIconEvent& event)
    {
        event.Skip();
        RaiseEvent(NotifyIconEvent::RightMouseButtonDoubleClick);
    }

    void NotifyIcon::OnClick(wxTaskBarIconEvent& event)
    {
        event.Skip();
        RaiseEvent(NotifyIconEvent::Click);
    }

    void NotifyIcon::OnLeftMouseButtonUp(wxTaskBarIconEvent& event)
    {
        event.Skip();
        RaiseEvent(NotifyIconEvent::LeftMouseButtonUp);
    }

    void NotifyIcon::OnLeftMouseButtonDoubleClick(wxTaskBarIconEvent& event)
    {
        event.Skip();
        RaiseEvent(NotifyIconEvent::LeftMouseButtonDoubleClick);
    }

    void NotifyIcon::ApplyTextAndIcon()
    {
        if (_taskBarIcon == nullptr)
            throwExNoInfo;

        auto text = wxStr(_text.value_or(u""));

        if (_icon != nullptr)
        {
            wxBitmapBundle bundle(_icon->GetBitmap());
            _taskBarIcon->SetIcon(bundle, text);
        }
        else
        {
            wxBitmapBundle bundle;
            _taskBarIcon->SetIcon(bundle, text);
        }
    }

    void NotifyIcon::CreateTaskBarIcon()
    {
        if (_taskBarIcon != nullptr)
            throwExNoInfo;

        _taskBarIcon = new TaskBarIcon(this);

        _taskBarIcon->Bind(wxEVT_TASKBAR_LEFT_UP, &NotifyIcon::OnLeftMouseButtonUp, this);
        _taskBarIcon->Bind(wxEVT_TASKBAR_LEFT_DCLICK, &NotifyIcon::OnLeftMouseButtonDoubleClick, this);

        _taskBarIcon->Bind(wxEVT_TASKBAR_LEFT_DOWN, &NotifyIcon::OnLeftMouseButtonDown, this);
        _taskBarIcon->Bind(wxEVT_TASKBAR_RIGHT_DOWN, &NotifyIcon::OnRightMouseButtonDown, this);
        _taskBarIcon->Bind(wxEVT_TASKBAR_RIGHT_UP, &NotifyIcon::OnRightMouseButtonUp, this);
        _taskBarIcon->Bind(wxEVT_TASKBAR_RIGHT_DCLICK, &NotifyIcon::OnRightMouseButtonDoubleClick, this);
        _taskBarIcon->Bind(wxEVT_TASKBAR_CLICK, &NotifyIcon::OnClick, this);

        ApplyTextAndIcon();

        RaiseEvent(NotifyIconEvent::Created);
    }

    void NotifyIcon::DeleteTaskBarIcon()
    {
        if (_taskBarIcon != nullptr)
        {
            _taskBarIcon->Unbind(wxEVT_TASKBAR_LEFT_UP, &NotifyIcon::OnLeftMouseButtonUp, this);

            _taskBarIcon->Unbind(wxEVT_TASKBAR_LEFT_DCLICK,
                &NotifyIcon::OnLeftMouseButtonDoubleClick, this);

            _taskBarIcon->Unbind(wxEVT_TASKBAR_LEFT_DOWN, &NotifyIcon::OnLeftMouseButtonDown, this);
            _taskBarIcon->Unbind(wxEVT_TASKBAR_RIGHT_DOWN, &NotifyIcon::OnRightMouseButtonDown, this);
            _taskBarIcon->Unbind(wxEVT_TASKBAR_RIGHT_UP, &NotifyIcon::OnRightMouseButtonUp, this);
            
            _taskBarIcon->Unbind(wxEVT_TASKBAR_RIGHT_DCLICK,
                &NotifyIcon::OnRightMouseButtonDoubleClick, this);

            _taskBarIcon->Unbind(wxEVT_TASKBAR_CLICK, &NotifyIcon::OnClick, this);

            delete _taskBarIcon;
            _taskBarIcon = nullptr;
        }
    }

    void NotifyIcon::RecreateTaskBarIconIfNeeded()
    {
        DeleteTaskBarIcon();

        if (_visible)
            CreateTaskBarIcon();
    }

    optional<string> NotifyIcon::GetText()
    {
        return _text;
    }

    bool NotifyIcon::GetIsAvailable()
    {
        return wxTaskBarIcon::IsAvailable();
    }

    bool NotifyIcon::GetIsIconInstalled()
    {
        if (_taskBarIcon == nullptr)
            RecreateTaskBarIconIfNeeded();
        return _taskBarIcon->IsIconInstalled();
    }

    bool NotifyIcon::GetIsOk()
    {
        if (_taskBarIcon == nullptr)
            RecreateTaskBarIconIfNeeded();
        return _taskBarIcon->IsOk();
    }

    void NotifyIcon::SetText(optional<string> value)
    {
        _text = value;

        if (_taskBarIcon == nullptr)
            RecreateTaskBarIconIfNeeded();
        else
            ApplyTextAndIcon();
    }

    Image* NotifyIcon::GetIcon()
    {
        _icon->AddRef();
        return _icon;
    }

    void NotifyIcon::SetIcon(Image* value)
    {
        if (_icon != nullptr)
            _icon->Release();

        _icon = value;

        if (_icon != nullptr)
            _icon->AddRef();

        if (_taskBarIcon == nullptr)
            RecreateTaskBarIconIfNeeded();
        else
            ApplyTextAndIcon();
    }

    void NotifyIcon::SetPopupMenu(void* menuHandle)
    {
        wxObject* obj = static_cast<wxObject*>(menuHandle);
        wxMenu* menu = wxDynamicCast(obj, wxMenu);
        if (_taskBarIcon == nullptr)
            RecreateTaskBarIconIfNeeded();
        else
            _taskBarIcon->_menu = menu;
    }

    void NotifyIcon::ShowPopup(void* menuHandle)
    {
        wxObject* obj = static_cast<wxObject*>(menuHandle);
        wxMenu* menu = wxDynamicCast(obj, wxMenu);

        if (_taskBarIcon == nullptr)
            RecreateTaskBarIconIfNeeded();
		_taskBarIcon->PopupMenu(menu);
    }

    bool NotifyIcon::GetVisible()
    {
        return _visible;
    }

    void NotifyIcon::SetVisible(bool value)
    {
        _visible = value;
        RecreateTaskBarIconIfNeeded();
    }
}
