using System;

namespace NativeApi.Api
{
    public class MainMenu : Object
    {
        public int ItemsCount { get; }

        public void InsertItemAt(int index, Menu menu, string text) => throw new Exception();

        public void RemoveItemAt(int index) => throw new Exception();

        public void SetItemText(int index, string text) => throw new Exception();
    }
}