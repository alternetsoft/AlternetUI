using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class ListBox : Control
    {
        public void InsertItem(int index, string value) => throw new Exception();
        public void RemoveItemAt(int index) => throw new Exception();
        public void ClearItems() => throw new Exception();
        public int ItemsCount { get; }
    }
}