using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_sizer.html
    internal interface ISizer : IDisposableObject
    {
        Int32Size CalcMin();

        void RepositionChildren(Int32Size minSize);

        /*
                IntPtr AddWindow(Control window,
                                     int proportion,
                                     int flag,
                                     int border,
                                     IntPtr userData);

                IntPtr AddSizer(ISizer sizer,
                                 int proportion,
                                 int flag,
                                 int border,
                                 IntPtr userData);

                IntPtr AddCustomBox(int width,
                                 int height,
                                 int proportion,
                                 int flag,
                                 int border,
                                 IntPtr userData);

                IntPtr AddWindow(IntPtr window, IntPtr sizerFlags);

                IntPtr AddSizer(IntPtr sizer, IntPtr sizerFlags);

                IntPtr AddCustomBox(int width, int height,
                    IntPtr sizerFlags);

                IntPtr AddItem(IntPtr item);

                IntPtr AddSpacer(int size);
                IntPtr AddStretchSpacer(int prop = 1);

                IntPtr InsertWindow(int index,
                                    Control window,
                                    int proportion,
                                    int flag,
                                    int border,
                                    IntPtr userData);

                IntPtr InsertSizer(int index,
                                    ISizer sizer,
                                    int proportion,
                                    int flag,
                                    int border,
                                    IntPtr userData);

                IntPtr InsertCustomBox(int index,
                                    int width,
                                    int height,
                                    int proportion,
                                    int flag,
                                    int border,
                                    IntPtr userData);

                IntPtr InsertWindow(int index,
                                    IntPtr window,
                                IntPtr sizerFlags);

                IntPtr InsertSizer(int index,
                                    IntPtr sizer,
                                    IntPtr sizerFlags);

                IntPtr InsertCustomBox(int index,
                                    int width,
                                    int height,
                                    IntPtr sizerFlags);

                IntPtr InsertItem(int index, IntPtr item);

                IntPtr InsertSpacer(int index, int size);

                IntPtr InsertStretchSpacer(int index, int prop);

                IntPtr PrependWindow(Control window,
                                     int proportion,
                                     int flag,
                                     int border,
                                     IntPtr userData);

                IntPtr PrependSizer(ISizer sizer,
                                     int proportion,
                                     int flag,
                                     int border,
                                     IntPtr userData);

                IntPtr PrependCustomBox(int width,
                                     int height,
                                     int proportion,
                                     int flag,
                                     int border,
                                     IntPtr userData);

                IntPtr PrependWindow(IntPtr window, IntPtr sizerFlags);

                IntPtr PrependSizer(IntPtr sizer, IntPtr sizerFlags);

                IntPtr PrependCustomBox(int width, int height, IntPtr sizerFlags);

                IntPtr PrependItem(IntPtr item);

                IntPtr PrependSpacer(int size);

                IntPtr PrependStretchSpacer(int prop = 1);

                void SetContainingWindow(Control window);

                IntPtr GetContainingWindow();

                bool Remove(IntPtr sizer);

                bool Remove(int index);

                bool DetachWindow(IntPtr window);

                bool DetachSizer(IntPtr sizer);

                bool Detach(int index);

                bool ReplaceWindow(Control oldwin, Control newwin, bool recursive);

                bool ReplaceSizer(ISizer oldsz, ISizer newsz, bool recursive);

                bool ReplaceItem(int index, IntPtr newitem);

                void Clear(bool delete_windows);

                void DeleteWindows();

                bool InformFirstDirection(int direction, int size,
                    int availableOtherDir);

                void SetMinSize(int width, int height);

                bool SetWindowItemMinSize(IntPtr window,
                    int width, int height);

                bool SetSizerItemMinSize(IntPtr sizer,
                    int width, int height);

                bool SetCustomBoxItemMinSize(int index,
                    int width, int height);

                Int32Size GetSize();

                Int32Point GetPosition();

                Int32Size GetMinSize();

                void RecalcSizes();

                void Layout();

                Int32Size ComputeFittingClientSize(IntPtr window);

                Int32Size ComputeFittingWindowSize(IntPtr window);

                Int32Size Fit(IntPtr window);

                void FitInside(IntPtr window);

                void SetSizeHints(IntPtr window);

                IntPtr GetChildren();

                void SetDimension(int x, int y, int width, int height);

                int GetItemCount();

                bool IsEmpty();

                IntPtr GetItemWindow(IntPtr window, bool recursive);

                IntPtr GetItemSizer(IntPtr sizer, bool recursive);

                IntPtr GetItem(int index);

                IntPtr GetItemById(int id, bool recursive = false);

                bool ShowWindow(Control window, bool show,
                    bool recursive);

                bool ShowSizer(ISizer sizer, bool show,
                    bool recursive);

                bool ShowItem(int index, bool show);

                bool HideSizer(ISizer sizer, bool recursive);

                bool HideWindow(Contol window, bool recursive);

                bool Hide(int index);

                bool IsShownWindow(Control window);

                bool IsShownSizer(ISizer sizer);

                bool IsShown(int index);

                void ShowItems(bool show);

                void Show(bool show);

                bool AreAnyItemsShown();
        */
    }
}