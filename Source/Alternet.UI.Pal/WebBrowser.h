#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"

namespace Alternet::UI
{
    class WebBrowser : public Control
    {
#include "Api/WebBrowser.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        wxWebView* GetWebViewCtrl();

        wxWebViewZoom GetWxWebViewZoom(WebViewZoom value);
        WebViewZoom GetWxWebViewZoom(wxWebViewZoom value);

    protected:
        virtual void OnWxWindowCreated() override;

    private:
        void OnNavigating(wxWebViewEvent& event);
        void OnNavigated(wxWebViewEvent& event);
        void OnLoaded(wxWebViewEvent& event);
        void OnError(wxWebViewEvent& event);
        void OnNewWindow(wxWebViewEvent& event);
        void OnTitleChanged(wxWebViewEvent& event);
    };
}
