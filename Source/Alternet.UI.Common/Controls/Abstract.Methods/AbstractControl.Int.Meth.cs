using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        internal void SetParentInternal(AbstractControl? value)
        {
            parent = value;
            LogicalParent = value;
        }

        internal void OnChildrenItemRemoved(object? sender, int index, AbstractControl item)
        {
            RaiseChildRemoved(item);
        }

        internal void OnChildrenItemInserted(object? sender, int index, AbstractControl item)
        {
            RaiseChildInserted(index, item);
        }
    }
}
