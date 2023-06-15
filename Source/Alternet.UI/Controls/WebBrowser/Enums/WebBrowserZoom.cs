using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Alternet.UI
{
    /// <summary>
    ///     The type of zooming that the WebBrowser control can perform.
    /// </summary>
    public enum WebBrowserZoomType
    {
        /// <summary>
        /// The entire page layout scales when zooming, including images.
        /// </summary>
        Layout,

        /// <summary>
        ///     Only the text changes in size when zooming. Images and other 
        ///     layout elements retain their initial size.
        /// </summary>
        Text,
    };

    /// <summary>
    ///     Zoom levels available in WebBrowser.
    /// </summary>
    public enum WebBrowserZoom
    {
        /// <summary>
        ///     Zoom page to 40% of original size.
        /// </summary>
        Tiny = 0,

        /// <summary>
        ///     Zoom page to 70% of original size.
        /// </summary>
        Small = 1,

        /// <summary>
        ///      Zoom page to original size. This is default size.
        /// </summary>
        Medium = 2,

        /// <summary>
        ///     Zoom page to 130% of original size.
        /// </summary>
        Large = 3,

        /// <summary>
        ///     Zoom page to 160% of original size.
        /// </summary>
        Largest = 4,
    }
}
