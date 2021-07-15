using ApiCommon;
using System;
using System.Drawing;

namespace NativeApi.Api
{
    public class TabControl : Control
    {
        public void InsertPage(int index, Control page, string title) => throw new Exception();

        public void RemovePage(int index, Control page) => throw new Exception();

        public int PageCount { get; }

        public SizeF GetTotalPreferredSizeFromPageSize(SizeF pageSize) => throw new Exception();
    }
}