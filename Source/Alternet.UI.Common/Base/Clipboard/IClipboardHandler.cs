using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IClipboardHandler : IDisposable
    {
        IDataObject? GetData();

        void SetData(IDataObject value);
    }
}
