using NativeApi.Api.ManagedServers;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class Image
    {
        public void LoadFromStream(InputStream stream) => throw new Exception();
        public void Initialize(Size size) => throw new Exception();


        public Size Size { get; }
        public Int32Size PixelSize { get; }
    }
}