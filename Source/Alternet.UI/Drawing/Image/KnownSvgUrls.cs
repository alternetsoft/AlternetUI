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
    public static class KnownSvgUrls
    {
        private const string ResTemplate =
            "embres:Alternet.UI.Resources.Svg.{0}.svg?assembly=Alternet.UI";

        /// <summary>
        /// Gets or sets url used to load "Error" svg image used in MessageBox like dialogs.
        /// </summary>
        public static string UrlImageMessageBoxError { get; set; } =
            GetImageUrl("alternet-circle-xmark");

        /// <summary>
        /// Gets or sets url used to load "File|New" svg image used in toolbars.
        /// </summary>
        public static string UrlImageFileNew { get; set; } =
            GetImageUrl("alternet-file");

        /// <summary>
        /// Gets or sets url used to load "Plus Inside Square" svg image.
        /// </summary>
        public static string UrlImageSquarePlus { get; set; } = GetImageUrl("alternet-square-plus");

        /// <summary>
        /// Gets or sets url used to load "Minus Inside Square" svg image.
        /// </summary>
        public static string UrlImageSquareMinus { get; set; } = GetImageUrl("alternet-square-minus");

        /// <summary>
        /// Gets or sets url used to load "File|Save" svg image used in toolbars.
        /// </summary>
        public static string UrlImageFileSave { get; set; } =
            GetImageUrl("alternet-floppy-disk");

        /// <summary>
        /// Gets or sets url used to load "Undo" svg image used in toolbars.
        /// </summary>
        public static string UrlImageUndo { get; set; } =
            GetImageUrl("alternet-undo");

        /// <summary>
        /// Gets or sets url used to load "Redo" svg image used in toolbars.
        /// </summary>
        public static string UrlImageRedo { get; set; } =
            GetImageUrl("alternet-redo");

        /// <summary>
        /// Gets or sets url used to load "Bold" svg image used in toolbars.
        /// </summary>
        public static string UrlImageBold { get; set; } =
            GetImageUrl("alternet-bold");

        /// <summary>
        /// Gets or sets url used to load "Italic" svg image used in toolbars.
        /// </summary>
        public static string UrlImageItalic { get; set; } =
            GetImageUrl("alternet-italic");

        /// <summary>
        /// Gets or sets url used to load "Underline" svg image used in toolbars.
        /// </summary>
        public static string UrlImageUnderline { get; set; } =
            GetImageUrl("alternet-underline");

        /// <summary>
        /// Gets or sets url used to load "File|Open" svg image used in toolbars.
        /// </summary>
        public static string UrlImageFileOpen { get; set; } =
            GetImageUrl("alternet-file-open");

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
        /// Gets or sets url used to load svg image used in "Yes" buttons.
        /// </summary>
        public static string UrlImageYes { get; set; } = GetImageUrl("alternet-check");

        /// <summary>
        /// Gets or sets url used to load svg image used in "No" buttons.
        /// </summary>
        public static string UrlImageNo { get; set; } = GetImageUrl("alternet-xmark");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Abort" buttons.
        /// </summary>
        public static string? UrlImageAbort { get; set; } = GetImageUrl("ban");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Retry" buttons.
        /// </summary>
        public static string? UrlImageRetry { get; set; } = GetImageUrl("arrows-rotate");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Ignore" buttons.
        /// </summary>
        public static string? UrlImageIgnore { get; set; }

        /// <summary>
        /// Gets or sets url used to load svg image used in "Help" buttons.
        /// </summary>
        public static string? UrlImageHelp { get; set; } = GetImageUrl("alternet-question");

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

        /// <summary>
        /// Gets or sets url used to load "Angle Down" svg image.
        /// </summary>
        public static string UrlImageAngleDown { get; set; } = GetImageUrl("alternet-angle-down");

        /// <summary>
        /// Gets or sets url used to load "Angle Up" svg image.
        /// </summary>
        public static string UrlImageAngleUp { get; set; } = GetImageUrl("alternet-angle-up");

        /// <summary>
        /// Gets or sets url used to load "Arrow Up" svg image.
        /// </summary>
        public static string UrlImageArrowUp { get; set; } = GetImageUrl("alternet-arrow-up");

        /// <summary>
        /// Gets or sets url used to load "Gear" svg image.
        /// </summary>
        public static string UrlImageGear { get; set; } = GetImageUrl("gear");

        /// <summary>
        /// Gets or sets url used to load "Arrow Down" svg image.
        /// </summary>
        public static string UrlImageArrowDown { get; set; } = GetImageUrl("alternet-arrow-down");

        /// <summary>
        /// Gets or sets url used to load svg image used in horizontal "More Actions" toolbar buttons.
        /// </summary>
        public static string UrlImageMoreActionsHorz { get; set; } =
            GetImageUrl("alternet-ellipsis");

        private static string GetImageUrl(string name) => string.Format(ResTemplate, name);
    }
}
