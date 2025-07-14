#pragma once

#include "Common.h"
#include "Window.h"
#include "Keyboard.h"
#include "Clipboard.h"
#include "Mouse.h"
#include "Object.h"
#include "WindowsVisualThemeSupport.h"
#include "WxAlternet/wxAlternetLogFormatter.h"

#include <wx/log.h>

namespace Alternet::UI
{
    class Application;

    class App : public wxApp
    {
    public:
        App();

#if defined(__WXGTK__)
        void InjectGtkCss()
#endif

        void OnAssertFailure(const wxChar* file, int line, const wxChar* func,
            const wxChar* cond, const wxChar* msg) override;
        void OnUnhandledException() override;
        void OnFatalException() override;
        bool OnExceptionInMainLoop() override;
        bool OnInit() override;
        int OnExit() override;
        wxWindow* GetTopWindow() const override;

        void Run();

        int FilterEvent(wxEvent& event) override;
        void ProcessMouseEvent(wxMouseEvent& e, bool& handled);
        void ProcessKeyEvent(wxKeyEvent& e, bool& handled);

        void SetOwner(Application* value);
    private:

        Application* _owner = nullptr;

       // BYREF_ONLY(App);
    };

    class Application : public Object
    {
#include "Api/Application.inc"
    public:
        static wxString _gtkCss;
        static bool _injectGtkCss;

        Mouse* GetMouseInternal();
        Keyboard* GetKeyboardInternal();

        void RaiseIdle();

        static Application* GetCurrent();

        void DoLogRecord(wxLogLevel level, const wxString& msg,
            const wxLogRecordInfo& info);
        static void Log(const wxString& msg);
        static void Log(const string& msg);

        static void LogSeparator()
        {
            Log("-");
        }

        bool OnFatalException();
        bool OnAssertFailure(const wxChar* file, int line, const wxChar* func,
            const wxChar* cond, const wxChar* msg);       
        bool OnUnhandledException();
        bool OnExceptionInMainLoop();

    private:

        App* _app = nullptr;
        string _eventArgString;

        Keyboard* _keyboard = nullptr;
        Mouse* _mouse = nullptr;
        Clipboard* _clipboard = nullptr;

        bool _inUixmlPreviewerMode = false;

        inline static Application* s_current = nullptr;

#ifdef __WXMSW__
        ULONG_PTR windowsVisualThemeSupportCookie = NULL;
#endif
    };

    // https://docs.wxwidgets.org/3.2/classwx_log.html
    class wxAlternetLog : public wxLog
    {
        void DoLogRecord(wxLogLevel level, const wxString& msg,
            const wxLogRecordInfo& info) override
        {
            auto app = Application::GetCurrent();
            if (app != nullptr)
            {
                app->DoLogRecord(level, msg, info);
            }

            wxLog::DoLogRecord(level, msg, info);
        }

        void DoLogText(const wxString& WXUNUSED(msg)) override
        {
        }
    };

}