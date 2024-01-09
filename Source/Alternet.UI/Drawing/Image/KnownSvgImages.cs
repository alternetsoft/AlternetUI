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
    public class KnownSvgImages
    {
        private static readonly AdvDictionary<SizeI, List<KnownSvgImages>> Images = [];

        private readonly SizeI size;
        private readonly Color color;

        private ImageSet? imgBrowserBack;
        private ImageSet? imgBrowserForward;
        private ImageSet? imgZoomIn;
        private ImageSet? imgZoomOut;
        private ImageSet? imgBrowserGo;
        private ImageSet? imgAdd;
        private ImageSet? imgMoreActions;
        private ImageSet? imgMoreActionsHorz;
        private ImageSet? imgRemove;
        private ImageSet? imgOk;
        private ImageSet? imgCancel;
        private ImageSet? imgAddChild;
        private ImageSet? imgRemoveAll;
        private ImageSet? imgMessageBoxError;
        private ImageSet? imgMessageBoxInformation;
        private ImageSet? imgMessageBoxWarning;
        private ImageSet? imgFileNew;
        private ImageSet? imgFileOpen;
        private ImageSet? imgFileSave;
        private ImageSet? imgBold;
        private ImageSet? imgItalic;
        private ImageSet? imgUnderline;
        private ImageSet? imgUndo;
        private ImageSet? imgRedo;
        private ImageSet? imgSquarePlus;
        private ImageSet? imgSquareMinus;
        private ImageSet? imgYes;
        private ImageSet? imgNo;
        private ImageSet? imgAbort;
        private ImageSet? imgRetry;
        private ImageSet? imgIgnore;
        private ImageSet? imgHelp;
        private ImageSet? imgAngleDown;
        private ImageSet? imgAngleUp;
        private ImageSet? imgArrowDown;
        private ImageSet? imgArrowUp;
        private ImageSet? imgGear;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownSvgImages"/> class.
        /// </summary>
        /// <param name="size">Images size.</param>
        /// <param name="color">Images color.</param>
        public KnownSvgImages(Color color, SizeI size)
        {
            this.size = size;
            this.color = color;
        }

        /// <summary>
        /// Gets images color.
        /// </summary>
        public Color Color => color;

        /// <summary>
        /// Gets images size.
        /// </summary>
        public SizeI Size => size;

        /// <summary>
        /// Gets or sets 'Arrow Down' image.
        /// </summary>
        public ImageSet ImgArrowDown
        {
            get => imgArrowDown ??= Load(KnownSvgUrls.UrlImageArrowDown);
            set => imgArrowDown = value;
        }

        /// <summary>
        /// Gets or sets 'Arrow Up' image.
        /// </summary>
        public ImageSet ImgArrowUp
        {
            get => imgArrowUp ??= Load(KnownSvgUrls.UrlImageArrowUp);
            set => imgArrowUp = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Up' image.
        /// </summary>
        public ImageSet ImgAngleUp
        {
            get => imgAngleUp ??= Load(KnownSvgUrls.UrlImageAngleUp);
            set => imgAngleUp = value;
        }

        /// <summary>
        /// Gets or sets 'Angle Up' image.
        /// </summary>
        public ImageSet ImgAngleDown
        {
            get => imgAngleDown ??= Load(KnownSvgUrls.UrlImageAngleDown);
            set => imgAngleDown = value;
        }

        /// <summary>
        /// Gets or sets 'Gear' image.
        /// </summary>
        public ImageSet ImgGear
        {
            get => imgGear ??= Load(KnownSvgUrls.UrlImageGear);
            set => imgGear = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Back" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public ImageSet ImgBrowserBack
        {
            get => imgBrowserBack ??= Load(KnownSvgUrls.UrlImageWebBrowserBack);
            set => imgBrowserBack = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Forward" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public ImageSet ImgBrowserForward
        {
            get => imgBrowserForward ??= Load(KnownSvgUrls.UrlImageWebBrowserForward);
            set => imgBrowserForward = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Error" sign.
        /// </summary>
        public ImageSet ImgMessageBoxError
        {
            get => imgMessageBoxError ??= Load(KnownSvgUrls.UrlImageMessageBoxError);
            set => imgMessageBoxError = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Information" sign.
        /// </summary>
        public ImageSet ImgMessageBoxInformation
        {
            get => imgMessageBoxInformation ??= Load(KnownSvgUrls.UrlImageMessageBoxInformation);
            set => imgMessageBoxInformation = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|New" toolbar items.
        /// </summary>
        public ImageSet ImgFileNew
        {
            get => imgFileNew ??= Load(KnownSvgUrls.UrlImageFileNew);
            set => imgFileNew = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|Save" toolbar items.
        /// </summary>
        public ImageSet ImgFileSave
        {
            get => imgFileSave ??= Load(KnownSvgUrls.UrlImageFileSave);
            set => imgFileSave = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Plus Inside Square' image.
        /// </summary>
        public ImageSet ImgSquarePlus
        {
            get => imgSquarePlus ??= Load(KnownSvgUrls.UrlImageSquarePlus);
            set => imgSquarePlus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used as 'Minus Inside Square' image.
        /// </summary>
        public ImageSet ImgSquareMinus
        {
            get => imgSquareMinus ??= Load(KnownSvgUrls.UrlImageSquareMinus);
            set => imgSquareMinus = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "File|Open" toolbar items.
        /// </summary>
        public ImageSet ImgFileOpen
        {
            get => imgFileOpen ??= Load(KnownSvgUrls.UrlImageFileOpen);
            set => imgFileOpen = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in MessageBox like dialogs as "Warning" sign.
        /// </summary>
        public ImageSet ImgMessageBoxWarning
        {
            get => imgMessageBoxWarning ??= Load(KnownSvgUrls.UrlImageMessageBoxWarning);
            set => imgMessageBoxWarning = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom in" toolbar buttons.
        /// </summary>
        public ImageSet ImgZoomIn
        {
            get => imgZoomIn ??= Load(KnownSvgUrls.UrlImageZoomIn);
            set => imgZoomIn = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom out" toolbar buttons.
        /// </summary>
        public ImageSet ImgZoomOut
        {
            get => imgZoomOut ??= Load(KnownSvgUrls.UrlImageZoomOut);
            set => imgZoomOut = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Go" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public ImageSet ImgBrowserGo
        {
            get => imgBrowserGo ??= Load(KnownSvgUrls.UrlImageWebBrowserGo);
            set => imgBrowserGo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add" toolbar buttons.
        /// </summary>
        public ImageSet ImgAdd
        {
            get => imgAdd ??= Load(KnownSvgUrls.UrlImagePlus);
            set => imgAdd = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Yes" buttons.
        /// </summary>
        public ImageSet? ImgYes
        {
            get => imgYes ??= Load(KnownSvgUrls.UrlImageYes);
            set => imgYes = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "No" buttons.
        /// </summary>
        public ImageSet? ImgNo
        {
            get => imgNo ??= Load(KnownSvgUrls.UrlImageNo);
            set => imgNo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Abort" buttons.
        /// </summary>
        public ImageSet? ImgAbort
        {
            get => imgAbort ??= LoadIfExists(KnownSvgUrls.UrlImageAbort);
            set => imgAbort = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Retry" buttons.
        /// </summary>
        public ImageSet? ImgRetry
        {
            get => imgRetry ??= LoadIfExists(KnownSvgUrls.UrlImageRetry);
            set => imgRetry = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Ignore" buttons.
        /// </summary>
        public ImageSet? ImgIgnore
        {
            get => imgIgnore ??= LoadIfExists(KnownSvgUrls.UrlImageIgnore);
            set => imgIgnore = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Help" buttons.
        /// </summary>
        public ImageSet? ImgHelp
        {
            get => imgHelp ??= LoadIfExists(KnownSvgUrls.UrlImageHelp);
            set => imgHelp = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "More Actions" toolbar buttons.
        /// </summary>
        public ImageSet ImgMoreActions
        {
            get => imgMoreActions ??= Load(KnownSvgUrls.UrlImageMoreActions);
            set => imgMoreActions = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in horizontal "More Actions" toolbar buttons.
        /// </summary>
        public ImageSet ImgMoreActionsHorz
        {
            get => imgMoreActionsHorz ??= Load(KnownSvgUrls.UrlImageMoreActionsHorz);
            set => imgMoreActionsHorz = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public ImageSet ImgRemove
        {
            get => imgRemove ??= Load(KnownSvgUrls.UrlImageMinus);
            set => imgRemove = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Ok" buttons.
        /// </summary>
        public ImageSet ImgOk
        {
            get => imgOk ??= Load(KnownSvgUrls.UrlImageOk);
            set => imgOk = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Cancel" buttons.
        /// </summary>
        public ImageSet ImgCancel
        {
            get => imgCancel ??= Load(KnownSvgUrls.UrlImageCancel);
            set => imgCancel = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add child" toolbar buttons.
        /// </summary>
        public ImageSet ImgAddChild
        {
            get => imgAddChild ??= Load(KnownSvgUrls.UrlImageAddChild);
            set => imgAddChild = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public ImageSet ImgRemoveAll
        {
            get => imgRemoveAll ??= Load(KnownSvgUrls.UrlImageRemoveAll);
            set => imgRemoveAll = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Undo" toolbar buttons.
        /// </summary>
        public ImageSet ImgUndo
        {
            get => imgUndo ??= Load(KnownSvgUrls.UrlImageUndo);
            set => imgUndo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Redo" toolbar buttons.
        /// </summary>
        public ImageSet ImgRedo
        {
            get => imgRedo ??= Load(KnownSvgUrls.UrlImageRedo);
            set => imgRedo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Bold" toolbar buttons.
        /// </summary>
        public ImageSet ImgBold
        {
            get => imgBold ??= Load(KnownSvgUrls.UrlImageBold);
            set => imgBold = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Italic" toolbar buttons.
        /// </summary>
        public ImageSet ImgItalic
        {
            get => imgItalic ??= Load(KnownSvgUrls.UrlImageItalic);
            set => imgItalic = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Underline" toolbar buttons.
        /// </summary>
        public ImageSet ImgUnderline
        {
            get => imgUnderline ??= Load(KnownSvgUrls.UrlImageUnderline);
            set => imgUnderline = value;
        }

        /// <summary>
        /// Gets <see cref="KnownSvgImages"/> for the specified bitmap size.
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <param name="color">Image color.</param>
        public static KnownSvgImages GetForSize(Color color, SizeI size)
        {
            var images = Images.GetOrCreate(size, () => new List<KnownSvgImages>());
            foreach(var item in images)
            {
                if (item.Color.EqualARGB(color))
                    return item;
            }

            var result = new KnownSvgImages(color, size);
            images.Add(result);
            return result;
        }

        /// <summary>
        /// Gets <see cref="KnownSvgImages"/> for the specified bitmap size.
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <param name="color">Image color.</param>
        public static KnownSvgImages GetForSize(Color color, int size)
            => GetForSize(color, new SizeI(size));

        /// <summary>
        /// Gets <see cref="KnownSvgImages.ImgMessageBoxWarning"/> image for the specified bitmap size.
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <param name="color">Image color.</param>
        public static Image? GetWarningImage(Color color, int? size = null)
        {
            size ??= Toolbar.GetDefaultImageSize().Width;
            var imageSet = GetForSize(color, size.Value).ImgMessageBoxWarning;
            var image = imageSet.AsImage(size.Value);
            return image;
        }

        /// <summary>
        /// Gets all images in <see cref="KnownSvgImages"/>.
        /// </summary>
        public IEnumerable<ImageSet> GetAllImages()
        {
            List<ImageSet> result = [];

            var props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var p in props)
            {
                if (p.PropertyType != typeof(ImageSet))
                    continue;

                if (p.GetValue(this) is not ImageSet value)
                    continue;
                result.Add(value);
            }

            return result;
        }

        private ImageSet? LoadIfExists(string? url)
        {
            if (url is null)
                return null;
            return AuiToolbar.LoadSvgImage(url, size, color);
        }

        private ImageSet Load(string url)
        {
            return AuiToolbar.LoadSvgImage(url, size, color);
        }
    }
}