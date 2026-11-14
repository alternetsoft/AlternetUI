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

        private static SvgImage? imgAnglesDown;
        private static SvgImage? imgAnglesUp;
        private static SvgImage? imgAnglesLeft;
        private static SvgImage? imgAnglesRight;

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
        private static SvgImage? imgCheck;
        private static SvgImage? imgRightFromBracket;
        private static SvgImage? imgRightToBracket;
        private static SvgImage? imgWindowMaximize;
        private static SvgImage? imgWindowMinimize;
        private static SvgImage? imgWindowRestore;
        private static SvgImage? imgCircleCheckmark;
        private static SvgImage? imgClock;
        private static SvgImage? imgCalendar;
        private static SvgImage? imgCalendarCheck;
        private static SvgImage? imgRotate;
        private static SvgImage? imgArrowsSpin;
        private static SvgImage? imgBell;
        private static SvgImage? imgBackspace;
        private static SvgImage? imgAddressCard;
        private static SvgImage? imgFolderEllipsis;
        private static SvgImage? imgHourglassHalf;
        private static SvgImage? imgHourglass;
        private static SvgImage? imgSlash;
        private static SvgImage? imgSmallPen;
        private static SvgImage? imgArrowsSpinPencil;
        private static SvgImage? imgFormatAlignLeft;
        private static SvgImage? imgLocation;
        private static SvgImage? imgUser;
        private static SvgImage? imgWorkOutline;
        private static SvgImage? imgTag;
        private static SvgImage? imgPenToSquare;

        static KnownSvgImages()
        {
            SystemSettings.SystemColorsChanged += () =>
            {
                foreach (var image in GetAllAllocatedImages())
                {
                    image.ResetCachedImages();
                }
            };
        }

        /// <summary>
        /// Gets or sets "Arrows Spin Pencil" svg image which includes a pencil icon
        /// in the center of a spinning arrows icon.
        /// </summary>
        public static SvgImage ImgArrowsSpinPencil
        {
            get
            {
                return imgArrowsSpinPencil ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageArrowsSpinPencil);
            }

            set
            {
                imgArrowsSpinPencil = value;
            }
        }

        /// <summary>
        /// Gets or sets "Format Align Left" svg image which includes a left-aligned text icon.
        /// </summary>
        public static SvgImage ImgFormatAlignLeft
        {
            get
            {
                return imgFormatAlignLeft ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageFormatAlignLeft);
            }

            set
            {
                imgFormatAlignLeft = value;
            }
        }

        /// <summary>
        /// Gets or sets "Pen To Square" svg image which includes a pen icon pointing to a square icon.
        /// </summary>
        public static SvgImage ImgPenToSquare
        {
            get
            {
                return imgPenToSquare ??= CreateMonoSvgImage(KnownSvgUrls.UrlImagePenToSquare);
            }

            set
            {
                imgPenToSquare = value;
            }
        }

        /// <summary>
        /// Gets or sets "Tag" svg image which includes a tag icon.
        /// </summary>
        public static SvgImage ImgTag
        {
            get
            {
                return imgTag ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageTag);
            }

            set
            {
                imgTag = value;
            }
        }

        /// <summary>
        /// Gets or sets "Location" svg image which includes a location pin icon.
        /// </summary>
        public static SvgImage ImgLocation
        {
            get
            {
                return imgLocation ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageLocation);
            }

            set
            {
                imgLocation = value;
            }
        }

        /// <summary>
        /// Gets or sets "User" svg image which includes a user icon.
        /// </summary>
        public static SvgImage ImgUser
        {
            get
            {
                return imgUser ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageUser);
            }

            set
            {
                imgUser = value;
            }
        }

        /// <summary>
        /// Gets or sets "Work Outline" svg image which includes a briefcase icon.
        /// </summary>
        public static SvgImage ImgWorkOutline
        {
            get
            {
                return imgWorkOutline ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageWorkOutline);
            }

            set
            {
                imgWorkOutline = value;
            }
        }

        /// <summary>
        /// Gets or sets "Slash" svg image.
        /// </summary>
        public static SvgImage ImgSlash
        {
            get
            {
                return imgSlash ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSlash);
            }

            set
            {
                imgSlash = value;
            }
        }

        /// <summary>
        /// Gets or sets "Small Pen" svg image.
        /// </summary>
        public static SvgImage ImgSmallPen
        {
            get
            {
                return imgSmallPen ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSmallPen);
            }

            set
            {
                imgSmallPen = value;
            }
        }

        /// <summary>
        /// Gets or sets svg image which can be used
        /// for indicating a checked state of the menu item.
        /// </summary>
        public static SvgImage ImgCircleCheckmark
        {
            get => imgCircleCheckmark ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCircleCheckmark);
            set => imgCircleCheckmark = value;
        }

        /// <summary>
        /// Gets or sets svg image which can be used for indicating a folder selection action.
        /// </summary>
        public static SvgImage ImgFolderEllipsis
        {
            get => imgFolderEllipsis ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageFolderEllipsis);
            set => imgFolderEllipsis = value;
        }

        /// <summary>
        /// Gets or sets svg image which can be used for displaying a half-filled hourglass.
        /// </summary>
        public static SvgImage ImgHourglassHalf
        {
            get => imgHourglassHalf ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageHourglassHalf);
            set => imgHourglassHalf = value;
        }

        /// <summary>
        /// Gets or sets svg image which can be used for displaying a hourglass.
        /// </summary>
        public static SvgImage ImgHourglass
        {
            get => imgHourglass ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageHourglass);
            set => imgHourglass = value;
        }

        /// <summary>
        /// Gets or sets svg image which can be used for indicating a backspace action.
        /// </summary>
        public static SvgImage ImgBackspace
        {
            get => imgBackspace ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageBackspace);
            set => imgBackspace = value;
        }

        /// <summary>
        /// Gets or sets svg image which can be used for indicating a bell icon, typically used for notifications or alerts.
        /// </summary>
        public static SvgImage ImgBell
        {
            get => imgBell ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageBell);
            set => imgBell = value;
        }

        /// <summary>
        /// Gets or sets "Window Maximize" svg image.
        /// </summary>
        public static SvgImage ImgWindowMaximize
        {
            get
            {
                return imgWindowMaximize ?? ImgSquare;
            }

            set
            {
                imgWindowMaximize = value;
            }
        }

        /// <summary>
        /// Gets or sets "Window Minimize" svg image.
        /// </summary>
        public static SvgImage ImgWindowMinimize
        {
            get
            {
                return imgWindowMinimize ?? ImgMinus;
            }
            set
            {
                imgWindowMinimize = value;
            }
        }

        /// <summary>
        /// Gets or sets "Window Restore" svg image.
        /// </summary>
        public static SvgImage ImgWindowRestore
        {
            get
            {
                return imgWindowRestore ?? ImgSquare;
            }
            set
            {
                imgWindowRestore = value;
            }
        }

        /// <summary>
        /// Gets or sets the SVG image with an arrows pointing in a circular direction,
        /// typically used to represent a "rotate" action.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgRotate
        {
            get => imgRotate ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageRotate);
            set => imgRotate = value;
        }

        /// <summary>
        /// Gets or sets the SVG image with a clock, typically used to represent time-related actions or information.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgClock
        {
            get => imgClock ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageClock);
            set => imgClock = value;
        }

        /// <summary>
        /// Gets or sets the SVG image with an address card, typically used to represent contact information or business cards.
        /// </summary>
        public static SvgImage ImgAddressCard
        {
            get => imgAddressCard ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAddressCard);
            set => imgAddressCard = value;
        }

        /// <summary>
        /// Gets or sets the SVG image with a calendar, typically used to represent date-related actions or information.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgCalendar
        {
            get => imgCalendar ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCalendar);
            set => imgCalendar = value;
        }

        /// <summary>
        /// Gets or sets the SVG image with a checked calendar.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgCalendarCheck
        {
            get => imgCalendarCheck ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCalendarCheck);
            set => imgCalendarCheck = value;
        }

        /// <summary>
        /// Gets or sets the SVG image with an arrow pointing from a bracket.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgRightFromBracket
        {
            get => imgRightFromBracket ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageRightFromBracket);
            set => imgRightFromBracket = value;
        }

        /// <summary>
        /// Gets or sets the SVG image with an arrow pointing to a bracket.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgRightToBracket
        {
            get => imgRightToBracket ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageRightToBracket);
            set => imgRightToBracket = value;
        }

        /// <summary>
        /// Gets or sets the SVG image with the bars.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgBars
        {
            get => imgBars ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageBars);
            set => imgBars = value;
        }

        /// <summary>
        /// Gets or sets the SVG image used for the 'filter' icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgFilter
        {
            get => imgFilter ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageFilter);
            set => imgFilter = value;
        }

        /// <summary>
        /// Gets or sets the SVG image used to represent a check mark icon.
        /// </summary>
        public static SvgImage ImgCheck
        {
            get => imgCheck ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCheck);
            set => imgCheck = value;
        }

        /// <summary>
        /// Gets or sets the SVG image used for the left sizing grip.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgSizingGripLeft
        {
            get => imgSizingGripLeft ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSizingGripLeft);
            set => imgSizingGripLeft = value;
        }

        /// <summary>
        /// Gets or sets the SVG image used for the right sizing grip.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgSizingGripRight
        {
            get => imgSizingGripRight ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSizingGripRight);
            set => imgSizingGripRight = value;
        }

        /// <summary>
        /// Gets or sets image for the regular file icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgIconFile
        {
            get => imgIconFile ??= CreateMonoSvgImage(KnownSvgUrls.UrlIconFile);
            set => imgIconFile = value;
        }

        /// <summary>
        /// Gets or sets image for the solid file icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgIconFileSolid
        {
            get => imgIconFileSolid ??= CreateMonoSvgImage(KnownSvgUrls.UrlIconFileSolid);
            set => imgIconFileSolid = value;
        }

        /// <summary>
        /// Gets or sets image for the solid folder icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgIconFolderSolid
        {
            get => imgIconFolderSolid ??= CreateMonoSvgImage(KnownSvgUrls.UrlIconFolderSolid);
            set => imgIconFolderSolid = value;
        }

        /// <summary>
        /// Gets or sets image for the regular folder icon.
        /// </summary>
        /// <remarks>The default value is an SVG image loaded from a predefined URL. Assign a custom value
        /// to override the default icon.</remarks>
        public static SvgImage ImgIconFolder
        {
            get => imgIconFolder ??= CreateMonoSvgImage(KnownSvgUrls.UrlIconFolder);
            set => imgIconFolder = value;
        }

        /// <summary>
        /// Gets or sets 'Eye On' image.
        /// </summary>
        public static SvgImage ImgEyeOn
        {
            get => imgEyeOn ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageEyeOn);
            set => imgEyeOn = value;
        }

        /// <summary>
        /// Gets or sets 'Search' image.
        /// </summary>
        public static SvgImage ImgSearch
        {
            get => imgSearch ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSearch);
            set => imgSearch = value;
        }

        /// <summary>
        /// Gets or sets 'Eye Off' image.
        /// </summary>
        public static SvgImage ImgEyeOff
        {
            get => imgEyeOff ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageEyeOff);
            set => imgEyeOff = value;
        }

        /// <summary>
        /// Gets or sets 'Circle Filled' image.
        /// </summary>
        public static SvgImage ImgCircleFilled
        {
            get => imgCircleFilled ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCircleFilled);
            set => imgCircleFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Debug Run' image.
        /// </summary>
        public static SvgImage ImgDebugRun
        {
            get => imgDebugRun ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageDebugRun);
            set => imgDebugRun = value;
        }

        /// <summary>
        /// Gets or sets 'Copy' image.
        /// </summary>
        public static SvgImage ImgCopy
        {
            get => imgCopy ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCopy);
            set => imgCopy = value;
        }

        /// <summary>
        /// Gets or sets 'Cut' image.
        /// </summary>
        public static SvgImage ImgCut
        {
            get => imgCut ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCut);
            set => imgCut = value;
        }

        /// <summary>
        /// Gets or sets 'Trash Can' image.
        /// </summary>
        public static SvgImage ImgTrashCan
        {
            get => imgTrashCan ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageTrashCan);
            set => imgTrashCan = value;
        }

        /// <summary>
        /// Gets or sets 'Paste' image.
        /// </summary>
        public static SvgImage ImgPaste
        {
            get => imgPaste ??= CreateMonoSvgImage(KnownSvgUrls.UrlImagePaste);
            set => imgPaste = value;
        }

        /// <summary>
        /// Gets or sets 'Diamond Filled' image.
        /// </summary>
        public static SvgImage ImgDiamondFilled
        {
            get => imgDiamondFilled ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageDiamondFilled);
            set => imgDiamondFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Keyboard' image.
        /// </summary>
        public static SvgImage ImgKeyboard
        {
            get => imgKeyboard ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageKeyboard);
            set => imgKeyboard = value;
        }

        /// <summary>
        /// Gets or sets 'Triangle Arrow Down' image.
        /// </summary>
        public static SvgImage ImgTriangleArrowDown
        {
            get => imgTriangleArrowDown ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageTriangleArrowDown);
            set => imgTriangleArrowDown = value;
        }

        /// <summary>
        /// Gets or sets 'Triangle Arrow Up' image.
        /// </summary>
        public static SvgImage ImgTriangleArrowUp
        {
            get => imgTriangleArrowUp ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageTriangleArrowUp);
            set => imgTriangleArrowUp = value;
        }

        /// <summary>
        /// Gets or sets 'Triangle Arrow Left' image.
        /// </summary>
        public static SvgImage ImgTriangleArrowLeft
        {
            get => imgTriangleArrowLeft ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageTriangleArrowLeft);
            set => imgTriangleArrowLeft = value;
        }

        /// <summary>
        /// Gets or sets 'Triangle Arrow Right' image.
        /// </summary>
        public static SvgImage ImgTriangleArrowRight
        {
            get => imgTriangleArrowRight ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageTriangleArrowRight);
            set => imgTriangleArrowRight = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Ok" buttons.
        /// </summary>
        public static SvgImage ImgOk
        {
            get => imgOk ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageOk);
            set => imgOk = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Down' image.
        /// </summary>
        public static SvgImage ImgArrowDown
        {
            get => imgArrowDown ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageArrowDown);
            set => imgArrowDown = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Right' image.
        /// </summary>
        public static SvgImage ImgArrowRight
        {
            get => imgArrowRight ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageArrowRight);
            set => imgArrowRight = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Left' image.
        /// </summary>
        public static SvgImage ImgArrowLeft
        {
            get => imgArrowLeft ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageArrowLeft);
            set => imgArrowLeft = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Up' image.
        /// </summary>
        public static SvgImage ImgArrowUp
        {
            get => imgArrowUp ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageArrowUp);
            set => imgArrowUp = value;
        }

        /// <summary>
        /// Gets or sets 'Replace' image.
        /// </summary>
        public static SvgImage ImgReplace
        {
            get => imgReplace ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageReplace);
            set => imgReplace = value;
        }

        /// <summary>
        /// Gets or sets 'Replace All' image.
        /// </summary>
        public static SvgImage ImgReplaceAll
        {
            get => imgReplaceAll ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageReplaceAll);
            set => imgReplaceAll = value;
        }

        /// <summary>
        /// Gets or sets 'Use Regular Expressions' image.
        /// </summary>
        public static SvgImage ImgRegularExpr
        {
            get => imgRegularExpr ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageRegularExpr);
            set => imgRegularExpr = value;
        }

        /// <summary>
        /// Gets or sets 'Find Match Case' image.
        /// </summary>
        public static SvgImage ImgFindMatchCase
        {
            get => imgFindMatchCase ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageFindMatchCase);
            set => imgFindMatchCase = value;
        }

        /// <summary>
        /// Gets or sets 'Find Match Full Word' image.
        /// </summary>
        public static SvgImage ImgFindMatchFullWord
        {
            get => imgFindMatchFullWord ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageFindMatchFullWord);
            set => imgFindMatchFullWord = value;
        }

        /// <summary>
        /// Gets or sets empty image.
        /// </summary>
        public static SvgImage ImgEmpty
        {
            get => imgEmpty ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageEmpty);
            set => imgEmpty = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Up' image.
        /// </summary>
        public static SvgImage ImgAngleDown
        {
            get => imgAngleDown ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAngleDown);
            set => imgAngleDown = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Up' image.
        /// </summary>
        public static SvgImage ImgAngleUp
        {
            get => imgAngleUp ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAngleUp);
            set => imgAngleUp = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Right' image.
        /// </summary>
        public static SvgImage ImgAngleRight
        {
            get => imgAngleRight ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAngleRight);
            set => imgAngleRight = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Left' image.
        /// </summary>
        public static SvgImage ImgAngleLeft
        {
            get => imgAngleLeft ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAngleLeft);
            set => imgAngleLeft = value;
        }

        /// <summary>
        /// Gets or sets 'Gear' image.
        /// </summary>
        public static SvgImage ImgGear
        {
            get => imgGear ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageGear);
            set => imgGear = value;
        }

        /// <summary>
        /// Gets or sets 'Circle' image.
        /// </summary>
        public static SvgImage ImgCircle
        {
            get => imgCircle ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCircle);
            set => imgCircle = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Back" toolbar buttons
        /// for the web browser control.
        /// </summary>
        public static SvgImage ImgBrowserBack
        {
            get => imgBrowserBack ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageWebBrowserBack);
            set => imgBrowserBack = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Forward" toolbar buttons
        /// for the web browser control.
        /// </summary>
        public static SvgImage ImgBrowserForward
        {
            get => imgBrowserForward ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageWebBrowserForward);
            set => imgBrowserForward = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Error" sign.
        /// </summary>
        public static SvgImage ImgMessageBoxError
        {
            get => imgMessageBoxError ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageMessageBoxError);
            set => imgMessageBoxError = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Information" sign.
        /// </summary>
        public static SvgImage ImgMessageBoxInformation
        {
            get => imgMessageBoxInformation ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageMessageBoxInformation);
            set => imgMessageBoxInformation = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|New" toolbar items.
        /// </summary>
        public static SvgImage ImgFileNew
        {
            get => imgFileNew ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageFileNew);
            set => imgFileNew = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|Save" toolbar items.
        /// </summary>
        public static SvgImage ImgFileSave
        {
            get => imgFileSave ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageFileSave);
            set => imgFileSave = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as plus image.
        /// </summary>
        public static SvgImage ImgPlus
        {
            get => imgPlus ??= CreateMonoSvgImage(KnownSvgUrls.UrlImagePlus);
            set => imgPlus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Minus' image.
        /// </summary>
        public static SvgImage ImgMinus
        {
            get => imgMinus ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageMinus);
            set => imgMinus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Plus Inside Square' image.
        /// </summary>
        public static SvgImage ImgSquarePlus
        {
            get => imgSquarePlus ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSquarePlus);
            set => imgSquarePlus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Minus Inside Square' image.
        /// </summary>
        public static SvgImage ImgSquareMinus
        {
            get => imgSquareMinus ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSquareMinus);
            set => imgSquareMinus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|Open" toolbar items.
        /// </summary>
        public static SvgImage ImgFileOpen
        {
            get => imgFileOpen ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageFileOpen);
            set => imgFileOpen = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Warning" sign.
        /// </summary>
        public static SvgImage ImgMessageBoxWarning
        {
            get => imgMessageBoxWarning ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageMessageBoxWarning);
            set => imgMessageBoxWarning = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom in" toolbar buttons.
        /// </summary>
        public static SvgImage ImgZoomIn
        {
            get => imgZoomIn ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageZoomIn);
            set => imgZoomIn = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom out" toolbar buttons.
        /// </summary>
        public static SvgImage ImgZoomOut
        {
            get => imgZoomOut ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageZoomOut);
            set => imgZoomOut = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Go" toolbar buttons
        /// for the web browser control.
        /// </summary>
        public static SvgImage ImgBrowserGo
        {
            get => imgBrowserGo ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageWebBrowserGo);
            set => imgBrowserGo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add" toolbar buttons.
        /// </summary>
        public static SvgImage ImgAdd
        {
            get => imgAdd ??= CreateMonoSvgImage(KnownSvgUrls.UrlImagePlus);
            set => imgAdd = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Yes" buttons.
        /// </summary>
        public static SvgImage ImgYes
        {
            get => imgYes ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageYes);
            set => imgYes = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "No" buttons.
        /// </summary>
        public static SvgImage ImgNo
        {
            get => imgNo ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageNo);
            set => imgNo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Abort" buttons.
        /// </summary>
        public static SvgImage ImgAbort
        {
            get => imgAbort ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAbort);
            set => imgAbort = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Retry" buttons.
        /// </summary>
        public static SvgImage ImgRetry
        {
            get => imgRetry ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageRetry);
            set => imgRetry = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Retry" buttons.
        /// </summary>
        public static SvgImage ImgArrowsSpin
        {
            get => imgArrowsSpin ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageArrowsSpin);
            set => imgArrowsSpin = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Ignore" buttons.
        /// </summary>
        public static SvgImage ImgIgnore
        {
            get => imgIgnore ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageIgnore);
            set => imgIgnore = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Help" buttons.
        /// </summary>
        public static SvgImage ImgHelp
        {
            get => imgHelp ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageHelp);
            set => imgHelp = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "More Actions" toolbar buttons.
        /// </summary>
        public static SvgImage ImgMoreActions
        {
            get => imgMoreActions ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageMoreActions);
            set => imgMoreActions = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in horizontal "More Actions" toolbar buttons.
        /// </summary>
        public static SvgImage ImgMoreActionsHorz
        {
            get => imgMoreActionsHorz ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageMoreActionsHorz);
            set => imgMoreActionsHorz = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public static SvgImage ImgPaintBrush
        {
            get => imgPaintBrush ??= CreateMonoSvgImage(KnownSvgUrls.UrlImagePaintBrush);
            set => imgPaintBrush = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public static SvgImage ImgRemove
        {
            get => imgRemove ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageMinus);
            set => imgRemove = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Cancel" buttons.
        /// </summary>
        public static SvgImage ImgCancel
        {
            get => imgCancel ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCancel);
            set => imgCancel = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Close" buttons.
        /// </summary>
        public static SvgImage ImgClose
        {
            get => imgClose ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageClose);
            set => imgClose = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add child" toolbar buttons.
        /// </summary>
        public static SvgImage ImgAddChild
        {
            get => imgAddChild ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAddChild);
            set => imgAddChild = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public static SvgImage ImgRemoveAll
        {
            get => imgRemoveAll ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageRemoveAll);
            set => imgRemoveAll = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Undo" toolbar buttons.
        /// </summary>
        public static SvgImage ImgUndo
        {
            get => imgUndo ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageUndo);
            set => imgUndo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Redo" toolbar buttons.
        /// </summary>
        public static SvgImage ImgRedo
        {
            get => imgRedo ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageRedo);
            set => imgRedo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Bold" toolbar buttons.
        /// </summary>
        public static SvgImage ImgBold
        {
            get => imgBold ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageBold);
            set => imgBold = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Italic" toolbar buttons.
        /// </summary>
        public static SvgImage ImgItalic
        {
            get => imgItalic ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageItalic);
            set => imgItalic = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Underline" toolbar buttons.
        /// </summary>
        public static SvgImage ImgUnderline
        {
            get => imgUnderline ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageUnderline);
            set => imgUnderline = value;
        }

        /// <summary>
        /// Gets or sets 'Square Check Filled' image.
        /// </summary>
        public static SvgImage ImgSquareCheckFilled
        {
            get => imgSquareCheckFilled ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSquareCheckFilled);
            set => imgSquareCheckFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Square Minus Filled' image.
        /// </summary>
        public static SvgImage ImgSquareMinusFilled
        {
            get => imgSquareMinusFilled ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSquareMinusFilled);
            set => imgSquareMinusFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Square Plus Filled' image.
        /// </summary>
        public static SvgImage ImgSquarePlusFilled
        {
            get => imgSquarePlusFilled ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSquarePlusFilled);
            set => imgSquarePlusFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Square' image.
        /// </summary>
        public static SvgImage ImgSquare
        {
            get => imgSquare ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageSquare);
            set => imgSquare = value;
        }

        /// <summary>
        /// Gets or sets 'Circle Dot Filled' image.
        /// </summary>
        public static SvgImage ImgCircleDotFilled
        {
            get => imgCircleDotFilled ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCircleDotFilled);
            set => imgCircleDotFilled = value;
        }

        /// <summary>
        /// Gets or sets 'Circle Dot' image.
        /// </summary>
        public static SvgImage ImgCircleDot
        {
            get => imgCircleDot ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageCircleDot);
            set => imgCircleDot = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Down' image.
        /// </summary>
        public static SvgImage ImgAnglesDown
        {
            get => imgAnglesDown ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAnglesDown);
            set => imgAnglesDown = value;
        }

        /// <summary>
        /// Gets or sets 'Angles Up' image.
        /// </summary>
        public static SvgImage ImgAnglesUp
        {
            get => imgAnglesUp ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAnglesUp);
            set => imgAnglesUp = value;
        }

        /// <summary>
        /// Gets or sets 'Angles Right' image.
        /// </summary>
        public static SvgImage ImgAnglesRight
        {
            get => imgAnglesRight ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAnglesRight);
            set => imgAnglesRight = value;
        }

        /// <summary>
        /// Gets or sets 'Angles Left' image.
        /// </summary>
        public static SvgImage ImgAnglesLeft
        {
            get => imgAnglesLeft ??= CreateMonoSvgImage(KnownSvgUrls.UrlImageAnglesLeft);
            set => imgAnglesLeft = value;
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
        /// Creates a <see cref="MonoSvgImage"/> instance from the specified URL.
        /// </summary>
        /// <param name="url">The URL of the SVG image.</param>
        /// <returns>A <see cref="MonoSvgImage"/> instance.</returns>
        public static MonoSvgImage CreateMonoSvgImage(string url)
        {
            return new MonoSvgImage(url, SvgImageDataKind.Url, throwException: false);
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