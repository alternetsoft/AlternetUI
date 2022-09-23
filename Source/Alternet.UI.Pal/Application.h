#pragma once

#include "Common.h"
#include "Window.h"
#include "Keyboard.h"
#include "Clipboard.h"
#include "Mouse.h"
#include "Object.h"
#include "WindowsVisualThemeSupport.h"

namespace Alternet::UI
{
    class Application;

    class App : public wxApp
    {
    public:
        App();

        bool OnInit() override;
        int OnRun() override;
        int OnExit() override;

        void Run();

        int FilterEvent(wxEvent& event) override;

        void SetOwner(Application* value);

    private:

        Application* _owner = nullptr;

        BYREF_ONLY(App);
    };

    class Application : public Object
    {
#include "Api/Application.inc"
    public:
        Mouse* GetMouseInternal();
        Keyboard* GetKeyboardInternal();

        void RaiseIdle();

        static Application* GetCurrent();
    private:

        App* _app = nullptr;

        string _name;

        Keyboard* _keyboard = nullptr;
        Mouse* _mouse = nullptr;
        Clipboard* _clipboard = nullptr;

        bool _inUixmlPreviewerMode = false;

        inline static Application* s_current = nullptr;

#ifdef __WXMSW__
        ULONG_PTR windowsVisualThemeSupportCookie = NULL;
#endif
    };
}