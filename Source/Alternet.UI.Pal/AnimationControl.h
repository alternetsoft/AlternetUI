#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"
#include "ImageSet.h"
#include "Api/InputStream.h"
#include "ManagedInputStream.h"
#include "wx/animate.h"
#include "wx/generic/animate.h"

namespace Alternet::UI
{
    class AnimationControl : public Control
    {
#include "Api/AnimationControl.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        bool _useGeneric = false;

    private:
        wxAnimationCtrlBase* GetAnimation();
    };
}
