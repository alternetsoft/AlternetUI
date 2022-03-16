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

        _app = static_cast<App*>(wxTheApp);

        ParkingWindow::SetIdleCallback(IdleCallback);
    }

    Application::~Application()
    {
        s_current = nullptr;
    }

    void Application::RaiseIdle()
    {
        RaiseEvent(ApplicationEvent::Idle);
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