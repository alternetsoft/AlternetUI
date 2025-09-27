#include <wx/version.h>
#include <wx/webviewarchivehandler.h>
#include <wx/filesys.h>
#include <wx/webviewfshandler.h>
#include <wx/fs_mem.h>

#if defined(__WXOSX__)
#include <wx/osx/webview_webkit.h>
#endif

#if defined(__WXGTK__)

#endif

#if defined(__WXMSW__)
#include <wx/msw/webview_ie.h>
#endif

#if defined(__WXMSW__)
#include <wx/private/jsscriptwrapper.h>
#include <mshtml.h>
#include <wx/msw/private/comptr.h>
#include <wx/msw/ole/automtn.h>
#endif

#include <wx/webviewarchivehandler.h>
#include <wx/webviewfshandler.h>
#include <wx/filesys.h>
#include <wx/fs_arc.h>
#include <wx/fs_mem.h>
#include <wx/log.h>

#if defined(__WXMSW__)
#ifdef wxUSE_WEBVIEW_EDGE
#define _MSW_EGDE_
#include <wx/msw/webview_edge.h>
#include "../../External/WxWidgets/3rdparty/webview2/build/native/include/WebView2EnvironmentOptions.h"
#include "../../External/WxWidgets/3rdparty/webview2/build/native/include/WebView2.h"
#endif
#endif

#include "WebBrowser.h"

namespace Alternet::UI
{

    WebBrowserBackend WebBrowser::DefaultBackend = WebBrowserBackend::Default;
    wxString WebBrowser::DefaultPage = "about:blank";
    bool WebBrowser::IELatest = false;

    wxString WebBrowser::DefaultUserAgent = wxEmptyString;
    wxString WebBrowser::DefaultScriptMesageName = wxEmptyString;
    wxString WebBrowser::DefaultFSNameMemory = wxEmptyString;
    wxString WebBrowser::DefaultFSNameArchive = wxEmptyString;
    wxString WebBrowser::DefaultFSNameCustom = wxEmptyString;

#if defined(__WXMSW__)
#ifdef wxUSE_WEBVIEW_EDGE
    ICoreWebView2_2* GetWebView2(void* native)
    {
        ICoreWebView2_2* wb = static_cast<ICoreWebView2_2*>(native);
        return wb;
    }

    ICoreWebView2Environment* GetWebViewEnvironment(ICoreWebView2_2* wb)
    {
        if (wb == nullptr)
            return nullptr;

        wxCOMPtr<ICoreWebView2Environment> baseWebView;
        auto hr = wb->get_Environment(&baseWebView);

        if (FAILED(hr))
        {
            wxLogApiError("WebBrowser::GetWebViewEnvironment", hr);
            return nullptr;
        }

        return baseWebView;
    }
#endif
#endif

    void Nop() {};

    bool _isEdgeBackendEnabled = false;

    bool WebBrowser::GetIsEdgeBackendEnabled()
    {
        return _isEdgeBackendEnabled;
    }

    void WebBrowser::SetIsEdgeBackendEnabled(bool value)
    {
        _isEdgeBackendEnabled = value;
    }

    void WebBrowser::OnSizeChanged(wxSizeEvent& event)
    {
        Control::OnSizeChanged(event);

#if defined(__WXMSW__)
#ifdef wxUSE_WEBVIEW_EDGE
/*
        if (!GetIsEdge())
            return;

        auto wb = GetWebView2(GetNativeBackend());
        auto environment = GetWebViewEnvironment(wb);

        if (environment == nullptr)
            return;
*/
#endif
#endif
    }

    void WebBrowser::SetDefaultUserAgent(const string& value) 
    {
        WebBrowser::DefaultUserAgent = wxStr(value);
    }

    void WebBrowser::SetDefaultScriptMesageName(const string& value) 
    {
        WebBrowser::DefaultScriptMesageName = wxStr(value);
    }
    
    void WebBrowser::SetDefaultFSNameMemory(const string& value) 
    {
        WebBrowser::DefaultFSNameMemory = wxStr(value);
    }
    
    void WebBrowser::SetDefaultFSNameArchive(const string& value) 
    {
        WebBrowser::DefaultFSNameArchive = wxStr(value);
    }

    void WebBrowser::SetBackend(WebBrowserBackend value)
    {
        WebBrowser::DefaultBackend = value;
    }
    
    bool WebBrowser::IsBackendWebKitAvailable()
    {
        return wxWebView::IsBackendAvailable(wxWebViewBackendWebKit);
    }    
    
    bool WebBrowser::IsBackendIEAvailable()
    {
        return wxWebView::IsBackendAvailable(wxWebViewBackendIE);
    }
    
    bool WebBrowser::IsBackendEdgeAvailable()
    {
        return _isEdgeBackendEnabled && wxWebView::IsBackendAvailable(wxWebViewBackendEdge);
    }
    
    bool WebBrowser::IsBackendAvailable(const string& value)
    {
        return wxWebView::IsBackendAvailable(wxStr(value));
    }
    
    void WebBrowser::Stop() 
    {
        GetWebViewCtrl()->Stop();
    }
    
