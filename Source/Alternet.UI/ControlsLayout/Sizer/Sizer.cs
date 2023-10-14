using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class Sizer : DisposableObject, ISizer
    {
        private const bool DisposeItemHandle = false;

        internal Sizer(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public Int32Size CalcMin()
        {
            return Native.Sizer.CalcMin(Handle);
        }

        public void RepositionChildren(Int32Size minSize)
        {
            Native.Sizer.RepositionChildren(Handle, minSize);
        }

        public ISizerItem Add(Control window, int proportion, SizerFlag flag, int border)
        {
            var result = Native.Sizer.AddWindow(
                Handle,
                window.WxWidget,
                proportion,
                (int)flag,
                border,
                IntPtr.Zero);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(ISizer sizer, int proportion, SizerFlag flag, int border)
        {
            var result = Native.Sizer.AddSizer(
                Handle,
                sizer.Handle,
                proportion,
                (int)flag,
                border,
                IntPtr.Zero);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(int width, int height, int proportion, SizerFlag flag, int border) => default;

        public ISizerItem AddSpacer(int size) => default;

        public ISizerItem AddStretchSpacer(int prop = 1) => default;

        public ISizerItem Insert(
            int index,
            Control window,
            int proportion,
            SizerFlag flag,
            int border) => default;

        public ISizerItem Insert(
            int index,
            ISizer sizer,
            int proportion,
            SizerFlag flag,
            int border) => default;

        public ISizerItem Insert(
            int index,
            int width,
            int height,
            int proportion,
            SizerFlag flag,
            int border) => default;

        public ISizerItem InsertSpacer(int index, int size) => default;

        public ISizerItem InsertStretchSpacer(int index, int prop) => default;

        public ISizerItem Prepend(
            Control window,
            int proportion,
            SizerFlag flag,
            int border) => default;

        public ISizerItem Prepend(
            ISizer sizer,
            int proportion,
            SizerFlag flag,
            int border) => default;

        public ISizerItem Prepend(
            int width,
            int height,
            int proportion,
            SizerFlag flag,
            int border) => default;

        public ISizerItem PrependSpacer(int size) => default;

        public ISizerItem PrependStretchSpacer(int prop = 1) => default;

        public bool Remove(ISizer sizer) => default;

        public bool Remove(int index) => default;

        public bool Detach(Control window) => default;

        public bool Detach(ISizer sizer) => default;

        public bool Detach(int index) => default;

        public bool Replace(Control oldwin, Control newwin, bool recursive) => default;

        public bool Replace(ISizer oldsz, ISizer newsz, bool recursive) => default;

        public void Clear(bool delete_windows)
        {
        }

        public void DeleteWindows()
        {
        }

        public bool InformFirstDirection(int direction, int size, int availableOtherDir) => default;

        public void SetMinSize(int width, int height)
        {
        }

        public bool SetItemMinSize(Control window, int width, int height) => default;

        public bool SetItemMinSize(ISizer sizer, int width, int height) => default;

        public bool SetItemMinSize(int index, int width, int height) => default;

        public Int32Size GetSize() => default;

        public Int32Point GetPosition() => default;

        public Int32Size GetMinSize() => default;

        public void RecalcSizes()
        {
        }

        public void Layout()
        {
        }

        public Int32Size ComputeFittingClientSize(Control window) => default;

        public Int32Size ComputeFittingWindowSize(Control window) => default;

        public Int32Size Fit(Control window) => default;

        public void FitInside(Control window)
        {
        }

        public void SetSizeHints(Control window)
        {
        }

        public void SetDimension(int x, int y, int width, int height)
        {
        }

        public int GetItemCount() => default;

        public bool IsEmpty() => default;

        public bool Show(Control window, bool show, bool recursive) => default;

        public bool Show(ISizer sizer, bool show, bool recursive) => default;

        public bool Show(int index, bool show) => default;

        public bool Hide(ISizer sizer, bool recursive) => default;

        public bool Hide(Control window, bool recursive) => default;

        public bool Hide(int index) => default;

        public bool IsShown(Control window) => default;

        public bool IsShown(ISizer sizer) => default;

        public bool IsShown(int index) => default;

        public void ShowItems(bool show)
        {
        }

        public void Show(bool show)
        {
        }

        public bool AreAnyItemsShown() => default;

        public ISizerItem Insert(int index, ISizerItem item) => default;

        public ISizerItem Prepend(ISizerItem item) => default;

        public ISizerItem Add(ISizerItem item) => default;

        public bool Replace(int index, ISizerItem newitem) => default;

        public ISizerItem GetItem(Control window, bool recursive) => default;

        public ISizerItem GetItem(ISizer sizer, bool recursive) => default;

        public ISizerItem GetItem(int index) => default;

        public ISizerItem GetItemById(int id, bool recursive = false) => default;

        public ISizerItem Prepend(Control window, ISizerFlags sizerFlags) => default;

        public ISizerItem Prepend(ISizer sizer, ISizerFlags sizerFlags) => default;

        public ISizerItem Prepend(int width, int height, ISizerFlags sizerFlags) => default;

        public ISizerItem Insert(int index, Control window, ISizerFlags sizerFlags) => default;

        public ISizerItem Insert(int index, ISizer sizer, ISizerFlags sizerFlags) => default;

        public ISizerItem Insert(int index, int width, int height, ISizerFlags sizerFlags) => default;

        public ISizerItem Add(Control window, ISizerFlags sizerFlags) => default;

        public ISizerItem Add(ISizer sizer, ISizerFlags sizerFlags) => default;

        public ISizerItem Add(int width, int height, ISizerFlags sizerFlags) => default;
    }
}
