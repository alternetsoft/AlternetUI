#include "Sizer.h"

namespace Alternet::UI
{
    Sizer::Sizer()
    {
    }

    Sizer::~Sizer()
    {
    }

    void* Sizer::AddWindow(void* handle, void* window, int proportion, int flag,
        int border, void* userData)
    {
        return ((wxSizer*)handle)->Add((wxWindow*)window, proportion, flag, border, (wxObject*)userData);
    }

    void* Sizer::AddSizer(void* handle, void* sizer, int proportion, int flag,
        int border, void* userData)
    {
        return ((wxSizer*)handle)->Add((wxSizer*)sizer, proportion, flag,
            border, (wxObject*)userData);
    }

    void* Sizer::AddCustomBox(void* handle, int width, int height, int proportion,
        int flag, int border, void* userData)
    {
        return ((wxSizer*)handle)->Add(width, height, proportion,
            flag, border, (wxObject*)userData);
    }

    void* Sizer::AddWindow2(void* handle, void* window, void* sizerFlags)
    {
        return ((wxSizer*)handle)->Add((wxWindow*) window, *(wxSizerFlags*) sizerFlags);
    }

    void* Sizer::AddSizer2(void* handle, void* sizer, void* sizerFlags)
    {
        return ((wxSizer*)handle)->Add((wxSizer*)sizer, *(wxSizerFlags*)sizerFlags);
    }

    void* Sizer::AddCustomBox2(void* handle, int width, int height, void* sizerFlags)
    {
        return ((wxSizer*)handle)->Add(width, height, *(wxSizerFlags*)sizerFlags);
    }

    void* Sizer::AddItem(void* handle, void* item)
    {
        return ((wxSizer*)handle)->Add((wxSizerItem*)item);
    }

    void* Sizer::AddSpacer(void* handle, int size)
    {
        return ((wxSizer*)handle)->AddSpacer(size);
    }

    void* Sizer::AddStretchSpacer(void* handle, int prop)
    {
        return ((wxSizer*)handle)->AddStretchSpacer(prop);
    }

    void* Sizer::InsertWindow(void* handle, int index, void* window, int proportion,
        int flag, int border, void* userData)
    {
        return ((wxSizer*)handle)->Insert(index, (wxWindow*) window, proportion,
            flag, border, (wxObject*)userData);
    }

    void* Sizer::InsertSizer(void* handle, int index, void* sizer, int proportion,
        int flag, int border, void* userData)
    {
        return ((wxSizer*)handle)->Insert(index, (wxSizer*)sizer, proportion,
            flag, border, (wxObject*)userData);
    }

    void* Sizer::InsertCustomBox(void* handle, int index, int width, int height,
        int proportion, int flag, int border, void* userData)
    {
        return ((wxSizer*)handle)->Insert(index, width, height,
            proportion, flag, border, (wxObject*)userData);
    }

    void* Sizer::InsertWindow2(void* handle, int index, void* window, void* sizerFlags)
    {
        return ((wxSizer*)handle)->Insert(index, (wxWindow*) window, *(wxSizerFlags*)sizerFlags);
    }

    void* Sizer::InsertSizer2(void* handle, int index, void* sizer, void* sizerFlags)
    {
        return ((wxSizer*)handle)->Insert(index, (wxSizer*)sizer, *(wxSizerFlags*)sizerFlags);;
    }

    void* Sizer::InsertCustomBox2(void* handle, int index, int width, int height, void* sizerFlags)
    {
        return ((wxSizer*)handle)->Insert(index, width, height, *(wxSizerFlags*)sizerFlags);
    }

    void* Sizer::InsertItem(void* handle, int index, void* item)
    {
        return ((wxSizer*)handle)->Insert(index, (wxSizerItem*)item);
    }

    void* Sizer::InsertSpacer(void* handle, int index, int size)
    {
        return ((wxSizer*)handle)->InsertSpacer(index, size);
    }

    void* Sizer::InsertStretchSpacer(void* handle, int index, int prop)
    {
        return ((wxSizer*)handle)->InsertStretchSpacer(index, prop);
    }

    void* Sizer::PrependWindow(void* handle, void* window, int proportion, int flag,
        int border, void* userData)
    {
        return ((wxSizer*)handle)->Prepend((wxWindow*) window, proportion, flag,
            border, (wxObject*)userData);
    }

    void* Sizer::PrependSizer(void* handle, void* sizer, int proportion, int flag,
        int border, void* userData)
    {
        return ((wxSizer*)handle)->Prepend((wxSizer*)sizer, proportion, flag,
            border, (wxObject*)userData);
    }

