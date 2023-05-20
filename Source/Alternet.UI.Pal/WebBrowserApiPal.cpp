#include "WebBrowser.h"

namespace Alternet::UI
{
    //-------------------------------------------------
   /* ALTERNET_UI_API void WebBrowser_SelectAll_(WebBrowser* obj)
    {
        MarshalExceptions<void>([&]() {
            obj->SelectAll();
            });
    }*/
    //-------------------------------------------------
    //ALTERNET_UI_API c_bool WebBrowser_HasSelection_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<c_bool>([&]() {
    //        return obj->HasSelection();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API void WebBrowser_DeleteSelection_(WebBrowser* obj)
    //{
    //    MarshalExceptions<void>([&]() {
    //        obj->DeleteSelection();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API char16_t* WebBrowser_GetSelectedText_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<char16_t*>([&]() {
    //        return AllocPInvokeReturnString(obj->GetSelectedText());
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API char16_t* WebBrowser_GetSelectedSource_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<char16_t*>([&]() {
    //        return AllocPInvokeReturnString(obj->GetSelectedSource());
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API void WebBrowser_ClearSelection_(WebBrowser* obj)
    //{
    //    MarshalExceptions<void>([&]() {
    //        obj->ClearSelection();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API c_bool WebBrowser_CanCut_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<c_bool>([&]() {
    //        return obj->CanCut();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API c_bool WebBrowser_CanCopy_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<c_bool>([&]() {
    //        return obj->CanCopy();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API c_bool WebBrowser_CanPaste_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<c_bool>([&]() {
    //        return obj->CanPaste();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API void WebBrowser_Cut_(WebBrowser* obj)
    //{
    //    MarshalExceptions<void>([&]() {
    //        obj->Cut();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API void WebBrowser_Copy_(WebBrowser* obj)
    //{
    //    MarshalExceptions<void>([&]() {
    //        obj->Copy();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API void WebBrowser_Paste_(WebBrowser* obj)
    //{
    //    MarshalExceptions<void>([&]() {
    //        obj->Paste();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API c_bool WebBrowser_CanUndo_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<c_bool>([&]() {
    //        return obj->CanUndo();
    //        });
    //}
    //-------------------------------------------------
    ALTERNET_UI_API int WebBrowser_GetBackend_(WebBrowser* obj)
    {
        return MarshalExceptions<int>([&]() {
            return obj->GetBackend();
            });
    }
    //-------------------------------------------------
    //ALTERNET_UI_API c_bool WebBrowser_CanRedo_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<c_bool>([&]() {
    //        return obj->CanRedo();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API void WebBrowser_Undo_(WebBrowser* obj)
    //{
    //    MarshalExceptions<void>([&]() {
    //        obj->Undo();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API void WebBrowser_Redo_(WebBrowser* obj)
    //{
    //    MarshalExceptions<void>([&]() {
    //        obj->Redo();
    //        });
    //}
    //-------------------------------------------------
   /* ALTERNET_UI_API void WebBrowser_SetDefaultPage_(const char16_t* value)
    {
        MarshalExceptions<void>([&]() {
            WebBrowser::SetDefaultPage(value);
            });
    }*/
    //-------------------------------------------------
    ALTERNET_UI_API long WebBrowser_Find_(WebBrowser* obj, const char16_t* text, int flags)
    {
        return MarshalExceptions<long>([&]() {
            return obj->Find(text,flags);
            });
    }
    //-------------------------------------------------
    //ALTERNET_UI_API c_bool WebBrowser_IsBusy_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<c_bool>([&]() {
    //        return obj->IsBusy();
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API c_bool WebBrowser_IsContextMenuEnabled_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<c_bool>([&]() {
    //        return obj->IsContextMenuEnabled();
    //        });
    //}
    //-------------------------------------------------
  /*  ALTERNET_UI_API c_bool WebBrowser_IsAccessToDevToolsEnabled_(WebBrowser* obj)
    {
        return MarshalExceptions<c_bool>([&]() {
            return obj->IsAccessToDevToolsEnabled();
            });
    }*/
    //-------------------------------------------------
   /* ALTERNET_UI_API void WebBrowser_Print_(WebBrowser* obj)
    {
        MarshalExceptions<void>([&]() {
            obj->Print();
            });
    }*/
    //-------------------------------------------------
 /*   ALTERNET_UI_API c_bool WebBrowser_SetUserAgent_(WebBrowser* obj, const char16_t* userAgent)
    {
        return MarshalExceptions<c_bool>([&]() {
            return obj->SetUserAgent(userAgent);
            });
    }*/
    //-------------------------------------------------
    //ALTERNET_UI_API char16_t* WebBrowser_GetPageSource_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<char16_t*>([&]() {
    //        return AllocPInvokeReturnString(obj->GetPageSource());
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API char16_t* WebBrowser_GetPageText_(WebBrowser* obj)
    //{
    //    return MarshalExceptions<char16_t*>([&]() {
    //        return AllocPInvokeReturnString(obj->GetPageText());
    //        });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API c_bool WebBrowser_AddUserScript_(WebBrowser* obj, const char16_t* javascript,
    //    int injectionTime)
    //{
    //    return MarshalExceptions<c_bool>([&]() {

    //        auto result = obj->AddUserScript(javascript);
    //        return result;
    //    });
    //}
    //-------------------------------------------------
    //ALTERNET_UI_API char16_t* WebBrowser_RunScript_(WebBrowser* obj, const char16_t* javascript) 
    //{
    //    return MarshalExceptions<char16_t*>([&]() {
    //        
    //        auto result = obj->RunScript(javascript);
    //        return AllocPInvokeReturnString(result);
    //     });
    //}
    //-------------------------------------------------
   /* ALTERNET_UI_API char16_t* WebBrowser_GetUserAgent_(WebBrowser* obj)
    {
        return MarshalExceptions<char16_t*>([&]() {
            return AllocPInvokeReturnString(obj->GetUserAgent());
            });
    }*/
    //-------------------------------------------------
    /*ALTERNET_UI_API void WebBrowser_EnableContextMenu_(WebBrowser* obj, c_bool enable)
    {
        MarshalExceptions<void>([&]() {
            obj->EnableContextMenu(enable);
            });
    }*/
    //-------------------------------------------------
  /*  ALTERNET_UI_API void WebBrowser_EnableAccessToDevTools_(WebBrowser* obj, c_bool enable)
    {
        MarshalExceptions<void>([&]() {
            obj->EnableAccessToDevTools(enable);
            });
    }*/
    //-------------------------------------------------
   /* ALTERNET_UI_API void WebBrowser_RemoveAllUserScripts_(WebBrowser* obj)
    {
        MarshalExceptions<void>([&]() {
            obj->RemoveAllUserScripts();
            });
    }*/
    //-------------------------------------------------
  /*  ALTERNET_UI_API c_bool WebBrowser_AddScriptMessageHandler_(WebBrowser* obj, 
        const char16_t* name)
    {
        return MarshalExceptions<c_bool>([&]() {
            return obj->AddScriptMessageHandler(name);
            });
    }*/
    //-------------------------------------------------
    /*ALTERNET_UI_API c_bool WebBrowser_RemoveScriptMessageHandler_(WebBrowser* obj, const char16_t* name)
    {
        return MarshalExceptions<c_bool>([&]() {
            return obj->RemoveScriptMessageHandler(name);
            });
    }*/
    //-------------------------------------------------
    //ALTERNET_UI_API void WebBrowser_SetZoomFactor_(WebBrowser* obj, float zoom)
    //{
    //    MarshalExceptions<void>([&]() {
    //        obj->SetZoomFactor(zoom);
    //        });
    //}
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
    /*ALTERNET_UI_API float WebBrowser_GetZoomFactor_(WebBrowser* obj)
    {
        return MarshalExceptions<float>([&]() {
            return obj->GetZoomFactor();
            });
    }*/
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
        MarshalExceptions<void>([&]() {
            _CrtSetDbgFlag(value);
            });
    }
    //-------------------------------------------------
    ALTERNET_UI_API c_bool WebBrowser_IsBackendEdgeAvailable_()
    {
        return MarshalExceptions<c_bool>([&]() {
            return WebBrowser::IsBackendEdgeAvailable();
            });
    }
    //-------------------------------------------------
  /*  ALTERNET_UI_API void WebBrowser_SetPage_(WebBrowser* obj, const char16_t* html, const char16_t* baseUrl)
    {
        MarshalExceptions<void>([&]() {
            obj->SetPage(html, baseUrl);
            });
    }*/
    //-------------------------------------------------
   /* ALTERNET_UI_API void WebBrowser_ReloadDefault_(WebBrowser* obj)
    {
        MarshalExceptions<void>([&]()
            {
                obj->Reload();
            }
        );
    }*/
    //-------------------------------------------------
    /*ALTERNET_UI_API void WebBrowser_Reload_(WebBrowser* obj, bool noCache)
    {
        MarshalExceptions<void>([&]()
            {
                obj->Reload(noCache);
            }
        );
    }*/
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
    /*ALTERNET_UI_API void WebBrowser_Stop_(WebBrowser* obj)
    {
        MarshalExceptions<void>([&]()
            {
                obj->Stop();
            }
        );
    }*/
    //-------------------------------------------------
    /*ALTERNET_UI_API c_bool WebBrowser_CanGoBack_(WebBrowser* obj)
    {
        return MarshalExceptions<c_bool>([&]() {
            return obj->CanGoBack();
            });
    }*/
    //-------------------------------------------------
   /* ALTERNET_UI_API c_bool WebBrowser_CanGoForward_(WebBrowser* obj)
    {
        return MarshalExceptions<c_bool>([&]() {
            return obj->CanGoForward();
            });
    }*/
    //-------------------------------------------------
    /*ALTERNET_UI_API void WebBrowser_GoBack_(WebBrowser* obj)
    {
        MarshalExceptions<void>([&]()
            {
                obj->GoBack();
            }
        );
    }*/
    //-------------------------------------------------
    /*ALTERNET_UI_API void WebBrowser_GoForward_(WebBrowser* obj)
    {
        MarshalExceptions<void>([&]()
            {
                obj->GoForward();
            }
        );
    }*/
    //-------------------------------------------------
   /* ALTERNET_UI_API void WebBrowser_ClearHistory_(WebBrowser* obj)
    {
        MarshalExceptions<void>([&]()
            {
                obj->ClearHistory();
            }
        );
    }*/
    //-------------------------------------------------
    /*ALTERNET_UI_API void WebBrowser_EnableHistory_(WebBrowser* obj, c_bool enable = true)
    {
        MarshalExceptions<void>([&]()
            {
                obj->EnableHistory(enable);
            }
        );
    }*/
    //-------------------------------------------------
}
//-------------------------------------------------
