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
        private static SvgImage? imgYes;
        private static SvgImage? imgNo;
        private static SvgImage? imgAbort;
        private static SvgImage? imgRetry;
        private static SvgImage? imgIgnore;
        private static SvgImage? imgHelp;
        private static SvgImage? imgAngleDown;
        private static SvgImage? imgAngleUp;
        private static SvgImage? imgArrowDown;
        private static SvgImage? imgArrowUp;
        private static SvgImage? imgGear;
        private static SvgImage? imgCircle;
        private static SvgImage? imgRegularExpr;
        private static SvgImage? imgFindMatchCase;
        private static SvgImage? imgFindMatchFullWord;
        private static SvgImage? imgEmpty;
        private static SvgImage? imgReplace;
        private static SvgImage? imgReplaceAll;

        /// <summary>
        /// Gets or sets image that can be used in "Ok" buttons.
        /// </summary>
        public static SvgImage ImgOk
        {
            get => imgOk ??= new(KnownSvgUrls.UrlImageOk);
            set => imgOk = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Down' image.
        /// </summary>
        public static SvgImage ImgArrowDown
        {
            get => imgArrowDown ??= new(KnownSvgUrls.UrlImageArrowDown);
            set => imgArrowDown = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Up' image.
        /// </summary>
        public static SvgImage ImgArrowUp
        {
            get => imgArrowUp ??= new(KnownSvgUrls.UrlImageArrowUp);
            set => imgArrowUp = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Up' image.
        /// </summary>
        public static SvgImage ImgAngleUp
        {
            get => imgAngleUp ??= new(KnownSvgUrls.UrlImageAngleUp);
            set => imgAngleUp = value;
        }

        /// <summary>
        /// Gets or sets 'Replace' image.
        /// </summary>
        public static SvgImage ImgReplace
        {
            get => imgReplace ??= new(KnownSvgUrls.UrlImageReplace);
            set => imgReplace = value;
        }

        /// <summary>
        /// Gets or sets 'Replace All' image.
        /// </summary>
        public static SvgImage ImgReplaceAll
        {
            get => imgReplaceAll ??= new(KnownSvgUrls.UrlImageReplaceAll);
            set => imgReplaceAll = value;
        }

        /// <summary>
        /// Gets or sets 'Use Regular Exprpressions' image.
        /// </summary>
        public static SvgImage ImgRegularExpr
        {
            get => imgRegularExpr ??= new(KnownSvgUrls.UrlImageRegularExpr);
            set => imgRegularExpr = value;
        }

        /// <summary>
        /// Gets or sets 'Find Match Case' image.
        /// </summary>
        public static SvgImage ImgFindMatchCase
        {
            get => imgFindMatchCase ??= new(KnownSvgUrls.UrlImageFindMatchCase);
            set => imgFindMatchCase = value;
        }

        /// <summary>
        /// Gets or sets 'Find Match Full Word' image.
        /// </summary>
        public static SvgImage ImgFindMatchFullWord
        {
            get => imgFindMatchFullWord ??= new(KnownSvgUrls.UrlImageFindMatchFullWord);
            set => imgFindMatchFullWord = value;
        }

        /// <summary>
        /// Gets or sets empty image.
        /// </summary>
        public static SvgImage ImgEmpty
        {
            get => imgEmpty ??= new(KnownSvgUrls.UrlImageEmpty);
            set => imgEmpty = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Up' image.
        /// </summary>
        public static SvgImage ImgAngleDown
        {
            get => imgAngleDown ??= new(KnownSvgUrls.UrlImageAngleDown);
            set => imgAngleDown = value;
        }

        /// <summary>
        /// Gets or sets 'Gear' image.
        /// </summary>
        public static SvgImage ImgGear
        {
            get => imgGear ??= new(KnownSvgUrls.UrlImageGear);
            set => imgGear = value;
        }

        /// <summary>
        /// Gets or sets 'Circle' image.
        /// </summary>
        public static SvgImage ImgCircle
        {
            get => imgCircle ??= new(KnownSvgUrls.UrlImageCircle);
            set => imgCircle = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Back" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static SvgImage ImgBrowserBack
        {
            get => imgBrowserBack ??= new(KnownSvgUrls.UrlImageWebBrowserBack);
            set => imgBrowserBack = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Forward" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static SvgImage ImgBrowserForward
        {
            get => imgBrowserForward ??= new(KnownSvgUrls.UrlImageWebBrowserForward);
            set => imgBrowserForward = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Error" sign.
        /// </summary>
        public static SvgImage ImgMessageBoxError
        {
            get => imgMessageBoxError ??= new(KnownSvgUrls.UrlImageMessageBoxError);
            set => imgMessageBoxError = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Information" sign.
        /// </summary>
        public static SvgImage ImgMessageBoxInformation
        {
            get => imgMessageBoxInformation ??= new(KnownSvgUrls.UrlImageMessageBoxInformation);
            set => imgMessageBoxInformation = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|New" toolbar items.
        /// </summary>
        public static SvgImage ImgFileNew
        {
            get => imgFileNew ??= new(KnownSvgUrls.UrlImageFileNew);
            set => imgFileNew = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|Save" toolbar items.
        /// </summary>
        public static SvgImage ImgFileSave
        {
            get => imgFileSave ??= new(KnownSvgUrls.UrlImageFileSave);
            set => imgFileSave = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Plus Inside Square' image.
        /// </summary>
        public static SvgImage ImgSquarePlus
        {
            get => imgSquarePlus ??= new(KnownSvgUrls.UrlImageSquarePlus);
            set => imgSquarePlus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Minus Inside Square' image.
        /// </summary>
        public static SvgImage ImgSquareMinus
        {
            get => imgSquareMinus ??= new(KnownSvgUrls.UrlImageSquareMinus);
            set => imgSquareMinus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|Open" toolbar items.
        /// </summary>
        public static SvgImage ImgFileOpen
        {
            get => imgFileOpen ??= new(KnownSvgUrls.UrlImageFileOpen);
            set => imgFileOpen = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Warning" sign.
        /// </summary>
        public static SvgImage ImgMessageBoxWarning
        {
            get => imgMessageBoxWarning ??= new(KnownSvgUrls.UrlImageMessageBoxWarning);
            set => imgMessageBoxWarning = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom in" toolbar buttons.
        /// </summary>
        public static SvgImage ImgZoomIn
        {
            get => imgZoomIn ??= new(KnownSvgUrls.UrlImageZoomIn);
            set => imgZoomIn = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom out" toolbar buttons.
        /// </summary>
        public static SvgImage ImgZoomOut
        {
            get => imgZoomOut ??= new(KnownSvgUrls.UrlImageZoomOut);
            set => imgZoomOut = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Go" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public static SvgImage ImgBrowserGo
        {
            get => imgBrowserGo ??= new(KnownSvgUrls.UrlImageWebBrowserGo);
            set => imgBrowserGo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add" toolbar buttons.
        /// </summary>
        public static SvgImage ImgAdd
        {
            get => imgAdd ??= new(KnownSvgUrls.UrlImagePlus);
            set => imgAdd = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Yes" buttons.
        /// </summary>
        public static SvgImage ImgYes
        {
            get => imgYes ??= new(KnownSvgUrls.UrlImageYes);
            set => imgYes = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "No" buttons.
        /// </summary>
        public static SvgImage ImgNo
        {
            get => imgNo ??= new(KnownSvgUrls.UrlImageNo);
            set => imgNo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Abort" buttons.
        /// </summary>
        public static SvgImage ImgAbort
        {
            get => imgAbort ??= new(KnownSvgUrls.UrlImageAbort);
            set => imgAbort = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Retry" buttons.
        /// </summary>
        public static SvgImage ImgRetry
        {
            get => imgRetry ??= new(KnownSvgUrls.UrlImageRetry);
            set => imgRetry = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Ignore" buttons.
        /// </summary>
        public static SvgImage ImgIgnore
        {
            get => imgIgnore ??= new(KnownSvgUrls.UrlImageIgnore);
            set => imgIgnore = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Help" buttons.
        /// </summary>
        public static SvgImage ImgHelp
        {
            get => imgHelp ??= new(KnownSvgUrls.UrlImageHelp);
            set => imgHelp = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "More Actions" toolbar buttons.
        /// </summary>
        public static SvgImage ImgMoreActions
        {
            get => imgMoreActions ??= new(KnownSvgUrls.UrlImageMoreActions);
            set => imgMoreActions = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in horizontal "More Actions" toolbar buttons.
        /// </summary>
        public static SvgImage ImgMoreActionsHorz
        {
            get => imgMoreActionsHorz ??= new(KnownSvgUrls.UrlImageMoreActionsHorz);
            set => imgMoreActionsHorz = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public static SvgImage ImgRemove
        {
            get => imgRemove ??= new(KnownSvgUrls.UrlImageMinus);
            set => imgRemove = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Cancel" buttons.
        /// </summary>
        public static SvgImage ImgCancel
        {
            get => imgCancel ??= new(KnownSvgUrls.UrlImageCancel);
            set => imgCancel = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add child" toolbar buttons.
        /// </summary>
        public static SvgImage ImgAddChild
        {
            get => imgAddChild ??= new(KnownSvgUrls.UrlImageAddChild);
            set => imgAddChild = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public static SvgImage ImgRemoveAll
        {
            get => imgRemoveAll ??= new(KnownSvgUrls.UrlImageRemoveAll);
            set => imgRemoveAll = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Undo" toolbar buttons.
        /// </summary>
        public static SvgImage ImgUndo
        {
            get => imgUndo ??= new(KnownSvgUrls.UrlImageUndo);
            set => imgUndo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Redo" toolbar buttons.
        /// </summary>
        public static SvgImage ImgRedo
        {
            get => imgRedo ??= new(KnownSvgUrls.UrlImageRedo);
            set => imgRedo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Bold" toolbar buttons.
        /// </summary>
        public static SvgImage ImgBold
        {
            get => imgBold ??= new(KnownSvgUrls.UrlImageBold);
            set => imgBold = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Italic" toolbar buttons.
        /// </summary>
        public static SvgImage ImgItalic
        {
            get => imgItalic ??= new(KnownSvgUrls.UrlImageItalic);
            set => imgItalic = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Underline" toolbar buttons.
        /// </summary>
        public static SvgImage ImgUnderline
        {
            get => imgUnderline ??= new(KnownSvgUrls.UrlImageUnderline);
            set => imgUnderline = value;
        }

        /// <summary>
        /// Gets all images in <see cref="KnownSvgImages"/>.
        /// </summary>
        public static IEnumerable<SvgImage> GetAllImages()
        {
            List<SvgImage> result = new();

            var props = typeof(KnownSvgImages).GetProperties(
                BindingFlags.Instance | BindingFlags.Public);

            foreach (var p in props)
            {
                if (p.PropertyType != typeof(SvgImage))
                    continue;

                if (p.GetValue(null) is not SvgImage value)
                    continue;
                result.Add(value);
            }

            return result;
        }

        /// <summary>
        /// Gets <see cref="ImgAngleUp"/> or <see cref="ImgAngleDown"/> image.
        /// </summary>
        /// <param name="up">Up or Down image.</param>
        /// <returns></returns>
        public static SvgImage GetImgAngleUpDown(bool up) => up ? ImgAngleUp : ImgAngleDown;
     }
}