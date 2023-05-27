#include "WebBrowser.h"
#include "wx/webview.h"
#include "wx/version.h"
#include "wx/webviewarchivehandler.h"
#include "wx/filesys.h"
#include <wx/webviewfshandler.h>
#include "wx/fs_mem.h"

#if defined(__WXOSX__)
#include "wx/osx/webview_webkit.h"
#endif

#if defined(__WXGTK__)

#endif

#if defined(__WXMSW__)
#include "wx/msw/webview_ie.h"
#include "wx/msw/webview_edge.h"
#endif


namespace Alternet::UI
{
    //-------------------------------------------------
    WebBrowserBackend WebBrowser::DefaultBackend = WEBBROWSER_BACKEND_DEFAULT;
    wxString WebBrowser::DefaultPage = "about:blank";
    bool WebBrowser::IELatest = false;
    //-------------------------------------------------
    void WebBrowser::SetBackend(WebBrowserBackend value)
    {
        WebBrowser::DefaultBackend = value;
    }
    //-------------------------------------------------
    bool WebBrowser::IsBackendWebKitAvailable()
    {
        return wxWebView::IsBackendAvailable(wxWebViewBackendWebKit);
    }    
    //-------------------------------------------------
    /*bool WebBrowser::IsBackendWebKit2Available()
    {
        return wxWebView::IsBackendAvailable(wxWebViewBackendWebKit2);
    } */
    //-------------------------------------------------
    bool WebBrowser::IsBackendIEAvailable()
    {
        return wxWebView::IsBackendAvailable(wxWebViewBackendIE);
    }
    //-------------------------------------------------
    bool WebBrowser::IsBackendEdgeAvailable()
    {
        return wxWebView::IsBackendAvailable(wxWebViewBackendEdge);
    }
    //-------------------------------------------------
    bool WebBrowser::IsBackendAvailable(const string& value)
    {
        return wxWebView::IsBackendAvailable(wxStr(value));
    }
    //-------------------------------------------------
    void WebBrowser::Stop() 
    {
        GetWebViewCtrl()->Stop();
    }
    //-------------------------------------------------
    bool WebBrowser::GetCanGoBack()
    {
        return GetWebViewCtrl()->CanGoBack();
    }
    //-------------------------------------------------
    bool WebBrowser::GetCanGoForward()
    {
        return GetWebViewCtrl()->CanGoForward();
    }
    //-------------------------------------------------
    void WebBrowser::GoBack()
    {
        GetWebViewCtrl()->GoBack();
    }
    //-------------------------------------------------
    void WebBrowser::GoForward()
    {
        GetWebViewCtrl()->GoForward();
    }
    //-------------------------------------------------
    void WebBrowser::ClearHistory()
    {
        GetWebViewCtrl()->ClearHistory();
    }
    //-------------------------------------------------
    void WebBrowser::EnableHistory(bool enable)
    {
        GetWebViewCtrl()->EnableHistory(enable);
    }
    //-------------------------------------------------
    string WebBrowser::GetLibraryVersionString() 
    {
        wxVersionInfo version = wxGetLibraryVersionInfo();
        return wxStr(version.GetVersionString());
    }
    //-------------------------------------------------
    WebBrowser::WebBrowser()
    {
    }
    //-------------------------------------------------
    wxString WebBrowser::WebViewBackendNameFromId(WebBrowserBackend id)
    {
        auto backend = wxASCII_STR(wxWebViewBackendDefault);

        switch (id)
        {
        case WEBBROWSER_BACKEND_IE:
            backend = wxASCII_STR(wxWebViewBackendIE);
            break;
        case WEBBROWSER_BACKEND_IELATEST:
            backend = wxASCII_STR(wxWebViewBackendIE);
            if (!WebBrowser::IELatest)
            {
                wxWebViewIE::MSWSetEmulationLevel();
                WebBrowser::IELatest = true;
            }
            break;
        case WEBBROWSER_BACKEND_EDGE:
            backend = wxASCII_STR(wxWebViewBackendEdge);
            break;
        case WEBBROWSER_BACKEND_WEBKIT:
            backend = wxASCII_STR(wxWebViewBackendWebKit);
            break;
        }
        return backend;
    }
    //-------------------------------------------------
    WebBrowserBackend WebBrowser::GetBackend()
    {
        return Backend;
    }
    //-------------------------------------------------
    void WebBrowser::SetDefaultPage(const string& value)
    {
        WebBrowser::DefaultPage = wxStr(value);
    }
    //-------------------------------------------------
    void* WebBrowser::GetNativeBackend() 
    {
        return GetWebViewCtrl()->GetNativeBackend();
    }
    //-------------------------------------------------
    wxWindow* WebBrowser::CreateWxWindowCore(wxWindow* parent)
    {
        auto backend = WebViewBackendNameFromId(WebBrowser::DefaultBackend);

        Backend = WebBrowser::DefaultBackend;
       /*
        auto webView = new wxWebViewEdge(
            parent,
            -1,
            WebBrowser::DefaultPage,
            wxDefaultPosition,
            wxDefaultSize,
            0,
            "");*/


        auto webView = wxWebView::New(
            parent, 
            -1, 
            WebBrowser::DefaultPage,
            wxDefaultPosition, 
            wxDefaultSize, 
            backend,
            0, 
            "");
        
        //-------------------------------------------------
        webView->Bind(wxEVT_WEBVIEW_NAVIGATING, &WebBrowser::OnNavigating, this);
        webView->Bind(wxEVT_WEBVIEW_NAVIGATED, &WebBrowser::OnNavigated, this);
        webView->Bind(wxEVT_WEBVIEW_LOADED, &WebBrowser::OnLoaded, this);
        webView->Bind(wxEVT_WEBVIEW_ERROR, &WebBrowser::OnError, this);
        webView->Bind(wxEVT_WEBVIEW_NEWWINDOW, &WebBrowser::OnNewWindow, this);
        webView->Bind(wxEVT_WEBVIEW_TITLE_CHANGED, &WebBrowser::OnTitleChanged, this);
        webView->Bind(wxEVT_WEBVIEW_FULLSCREEN_CHANGED, &WebBrowser::OnFullScreenChanged, this);
        webView->Bind(wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED, &WebBrowser::OnScriptMessageReceived, this);
        webView->Bind(wxEVT_WEBVIEW_SCRIPT_RESULT, &WebBrowser::OnScriptResult, this);
        //-------------------------------------------------

    
        //-------------------------------------------------
        return webView;
    }
    //-------------------------------------------------
    WebBrowser::~WebBrowser()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_WEBVIEW_NAVIGATING, &WebBrowser::OnNavigating, this);
                window->Unbind(wxEVT_WEBVIEW_NAVIGATED, &WebBrowser::OnNavigated, this);
                window->Unbind(wxEVT_WEBVIEW_LOADED, &WebBrowser::OnLoaded, this);
                window->Unbind(wxEVT_WEBVIEW_ERROR, &WebBrowser::OnError, this);
                window->Unbind(wxEVT_WEBVIEW_NEWWINDOW, &WebBrowser::OnNewWindow, this);
                window->Unbind(wxEVT_WEBVIEW_TITLE_CHANGED, &WebBrowser::OnTitleChanged, this);
                window->Unbind(wxEVT_WEBVIEW_FULLSCREEN_CHANGED, &WebBrowser::OnFullScreenChanged, this);
                window->Unbind(wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED, &WebBrowser::OnScriptMessageReceived, this);
                window->Unbind(wxEVT_WEBVIEW_SCRIPT_RESULT, &WebBrowser::OnScriptResult, this);
            }
        }
    }
    //-------------------------------------------------
    void WebBrowser::OnWxWindowCreated()
    {
    }
    //-------------------------------------------------
    #define scast(v) (const_cast<char16_t*>(v.c_str()))
    void WebBrowser::RaiseEventEx(WebBrowserEvent eventId, wxWebViewEvent& event, bool canVeto)
    {
        WebBrowserEventData data = { 0 };

        auto url = wxStr(event.GetURL());
        data.Url = scast(url);

        auto intVal = event.GetInt();
        data.IntVal = intVal;

        auto text = wxStr(event.GetString());
        data.Text = scast(text);

        auto target = wxStr(event.GetTarget());
        data.Target = scast(target);

        auto messageHandler = wxStr(event.GetMessageHandler());
        data.MessageHandler = scast(messageHandler);

        data.ActionFlags = event.GetNavigationAction();
        data.IsError = event.IsError();

        if (canVeto) 
        {
            auto result = RaiseEventWithPointerResult(eventId, &data);

            if (result != 0)
                event.Veto();
        } else
            RaiseEvent(eventId, &data);

    }
    //-------------------------------------------------
    void WebBrowser::OnFullScreenChanged(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::FullScreenChanged, event);
    }
    //-------------------------------------------------
    void WebBrowser::OnScriptMessageReceived(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::ScriptMessageReceived, event);
    }
    //-------------------------------------------------
    void WebBrowser::OnScriptResult(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::ScriptResult, event);
    }
    //-------------------------------------------------
    void WebBrowser::OnNavigating(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::Navigating, event,true);
    }
    //-------------------------------------------------
    void WebBrowser::OnNavigated(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::Navigated,event);
    }
    //-------------------------------------------------
    void WebBrowser::OnLoaded(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::Loaded,event);
    }
    //-------------------------------------------------
    void WebBrowser::OnError(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::Error,event);
    }
    //-------------------------------------------------
    void WebBrowser::OnNewWindow(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::NewWindow,event);
    }
    //-------------------------------------------------
    void WebBrowser::OnTitleChanged(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::TitleChanged,event);
    }
    //-------------------------------------------------
    void WebBrowser::RegisterHandlerZip(const string& schemeName)
    {
        GetWebViewCtrl()->RegisterHandler(
            wxSharedPtr<wxWebViewHandler>(new wxWebViewArchiveHandler(wxStr(schemeName))));
    }
    //-------------------------------------------------
    void WebBrowser::RegisterHandlerMemory(const string& schemeName)
    {
        wxFileSystem::AddHandler(new wxMemoryFSHandler);
        GetWebViewCtrl()->RegisterHandler(
            wxSharedPtr<wxWebViewHandler>(new wxWebViewFSHandler(wxStr(schemeName))));
    }
    //-------------------------------------------------
    string WebBrowser::DoCommandGlobal(const string& cmdName, const string& cmdParam1, const string& cmdParam2)
    {
        string noresult = wxStr("");

        return noresult;
    }
    //-------------------------------------------------
    string WebBrowser::DoCommand(const string& cmdName, const string& cmdParam1, const string& cmdParam2)
    {
        string noresult = wxStr("");
        //-------------------------------------------------
        if (cmdName == wxStr("ZipScheme.Init"))
        {
            RegisterHandlerZip(cmdParam1);
            return noresult;
        }
        if (cmdName == wxStr("MemoryScheme.Init"))
        {
            RegisterHandlerMemory(cmdParam1);
            return noresult;
        }
        //-------------------------------------------------
#if defined(__WXMSW__)
        if (cmdName == wxStr("IE.ShowPrintPreviewDialog"))
        {
            IEShowPrintPreviewDialog();
            return noresult;
        }
#endif
        return noresult;
    }
    //-------------------------------------------------
