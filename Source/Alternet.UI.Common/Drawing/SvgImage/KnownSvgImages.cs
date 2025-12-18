using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to get known svg images as <see cref="ImageSet"/> instances.
    /// </summary>
    public static class KnownSvgImages
    {
        private static SvgImage? imgPaintBrush;
        private static SvgImage? imgDebugRun;
        private static SvgImage? imgTrashCan;
        private static SvgImage? imgCopy;
        private static SvgImage? imgCut;
        private static SvgImage? imgPaste;
        private static SvgImage? imgOk;
        private static SvgImage? imgBrowserBack;
        private static SvgImage? imgBrowserForward;
        private static SvgImage? imgZoomIn;
        private static SvgImage? imgZoomOut;
        private static SvgImage? imgBrowserGo;
        private static SvgImage? imgAdd;
        private static SvgImage? imgMoreActions;
        private static SvgImage? imgMoreActionsHorz;
        private static SvgImage? imgRemove;
        private static SvgImage? imgCancel;
        private static SvgImage? imgClose;
        private static SvgImage? imgAddChild;
        private static SvgImage? imgRemoveAll;
        private static SvgImage? imgMessageBoxError;
        private static SvgImage? imgMessageBoxInformation;
        private static SvgImage? imgMessageBoxWarning;
        private static SvgImage? imgFileNew;
        private static SvgImage? imgFileOpen;
        private static SvgImage? imgFileSave;
        private static SvgImage? imgBold;
        private static SvgImage? imgItalic;
        private static SvgImage? imgUnderline;
        private static SvgImage? imgUndo;
        private static SvgImage? imgRedo;
        private static SvgImage? imgSquarePlus;
        private static SvgImage? imgSquareMinus;
        private static SvgImage? imgPlus;
        private static SvgImage? imgMinus;
        private static SvgImage? imgYes;
        private static SvgImage? imgNo;
        private static SvgImage? imgAbort;
        private static SvgImage? imgRetry;
        private static SvgImage? imgIgnore;
        private static SvgImage? imgHelp;

        private static SvgImage? imgAngleDown;
        private static SvgImage? imgAngleUp;
        private static SvgImage? imgAngleLeft;
        private static SvgImage? imgAngleRight;

        private static SvgImage? imgArrowDown;
        private static SvgImage? imgArrowUp;
        private static SvgImage? imgArrowRight;
        private static SvgImage? imgArrowLeft;

        private static SvgImage? imgGear;
        private static SvgImage? imgCircle;
        private static SvgImage? imgRegularExpr;
        private static SvgImage? imgFindMatchCase;
        private static SvgImage? imgFindMatchFullWord;
        private static SvgImage? imgEmpty;
        private static SvgImage? imgReplace;
        private static SvgImage? imgReplaceAll;
        private static SvgImage? imgTriangleArrowDown;
        private static SvgImage? imgTriangleArrowUp;
        private static SvgImage? imgTriangleArrowLeft;
        private static SvgImage? imgTriangleArrowRight;
        private static SvgImage? imgKeyboard;
        private static SvgImage? imgCircleFilled;
        private static SvgImage? imgDiamondFilled;
        private static SvgImage? imgEyeOn;
        private static SvgImage? imgEyeOff;
        private static SvgImage? imgSearch;

        private static SvgImage? imgIconFile;
        private static SvgImage? imgIconFileSolid;
        private static SvgImage? imgIconFolder;
        private static SvgImage? imgIconFolderSolid;

        private static SvgImage? imgSquareCheckFilled;
        private static SvgImage? imgSquareMinusFilled;
        private static SvgImage? imgSquarePlusFilled;
        private static SvgImage? imgSquare;
        private static SvgImage? imgCircleDotFilled;
        private static SvgImage? imgCircleDot;

        private static SvgImage? imgSizingGripLeft;
        private static SvgImage? imgSizingGripRight;
        private static SvgImage? imgBars;
        private static SvgImage? imgFilter;

        static KnownSvgImages()
        {
            SystemSettings.SystemColorsChanged += () =>
            {
                foreach(var image in GetAllAllocatedImages())
                {
                    image.ResetCachedImages();
                }
            };
        }

        /// <summary>
        /// Gets or sets the SVG image with the bars.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgBars
        {
            get => imgBars ??= new MonoSvgImage(KnownSvgUrls.UrlImageBars);
            set => imgBars = value;
        }

        /// <summary>
        /// Gets or sets the SVG image used for the 'filter' icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgFilter
        {
            get => imgFilter ??= new MonoSvgImage(KnownSvgUrls.UrlImageFilter);
            set => imgFilter = value;
        }

        /// <summary>
        /// Gets or sets the SVG image used for the left sizing grip.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgSizingGripLeft
        {
            get => imgSizingGripLeft ??= new MonoSvgImage(KnownSvgUrls.UrlImageSizingGripLeft);
            set => imgSizingGripLeft = value;
        }

        /// <summary>
        /// Gets or sets the SVG image used for the right sizing grip.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgSizingGripRight
        {
            get => imgSizingGripRight ??= new MonoSvgImage(KnownSvgUrls.UrlImageSizingGripRight);
            set => imgSizingGripRight = value;
        }

        /// <summary>
        /// Gets or sets image for the regular file icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgIconFile
        {
            get => imgIconFile ??= new MonoSvgImage(KnownSvgUrls.UrlIconFile);
            set => imgIconFile = value;
        }

        /// <summary>
        /// Gets or sets image for the solid file icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgIconFileSolid
        {
            get => imgIconFileSolid ??= new MonoSvgImage(KnownSvgUrls.UrlIconFileSolid);
            set => imgIconFileSolid = value;
        }

        /// <summary>
        /// Gets or sets image for the solid folder icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgIconFolderSolid
        {
            get => imgIconFolderSolid ??= new MonoSvgImage(KnownSvgUrls.UrlIconFolderSolid);
            set => imgIconFolderSolid = value;
        }

        /// <summary>
        /// Gets or sets image for the regular folder icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgIconFolder
        {
            get => imgIconFolder ??= new MonoSvgImage(KnownSvgUrls.UrlIconFolder);
            set => imgIconFolder = value;
        }

        /// <summary>
        /// Gets or sets 'Eye On' image.
        /// </summary>
        public static SvgImage ImgEyeOn
        {
            get => imgEyeOn ??= new MonoSvgImage(KnownSvgUrls.UrlImageEyeOn);
            set => imgEyeOn = value;
        }

        /// <summary>
        /// Gets or sets 'Search' image.
        /// </summary>
        public static SvgImage ImgSearch
        {
            get => imgSearch ??= new MonoSvgImage(KnownSvgUrls.UrlImageSearch);
            set => imgSearch = value;
        }

        /// <summary>
        /// Gets or sets 'Eye Off' image.
        /// </summary>
        public static SvgImage ImgEyeOff
        {
            get => imgEyeOff ??= new MonoSvgImage(KnownSvgUrls.UrlImageEyeOff);
            set => imgEyeOff = value;
        }

        /// <summary>
        /// Gets or sets 'Circle Filled' image.
        /// </summary>
        public static SvgImage ImgCircleFilled
        {
            get => imgCircleFilled ??= new MonoSvgImage(KnownSvgUrls.UrlImageCircleFilled);
            set => imgCircleFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Debug Run' image.
        /// </summary>
        public static SvgImage ImgDebugRun
        {
            get => imgDebugRun ??= new MonoSvgImage(KnownSvgUrls.UrlImageDebugRun);
            set => imgDebugRun = value;
        }

        /// <summary>
        /// Gets or sets 'Copy' image.
        /// </summary>
        public static SvgImage ImgCopy
        {
            get => imgCopy ??= new MonoSvgImage(KnownSvgUrls.UrlImageCopy);
            set => imgCopy = value;
        }

        /// <summary>
        /// Gets or sets 'Cut' image.
        /// </summary>
        public static SvgImage ImgCut
        {
            get => imgCut ??= new MonoSvgImage(KnownSvgUrls.UrlImageCut);
            set => imgCut = value;
        }

        /// <summary>
        /// Gets or sets 'Trash Can' image.
        /// </summary>
        public static SvgImage ImgTrashCan
        {
            get => imgTrashCan ??= new MonoSvgImage(KnownSvgUrls.UrlImageTrashCan);
            set => imgTrashCan = value;
        }

        /// <summary>
        /// Gets or sets 'Paste' image.
        /// </summary>
        public static SvgImage ImgPaste
        {
            get => imgPaste ??= new MonoSvgImage(KnownSvgUrls.UrlImagePaste);
            set => imgPaste = value;
        }

        /// <summary>
        /// Gets or sets 'Diamond Filled' image.
        /// </summary>
        public static SvgImage ImgDiamondFilled
        {
            get => imgDiamondFilled ??= new MonoSvgImage(KnownSvgUrls.UrlImageDiamondFilled);
            set => imgDiamondFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Keyboard' image.
        /// </summary>
        public static SvgImage ImgKeyboard
        {
            get => imgKeyboard ??= new MonoSvgImage(KnownSvgUrls.UrlImageKeyboard);
            set => imgKeyboard = value;
        }

        /// <summary>
        /// Gets or sets 'Triangle Arrow Down' image.
        /// </summary>
        public static SvgImage ImgTriangleArrowDown
        {
            get => imgTriangleArrowDown ??= new MonoSvgImage(KnownSvgUrls.UrlImageTriangleArrowDown);
            set => imgTriangleArrowDown = value;
        }

        /// <summary>
        /// Gets or sets 'Triangle Arrow Up' image.
        /// </summary>
        public static SvgImage ImgTriangleArrowUp
        {
            get => imgTriangleArrowUp ??= new MonoSvgImage(KnownSvgUrls.UrlImageTriangleArrowUp);
            set => imgTriangleArrowUp = value;
        }

        /// <summary>
        /// Gets or sets 'Triangle Arrow Left' image.
        /// </summary>
        public static SvgImage ImgTriangleArrowLeft
        {
            get => imgTriangleArrowLeft ??= new MonoSvgImage(KnownSvgUrls.UrlImageTriangleArrowLeft);
            set => imgTriangleArrowLeft = value;
        }

        /// <summary>
        /// Gets or sets 'Triangle Arrow Right' image.
        /// </summary>
        public static SvgImage ImgTriangleArrowRight
        {
            get => imgTriangleArrowRight ??= new MonoSvgImage(KnownSvgUrls.UrlImageTriangleArrowRight);
            set => imgTriangleArrowRight = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Ok" buttons.
        /// </summary>
        public static SvgImage ImgOk
        {
            get => imgOk ??= new MonoSvgImage(KnownSvgUrls.UrlImageOk);
            set => imgOk = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Down' image.
        /// </summary>
        public static SvgImage ImgArrowDown
        {
            get => imgArrowDown ??= new MonoSvgImage(KnownSvgUrls.UrlImageArrowDown);
            set => imgArrowDown = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Right' image.
        /// </summary>
        public static SvgImage ImgArrowRight
        {
            get => imgArrowRight ??= new MonoSvgImage(KnownSvgUrls.UrlImageArrowRight);
            set => imgArrowRight = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Left' image.
        /// </summary>
        public static SvgImage ImgArrowLeft
        {
            get => imgArrowLeft ??= new MonoSvgImage(KnownSvgUrls.UrlImageArrowLeft);
            set => imgArrowLeft = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Up' image.
        /// </summary>
        public static SvgImage ImgArrowUp
        {
            get => imgArrowUp ??= new MonoSvgImage(KnownSvgUrls.UrlImageArrowUp);
            set => imgArrowUp = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Up' image.
        /// </summary>
        public static SvgImage ImgAngleUp
        {
            get => imgAngleUp ??= new MonoSvgImage(KnownSvgUrls.UrlImageAngleUp);
            set => imgAngleUp = value;
        }

        /// <summary>
        /// Gets or sets 'Replace' image.
        /// </summary>
        public static SvgImage ImgReplace
        {
            get => imgReplace ??= new MonoSvgImage(KnownSvgUrls.UrlImageReplace);
            set => imgReplace = value;
        }

        /// <summary>
        /// Gets or sets 'Replace All' image.
        /// </summary>
        public static SvgImage ImgReplaceAll
        {
            get => imgReplaceAll ??= new MonoSvgImage(KnownSvgUrls.UrlImageReplaceAll);
            set => imgReplaceAll = value;
        }

        /// <summary>
        /// Gets or sets 'Use Regular Expressions' image.
        /// </summary>
        public static SvgImage ImgRegularExpr
        {
            get => imgRegularExpr ??= new MonoSvgImage(KnownSvgUrls.UrlImageRegularExpr);
            set => imgRegularExpr = value;
        }

        /// <summary>
        /// Gets or sets 'Find Match Case' image.
        /// </summary>
        public static SvgImage ImgFindMatchCase
        {
            get => imgFindMatchCase ??= new MonoSvgImage(KnownSvgUrls.UrlImageFindMatchCase);
            set => imgFindMatchCase = value;
        }

        /// <summary>
        /// Gets or sets 'Find Match Full Word' image.
        /// </summary>
        public static SvgImage ImgFindMatchFullWord
        {
            get => imgFindMatchFullWord ??= new MonoSvgImage(KnownSvgUrls.UrlImageFindMatchFullWord);
            set => imgFindMatchFullWord = value;
        }

        /// <summary>
        /// Gets or sets empty image.
        /// </summary>
        public static SvgImage ImgEmpty
        {
            get => imgEmpty ??= new MonoSvgImage(KnownSvgUrls.UrlImageEmpty);
            set => imgEmpty = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Up' image.
        /// </summary>
        public static SvgImage ImgAngleDown
        {
            get => imgAngleDown ??= new MonoSvgImage(KnownSvgUrls.UrlImageAngleDown);
            set => imgAngleDown = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Right' image.
        /// </summary>
        public static SvgImage ImgAngleRight
        {
            get => imgAngleRight ??= new MonoSvgImage(KnownSvgUrls.UrlImageAngleRight);
            set => imgAngleRight = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Left' image.
        /// </summary>
        public static SvgImage ImgAngleLeft
        {
            get => imgAngleLeft ??= new MonoSvgImage(KnownSvgUrls.UrlImageAngleLeft);
            set => imgAngleLeft = value;
        }

        /// <summary>
        /// Gets or sets 'Gear' image.
        /// </summary>
        public static SvgImage ImgGear
        {
            get => imgGear ??= new MonoSvgImage(KnownSvgUrls.UrlImageGear);
            set => imgGear = value;
        }

        /// <summary>
        /// Gets or sets 'Circle' image.
        /// </summary>
        public static SvgImage ImgCircle
        {
            get => imgCircle ??= new MonoSvgImage(KnownSvgUrls.UrlImageCircle);
            set => imgCircle = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Back" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static SvgImage ImgBrowserBack
        {
            get => imgBrowserBack ??= new MonoSvgImage(KnownSvgUrls.UrlImageWebBrowserBack);
            set => imgBrowserBack = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Forward" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static SvgImage ImgBrowserForward
        {
            get => imgBrowserForward ??= new MonoSvgImage(KnownSvgUrls.UrlImageWebBrowserForward);
            set => imgBrowserForward = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Error" sign.
        /// </summary>
        public static SvgImage ImgMessageBoxError
        {
            get => imgMessageBoxError ??= new MonoSvgImage(KnownSvgUrls.UrlImageMessageBoxError);
            set => imgMessageBoxError = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Information" sign.
        /// </summary>
        public static SvgImage ImgMessageBoxInformation
        {
            get => imgMessageBoxInformation ??= new MonoSvgImage(KnownSvgUrls.UrlImageMessageBoxInformation);
            set => imgMessageBoxInformation = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|New" toolbar items.
        /// </summary>
        public static SvgImage ImgFileNew
        {
            get => imgFileNew ??= new MonoSvgImage(KnownSvgUrls.UrlImageFileNew);
            set => imgFileNew = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|Save" toolbar items.
        /// </summary>
        public static SvgImage ImgFileSave
        {
            get => imgFileSave ??= new MonoSvgImage(KnownSvgUrls.UrlImageFileSave);
            set => imgFileSave = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as plus image.
        /// </summary>
        public static SvgImage ImgPlus
        {
            get => imgPlus ??= new MonoSvgImage(KnownSvgUrls.UrlImagePlus);
            set => imgPlus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Minus' image.
        /// </summary>
        public static SvgImage ImgMinus
        {
            get => imgMinus ??= new MonoSvgImage(KnownSvgUrls.UrlImageMinus);
            set => imgMinus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Plus Inside Square' image.
        /// </summary>
        public static SvgImage ImgSquarePlus
        {
            get => imgSquarePlus ??= new MonoSvgImage(KnownSvgUrls.UrlImageSquarePlus);
            set => imgSquarePlus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Minus Inside Square' image.
        /// </summary>
        public static SvgImage ImgSquareMinus
        {
            get => imgSquareMinus ??= new MonoSvgImage(KnownSvgUrls.UrlImageSquareMinus);
            set => imgSquareMinus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|Open" toolbar items.
        /// </summary>
        public static SvgImage ImgFileOpen
        {
            get => imgFileOpen ??= new MonoSvgImage(KnownSvgUrls.UrlImageFileOpen);
            set => imgFileOpen = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Warning" sign.
        /// </summary>
        public static SvgImage ImgMessageBoxWarning
        {
            get => imgMessageBoxWarning ??= new MonoSvgImage(KnownSvgUrls.UrlImageMessageBoxWarning);
            set => imgMessageBoxWarning = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom in" toolbar buttons.
        /// </summary>
        public static SvgImage ImgZoomIn
        {
            get => imgZoomIn ??= new MonoSvgImage(KnownSvgUrls.UrlImageZoomIn);
            set => imgZoomIn = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom out" toolbar buttons.
        /// </summary>
        public static SvgImage ImgZoomOut
        {
            get => imgZoomOut ??= new MonoSvgImage(KnownSvgUrls.UrlImageZoomOut);
            set => imgZoomOut = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Go" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static SvgImage ImgBrowserGo
        {
            get => imgBrowserGo ??= new MonoSvgImage(KnownSvgUrls.UrlImageWebBrowserGo);
            set => imgBrowserGo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add" toolbar buttons.
        /// </summary>
        public static SvgImage ImgAdd
        {
            get => imgAdd ??= new MonoSvgImage(KnownSvgUrls.UrlImagePlus);
            set => imgAdd = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Yes" buttons.
        /// </summary>
        public static SvgImage ImgYes
        {
            get => imgYes ??= new MonoSvgImage(KnownSvgUrls.UrlImageYes);
            set => imgYes = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "No" buttons.
        /// </summary>
        public static SvgImage ImgNo
        {
            get => imgNo ??= new MonoSvgImage(KnownSvgUrls.UrlImageNo);
            set => imgNo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Abort" buttons.
        /// </summary>
        public static SvgImage ImgAbort
        {
            get => imgAbort ??= new MonoSvgImage(KnownSvgUrls.UrlImageAbort);
            set => imgAbort = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Retry" buttons.
        /// </summary>
        public static SvgImage ImgRetry
        {
            get => imgRetry ??= new MonoSvgImage(KnownSvgUrls.UrlImageRetry);
            set => imgRetry = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Ignore" buttons.
        /// </summary>
        public static SvgImage ImgIgnore
        {
            get => imgIgnore ??= new MonoSvgImage(KnownSvgUrls.UrlImageIgnore);
            set => imgIgnore = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Help" buttons.
        /// </summary>
        public static SvgImage ImgHelp
        {
            get => imgHelp ??= new MonoSvgImage(KnownSvgUrls.UrlImageHelp);
            set => imgHelp = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "More Actions" toolbar buttons.
        /// </summary>
        public static SvgImage ImgMoreActions
        {
            get => imgMoreActions ??= new MonoSvgImage(KnownSvgUrls.UrlImageMoreActions);
            set => imgMoreActions = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in horizontal "More Actions" toolbar buttons.
        /// </summary>
        public static SvgImage ImgMoreActionsHorz
        {
            get => imgMoreActionsHorz ??= new MonoSvgImage(KnownSvgUrls.UrlImageMoreActionsHorz);
            set => imgMoreActionsHorz = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public static SvgImage ImgPaintBrush
        {
            get => imgPaintBrush ??= new MonoSvgImage(KnownSvgUrls.UrlImagePaintBrush);
            set => imgPaintBrush = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public static SvgImage ImgRemove
        {
            get => imgRemove ??= new MonoSvgImage(KnownSvgUrls.UrlImageMinus);
            set => imgRemove = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Cancel" buttons.
        /// </summary>
        public static SvgImage ImgCancel
        {
            get => imgCancel ??= new MonoSvgImage(KnownSvgUrls.UrlImageCancel);
            set => imgCancel = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Close" buttons.
        /// </summary>
        public static SvgImage ImgClose
        {
            get => imgClose ??= new MonoSvgImage(KnownSvgUrls.UrlImageClose);
            set => imgClose = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add child" toolbar buttons.
        /// </summary>
        public static SvgImage ImgAddChild
        {
            get => imgAddChild ??= new MonoSvgImage(KnownSvgUrls.UrlImageAddChild);
            set => imgAddChild = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public static SvgImage ImgRemoveAll
        {
            get => imgRemoveAll ??= new MonoSvgImage(KnownSvgUrls.UrlImageRemoveAll);
            set => imgRemoveAll = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Undo" toolbar buttons.
        /// </summary>
        public static SvgImage ImgUndo
        {
            get => imgUndo ??= new MonoSvgImage(KnownSvgUrls.UrlImageUndo);
            set => imgUndo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Redo" toolbar buttons.
        /// </summary>
        public static SvgImage ImgRedo
        {
            get => imgRedo ??= new MonoSvgImage(KnownSvgUrls.UrlImageRedo);
            set => imgRedo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Bold" toolbar buttons.
        /// </summary>
        public static SvgImage ImgBold
        {
            get => imgBold ??= new MonoSvgImage(KnownSvgUrls.UrlImageBold);
            set => imgBold = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Italic" toolbar buttons.
        /// </summary>
        public static SvgImage ImgItalic
        {
            get => imgItalic ??= new MonoSvgImage(KnownSvgUrls.UrlImageItalic);
            set => imgItalic = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Underline" toolbar buttons.
        /// </summary>
        public static SvgImage ImgUnderline
        {
            get => imgUnderline ??= new MonoSvgImage(KnownSvgUrls.UrlImageUnderline);
            set => imgUnderline = value;
        }

        /// <summary>
        /// Gets or sets 'Square Check Filled' image.
        /// </summary>
        public static SvgImage ImgSquareCheckFilled
        {
            get => imgSquareCheckFilled ??= new MonoSvgImage(KnownSvgUrls.UrlImageSquareCheckFilled);
            set => imgSquareCheckFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Square Minus Filled' image.
        /// </summary>
        public static SvgImage ImgSquareMinusFilled
        {
            get => imgSquareMinusFilled ??= new MonoSvgImage(KnownSvgUrls.UrlImageSquareMinusFilled);
            set => imgSquareMinusFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Square Plus Filled' image.
        /// </summary>
        public static SvgImage ImgSquarePlusFilled
        {
            get => imgSquarePlusFilled ??= new MonoSvgImage(KnownSvgUrls.UrlImageSquarePlusFilled);
            set => imgSquarePlusFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Square' image.
        /// </summary>
        public static SvgImage ImgSquare
        {
            get => imgSquare ??= new MonoSvgImage(KnownSvgUrls.UrlImageSquare);
            set => imgSquare = value;
        }

        /// <summary>
        /// Gets or sets 'Circle Dot Filled' image.
        /// </summary>
        public static SvgImage ImgCircleDotFilled
        {
            get => imgCircleDotFilled ??= new MonoSvgImage(KnownSvgUrls.UrlImageCircleDotFilled);
            set => imgCircleDotFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Circle Dot' image.
        /// </summary>
        public static SvgImage ImgCircleDot
        {
            get => imgCircleDot ??= new MonoSvgImage(KnownSvgUrls.UrlImageCircleDot);
            set => imgCircleDot = value;
        }

        /// <summary>
        /// Retrieves all allocated <see cref="SvgImage"/> instances defined
        /// in the <see cref="KnownSvgImages"/> class.
        /// </summary>
        /// <remarks>This method uses reflection to retrieve static fields of type <see cref="SvgImage"/>
        /// from the <see cref="KnownSvgImages"/> class.
        /// It is intended for scenarios where all allocated SVG images
        /// need to be enumerated.</remarks>
        /// <returns>An <see cref="IEnumerable{T}"/> containing
        /// all <see cref="SvgImage"/> objects which are allocated in static fields in
        /// the <see cref="KnownSvgImages"/> class. The collection
        /// will be empty if no such fields are allocated.</returns>
        public static IEnumerable<SvgImage> GetAllAllocatedImages()
        {
            return AssemblyUtils.GetStaticFields<SvgImage>(typeof(KnownSvgImages), true);
        }

        /// <summary>
        /// Gets all images in <see cref="KnownSvgImages"/>.
        /// </summary>
        public static IEnumerable<SvgImage> GetAllImages()
        {
            return AssemblyUtils.GetStaticProperties<SvgImage>(typeof(KnownSvgImages));
        }

        /// <summary>
        /// Gets <see cref="ImgAngleUp"/> or <see cref="ImgAngleDown"/> image.
        /// </summary>
        /// <param name="up">Up or Down image.</param>
        /// <returns></returns>
        public static SvgImage GetImgAngleUpDown(bool up) => up ? ImgAngleUp : ImgAngleDown;

        /// <summary>
        /// Gets triangle arrow image with the specified direction.
        /// </summary>
        /// <param name="direction">Arrow direction.</param>
        /// <returns></returns>
        public static SvgImage GetImgTriangleArrow(ArrowDirection direction)
        {
            switch (direction)
            {
                case ArrowDirection.Up:
                    return ImgTriangleArrowUp;
                case ArrowDirection.Down:
                    return ImgTriangleArrowDown;
                case ArrowDirection.Left:
                    return ImgTriangleArrowLeft;
                case ArrowDirection.Right:
                    return ImgTriangleArrowRight;
                default:
                    return ImgEmpty;
            }
        }

        /// <summary>
        /// Gets tree view button images based on the specified kind.
        /// </summary>
        /// <param name="kind">The kind of tree view buttons.</param>
        /// <returns>A tuple containing the closed and opened images for the tree view buttons.</returns>
        public static (SvgImage? Closed, SvgImage? Opened) GetTreeViewButtonImages(
            TreeViewButtonsKind kind)
        {
            switch (kind)
            {
                default:
                case TreeViewButtonsKind.Null:
                    return (null, null);
                case TreeViewButtonsKind.Empty:
                    return (KnownSvgImages.ImgEmpty, KnownSvgImages.ImgEmpty);
                case TreeViewButtonsKind.Arrow:
                    return (ImgArrowRight, ImgArrowDown);
                case TreeViewButtonsKind.Angle:
                    return (ImgAngleRight, ImgAngleDown);
                case TreeViewButtonsKind.TriangleArrow:
                    return (ImgTriangleArrowRight, ImgTriangleArrowDown);
                case TreeViewButtonsKind.PlusMinus:
                    return (KnownSvgImages.ImgPlus, KnownSvgImages.ImgMinus);
                case TreeViewButtonsKind.PlusMinusSquare:
                    return (KnownSvgImages.ImgSquarePlus, KnownSvgImages.ImgSquareMinus);
            }
        }
    }
}