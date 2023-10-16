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

        ISizerItem Add(Control window, int proportion = 0, SizerFlag flag = 0, int border = 0);

        ISizerItem Add(ISizer sizer, int proportion = 0, SizerFlag flag = 0, int border = 0);

        ISizerItem Add(int width, int height, int proportion = 0, SizerFlag flag = 0, int border = 0);

        /// <summary>
        /// Adds non-stretchable space to the main orientation of the sizer only.
        /// </summary>
        /// <param name="size"></param>
        ISizerItem AddSpacer(int size);

        ISizerItem AddStretchSpacer(int prop = 1);

        ISizerItem Insert(
            int index,
            Control window,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        ISizerItem Insert(
            int index,
            ISizer sizer,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        ISizerItem Insert(
            int index,
            int width,
            int height,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        ISizerItem InsertSpacer(int index, int size);

        ISizerItem InsertStretchSpacer(int index, int prop);

        ISizerItem Prepend(
            Control window,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        ISizerItem Prepend(
            ISizer sizer,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        ISizerItem Prepend(
            int width,
            int height,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        ISizerItem PrependSpacer(int size);

        ISizerItem PrependStretchSpacer(int prop = 1);

        bool Remove(ISizer sizer);

        bool Remove(int index);

        bool Detach(Control window);

        bool Detach(ISizer sizer);

        bool Detach(int index);

        bool Replace(Control oldwin, Control newwin, bool recursive);

        bool Replace(ISizer oldsz, ISizer newsz, bool recursive);

        void Clear();

        internal void DeleteWindows();

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

        ISizerItem Insert(int index, ISizerItem item);

        ISizerItem Prepend(ISizerItem item);

        ISizerItem Add(ISizerItem item);

        bool Replace(int index, ISizerItem newitem);

        ISizerItem GetItem(Control window, bool recursive);

        ISizerItem GetItem(ISizer sizer, bool recursive);

        ISizerItem GetItem(int index);

        ISizerItem GetItemById(int id, bool recursive = false);

        ISizerItem Prepend(Control window, ISizerFlags sizerFlags);

        ISizerItem Prepend(ISizer sizer, ISizerFlags sizerFlags);

        ISizerItem Prepend(int width, int height, ISizerFlags sizerFlags);

        ISizerItem Insert(int index, Control window, ISizerFlags sizerFlags);

        ISizerItem Insert(int index, ISizer sizer, ISizerFlags sizerFlags);

        ISizerItem Insert(int index, int width, int height, ISizerFlags sizerFlags);

        ISizerItem Add(Control window, ISizerFlags sizerFlags);

        ISizerItem Add(ISizer sizer, ISizerFlags sizerFlags);

        ISizerItem Add(int width, int height, ISizerFlags sizerFlags);

        // IntPtr GetChildren();
        // void SetContainingWindow(Control window);
        // IntPtr GetContainingWindow();
    }
}