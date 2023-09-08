using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class CommonStrings
    {
        public static CommonStrings Default { get; set; } = new();

        public string ButtonOk { get; set; } = "Ok";

        public string ButtonCancel { get; set; } = "Cancel";

        public string ButtonApply { get; set; } = "Apply";
    }
}
