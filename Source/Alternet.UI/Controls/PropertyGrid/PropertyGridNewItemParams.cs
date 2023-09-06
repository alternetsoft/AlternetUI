using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridNewItemParams : IPropertyGridNewItemParams
    {
        public static readonly IPropertyGridNewItemParams Default = new PropertyGridNewItemParams();

        public string? Label { get; set; }
    }
}
