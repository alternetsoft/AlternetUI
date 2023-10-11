using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_sizer.html
    internal interface ISizer : IDisposable
    {
/*
        public static IntPtr AddWindow(IntPtr handle, IntPtr window,
                             int proportion,
                             int flag,
                             int border,
                             IntPtr userData) => default;
        public static IntPtr AddSizer(IntPtr handle, IntPtr sizer,
                         int proportion,
                         int flag,
                         int border,
                         IntPtr userData) => default;
        public static IntPtr AddCustomBox(IntPtr handle, int width,
                         int height,
                         int proportion,
                         int flag,
                         int border,
                         IntPtr userData) => default;
        public static IntPtr AddWindow2(IntPtr handle, IntPtr window, IntPtr sizerFlags) => default;
        public static IntPtr AddSizer2(IntPtr handle, IntPtr sizer, IntPtr sizerFlags) => default;
        public static IntPtr AddCustomBox2(IntPtr handle, int width, int height,
            IntPtr sizerFlags) => default;
        public static IntPtr AddItem(IntPtr handle, IntPtr item) => default;

        public static IntPtr AddSpacer(IntPtr handle, int size) => default;
        public static IntPtr AddStretchSpacer(IntPtr handle, int prop = 1) => default;

        public static IntPtr InsertWindow(IntPtr handle, int index,
                            IntPtr window,
                            int proportion,
                            int flag,
                            int border,
                            IntPtr userData) => default;
        public static IntPtr InsertSizer(IntPtr handle, int index,
                            IntPtr sizer,
                            int proportion,
                            int flag,
                            int border,
                            IntPtr userData) => default;
        public static IntPtr InsertCustomBox(IntPtr handle, int index,
                            int width,
                            int height,
                            int proportion,
                            int flag,
                            int border,
                            IntPtr userData) => default;
        public static IntPtr InsertWindow2(IntPtr handle, int index,
                            IntPtr window,
                        IntPtr sizerFlags) => default;
        public static IntPtr InsertSizer2(IntPtr handle, int index,
                            IntPtr sizer,
                            IntPtr sizerFlags) => default;
        public static IntPtr InsertCustomBox2(IntPtr handle, int index,
                            int width,
                            int height,
                            IntPtr sizerFlags) => default;

        public static IntPtr InsertItem(IntPtr handle, int index, IntPtr item) => default;

        public static IntPtr InsertSpacer(IntPtr handle, int index, int size) => default;
        public static IntPtr InsertStretchSpacer(IntPtr handle, int index, int prop) => default;

        public static IntPtr PrependWindow(IntPtr handle, IntPtr window,
                             int proportion,
                             int flag,
                             int border,
                             IntPtr userData) => default;
        public static IntPtr PrependSizer(IntPtr handle, IntPtr sizer,
                             int proportion,
                             int flag,
                             int border,
                             IntPtr userData) => default;
        public static IntPtr PrependCustomBox(IntPtr handle, int width,
                             int height,
                             int proportion,
                             int flag,
                             int border,
                             IntPtr userData) => default;
        public static IntPtr PrependWindow2(IntPtr handle, IntPtr window, IntPtr sizerFlags) => default;
        public static IntPtr PrependSizer2(IntPtr handle, IntPtr sizer, IntPtr sizerFlags) => default;
        public static IntPtr PrependCustomBox2(IntPtr handle, int width, int height, IntPtr sizerFlags) => default;
        public static IntPtr PrependItem(IntPtr handle, IntPtr item) => default;

        public static IntPtr PrependSpacer(IntPtr handle, int size) => default;
        public static IntPtr PrependStretchSpacer(IntPtr handle, int prop = 1) => default;

        public static void SetContainingWindow(IntPtr handle, IntPtr window) { }
        public static IntPtr GetContainingWindow(IntPtr handle) => default;

        public static bool Remove(IntPtr handle, IntPtr sizer) => default;
        public static bool Remove2(IntPtr handle, int index) => default;

        public static bool DetachWindow(IntPtr handle, IntPtr window) => default;
        public static bool DetachSizer(IntPtr handle, IntPtr sizer) => default;
        public static bool Detach(IntPtr handle, int index) => default;

        public static bool ReplaceWindow(IntPtr handle, IntPtr oldwin, IntPtr newwin,
            bool recursive) => default;
        public static bool ReplaceSizer(IntPtr handle, IntPtr oldsz, IntPtr newsz,
            bool recursive) => default;
        public static bool ReplaceItem(IntPtr handle, int index, IntPtr newitem) => default;

        public static void Clear(IntPtr handle, bool delete_windows) { }
        public static void DeleteWindows(IntPtr handle) { }

        public static bool InformFirstDirection(IntPtr handle, int direction, int size,
            int availableOtherDir) => default;

        public static void SetMinSize(IntPtr handle, int width, int height) { }

        public static bool SetWindowItemMinSize(IntPtr handle, IntPtr window,
            int width, int height) => default;

        public static bool SetSizerItemMinSize(IntPtr handle, IntPtr sizer,
            int width, int height) => default;

        public static bool SetCustomBoxItemMinSize(IntPtr handle, int index,
            int width, int height) => default;

        public static Int32Size GetSize(IntPtr handle) => default;
        public static Int32Point GetPosition(IntPtr handle) => default;
        public static Int32Size GetMinSize(IntPtr handle) => default;
        public static Int32Size CalcMin(IntPtr handle) => default;
        public static void RepositionChildren(IntPtr handle, Int32Size minSize) { }
        public static void RecalcSizes(IntPtr handle) { }
        public static void Layout(IntPtr handle) { }
        public static Int32Size ComputeFittingClientSize(IntPtr handle, IntPtr window) => default;
        public static Int32Size ComputeFittingWindowSize(IntPtr handle, IntPtr window) => default;
        public static Int32Size Fit(IntPtr handle, IntPtr window) => default;
        public static void FitInside(IntPtr handle, IntPtr window) { }
        public static void SetSizeHints(IntPtr handle, IntPtr window) { }
        public static IntPtr GetChildren(IntPtr handle) => default;
        public static void SetDimension(IntPtr handle, int x, int y, int width, int height) { }
        public static int GetItemCount(IntPtr handle) => default;
        public static bool IsEmpty(IntPtr handle) => default;
        public static IntPtr GetItemWindow(IntPtr handle, IntPtr window, bool recursive) => default;
        public static IntPtr GetItemSizer(IntPtr handle, IntPtr sizer, bool recursive) => default;
        public static IntPtr GetItem(IntPtr handle, int index) => default;
        public static IntPtr GetItemById(IntPtr handle, int id, bool recursive = false) => default;

        public static bool ShowWindow(IntPtr handle, IntPtr window, bool show,
            bool recursive) => default;
        public static bool ShowSizer(IntPtr handle, IntPtr sizer, bool show,
            bool recursive) => default;
        public static bool ShowItem(IntPtr handle, int index, bool show) => default;

        public static bool HideSizer(IntPtr handle, IntPtr sizer, bool recursive) => default;
        public static bool HideWindow(IntPtr handle, IntPtr window, bool recursive) => default;
        public static bool Hide(IntPtr handle, int index) => default;

        public static bool IsShownWindow(IntPtr handle, IntPtr window) => default;
        public static bool IsShownSizer(IntPtr handle, IntPtr sizer) => default;
        public static bool IsShown(IntPtr handle, int index) => default;
        public static void ShowItems(IntPtr handle, bool show) { }

        public static void Show(IntPtr handle, bool show) { }
        public static bool AreAnyItemsShown(IntPtr handle) => default;
*/
    }
}