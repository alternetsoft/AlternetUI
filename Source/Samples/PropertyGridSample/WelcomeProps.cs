using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyGridSample
{
    internal class WelcomeProps
    {
        public static WelcomeProps Default = new();

        public byte AsByte { get; set; }
        public byte? AsByteNullable { get; set; }
    }
}
