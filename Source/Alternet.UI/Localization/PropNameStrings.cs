using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropNameStrings
    {
        public static PropNameStrings Default { get; set; } = new();

        public string Left { get; set; } = "Left";

        public string Top { get; set; } = "Top";

        public string Right { get; set; } = "Right";

        public string Bottom { get; set; } = "Bottom";

        public string Width { get; set; } = "Width";

        public string Height { get; set; } = "Height";

        public string X { get; set; } = "X";

        public string Y { get; set; } = "Y";
    }
}
