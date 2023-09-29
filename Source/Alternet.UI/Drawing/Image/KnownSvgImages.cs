using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly AdvDictionary<Int32Size, KnownSvgImages> Images = new();
        private ImageSet? imgBrowserBack;
        private ImageSet? imgBrowserForward;
        private ImageSet? imgZoomIn;
        private ImageSet? imgZoomOut;
        private ImageSet? imgBrowserGo;
        private ImageSet? imgAdd;
        private ImageSet? imgRemove;
        private ImageSet? imgOk;
        private ImageSet? imgCancel;
        private ImageSet? imgAddChild;
        private ImageSet? imgRemoveAll;

        private Int32Size size;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownSvgImages"/> class.
        /// </summary>
        /// <param name="size">Images size.</param>
        public KnownSvgImages(Int32Size size)
        {
            this.size = size;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Back" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public ImageSet ImgBrowserBack
        {
            get => imgBrowserBack ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImageWebBrowserBack, size);
            set => imgBrowserBack = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Forward" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public ImageSet ImgBrowserForward
        {
            get => imgBrowserForward ??=
                AuiToolbar.LoadSvgImage(SvgUtils.UrlImageWebBrowserForward, size);
            set => imgBrowserForward = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom in" toolbar buttons.
        /// </summary>
        public ImageSet ImgZoomIn
        {
            get => imgZoomIn ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImageZoomIn, size);
            set => imgZoomIn = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Zoom out" toolbar buttons.
        /// </summary>
        public ImageSet ImgZoomOut
        {
            get => imgZoomOut ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImageZoomOut, size);
            set => imgZoomOut = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Go" toolbar buttons
        /// for the <see cref="WebBrowser"/>.
        /// </summary>
        public ImageSet ImgBrowserGo
        {
            get => imgBrowserGo ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImageWebBrowserGo, size);
            set => imgBrowserGo = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add" toolbar buttons.
        /// </summary>
        public ImageSet ImgAdd
        {
            get => imgAdd ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImagePlus, size);
            set => imgAdd = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public ImageSet ImgRemove
        {
            get => imgRemove ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImageMinus, size);
            set => imgRemove = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Ok" buttons.
        /// </summary>
        public ImageSet ImgOk
        {
            get => imgOk ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImageOk, size);
            set => imgOk = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Cancel" buttons.
        /// </summary>
        public ImageSet ImgCancel
        {
            get => imgCancel ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImageCancel, size);
            set => imgCancel = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Add child" toolbar buttons.
        /// </summary>
        public ImageSet ImgAddChild
        {
            get => imgAddChild ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImageAddChild, size);
            set => imgAddChild = value;
        }

        /// <summary>
        /// Gets or sets image that can be used in "Remove" toolbar buttons.
        /// </summary>
        public ImageSet ImgRemoveAll
        {
            get => imgRemoveAll ??= AuiToolbar.LoadSvgImage(SvgUtils.UrlImageRemoveAll, size);
            set => imgRemoveAll = value;
        }

        /// <summary>
        /// Gets <see cref="KnownSvgImages"/> for the specified bitmap size.
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <returns></returns>
        public static KnownSvgImages GetForSize(Int32Size size)
        {
            var images = Images.GetOrCreate(size, () => new KnownSvgImages(size));
            return images;
        }
    }
}