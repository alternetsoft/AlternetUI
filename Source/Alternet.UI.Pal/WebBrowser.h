#pragma once



#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"
#include "Exceptions.h"


namespace Alternet::UI
{
    enum WebBrowserBackend
    {
        WEBBROWSER_BACKEND_DEFAULT = 0,
        WEBBROWSER_BACKEND_IE = 1,
        WEBBROWSER_BACKEND_IELATEST=2,
        WEBBROWSER_BACKEND_EDGE=3,
        WEBBROWSER_BACKEND_WEBKIT=4,
    };

    class WebBrowser : public Control
    {
#include "Api/WebBrowser.inc"
    private:
        static WebBrowserBackend DefaultBackend;
        static wxString DefaultPage;
        static bool IELatest;
        static wxString WebViewBackendNameFromId(WebBrowserBackend id);

        WebBrowserBackend Backend;

        void RaiseEventEx(WebBrowserEvent eventID, wxWebViewEvent& event,bool canVeto=FALSE);
        void IEShowPrintPreviewDialog();
    public:
        WebBrowserBackend GetBackend();

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWebView* GetWebViewCtrl();

        void RegisterHandlerZip(const string& schemeName);
        void RegisterHandlerMemory(const string& schemeName);

        static void SetDefaultPage(const string& value);
        static bool IsBackendAvailable(const string& value);
        static bool IsBackendIEAvailable();
        static bool IsBackendEdgeAvailable();
        static bool IsBackendWebKitAvailable();
        //static bool IsBackendWeb2KitAvailable();
        static void SetBackend(WebBrowserBackend value);
        static string GetBackendVersionString(WebBrowserBackend id);
        static string GetLibraryVersionString();

        bool CanSetZoomType(wxWebViewZoomType type);
        wxWebViewZoomType GetZoomType();
        void SetZoomType(wxWebViewZoomType zoomType);

        long Find(const string& text, int flags);
    protected:
        virtual void OnWxWindowCreated() override;

    private:
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


