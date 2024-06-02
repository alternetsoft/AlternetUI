using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxSoundFactoryHandler : DisposableObject, ISoundFactoryHandler
    {
        public void StopSound()
        {
            Native.WxOtherFactory.SoundStop();
        }

        public ISoundPlayerHandler CreateSoundPlayerHandler(string fileName)
        {
            return new WxSoundPlayerHandler(fileName);
        }

        public void MessageBeep(SystemSoundType soundType)
        {
            if (App.IsWindowsOS)
                SafeNativeMethods.MessageBeep((int)soundType);
            else
                Bell();
        }

        public void Bell()
        {
            Native.WxOtherFactory.Bell();
        }

        private class SafeNativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool MessageBeep(int type);
        }
    }
}
