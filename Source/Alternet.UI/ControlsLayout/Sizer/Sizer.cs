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

        public ISizerItem Add(int width, int height, int proportion, SizerFlag flag, int border)
        {
            var result = Native.Sizer.AddCustomBox(
                Handle,
                width,
                height,
                proportion,
                (int)flag,
                border,
                IntPtr.Zero);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem AddSpacer(int size)
        {
            var result = Native.Sizer.AddSpacer(Handle, size);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem AddStretchSpacer(int prop = 1)
        {
            var result = Native.Sizer.AddStretchSpacer(Handle, prop);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Insert(
            int index,
            Control window,
            int proportion,
            SizerFlag flag,
            int border)
        {
            var result = Native.Sizer.InsertWindow(
                Handle,
                index,
                window.WxWidget,
                proportion,
                (int)flag,
                border,
                IntPtr.Zero);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Insert(
            int index,
            ISizer sizer,
            int proportion,
            SizerFlag flag,
            int border)
        {
            var result = Native.Sizer.InsertSizer(
                Handle,
                index,
                sizer.Handle,
                proportion,
                (int)flag,
                border,
                IntPtr.Zero);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Insert(
            int index,
            int width,
            int height,
            int proportion,
            SizerFlag flag,
            int border)
        {
            var result = Native.Sizer.InsertCustomBox(
                Handle,
                index,
                width,
                height,
                proportion,
                (int)flag,
                border,
                IntPtr.Zero);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem InsertSpacer(int index, int size)
        {
            var result = Native.Sizer.InsertSpacer(Handle, index, size);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem InsertStretchSpacer(int index, int prop)
        {
            var result = Native.Sizer.InsertStretchSpacer(Handle, index, prop);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(
            Control window,
            int proportion,
            SizerFlag flag,
            int border)
        {
            var result = Native.Sizer.PrependWindow(
                Handle,
                window.WxWidget,
                proportion,
                (int)flag,
                border,
                IntPtr.Zero);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(
            ISizer sizer,
            int proportion,
            SizerFlag flag,
            int border)
        {
            var result = Native.Sizer.PrependSizer(
                Handle,
                sizer.Handle,
                proportion,
                (int)flag,
                border,
                IntPtr.Zero);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(
            int width,
            int height,
            int proportion,
            SizerFlag flag,
            int border)
        {
            var result = Native.Sizer.PrependCustomBox(
                Handle,
                width,
                height,
                proportion,
                (int)flag,
                border,
                IntPtr.Zero);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem PrependSpacer(int size)
        {
            var result = Native.Sizer.PrependSpacer(Handle, size);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem PrependStretchSpacer(int prop = 1)
        {
            var result = Native.Sizer.PrependStretchSpacer(Handle, prop);
            return new SizerItem(result, DisposeItemHandle);
        }

        public bool Remove(ISizer sizer)
        {
            return Native.Sizer.Remove(Handle, sizer.Handle);
        }

        public bool Remove(int index)
        {
            return Native.Sizer.Remove2(Handle, index);
        }

        public bool Detach(Control window)
        {
            return Native.Sizer.DetachWindow(Handle, window.WxWidget);
        }

        public bool Detach(ISizer sizer)
        {
            return Native.Sizer.DetachSizer(Handle, sizer.Handle);
        }

        public bool Detach(int index)
        {
            return Native.Sizer.Detach(Handle, index);
        }

        public bool Replace(Control oldwin, Control newwin, bool recursive)
        {
            return Native.Sizer.ReplaceWindow(Handle, oldwin.WxWidget, newwin.WxWidget, recursive);
        }

        public bool Replace(ISizer oldsz, ISizer newsz, bool recursive)
        {
            return Native.Sizer.ReplaceSizer(Handle, oldsz.Handle, newsz.Handle, recursive);
        }

        public void Clear()
        {
            Native.Sizer.Clear(Handle, false);
        }

        public void DeleteWindows()
        {
            Native.Sizer.DeleteWindows(Handle);
        }

        public bool InformFirstDirection(int direction, int size, int availableOtherDir)
        {
            return Native.Sizer.InformFirstDirection(Handle, direction, size, availableOtherDir);
        }

        public void SetMinSize(int width, int height)
        {
            Native.Sizer.SetMinSize(Handle, width, height);
        }

        public bool SetItemMinSize(Control window, int width, int height)
        {
            return Native.Sizer.SetWindowItemMinSize(Handle, window.WxWidget, width, height);
        }

        public bool SetItemMinSize(ISizer sizer, int width, int height)
        {
            return Native.Sizer.SetSizerItemMinSize(Handle, sizer.Handle, width, height);
        }

        public bool SetItemMinSize(int index, int width, int height)
        {
            return Native.Sizer.SetCustomBoxItemMinSize(Handle, index, width, height);
        }

        public Int32Size GetSize()
        {
            return Native.Sizer.GetSize(Handle);
        }

        public Int32Point GetPosition()
        {
            return Native.Sizer.GetPosition(Handle);
        }

        public Int32Size GetMinSize()
        {
            return Native.Sizer.GetMinSize(Handle);
        }

        public void RecalcSizes()
        {
            Native.Sizer.RecalcSizes(Handle);
        }

        public void Layout()
        {
            Native.Sizer.Layout(Handle);
        }

        public Int32Size ComputeFittingClientSize(Control window)
        {
            return Native.Sizer.ComputeFittingClientSize(Handle, window.WxWidget);
        }

        public Int32Size ComputeFittingWindowSize(Control window)
        {
            return Native.Sizer.ComputeFittingWindowSize(Handle, window.WxWidget);
        }

        public Int32Size Fit(Control window)
        {
            return Native.Sizer.Fit(Handle, window.WxWidget);
        }

        public void FitInside(Control window)
        {
            Native.Sizer.FitInside(Handle, window.WxWidget);
        }

        public void SetSizeHints(Control window)
        {
            Native.Sizer.SetSizeHints(Handle, window.WxWidget);
        }

        public void SetDimension(int x, int y, int width, int height)
        {
            Native.Sizer.SetDimension(Handle, x, y, width, height);
        }

        public int GetItemCount()
        {
            return Native.Sizer.GetItemCount(Handle);
        }

        public bool IsEmpty()
        {
            return Native.Sizer.IsEmpty(Handle);
        }

        public bool Show(Control window, bool show, bool recursive)
        {
            return Native.Sizer.ShowWindow(Handle, window.WxWidget, show, recursive);
        }

        public bool Show(ISizer sizer, bool show, bool recursive)
        {
            return Native.Sizer.ShowSizer(Handle, sizer.Handle, show, recursive);
        }

        public bool Show(int index, bool show)
        {
            return Native.Sizer.ShowItem(Handle, index, show);
        }

        public bool Hide(ISizer sizer, bool recursive)
        {
            return Native.Sizer.HideSizer(Handle, sizer.Handle, recursive);
        }

        public bool Hide(Control window, bool recursive)
        {
            return Native.Sizer.HideWindow(Handle, window.WxWidget, recursive);
        }

        public bool Hide(int index)
        {
            return Native.Sizer.Hide(Handle, index);
        }

        public bool IsShown(Control window)
        {
            return Native.Sizer.IsShownWindow(Handle, window.WxWidget);
        }

        public bool IsShown(ISizer sizer)
        {
            return Native.Sizer.IsShownSizer(Handle, sizer.Handle);
        }

        public bool IsShown(int index)
        {
            return Native.Sizer.IsShown(Handle, index);
        }

        public void ShowItems(bool show)
        {
            Native.Sizer.ShowItems(Handle, show);
        }

        public void Show(bool show)
        {
            Native.Sizer.Show(Handle, show);
        }

        public bool AreAnyItemsShown()
        {
            return Native.Sizer.AreAnyItemsShown(Handle);
        }

        public ISizerItem Insert(int index, ISizerItem item)
        {
            var result = Native.Sizer.InsertItem(Handle, index, item.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(ISizerItem item)
        {
            var result = Native.Sizer.PrependItem(Handle, item.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(ISizerItem item)
        {
            var result = Native.Sizer.AddItem(Handle, item.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public bool Replace(int index, ISizerItem newitem)
        {
            return Native.Sizer.ReplaceItem(Handle, index, newitem.Handle);
        }

        public ISizerItem? GetItem(Control window, bool recursive)
        {
            var result = Native.Sizer.GetItemWindow(Handle, window.WxWidget, recursive);
            if (result == default)
                return null;
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem? GetItem(ISizer sizer, bool recursive)
        {
            var result = Native.Sizer.GetItemSizer(Handle, sizer.Handle, recursive);
            if (result == default)
                return null;
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem? GetItem(int index)
        {
            var result = Native.Sizer.GetItem(Handle, index);
            if (result == default)
                return null;
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem? GetItemById(int id, bool recursive = false)
        {
            var result = Native.Sizer.GetItemById(Handle, id, recursive);
            if (result == default)
                return null;
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(Control window, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.PrependWindow2(Handle, window.WxWidget, sizerFlags.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(ISizer sizer, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.PrependSizer2(Handle, sizer.Handle, sizerFlags.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(int width, int height, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.PrependCustomBox2(Handle, width, height, sizerFlags.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Insert(int index, Control window, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.InsertWindow2(Handle, index, window.WxWidget, sizerFlags.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Insert(int index, ISizer sizer, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.InsertSizer2(Handle, index, sizer.Handle, sizerFlags.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Insert(int index, int width, int height, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.InsertCustomBox2(Handle, index, width, height, sizerFlags.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(Control window, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.AddWindow2(Handle, window.WxWidget, sizerFlags.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(ISizer sizer, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.AddSizer2(Handle, sizer.Handle, sizerFlags.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(int width, int height, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.AddCustomBox2(Handle, width, height, sizerFlags.Handle);
            return new SizerItem(result, DisposeItemHandle);
        }
    }
}
