#include "AnimationControl.h"

namespace Alternet::UI
{
	AnimationControl::AnimationControl()
	{

	}

	AnimationControl::~AnimationControl()
	{
	}

    wxAnimationCtrl* AnimationControl::GetAnimation()
    {
        return dynamic_cast<wxAnimationCtrl*>(GetWxWindow());
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
    };

    wxWindow* AnimationControl::CreateWxWindowCore(wxWindow* parent)
    {
        long style = wxAC_DEFAULT_STYLE;

        auto control = new wxAnimationCtrl2(parent,
            wxID_ANY,
            wxNullAnimation,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        return control;
    }
}
