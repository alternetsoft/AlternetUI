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
            Native.WebBrowser.SetDefaultUserAgent(value);
        }

        public void SetDefaultScriptMesageName(string value)
        {
            Native.WebBrowser.SetDefaultScriptMesageName(value);
        }

        public void SetDefaultFSNameMemory(string value)
        {
            Native.WebBrowser.SetDefaultFSNameMemory(value);
        }

        public void SetDefaultFSNameArchive(string value)
        {
            Native.WebBrowser.SetDefaultFSNameArchive(value);
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
            Native.WebBrowser.SetEdgePath(path);
        }

        public string GetBackendVersionString(WebBrowserBackend value)
        {
            return WxWebBrowserHandlerApi.WebBrowser_GetBackendVersionString_((int)value);
        }

        public void ThrowError(int errorId)
        {
            Native.Application.ThrowError(errorId);
        }

        public string GetLibraryVersionString()
        {
            return WxWebBrowserHandlerApi.WebBrowser_GetLibraryVersionString_();
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
