#pragma once



#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"
#include "Exceptions.h"

#include <wx/webview.h>

namespace Alternet::UI
{
    class WebBrowser : public Control
    {
#include "Api/WebBrowser.inc"
    private:
        static wxString DefaultUserAgent;
        static wxString DefaultScriptMesageName;
        static wxString DefaultFSNameMemory;
        static wxString DefaultFSNameArchive;
        static wxString DefaultFSNameCustom;
        static WebBrowserBackend DefaultBackend;
        static wxString DefaultPage;
        static bool IELatest;
        static wxString WebViewBackendNameFromId(WebBrowserBackend id);

        WebBrowserBackend Backend = WebBrowserBackend::Default;
        int preferredColorScheme = 0;
        bool hasBorder = true;
        wxWebView* webView = nullptr;
        wxWindow* webViewParent = nullptr;
        bool DefaultUserAgentDone = false;
        bool DefaultScriptMesageNameDone = false;
        bool DefaultFSNameMemoryDone = false;
        bool DefaultFSNameArchiveDone = false;
        bool DefaultFSNameCustomDone = false;

        bool IsBackendIE();
        void RaiseEventEx(WebBrowserEvent eventID, wxWebViewEvent& event,bool canVeto=FALSE);
        void RaiseSimpleEvent(WebBrowserEvent eventId, bool canVeto = FALSE);
        
#if defined(__WXMSW__)
       bool IERunScript(const wxString& javascript, wxString* output);
       int IEGetScriptErrorsSuppressed();
       void IEShowPrintPreviewDialog();
       void IESetScriptErrorsSuppressed(bool value);
#endif

        void ProcessDefaultsOnCreate(bool before);
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;
        wxWebView* GetWebViewCtrl();

        void RegisterHandlerZip(const wxString& schemeName);
        void RegisterHandlerMemory(const wxString& schemeName);

        WebBrowser(const wxString& url);

        static bool IsBackendAvailable(const wxString& value);
        static bool IsBackendIEAvailable();
        static bool IsBackendEdgeAvailable();
        static bool IsBackendWebKitAvailable();
        static void SetBackend(WebBrowserBackend value);
        static wxString GetBackendVersionString(WebBrowserBackend id);
        static wxString GetLibraryVersionString();

        virtual void OnSizeChanged(wxSizeEvent& event) override;

    protected:
        void OnWxWindowCreated() override;
        bool IsCursorSuppressed() override { return true; }

    private:
        wxString _defaultPage;
        wxString _urlContainer;
        wxString _textContainer;
        wxString _targetContainer;
        wxString _messageHandlerContainer;

        void OnBeforeBrowserCreate(wxWebViewEvent& event);
        void OnNavigating(wxWebViewEvent& event);
        void OnNavigated(wxWebViewEvent& event);
        void OnLoaded(wxWebViewEvent& event);
        void OnError(wxWebViewEvent& event);
        void OnNewWindow(wxWebViewEvent& event);
        void OnTitleChanged(wxWebViewEvent& event);
        void OnFullScreenChanged(wxWebViewEvent& event);
        void OnScriptMessageReceived(wxWebViewEvent& event);
        void OnScriptResult(wxWebViewEvent& event);
    };
}


