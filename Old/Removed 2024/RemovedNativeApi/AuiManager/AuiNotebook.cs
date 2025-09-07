#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_aui_notebook.html
    public class AuiNotebook : Control
	{
        public int EventSelection { get; }
        public int EventOldSelection { get; }

        public static IntPtr CreateEx(long styles) => throw new Exception();

        public void SetArtProvider(IntPtr art) => throw new Exception();
        public IntPtr GetArtProvider() => throw new Exception();

        public void SetUniformBitmapSize(int width, int height) => throw new Exception();
        
        public void SetTabCtrlHeight(int height) => throw new Exception();

        public bool AddPage(IntPtr page,
                     string caption,
                     bool select,
                     ImageSet? bitmap) => throw new Exception();

        public bool InsertPage(ulong pageIdx,
                        IntPtr page,
                        string caption,
                        bool select,
                        ImageSet? bitmap) => throw new Exception();

        public bool DeletePage(ulong page) => throw new Exception();
        public bool RemovePage(ulong page) => throw new Exception();

        public ulong GetPageCount() => throw new Exception();
        public IntPtr GetPage(ulong pageIdx) => throw new Exception();
        public int FindPage(IntPtr page) => throw new Exception();

        public bool SetPageText(ulong page, string text) => throw new Exception();
        public string GetPageText(ulong pageIdx) => throw new Exception();

        public bool SetPageToolTip(ulong page, string text) => throw new Exception();
        public string GetPageToolTip(ulong pageIdx) => throw new Exception();

        public bool SetPageBitmap(ulong page, ImageSet? bitmap) =>
            throw new Exception();

        public long SetSelection(ulong newPage) => throw new Exception();
        public long GetSelection() => throw new Exception();

        public long ChangeSelection(ulong newPage) => throw new Exception();
        public void AdvanceSelection(bool forward = true) => throw new Exception();

        public void SetMeasuringFont(Font? font) => throw new Exception();
        public void SetNormalFont(Font? font) => throw new Exception();
        public void SetSelectedFont(Font? font) => throw new Exception();

        public void Split(ulong page, int direction) => throw new Exception();

        // Gets the tab control height
        public int GetTabCtrlHeight() => throw new Exception();

        // Gets the height of the notebook for a given page height
        public int GetHeightForPageHeight(int pageHeight) => throw new Exception();

        public bool ShowWindowMenu() => throw new Exception();

        public bool DeleteAllPages() => throw new Exception();

        public long CreateStyle { get; set; }

        [NativeEvent(cancellable: true)]
        public event EventHandler? PageClose;
        
        public event EventHandler? PageClosed;
        
        public event EventHandler? PageChanged;

        [NativeEvent(cancellable: true)]
        public event EventHandler? PageChanging;

        [NativeEvent(cancellable: true)]
        public event EventHandler? PageButton;

        public event EventHandler? BeginDrag;
        
        public event EventHandler? EndDrag;
        
        public event EventHandler? DragMotion;

        public event EventHandler? AllowTabDrop;
        
        public event EventHandler? DragDone;
        
        public event EventHandler? TabMiddleMouseDown;
        
        public event EventHandler? TabMiddleMouseUp;
        
        public event EventHandler? TabRightMouseDown;
        
        public event EventHandler? TabRightMouseUp;
        
        public event EventHandler? BgDclickMouse;
    }
}