    void* Sizer::PrependCustomBox(void* handle, int width, int height, int proportion,
        int flag, int border, void* userData)
    {
        return ((wxSizer*)handle)->Prepend(width, height, proportion,
            flag, border, (wxObject*)userData);
    }

    void* Sizer::PrependWindow2(void* handle, void* window, void* sizerFlags)
    {
        return ((wxSizer*)handle)->Prepend((wxWindow*)window, *(wxSizerFlags*)sizerFlags);
    }

    void* Sizer::PrependSizer2(void* handle, void* sizer, void* sizerFlags)
    {
        return ((wxSizer*)handle)->Prepend((wxSizer*)sizer, *(wxSizerFlags*)sizerFlags);
    }

    void* Sizer::PrependCustomBox2(void* handle, int width, int height, void* sizerFlags)
    {
        return ((wxSizer*)handle)->Prepend(width, height, *(wxSizerFlags*)sizerFlags);
    }

    void* Sizer::PrependItem(void* handle, void* item)
    {
        return ((wxSizer*)handle)->Prepend((wxSizerItem*) item);
    }

    void* Sizer::PrependSpacer(void* handle, int size)
    {
        return ((wxSizer*)handle)->PrependSpacer(size);
    }

    void* Sizer::PrependStretchSpacer(void* handle, int prop)
    {
        return ((wxSizer*)handle)->PrependStretchSpacer(prop);
    }

    void Sizer::SetContainingWindow(void* handle, void* window)
    {
        return ((wxSizer*)handle)->SetContainingWindow((wxWindow*)window);
    }

    void* Sizer::GetContainingWindow(void* handle)
    {
        return ((wxSizer*)handle)->GetContainingWindow();
    }

    bool Sizer::Remove(void* handle, void* sizer)
    {
        return ((wxSizer*)handle)->Remove((wxSizer*) sizer);
    }

    bool Sizer::Remove2(void* handle, int index)
    {
        return ((wxSizer*)handle)->Remove(index);
    }

    bool Sizer::DetachWindow(void* handle, void* window)
    {
        return ((wxSizer*)handle)->Detach((wxWindow*)window);
    }

    bool Sizer::DetachSizer(void* handle, void* sizer)
    {
        return ((wxSizer*)handle)->Detach((wxSizer*)sizer);
    }

    bool Sizer::Detach(void* handle, int index)
    {
        return ((wxSizer*)handle)->Detach(index);
    }

    bool Sizer::ReplaceWindow(void* handle, void* oldwin, void* newwin, bool recursive)
    {
        return ((wxSizer*)handle)->Replace((wxWindow*)oldwin, (wxWindow*)newwin, recursive);
    }

    bool Sizer::ReplaceSizer(void* handle, void* oldsz, void* newsz, bool recursive)
    {
        return ((wxSizer*)handle)->Replace((wxSizer*)oldsz, (wxSizer*)newsz, recursive);
    }

    bool Sizer::ReplaceItem(void* handle, int index, void* newitem)
    {
        return ((wxSizer*)handle)->Replace(index, (wxSizerItem*)newitem);
    }

    void Sizer::Clear(void* handle, bool delete_windows)
    {
        return ((wxSizer*)handle)->Clear(delete_windows);
    }

    void Sizer::DeleteWindows(void* handle)
    {
        return ((wxSizer*)handle)->DeleteWindows();
    }

    bool Sizer::InformFirstDirection(void* handle, int direction, int size, int availableOtherDir)
    {
        return ((wxSizer*)handle)->InformFirstDirection(direction, size, availableOtherDir);
    }

    void Sizer::SetMinSize(void* handle, int width, int height)
    {
        return ((wxSizer*)handle)->SetMinSize(width, height);
    }

    bool Sizer::SetWindowItemMinSize(void* handle, void* window, int width, int height)
    {
        return ((wxSizer*)handle)->SetItemMinSize((wxWindow*)window, width, height);
    }

    bool Sizer::SetSizerItemMinSize(void* handle, void* sizer, int width, int height)
    {
        return ((wxSizer*)handle)->SetItemMinSize((wxSizer*)sizer, width, height);
    }

    bool Sizer::SetCustomBoxItemMinSize(void* handle, int index, int width, int height)
    {
        return ((wxSizer*)handle)->SetItemMinSize(index, width, height);
    }

    Int32Size Sizer::GetSize(void* handle)
    {
        return ((wxSizer*)handle)->GetSize();
    }

    Int32Point Sizer::GetPosition(void* handle)
    {
        return ((wxSizer*)handle)->GetPosition();
    }

