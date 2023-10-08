using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;
using System.Reflection;

namespace ControlsSample
{
    internal static class ResourceLoader
    {
        private static ImageLists? imageLists;

        public static ImageLists LoadImageLists()
        {
            imageLists ??= LoadImageListsCore();

            return imageLists;
        }

        private static ControlStateImages? buttonImages;

        public static ControlStateImages ButtonImages => buttonImages ??= LoadButtonImages();

        private static ControlStateImages LoadButtonImages()
        {
            static Image LoadImage(string stateName) =>
                new Bitmap(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(
                        $"ControlsSample.Resources.ButtonImages.ButtonImage{stateName}.png")
                    ?? throw new Exception());

            return new ControlStateImages
            {
                NormalImage = LoadImage("Normal"),
                HoveredImage = LoadImage("Hovered"),
                PressedImage = LoadImage("Pressed"),
                DisabledImage = LoadImage("Disabled"),
				FocusedImage = LoadImage("Focused"),
            };
        }

        private static ImageLists LoadImageListsCore()
        {
            var smallImageList = new ImageList();
            var largeImageList = new ImageList() { ImageSize = new Size(32, 32) };

            var assembly = Assembly.GetExecutingAssembly();
            var allResourceNames = assembly.GetManifestResourceNames();
            var allImageResourceNames =
                allResourceNames.Where(x => x.StartsWith("ControlsSample.Resources.ImageListIcons."));
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