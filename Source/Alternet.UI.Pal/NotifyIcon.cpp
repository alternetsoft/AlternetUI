#include "NotifyIcon.h"

namespace Alternet::UI
{
    NotifyIcon::TaskBarIcon::TaskBarIcon(NotifyIcon* owner) : _owner(owner)
    {
    }

    wxMenu* NotifyIcon::TaskBarIcon::GetPopupMenu()
    {
        if (_owner->_menu == nullptr)
            return nullptr;

        return _owner->_menu->GetWxMenu();
    }

    wxMenu* NotifyIcon::TaskBarIcon::CreatePopupMenu()
    {
#if __WXOSX_COCOA__
        // macOS implementation does not call GetPopupMenu, so here is a workaround.
        if (_owner->_menu != nullptr)
            PopupMenu(_owner->_menu->GetWxMenu());

        return nullptr;
#else
        return wxTaskBarIcon::CreatePopupMenu();
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

    void NotifyIcon::DestroyAllNotifyIcons()
    {
        for (auto icon : _taskBarIcons)
            icon->Destroy();

        _taskBarIcons.clear();
    }

    void NotifyIcon::OnLeftMouseButtonUp(wxTaskBarIconEvent& event)
    {
        RaiseEvent(NotifyIconEvent::Click);
    }

    void NotifyIcon::OnLeftMouseButtonDoubleClick(wxTaskBarIconEvent& event)
    {
        RaiseEvent(NotifyIconEvent::DoubleClick);
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

        _taskBarIcons.push_back(_taskBarIcon);

        ApplyTextAndIcon();
    }

    void NotifyIcon::DeleteTaskBarIcon()
    {
        if (_taskBarIcon != nullptr)
        {
            _taskBarIcons.erase(std::find(_taskBarIcons.begin(), _taskBarIcons.end(), _taskBarIcon));
            
            _taskBarIcon->Unbind(wxEVT_TASKBAR_LEFT_UP, &NotifyIcon::OnLeftMouseButtonUp, this);
            _taskBarIcon->Unbind(wxEVT_TASKBAR_LEFT_DCLICK, &NotifyIcon::OnLeftMouseButtonDoubleClick, this);
            
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

    Menu* NotifyIcon::GetMenu()
    {
        _menu->AddRef();
        return _menu;
    }

    void NotifyIcon::SetMenu(Menu* value)
    {
        if (_menu != nullptr)
            _menu->Release();

        _menu = value;

        if (_menu != nullptr)
            _menu->AddRef();
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
