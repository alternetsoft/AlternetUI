using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IGroupBoxHandler : IControlHandler
    {
        int GetTopBorderForSizer();

        int GetOtherBorderForSizer();
    }
}
