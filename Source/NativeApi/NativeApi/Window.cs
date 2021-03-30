using System;
using ApiCommon;

namespace NativeApi.Api
{
    [Api]
    public class Window
    {
        public string Title { get => throw new Exception(); set => throw new Exception(); }

        public void Show() => throw new Exception();
    }
}