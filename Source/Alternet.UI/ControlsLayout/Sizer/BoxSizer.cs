using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class BoxSizer : Sizer, IBoxSizer
    {
        internal const int OrientationHorizontal = 0x0004;
        internal const int OrientationVertical = 0x0008;

        public BoxSizer(bool isVertical, bool disposeHandle)
            : this(Native.BoxSizer.CreateBoxSizer(GetIntOrientation(isVertical)), disposeHandle)
        {
        }

        internal BoxSizer(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public bool IsVertical
        {
            get
            {
                var orientation = Native.BoxSizer.GetOrientation(Handle);
                var result = (orientation & OrientationVertical) != 0;
                return result;
            }

            set
            {
                Native.BoxSizer.SetOrientation(Handle, GetIntOrientation(value));
            }
        }

        internal static int GetIntOrientation(bool isVertical)
        {
            if (isVertical)
                return OrientationVertical;
            else
                return OrientationHorizontal;
        }
    }
}