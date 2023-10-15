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
        return nullptr;
    }

    void* SizerItem::CreateSizerItem2(void* window, void* sizerFlags)
    {
        return nullptr;
    }

    void* SizerItem::CreateSizerItem3(void* sizer, int proportion, int flag, int border, void* userData)
    {
        return nullptr;
    }

    void* SizerItem::CreateSizerItem4(void* sizer, void* sizerFlags)
    {
        return nullptr;
    }

    void* SizerItem::CreateSizerItem5(int width, int height, int proportion, int flag,
        int border, void* userData)
    {
        return nullptr;
    }

    void* SizerItem::CreateSizerItem6(int width, int height, void* sizerFlags)
    {
        return nullptr;
    }

    void* SizerItem::CreateSizerItem7()
    {
        return nullptr;
    }

    void SizerItem::DeleteWindows(void* handle)
    {
    }

    void SizerItem::DetachSizer(void* handle)
    {
    }

    void SizerItem::DetachWindow(void* handle)
    {
    }

    Int32Size SizerItem::GetSize(void* handle)
    {
        return Int32Size();
    }

    Int32Size SizerItem::CalcMin(void* handle)
    {
        return Int32Size();
    }

    void SizerItem::SetDimension(void* handle, const Int32Point& pos, const Int32Size& size)
    {
    }

    Int32Size SizerItem::GetMinSize(void* handle)
    {
        return Int32Size();
    }

    Int32Size SizerItem::GetMinSizeWithBorder(void* handle)
    {
        return Int32Size();
    }

    Int32Size SizerItem::GetMaxSize(void* handle)
    {
        return Int32Size();
    }

    Int32Size SizerItem::GetMaxSizeWithBorder(void* handle)
    {
        return Int32Size();
    }

    void SizerItem::SetMinSize(void* handle, int x, int y)
    {
    }

    void SizerItem::SetInitSize(void* handle, int x, int y)
    {
    }

    void SizerItem::SetRatio(void* handle, int width, int height)
    {
    }

    void SizerItem::SetRatio2(void* handle, float ratio)
    {
    }

    float SizerItem::GetRatio(void* handle)
    {
        return 0;
    }

    Int32Rect SizerItem::GetRect(void* handle)
    {
        return Int32Rect();
    }

    void SizerItem::SetId(void* handle, int id)
    {
    }

    int SizerItem::GetId(void* handle)
    {
        return 0;
    }

    bool SizerItem::IsWindow(void* handle)
    {
        return false;
    }

    bool SizerItem::IsSizer(void* handle)
    {
        return false;
    }

    bool SizerItem::IsSpacer(void* handle)
    {
        return false;
    }

    void SizerItem::SetProportion(void* handle, int proportion)
    {
    }

    int SizerItem::GetProportion(void* handle)
    {
        return 0;
    }

    void SizerItem::SetFlag(void* handle, int flag)
    {
    }

    int SizerItem::GetFlag(void* handle)
    {
        return 0;
    }

    void SizerItem::SetBorder(void* handle, int border)
    {
    }

    int SizerItem::GetBorder(void* handle)
    {
        return 0;
    }

    void* SizerItem::GetWindow(void* handle)
    {
        return nullptr;
    }

    void* SizerItem::GetSizer(void* handle)
    {
        return nullptr;
    }

    Int32Size SizerItem::GetSpacer(void* handle)
    {
        return Int32Size();
    }

    bool SizerItem::IsShown(void* handle)
    {
        return false;
    }

    void SizerItem::Show(void* handle, bool show)
    {
    }

    void SizerItem::SetUserData(void* handle, void* userData)
    {
    }

    void* SizerItem::GetUserData(void* handle)
    {
        return nullptr;
    }

    Int32Point SizerItem::GetPosition(void* handle)
    {
        return Int32Point();
    }

    bool SizerItem::InformFirstDirection(void* handle, int direction, int size, int availableOtherDir)
    {
        return false;
    }

    void SizerItem::AssignWindow(void* handle, void* window)
    {
    }

    void SizerItem::AssignSizer(void* handle, void* sizer)
    {
    }

    void SizerItem::AssignSpacer(void* handle, int w, int h)
    {
    }
}
