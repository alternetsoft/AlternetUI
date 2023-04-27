#include "WebBrowser.h"

namespace Alternet::UI
{
    WebBrowser::WebBrowser()
    {
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
            }
        }
    }

    wxWindow* WebBrowser::CreateWxWindowCore(wxWindow* parent)
    {
        auto webView = wxWebView::New(parent, -1, "", wxDefaultPosition, wxDefaultSize, "", 0, "");
        webView->Bind(wxEVT_WEBVIEW_NAVIGATING, &WebBrowser::OnNavigating, this);
        webView->Bind(wxEVT_WEBVIEW_NAVIGATED, &WebBrowser::OnNavigated, this);
        webView->Bind(wxEVT_WEBVIEW_LOADED, &WebBrowser::OnLoaded, this);
        webView->Bind(wxEVT_WEBVIEW_ERROR, &WebBrowser::OnError, this);
        webView->Bind(wxEVT_WEBVIEW_NEWWINDOW, &WebBrowser::OnNewWindow, this);
        webView->Bind(wxEVT_WEBVIEW_TITLE_CHANGED, &WebBrowser::OnTitleChanged, this);
        return webView;
    }

    void WebBrowser::OnWxWindowCreated()
    {
    }

    void WebBrowser::OnNavigating(wxWebViewEvent& event)
    {
        RaiseEvent(WebBrowserEvent::Navigating);
    }

    void WebBrowser::OnNavigated(wxWebViewEvent& event)
    {
        RaiseEvent(WebBrowserEvent::Navigated);
    }

    void WebBrowser::OnLoaded(wxWebViewEvent& event)
    {
        RaiseEvent(WebBrowserEvent::Loaded);
    }

    void WebBrowser::OnError(wxWebViewEvent& event)
    {
        RaiseEvent(WebBrowserEvent::Error);
    }

    void WebBrowser::OnNewWindow(wxWebViewEvent& event)
    {
        RaiseEvent(WebBrowserEvent::NewWindow);
    }

    void WebBrowser::OnTitleChanged(wxWebViewEvent& event)
    {
        RaiseEvent(WebBrowserEvent::TitleChanged);
    }
    

    WebViewZoom WebBrowser::GetWxWebViewZoom(wxWebViewZoom value)
    {
        switch (value)
        {
            case wxWebViewZoom::wxWEBVIEW_ZOOM_TINY:
                return WebViewZoom::Tiny;
            case wxWebViewZoom::wxWEBVIEW_ZOOM_SMALL:
                return WebViewZoom::Small;
            case wxWebViewZoom::wxWEBVIEW_ZOOM_MEDIUM:
                return WebViewZoom::Medium;
            case wxWebViewZoom::wxWEBVIEW_ZOOM_LARGE:
                return WebViewZoom::Large;
            case wxWebViewZoom::wxWEBVIEW_ZOOM_LARGEST:
                return WebViewZoom::Largest;
        }
    }

    wxWebViewZoom WebBrowser::GetWxWebViewZoom(WebViewZoom value)
    {
        switch (value)
        {
        case WebViewZoom::Tiny:
            return wxWebViewZoom::wxWEBVIEW_ZOOM_TINY;
        case WebViewZoom::Small:
            return wxWebViewZoom::wxWEBVIEW_ZOOM_SMALL;
        case WebViewZoom::Medium:
            return wxWebViewZoom::wxWEBVIEW_ZOOM_MEDIUM;
        case WebViewZoom::Large:
            return wxWebViewZoom::wxWEBVIEW_ZOOM_LARGE;
        case WebViewZoom::Largest:
            return wxWebViewZoom::wxWEBVIEW_ZOOM_LARGEST;
        default:
            throwExNoInfo;
        }
    }

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

    WebViewZoom WebBrowser::GetZoom()
    {
        return GetWxWebViewZoom(GetWebViewCtrl()->GetZoom());
    }

    void WebBrowser::SetZoom(WebViewZoom value)
    {
        GetWebViewCtrl()->SetZoom(GetWxWebViewZoom(value));
    }

    void WebBrowser::LoadURL(const string& url)
    {
        GetWebViewCtrl()->LoadURL(wxStr(url));
    }

    wxWebView* WebBrowser::GetWebViewCtrl()
    {
        return dynamic_cast<wxWebView*>(GetWxWindow());
    }
}
