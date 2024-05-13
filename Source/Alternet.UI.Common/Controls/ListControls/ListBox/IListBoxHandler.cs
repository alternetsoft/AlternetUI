using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IListBoxHandler : IControlHandler
    {
        bool HasBorder { get; set; }

        void EnsureVisible(int itemIndex);

        int? HitTest(PointD position);
    }
}
