using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    
    /// <summary>
    ///     All possible backends supported by the WebBrowser control.
    /// </summary>
    public enum WebBrowserBackend
    {
        
        /// <summary>
        ///     Depending on the operating system, the appropriate backend will be used.
        /// </summary>
        Default = 0,
        
        /// <summary>
        ///     The IE backend uses Microsoft's WebBrowser control, which depends on the locally 
        ///     installed version of Internet Explorer. By default this backend emulates IE7. 
        ///     IE backend has full support for custom schemes and virtual file systems.
        ///     If you plan to display any modern web pages you should consider using Edge 
        ///     backend under MSW.
        /// </summary>
        IE = 1,
        
        /// <summary>
        ///     The IELatest backend uses Internet Explorer 11.
        /// </summary>
        IELatest = 2,
        
        /// <summary>
        ///     The Edge backend uses Microsoft's Edge WebView2. It is available for 
        ///     Windows 7 and newer. This backend does not support custom schemes 
        ///     and virtual file systems.
        /// </summary>
        Edge = 3,
        
        /// <summary>
        ///     Under GTK the WebKit backend uses WebKitGTK+. 
        ///     The macOS WebKit backend uses Apple's WKWebView class. 
        ///     Custom schemes and virtual files systems are supported under this backend. 
        ///     For further details see the documentation on wxWEBVIEW_WEBKIT.
        /// </summary>
        WebKit = 4,
        
    }
    
    public enum WebBrowserBackendOS 
    {
        Unknown = 0,
        MacOSX = 1,
        Unix = 2,
        Windows = 3,
    }
    
}
