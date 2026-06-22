using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxWebBrowserFactoryHandler : DisposableObject, IWebBrowserFactoryHandler
    {
        public bool IsEdgeBackendEnabled
        {
            get
            {
                return Native.WebBrowser.IsEdgeBackendEnabled;
            }

            set
            {
                Native.WebBrowser.IsEdgeBackendEnabled = value;
            }
        }

        public void SetDefaultUserAgent(string value)
        {
            NativeStringSpan.Invoke(value, span =>
            {
                Native.WebBrowser.SetDefaultUserAgent(span);
            });
        }

        public void SetDefaultScriptMesageName(string value)
        {
            NativeStringSpan.Invoke(value, span =>
            {
                Native.WebBrowser.SetDefaultScriptMesageName(span);
            });
        }

        public void SetDefaultFSNameMemory(string value)
        {
            NativeStringSpan.Invoke(value, span =>
            {
                Native.WebBrowser.SetDefaultFSNameMemory(span);
            });
        }

        public void SetDefaultFSNameArchive(string value)
        {
            NativeStringSpan.Invoke(value, span =>
            {
                Native.WebBrowser.SetDefaultFSNameArchive(span);
            });
        }

        public WebBrowserBackendOS GetBackendOS()
        {
            return (WebBrowserBackendOS)Native.WebBrowser.GetBackendOS();
        }

        public bool IsBackendIEAvailable()
            => WxWebBrowserHandler.IsBackendIEAvailable();

        public bool IsBackendEdgeAvailable()
            => WxWebBrowserHandler.IsBackendEdgeAvailable();

        public bool IsBackendWebKitAvailable()
            => WxWebBrowserHandler.IsBackendWebKitAvailable();

        public string? DoCommand(string cmdName, params object?[] args)
        {
            return WxWebBrowserHandler.DoCommandGlobal(cmdName, args);
        }

        public IWebBrowserMemoryFS CreateMemoryFileSystem(WebBrowser browser)
        {
            return new WxWebBrowserMemoryFS(browser);
        }

        public void SetEdgePath(string path)
        {
            NativeStringSpan.Invoke(path, span =>
            {
                Native.WebBrowser.SetEdgePath(span);
            });
        }

        public string GetBackendVersionString(WebBrowserBackend value)
        {
            return Native.WebBrowser.GetBackendVersionString((int)value);
        }

        public void ThrowError(int errorId)
        {
            Native.Application.ThrowError(errorId);
        }

        public string GetLibraryVersionString()
        {
            return Native.WebBrowser.GetLibraryVersionString();
        }

        public bool IsBackendAvailable(WebBrowserBackend value)
        {
            return value switch
            {
                WebBrowserBackend.Default => true,
                WebBrowserBackend.IE or WebBrowserBackend.IELatest =>
                    IsBackendIEAvailable(),
                WebBrowserBackend.Edge =>
                    IsBackendEdgeAvailable(),
                WebBrowserBackend.WebKit =>
                    IsBackendWebKitAvailable(),
                _ => false,
            };
        }

        public void SetBackend(WebBrowserBackend value)
        {
            WxWebBrowserHandlerApi.WebBrowser_SetBackend_((int)value);
        }

        public void SetDefaultPage(string url)
        {
            WxWebBrowserHandlerApi.WebBrowser_SetDefaultPage_(url);
        }

        public IControlHandler CreateWebBrowserHandler(WebBrowser control)
        {
            return new WxWebBrowserHandler();
        }
    }
}
