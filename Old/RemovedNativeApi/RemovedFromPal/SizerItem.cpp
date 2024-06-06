#include "SizerItem.h"

namespace Alternet::UI
{
    SizerItem::SizerItem()
    {
    }

    SizerItem::~SizerItem()
    {
    }

    void* SizerItem::CreateSizerItem(void* window, int proportion,
        int flag, int border, void* userData)
    {
        return new wxSizerItem((wxWindow*)window, proportion,
            flag, border, (wxObject*)userData);
    }

    void* SizerItem::CreateSizerItem2(void* window, void* sizerFlags)
    {
        return new wxSizerItem((wxWindow*)window, *(wxSizerFlags*)sizerFlags);
    }

    void* SizerItem::CreateSizerItem3(void* sizer, int proportion, int flag, int border, void* userData)
    {
        return new wxSizerItem((wxSizer*)sizer, proportion, flag, border, (wxObject*) userData);
    }

    void* SizerItem::CreateSizerItem4(void* sizer, void* sizerFlags)
    {
        return new wxSizerItem((wxSizer*)sizer, *(wxSizerFlags*)sizerFlags);
    }

    void* SizerItem::CreateSizerItem5(int width, int height, int proportion, int flag,
        int border, void* userData)
    {
        return new wxSizerItem(width, height, proportion, flag,
            border, (wxObject*) userData);
    }

    void* SizerItem::CreateSizerItem6(int width, int height, void* sizerFlags)
    {
        return new wxSizerItem(width, height, *(wxSizerFlags*) sizerFlags);
    }

    void* SizerItem::CreateSizerItem7()
    {
        return new wxSizerItem();
    }

    void SizerItem::DeleteWindows(void* handle)
    {
        ((wxSizerItem*)handle)->DeleteWindows();
    }

    void SizerItem::DetachSizer(void* handle)
    {
        ((wxSizerItem*)handle)->DetachSizer();
    }

    void SizerItem::DetachWindow(void* handle)
    {
        ((wxSizerItem*)handle)->DetachWindow();
    }

    Int32Size SizerItem::GetSize(void* handle)
    {
        return ((wxSizerItem*)handle)->GetSize();
    }

    Int32Size SizerItem::CalcMin(void* handle)
    {
        return ((wxSizerItem*)handle)->CalcMin();
    }

    void SizerItem::SetDimension(void* handle, const Int32Point& pos, const Int32Size& size)
    {
        ((wxSizerItem*)handle)->SetDimension(pos, size);
    }

    Int32Size SizerItem::GetMinSize(void* handle)
    {
        return ((wxSizerItem*)handle)->GetMinSize();
    }

    Int32Size SizerItem::GetMinSizeWithBorder(void* handle)
    {
        return ((wxSizerItem*)handle)->GetMinSizeWithBorder();
    }

    Int32Size SizerItem::GetMaxSize(void* handle)
    {
        return ((wxSizerItem*)handle)->GetMaxSize();
    }

    Int32Size SizerItem::GetMaxSizeWithBorder(void* handle)
    {
        return ((wxSizerItem*)handle)->GetMaxSizeWithBorder();
    }

    void SizerItem::SetMinSize(void* handle, int x, int y)
    {
        return ((wxSizerItem*)handle)->SetMinSize(x, y);
    }

    void SizerItem::SetInitSize(void* handle, int x, int y)
    {
        return ((wxSizerItem*)handle)->SetInitSize(x, y);
    }

    void SizerItem::SetRatio(void* handle, int width, int height)
    {
        return ((wxSizerItem*)handle)->SetRatio(width, height);
    }

    void SizerItem::SetRatio2(void* handle, float ratio)
    {
        return ((wxSizerItem*)handle)->SetRatio(ratio);
    }

    float SizerItem::GetRatio(void* handle)
    {
        return ((wxSizerItem*)handle)->GetRatio();
    }

    Int32Rect SizerItem::GetRect(void* handle)
    {
        return ((wxSizerItem*)handle)->GetRect();
    }

    void SizerItem::SetId(void* handle, int id)
    {
        return ((wxSizerItem*)handle)->SetId(id);
    }

    int SizerItem::GetId(void* handle)
    {
        return ((wxSizerItem*)handle)->GetId();
    }

    bool SizerItem::IsWindow(void* handle)
    {
        return ((wxSizerItem*)handle)->IsWindow();
    }

    bool SizerItem::IsSizer(void* handle)
    {
        return ((wxSizerItem*)handle)->IsSizer();
    }

    bool SizerItem::IsSpacer(void* handle)
    {
        return ((wxSizerItem*)handle)->IsSpacer();
    }

    void SizerItem::SetProportion(void* handle, int proportion)
    {
        return ((wxSizerItem*)handle)->SetProportion(proportion);
    }

    int SizerItem::GetProportion(void* handle)
    {
        return ((wxSizerItem*)handle)->GetProportion();
    }

    void SizerItem::SetFlag(void* handle, int flag)
    {
        return ((wxSizerItem*)handle)->SetFlag(flag);
    }

    int SizerItem::GetFlag(void* handle)
    {
        return ((wxSizerItem*)handle)->GetFlag();
    }

    void SizerItem::SetBorder(void* handle, int border)
    {
        return ((wxSizerItem*)handle)->SetBorder(border);
    }

    int SizerItem::GetBorder(void* handle)
    {
        return ((wxSizerItem*)handle)->GetBorder();
    }

    void* SizerItem::GetWindow(void* handle)
    {
        return ((wxSizerItem*)handle)->GetWindow();
    }

    void* SizerItem::GetSizer(void* handle)
    {
        return ((wxSizerItem*)handle)->GetSizer();
    }

    Int32Size SizerItem::GetSpacer(void* handle)
    {
        return ((wxSizerItem*)handle)->GetSpacer();
    }

    bool SizerItem::IsShown(void* handle)
    {
        return ((wxSizerItem*)handle)->IsShown();
    }

    void SizerItem::Show(void* handle, bool show)
    {
        return ((wxSizerItem*)handle)->Show(show);
    }

    void SizerItem::SetUserData(void* handle, void* userData)
    {
        return ((wxSizerItem*)handle)->SetUserData((wxObject*)userData);
    }

    void* SizerItem::GetUserData(void* handle)
    {
        return ((wxSizerItem*)handle)->GetUserData();
    }

    Int32Point SizerItem::GetPosition(void* handle)
    {
        return ((wxSizerItem*)handle)->GetPosition();
    }

    bool SizerItem::InformFirstDirection(void* handle, int direction, int size, int availableOtherDir)
    {
        return ((wxSizerItem*)handle)->InformFirstDirection(direction, size, availableOtherDir);
    }

    void SizerItem::AssignWindow(void* handle, void* window)
    {
        return ((wxSizerItem*)handle)->AssignWindow((wxWindow*)window);
    }

    void SizerItem::AssignSizer(void* handle, void* sizer)
    {
        return ((wxSizerItem*)handle)->AssignSizer((wxSizer*)sizer);
    }

    void SizerItem::AssignSpacer(void* handle, int w, int h)
    {
        return ((wxSizerItem*)handle)->AssignSpacer(w, h);
    }
}
