#pragma warning disable
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Label : Control
    {
        public string Text { get; set; }

        public bool IsEllipsized() => false;

        public void Wrap(int width) { }
    }
}