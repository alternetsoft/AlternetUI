#pragma once

#include "Common.h"
#include "Window.h"
#include "WindowsVisualThemeSupport.h"

namespace Alternet::UI
{
    class App : public wxApp
    {
    public:
        App();

        bool OnInit() override;
        int OnRun() override;
        int OnExit() override;

        void Run();

    private:

        BYREF_ONLY(App);
    };

    class Application
    {
#include "Api/Application.inc"
    public:

    private:

        App* _app;

#ifdef __WXMSW__
        ULONG_PTR windowsVisualThemeSupportCookie = NULL;
#endif
    };
}