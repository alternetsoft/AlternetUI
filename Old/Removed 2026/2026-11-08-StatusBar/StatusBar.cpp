#include "StatusBar.h"
#include "Window.h"

namespace Alternet::UI
{
    void* StatusBar::GetRealHandle()
    {
        return GetWxStatusBar();
    }

    StatusBar::StatusBar()
    {
    }

    StatusBar::~StatusBar()
    {
        DestroyWxStatusBar();
    }

    wxStatusBar* StatusBar::GetWxStatusBar()
    {
        return _wxStatusBar;
    }

    void StatusBar::SetOwnerWindow(Window* window)
    {
        _ownerWindow = window;
        RecreateWxStatusBar();
    }

    void StatusBar::CreateWxStatusBar()
    {
        if (_wxStatusBar != nullptr)
            throwExNoInfo;

        if (_ownerWindow == nullptr)
            return;

        auto getStyle = [&]
        {
            auto style = (wxSTB_ELLIPSIZE_END | wxSTB_SHOW_TIPS | wxFULL_REPAINT_ON_RESIZE);

            if (_sizingGripperVisible)
                style |= wxSTB_SIZEGRIP;

            return style;
        };

        _wxStatusBar = _ownerWindow->GetFrame()->CreateStatusBar(1, getStyle());
        RaiseEvent(StatusBarEvent::ControlRecreated);
        _ownerWindow->GetFrame()->SetStatusBar(_wxStatusBar);
        _ownerWindow->GetFrame()->Layout();
        _ownerWindow->GetFrame()->PostSizeEvent();
    }

    void StatusBar::DestroyWxStatusBar()
    {
        if (_wxStatusBar != nullptr)
        {
            delete _wxStatusBar;
            _wxStatusBar = nullptr;
        }
    }

    void StatusBar::RecreateWxStatusBar()
    {
        DestroyWxStatusBar();
        CreateWxStatusBar();
    }

    bool StatusBar::GetSizingGripVisible()
    {
        return _sizingGripperVisible;
    }

    void StatusBar::SetSizingGripVisible(bool value)
    {
        if (_sizingGripperVisible == value)
            return;

        _sizingGripperVisible = value;
        
        if (_ownerWindow != nullptr)
            RecreateWxStatusBar();
    }
}
