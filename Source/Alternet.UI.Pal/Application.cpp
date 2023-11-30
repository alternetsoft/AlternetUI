#include "Application.h"
#include "Common.h"
#include "Window.h"
#include "Image.h"
#include "GenericImage.h"
#include "Exceptions.h"

#include <wx/sysopt.h>

IMPLEMENT_APP_NO_MAIN(Alternet::UI::App);
IMPLEMENT_WX_THEME_SUPPORT;

namespace Alternet::UI
{
    App::App()
    {
    }

    wxWindow* App::GetTopWindow() const
    {
        //return wxApp::GetTopWindow();
        
        wxWindow* window = m_topWindow;

        // If there is no top window or it is about to be destroyed,
        // we need to search for the first TLW which is not pending delete
        if (!window || wxPendingDelete.Member(window))
        {
            window = NULL;
            wxWindowList::compatibility_iterator node = wxTopLevelWindows.GetFirst();
            while (node)
            {
                wxWindow* win = node->GetData();
                if (!wxPendingDelete.Member(win))
                {
                    //if (win->IsShownOnScreen())
                    {
                        window = win;
                        break;
                    }
                }
                node = node->GetNext();
            }
        }

        return window;
    }

    bool App::OnInit()
    {
#if defined(__WXGTK__)
        wxApp::GTKAllowDiagnosticsControl();
#endif
#ifdef _DEBUG
        wxLog::SetActiveTarget(new wxAlternetLog());
        wxLog::GetActiveTarget()->SetFormatter(new wxAlternetLogFormatter());
#endif
        return wxApp::OnInit();
    }

    int App::OnExit()
    {
        return wxApp::OnExit();
    }

    void App::Run()
    {
        wxApp::OnRun();
    }

    void App::OnFatalException()
    {
        if (_owner != nullptr)
            _owner->OnFatalException();
        wxApp::OnFatalException();
    }

    void App::OnAssertFailure(const wxChar* file, int line, const wxChar* func,
        const wxChar* cond, const wxChar* msg)
    {
        if (_owner != nullptr)
            _owner->OnAssertFailure(file, line, func, cond, msg);
        wxApp::OnAssertFailure(file, line, func, cond, msg);
    }

    void App::OnUnhandledException()
    {
        if (_owner != nullptr)
            _owner->OnUnhandledException();
        wxApp::OnUnhandledException();
    }

