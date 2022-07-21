#include "Application.h"
#include "Common.h"
#include "Window.h"
#include "Image.h"

IMPLEMENT_APP_NO_MAIN(Alternet::UI::App);
IMPLEMENT_WX_THEME_SUPPORT;

namespace Alternet::UI
{
    App::App()
    {
    }

    bool App::OnInit()
    {
        return wxApp::OnInit();
    }

    int App::OnRun()
    {
        return 0;
    }

    int App::OnExit()
    {
        return wxApp::OnExit();
    }

    void App::Run()
    {
        wxApp::OnRun();
    }

    int App::FilterEvent(wxEvent& e)
    {
        if (_owner == nullptr)
            return Event_Skip;

        bool handled = false;

        auto eventType = e.GetEventType();

        if (eventType == wxEVT_KEY_UP)
            _owner->GetKeyboardInternal()->OnKeyUp((wxKeyEvent&)e, handled);
        else if (eventType == wxEVT_KEY_DOWN)
        {
#ifndef __WXOSX_COCOA__            
            // For some reason, on Windows and Linux wxEVT_CHAR_HOOK are not sent
            // when mouse capture is active.
            if (wxWindow::GetCapture() != nullptr)
                _owner->GetKeyboardInternal()->OnKeyDown((wxKeyEvent&)e, handled);
#endif                
        }
        else if (eventType == wxEVT_CHAR_HOOK)
            _owner->GetKeyboardInternal()->OnKeyDown((wxKeyEvent&)e, handled);
        else if (eventType == wxEVT_CHAR)
            _owner->GetKeyboardInternal()->OnChar((wxKeyEvent&)e, handled);
        else if (eventType == wxEVT_MOTION)
            _owner->GetMouseInternal()->OnMouseMove((wxMouseEvent&)e, handled);
        else if (eventType == wxEVT_MOUSEWHEEL)
            _owner->GetMouseInternal()->OnMouseWheel((wxMouseEvent&)e, handled);
        else if (eventType == wxEVT_LEFT_DOWN)
            _owner->GetMouseInternal()->OnMouseDown((wxMouseEvent&)e, MouseButton::Left, handled);
        else if (eventType == wxEVT_MIDDLE_DOWN)
            _owner->GetMouseInternal()->OnMouseDown((wxMouseEvent&)e, MouseButton::Middle, handled);
        else if (eventType == wxEVT_RIGHT_DOWN)
            _owner->GetMouseInternal()->OnMouseDown((wxMouseEvent&)e, MouseButton::Right, handled);
        else if (eventType == wxEVT_AUX1_DOWN)
            _owner->GetMouseInternal()->OnMouseDown((wxMouseEvent&)e, MouseButton::XButton1, handled);
        else if (eventType == wxEVT_AUX2_DOWN)
            _owner->GetMouseInternal()->OnMouseDown((wxMouseEvent&)e, MouseButton::XButton2, handled);
        else if (eventType == wxEVT_LEFT_UP)
            _owner->GetMouseInternal()->OnMouseUp((wxMouseEvent&)e, MouseButton::Left, handled);
        else if (eventType == wxEVT_MIDDLE_UP)
            _owner->GetMouseInternal()->OnMouseUp((wxMouseEvent&)e, MouseButton::Middle, handled);
        else if (eventType == wxEVT_RIGHT_UP)
            _owner->GetMouseInternal()->OnMouseUp((wxMouseEvent&)e, MouseButton::Right, handled);
        else if (eventType == wxEVT_AUX1_UP)
            _owner->GetMouseInternal()->OnMouseUp((wxMouseEvent&)e, MouseButton::XButton1, handled);
        else if (eventType == wxEVT_AUX2_UP)
            _owner->GetMouseInternal()->OnMouseUp((wxMouseEvent&)e, MouseButton::XButton2, handled);
        else if (eventType == wxEVT_LEFT_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick((wxMouseEvent&)e, MouseButton::Left, handled);
        else if (eventType == wxEVT_MIDDLE_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick((wxMouseEvent&)e, MouseButton::Middle, handled);
        else if (eventType == wxEVT_RIGHT_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick((wxMouseEvent&)e, MouseButton::Right, handled);
        else if (eventType == wxEVT_AUX1_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick((wxMouseEvent&)e, MouseButton::XButton1, handled);
        else if (eventType == wxEVT_AUX2_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick((wxMouseEvent&)e, MouseButton::XButton2, handled);

        return handled ? Event_Processed : Event_Skip;
    }

    void App::SetOwner(Application* value)
    {
        _owner = value;
    }

    //-----------------

    void IdleCallback()
    {
        Application::GetCurrent()->RaiseIdle();
    }

    Application::Application()
    {
        Image::EnsureImageHandlersInitialized();

        if (s_current != nullptr)
            throwExInvalidOp;
        s_current = this;

#ifdef __WXMSW__
        windowsVisualThemeSupportCookie = WindowsVisualThemeSupport::GetInstance().Enable();
#endif

        char b[] = "";
        char* argv[] = { b };
        int argc = 1;
        wxEntryStart(argc, argv);
        wxTheApp->CallOnInit();

        _keyboard = new Keyboard();
        _mouse = new Mouse();

        _app = static_cast<App*>(wxTheApp);
        _app->SetOwner(this);

        ParkingWindow::SetIdleCallback(IdleCallback);
    }

    Application::~Application()
    {
        s_current = nullptr;

        _keyboard->Release();
        _keyboard = nullptr;

        _mouse->Release();
        _mouse = nullptr;
    }

    void* Application::GetParentOverrideHandle()
    {
        return _parentOverrideHandle;
    }

    void Application::SetParentOverrideHandle(void* value)
    {
        _parentOverrideHandle = value;
    }

    void Application::WakeUpIdle()
    {
        _app->WakeUpIdle();
    }

    bool Application::GetInUixmlPreviewerMode()
    {
        return _inUixmlPreviewerMode;
    }

    void Application::SetInUixmlPreviewerMode(bool value)
    {
        _inUixmlPreviewerMode = value;
    }

    string Application::GetName()
    {
        return _name;
    }

    void Application::SetName(const string& value)
    {
        _name = value;
    }

    void Application::RaiseIdle()
    {
        RaiseEvent(ApplicationEvent::Idle);
    }

    Mouse* Application::GetMouse()
    {
        _mouse->AddRef();
        return _mouse;
    }

    Keyboard* Application::GetKeyboard()
    {
        _keyboard->AddRef();
        return _keyboard;
    }


    Mouse* Application::GetMouseInternal()
    {
        return _mouse;
    }

    Keyboard* Application::GetKeyboardInternal()
    {
        return _keyboard;
    }

    /*static*/ Application* Application::GetCurrent()
    {
        return s_current;
    }

    void Application::Run(Window* window)
    {
        verifyNonNull(window);

#ifdef __WXMSW__
        if (!WindowsVisualThemeSupport::GetInstance().IsEnabled())
            windowsVisualThemeSupportCookie = WindowsVisualThemeSupport::GetInstance().Enable();
#endif

        _app->Run();

#ifdef __WXMSW__
        if (WindowsVisualThemeSupport::GetInstance().IsEnabled())
            WindowsVisualThemeSupport::GetInstance().Disable(windowsVisualThemeSupportCookie);
#endif
    }
}