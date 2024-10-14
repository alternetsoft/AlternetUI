using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal interface IStyledText
    {
        void Draw(Graphics dc, PointD location, Font? font, Color? foreColor, Color? backColor);

        void Draw(Graphics dc, PointD location, Font? font, Brush? brush);

        SizeD Measure(Graphics dc, Font font);
    }
}
