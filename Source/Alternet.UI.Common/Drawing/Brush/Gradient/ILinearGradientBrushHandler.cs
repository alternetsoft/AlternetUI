﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface ILinearGradientBrushHandler : IBrushHandler
    {
        void Update(LinearGradientBrush brush);
    }
}