    bool WebBrowser::GetCanGoBack()
    {
        return GetWebViewCtrl()->CanGoBack();
    }
    
    bool WebBrowser::GetCanGoForward()
    {
        return GetWebViewCtrl()->CanGoForward();
    }
    
    void WebBrowser::GoBack()
    {
        GetWebViewCtrl()->GoBack();
    }
    
    void WebBrowser::GoForward()
    {
        GetWebViewCtrl()->GoForward();
    }
    
    void WebBrowser::ClearHistory()
    {
        GetWebViewCtrl()->ClearHistory();
    }
    
    void WebBrowser::EnableHistory(bool enable)
    {
        GetWebViewCtrl()->EnableHistory(enable);
    }
    
    string WebBrowser::GetLibraryVersionString() 
    {
        wxVersionInfo version = wxGetLibraryVersionInfo();
        return wxStr(version.GetVersionString());
    }
    
    wxString WebBrowser::WebViewBackendNameFromId(WebBrowserBackend id)
    {
        auto backend = wxASCII_STR(wxWebViewBackendDefault);

        switch (id)
        {
        case WebBrowserBackend::Default:
            break;
        case WebBrowserBackend::IE:
            backend = wxASCII_STR(wxWebViewBackendIE);
            break;
        case WebBrowserBackend::IELatest:
            backend = wxASCII_STR(wxWebViewBackendIE);
            if (!WebBrowser::IELatest)
            {
#if defined(__WXMSW__)
                wxWebViewIE::MSWSetEmulationLevel();
#endif
                WebBrowser::IELatest = true;
            }
            break;
        case WebBrowserBackend::Edge:
            backend = wxASCII_STR(wxWebViewBackendEdge);
            break;
        case WebBrowserBackend::WebKit:
            backend = wxASCII_STR(wxWebViewBackendWebKit);
            break;
        }
        return backend;
    }
    
    WebBrowserBackend WebBrowser::GetBackend()
    {
        return Backend;
    }
    
    void WebBrowser::SetEdgePath(const string& path)
    {
#if defined(__WXMSW__)
#ifdef wxUSE_WEBVIEW_EDGE
        wxWebViewEdge::MSWSetBrowserExecutableDir(wxStr(path));
#endif
#endif
    }
    
    void WebBrowser::SetDefaultPage(const string& value)
    {
        WebBrowser::DefaultPage = wxStr(value);
    }
    
    void* WebBrowser::GetNativeBackend() 
    {
        return GetWebViewCtrl()->GetNativeBackend();
    }

    void WebBrowser::ProcessDefaultsOnCreate(bool before)
    {
        auto realBackend = Backend; 
        
        auto isMacOS = WebBrowser::GetBackendOS() == (int)WebBrowserBackendOS::MacOS;
        auto isIE = IsBackendIE();
        auto isEdge = WebBrowser::GetBackend() == WebBrowserBackend::Edge;
        auto isWebkit = WebBrowser::GetBackend() == WebBrowserBackend::WebKit;

        auto registerHandlerBeforeCreate = isMacOS && isWebkit;
        auto registerHandlerSupported = !isEdge;
        
        // DefaultUserAgent
        if (!isIE && !DefaultUserAgentDone && 
            WebBrowser::DefaultUserAgent != wxEmptyString)
        {
            if ((isEdge && before) || !isEdge) 
                SetUserAgent(wxStr(WebBrowser::DefaultUserAgent));
            DefaultUserAgentDone = true;
        }

        // DefaultScriptMesageName
        if (!isIE && !DefaultScriptMesageNameDone && 
            WebBrowser::DefaultScriptMesageName != wxEmptyString)
        {
            AddScriptMessageHandler(wxStr(DefaultScriptMesageName));
            DefaultScriptMesageNameDone = true;
        }

        // here we register handlers
        if (!registerHandlerSupported)
            return;
        if (registerHandlerBeforeCreate && !before)
            return;
        if (!registerHandlerBeforeCreate && before)
            return;
        if (!DefaultFSNameMemoryDone && 
            WebBrowser::DefaultFSNameMemory != wxEmptyString)
        {
            RegisterHandlerMemory(wxStr(WebBrowser::DefaultFSNameMemory));
            DefaultFSNameMemoryDone = true;
        }
        if (!DefaultFSNameArchiveDone && 
            WebBrowser::DefaultFSNameArchive != wxEmptyString)
        {
            RegisterHandlerZip(wxStr(WebBrowser::DefaultFSNameArchive));
            DefaultFSNameArchiveDone = true;
        }
        if (!DefaultFSNameCustomDone && 
            WebBrowser::DefaultFSNameCustom != wxEmptyString)
        {
            DefaultFSNameCustomDone = true;
        }
    }

    void WebBrowser::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();

        try
        {
            ProcessDefaultsOnCreate(true);
        }
        catch (const std::exception&)
        {
        }
       
        CreateBackend();

