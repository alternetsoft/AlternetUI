using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class CheckListBox : ListBox
    {
        public event EventHandler? CheckedChanged { add => throw new Exception(); remove => throw new Exception(); }

        public int[] CheckedIndices { get => throw new Exception(); }

        public void ClearChecked() => throw new Exception();

        public void SetChecked(int index, bool value) => throw new Exception();

        public bool IsChecked(int item) => throw new Exception();
    }
}