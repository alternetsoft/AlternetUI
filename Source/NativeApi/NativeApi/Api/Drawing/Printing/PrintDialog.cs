using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class PrintDialog
    {
        public bool AllowSomePages { get => throw new Exception(); set => throw new Exception(); }
        public bool AllowSelection { get => throw new Exception(); set => throw new Exception(); }
        public bool AllowPrintToFile { get => throw new Exception(); set => throw new Exception(); }
        public bool ShowHelp { get => throw new Exception(); set => throw new Exception(); }
        public PrintDocument? Document { get => throw new Exception(); set => throw new Exception(); }
        public ModalResult ShowModal(Window? owner) => throw new Exception();
    }
}