        try
        {
            ProcessDefaultsOnCreate(false);
        }
        catch (const std::exception&)
        {
        }
    }

    bool WebBrowser::GetHasBorder()
    {
        return hasBorder;
    }

    void WebBrowser::SetHasBorder(bool value)
    {
        if (hasBorder == value)
            return;
        hasBorder = value;
        RecreateWxWindowIfNeeded();
    }

    void* WebBrowser::CreateWebBrowser(const string& url)
    {
        return new WebBrowser(url);
    }

    WebBrowser::WebBrowser()
    {
        _defaultPage = WebBrowser::DefaultPage;
        bindScrollEvents = false;
    }

    WebBrowser::WebBrowser(string url)
    {
        _defaultPage = wxStr(url);
        bindScrollEvents = false;
    }

    void WebBrowser::CreateBackend()
    {
        RaiseSimpleEvent(WebBrowserEvent::BeforeBrowserCreate);

        long style = wxFULL_REPAINT_ON_RESIZE;

        if (!hasBorder)
            style = style | wxBORDER_NONE;

        webView->Create(
            webViewParent,
            wxID_ANY,
            _defaultPage,
            wxDefaultPosition,
            wxDefaultSize,
            style);

#if defined(__WXMSW__)
#ifdef wxUSE_WEBVIEW_EDGE
        if (GetIsEdge())
        {
        }
#endif
#endif


    }

    wxWindow* WebBrowser::CreateWxWindowUnparented()
    {
        return nullptr;
    }

#if defined(__WXMSW__)
#ifdef wxUSE_WEBVIEW_EDGE
    class wxWebViewEdge2 : public wxWebViewEdge, public wxWidgetExtender
    {
    public:
        wxWebViewEdge2() {}
    };
#endif
#endif

    bool WebBrowser::GetIsEdge()
    {
#if defined(__WXMSW__)
#ifdef wxUSE_WEBVIEW_EDGE
        return dynamic_cast<wxWebViewEdge2*>(GetWxWindow()) != nullptr;
#endif
#endif
        return false;
    }

    wxWindow* WebBrowser::CreateWxWindowCore(wxWindow* parent)
    {
        Backend = WebBrowser::DefaultBackend;

        if (Backend == WebBrowserBackend::Default)
        {
            if (WebBrowser::IsBackendEdgeAvailable())
                Backend = WebBrowserBackend::Edge;
            else
            if (WebBrowser::IsBackendWebKitAvailable())
                Backend = WebBrowserBackend::WebKit;
            else
            if (WebBrowser::IsBackendIEAvailable())
                Backend = WebBrowserBackend::IELatest;
        }

        webView = nullptr;

#if defined(__WXMSW__)
#ifdef wxUSE_WEBVIEW_EDGE
        if (!_isEdgeBackendEnabled)
        {
            Backend = WebBrowserBackend::IELatest;
        }
        else
        if (Backend == WebBrowserBackend::Edge)
        {
            webView = new wxWebViewEdge2();
        }
#endif
#endif

        if (webView == nullptr)
        {
            auto backendName = WebViewBackendNameFromId(Backend);

            webView = wxWebView::New(backendName);
        }

        webViewParent = parent;

        webView->SetBackgroundStyle(wxBackgroundStyle::wxBG_STYLE_COLOUR);
        webView->SetBackgroundColour(*wxWHITE);
        
        webView->Bind(wxEVT_WEBVIEW_NAVIGATING, &WebBrowser::OnNavigating, this);
        webView->Bind(wxEVT_WEBVIEW_NAVIGATED, &WebBrowser::OnNavigated, this);
        webView->Bind(wxEVT_WEBVIEW_LOADED, &WebBrowser::OnLoaded, this);
        webView->Bind(wxEVT_WEBVIEW_ERROR, &WebBrowser::OnError, this);
        webView->Bind(wxEVT_WEBVIEW_NEWWINDOW, &WebBrowser::OnNewWindow, this);
        webView->Bind(wxEVT_WEBVIEW_TITLE_CHANGED, &WebBrowser::OnTitleChanged, this);
        webView->Bind(wxEVT_WEBVIEW_FULLSCREEN_CHANGED, &WebBrowser::OnFullScreenChanged, this);
        webView->Bind(wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED,
            &WebBrowser::OnScriptMessageReceived, this);
        webView->Bind(wxEVT_WEBVIEW_SCRIPT_RESULT, &WebBrowser::OnScriptResult, this);
        
        return webView;
    }
    
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
                window->Unbind(wxEVT_WEBVIEW_FULLSCREEN_CHANGED,
                    &WebBrowser::OnFullScreenChanged, this);
                window->Unbind(wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED, 
                    &WebBrowser::OnScriptMessageReceived, this);
                window->Unbind(wxEVT_WEBVIEW_SCRIPT_RESULT, &WebBrowser::OnScriptResult, this);
            }
        }
    }
    
    #define scast(v) (const_cast<char16_t*>(v.c_str()))
    void WebBrowser::RaiseEventEx(WebBrowserEvent eventId, wxWebViewEvent& event, 
        bool canVeto)
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

        void* clientData = event.GetClientData();
        data.ClientData = clientData;

        if (canVeto) 
        {
            auto result = RaiseEventWithPointerResult(eventId, &data);

            if (result != 0)
                event.Veto();
            else
                event.Skip();
        }
        else
        {
            event.Skip();
            RaiseEvent(eventId, &data);
        }
    }
    
    void WebBrowser::RaiseSimpleEvent(WebBrowserEvent eventId, bool canVeto)
    {
        WebBrowserEventData data = { 0 };
        if (eventCallback != nullptr)
            eventCallback(this, eventId, &data);
    }
    
    void WebBrowser::OnFullScreenChanged(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::FullScreenChanged, event);
    }
    
    void WebBrowser::OnScriptMessageReceived(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::ScriptMessageReceived, event);
    }
    
    void WebBrowser::OnScriptResult(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::ScriptResult, event);
    }
    
    void WebBrowser::OnBeforeBrowserCreate(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::BeforeBrowserCreate, event);
    }
    
    void WebBrowser::OnNavigating(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::Navigating, event,true);
    }
    
    void WebBrowser::OnNavigated(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::Navigated,event);
    }
    
    void WebBrowser::OnLoaded(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::Loaded,event);
    }
    
    void WebBrowser::OnError(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::Error,event);
    }
    
    void WebBrowser::OnNewWindow(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::NewWindow,event);
    }
    
    void WebBrowser::OnTitleChanged(wxWebViewEvent& event)
    {
        RaiseEventEx(WebBrowserEvent::TitleChanged,event);
    }
    
    static bool FSAddedZip = false;
    void WebBrowser::RegisterHandlerZip(const string& schemeName)
    {
        if (!FSAddedZip) 
        {
            wxFileSystem::AddHandler(new wxArchiveFSHandler);
            FSAddedZip = true;
        }
        webView->RegisterHandler(
            wxSharedPtr<wxWebViewHandler>(new wxWebViewArchiveHandler(wxStr(schemeName))));
    }
    
    bool WebBrowser::IsBackendIE()
    {
        if (Backend == WebBrowserBackend::IE || Backend == WebBrowserBackend::IELatest)
            return TRUE;
        return FALSE;
    }

    static bool FSAddedMemory = false;
    void WebBrowser::RegisterHandlerMemory(const string& schemeName)
    {
        if (!FSAddedMemory) 
        {
            wxFileSystem::AddHandler(new wxMemoryFSHandler);
            FSAddedMemory = true;
        }

        wxString s = wxStr(schemeName);

        webView->RegisterHandler(
            wxSharedPtr<wxWebViewHandler>(new wxWebViewFSHandler(s)));
    }
    
