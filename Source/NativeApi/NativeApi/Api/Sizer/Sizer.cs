#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_sizer.html
    public class Sizer
    {
        public IntPtr Handle { get => throw new Exception(); }

        public IntPtr AddWindow(IntPtr window,
                             int proportion,
                             int flag,
                             int border,
                             IntPtr userData) => throw new Exception();
        /*public abstract IntPtr AddSizer(IntPtr sizer,
                         int proportion,
                         int flag,
                         int border,
                         IntPtr userData);
        public abstract IntPtr AddCustomBox(int width,
                         int height,
                         int proportion,
                         int flag,
                         int border,
                         IntPtr userData);
        public abstract IntPtr AddWindow2(IntPtr window, wxSizerFlags& flags);
        public abstract IntPtr AddSizer2(IntPtr sizer, wxSizerFlags& flags);
        public abstract IntPtr AddCustomBox2(int width, int height, wxSizerFlags& flags);
        public abstract IntPtr AddItem(IntPtr item);

        public abstract IntPtr AddSpacer(int size);
        public abstract IntPtr AddStretchSpacer(int prop = 1);

        public abstract IntPtr InsertWindow(size_t index,
                            IntPtr window,
                            int proportion,
                            int flag,
                            int border,
                            IntPtr userData);
        public abstract IntPtr InsertSizer(size_t index,
                            IntPtr sizer,
                            int proportion,
                            int flag,
                            int border,
                            IntPtr userData);
        public abstract IntPtr InsertCustomBox(size_t index,
                            int width,
                            int height,
                            int proportion,
                            int flag,
                            int border,
                            IntPtr userData);
        public abstract IntPtr InsertWindow2(size_t index,
                            IntPtr window,
                        wxSizerFlags& flags);
        public abstract IntPtr InsertSizer2(size_t index,
                            IntPtr sizer,
                            wxSizerFlags& flags);
        public abstract IntPtr InsertCustomBox2(size_t index,
                            int width,
                            int height,
                            wxSizerFlags& flags);

        public abstract IntPtr InsertItem(size_t index, IntPtr item);

        public abstract IntPtr InsertSpacer(size_t index, int size);
        public abstract IntPtr InsertStretchSpacer(size_t index, int prop);

        public abstract IntPtr PrependWindow(IntPtr window,
                             int proportion,
                             int flag,
                             int border,
                             IntPtr userData);
        public abstract IntPtr PrependSizer(IntPtr sizer,
                             int proportion,
                             int flag,
                             int border,
                             IntPtr userData);
        public abstract IntPtr PrependCustomBox(int width,
                             int height,
                             int proportion,
                             int flag,
                             int border,
                             IntPtr userData);
        public abstract IntPtr PrependWindow(IntPtr window, wxSizerFlags& flags);
        public abstract IntPtr PrependSizer(IntPtr sizer, wxSizerFlags& flags);
        public abstract IntPtr PrependCustomBox(int width, int height, wxSizerFlags& flags);
        public abstract IntPtr PrependItem(IntPtr item);

        public abstract IntPtr PrependSpacer(int size);
        public abstract IntPtr PrependStretchSpacer(int prop = 1);

        public abstract void SetContainingWindow(IntPtr window);
        public abstract IntPtr GetContainingWindow();

        public abstract bool Remove(IntPtr sizer);
        public abstract bool Remove(int index);

        public abstract bool DetachWindow(IntPtr window);
        public abstract bool DetachSizer(IntPtr sizer);
        public abstract bool Detach(int index);

        public abstract bool ReplaceWindow(IntPtr oldwin, IntPtr newwin, bool recursive);
        public abstract bool ReplaceSizer(IntPtr oldsz, IntPtr newsz, bool recursive);
        public abstract bool ReplaceItem(size_t index, IntPtr newitem);

        public abstract void Clear(bool delete_windows);
        public abstract void DeleteWindows();

        public abstract bool InformFirstDirection(int direction, int size,
            int availableOtherDir);

        public abstract void SetMinSize(int width, int height);

        public abstract bool SetWindowItemMinSize(IntPtr window, int width, int height);

        public abstract bool SetSizerItemMinSize(IntPtr sizer, int width, int height)

        public abstract bool SetCustomBoxItemMinSize(size_t index, int width, int height);

        public abstract wxSize GetSize();
        public abstract wxPoint GetPosition();
        public abstract wxSize GetMinSize();
        public abstract wxSize CalcMin();
        public abstract void RepositionChildren(wxSize& minSize);
        public abstract void RecalcSizes();
        public abstract void Layout();
        public abstract wxSize ComputeFittingClientSize(IntPtr window);
        public abstract wxSize ComputeFittingWindowSize(IntPtr window);
        public abstract wxSize Fit(IntPtr window);
        public abstract void FitInside(IntPtr window);
        public abstract void SetSizeHints(IntPtr window);
        public abstract wxSizerItemList & GetChildren();
        public abstract void SetDimension(int x, int y, int width, int height);
        public abstract size_t GetItemCount();
        public abstract bool IsEmpty();
        public abstract IntPtr GetItemWindow(IntPtr window, bool recursive);
        public abstract IntPtr GetItemSizer(IntPtr sizer, bool recursive);
        public abstract IntPtr GetItem(size_t index);
        public abstract IntPtr GetItemById(int id, bool recursive = false);

        public abstract bool ShowWindow(IntPtr window, bool show, bool recursive);
        public abstract bool ShowSizer(IntPtr sizer, bool show, bool recursive);
        public abstract bool ShowItem(size_t index, bool show);

        public abstract bool HideSizer(IntPtr sizer, bool recursive);
        public abstract bool HideWindow(IntPtr window, bool recursive);
        public abstract bool Hide(size_t index);

        public abstract bool IsShownWindow(IntPtr window);
        public abstract bool IsShownSizer(IntPtr sizer);
        public abstract bool IsShown(size_t index);
        public abstract void ShowItems(bool show);

        public abstract void Show(bool show);
        public abstract bool AreAnyItemsShown();
        */
    }
}


