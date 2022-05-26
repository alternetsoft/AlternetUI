using System;

namespace NativeApi.Api
{
    public class Menu : Control
    {
        public int ItemsCount { get; }

        public void InsertItemAt(int index, MenuItem item) => throw new Exception();

        public void RemoveItemAt(int index) => throw new Exception();
    }
}