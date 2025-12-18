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
    /// Contains URLs for svg images that are included in the library resources.
    /// </summary>
    public static class KnownSvgUrls
    {
        /// <summary>
        /// Gets template for the svg resource URLs.
        /// </summary>
        public static string ResTemplate { get; } =
            "embres:Alternet.UI.Common.Resources.Svg.{0}.svg?assembly=Alternet.UI.Common";

        /// <summary>
        /// Gets or sets url used to load "Square Check Filled" svg image.
        /// </summary>
        public static string UrlImageSquareCheckFilled { get; set; }
            = GetImageUrl("alternet-square-check-filled");

        /// <summary>
        /// Gets or sets url used to load "Square Minus Filled" svg image.
        /// </summary>
        public static string UrlImageSquareMinusFilled { get; set; }
            = GetImageUrl("alternet-square-minus-filled");

        /// <summary>
        /// Gets or sets url used to load "Square Plus Filled" svg image.
        /// </summary>
        public static string UrlImageSquarePlusFilled { get; set; }
            = GetImageUrl("alternet-square-plus-filled");

        /// <summary>
        /// Gets or sets url used to load "Square" svg image.
        /// </summary>
        public static string UrlImageSquare { get; set; } = GetImageUrl("alternet-square");

        /// <summary>
        /// Gets or sets url used to load "Circle Dot Filled" svg image.
        /// </summary>
        public static string UrlImageCircleDotFilled { get; set; }
            = GetImageUrl("alternet-circle-dot-filled");

        /// <summary>
        /// Gets or sets url used to load "Circle Dot" svg image.
        /// </summary>
        public static string UrlImageCircleDot { get; set; } = GetImageUrl("alternet-circle-dot");

        /// <summary>
        /// Gets or sets url used to load "Triangle Arrow Down" svg image.
        /// </summary>
        public static string UrlImageTriangleArrowDown { get; set; }
            = GetImageUrl("TriangleArrow.alternet-triangle-arrow-down");

        /// <summary>
        /// Gets or sets url used to load "Triangle Arrow Left" svg image.
        /// </summary>
        public static string UrlImageTriangleArrowLeft { get; set; }
            = GetImageUrl("TriangleArrow.alternet-triangle-arrow-left");

        /// <summary>
        /// Gets or sets url used to load "Triangle Arrow Right" svg image.
        /// </summary>
        public static string UrlImageTriangleArrowRight { get; set; }
            = GetImageUrl("TriangleArrow.alternet-triangle-arrow-right");

        /// <summary>
        /// Gets or sets url used to load "Triangle Arrow Up" svg image.
        /// </summary>
        public static string UrlImageTriangleArrowUp { get; set; }
            = GetImageUrl("TriangleArrow.alternet-triangle-arrow-up");

        /// <summary>
        /// Gets or sets url used to load an empty svg image.
        /// </summary>
        public static string UrlImageEmpty { get; set; } = GetImageUrl("alternet-empty");

        /// <summary>
        /// Gets or sets url used to load "Error" svg image used in MessageBox like dialogs.
        /// </summary>
        public static string UrlImageMessageBoxError { get; set; } =
            GetImageUrl("alternet-circle-xmark");

        /// <summary>
        /// Gets or sets url used to load "Search" svg image used in toolbars.
        /// </summary>
        public static string UrlImageSearch { get; set; } =
            GetImageUrl("alternet-search");

        /// <summary>
        /// Gets or sets url used to load "File|New" svg image used in toolbars.
        /// </summary>
        public static string UrlImageFileNew { get; set; } =
            GetImageUrl("alternet-file");

        /// <summary>
        /// Gets or sets url used to load "Eye on" svg image.
        /// </summary>
        public static string UrlImageEyeOn { get; set; } = GetImageUrl("alternet-eye-on");

        /// <summary>
        /// Gets or sets url used to load "Eye off" svg image.
        /// </summary>
        public static string UrlImageEyeOff { get; set; } = GetImageUrl("alternet-eye-off");

        /// <summary>
        /// Gets or sets url used to load "Plus Inside Square" svg image.
        /// </summary>
        public static string UrlImageSquarePlus { get; set; } = GetImageUrl("alternet-square-plus");

        /// <summary>
        /// Gets or sets url used to load "Keyboard" svg image.
        /// </summary>
        public static string UrlImageKeyboard { get; set; } = GetImageUrl("alternet-keyboard");

        /// <summary>
        /// Gets or sets url used to load "Minus Inside Square" svg image.
        /// </summary>
        public static string UrlImageSquareMinus { get; set; } = GetImageUrl("alternet-square-minus");

        /// <summary>
        /// Gets or sets url used to load "Replace" svg image.
        /// </summary>
        public static string UrlImageReplace { get; set; } = GetImageUrl("alternet-replace");

        /// <summary>
        /// Gets or sets url used to load "Replace All" svg image.
        /// </summary>
        public static string UrlImageReplaceAll { get; set; } = GetImageUrl("alternet-replace-all");

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
        /// Gets or sets url used to load "Circle Filled" svg image.
        /// </summary>
        public static string UrlImageCircleFilled { get; set; }
            = GetImageUrl("alternet-circle-filled");

        /// <summary>
        /// Gets or sets url used to load "Debug Run" svg image.
        /// </summary>
        public static string UrlImageDebugRun { get; set; } = GetImageUrl("alternet-debug-run");

        /// <summary>
        /// Gets or sets url used to load "Trash Can" svg image.
        /// </summary>
        public static string UrlImageTrashCan { get; set; } = GetImageUrl("alternet-trash-can");

        /// <summary>
        /// Gets or sets url used to load "Paste" svg image.
        /// </summary>
        public static string UrlImagePaste { get; set; } = GetImageUrl("alternet-paste");

        /// <summary>
        /// Gets or sets url used to load "Copy" svg image.
        /// </summary>
        public static string UrlImageCopy { get; set; } = GetImageUrl("alternet-copy");

        /// <summary>
        /// Gets or sets url used to load "Cut" svg image.
        /// </summary>
        public static string UrlImageCut { get; set; } = GetImageUrl("alternet-scissors");

        /// <summary>
        /// Gets or sets url used to load "Diamond Filled" svg image.
        /// </summary>
        public static string UrlImageDiamondFilled { get; set; }
            = GetImageUrl("alternet-diamond-filled");

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
        /// Gets or sets url used to load paint brush svg image.
        /// </summary>
        public static string UrlImagePaintBrush { get; set; } = GetImageUrl("alternet-paintbrush");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Close" toolbar buttons.
        /// </summary>
        public static string UrlImageClose { get; set; } = GetImageUrl("alternet-xmark");

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
        public static string UrlImageAbort { get; set; } = GetImageUrl("alternet-ban");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Retry" buttons.
        /// </summary>
        public static string UrlImageRetry { get; set; } = GetImageUrl("alternet-arrows-rotate");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Ignore" buttons.
        /// </summary>
        public static string UrlImageIgnore { get; set; } = GetImageUrl("alternet-empty"); /* !!!! */

        /// <summary>
        /// Gets or sets url used to load svg image used in "Help" buttons.
        /// </summary>
        public static string UrlImageHelp { get; set; } = GetImageUrl("alternet-question");

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
        public static string UrlImageWebBrowserBack { get; set; }
            = GetImageUrl("alternet-arrow-left");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Home" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static string UrlImageWebBrowserHome { get; set; } = GetImageUrl("alternet-house");

        /// <summary>
        /// Gets or sets url used to load svg image used in "Forward" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static string UrlImageWebBrowserForward { get; set; }
            = GetImageUrl("alternet-arrow-right");

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
        public static string UrlImageWebBrowserRefresh { get; set; }
            = GetImageUrl("alternet-rotate-right");

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
        /// Gets or sets url used to load "Angle Right" svg image.
        /// </summary>
        public static string UrlImageAngleRight { get; set; } = GetImageUrl("alternet-angle-right");

        /// <summary>
        /// Gets or sets url used to load "Angle Left" svg image.
        /// </summary>
        public static string UrlImageAngleLeft { get; set; } = GetImageUrl("alternet-angle-left");

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
        public static string UrlImageGear { get; set; } = GetImageUrl("alternet-gear");

        /// <summary>
        /// Gets or sets url used to load "Use regular expressions" svg image.
        /// </summary>
        public static string UrlImageRegularExpr { get; set; } = GetImageUrl("alternet-regular-expr");

        /// <summary>
        /// Gets or sets url used to load "Find Match Case" svg image.
        /// </summary>
        public static string UrlImageFindMatchCase { get; set; } = GetImageUrl("alternet-match-case");

        /// <summary>
        /// Gets or sets url used to load "Find Match Full Word" svg image.
        /// </summary>
        public static string UrlImageFindMatchFullWord { get; set; }
            = GetImageUrl("alternet-match-full-word");

        /// <summary>
        /// Gets or sets url used to load "Arrow Down" svg image.
        /// </summary>
        public static string UrlImageArrowDown { get; set; } = GetImageUrl("alternet-arrow-down");

        /// <summary>
        /// Gets or sets url used to load "Arrow Left" svg image.
        /// </summary>
        public static string UrlImageArrowLeft { get; set; } = GetImageUrl("alternet-arrow-left");

        /// <summary>
        /// Gets or sets url used to load "Arrow Right" svg image.
        /// </summary>
        public static string UrlImageArrowRight { get; set; } = GetImageUrl("alternet-arrow-right");

        /// <summary>
        /// Gets or sets url used to load "Circle" svg image.
        /// </summary>
        public static string UrlImageCircle { get; set; } = GetImageUrl("alternet-circle");

        /// <summary>
        /// Gets or sets url used to load svg image used in horizontal "More Actions" toolbar buttons.
        /// </summary>
        public static string UrlImageMoreActionsHorz { get; set; } =
            GetImageUrl("alternet-ellipsis");

        /// <summary>
        /// Gets or sets url used to load regular file icon.
        /// </summary>
        public static string UrlIconFile { get; set; } = GetImageUrl("alternet-file");

        /// <summary>
        /// Gets or sets url used to load solid file icon.
        /// </summary>
        public static string UrlIconFileSolid { get; set; } = GetImageUrl("alternet-file-solid");

        /// <summary>
        /// Gets or sets url used to load regular folder icon.
        /// </summary>
        public static string UrlIconFolder { get; set; } = GetImageUrl("alternet-folder");

        /// <summary>
        /// Gets or sets url used to load solid folder icon.
        /// </summary>
        public static string UrlIconFolderSolid { get; set; } = GetImageUrl("alternet-folder-solid");

        /// <summary>
        /// Gets or sets the URL of the image used for the left sizing grip.
        /// </summary>
        public static string UrlImageSizingGripLeft { get; set; }
            = GetImageUrl("alternet-sizinggrip-left");

        /// <summary>
        /// Gets or sets the URL of the image used for the right sizing grip.
        /// </summary>
        public static string UrlImageSizingGripRight { get; set; }
            = GetImageUrl("alternet-sizinggrip-right");

        /// <summary>
        /// Gets or sets the URL of the image with the bars.
        /// </summary>
        public static string UrlImageBars { get; set; }
            = GetImageUrl("alternet-bars");

        /// <summary>
        /// Gets or sets url used to load "Filter" svg image.
        /// </summary>
        public static string UrlImageFilter { get; set; } = GetImageUrl("alternet-filter");

        private static string GetImageUrl(string name) => string.Format(ResTemplate, name);
    }
}