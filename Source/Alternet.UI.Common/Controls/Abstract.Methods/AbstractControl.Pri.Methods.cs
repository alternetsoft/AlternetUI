﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        private void Children_ItemInserted(object? sender, int index, AbstractControl item)
        {
            item.SetParentInternal(this);
            RaiseChildInserted(index, item);
            PerformLayout();
        }

        private void Children_ItemRemoved(object? sender, int index, AbstractControl item)
        {
            item.SetParentInternal(null);
            RaiseChildRemoved(item);
            PerformLayout();
        }
    }
}
