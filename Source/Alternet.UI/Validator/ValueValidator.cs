using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ValueValidator : DisposableObject, IValueValidator
    {
        public ValueValidator(IntPtr handle = default, bool disposeHandle = true)
            : base(handle, disposeHandle)
        {
        }
    }
}
