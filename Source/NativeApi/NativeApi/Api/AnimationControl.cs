#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeApi.Api.ManagedServers;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_animation_ctrl.html
    // #include <wx/animate.h>
    public class AnimationControl : Control
    {
        public bool Play() => default;
        public void Stop() { }

        public bool IsPlaying() => default;

        public bool LoadFile(string filename, int type /*= wxANIMATION_TYPE_ANY*/) => default;
        public bool Load(InputStream stream, int type /*= wxANIMATION_TYPE_ANY*/) => default;

        public void SetInactiveBitmap(ImageSet? bitmapBundle) { }
    }
}

/*
    void SetAnimation(const wxAnimation &anim) = 0;
    wxAnimation GetAnimation()

    wxAnimation CreateAnimation() const
        { return MakeAnimFromImpl(DoCreateAnimationImpl()); }

 */
