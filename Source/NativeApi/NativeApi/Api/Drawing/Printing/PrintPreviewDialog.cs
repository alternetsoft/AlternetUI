using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class PrintPreviewDialog
    {
        public ModalResult ShowModal(Window? owner) => throw new Exception();
        public PrintDocument? Document { get => throw new Exception(); set => throw new Exception(); }
    }
}