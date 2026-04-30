using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class ColorDialog
    {
        public ModalResult ShowModal(Window? owner) => throw new Exception();

        public void SetColor(Color color) => throw new Exception();

        public byte GetColorR() => throw new Exception();
        public byte GetColorG() => throw new Exception();
        public byte GetColorB() => throw new Exception();
        public byte GetColorA() => throw new Exception();
        public byte GetColorState() => throw new Exception();

        public string? Title { get => throw new Exception(); set => throw new Exception(); }
    }
}