using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class SelectDirectoryDialog
    {
        public ModalResult ShowModal(Window? owner) => throw new Exception();

        public NativeStringSpan GetInitialDirectory() => throw new Exception();
        public void SetInitialDirectory(NativeStringSpan value) => throw new Exception();

        public NativeStringSpan GetTitle() => throw new Exception();
        public void SetTitle(NativeStringSpan value) => throw new Exception();

        public NativeStringSpan GetDirectoryName() => throw new Exception();
        public void SetDirectoryName(NativeStringSpan value) => throw new Exception();
    }
}