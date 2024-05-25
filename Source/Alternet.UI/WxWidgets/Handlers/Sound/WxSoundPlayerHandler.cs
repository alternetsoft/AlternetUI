using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxSoundPlayerHandler : DisposableObject<IntPtr>, ISoundPlayerHandler
    {
        public WxSoundPlayerHandler(string fileName)
            : base(Native.WxOtherFactory.SoundCreate2(fileName, false), true)
        {
        }

        public bool IsOk
        {
            get
            {
                if (IsDisposed)
                    return false;
                return Native.WxOtherFactory.SoundIsOk(Handle);
            }
        }

        public void Stop()
        {
            SoundUtils.Handler.StopSound();
        }

        public bool Play(SoundPlayFlags flags = SoundPlayFlags.Asynchronous)
        {
            if (IsDisposed || !IsOk)
                return false;
            return Native.WxOtherFactory.SoundPlay(Handle, (uint)flags);
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            Native.WxOtherFactory.SoundDelete(Handle);
        }
    }
}
