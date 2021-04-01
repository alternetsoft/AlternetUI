using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class StackLayoutPanel : Control, IDisposable
    {
        void IDisposable.Dispose() => throw new Exception();

        public StackLayoutOrientation Orientation { get; set; }
    }
}