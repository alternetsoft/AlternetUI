#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"
#include "ImageSet.h"
#include "Api/InputStream.h"
#include "ManagedInputStream.h"

#include <wx/animate.h>
#include <wx/generic/animate.h>

namespace Alternet::UI
{
    class AnimationControl : public Control
    {
#include "Api/AnimationControl.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

#if defined(__WXGTK__)
        bool _useGeneric = true;
#else
        bool _useGeneric = false;
#endif

    private:
        wxAnimationCtrlBase* GetAnimation();
    };
}