#if defined(__WXOSX__)

#endif
    //-------------------------------------------------
#if defined(__WXGTK__)

#endif
    //-------------------------------------------------
#if defined(__WXMSW__)
    void WebBrowser::IEShowPrintPreviewDialog() 
    {
        void* native = GetNativeBackend();
        IWebBrowser2* wb = static_cast<IWebBrowser2*>(native);
        wb->ExecWB(OLECMDID_PRINTPREVIEW,
            OLECMDEXECOPT_PROMPTUSER, NULL, NULL);
    }
    /*

    void wxWebViewIE::Print()
    {
        m_webBrowser->ExecWB(OLECMDID_PRINTPREVIEW,
                                     OLECMDEXECOPT_DODEFAULT, NULL, NULL);
    }


    public void ShowPrintPreviewDialog()
    {
        IntSecurity.SafePrinting.Demand();
        object pvaIn = null;
        try
        {
            AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PRINTPREVIEW,
            NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER, ref pvaIn, IntPtr.Zero);
        }
        catch (Exception ex)
        {
            if (ClientUtils.IsSecurityOrCriticalException(ex))
            {
                throw;
            }
        }
    }

    */

#endif
    //-------------------------------------------------
    string WebBrowser::GetCurrentTitle()
    {
        return wxStr(GetWebViewCtrl()->GetCurrentTitle());
    }
    //-------------------------------------------------
    string WebBrowser::GetCurrentURL()
    {
        return wxStr(GetWebViewCtrl()->GetCurrentURL());
    }
    //-------------------------------------------------
    bool WebBrowser::GetEditable()
    {
        return GetWebViewCtrl()->IsEditable();
    }
    //-------------------------------------------------
    void WebBrowser::SetEditable(bool value)
    {
        GetWebViewCtrl()->SetEditable(value);
    }
    //-------------------------------------------------
    bool WebBrowser::CanSetZoomType(wxWebViewZoomType type) 
    {
        return GetWebViewCtrl()->CanSetZoomType(type);
    }
    //-------------------------------------------------
    float WebBrowser::GetZoomFactor()
    {
        return GetWebViewCtrl()->GetZoomFactor();
    }
    //-------------------------------------------------
    wxWebViewZoomType WebBrowser::GetZoomType()
    {
        return GetWebViewCtrl()->GetZoomType();
    }
    //-------------------------------------------------
    void WebBrowser::RunScriptAsync(const string& javascript, void* clientData)
    {
        GetWebViewCtrl()->RunScriptAsync(wxStr(javascript), clientData);
    }
    //-------------------------------------------------
    void WebBrowser::SetZoomFactor(float zoom)
    {
        GetWebViewCtrl()->SetZoomFactor(zoom);
    }
    //-------------------------------------------------
    void WebBrowser::SetZoomType(wxWebViewZoomType zoomType)
    {
        GetWebViewCtrl()->SetZoomType(zoomType);
    }
    //-------------------------------------------------
    int WebBrowser::GetZoom()
    {
        return GetWebViewCtrl()->GetZoom();
    }
    //-------------------------------------------------
    void WebBrowser::SetZoom(int value)
    {
        GetWebViewCtrl()->SetZoom((wxWebViewZoom)value);
    }
    //-------------------------------------------------
    void WebBrowser::LoadURL(const string& url)
    {
        GetWebViewCtrl()->LoadURL(wxStr(url));
    }
    //-------------------------------------------------
    void WebBrowser::ReloadDefault()
    {
        GetWebViewCtrl()->Reload();
    }
    //-------------------------------------------------
    void WebBrowser::Reload(bool noCache)
    {
        if(noCache)
            GetWebViewCtrl()->Reload();
        else
            GetWebViewCtrl()->Reload(wxWEBVIEW_RELOAD_NO_CACHE);
    }
    //-------------------------------------------------
    void WebBrowser::SetPage(const string& html, const string& baseUrl)
    {
        GetWebViewCtrl()->SetPage(wxStr(html), wxStr(baseUrl));
    }
    //-------------------------------------------------
    string WebBrowser::GetBackendVersionString(WebBrowserBackend id)
    {
        auto backend = WebViewBackendNameFromId(id);
        wxVersionInfo info = wxWebView::GetBackendVersionInfo(backend);
        auto s = info.GetVersionString();
        return wxStr(s);
    }
    //-------------------------------------------------
    void WebBrowser::SelectAll()
    {
        GetWebViewCtrl()->SelectAll();
    }
    //-------------------------------------------------
    bool WebBrowser::GetHasSelection() 
    { 
        return GetWebViewCtrl()->HasSelection();
    }
    //-------------------------------------------------
    void WebBrowser::DeleteSelection()
    {
        GetWebViewCtrl()->DeleteSelection();
    }
    //-------------------------------------------------
    string WebBrowser::GetSelectedText() 
    { 
        return wxStr(GetWebViewCtrl()->GetSelectedText());
    }
    //-------------------------------------------------
    string WebBrowser::GetSelectedSource() 
    { 
        return wxStr(GetWebViewCtrl()->GetSelectedSource());
    }
    //-------------------------------------------------
    void WebBrowser::ClearSelection()
    {
        GetWebViewCtrl()->ClearSelection();
    }
    //-------------------------------------------------
    bool WebBrowser::GetCanCut() 
    { 
        return GetWebViewCtrl()->CanCut();
    }
    //-------------------------------------------------
    bool WebBrowser::GetCanCopy()
    { 
        return GetWebViewCtrl()->CanCopy();
    }
    //-------------------------------------------------
    bool WebBrowser::GetCanPaste()
    { 
        return GetWebViewCtrl()->CanPaste();
    }
    //-------------------------------------------------
    void WebBrowser::Cut() 
    {
        GetWebViewCtrl()->Cut();
    }
    //-------------------------------------------------
    void WebBrowser::Copy() 
    {
        GetWebViewCtrl()->Copy();
    }
    //-------------------------------------------------
    void WebBrowser::Paste() 
    {
        GetWebViewCtrl()->Paste();
    }
    //-------------------------------------------------
    bool WebBrowser::GetCanUndo() 
    { 
        return GetWebViewCtrl()->CanUndo();
    }
    //-------------------------------------------------
    bool WebBrowser::GetCanRedo() 
    { 
        return GetWebViewCtrl()->CanRedo();
    }
    //-------------------------------------------------
    void WebBrowser::Undo() 
    {
        GetWebViewCtrl()->Undo();
    }
    //-------------------------------------------------
    void WebBrowser::Redo() 
    {
        GetWebViewCtrl()->Redo();
    }
    //-------------------------------------------------
    //https://docs.wxwidgets.org/3.2/classwx_web_view.html#a67000a368c45f3684efd460d463ffb98
    string WebBrowser::RunScript(const string& javascript) 
    {
        wxString output;
        wxString script = wxStr(javascript);
        bool result = GetWebViewCtrl()->RunScript(script, &output);
        if(result)
            return wxStr(output);
        return wxStr("3140D0CF550442968B792551E6DCBEC1");
    }
    //-------------------------------------------------
    bool WebBrowser::AddUserScript(const string& javascript,int injectionTime) 
    {
        wxString script = wxStr(javascript);
        bool result = GetWebViewCtrl()->AddUserScript(script, 
            (wxWebViewUserScriptInjectionTime)injectionTime);
        return result;
    }
    //-------------------------------------------------
    long WebBrowser::Find(const string& text, int flags) 
    {
        return GetWebViewCtrl()->Find(wxStr(text), flags);
    }
    //-------------------------------------------------
    bool WebBrowser::GetIsBusy() 
    { 
        return GetWebViewCtrl()->IsBusy();
    }
    //-------------------------------------------------
    bool WebBrowser::GetContextMenuEnabled() 
    { 
        return GetWebViewCtrl()->IsContextMenuEnabled();
    }
    //-------------------------------------------------
    bool WebBrowser::GetAccessToDevToolsEnabled() 
    { 
        return GetWebViewCtrl()->IsAccessToDevToolsEnabled();
    }
    //-------------------------------------------------
    void WebBrowser::Print()
    {
        GetWebViewCtrl()->Print();
    }
    //-------------------------------------------------
    void WebBrowser::SetUserAgent(const string& value) 
    { 
        GetWebViewCtrl()->SetUserAgent(wxStr(value));
    }
    //-------------------------------------------------
    string WebBrowser::GetPageSource() 
    { 
        return wxStr(GetWebViewCtrl()->GetPageSource());
    }
    //-------------------------------------------------
    string WebBrowser::GetPageText() 
    { 
        return wxStr(GetWebViewCtrl()->GetPageText());
    }
    //-------------------------------------------------
    string WebBrowser::GetUserAgent() 
    { 
        return wxStr(GetWebViewCtrl()->GetUserAgent());
    }
    //-------------------------------------------------
    void WebBrowser::SetContextMenuEnabled(bool enable)
    {
        GetWebViewCtrl()->EnableContextMenu(enable);
    }
    //-------------------------------------------------
    void WebBrowser::SetAccessToDevToolsEnabled(bool enable)
    {
        GetWebViewCtrl()->EnableAccessToDevTools(enable);
    }
    //-------------------------------------------------
    void WebBrowser::RemoveAllUserScripts()
    {
        GetWebViewCtrl()->RemoveAllUserScripts();
    }
    //-------------------------------------------------
    //https://docs.wxwidgets.org/3.2/classwx_web_view.html#a2597c3371ed654bf03262ec6d34a0126
    //The Edge backend only supports a single message handler and the 
    //IE backend does not support script message handlers.
    bool WebBrowser::AddScriptMessageHandler(const string& name)
    {
        return GetWebViewCtrl()->AddScriptMessageHandler(wxStr(name));
    }
    //-------------------------------------------------
    bool WebBrowser::RemoveScriptMessageHandler(const string& name)
    { 
        return GetWebViewCtrl()->RemoveScriptMessageHandler(wxStr(name));
    }
    //-------------------------------------------------
    wxWebView* WebBrowser::GetWebViewCtrl()
    {
        return dynamic_cast<wxWebView*>(GetWxWindow());
    }
    //-------------------------------------------------
}
//-------------------------------------------------
