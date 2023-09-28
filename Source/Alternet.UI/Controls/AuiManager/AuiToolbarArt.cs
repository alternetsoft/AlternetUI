using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class AuiToolbarArt : DisposableObject, IAuiToolbarArt
    {
        internal AuiToolbarArt(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public void SetFlags(uint flags)
        {
            Native.AuiToolBarArt.SetFlags(Handle, flags);
        }

        public uint GetFlags()
        {
            return Native.AuiToolBarArt.GetFlags(Handle);
        }

        public void SetTextOrientation(AuiToolbarTextOrientation orientation)
        {
            Native.AuiToolBarArt.SetTextOrientation(Handle, (int)orientation);
        }

        public AuiToolbarTextOrientation GetTextOrientation()
        {
            return (AuiToolbarTextOrientation)Native.AuiToolBarArt.GetTextOrientation(Handle);
        }

        // Note that these functions work with the size in DIPs, not physical pixels.
        public int GetElementSize(int elementId)
        {
            return Native.AuiToolBarArt.GetElementSize(Handle, elementId);
        }

        public void SetElementSize(int elementId, int size)
        {
            Native.AuiToolBarArt.SetElementSize(Handle, elementId, size);
        }

        // Provide opportunity for subclasses to recalculate colors
        public void UpdateColorsFromSystem()
        {
            Native.AuiToolBarArt.UpdateColorsFromSystem(Handle);
        }
    }
}