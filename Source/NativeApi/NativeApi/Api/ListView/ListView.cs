using System;

namespace NativeApi.Api
{
    public class ListView : Control
    {
        public int ItemsCount { get; }

        public void InsertItemAt(int index, string value) => throw new Exception();

        public void RemoveItemAt(int index) => throw new Exception();

        public void ClearItems() => throw new Exception();

        public ListViewView CurrentView { get; set; }
    }
}