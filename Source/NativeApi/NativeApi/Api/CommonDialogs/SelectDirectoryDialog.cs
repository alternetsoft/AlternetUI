using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class SelectDirectoryDialog
    {
        public ModalResult ShowModal(Window? owner) => throw new Exception();

        public string? InitialDirectory { get => throw new Exception(); set => throw new Exception(); }

        public string? Title { get => throw new Exception(); set => throw new Exception(); }

        public string? DirectoryName { get => throw new Exception(); set => throw new Exception(); }
    }
}