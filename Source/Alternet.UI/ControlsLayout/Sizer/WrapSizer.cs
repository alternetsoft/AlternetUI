using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WrapSizer : BoxSizer, IWrapSizer
    {
        public WrapSizer(bool isVertical, WrapSizerFlag flags, bool disposeHandle)
            : base(
                  Native.WrapSizer.CreateWrapSizer(GetIntOrientation(isVertical), (int)flags),
                  disposeHandle)
        {
        }
    }
}
