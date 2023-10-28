using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to Svg image handling.
    /// </summary>
    public static class SvgUtils
    {
        private const string ResTemplate =
            "embres:Alternet.UI.Resources.Svg.{0}.svg?assembly=Alternet.UI";

        /// <summary>
        /// Gets or sets url used to load "Error" svg image used in MessageBox like dialogs.
        /// </summary>
        public static string UrlImageMessageBoxError { get; set; } =
            GetImageUrl("alternet-circle-xmark");

        /// <summary>
        /// Gets or sets url used to load "Information" svg image used in MessageBox like dialogs.
        /// </summary>
        public static string UrlImageMessageBoxInformation { get; set; } =
            GetImageUrl("alternet-circle-info");

        /// <summary>
        /// Gets or sets url used to load "Warning" svg image used in MessageBox like dialogs.
        /// </summary>
        public static string UrlImageMessageBoxWarning { get; set; } =
            GetImageUrl("alternet-circle-exclamation");

        /// <summary>
        /// Gets or sets url used to load "plus" svg image used in "Add" toolbar buttons.
        /// </summary>
        public static string UrlImagePlus { get; set; } = GetImageUrl("alternet-plus");

        /// <summary>
        /// Gets or sets url used to load "minus" svg image used in "Remove" toolbar buttons.
        /// </summary>
        public static string UrlImageMinus { get; set; } = GetImageUrl("alternet-minus");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Ok" toolbar buttons.
        /// </summary>
        public static string UrlImageOk { get; set; } = GetImageUrl("alternet-check");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Cancel" toolbar buttons.
        /// </summary>
        public static string UrlImageCancel { get; set; } = GetImageUrl("alternet-xmark");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Add child" toolbar buttons.
        /// </summary>
        public static string UrlImageAddChild { get; set; } = GetImageUrl("alternet-add-child");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Remove All" toolbar buttons.
        /// </summary>
        public static string UrlImageRemoveAll { get; set; } = GetImageUrl("alternet-eraser");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Apply" toolbar buttons.
        /// </summary>
        public static string UrlImageApply { get; set; } = GetImageUrl("alternet-square-check");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Back" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static string UrlImageWebBrowserBack { get; set; } = GetImageUrl("alternet-arrow-left");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Home" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static string UrlImageWebBrowserHome { get; set; } = GetImageUrl("alternet-house");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Forward" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static string UrlImageWebBrowserForward { get; set; } = GetImageUrl("alternet-arrow-right");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Zoom In" toolbar buttons.
        /// </summary>
        public static string UrlImageZoomIn { get; set; } = GetImageUrl("alternet-zoomin");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Zoom Out" toolbar buttons.
        /// </summary>
        public static string UrlImageZoomOut { get; set; } = GetImageUrl("alternet-zoomout");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Go" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static string UrlImageWebBrowserGo { get; set; } = GetImageUrl("alternet-caret-right");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Refresh" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static string UrlImageWebBrowserRefresh { get; set; } = GetImageUrl("alternet-rotate-right");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Stop" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static string UrlImageWebBrowserStop { get; set; } = GetImageUrl("alternet-xmark");

        /// <summary>
        /// Gets or sets url used to load svg image used in "More Actions" toolbar buttons.
        /// </summary>
        public static string UrlImageMoreActions { get; set; } =
            GetImageUrl("alternet-ellipsis-vertical");

        private static string GetImageUrl(string name) => string.Format(ResTemplate, name);
    }
}
