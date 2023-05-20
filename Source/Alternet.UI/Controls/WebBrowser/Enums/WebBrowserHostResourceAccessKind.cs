using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    ///     Kind of cross origin resource access allowed for host 
    ///     resources during download.
    /// </summary>
    public enum WebBrowserHostResourceAccessKind
    {
        /// <summary>
        ///     All cross origin resource access is denied, including 
        ///     normal sub resource access as src of a script or image element.
        /// </summary>
        Deny,

        /// <summary>
        ///     All cross origin resource access is allowed, including accesses 
        ///     that are subject to Cross-Origin Resource Sharing(CORS) check. 
        ///     The behavior is similar to a web site sends back http 
        ///     header Access-Control-Allow-Origin: *.
        /// </summary>
        Allow,

        /// <summary>
        ///     Cross origin resource access is allowed for normal 
        ///     sub resource access like as src of a script or image element, 
        ///     while any access that subjects to CORS check will be denied. 
        ///     See <see 
        ///     href="https://developer.mozilla.org/docs/Web/HTTP/CORS">
        ///     Cross-Origin Resource Sharing</see>
        ///     for more information.
        /// </summary>
        DenyCors
    }
}
