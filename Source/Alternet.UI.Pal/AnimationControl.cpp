#include "AnimationControl.h"
#include "GenericImage.h"

namespace Alternet::UI
{
    int AnimationControl::GetDelay(uint32_t i)
    {
        auto ani = GetAnimation();
        if(ani == nullptr)
            return 0;

        return ani->GetAnimation().GetDelay(i);
    }

    uint32_t AnimationControl::GetFrameCount()
    {
        auto ani = GetAnimation();
        if (ani == nullptr)
            return 0;

        return ani->GetAnimation().GetFrameCount();
    }

    SizeI AnimationControl::GetSize()
    {
        return GetAnimation()->GetSize();
    }

    bool AnimationControl::IsOk()
    {
        auto ani = GetAnimation();
        if (ani == nullptr)
            return false;

        return ani->GetAnimation().IsOk();
    }

    void* AnimationControl::GetFrame(uint32_t i)
    {
        auto ani = GetAnimation();
        if (ani == nullptr)
            return nullptr;

        auto image = ani->GetAnimation().GetFrame(i);

        return new GenericImage(image);
    }

    AnimationControl::AnimationControl()
	{

	}

	AnimationControl::~AnimationControl()
	{
	}

    wxGenericAnimationCtrl* AnimationControl::GetGenericAnimation()
    {
        return dynamic_cast<wxGenericAnimationCtrl*>(GetWxWindow());
    }

    wxAnimationCtrl* AnimationControl::GetCtrlAnimation()
    {
        return dynamic_cast<wxAnimationCtrl*>(GetWxWindow());
    }

    wxAnimationCtrlBase* AnimationControl::GetAnimation()
    {
        return dynamic_cast<wxAnimationCtrlBase*>(GetWxWindow());
    }

    bool AnimationControl::Play()
    {
        return GetAnimation()->Play();
    }

    void AnimationControl::Stop()
    {
        GetAnimation()->Stop();
    }

    bool AnimationControl::IsPlaying()
    {
        return GetAnimation()->IsPlaying();
    }

    bool AnimationControl::GetUseGeneric()
    {
        return _useGeneric;
    }

    void AnimationControl::SetUseGeneric(bool value)
    {
        if (_useGeneric == value)
            return;
        _useGeneric = value;
        RecreateWxWindowIfNeeded();
    }

    bool AnimationControl::LoadFile(const string& filename, int type)
    {
        return GetAnimation()->LoadFile(wxStr(filename), (wxAnimationType)type);
    }

    bool AnimationControl::Load(void* stream, int type)
    {
        InputStream inputStream(stream);
        ManagedInputStream managedInputStream(&inputStream);
        return GetAnimation()->Load(managedInputStream, (wxAnimationType)type);
    }

    void AnimationControl::SetInactiveBitmap(ImageSet* bitmapBundle)
    {
        return GetAnimation()->SetInactiveBitmap(ImageSet::BitmapBundle(bitmapBundle));
    }

    class wxAnimationCtrl2 : public wxAnimationCtrl, public wxWidgetExtender
    {
    public:
        wxAnimationCtrl2(wxWindow* parent,
            wxWindowID id,
            const wxAnimation& anim = wxNullAnimation,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxAC_DEFAULT_STYLE,
            const wxString& name = wxASCII_STR(wxAnimationCtrlNameStr))
            : wxAnimationCtrl(parent, id, anim, pos, size, style, name)
        {}

        wxAnimationCtrl2() {}
    };

    class wxGenericAnimationCtrl2 : public wxGenericAnimationCtrl, public wxWidgetExtender
    {
    public:
        wxGenericAnimationCtrl2(wxWindow* parent,
            wxWindowID id,
            const wxAnimation& anim = wxNullAnimation,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxAC_DEFAULT_STYLE,
            const wxString& name = wxASCII_STR(wxAnimationCtrlNameStr))
            : wxGenericAnimationCtrl(parent, id, anim, pos, size, style, name)
        {}

        wxGenericAnimationCtrl2(){}
    };

    wxWindow* AnimationControl::CreateWxWindowUnparented()
    {
        if (_useGeneric)
            return new wxGenericAnimationCtrl2();
        else
            return new wxAnimationCtrl2();
    }

    wxWindow* AnimationControl::CreateWxWindowCore(wxWindow* parent)
    {
        long style = wxAC_DEFAULT_STYLE /* || wxAC_NO_AUTORESIZE */;

        wxWindow* control;

        if (_useGeneric)
        {
            control = new wxGenericAnimationCtrl2(parent,
                wxID_ANY,
                wxNullAnimation,
                wxDefaultPosition,
                wxDefaultSize,
                style);
        }
        else
        {
            control = new wxAnimationCtrl2(parent,
                wxID_ANY,
                wxNullAnimation,
                wxDefaultPosition,
                wxDefaultSize,
                style);
        }

        return control;
    }
}
