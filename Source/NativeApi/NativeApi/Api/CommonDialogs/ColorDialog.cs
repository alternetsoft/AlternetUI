using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class ColorDialog
    {
        public ModalResult ShowModal(Window? owner) => throw new Exception();

        public Color Color { get => throw new Exception(); set => throw new Exception(); }

        public string? Title { get => throw new Exception(); set => throw new Exception(); }
    }
}