#if defined(__WXOSX__)
    
    int WebBrowser::GetBackendOS()
    {
        return (int)WebBrowserBackendOS::MacOS;
    }
    
#endif

#if defined(__WXGTK__)
    
    int WebBrowser::GetBackendOS()
    {
        return (int)WebBrowserBackendOS::Unix;
    }
    
#endif

    
    //https://docs.wxwidgets.org/3.2/classwx_web_view.html#a67000a368c45f3684efd460d463ffb98
    string WebBrowser::RunScript(const string& javascript)
    {
        wxString output;
        wxString script = wxStr(javascript);

#if defined(__WXMSW__)
        bool result;
        
        if(IsBackendIE())
            result = IERunScript(script, &output);
        else
            result = GetWebViewCtrl()->RunScript(script, &output);
#else
        bool result = GetWebViewCtrl()->RunScript(script, &output);
#endif

        auto errorCode = wxSysErrorCode();
        auto errorMsg = wxSysErrorMsg(errorCode);

        if (result)
            return wxStr(output);
        wxString s1 = "3140D0CF550442968B792551E6DCBEC1";
        wxString s2 = s1 + output;
        return wxStr(s2);
    }

// This section is for MSW related code
#if defined(__WXMSW__)
    
    static wxCOMPtr<IHTMLDocument2> IEGetDocument(void* native)
    {
        IWebBrowser2* wb = static_cast<IWebBrowser2*>(native);

        wxCOMPtr<IDispatch> dispatch;
        wxCOMPtr<IHTMLDocument2> document;
        HRESULT result = wb->get_Document(&dispatch);
        if (dispatch && SUCCEEDED(result))
        {
            //document is set to null automatically if the interface isn't supported
            dispatch->QueryInterface(IID_IHTMLDocument2, (void**)&document);
        }
        return document;
    }
    
    static
        bool CallEval(const wxString& code,
            wxAutomationObject& scriptAO,
            wxVariant* varResult)
    {
        wxVariant varCode(code);
        return scriptAO.Invoke("eval", DISPATCH_METHOD, *varResult, 1, &varCode);
    }

    bool WebBrowser::IERunScript(const wxString& javascript, wxString* output)
    {
        void* native = GetNativeBackend();
        IWebBrowser2* wb = static_cast<IWebBrowser2*>(native);

        wxCOMPtr<IHTMLDocument2> document(IEGetDocument(native));
        if (!document)
        {
            if (output)
                *output = _("Can't run JavaScript script without a valid HTML document");
            return false;
        }

        IDispatch* scriptDispatch = NULL;
        if (FAILED(document->get_Script(&scriptDispatch)))
        {
            if (output)
                *output = _("Can't get the JavaScript object");
            return false;
        }

        wxJSScriptWrapper wrapJS(javascript, wxJSScriptWrapper::JS_OUTPUT_IE);

        wxAutomationObject scriptAO(scriptDispatch);
        wxVariant varResult;

        bool success = false;
        wxString scriptOutput;
        if (CallEval(wrapJS.GetWrappedCode(), scriptAO, &varResult))
            success = wxJSScriptWrapper::ExtractOutput(varResult.MakeString(), &scriptOutput);
        else
            scriptOutput = _("failed to evaluate");

        if (output)
            *output = scriptOutput;

        return success;
    }
    
    int WebBrowser::GetBackendOS()
    {
        return (int)WebBrowserBackendOS::Windows;
    }
    
    int WebBrowser::IEGetScriptErrorsSuppressed()
    {
        if (!IsBackendIE())
            return 0;
        void* native = GetNativeBackend();
        IWebBrowser2* wb = static_cast<IWebBrowser2*>(native);
        VARIANT_BOOL pbSilent;
        wb->get_Silent(&pbSilent);
        return pbSilent;
    }
    
    void WebBrowser::IESetScriptErrorsSuppressed(bool value)
    {
        if (!IsBackendIE())
            return;
        void* native = GetNativeBackend();
        IWebBrowser2* wb = static_cast<IWebBrowser2*>(native);

        int vBefore = IEGetScriptErrorsSuppressed();

        VARIANT_BOOL pbSilent;

        // 0 == FALSE, -1 == TRUE 
        if (value)
            pbSilent = -1;
        else
            pbSilent = 0;
        wb->put_Silent(pbSilent);
        int vAfter = IEGetScriptErrorsSuppressed();
        return;
    }
    
    void WebBrowser::IEShowPrintPreviewDialog()
    {
        if (!IsBackendIE())
            return;
        void* native = GetNativeBackend();
        IWebBrowser2* wb = static_cast<IWebBrowser2*>(native);
        wb->ExecWB(OLECMDID_PRINTPREVIEW,
            OLECMDEXECOPT_PROMPTUSER, NULL, NULL);
    }