    Int32Size Sizer::GetMinSize(void* handle)
    {
        return ((wxSizer*)handle)->GetMinSize();
    }

    Int32Size Sizer::CalcMin(void* handle)
    {
        return ((wxSizer*)handle)->CalcMin();
    }

    void Sizer::RepositionChildren(void* handle, const Int32Size& minSize)
    {
        wxSize size = minSize;
        return ((wxSizer*)handle)->RepositionChildren(size);
    }

    void Sizer::RecalcSizes(void* handle)
    {
        return ((wxSizer*)handle)->RecalcSizes();
    }

    void Sizer::Layout(void* handle)
    {
        return ((wxSizer*)handle)->Layout();
    }

    Int32Size Sizer::ComputeFittingClientSize(void* handle, void* window)
    {
        return ((wxSizer*)handle)->ComputeFittingClientSize((wxWindow*)window);
    }

    Int32Size Sizer::ComputeFittingWindowSize(void* handle, void* window)
    {
        return ((wxSizer*)handle)->ComputeFittingWindowSize((wxWindow*)window);
    }

    Int32Size Sizer::Fit(void* handle, void* window)
    {
        return ((wxSizer*)handle)->Fit((wxWindow*)window);
    }

    void Sizer::FitInside(void* handle, void* window)
    {
        return ((wxSizer*)handle)->FitInside((wxWindow*)window);
    }

    void Sizer::SetSizeHints(void* handle, void* window)
    {
        return ((wxSizer*)handle)->SetSizeHints((wxWindow*)window);
    }

    void* Sizer::GetChildren(void* handle)
    {
        return nullptr /*((wxSizer*)handle)->GetChildren()*/;
    }

    void Sizer::SetDimension(void* handle, int x, int y, int width, int height)
    {
        return ((wxSizer*)handle)->SetDimension(x, y, width, height);
    }

    int Sizer::GetItemCount(void* handle)
    {
        return ((wxSizer*)handle)->GetItemCount();
    }

    bool Sizer::IsEmpty(void* handle)
    {
        return ((wxSizer*)handle)->IsEmpty();
    }

    void* Sizer::GetItemWindow(void* handle, void* window, bool recursive)
    {
        return ((wxSizer*)handle)->GetItem((wxWindow*)window, recursive);
    }

    void* Sizer::GetItemSizer(void* handle, void* sizer, bool recursive)
    {
        return ((wxSizer*)handle)->GetItem((wxSizer*)sizer, recursive);
    }

    void* Sizer::GetItem(void* handle, int index)
    {
        return ((wxSizer*)handle)->GetItem(index);
    }

    void* Sizer::GetItemById(void* handle, int id, bool recursive)
    {
        return ((wxSizer*)handle)->GetItemById(id, recursive);
    }

    bool Sizer::ShowWindow(void* handle, void* window, bool show, bool recursive)
    {
        return ((wxSizer*)handle)->Show((wxWindow*)window, show, recursive);
    }

    bool Sizer::ShowSizer(void* handle, void* sizer, bool show, bool recursive)
    {
        return ((wxSizer*)handle)->Show((wxSizer*)sizer, show, recursive);
    }

    bool Sizer::ShowItem(void* handle, int index, bool show)
    {
        return ((wxSizer*)handle)->Show(index, show);
    }

    bool Sizer::HideSizer(void* handle, void* sizer, bool recursive)
    {
        return ((wxSizer*)handle)->Hide((wxSizer*)sizer, recursive);
    }

    bool Sizer::HideWindow(void* handle, void* window, bool recursive)
    {
        return ((wxSizer*)handle)->Hide((wxWindow*)window, recursive);
    }

    bool Sizer::Hide(void* handle, int index)
    {
        return ((wxSizer*)handle)->Hide(index);
    }

    bool Sizer::IsShownWindow(void* handle, void* window)
    {
        return ((wxSizer*)handle)->IsShown((wxWindow*)window);
    }

    bool Sizer::IsShownSizer(void* handle, void* sizer)
    {
        return ((wxSizer*)handle)->IsShown((wxSizer*)sizer);
    }

    bool Sizer::IsShown(void* handle, int index)
    {
        return ((wxSizer*)handle)->IsShown(index);
    }

    void Sizer::ShowItems(void* handle, bool show)
    {
        return ((wxSizer*)handle)->ShowItems(show);
    }

    void Sizer::Show(void* handle, bool show)
    {
        return ((wxSizer*)handle)->Show(show);
    }

    bool Sizer::AreAnyItemsShown(void* handle)
    {
        return ((wxSizer*)handle)->AreAnyItemsShown();
    }
}
