using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class TabControl : Control
    {
        public void InsertPage(int index, Control page, string title) => throw new Exception();

        public void RemovePage(int index) => throw new Exception();

        public int PageCount { get; }
    }
}