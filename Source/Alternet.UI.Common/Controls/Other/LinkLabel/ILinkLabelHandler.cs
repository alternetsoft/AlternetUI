using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface ILinkLabelHandler : IControlHandler
    {
        string Text { get; set; }

        string Url { get; set; }

        Color HoverColor { get; set; }

        Color NormalColor { get; set; }

        Color VisitedColor { get; set; }

        bool Visited { get; set; }
    }
}
