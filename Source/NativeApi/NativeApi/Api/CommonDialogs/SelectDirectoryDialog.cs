using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class SelectDirectoryDialog
    {
        public ModalResult ShowModal(Window? owner) => throw new Exception();

        public NativeStringSpan InitialDirectory { get => throw new Exception(); set => throw new Exception(); }

        public NativeStringSpan Title { get => throw new Exception(); set => throw new Exception(); }

        public NativeStringSpan DirectoryName { get => throw new Exception(); set => throw new Exception(); }
    }
}