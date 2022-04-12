#include "Application.h"
#include "Common.h"
#include "Window.h"

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
        {
            _owner->GetKeyboard()->OnKeyUp((wxKeyEvent&)e, handled);
        }
        else if (eventType == wxEVT_CHAR_HOOK)
        {
            _owner->GetKeyboard()->OnKeyDown((wxKeyEvent&)e, handled);
        }
        else if (eventType == wxEVT_CHAR)
        {
            _owner->GetKeyboard()->OnChar((wxKeyEvent&)e, handled);
        }
        else if (eventType == wxEVT_MOTION)
        {
            _owner->GetMouse()->OnMouseMove((wxMouseEvent&)e, handled);
        }

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
        wxASSERT(s_current == nullptr);
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

    void Application::RaiseIdle()
    {
        RaiseEvent(ApplicationEvent::Idle);
    }

    Mouse* Application::GetMouse()
    {
        return _mouse;
    }

    /*static*/ Application* Application::GetCurrent()
    {
        return s_current;
    }

    Keyboard* Application::GetKeyboard()
    {
        return _keyboard;
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