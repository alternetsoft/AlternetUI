#include "WebBrowser.h"

namespace Alternet::UI
{
  
    //-------------------------------------------------
    ALTERNET_UI_API int WebBrowser_GetBackend_(WebBrowser* obj)
    {
        return MarshalExceptions<int>([&]() {
            return obj->GetBackend();
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API long WebBrowser_Find_(WebBrowser* obj, const char16_t* text, int flags)
    {
        return MarshalExceptions<long>([&]() {
            return obj->Find(text,flags);
            });
    }
    //-------------------------------------------------
    wxWebViewZoomType ToWebViewZoomType(int zoomType)
    {
        wxWebViewZoomType zt = wxWEBVIEW_ZOOM_TYPE_LAYOUT;
        if (zoomType > 0)
            zt = wxWEBVIEW_ZOOM_TYPE_TEXT;
        return zt;
    }
    //-------------------------------------------------
    ALTERNET_UI_API void WebBrowser_SetZoomType_(WebBrowser* obj, int zoomType)
    {
        MarshalExceptions<void>([&]() {
            obj->SetZoomType(ToWebViewZoomType(zoomType));
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API c_bool WebBrowser_CanSetZoomType_(WebBrowser* obj, int type)
    {
        return MarshalExceptions<c_bool>([&]() {
            return obj->CanSetZoomType(ToWebViewZoomType(type));
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API int WebBrowser_GetZoomType_(WebBrowser* obj)
    {
        return MarshalExceptions<int>([&]() {
            return obj->GetZoomType();
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API c_bool WebBrowser_IsBackendWebKitAvailable_()
    {
        return MarshalExceptions<c_bool>([&]() {
            return WebBrowser::IsBackendWebKitAvailable();
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API c_bool WebBrowser_IsBackendIEAvailable_()
    {
        return MarshalExceptions<c_bool>([&]() {
            return WebBrowser::IsBackendIEAvailable();
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API void WebBrowser_CrtSetDbgFlag_(int value)
    {
        #ifdef _DEBUG
        MarshalExceptions<void>([&]() {
            _CrtSetDbgFlag(value);
            });
        #endif.
    }
    //-------------------------------------------------
    ALTERNET_UI_API c_bool WebBrowser_IsBackendEdgeAvailable_()
    {
        return MarshalExceptions<c_bool>([&]() {
            return WebBrowser::IsBackendEdgeAvailable();
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API char16_t* WebBrowser_GetLibraryVersionString_()
    {
        return MarshalExceptions<char16_t*>([&]() {
            return AllocPInvokeReturnString(WebBrowser::GetLibraryVersionString());
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API char16_t* WebBrowser_GetBackendVersionString_(int backend)
    {
        return MarshalExceptions<char16_t*>([&]() {
            return AllocPInvokeReturnString(
                WebBrowser::GetBackendVersionString((WebBrowserBackend)backend));
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API void WebBrowser_SetBackend_(int value)
    {
        MarshalExceptions<void>([&]() {
            WebBrowser::SetBackend((WebBrowserBackend)value);
            });
    }
    //-------------------------------------------------
}
//-------------------------------------------------