#endif
    
    string WebBrowser::GetCurrentTitle()
    {
        return wxStr(GetWebViewCtrl()->GetCurrentTitle());
    }
    
    string WebBrowser::GetCurrentURL()
    {
        return wxStr(GetWebViewCtrl()->GetCurrentURL());
    }
    
    bool WebBrowser::GetEditable()
    {
        return GetWebViewCtrl()->IsEditable();
    }
    
    void WebBrowser::SetEditable(bool value)
    {
        GetWebViewCtrl()->SetEditable(value);
    }
    
    bool WebBrowser::CanSetZoomType(wxWebViewZoomType type) 
    {
        return GetWebViewCtrl()->CanSetZoomType(type);
    }
    
    float WebBrowser::GetZoomFactor()
    {
        return GetWebViewCtrl()->GetZoomFactor();
    }
    
    wxWebViewZoomType WebBrowser::GetZoomType()
    {
        return GetWebViewCtrl()->GetZoomType();
    }
    
    void WebBrowser::RunScriptAsync(const string& javascript, void* clientData)
    {
        GetWebViewCtrl()->RunScriptAsync(wxStr(javascript), clientData);
    }
    
    void WebBrowser::SetZoomFactor(float zoom)
    {
        GetWebViewCtrl()->SetZoomFactor(zoom);
    }
    
    void WebBrowser::SetZoomType(wxWebViewZoomType zoomType)
    {
        GetWebViewCtrl()->SetZoomType(zoomType);
    }
    
    int WebBrowser::GetZoom()
    {
        return GetWebViewCtrl()->GetZoom();
    }
    
    void WebBrowser::SetZoom(int value)
    {
        GetWebViewCtrl()->SetZoom((wxWebViewZoom)value);
    }
    
    void WebBrowser::LoadURL(const string& url)
    {
        GetWebViewCtrl()->LoadURL(wxStr(url));
    }
    
    void WebBrowser::ReloadDefault()
    {
        GetWebViewCtrl()->Reload();
    }
    
    void WebBrowser::Reload(bool noCache)
    {
        if(noCache)
            GetWebViewCtrl()->Reload();
        else
            GetWebViewCtrl()->Reload(wxWEBVIEW_RELOAD_NO_CACHE);
    }
    
    void WebBrowser::SetPage(const string& html, const string& baseUrl)
    {
        GetWebViewCtrl()->SetPage(wxStr(html), wxStr(baseUrl));
    }
    
    string WebBrowser::GetBackendVersionString(WebBrowserBackend id)
    {
        auto backend = WebViewBackendNameFromId(id);
        wxVersionInfo info = wxWebView::GetBackendVersionInfo(backend);
        auto s = info.GetVersionString();
        return wxStr(s);
    }
    
    void WebBrowser::SelectAll()
    {
        GetWebViewCtrl()->SelectAll();
    }
    
    bool WebBrowser::GetHasSelection() 
    { 
        return GetWebViewCtrl()->HasSelection();
    }
    
    void WebBrowser::DeleteSelection()
    {
        GetWebViewCtrl()->DeleteSelection();
    }
    
    string WebBrowser::GetSelectedText() 
    { 
        return wxStr(GetWebViewCtrl()->GetSelectedText());
    }
    
    string WebBrowser::GetSelectedSource() 
    { 
        return wxStr(GetWebViewCtrl()->GetSelectedSource());
    }
    
    void WebBrowser::ClearSelection()
    {
        GetWebViewCtrl()->ClearSelection();
    }
    
    bool WebBrowser::GetCanCut() 
    { 
        return GetWebViewCtrl()->CanCut();
    }
    
    bool WebBrowser::GetCanCopy()
    { 
        return GetWebViewCtrl()->CanCopy();
    }
    
    bool WebBrowser::GetCanPaste()
    { 
        return GetWebViewCtrl()->CanPaste();
    }
    
    void WebBrowser::Cut() 
    {
        GetWebViewCtrl()->Cut();
    }
    
    void WebBrowser::Copy() 
    {
        GetWebViewCtrl()->Copy();
    }
    
    void WebBrowser::Paste() 
    {
        GetWebViewCtrl()->Paste();
    }
    
    bool WebBrowser::GetCanUndo() 
    { 
        return GetWebViewCtrl()->CanUndo();
    }
    
    bool WebBrowser::GetCanRedo() 
    { 
        return GetWebViewCtrl()->CanRedo();
    }
    
    void WebBrowser::Undo() 
    {
        GetWebViewCtrl()->Undo();
    }
    
    void WebBrowser::Redo() 
    {
        GetWebViewCtrl()->Redo();
    }
    
    bool WebBrowser::AddUserScript(const string& javascript,int injectionTime) 
    {
        wxString script = wxStr(javascript);
        bool result = GetWebViewCtrl()->AddUserScript(script, 
            (wxWebViewUserScriptInjectionTime)injectionTime);
        return result;
    }
    
    int WebBrowser::Find(const string& text, int flags) 
    {
        auto result = GetWebViewCtrl()->Find(wxStr(text), flags);
        if (result == wxNOT_FOUND)
            return -1;
        return result;
    }
    
    bool WebBrowser::GetIsBusy() 
    { 
        return GetWebViewCtrl()->IsBusy();
    }
    
    bool WebBrowser::GetContextMenuEnabled() 
    { 
        return GetWebViewCtrl()->IsContextMenuEnabled();
    }
    
    bool WebBrowser::GetAccessToDevToolsEnabled() 
    { 
        return GetWebViewCtrl()->IsAccessToDevToolsEnabled();
    }
    
    void WebBrowser::Print()
    {
        GetWebViewCtrl()->Print();
    }
    
    void WebBrowser::SetUserAgent(const string& value) 
    { 
        webView->SetUserAgent(wxStr(value));
    }
    
    string WebBrowser::GetPageSource() 
    { 
        return wxStr(GetWebViewCtrl()->GetPageSource());
    }
    
    string WebBrowser::GetPageText() 
    { 
        return wxStr(GetWebViewCtrl()->GetPageText());
    }

    int WebBrowser::GetPreferredColorScheme()
    {
        return preferredColorScheme;
    }

