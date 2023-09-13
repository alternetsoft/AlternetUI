#pragma warning disable
using NativeApi.Api.ManagedServers;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class ImageSet
    {
        public void LoadFromStream(InputStream stream) { }
        public void AddImage(Image image) { }
        public void Clear() { }
        public bool IsOk { get; }
        public bool IsReadOnly { get; }
        public void LoadSvgFromStream(InputStream stream, int width, int height) { }
    }
}