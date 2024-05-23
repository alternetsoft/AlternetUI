using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class AuiToolbarArt : DisposableObject<IntPtr>, IAuiToolbarArt
    {
        internal AuiToolbarArt(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public AuiToolbarTextOrientation TextOrientation
        {
            get
            {
                return (AuiToolbarTextOrientation)Native.AuiToolBarArt.GetTextOrientation(Handle);
            }

            set
            {
                Native.AuiToolBarArt.SetTextOrientation(Handle, (int)value);
            }
        }

        public void SetFlags(uint flags)
        {
            Native.AuiToolBarArt.SetFlags(Handle, flags);
        }

        public uint GetFlags()
        {
            return Native.AuiToolBarArt.GetFlags(Handle);
        }

        // Note that these functions work with the size in DIPs, not physical pixels.
        public int GetElementSize(AuiToolBarArtSetting elementId)
        {
            return Native.AuiToolBarArt.GetElementSize(Handle, (int)elementId);
        }

        public void SetElementSize(AuiToolBarArtSetting elementId, int size)
        {
            Native.AuiToolBarArt.SetElementSize(Handle, (int)elementId, size);
        }

        // Provide opportunity for subclasses to recalculate colors
        public void UpdateColorsFromSystem()
        {
            Native.AuiToolBarArt.UpdateColorsFromSystem(Handle);
        }
    }
}