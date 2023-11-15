#pragma warning disable
using NativeApi.Api.ManagedServers;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class Image
    {
        public void LoadFromStream(InputStream stream) { }
        public void LoadSvgFromStream(InputStream stream, int width, int height, Color color) { }
        public void Initialize(Size size) { }
        public void CopyFrom(Image otherImage) { }

        public void SaveToStream(OutputStream stream, string format) { }
        public void SaveToFile(string fileName) { }

        public Size Size { get; }
        public Int32Size PixelSize { get; }

        public bool GrayScale() => throw new Exception();
    }
}