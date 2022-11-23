using ApiCommon;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class TabControl : Control
    {
        public void InsertPage(int index, Control page, string title) => throw new Exception();

        public void RemovePage(int index, Control page) => throw new Exception();

        public void SetPageTitle(int index, string title) => throw new Exception();

        public int PageCount { get; }

        public int SelectedPageIndex { get; set; }

        public event EventHandler SelectedPageIndexChanged { add => throw new Exception(); remove => throw new Exception(); }

        public Size GetTotalPreferredSizeFromPageSize(Size pageSize) => throw new Exception();
    }
}