using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WebBrowserFactoryHandler : DisposableObject, IWebBrowserFactoryHandler
    {
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
            => WebBrowserHandler.IsBackendIEAvailable();

        public bool IsBackendEdgeAvailable()
            => WebBrowserHandler.IsBackendEdgeAvailable();

        public bool IsBackendWebKitAvailable()
            => WebBrowserHandler.IsBackendWebKitAvailable();

        public string? DoCommand(string cmdName, params object?[] args)
        {
            return WebBrowserHandler.DoCommandGlobal(cmdName, args);
        }

        public IWebBrowserMemoryFS CreateMemoryFileSystem(WebBrowser browser)
        {
            return new WebBrowserMemoryFS(browser);
        }

        public void SetEdgePath(string path)
        {
            Native.WebBrowser.SetEdgePath(path);
        }

        public string GetBackendVersionString(WebBrowserBackend value)
        {
            return WebBrowserHandlerApi.WebBrowser_GetBackendVersionString_((int)value);
        }

        public void ThrowError(int errorId)
        {
            Native.Application.ThrowError(errorId);
        }

        public string GetLibraryVersionString()
        {
            return WebBrowserHandlerApi.WebBrowser_GetLibraryVersionString_();
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
            WebBrowserHandlerApi.WebBrowser_SetBackend_((int)value);
        }

        public void SetDefaultPage(string url)
        {
            WebBrowserHandlerApi.WebBrowser_SetDefaultPage_(url);
        }

        public IWebBrowserHandler CreateWebBrowserHandler(WebBrowser control)
        {
            return new WebBrowserHandler();
        }
    }
}
