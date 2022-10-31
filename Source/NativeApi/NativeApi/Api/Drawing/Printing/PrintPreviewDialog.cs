using System;

namespace NativeApi.Api
{
    public class PrintPreviewDialog
    {
        public string? Title { get; set; }
        public void Show(Window? owner) => throw new Exception();
        public PrintDocument? Document { get => throw new Exception(); set => throw new Exception(); }
    }
}