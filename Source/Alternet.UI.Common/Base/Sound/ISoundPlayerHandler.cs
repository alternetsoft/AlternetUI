using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface ISoundPlayerHandler : IDisposable
    {
        bool IsOk { get; }

        void Stop();

        bool Play(SoundPlayFlags flags = SoundPlayFlags.Asynchronous);
    }
}
