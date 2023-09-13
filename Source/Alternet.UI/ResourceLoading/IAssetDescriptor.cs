using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal interface IAssetDescriptor
    {
        Assembly Assembly { get; }

        Stream GetStream();
    }
}
