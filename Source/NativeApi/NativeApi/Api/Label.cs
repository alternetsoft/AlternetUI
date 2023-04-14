using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Label : Control
    {
        public string Text { get => throw new Exception(); set => throw new Exception(); }

        public bool IsEllipsized() => throw new Exception();

        public void Wrap(int width) => throw new Exception();
    }
}