#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeApi.Api.ManagedServers;
using Alternet.UI;
using Alternet.Drawing;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_animation_ctrl.html
    // #include <wx/animate.h>
    public class AnimationControl : Control
    {
        // Returns the delay for the i-th frame in milliseconds.
        public int GetDelay(uint i) => default;

        // Returns the i-th frame as a wxImage.
        public IntPtr GetFrame(uint i) => default;

        // Returns the number of frames for this animation.
        public uint GetFrameCount() => default;

        // Returns the size of the animation.
        public SizeI GetSize() => default;

        // Returns true if animation data is present.
        public bool IsOk() => default;

        public bool Play() => default;
        public void Stop() { }
        public bool UseGeneric { get; set; }

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
