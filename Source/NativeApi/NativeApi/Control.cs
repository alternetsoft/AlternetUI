using System;
using ApiCommon;

namespace NativeApi.Api
{
    [Api]
    public abstract class Control : IDisposable
    {
        void IDisposable.Dispose() => throw new NotImplementedException();
    }
}