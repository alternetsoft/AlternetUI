using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class PreviewInBrowser : Control, IFilePreview
    {
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

        public PreviewInBrowser()
        {
            browser.Parent = this;
        }

        public string? FileName
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

        public static bool IsSupportedFile(string fileName)
        {
            var ext = PathUtils.GetExtensionLower(fileName);

            var equals = EnumerableUtils.EqualsRange(ext, SupportedExtensions);

            return equals;
        }

        public static IFilePreview CreatePreviewControl()
        {
            return new PreviewInBrowser();
        }

        public void Reload()
        {
            if(FileName is null)
            {
                browser.Url = "about: blank";
                return;
            }

            var url = WebBrowser.PrepareFileUrl(FileName);

            browser.Url = url;
        }

        public Control Control { get => this; }
    }
}
