#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_animation_ctrl.html
    // #include <wx/animate.h>
    public class Animation
    {
    }
}

/*
    wxAnimationCtrlBase() { }

    // public API
    virtual bool LoadFile(const wxString& filename,
                          wxAnimationType type = wxANIMATION_TYPE_ANY) = 0;
    virtual bool Load(wxInputStream& stream,
                      wxAnimationType type = wxANIMATION_TYPE_ANY) = 0;

    virtual void SetAnimation(const wxAnimation &anim) = 0;
    wxAnimation GetAnimation() const { return m_animation; }

    virtual bool Play() = 0;
    virtual void Stop() = 0;

    virtual bool IsPlaying() const = 0;

    virtual void SetInactiveBitmap(const wxBitmapBundle &bmp);

    // always return the original bitmap set in this control
    wxBitmap GetInactiveBitmap() const
        { return m_bmpStatic.GetBitmapFor(this); }

    wxAnimation CreateAnimation() const
        { return MakeAnimFromImpl(DoCreateAnimationImpl()); }

 */
