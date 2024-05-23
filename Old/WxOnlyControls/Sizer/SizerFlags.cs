using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_sizer_flags.html
    internal class SizerFlags : DisposableObject<IntPtr>, ISizerFlags
    {
        internal SizerFlags(int proportion = 0)
            : base(Native.SizerFlags.CreateSizerFlags(proportion), true)
        {
        }

        public static int GetDefaultBorder()
        {
            return Native.SizerFlags.GetDefaultBorder();
        }

        public static float GetDefaultBorderFractional()
        {
            return Native.SizerFlags.GetDefaultBorderFractional();
        }

        int ISizerFlags.GetDefaultBorder()
        {
            return Native.SizerFlags.GetDefaultBorder();
        }

        float ISizerFlags.GetDefaultBorderFractional()
        {
            return Native.SizerFlags.GetDefaultBorderFractional();
        }

        public int GetProportion()
        {
            return Native.SizerFlags.GetProportion(Handle);
        }

        public SizerFlag GetFlags()
        {
            return (SizerFlag)Native.SizerFlags.GetFlags(Handle);
        }

        public int GetBorderInPixels()
        {
            return Native.SizerFlags.GetBorderInPixels(Handle);
        }

        public ISizerFlags Proportion(int proportion)
        {
            Native.SizerFlags.Proportion(Handle, proportion);
            return this;
        }

        public ISizerFlags Expand()
        {
            Native.SizerFlags.Expand(Handle);
            return this;
        }

        public ISizerFlags Align(GenericAlignment alignment)
        {
            Native.SizerFlags.Align(Handle, (int)alignment);
            return this;
        }

        public ISizerFlags Center()
        {
            Native.SizerFlags.Center(Handle);
            return this;
        }

        public ISizerFlags CenterVertical()
        {
            Native.SizerFlags.CenterVertical(Handle);
            return this;
        }

        public ISizerFlags CenterHorizontal()
        {
            Native.SizerFlags.CenterHorizontal(Handle);
            return this;
        }

        public ISizerFlags Top()
        {
            Native.SizerFlags.Top(Handle);
            return this;
        }

        public ISizerFlags Left()
        {
            Native.SizerFlags.Left(Handle);
            return this;
        }

        public ISizerFlags Right()
        {
            Native.SizerFlags.Right(Handle);
            return this;
        }

        public ISizerFlags Bottom()
        {
            Native.SizerFlags.Bottom(Handle);
            return this;
        }

        public ISizerFlags Border(GenericDirection direction, int borderInPixels)
        {
            Native.SizerFlags.Border(Handle, (int)direction, borderInPixels);
            return this;
        }

        public ISizerFlags Border(GenericDirection direction = GenericDirection.All)
        {
            Native.SizerFlags.Border2(Handle, (int)direction);
            return this;
        }

        public ISizerFlags DoubleBorder(GenericDirection direction = GenericDirection.All)
        {
            Native.SizerFlags.DoubleBorder(Handle, (int)direction);
            return this;
        }

        public ISizerFlags TripleBorder(GenericDirection direction = GenericDirection.All)
        {
            Native.SizerFlags.TripleBorder(Handle, (int)direction);
            return this;
        }

        public ISizerFlags HorzBorder()
        {
            Native.SizerFlags.HorzBorder(Handle);
            return this;
        }

        public ISizerFlags DoubleHorzBorder()
        {
            Native.SizerFlags.DoubleHorzBorder(Handle);
            return this;
        }

        public ISizerFlags Shaped()
        {
            Native.SizerFlags.Shaped(Handle);
            return this;
        }

        public ISizerFlags FixedMinSize()
        {
            Native.SizerFlags.FixedMinSize(Handle);
            return this;
        }

        // makes the item ignore window's visibility status
        public ISizerFlags ReserveSpaceEvenIfHidden()
        {
            Native.SizerFlags.ReserveSpaceEvenIfHidden(Handle);
            return this;
        }
    }
}