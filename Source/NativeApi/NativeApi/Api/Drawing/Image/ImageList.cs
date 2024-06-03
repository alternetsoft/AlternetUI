using NativeApi.Api.ManagedServers;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class ImageList
    {
        public void AddImage(Image image) => throw new Exception();

        public SizeI PixelImageSize { get; set; }

        public SizeD ImageSize { get; set; }

        // Removes the image at the given index.
        public bool Remove(int index) => default;

        // Remove all images
        public bool Clear() => default;
    }
}