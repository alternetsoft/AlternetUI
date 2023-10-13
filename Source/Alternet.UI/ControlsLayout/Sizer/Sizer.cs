using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class Sizer : DisposableObject, ISizer
    {
        internal Sizer(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public Int32Size CalcMin()
        {
            return Native.Sizer.CalcMin(Handle);
        }

        public void RepositionChildren(Int32Size minSize)
        {
            Native.Sizer.RepositionChildren(Handle, minSize);
        }
    }
}