    bool App::OnExceptionInMainLoop()
    {
        if (_owner != nullptr)
            _owner->OnExceptionInMainLoop();
        return wxApp::OnExceptionInMainLoop();
    }

/*

wxDEFINE_EVENT( wxEVT_LEFT_DOWN, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_LEFT_UP, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_MIDDLE_DOWN, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_MIDDLE_UP, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_RIGHT_DOWN, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_RIGHT_UP, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_MOTION, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_ENTER_WINDOW, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_LEAVE_WINDOW, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_LEFT_DCLICK, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_MIDDLE_DCLICK, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_RIGHT_DCLICK, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_SET_FOCUS, wxFocusEvent );
wxDEFINE_EVENT( wxEVT_KILL_FOCUS, wxFocusEvent );
wxDEFINE_EVENT( wxEVT_CHILD_FOCUS, wxChildFocusEvent );
wxDEFINE_EVENT( wxEVT_MOUSEWHEEL, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_AUX1_DOWN, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_AUX1_UP, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_AUX1_DCLICK, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_AUX2_DOWN, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_AUX2_UP, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_AUX2_DCLICK, wxMouseEvent );
wxDEFINE_EVENT( wxEVT_MAGNIFY, wxMouseEvent );
*/
    void App::ProcessMouseEvent(wxMouseEvent& e, bool& handled)
    {
        auto eventType = e.GetEventType();

        if (eventType == wxEVT_MOTION)
        {
            _owner->GetMouseInternal()->OnMouseMove(e, handled);
            return;
        }      

        if (eventType == wxEVT_MOUSEWHEEL)
            _owner->GetMouseInternal()->OnMouseWheel(e, handled);
        else if (eventType == wxEVT_LEFT_DOWN)
        {
            _owner->GetMouseInternal()->OnMouseDown(e, MouseButton::Left, handled);
        }
        else if (eventType == wxEVT_MIDDLE_DOWN)
            _owner->GetMouseInternal()->OnMouseDown(e, MouseButton::Middle, handled);
        else if (eventType == wxEVT_RIGHT_DOWN)
            _owner->GetMouseInternal()->OnMouseDown(e, MouseButton::Right, handled);
        else if (eventType == wxEVT_AUX1_DOWN)
            _owner->GetMouseInternal()->OnMouseDown(
                e, MouseButton::XButton1, handled);
        else if (eventType == wxEVT_AUX2_DOWN)
            _owner->GetMouseInternal()->OnMouseDown(
                e, MouseButton::XButton2, handled);
        else if (eventType == wxEVT_LEFT_UP)
        {
            _owner->GetMouseInternal()->OnMouseUp(e, MouseButton::Left, handled);
        }
        else if (eventType == wxEVT_MIDDLE_UP)
            _owner->GetMouseInternal()->OnMouseUp(e, MouseButton::Middle, handled);
        else if (eventType == wxEVT_RIGHT_UP)
            _owner->GetMouseInternal()->OnMouseUp(e, MouseButton::Right, handled);
        else if (eventType == wxEVT_AUX1_UP)
            _owner->GetMouseInternal()->OnMouseUp(e, MouseButton::XButton1, handled);
        else if (eventType == wxEVT_AUX2_UP)
            _owner->GetMouseInternal()->OnMouseUp(e, MouseButton::XButton2, handled);
        else if (eventType == wxEVT_LEFT_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick(
                e, MouseButton::Left, handled);
        else if (eventType == wxEVT_MIDDLE_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick(
                e, MouseButton::Middle, handled);
        else if (eventType == wxEVT_RIGHT_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick(
                e, MouseButton::Right, handled);
        else if (eventType == wxEVT_AUX1_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick(
                e, MouseButton::XButton1, handled);
        else if (eventType == wxEVT_AUX2_DCLICK)
            _owner->GetMouseInternal()->OnMouseDoubleClick(
                e, MouseButton::XButton2, handled);
    }

/*
wxDEFINE_EVENT( wxEVT_CHAR, wxKeyEvent );
wxDEFINE_EVENT( wxEVT_AFTER_CHAR, wxKeyEvent );
wxDEFINE_EVENT( wxEVT_CHAR_HOOK, wxKeyEvent );
wxDEFINE_EVENT( wxEVT_NAVIGATION_KEY, wxNavigationKeyEvent );
wxDEFINE_EVENT( wxEVT_KEY_DOWN, wxKeyEvent );
wxDEFINE_EVENT( wxEVT_KEY_UP, wxKeyEvent );
#if wxUSE_HOTKEY
wxDEFINE_EVENT( wxEVT_HOTKEY, wxKeyEvent );
#endif
*/
    void App::ProcessKeyEvent(wxKeyEvent& e, bool& handled)
    {
        auto eventType = e.GetEventType();
        if (eventType == wxEVT_KEY_UP)
            _owner->GetKeyboardInternal()->OnKeyUp(e, handled);
        else if (eventType == wxEVT_KEY_DOWN)
        {
#ifndef __WXOSX_COCOA__            
            // For some reason, on Windows and Linux wxEVT_CHAR_HOOK are not sent
            // when mouse capture is active.
            if (wxWindow::GetCapture() != nullptr)
                _owner->GetKeyboardInternal()->OnKeyDown(e, handled);
#endif                
        }
        else if (eventType == wxEVT_CHAR_HOOK)
            _owner->GetKeyboardInternal()->OnKeyDown(e, handled);
        else if (eventType == wxEVT_CHAR)
            _owner->GetKeyboardInternal()->OnChar(e, handled);
    }

    int App::FilterEvent(wxEvent& e)
    {
        if (_owner == nullptr)
            return Event_Skip;

        auto category = e.GetEventCategory();

        if(category != wxEVT_CATEGORY_USER_INPUT)
            return Event_Skip;

        bool handled = false;

        auto eventType = e.GetEventType();

        if (eventType >= wxEVT_LEFT_DOWN && eventType <= wxEVT_MAGNIFY)
            ProcessMouseEvent((wxMouseEvent&)e, handled);
        else
        if (eventType >= wxEVT_CHAR && eventType <= wxEVT_KEY_UP)
            ProcessKeyEvent((wxKeyEvent&)e, handled);

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

    void LogExceptionInfo(const wxString& s)
    {
        Application::LogSeparator();
        Application::Log(s);
        Application::Log(Exception::_exception);
        Application::LogSeparator();
    }

    Application::Application()
    {
        Exception::_logMessageProc = LogExceptionInfo;

        GenericImage::EnsureImageHandlersInitialized();

        if (s_current != nullptr)
            throwExInvalidOpWithInfo(wxStr("Application::Application"));
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
        _clipboard = new Clipboard();

        _app = static_cast<App*>(wxTheApp);
        _app->SetOwner(this);

        ParkingWindow::SetIdleCallback(IdleCallback);
    }

    void Application::SuppressDiagnostics(int flags)
    {
#if defined(__WXGTK__)
        wxApp::GTKSuppressDiagnostics(flags);
#endif
    }

    Application::~Application()
    {
        s_current = nullptr;

        _keyboard->Release();
        _keyboard = nullptr;

        _mouse->Release();
        _mouse = nullptr;

        _clipboard->Release();
        _clipboard = nullptr;
    }

    void* Application::GetDisplayMode()
    {
        return nullptr;
    }

    bool Application::GetExitOnFrameDelete()
    {
        return _app->GetExitOnFrameDelete();
    }

    int Application::GetLayoutDirection()
    {
        return _app->GetLayoutDirection();
    }

    bool Application::GetUseBestVisual()
    {
        return _app->GetUseBestVisual();
    }

    bool Application::IsActive()
    {
        return _app->IsActive();
    }

    bool Application::SafeYield(void* window, bool onlyIfNeeded)
    {
        return _app->SafeYield((wxWindow*)window, onlyIfNeeded);
    }

    bool Application::SafeYieldFor(void* window, int64_t eventsToProcess)
    {
        return _app->SafeYieldFor((wxWindow*)window, eventsToProcess);
    }

    bool Application::SetDisplayMode(void* videoMode)
    {
        return false;
    }

    void Application::SetExitOnFrameDelete(bool flag)
    {
        _app->SetExitOnFrameDelete(flag);
    }

    bool Application::SetNativeTheme(const string& theme)
    {
        return _app->SetNativeTheme(wxStr(theme));
    }

    void Application::SetTopWindow(void* window)
    {
        _app->SetTopWindow((wxWindow*)window);
    }

    void Application::SetUseBestVisual(bool flag, bool forceTrueColour)
    {
        _app->SetUseBestVisual(flag, forceTrueColour);
    }

    void Application::WakeUpIdle()
    {
        _app->WakeUpIdle();
    }

    void* Application::GetTopWindow()
    {
        return _app->GetTopWindow();
    }

    void Application::ExitMainLoop()
    {
        _app->ExitMainLoop();
    }

    void Application::Exit()
    {
        _app->Exit();
    }

    bool Application::GetInUixmlPreviewerMode()
    {
        return _inUixmlPreviewerMode;
    }

    void Application::SetInUixmlPreviewerMode(bool value)
    {
        _inUixmlPreviewerMode = value;
    }

    string Application::GetDisplayName() 
    {
        return wxStr(_app->GetAppDisplayName());
    }

    void Application::SetDisplayName(const string& value) 
    {
        _app->SetAppDisplayName(wxStr(value));
    }

    string Application::GetAppClassName() 
    {
        return wxStr(_app->GetClassName());
    }
    void Application::SetAppClassName(const string& value)
    {
        _app->SetClassName(wxStr(value));
    }

    string Application::GetVendorName()
    {
        return wxStr(_app->GetVendorName());
    }

    void Application::SetVendorName(const string& value)
    {
        _app->SetVendorName(wxStr(value));
    }

    string Application::GetVendorDisplayName()
    {
        return wxStr(_app->GetVendorDisplayName());
    }

    void Application::SetVendorDisplayName(const string& value)
    {
        _app->SetVendorDisplayName(wxStr(value));
    }

    string Application::GetName()
    {
        return wxStr(_app->GetAppName());
    }

    void Application::SetName(const string& value)
    {
        _app->SetAppName(wxStr(value));
    }

    void Application::RaiseIdle()
    {
        RaiseEvent(ApplicationEvent::Idle);
    }

    Clipboard* Application::GetClipboard()
    {
        _clipboard->AddRef();
        return _clipboard;
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


    bool Application::GetInvokeRequired()
    {
        return !wxThread::IsMain();
    }

    void Application::BeginInvoke(PInvokeCallbackActionType action)
    {
        ParkingWindow::GetWindow()->CallAfter([=]() { action(); });
    }

    Mouse* Application::GetMouseInternal()
    {
        return _mouse;
    }

    Keyboard* Application::GetKeyboardInternal()
    {
        return _keyboard;
    }

    string Application::GetEventArgString()
    {
        return _eventArgString;
    }

    void Application::Log(const wxString& msg)
    {
        Log(wxStr(msg));
    }

    void Application::Log(const string& msg)
    {
        GetCurrent()->_eventArgString = msg;
        GetCurrent()->RaiseEvent(ApplicationEvent::LogMessage);
    }

    void Application::DoLogRecord(wxLogLevel level /*unsigned long*/, const wxString& msg,
        const wxLogRecordInfo& info)
    {
        _eventArgString = wxStr(msg);
        RaiseEvent(ApplicationEvent::LogMessage);
    }

    /*static*/ Application* Application::GetCurrent()
    {
        return s_current;
    }

    void Application::SetSystemOptionInt(const string& name, int value)
    {
        wxSystemOptions::SetOption(wxStr(name), value);
    }

    void Application::ProcessPendingEvents()
    {
        _app->ProcessPendingEvents();
    }

    bool Application::HasPendingEvents()
    {
        return _app->HasPendingEvents();
    }

    void Application::ThrowError(int value)
    {
        throwExInvalidOpWithInfo(wxStr("Sample exception"));
        //throw value;
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

    /*
This function may be called if something fatal happens: an unhandled exception under Win32 or a
fatal signal under Unix, for example.
However, this will not happen by default: you have to explicitly call wxHandleFatalExceptions()
to enable this.
Generally speaking, this function should only show a message to the user and return. You may
attempt to save unsaved data but this is not guaranteed to work and, in fact, probably won't.

wxHandleFatalExceptions:
If doIt is true, the fatal exceptions (also known as general protection faults under Windows or
segmentation violations in the Unix world) will be caught and passed to wxApp::OnFatalException.
By default, i.e. before this function is called, they will be handled in the normal way which
usually just means that the application will be terminated. Calling wxHandleFatalExceptions()
with doIt equal to false will restore this default behaviour.
Notice that this function is only available if wxUSE_ON_FATAL_EXCEPTION is 1 and under Windows
platform this requires a compiler with support for SEH (structured exception handling) which
currently means only Microsoft Visual C++.
    */
    bool Application::OnFatalException()
    {
        Application::Log("Error: Fatal Exception");
        RaiseEvent(ApplicationEvent::FatalException);
        return false;
    }

    /*
This function is called when an assert failure occurs, i.e. the condition specified in wxASSERT()
macro evaluated to false.
It is only called in debug mode (when __WXDEBUG__ is defined) as asserts are not left in the
release code at all. The base class version shows the default assert failure dialog box proposing to the user to stop the program, continue or ignore all subsequent asserts.
Parameters
file
the name of the source file where the assert occurred
line
the line number in this file where the assert occurred
func
the name of the function where the assert occurred, may be empty if the compiler doesn't support
C99 __FUNCTION__
cond
the condition of the failed assert in text form
msg
the message specified as argument to wxASSERT_MSG or wxFAIL_MSG, will be NULL if just wxASSERT
or wxFAIL was used
    */
    bool Application::OnAssertFailure(const wxChar* file, int line, const wxChar* func,
        const wxChar* cond, const wxChar* msg)
    {
        wxString sFile = file;
        wxString sLine = std::to_string(line);
        wxString sFunc = func;
        wxString sCond = cond;
        wxString sMsg = msg;

        Application::LogSeparator();
        Application::Log("Error: Assert Failure");        
        Application::Log("File: " + sFile);
        Application::Log("Line: " + sLine);
        Application::Log("Func: " + sFunc);
        Application::Log("Cond: " + sCond);
        Application::Log("Msg:  " + sMsg);

        Application::LogSeparator();
        RaiseEvent(ApplicationEvent::AssertFailure);
        return false;
    }

/*
This function is called when an unhandled C++ exception occurs in user code called by wxWidgets.

Any unhandled exceptions thrown from (overridden versions of) OnInit() and OnExit() methods
as well as any exceptions thrown from inside the main loop and re-thrown by OnUnhandledException()
will result in a call to this function.

By the time this function is called, the program is already about to exit and the exception can't
be handled nor ignored any more, override OnUnhandledException() or use explicit try/catch blocks
around OnInit() body to be able to handle the exception earlier.

The default implementation dumps information about the exception using wxMessageOutputBest.
*/
    bool Application::OnUnhandledException()
    {
        Application::Log("Error: Unhandled Exception");
        RaiseEvent(ApplicationEvent::UnhandledException);
        return false;
    }

/*
virtual bool wxAppConsole::OnExceptionInMainLoop	(		)
This function is called if an unhandled exception occurs inside the main application event loop.

It can return true to ignore the exception and to continue running the loop or false to exit the
loop and terminate the program.

The default behaviour of this function is the latter in all ports except under Windows where a dialog
is shown to the user which allows him to choose between the different options. You may override this
function in your class to do something more appropriate.

If this method rethrows the exception and if the exception can't be stored for later processing using
StoreCurrentException(), the program will terminate after calling OnUnhandledException().

You should consider overriding this method to perform whichever last resort exception handling that
would be done in a typical C++ program in a try/catch block around the entire main() function. As
this method is called during exception handling, you may use the C++ throw keyword to rethrow
the current exception to catch it again and analyze it. For example:

class MyApp : public wxApp {
public:
    virtual bool OnExceptionInMainLoop()
    {
        wxString error;
        try {
            throw; // Rethrow the current exception.
        } catch (const MyException& e) {
            error = e.GetMyErrorMessage();
        } catch (const std::exception& e) {
            error = e.what();
        } catch ( ... ) {
            error = "unknown error.";
        }

        wxLogError("Unexpected exception has occurred: %s, the program will terminate.", error);

        // Exit the main loop and thus terminate the program.
        return false;
    }
};
*/
    bool Application::OnExceptionInMainLoop()
    {
        Application::Log("Error: Exception In Main Loop");
        RaiseEvent(ApplicationEvent::ExceptionInMainLoop);
        return false;
    }
}