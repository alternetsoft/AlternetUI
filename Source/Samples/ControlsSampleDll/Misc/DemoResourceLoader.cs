using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;
using System.Reflection;

namespace ControlsSample
{
    internal static class DemoResourceLoader
    {
        private static ImageLists? imageLists;

        public static ImageLists LoadImageLists()
        {
            imageLists ??= LoadImageListsCore();

            return imageLists;
        }

        internal static ControlStateImages LoadButtonImages(
            Control? control,
            string? url = null,
            SizeI? size = null)
        {
            url ??= "embres:ControlsSampleDll.Resources.ButtonImages.ButtonImage{0}.svg";

            Image LoadImage(string stateName, bool disabled = false)
            {
                Color? color = disabled ? SystemColors.GrayText : null;

                if(control is not null)
                {
                    bool isDark = control.RealBackgroundColor.IsDark();

                    if (disabled)
                        color = SvgColors.GetSvgColor(KnownSvgColor.Disabled, isDark);
                    else
                        color = SvgColors.GetSvgColor(KnownSvgColor.Normal, isDark);
                }

                var formattedUrl = string.Format(url, stateName);

                size ??= 16;

                return Image.FromSvgUrl(formattedUrl, size.Value.Width, size.Value.Height, color);
            }

            var normalImage = LoadImage("Normal");
            var disabledImage = LoadImage("Normal", true);

            return new ControlStateImages
            {
                Normal = normalImage,
                Hovered = LoadImage("Hovered"),
                Pressed = LoadImage("Pressed"),
                Disabled = disabledImage,
				Focused = LoadImage("Focused"),
            };
        }

        private static ImageLists LoadImageListsCore()
        {
            var smallImageList = new ImageList();
            var largeImageList = new ImageList() { ImageSize = new(32, 32) };

            var assembly = Assembly.GetExecutingAssembly();
            var allResourceNames = assembly.GetManifestResourceNames();
            var allImageResourceNames =
                allResourceNames.Where(x => x.StartsWith("ControlsSampleDll.Resources.ImageListIcons."));
            var smallImageResourceNames =
                allImageResourceNames.Where(x => x.Contains(".Small.")).ToArray();
            var largeImageResourceNames =
                allImageResourceNames.Where(x => x.Contains(".Large.")).ToArray();
            if (smallImageResourceNames.Length != largeImageResourceNames.Length)
                throw new Exception();

            Image LoadImage(string name) =>
                new Bitmap(assembly.GetManifestResourceStream(name) ?? throw new Exception());

            for (int i = 0; i < smallImageResourceNames.Length; i++)
            {
                smallImageList.Images.Add(LoadImage(smallImageResourceNames[i]));
                largeImageList.Images.Add(LoadImage(largeImageResourceNames[i]));
            }

            return new ImageLists(smallImageList, largeImageList);
        }

        public class ImageLists
        {
            public ImageLists(ImageList small, ImageList large)
            {
                Small = small;
                Large = large;
            }

            public ImageList Small { get; }

            public ImageList Large { get; }
        }
    }
}