#if defined(_MSW_EGDE_)
    static ICoreWebView2_3* GetWebView2_3(void* native)
    {
        ICoreWebView2* wb = static_cast<ICoreWebView2*>(native);
        if (wb == NULL)
        {
            wxLogApiError("WebBrowser::GetWebView2_3 1", 0);
            return NULL;
        }
        wxCOMPtr<ICoreWebView2_3> WebView3;
        auto hr = wb->QueryInterface(IID_PPV_ARGS(&WebView3));
        if (FAILED(hr))
        {
            wxLogApiError("WebBrowser::GetWebView2_3 2", hr);
            return NULL;
        }
        return WebView3;
    }

    static ICoreWebView2Profile* GetProfile(void* native) 
    {
        ICoreWebView2* wb = static_cast<ICoreWebView2*>(native);
        if (wb == NULL)
        {
            wxLogApiError("WebBrowser::EdgeGetProfile 1", 0);
            return NULL;
        }
        wxCOMPtr<ICoreWebView2_13> WebView13;
        auto hr = wb->QueryInterface(IID_PPV_ARGS(&WebView13));
        if (FAILED(hr))
        {
            wxLogApiError("WebBrowser::EdgeGetProfile 2", hr);
            return NULL;
        }
        ICoreWebView2Profile* profile;
        hr = WebView13->get_Profile(&profile);
        if (FAILED(hr))
        {
            wxLogApiError("WebBrowser::EdgeGetProfile 3", hr);
            return NULL;
        }
        return profile;
    }
