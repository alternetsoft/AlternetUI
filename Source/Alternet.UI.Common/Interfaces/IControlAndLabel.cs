using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IControlAndLabel
    {
        Control Label { get; }

        Control MainControl { get; }
    }
}
