using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IPropertyGridChoices
    {
        IntPtr Handle { get; }

        public void Add(string text, int value, ImageSet? bitmap = null);
    }
}
