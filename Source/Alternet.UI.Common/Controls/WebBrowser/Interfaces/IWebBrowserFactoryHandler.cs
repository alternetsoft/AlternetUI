using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IWebBrowserFactoryHandler : IDisposable
    {
        IWebBrowserHandler CreateWebBrowserHandler(WebBrowser control);

        void ThrowError(int errorId);

        void SetDefaultUserAgent(string value);

        void SetDefaultScriptMesageName(string value);

        void SetDefaultFSNameMemory(string value);

        void SetDefaultFSNameArchive(string value);

        string? DoCommand(string cmdName, params object?[] args);

        IWebBrowserMemoryFS CreateMemoryFileSystem(WebBrowser browser);

        WebBrowserBackendOS GetBackendOS();

        void SetEdgePath(string path);

        bool IsBackendAvailable(WebBrowserBackend value);

        void SetBackend(WebBrowserBackend value);

        string GetLibraryVersionString();

        string GetBackendVersionString(WebBrowserBackend value);

        void SetDefaultPage(string url);
    }
}
