using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface ISoundFactoryHandler : IDisposable
    {
        void StopSound();

        void MessageBeep(SystemSoundType soundType);

        void Bell();

        ISoundPlayerHandler CreateSoundPlayerHandler(string fileName);
    }
}
