using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NotImplementedPlatform : NativePlatform
    {
        public override IntPtr MemoryAlloc(int size)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryAllocLong(ulong size)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryCopy(IntPtr dest, IntPtr src, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryCopyLong(IntPtr dest, IntPtr src, ulong count)
        {
            throw new NotImplementedException();
        }

        public override void MemoryFree(IntPtr ptr)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryMove(IntPtr dest, IntPtr src, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryMoveLong(IntPtr dest, IntPtr src, ulong count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryRealloc(IntPtr ptr, int newSize)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryReallocLong(IntPtr ptr, ulong newSize)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemorySet(IntPtr dest, byte fillByte, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemorySetLong(IntPtr dest, byte fillByte, ulong count)
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsAppearanceIsDark()
        {
            throw new NotImplementedException();
        }

        public override string SystemSettingsAppearanceName()
        {
            throw new NotImplementedException();
        }

        public override Color SystemSettingsGetColor(SystemSettingsColor index)
        {
            throw new NotImplementedException();
        }

        public override Font SystemSettingsGetFont(SystemSettingsFont systemFont)
        {
            throw new NotImplementedException();
        }

        public override int SystemSettingsGetMetric(SystemSettingsMetric index)
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsHasFeature(SystemSettingsFeature index)
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsIsUsingDarkBackground()
        {
            throw new NotImplementedException();
        }
    }
}
