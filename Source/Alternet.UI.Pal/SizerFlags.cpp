#include "SizerFlags.h"

namespace Alternet::UI
{
    SizerFlags::SizerFlags()
    {
    }

    SizerFlags::~SizerFlags()
    {
    }

    void* SizerFlags::CreateSizerFlags(int proportion)
    {
        return new wxSizerFlags(proportion);
    }
    
    int SizerFlags::GetDefaultBorder() 
    {
        return wxSizerFlags::GetDefaultBorder();
    }
    
    float SizerFlags::GetDefaultBorderFractional() 
    {
        return wxSizerFlags::GetDefaultBorderFractional();
    }
    
    int SizerFlags::GetProportion(void* handle) 
    {
        return ((wxSizerFlags*)handle)->GetProportion();
    }
    
    int SizerFlags::GetFlags(void* handle) 
    {
        return ((wxSizerFlags*)handle)->GetFlags();
    }
    
    int SizerFlags::GetBorderInPixels(void* handle) 
    {
        return ((wxSizerFlags*)handle)->GetBorderInPixels();
    }
    
    void SizerFlags::Proportion(void* handle, int proportion) 
    {
        ((wxSizerFlags*)handle)->Proportion(proportion);
    }
    
    void SizerFlags::Expand(void* handle) 
    {
        ((wxSizerFlags*)handle)->Expand();
    }
    
    void SizerFlags::Align(void* handle, int alignment) 
    {
        ((wxSizerFlags*)handle)->Align(alignment);
    }
    
    void SizerFlags::Center(void* handle) 
    {
        ((wxSizerFlags*)handle)->Center();
    }
    
    void SizerFlags::CenterVertical(void* handle) 
    {
        ((wxSizerFlags*)handle)->CenterVertical();
    }
    
    void SizerFlags::CenterHorizontal(void* handle) 
    {
        ((wxSizerFlags*)handle)->CenterHorizontal();
    }
    
    void SizerFlags::Top(void* handle) 
    {
        ((wxSizerFlags*)handle)->Top();
    }
    
    void SizerFlags::Left(void* handle) 
    {
        ((wxSizerFlags*)handle)->Left();
    }
    
    void SizerFlags::Right(void* handle) 
    {
        ((wxSizerFlags*)handle)->Right();
    }
    
    void SizerFlags::Bottom(void* handle) 
    {
        ((wxSizerFlags*)handle)->Bottom();
    }
    
    void SizerFlags::Border(void* handle, int direction, int borderInPixels) 
    {
        ((wxSizerFlags*)handle)->Border(direction, borderInPixels);
    }
    
    void SizerFlags::Border2(void* handle, int direction) 
    {
        ((wxSizerFlags*)handle)->Border(direction);
    }
    
    void SizerFlags::DoubleBorder(void* handle, int direction) 
    {
        ((wxSizerFlags*)handle)->DoubleBorder(direction);
    }
    
    void SizerFlags::TripleBorder(void* handle, int direction) 
    {
        ((wxSizerFlags*)handle)->TripleBorder(direction);
    }
    
    void SizerFlags::HorzBorder(void* handle) 
    {
        ((wxSizerFlags*)handle)->HorzBorder();
    }
    
    void SizerFlags::DoubleHorzBorder(void* handle) 
    {
        ((wxSizerFlags*)handle)->DoubleHorzBorder();
    }
    
    void SizerFlags::Shaped(void* handle) 
    {
        ((wxSizerFlags*)handle)->Shaped();
    }
    
    void SizerFlags::FixedMinSize(void* handle) 
    {
        ((wxSizerFlags*)handle)->FixedMinSize();
    }
    
    void SizerFlags::ReserveSpaceEvenIfHidden(void* handle) 
    {
        ((wxSizerFlags*)handle)->ReserveSpaceEvenIfHidden();
    }
}
