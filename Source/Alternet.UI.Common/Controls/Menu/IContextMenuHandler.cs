﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IContextMenuHandler : IControlHandler
    {
        void Show(IControl control, PointD? position = null);
    }
}
