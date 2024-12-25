using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal partial class MauiWebBrowserFactoryHandler : DisposableObject, IWebBrowserFactoryHandler
    {
        public bool IsEdgeBackendEnabled { get; set; }

        public IWebBrowserMemoryFS CreateMemoryFileSystem(WebBrowser browser)
        {
            throw new NotImplementedException();
        }

        public IControlHandler CreateWebBrowserHandler(WebBrowser control)
        {
            throw new NotImplementedException();
        }

        public string? DoCommand(string cmdName, params object?[] args)
        {
            return null;
        }

        public string GetBackendVersionString(WebBrowserBackend value)
        {
            return "Default";
        }

        public string GetLibraryVersionString()
        {
            return Environment.Version.ToString();
        }

        public bool IsBackendAvailable(WebBrowserBackend value)
        {
            return value == WebBrowserBackend.Default;
        }

        public void SetBackend(WebBrowserBackend value)
        {
        }

        public void SetDefaultFSNameArchive(string value)
        {
        }

        public void SetDefaultFSNameMemory(string value)
        {
        }

        public void SetDefaultPage(string url)
        {
        }

        public void SetDefaultScriptMesageName(string value)
        {
        }

        public void SetDefaultUserAgent(string value)
        {
        }

        public void SetEdgePath(string path)
        {
        }

        public void ThrowError(int errorId)
        {
        }
    }
}
