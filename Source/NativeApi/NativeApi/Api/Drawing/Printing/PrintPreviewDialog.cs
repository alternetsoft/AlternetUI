#pragma warning disable
using System;

namespace NativeApi.Api
{
    public class PrintPreviewDialog
    {
        public NativeStringSpan GetTitle() => throw new Exception();
        public void SetTitle(NativeStringSpan value) => throw new Exception();
        public void ShowModal(Window? owner) => throw new Exception();
        public PrintDocument? Document { get => throw new Exception(); set => throw new Exception(); }
    }
}