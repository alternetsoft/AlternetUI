using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Panel : Control, IDisposable
    {
        void IDisposable.Dispose() => throw new Exception();
    }
}