#endif

    void WebBrowser::SetVirtualHostNameToFolderMapping(const string& hostName,
        const string& folderPath, int accessKind) 
    {
#if defined(_MSW_EGDE_)
        if (Backend != WebBrowserBackend::Edge)
            return;
        wxCOMPtr<ICoreWebView2_3> webView3(GetWebView2_3(GetNativeBackend()));
        if (webView3 != NULL)
            webView3->SetVirtualHostNameToFolderMapping(wxStr(hostName),
                wxStr(folderPath), (COREWEBVIEW2_HOST_RESOURCE_ACCESS_KIND)accessKind);
#endif
    }

    void WebBrowser::SetPreferredColorScheme(int value)
    {
        preferredColorScheme = value;
#if defined(_MSW_EGDE_)
        if (Backend != WebBrowserBackend::Edge)
            return;
        wxCOMPtr<ICoreWebView2Profile> profile(GetProfile(GetNativeBackend()));
        if(profile != NULL)
            profile->put_PreferredColorScheme((COREWEBVIEW2_PREFERRED_COLOR_SCHEME)value);
#endif
    }

    string WebBrowser::GetUserAgent() 
    { 
        return wxStr(GetWebViewCtrl()->GetUserAgent());
    }
    
    void WebBrowser::SetContextMenuEnabled(bool enable)
    {
        GetWebViewCtrl()->EnableContextMenu(enable);
    }
    
    void WebBrowser::SetAccessToDevToolsEnabled(bool enable)
    {
        GetWebViewCtrl()->EnableAccessToDevTools(enable);
    }
    
    void WebBrowser::RemoveAllUserScripts()
    {
        GetWebViewCtrl()->RemoveAllUserScripts();
    }
    
    bool WebBrowser::AddScriptMessageHandler(const string& name)
    {
        return GetWebViewCtrl()->AddScriptMessageHandler(wxStr(name));
    }
    
    bool WebBrowser::RemoveScriptMessageHandler(const string& name)
    { 
        return GetWebViewCtrl()->RemoveScriptMessageHandler(wxStr(name));
    }
    
    wxWebView* WebBrowser::GetWebViewCtrl()
    {
        return dynamic_cast<wxWebView*>(GetWxWindow());
    }

    wxString GetUsefulDefines() 
    {
        wxString result = "Defines: ";
#ifdef wxUSE_UNSAFE_WXSTRING_CONV
        result += " wxUSE_UNSAFE_WXSTRING_CONV=1";
#else
        result += " wxUSE_UNSAFE_WXSTRING_CONV=0";
#endif

#ifdef wxUSE_ZIPSTREAM
        result += " wxUSE_ZIPSTREAM=1";
#else
        result += " wxUSE_ZIPSTREAM=0";
#endif

#ifdef wxUSE_DPI_AWARE_MANIFEST
        result += " wxUSE_DPI_AWARE_MANIFEST=1";
#else
        result += " wxUSE_DPI_AWARE_MANIFEST=0";
#endif

#ifdef wxUSE_NO_MANIFEST
        result += " wxUSE_NO_MANIFEST=1";
#else
        result += " wxUSE_NO_MANIFEST=0";
#endif

#ifdef wxUSE_RC_MANIFEST
        result += " wxUSE_RC_MANIFEST=1";
#else
        result += " wxUSE_RC_MANIFEST=0";
#endif

#ifdef wxUSE_VC_CRTDBG
        result += " wxUSE_VC_CRTDBG=1";
#else
        result += " wxUSE_VC_CRTDBG=0";
#endif

#ifdef wxUSE_UXTHEME
        result += " wxUSE_UXTHEME=1";
#else
        result += " wxUSE_UXTHEME=0";
#endif

#ifdef wxUSE_NANOSVG_EXTERNAL_ENABLE_IMPL
        result += " wxUSE_NANOSVG_EXTERNAL_ENABLE_IMPL=1";
#else
        result += " wxUSE_NANOSVG_EXTERNAL_ENABLE_IMPL=0";
#endif

#ifdef wxUSE_NANOSVG_EXTERNAL
        result += " wxUSE_NANOSVG_EXTERNAL=1";
#else
        result += " wxUSE_NANOSVG_EXTERNAL=0";
#endif

#ifdef wxUSE_FS_ZIP
        result += " wxUSE_FS_ZIP=1";
#else
        result += " wxUSE_FS_ZIP=0";
#endif

#ifdef wxUSE_FS_INET
        result += " wxUSE_FS_INET=1";
#else
        result += " wxUSE_FS_INET=0";
#endif

#ifdef wxUSE_FS_ARCHIVE
        result += " wxUSE_FS_ARCHIVE=1";
#else
        result += " wxUSE_FS_ARCHIVE=0";
#endif

#ifdef wxUSE_SVG
        result += " wxUSE_SVG=1";
#else
        result += " wxUSE_SVG=0";
#endif

#ifdef wxUSE_NANOSVG
        result += " wxUSE_NANOSVG=1";
#else
        result += " wxUSE_NANOSVG=0";
#endif

#ifdef wxUSE_MEMORY_TRACING
        result += " wxUSE_MEMORY_TRACING=1";
#else
        result += " wxUSE_MEMORY_TRACING=0";
#endif

#ifdef wxUSE_FILESYSTEM
        result += " wxUSE_FILESYSTEM=1";
#else
        result += " wxUSE_FILESYSTEM=0";
#endif

#ifdef wxUSE_EXCEPTIONS
        result += " wxUSE_EXCEPTIONS=1";
#else
        result += " wxUSE_EXCEPTIONS=0";
#endif

#ifdef wxUSE_STD_STRING
        result += " wxUSE_STD_STRING=1";
#else
        result += " wxUSE_STD_STRING=0";
#endif

#ifdef wxUSE_STL
        result += " wxUSE_STL=1";
#else
        result += " wxUSE_STL=0";
#endif

#ifdef wxUSE_ARCHIVE_STREAMS
        result += " wxUSE_ARCHIVE_STREAMS=1";
#else
        result += " wxUSE_ARCHIVE_STREAMS=0";
#endif

#ifdef wxNO_UNSAFE_WXSTRING_CONV
        result += " wxNO_UNSAFE_WXSTRING_CONV=1";
#else
        result += " wxNO_UNSAFE_WXSTRING_CONV=0";
#endif

        return result;
    }

    // https://stackoverflow.com/questions/152016/detecting-cpu-architecture-compile-time
    extern "C" {
        const char* GetCPUArchitecture() {
#if defined(__x86_64__) || defined(_M_X64)
            return "X64";
#elif defined(i386) || defined(__i386__) || defined(__i386) || defined(_M_IX86)
            return "X86";
#elif defined(__ARM_ARCH_2__)
            return "ARM2";
#elif defined(__ARM_ARCH_3__) || defined(__ARM_ARCH_3M__)
            return "ARM3";
#elif defined(__ARM_ARCH_4T__) || defined(__TARGET_ARM_4T)
            return "ARM4T";
#elif defined(__ARM_ARCH_5_) || defined(__ARM_ARCH_5E_)
            return "ARM5"
#elif defined(__ARM_ARCH_6T2_) || defined(__ARM_ARCH_6T2_)
            return "ARM6T2";
#elif defined(__ARM_ARCH_6__) || defined(__ARM_ARCH_6J__) || defined(__ARM_ARCH_6K__) || defined(__ARM_ARCH_6Z__) || defined(__ARM_ARCH_6ZK__)
            return "ARM6";
#elif defined(__ARM_ARCH_7__) || defined(__ARM_ARCH_7A__) || defined(__ARM_ARCH_7R__) || defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7S__)
            return "ARM7";
#elif defined(__ARM_ARCH_7A__) || defined(__ARM_ARCH_7R__) || defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7S__)
            return "ARM7A";
#elif defined(__ARM_ARCH_7R__) || defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7S__)
            return "ARM7R";
#elif defined(__ARM_ARCH_7M__)
            return "ARM7M";
#elif defined(__ARM_ARCH_7S__)
            return "ARM7S";
#elif defined(__aarch64__) || defined(_M_ARM64)
            return "ARM64";
#elif defined(mips) || defined(__mips__) || defined(__mips)
            return "MIPS";
#elif defined(__sh__)
            return "SUPERH";
#elif defined(__powerpc) || defined(__powerpc__) || defined(__powerpc64__) || defined(__POWERPC__) || defined(__ppc__) || defined(__PPC__) || defined(_ARCH_PPC)
            return "POWERPC";
#elif defined(__PPC64__) || defined(__ppc64__) || defined(_ARCH_PPC64)
            return "POWERPC64";
#elif defined(__sparc__) || defined(__sparc)
            return "SPARC";
#elif defined(__m68k__)
            return "M68K";
#else
            return "UNKNOWN";
#endif
        }
    }

    string WebBrowser::DoCommandGlobal(const string& cmdName,
        const string& cmdParam1, const string& cmdParam2)
    {
        if (cmdName == wxStr("CPU"))
        {
            return wxStr(GetCPUArchitecture());
        }
        
        if (cmdName == wxStr("wxCHECK_MSG"))
        {
            wxCHECK_MSG(false, wxStr("true"), wxT("This is wxCHECK_MSG test message"));
            return wxStr("true");
        }

        if (cmdName == wxStr("GetUsefulDefines"))
            return wxStr(GetUsefulDefines());

        if (cmdName == wxStr("SizeOfLong")) 
        {
            return wxStr(std::to_string(sizeof(long)));
        }

        if (cmdName == wxStr("IsDebug"))
        {
#ifdef _DEBUG
            return wxStr("true");
#else
            return wxStr("false");
#endif
        }

        return wxStr(wxEmptyString);
    }

    string WebBrowser::DoCommand(const string& cmdName, const string& cmdParam1,
        const string& cmdParam2)
    {
        string noresult = wxStr("");

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

#if defined(__WXMSW__)
        if (cmdName == wxStr("IE.ShowPrintPreviewDialog"))
        {
            IEShowPrintPreviewDialog();
            return noresult;
        }
        if (cmdName == wxStr("IE.SetScriptErrorsSuppressed"))
        {
            bool supressErrors = (cmdParam1 == wxStr("true"));
            IESetScriptErrorsSuppressed(supressErrors);
            return noresult;
        }
#endif
        return noresult;
    }

}

