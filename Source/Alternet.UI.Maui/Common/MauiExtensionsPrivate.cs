using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Graphics;

namespace Alternet.UI.Extensions
{
    public static class MauiExtensionsPrivate
    {
        public static RectD ToRectD(this RectF value)
        {
            return new(value.X, value.Y, value.Width, value.Height);
        }
    }
}
