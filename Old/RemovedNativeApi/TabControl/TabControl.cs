#pragma warning disable
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

        public event NativeEventHandler<TabPageSelectionEventData>? SelectedPageIndexChanging { add => throw new Exception(); remove => throw new Exception(); }

        public SizeD GetTotalPreferredSizeFromPageSize(SizeD pageSize) => throw new Exception();

        public TabAlignment TabAlignment { get; set; }
    }
}