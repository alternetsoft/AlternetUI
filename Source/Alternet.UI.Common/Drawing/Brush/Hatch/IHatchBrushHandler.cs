using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public interface IHatchBrushHandler : IBrushHandler
    {
        void Update(HatchBrush brush);
    }
}
