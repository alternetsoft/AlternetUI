using NativeApi.Api.ManagedServers;
using System;
using System.Drawing;

namespace NativeApi.Api
{
    public class Image
    {
        public void LoadFromStream(InputStream stream) => throw new Exception();

        public SizeF Size { get; }
        public Size PixelSize { get; }
    }
}