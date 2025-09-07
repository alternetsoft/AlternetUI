using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class Sizer : DisposableObject<IntPtr>, ISizer
    {
        private const bool DisposeItemHandle = false;

        internal Sizer(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public SizeI CalcMin()
        {
            return Native.Sizer.CalcMin(Handle);
        }

        public void RepositionChildren(SizeI minSize)
        {
            Native.Sizer.RepositionChildren(Handle, minSize);
        }

        public ISizerItem Add(Control window, int proportion, SizerFlag flag, int border)
        {
            var result = Native.Sizer.AddWindow(
                Handle,
                WxPlatform.WxWidget(window),
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
                ((Sizer)sizer).Handle,
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
                WxPlatform.WxWidget(window),
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
                ((Sizer)sizer).Handle,
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
                WxPlatform.WxWidget(window),
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
                ((Sizer)sizer).Handle,
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
            return Native.Sizer.Remove(Handle, ((Sizer)sizer).Handle);
        }

        public bool Remove(int index)
        {
            return Native.Sizer.Remove2(Handle, index);
        }

        public bool Detach(Control window)
        {
            return Native.Sizer.DetachWindow(Handle, WxPlatform.WxWidget(window));
        }

        public bool Detach(ISizer sizer)
        {
            return Native.Sizer.DetachSizer(Handle, ((Sizer)sizer).Handle);
        }

        public bool Detach(int index)
        {
            return Native.Sizer.Detach(Handle, index);
        }

        public bool Replace(Control oldwin, Control newwin, bool recursive)
        {
            return Native.Sizer.ReplaceWindow(
                Handle,
                WxPlatform.WxWidget(oldwin),
                WxPlatform.WxWidget(newwin),
                recursive);
        }

        public bool Replace(ISizer oldsz, ISizer newsz, bool recursive)
        {
            return Native.Sizer.ReplaceSizer(Handle, ((Sizer)oldsz).Handle, ((Sizer)newsz).Handle, recursive);
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
            return Native.Sizer.SetWindowItemMinSize(Handle, WxPlatform.WxWidget(window), width, height);
        }

        public bool SetItemMinSize(ISizer sizer, int width, int height)
        {
            return Native.Sizer.SetSizerItemMinSize(Handle, ((Sizer)sizer).Handle, width, height);
        }

        public bool SetItemMinSize(int index, int width, int height)
        {
            return Native.Sizer.SetCustomBoxItemMinSize(Handle, index, width, height);
        }

        public SizeI GetSize()
        {
            return Native.Sizer.GetSize(Handle);
        }

        public PointI GetPosition()
        {
            return Native.Sizer.GetPosition(Handle);
        }

        public SizeI GetMinSize()
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

        public SizeI ComputeFittingClientSize(Control window)
        {
            return Native.Sizer.ComputeFittingClientSize(Handle, WxPlatform.WxWidget(window));
        }

        public SizeI ComputeFittingWindowSize(Control window)
        {
            return Native.Sizer.ComputeFittingWindowSize(Handle, WxPlatform.WxWidget(window));
        }

        public SizeI Fit(Control window)
        {
            return Native.Sizer.Fit(Handle, WxPlatform.WxWidget(window));
        }

        public void FitInside(Control window)
        {
            Native.Sizer.FitInside(Handle, WxPlatform.WxWidget(window));
        }

        public void SetSizeHints(Control window)
        {
            Native.Sizer.SetSizeHints(Handle, WxPlatform.WxWidget(window));
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
            return Native.Sizer.ShowWindow(Handle, WxPlatform.WxWidget(window), show, recursive);
        }

        public bool Show(ISizer sizer, bool show, bool recursive)
        {
            return Native.Sizer.ShowSizer(Handle, ((Sizer)sizer).Handle, show, recursive);
        }

        public bool Show(int index, bool show)
        {
            return Native.Sizer.ShowItem(Handle, index, show);
        }

        public bool Hide(ISizer sizer, bool recursive)
        {
            return Native.Sizer.HideSizer(Handle, ((Sizer)sizer).Handle, recursive);
        }

        public bool Hide(Control window, bool recursive)
        {
            return Native.Sizer.HideWindow(Handle, WxPlatform.WxWidget(window), recursive);
        }

        public bool Hide(int index)
        {
            return Native.Sizer.Hide(Handle, index);
        }

        public bool IsShown(Control window)
        {
            return Native.Sizer.IsShownWindow(Handle, WxPlatform.WxWidget(window));
        }

        public bool IsShown(ISizer sizer)
        {
            return Native.Sizer.IsShownSizer(Handle, ((Sizer)sizer).Handle);
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
            var result = Native.Sizer.InsertItem(Handle, index, ((SizerItem)item).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(ISizerItem item)
        {
            var result = Native.Sizer.PrependItem(Handle, ((SizerItem)item).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(ISizerItem item)
        {
            var result = Native.Sizer.AddItem(Handle, ((SizerItem)item).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public bool Replace(int index, ISizerItem newitem)
        {
            return Native.Sizer.ReplaceItem(Handle, index, ((SizerItem)newitem).Handle);
        }

        public ISizerItem? GetItem(Control window, bool recursive)
        {
            var result = Native.Sizer.GetItemWindow(Handle, WxPlatform.WxWidget(window), recursive);
            if (result == default)
                return null;
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem? GetItem(ISizer sizer, bool recursive)
        {
            var result = Native.Sizer.GetItemSizer(Handle, ((Sizer)sizer).Handle, recursive);
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
            var result = Native.Sizer.PrependWindow2(
                Handle,
                WxPlatform.WxWidget(window),
                ((SizerFlags)sizerFlags).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(ISizer sizer, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.PrependSizer2(Handle, ((Sizer)sizer).Handle, ((SizerFlags)sizerFlags).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Prepend(int width, int height, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.PrependCustomBox2(Handle, width, height, ((SizerFlags)sizerFlags).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Insert(int index, Control window, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.InsertWindow2(Handle, index, WxPlatform.WxWidget(window), ((SizerFlags)sizerFlags).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Insert(int index, ISizer sizer, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.InsertSizer2(Handle, index, ((Sizer)sizer).Handle, ((SizerFlags)sizerFlags).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Insert(int index, int width, int height, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.InsertCustomBox2(Handle, index, width, height, ((SizerFlags)sizerFlags).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(Control window, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.AddWindow2(Handle, WxPlatform.WxWidget(window), ((SizerFlags)sizerFlags).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(ISizer sizer, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.AddSizer2(Handle, ((Sizer)sizer).Handle, ((SizerFlags)sizerFlags).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }

        public ISizerItem Add(int width, int height, ISizerFlags sizerFlags)
        {
            var result = Native.Sizer.AddCustomBox2(Handle, width, height, ((SizerFlags)sizerFlags).Handle);
            return new SizerItem(result, DisposeItemHandle);
        }
    }
}
