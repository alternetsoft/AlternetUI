using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface ICheckBoxHandler : IControlHandler
    {
        CheckState CheckState { get; set; }

        bool AllowAllStatesForUser { get; set; }

        bool AlignRight { get; set; }

        bool ThreeState { get; set; }
    }
}
