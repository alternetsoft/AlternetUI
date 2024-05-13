using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class AuiDockArt : DisposableObject<IntPtr>, IAuiDockArt
    {
        internal AuiDockArt(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        /// <inheritdoc cref="IAuiDockArt.GetColor"/>
        public Color GetColor(AuiDockArtSetting id)
        {
            return Native.AuiDockArt.GetColor(Handle, (int)id);
        }

        /// <inheritdoc cref="IAuiDockArt.GetMetric"/>
        public int GetMetric(AuiDockArtSetting id)
        {
            return Native.AuiDockArt.GetMetric(Handle, (int)id);
        }

        /// <inheritdoc cref="IAuiDockArt.SetColor"/>
        public void SetColor(AuiDockArtSetting id, Color color)
        {
            Native.AuiDockArt.SetColor(Handle, (int)id, color);
        }

        /// <inheritdoc cref="IAuiDockArt.SetMetric"/>
        public void SetMetric(AuiDockArtSetting id, int value)
        {
            Native.AuiDockArt.SetMetric(Handle, (int)id, value);
        }
    }
}
