using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal interface IAssemblyDescriptor
    {
        Assembly Assembly { get; }

        Dictionary<string, IAssetDescriptor>? Resources { get; }

        Dictionary<string, IAssetDescriptor>? UIResources { get; }

        string? Name { get; }
    }
}
