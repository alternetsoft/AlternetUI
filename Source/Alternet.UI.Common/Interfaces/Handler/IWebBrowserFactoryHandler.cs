using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to create and manage web browser control.
    /// </summary>
    public interface IWebBrowserFactoryHandler : IDisposable
    {
        /// <inheritdoc cref="WebBrowser.IsEdgeBackendEnabled"/>
        bool IsEdgeBackendEnabled { get; set; }

        /// <summary>
        /// Creates <see cref="IControlHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateWebBrowserHandler(WebBrowser control);

        /// <summary>
        /// Throws C++ exception with the specified id.
        /// </summary>
        /// <param name="errorId"></param>
        void ThrowError(int errorId);

        /// <inheritdoc cref="WebBrowser.SetDefaultUserAgent"/>
        void SetDefaultUserAgent(string value);

        /// <inheritdoc cref="WebBrowser.SetDefaultScriptMessageName"/>
        void SetDefaultScriptMesageName(string value);

        /// <inheritdoc cref="WebBrowser.SetDefaultFSNameMemory"/>
        void SetDefaultFSNameMemory(string value);

        /// <inheritdoc cref="WebBrowser.SetDefaultFSNameArchive"/>
        void SetDefaultFSNameArchive(string value);

        /// <inheritdoc cref="WebBrowser.DoCommandGlobal"/>
        string? DoCommand(string cmdName, params object?[] args);

        /// <summary>
        /// Creates <see cref="IWebBrowserMemoryFS"/> interface provider.
        /// </summary>
        /// <param name="browser">Owner.</param>
        /// <returns></returns>
        IWebBrowserMemoryFS CreateMemoryFileSystem(WebBrowser browser);

        /// <inheritdoc cref="WebBrowser.SetBackendPath(string, bool)"/>
        void SetEdgePath(string path);

        /// <inheritdoc cref="WebBrowser.IsBackendAvailable"/>
        bool IsBackendAvailable(WebBrowserBackend value);

        /// <inheritdoc cref="WebBrowser.SetBackend"/>
        void SetBackend(WebBrowserBackend value);

        /// <inheritdoc cref="WebBrowser.GetLibraryVersionString"/>
        string GetLibraryVersionString();

        /// <inheritdoc cref="WebBrowser.GetBackendVersionString"/>
        string GetBackendVersionString(WebBrowserBackend value);

        /// <inheritdoc cref="WebBrowser.SetDefaultPage(string)"/>
        void SetDefaultPage(string url);
    }
}
