using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements preview control which uses <see cref="WebBrowser"/> for the preview
    /// of the files.
    /// </summary>
    public partial class PreviewInBrowser : HiddenBorder, IFilePreview
    {
        /// <summary>
        /// Gets or sets list of file extension which
        /// are supported in <see cref="PreviewInBrowser"/>.
        /// </summary>
        /// <remarks>
        /// Extensions must be in the lower case and without "." character.
        /// </remarks>
        public static List<string> SupportedExtensions = new()
        {
            "html",
            "htm",
            "gif",
            "png",
            "svg",
            "bmp",
            "mp3",
            "wav",
            "jpg",
            "jpeg",
        };

        private readonly WebBrowser browser = new()
        {
            HasBorder = false,
        };

        private string? fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewInBrowser"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PreviewInBrowser(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewInBrowser"/> class.
        /// </summary>
        public PreviewInBrowser()
        {
            browser.Parent = this;
        }

        /// <summary>
        /// <inheritdoc cref="IFilePreview.FileName"/>
        /// </summary>
        public virtual string? FileName
        {
            get => fileName;
            set
            {
                if (fileName == value)
                    return;
                fileName = value;
                Reload();
            }
        }

        /// <summary>
        /// <inheritdoc cref="IFilePreview.Control"/>
        /// </summary>
        public AbstractControl Control { get => this; }

        /// <summary>
        /// Gets whether specified file is supported in this preview control.
        /// </summary>
        /// <param name="fileName">Path to file.</param>
        /// <returns></returns>
        public static bool IsSupportedFile(string fileName)
        {
            var ext = PathUtils.GetExtensionLower(fileName);

            var equals = EnumerableUtils.EqualsRange(ext, SupportedExtensions);

            return equals;
        }

        /// <summary>
        /// Creates this preview control.
        /// </summary>
        /// <returns></returns>
        public static IFilePreview CreatePreviewControl()
        {
            return new PreviewInBrowser();
        }

        /// <summary>
        /// Reloads the file which is currently previewed.
        /// </summary>
        public virtual void Reload()
        {
            if(FileName is null)
            {
                browser.Url = "about: blank";
                return;
            }

            var url = WebBrowser.PrepareFileUrl(FileName);

            browser.Url = url;
        }
    }
}
