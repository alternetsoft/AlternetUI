using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public interface IPenHandler : IDisposable
    {
        void Update(Pen pen);
    }
}
