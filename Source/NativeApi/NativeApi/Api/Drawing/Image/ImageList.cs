using NativeApi.Api.ManagedServers;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class ImageList
    {
        public void AddImage(Image image) => throw new Exception();

        public int PixelImageSizeX { get; }
        public int PixelImageSizeY { get; }

        public void SetImageSize(float sizeX, float sizeY) { }

        public void SetPixelImageSize(int sizeX, int sizeY) { }

        public float ImageSizeX { get; }
        public float ImageSizeY { get; }

        // Removes the image at the given index.
        public bool Remove(int index) => default;

        // Remove all images
        public bool Clear() => default;
    }
}