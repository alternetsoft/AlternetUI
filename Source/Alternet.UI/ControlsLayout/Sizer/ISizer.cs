using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_sizer.html

    /// <summary>
    /// Provides methods and properties used for laying out sub-controls in a control.
    /// </summary>
    public interface ISizer : IDisposableObject
    {
        /// <summary>
        /// Does the actual calculation of children's minimal sizes.
        /// </summary>
        Int32Size CalcMin();

        void Add(Control window, int proportion, int flag, int border);

        void Add(ISizer sizer, int proportion, int flag, int border);

        void Add(int width, int height, int proportion, int flag, int border);

        void AddSpacer(int size);

        void AddStretchSpacer(int prop = 1);

        void Insert(
            int index,
            Control window,
            int proportion,
            int flag,
            int border);

        void Insert(
            int index,
            ISizer sizer,
            int proportion,
            int flag,
            int border);

        void Insert(
            int index,
            int width,
            int height,
            int proportion,
            int flag,
            int border);

        void InsertSpacer(int index, int size);

        void InsertStretchSpacer(int index, int prop);

        void Prepend(
            Control window,
            int proportion,
            int flag,
            int border);

        void Prepend(
            ISizer sizer,
            int proportion,
            int flag,
            int border);

        void Prepend(
            int width,
            int height,
            int proportion,
            int flag,
            int border);

        void PrependSpacer(int size);

        void PrependStretchSpacer(int prop = 1);

        bool Remove(ISizer sizer);

        bool Remove(int index);

        bool Detach(Control window);

        bool Detach(ISizer sizer);

        bool Detach(int index);

        bool Replace(Control oldwin, Control newwin, bool recursive);

        bool Replace(ISizer oldsz, ISizer newsz, bool recursive);

        void Clear(bool delete_windows);

        void DeleteWindows();

        bool InformFirstDirection(int direction, int size, int availableOtherDir);

        void SetMinSize(int width, int height);

        bool SetItemMinSize(Control window, int width, int height);

        bool SetItemMinSize(ISizer sizer, int width, int height);

        bool SetItemMinSize(int index, int width, int height);

        Int32Size GetSize();

        Int32Point GetPosition();

        Int32Size GetMinSize();

        void RecalcSizes();

        void Layout();

        Int32Size ComputeFittingClientSize(Control window);

        Int32Size ComputeFittingWindowSize(Control window);

        Int32Size Fit(Control window);

        void FitInside(Control window);

        void SetSizeHints(Control window);

        void SetDimension(int x, int y, int width, int height);

        int GetItemCount();

        bool IsEmpty();

        bool Show(Control window, bool show, bool recursive);

        bool Show(ISizer sizer, bool show, bool recursive);

        bool Show(int index, bool show);

        bool Hide(ISizer sizer, bool recursive);

        bool Hide(Control window, bool recursive);

        bool Hide(int index);

        bool IsShown(Control window);

        bool IsShown(ISizer sizer);

        bool IsShown(int index);

        void ShowItems(bool show);

        void Show(bool show);

        bool AreAnyItemsShown();

        // void Prepend(Control window, IntPtr sizerFlags);
        // void Prepend(ISizer sizer, IntPtr sizerFlags);
        // void Prepend(int width, int height, IntPtr sizerFlags);
        // void Prepend(IntPtr item);
        // IntPtr Insert(int index, Control window, IntPtr sizerFlags);
        // IntPtr Insert(int index, ISizer sizer, IntPtr sizerFlags);
        // IntPtr Insert(int index, int width, int height, IntPtr sizerFlags);
        // IntPtr Insert(int index, IntPtr item);
        // void Add(Control window, IntPtr sizerFlags);
        // void AddSizer(ISizer sizer, IntPtr sizerFlags);
        // void Add(int width, int height, IntPtr sizerFlags);
        // void AddItem(IntPtr item);
        // bool ReplaceItem(int index, IntPtr newitem);
        // IntPtr GetChildren();
        // IntPtr GetItemWindow(Control window, bool recursive);
        // IntPtr GetItemSizer(ISizer sizer, bool recursive);
        // IntPtr GetItem(int index);
        // IntPtr GetItemById(int id, bool recursive = false);
        // void SetContainingWindow(Control window);
        // IntPtr GetContainingWindow();
    }
}