using ApiCommon;
using System;

namespace NativeApi.Api
{
    public enum FileDialogMode
    {
        Open,
        Save
    }

    public class FileDialog
    {
        public FileDialogMode Mode { get; set; }

        public string InitialDirectory { get => throw new Exception(); set => throw new Exception(); }

        public string Title { get => throw new Exception(); set => throw new Exception(); }

        public string Filter { get => throw new Exception(); set => throw new Exception(); }

        public int SelectedFilterIndex { get => throw new Exception(); set => throw new Exception(); }

        public string FileName { get => throw new Exception(); set => throw new Exception(); }

        public bool AllowMultipleSelection { get => throw new Exception(); set => throw new Exception(); }

        public string[] FileNames { get => throw new Exception(); }

        public ModalResult ShowModal() => throw new Exception();
    }
}